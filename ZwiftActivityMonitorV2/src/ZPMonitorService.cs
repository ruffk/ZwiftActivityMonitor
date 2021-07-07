using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZwiftPacketMonitor;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class ZPMonitorService
    {
        public bool IsZPMonitorStarted { get; internal set; }
        public bool IsCollectionStartWaiting { get; internal set; }
        public bool IsCollectionStarted { get; internal set; }
        public bool IsCollectionPaused { get; internal set; }
        public int EventsProcessed { get; internal set; }
        public string Network { get; internal set; }
        public bool IsDebugMode { get; internal set; }
        public int TargetHeartrate { get; internal set; }
        public int TargetPlayerId { get; internal set; }
        private UserProfile CurrentUserProfile { get { return ZAMsettings.Settings.CurrentUser; } }

        private readonly ILogger<ZPMonitorService> Logger;
        private readonly ZwiftPacketMonitor.Monitor ZPMonitor;
        private readonly Timer PacketSmoothingTimer;
        private readonly Timer ActivitySimulationTimer;

        private int mTrackedPlayerId;
        //private DateTime mLastPlayerStateUpdate;
        private DateTime? mCollectionStartTime;
        private DateTime? mMonitorStartTime;
        //private PlayerStateEventArgs mLatestPlayerStateEventArgs;
        private RiderStateEventArgs mLatestRiderStateEventArgs;
        private TimeSpan mPlayerStateTime = TimeSpan.Zero;          // The PlayerState Time value. This corresponds to the elapsed time the player sees on screen.
        //private Dictionary<int, Zwifter> mZwifters;               // Used if tracking PlayerEnteredWorld events
        private CancellationTokenSource mCancellationTokenSource;   // Used to cancel wait for event clock
        private int mSimulationDistance;
        private int mSimulationRoadTime;
        private int mSimulationPowerLowRange;
        private int mSimulationPowerHighRange;

        private DateTime? mPlayerPauseStartTime;
        private TimeSpan mPauseDuration = TimeSpan.Zero;

        public bool SimulateRiderActivity { get; internal set; }


        public event EventHandler<RiderStateEventArgs> RiderStateEvent;
        public event EventHandler<RiderStateEventArgs> HighResRiderStateEvent;

        #region Internal classes

        internal class Zwifter
        {
            private int m_riderId;
            private string m_firstName;
            private string m_lastName;
            public Zwifter(Payload105 zwifterInfo)
            {
                m_riderId = zwifterInfo.RiderId;
                m_firstName = zwifterInfo.FirstName;
                m_lastName = zwifterInfo.LastName;

            }

            public int RiderId { get { return m_riderId; } }
            public string FirstName { get { return m_firstName; } }
            public string LastName { get { return m_lastName; } }
        }

        #endregion

        public event EventHandler<ZPMonitorServiceStatusChangedEventArgs> ZPMonitorServiceStatusChanged;
        public event EventHandler<CollectionStatusChangedEventArgs> CollectionStatusChanged;


        public ZPMonitorService(ILogger<ZPMonitorService> logger, ZwiftPacketMonitor.Monitor zpMonitor)
        {
            Logger = logger;
            ZPMonitor = zpMonitor;

            this.PacketSmoothingTimer = new Timer(OnPacketSmoothingTimerCallback);

            this.ActivitySimulationTimer = new Timer(ActivitySimulationTimerCallback);

            //mZwifters = new Dictionary<int, Zwifter>();

            Logger.LogDebug($"Class {this.GetType()} constructed.");
        }


        public void StartMonitor()
        {
            StartMonitor(false, 0, 0, false);
        }

        public void StartMonitor(bool debugMode, int targetHR, int targetPlayerId, bool simulateRiderActivity)
        {
            if (this.IsZPMonitorStarted)
            {
                Logger.LogWarning($"ZwiftPacketMonitor is already running.");
                return;
            }

            this.Network = ZAMsettings.Settings.Network;

            this.IsDebugMode = debugMode;
            this.TargetHeartrate = targetHR;
            this.TargetPlayerId = targetPlayerId;
            this.SimulateRiderActivity = simulateRiderActivity;

            this.EventsProcessed = 0;
            this.mPlayerStateTime = TimeSpan.Zero;

            if (!SimulateRiderActivity || IsDebugMode)
            {

                // Here we launch our own task to start monitoring.  It's not actually necessary
                // but it does allow some extra logging of threads used for startup, shutdown, etc.
                // You cannot wait for it to complete because it doesn't, at least not until stop is called.
                // Plus, we want to see if an exception occurs during startup and properly re-throw it.
                Task t = Task.Run(async () =>
                {
                    try
                    {
                        await StartMonitorAsync();//.ConfigureAwait(false);
                }
                    catch
                    {
                        throw;
                    }
                });

                // Since we're not waiting on the startup, sleep for a short time to let the startup thread run 
                // and see if there were any exceptions.
                Thread.Sleep(500);

                if (t.Exception != null)
                {
                    t.Exception.Handle((ex) =>
                        {
                            throw (ex);
                        });
                }
                
                // Debug mode will operate a little differently than the regular game mode.
                // When debug mode is on, we'll pick the first INCOMING player who either has a HR within (+=2 bpm),
                // or wait for an exact match to the PlayerId parameter.
                // We will then lock onto that PlayerId to filter out subsequent updates. This makes the testing more consistent. 
                // This way it's possible to test event dispatch w/o having to be on the bike with power meter 
                // and heart rate strap actually connected and outputting data.  Idea by Brad W.
                if (IsDebugMode == true)
                {
                    mTrackedPlayerId = 0; // reset
                    ZPMonitor.IncomingPlayerEvent += this.PlayerEventHandler;
                }
                else
                {
                    ZPMonitor.OutgoingPlayerEvent += this.PlayerEventHandler;
                }
            }
            else
            {
                // Simulating power is the prefered way to test during development.  Power and HR numbers are randomly generated, based loosely upon the current user's own FTP number.
                ActivitySimulationTimer.Change(0, 333); // starts the timer, about 3 times / second
                mSimulationDistance = 0;
                mSimulationRoadTime = 0;

                mSimulationPowerLowRange = (int)(ZAMsettings.Settings.CurrentUser.PowerThreshold * 0.6);
                mSimulationPowerHighRange = (int)(ZAMsettings.Settings.CurrentUser.PowerThreshold * 1.2);

                Logger.LogDebug($"ZwiftPacketMonitor starting in SIMULATION mode.");
            }

            PacketSmoothingTimer.Change(0, 1000); // starts the timer
            this.mMonitorStartTime = DateTime.Now;
            this.mLatestRiderStateEventArgs = null;

            this.IsZPMonitorStarted = true;

            this.OnZPMonitorServiceStatusChanged(this, new ZPMonitorServiceStatusChangedEventArgs(ZPMonitorServiceStatusChangedEventArgs.ActionType.Started));

            Logger.LogDebug($"ZwiftPacketMonitor started.");
        }


        public void StopMonitor()
        {
            this.IsZPMonitorStarted = false;

            PacketSmoothingTimer.Change(Timeout.Infinite, Timeout.Infinite); // stops the timer

            if (!SimulateRiderActivity || IsDebugMode)
            {
                if (IsDebugMode == true)
                {
                    ZPMonitor.IncomingPlayerEvent -= this.PlayerEventHandler;
                }
                else
                {
                    ZPMonitor.OutgoingPlayerEvent -= this.PlayerEventHandler;
                }

                Task.Run(() => { StopMonitorAsync().Wait(); });
            }
            else
            {
                ActivitySimulationTimer.Change(Timeout.Infinite, Timeout.Infinite); // stops the timer
            }

            this.mMonitorStartTime = null;
            this.IsDebugMode = false;
            this.TargetHeartrate = 0;
            this.TargetPlayerId = 0;
            this.mLatestRiderStateEventArgs = null;

            //m_zpMonitor.IncomingPlayerEnteredWorldEvent -= this.PlayerEnteredWorldEventHandler;

            this.OnZPMonitorServiceStatusChanged(this, new ZPMonitorServiceStatusChangedEventArgs(ZPMonitorServiceStatusChangedEventArgs.ActionType.Stopped));

            Logger.LogDebug($"ZwiftPacketMonitor stopped.");
        }

        public async void StartCollection(bool startWithEventTimer)
        {
            Logger.LogDebug($"StartCollection - startWithEventTimer: {startWithEventTimer}");

            if (!this.IsCollectionStarted && !this.IsCollectionStartWaiting)
            {
                if (startWithEventTimer)
                {
                    mCancellationTokenSource = new CancellationTokenSource();

                    this.IsCollectionStartWaiting = true;
                    this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Waiting));

                    // Start a thread to wait for the PlayerState.Time to become non-zero.  
                    // This can be cancelled by selecting Stop from the menu.
                    await WaitForRiderStartAsync(mCancellationTokenSource.Token);

                    this.IsCollectionStartWaiting = false;

                    bool isCancelled = mCancellationTokenSource.IsCancellationRequested;
                    mCancellationTokenSource = null;

                    if (isCancelled)
                    {
                        this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Cancelled));
                        Logger.LogDebug($"StartCollection - Cancelled");
                        return;
                    }
                }

                mCollectionStartTime = DateTime.Now;
                
                this.mPlayerPauseStartTime = null;
                this.mPauseDuration = TimeSpan.Zero;
                this.mLastEventTimeUpdate = null;

                this.IsCollectionStarted = true;
                this.IsCollectionPaused = false;
                this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Started));

                Logger.LogDebug($"StartCollection");
            }

        }

        /// <summary>
        /// Wait for the rider clock to begin counting.
        /// This allows user to select Start, and collection won't begin until the clock begins.  
        /// On a freeride, this is when they start pedalling.
        /// In a timed event, this is when banner drops.
        /// In a non-timed event, this is when the user crosses the timing start line, which is usually shortly after the banner.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task WaitForRiderStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug($"WaitForRiderStartAsync, Begin Waiting...");


            double currentTime = PlayerStateTime.TotalMilliseconds;

            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested && PlayerStateTime.TotalMilliseconds == currentTime)
                {
                    Task.Delay(250).Wait();
                }
            }, cancellationToken);

            Logger.LogDebug($"WaitForRiderStartAsync, Waiting completed.  Cancelled: {cancellationToken.IsCancellationRequested}");
        }


        /// <summary>
        /// Stops the data collectors and sets menu item enabled statuses accordingly.
        /// </summary>
        public void StopCollection()
        {
            if (this.IsCollectionStartWaiting && mCancellationTokenSource != null)
            {
                // Cancel the waiting thread
                mCancellationTokenSource.Cancel();
            }
            else if (this.IsCollectionStarted)
            {
                this.mCollectionStartTime = null;

                this.IsCollectionStarted = false;
                this.IsCollectionPaused = false;
                this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Stopped));
            }
            Logger.LogDebug($"StopCollection");
        }

        // Get the latest PlayerState.Time value. This corresponds to the elapsed time the player sees on screen. 
        public TimeSpan PlayerStateTime
        {
            get
            {
                lock(this)
                {
                    return mPlayerStateTime;
                }
            }
        }

        private DateTime? mLastEventTimeUpdate;

        /// <summary>
        /// Internal event handler to filter player events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerEventHandler(object sender, PlayerStateEventArgs e)
        {
            if (!IsZPMonitorStarted)
                return;

            try
            {
                if (IsDebugMode)
                {
                    if (this.mTrackedPlayerId == 0)
                    {
                        if (this.TargetHeartrate > 0 && (e.PlayerState.Heartrate >= TargetHeartrate - 2 && e.PlayerState.Heartrate <= TargetHeartrate + 2))
                        {
                            mTrackedPlayerId = e.PlayerState.Id;
                            Logger.LogDebug($"Monitoring player: {mTrackedPlayerId}");
                        }
                        else if (this.TargetPlayerId > 0 && this.TargetPlayerId == e.PlayerState.Id)
                        {
                            mTrackedPlayerId = e.PlayerState.Id;
                            Logger.LogDebug($"Monitoring player: {mTrackedPlayerId}");
                        }
                    }

                    if (mTrackedPlayerId != e.PlayerState.Id)
                    {
                        return; // not our guy
                    }
                }
                else
                {
                    //Logger.LogDebug($"TRACING-OUTGOING: {e.PlayerState}");
                }

                // Capture the latest PlayerState.Time value.  This corresponds to the elapsed time the player sees on screen.
                // We can use this to know when the Zwift ride actually starts so that the ZAM clocks start simutaneously. 
                //
                // Packets come in randomly but usually more than once per second.  Here we always capture the latest packet,
                // but it's only distributed once every second when the timer fires.  This is standard collection process.
                TimeSpan currentEventTime = new TimeSpan(0, 0, e.PlayerState.Time);
                lock (this)
                {
                    // Lock is used to avoid contention between threads (this thread, the timer callback thread, or the wait for event clock thread)

                    // Is the clock still running?  It is the responsibility of the event consumer to process this flag.
                    // A pause will occur only if the event clock stops running.  If rider is in an event and stops pedaling, that is not a pause.

                    TimeSpan pauseDuration = this.mPauseDuration;  // default to how long we've paused during this ride. Used to calculate an AdjustedElapsedTime
                    bool playerIsPaused = false;

                    if (this.IsCollectionStarted && this.CurrentUserProfile.AutoPause)
                    {
                        if (this.mPlayerPauseStartTime == null)
                        {
                            //Logger.LogDebug($"Not currently paused - currentEventTime {currentEventTime}, playerEventTime: {this.mPlayerStateTime}");

                            // not currently paused, see if clock is running
                            if (currentEventTime != this.mPlayerStateTime)
                            {
                                // clock is running, record time
                                this.mLastEventTimeUpdate = DateTime.Now;

                                //Logger.LogDebug($"Clock is running - lastEventTimeUpdate: {this.mLastEventTimeUpdate}");
                            }
                            else
                            {
                                // clock hasn't changed since last packet, wait up 2 seconds and then declare us paused
                                TimeSpan? dwellTime = this.mLastEventTimeUpdate == null ? null : DateTime.Now - this.mLastEventTimeUpdate.Value;

                                if (dwellTime != null && dwellTime.Value.TotalSeconds > 2.0)
                                {
                                    playerIsPaused = true;
                                    // set the pause start time to the last time we received an update
                                    this.mPlayerPauseStartTime = this.mLastEventTimeUpdate;

                                    pauseDuration = this.mPauseDuration + dwellTime.Value; // total pause time

                                    this.IsCollectionPaused = true;
                                    this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Paused));

                                    //Logger.LogDebug($"Switching to paused - dwellTime: {dwellTime.Value}");
                                }
                            }
                        }
                        else
                        {
                            // currently paused, see if clock is running
                            if (currentEventTime != this.mPlayerStateTime)
                            {
                                // clock is running again, record time
                                this.mLastEventTimeUpdate = DateTime.Now;
                                
                                TimeSpan pauseTime = DateTime.Now - mPlayerPauseStartTime.Value; // this pause time

                                // no longer paused, save the total as it may happen again
                                mPauseDuration += pauseTime;
                                this.mPlayerPauseStartTime = null;

                                pauseDuration = this.mPauseDuration;

                                this.IsCollectionPaused = false;
                                this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Resumed, pauseTime));

                                //Logger.LogDebug($"No longer paused - This pauseDuration: {pauseTime}, Total pauseDuration: {this.mPauseDuration}");
                            }
                            else
                            {
                                // clock is still not running, determine the total duration of this pause, and any previous pauses

                                TimeSpan pauseTime = DateTime.Now - mPlayerPauseStartTime.Value; // this pause time
                                pauseDuration = this.mPauseDuration + pauseTime; // total pause time
                                playerIsPaused = true;

                                //Logger.LogDebug($"Still paused - This pauseDuration: {pauseTime}, Total pauseDuration: {pauseDuration}");
                            }
                        }
                    }

                    this.mPlayerStateTime = currentEventTime;
                    this.mLatestRiderStateEventArgs = new RiderStateEventArgs(e, this.mCollectionStartTime, playerIsPaused, pauseDuration);
                }

                // For some collectors we might want to see every packet, so provide a high-resolution event.
                OnHighResRiderStateEvent(this.mLatestRiderStateEventArgs);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Caught in {this.GetType()} (PlayerEventHandler)");
            }
        }


        /// <summary>
        /// Timer callback delegate, called once per second.  Determines if a packet has been received and if so, raises RiderStateEvent.
        /// </summary>
        /// <param name="state"></param>
        private void OnPacketSmoothingTimerCallback(object state)
        {
            if (!IsZPMonitorStarted)
                return;

            RiderStateEventArgs riderState = null;

            lock(this)
            {
                if (mLatestRiderStateEventArgs != null)
                {
                    riderState = mLatestRiderStateEventArgs;
                    mLatestRiderStateEventArgs = null;
                }
            }

            if (riderState != null)
            {
                if (IsDebugMode)
                {
                    Logger.LogDebug($"TRACING-INCOMING: {riderState}");
                }
                else
                {
                    Logger.LogDebug($"TRACING-OUTGOING: {riderState}");
                }

                EventsProcessed++;

                OnRiderStateEvent(riderState);
            }
        }


        /// <summary>
        /// Simulate rider activity, should be called 2-3 times per second
        /// </summary>
        /// <param name="state"></param>
        private void ActivitySimulationTimerCallback(object state)
        {
            if (!IsZPMonitorStarted)
                return;

            Random r = new();

            this.mSimulationDistance += r.Next(3, 6); // Should increase about 10..20 per second
            this.mSimulationRoadTime += r.Next(500, 850); // Should increase about 1500..2500 per second

            bool playerIsPaused = false; 
            TimeSpan pauseDuration = TimeSpan.Zero;

            if (this.IsCollectionStarted)
            {
                playerIsPaused = (this.EventsProcessed >= 30 && this.EventsProcessed < 40);
                if (playerIsPaused)
                {
                    // 
                    if (mPlayerPauseStartTime == null)
                    {
                        // keep track of when the pause started
                        mPlayerPauseStartTime = DateTime.Now;
                    }
                    else
                    {
                        // determine the total duration of this pause, and any previous pauses
                        pauseDuration = mPauseDuration + (DateTime.Now - mPlayerPauseStartTime.Value);
                    }
                }
                else if (mPlayerPauseStartTime != null)
                {
                    // no longer paused, save the total as it may happen again
                    mPauseDuration += (DateTime.Now - mPlayerPauseStartTime.Value);
                    mPlayerPauseStartTime = null;
                }
            }

            RiderStateEventArgs e = new RiderStateEventArgs(mCollectionStartTime, playerIsPaused, pauseDuration)
            {
                Id = 422258,
                Power = r.Next(mSimulationPowerLowRange, mSimulationPowerHighRange),
                Heartrate = r.Next(130, 175),
                Distance = mSimulationDistance,
                RoadId = 1,
                IsForward = true,
                Course = 6,
                RoadTime = mSimulationRoadTime,
            };

            lock (this)
            {
                // Lock is used to avoid contention between threads (this thread and the timer callback thread)
                mPlayerStateTime = DateTime.Now - mMonitorStartTime.Value;
                mLatestRiderStateEventArgs = e;
            }

            // For some applications we might want to see every packet, so provide a high-resolution event.
            OnHighResRiderStateEvent(e);
        }

        /// <summary>
        /// This event will occur once per second as long as packets are being received.  The latest packet received is delivered in the event args.
        /// </summary>
        /// <param name="e"></param>
        private void OnRiderStateEvent(RiderStateEventArgs e)
        {
            EventHandler<RiderStateEventArgs> handler = RiderStateEvent;
            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnRiderStateEvent)");
                }
            }
        }

        /// <summary>
        /// This event will occur each time a packet is received.
        /// </summary>
        /// <param name="e"></param>
        private void OnHighResRiderStateEvent(RiderStateEventArgs e)
        {
            EventHandler<RiderStateEventArgs> handler = HighResRiderStateEvent;
            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnHighResRiderStateEvent)");
                }
            }
        }

        private void OnZPMonitorServiceStatusChanged(object sender, ZPMonitorServiceStatusChangedEventArgs e)
        {
            EventHandler<ZPMonitorServiceStatusChangedEventArgs> handler = ZPMonitorServiceStatusChanged;
            if (handler != null)
            {
                try
                {
                    handler(sender, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnZPMonitorServiceStatusChanged)");
                }
            }
        }
        private void OnCollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            EventHandler<CollectionStatusChangedEventArgs> handler = CollectionStatusChanged;
            if (handler != null)
            {
                try
                {
                    handler(sender, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnCollectionStatusChanged)");
                }
            }
        }


        private async Task StartMonitorAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug($"StartMonitorAsync, Before StartCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");

            try
            {
                await ZPMonitor.StartCaptureAsync(Network, cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occurred trying to start ZwiftPacketMonitor in StartMonitorAsync method.");
                throw;
            }


            Logger.LogDebug($"StartMonitorAsync, After StartCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        private async Task StopMonitorAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug($"StopMonitorAsync, Before StopCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");

            await ZPMonitor.StopCaptureAsync(cancellationToken);

            Logger.LogDebug($"StopMonitorAsync, After StopCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        //private void PlayerEnteredWorldEventHandler(object sender, PlayerEnteredWorldEventArgs e)
        //{
        //    if (!mZwifters.ContainsKey(e.PlayerUpdate.RiderId))
        //    {
        //        mZwifters.Add(e.PlayerUpdate.RiderId, new Zwifter(e.PlayerUpdate));

        //        if (mZwifters.Count % 100 == 0)
        //        {
        //            Logger.LogDebug($"Rider count: {mZwifters.Count}");
        //        }
        //    }
        //    else
        //    {
        //        //Logger.LogDebug($"Duplicate Rider: {e.PlayerUpdate.RiderId} FirstName: {e.PlayerUpdate.FirstName} LastName: {e.PlayerUpdate.LastName}");
        //    }
        //}
    }
}
