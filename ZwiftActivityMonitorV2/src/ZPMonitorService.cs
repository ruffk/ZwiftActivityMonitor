using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZwiftPacketMonitor;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
{
    public class ZPMonitorService
    {
        public bool IsZPMonitorStarted { get; internal set; }
        public bool IsCollectionStartWaiting { get; internal set; }
        public bool IsCollectionStarted { get; internal set; }
        public int EventsProcessed { get; internal set; }
        public string Network { get; internal set; }
        public bool IsDebugMode { get; internal set; }
        public int TargetHeartrate { get; internal set; }
        public int TargetPower { get; internal set; }

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
        private TimeSpan mPlayerStateTime;                          // The PlayerState Time value. This corresponds to the elapsed time the player sees on screen.
        private Dictionary<int, Zwifter> mZwifters;                 // Used if tracking PlayerEnteredWorld events
        private CancellationTokenSource mCancellationTokenSource;   // Used to cancel wait for event clock
        private int mSimulationDistance;
        private int mSimulationRoadTime;

        private const bool SimulateRiderActivity = true;


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

            PacketSmoothingTimer = new Timer(OnPacketSmoothingTimerCallback);

            ActivitySimulationTimer = new Timer(ActivitySimulationTimerCallback);

            mZwifters = new Dictionary<int, Zwifter>();
            mPlayerStateTime = TimeSpan.Zero;

            Logger.LogInformation($"Class {this.GetType()} constructed.");
        }


        public void StartMonitor()
        {
            StartMonitor(false, 0, 0);
        }

        public void StartMonitor(bool debugMode, int targetHR, int targetPower)
        {
            if (this.IsZPMonitorStarted)
            {
                Logger.LogWarning($"ZwiftPacketMonitor is already running.");
                return;
            }

            Network = ZAMsettings.Settings.Network;

            IsDebugMode = debugMode;
            TargetHeartrate = targetHR;
            TargetPower = targetPower;

            //mLastPlayerStateUpdate = DateTime.Now;
            EventsProcessed = 0;
            mPlayerStateTime = TimeSpan.Zero;

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
                Thread.Sleep(1000);

                if (t.Exception != null)
                {
                    t.Exception.Handle((ex) =>
                        {
                            throw (ex);
                        });
                }
                
                // Debug mode will operate a little differently than the regular game mode.
                // When debug mode is on, we'll either pick the first INCOMING player's data to use, or try to match
                // an INCOMING player's heartrate and power to the targetHR (+-2) and/or targetPower (+-10) parameters.
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
                ActivitySimulationTimer.Change(0, 333); // starts the timer, about 3 times / second
                mSimulationDistance = 0;
                mSimulationRoadTime = 0;

                Logger.LogInformation($"ZwiftPacketMonitor starting in SIMULATION mode.");
            }

            PacketSmoothingTimer.Change(0, 1000); // starts the timer
            mMonitorStartTime = DateTime.Now;

            this.IsZPMonitorStarted = true;

            this.OnZPMonitorServiceStatusChanged(this, new ZPMonitorServiceStatusChangedEventArgs(ZPMonitorServiceStatusChangedEventArgs.ActionType.Started));

            Logger.LogInformation($"ZwiftPacketMonitor started.");
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

            mMonitorStartTime = null;
            IsDebugMode = false;
            TargetHeartrate = 0;
            TargetPower = 0;

            //m_zpMonitor.IncomingPlayerEnteredWorldEvent -= this.PlayerEnteredWorldEventHandler;

            this.OnZPMonitorServiceStatusChanged(this, new ZPMonitorServiceStatusChangedEventArgs(ZPMonitorServiceStatusChangedEventArgs.ActionType.Stopped));

            Logger.LogInformation($"ZwiftPacketMonitor stopped.");
        }

        public async void StartCollection(bool startWithEventTimer)
        {
            if (!this.IsCollectionStarted && !this.IsCollectionStartWaiting)
            {
                // update all the menu items accordingly
                //OnCollectionStatusChanged();

                // view analysis window
                //tsbAnalysis.PerformClick();

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
                        Logger.LogInformation($"StartCollection - Cancelled");
                        return;
                    }
                }

                mCollectionStartTime = DateTime.Now;

                this.IsCollectionStarted = true;
                this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Started));

                Logger.LogInformation($"StartCollection");
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
            Logger.LogInformation($"WaitForRiderStartAsync, Begin Waiting...");


            double currentTime = PlayerStateTime.TotalMilliseconds;

            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested && PlayerStateTime.TotalMilliseconds == currentTime)
                {
                    Task.Delay(250).Wait();
                }
            }, cancellationToken);

            Logger.LogInformation($"WaitForRiderStartAsync, Waiting completed.  Cancelled: {cancellationToken.IsCancellationRequested}");
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
                this.OnCollectionStatusChanged(this, new CollectionStatusChangedEventArgs(CollectionStatusChangedEventArgs.ActionType.Stopped));
            }
            Logger.LogInformation($"StopCollection");
        }

        public void RequestStatusUpdate()
        {

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
                catch
                {
                    // Don't let downstream exceptions bubble up
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
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
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

        private void PlayerEnteredWorldEventHandler(object sender, PlayerEnteredWorldEventArgs e)
        {
            if (!mZwifters.ContainsKey(e.PlayerUpdate.RiderId))
            {
                mZwifters.Add(e.PlayerUpdate.RiderId, new Zwifter(e.PlayerUpdate));

                if (mZwifters.Count % 100 == 0)
                {
                    Logger.LogInformation($"Rider count: {mZwifters.Count}");
                }
            }
            else
            {
                //Logger.LogInformation($"Duplicate Rider: {e.PlayerUpdate.RiderId} FirstName: {e.PlayerUpdate.FirstName} LastName: {e.PlayerUpdate.LastName}");
            }
        }


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
                    if (mTrackedPlayerId == 0)
                    {
                        if (TargetHeartrate > 0 || TargetPower > 0) // these will both be zero if randomly choosing a player
                        {
                            if ((TargetHeartrate == 0 || (e.PlayerState.Heartrate >= TargetHeartrate - 2 && e.PlayerState.Heartrate <= TargetHeartrate + 2))
                                && (TargetPower == 0 || e.PlayerState.Power >= TargetPower - 10 && e.PlayerState.Power <= TargetPower + 10))
                            {
                                mTrackedPlayerId = e.PlayerState.Id;
                                Logger.LogInformation($"Monitoring player: {mTrackedPlayerId}");
                            }
                        }
                        else // randomly choose, not recommended
                        {
                            mTrackedPlayerId = e.PlayerState.Id;
                        }
                    }

                    if (mTrackedPlayerId != e.PlayerState.Id)
                    {
                        return; // not our guy
                    }
                }
                else
                {
                    //Logger.LogInformation($"TRACING-OUTGOING: {e.PlayerState}");
                }

                // Capture the latest PlayerState.Time value.  This corresponds to the elapsed time the player sees on screen.
                // We can use this to know when the Zwift ride actually starts so that the ZAM clocks start simutaneously. 
                //
                // Packets come in randomly but usually more than once per second.  Here we always capture the latest packet,
                // but it's only distributed once every second when the timer fires.  This is standard collection process.
                lock (this)
                {
                    // Lock is used to avoid contention between threads (this thread and the timer callback thread)
                    mPlayerStateTime = new TimeSpan(0, 0, e.PlayerState.Time);
                    mLatestRiderStateEventArgs = new RiderStateEventArgs(e, mCollectionStartTime);
                    //mLatestPlayerStateEventArgs = e;
                }

                // For some applications we might want to see every packet, so provide a high-resolution event.
                OnHighResRiderStateEvent(mLatestRiderStateEventArgs);
                //OnHighResRiderStateEvent(new RiderStateEventArgs(e, mCollectionStartTime));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred: {ex}");
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
                //if (mLatestPlayerStateEventArgs != null)
                //{
                //    riderState = new RiderStateEventArgs(mLatestPlayerStateEventArgs, mCollectionStartTime);
                //    mLatestPlayerStateEventArgs = null;
                //}
            }

            if (riderState != null)
            {
                if (IsDebugMode)
                {
                    Logger.LogInformation($"TRACING-INCOMING: {riderState}");
                }
                else
                {
                    Logger.LogInformation($"TRACING-OUTGOING: {riderState}");
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

            this.mSimulationDistance += r.Next(3, 7); // Should increase about 10..20 per second
            this.mSimulationRoadTime += r.Next(500, 850); // Should increase about 1500..2500 per second

            RiderStateEventArgs e = new RiderStateEventArgs(mCollectionStartTime)
            {
                Id = 422258,
                Power = r.Next(150, 251),
                Heartrate = r.Next(130, 151),
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
                catch
                {
                    // Don't let downstream exceptions bubble up
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
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
        }

        private async Task StartMonitorAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"StartMonitorAsync, Before StartCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");

            try
            {
                await ZPMonitor.StartCaptureAsync(Network, cancellationToken);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occurred trying to start ZwiftPacketMonitor in StartMonitorAsync method.");
                throw;
            }


            Logger.LogInformation($"StartMonitorAsync, After StartCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        private async Task StopMonitorAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"StopMonitorAsync, Before StopCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");

            await ZPMonitor.StopCaptureAsync(cancellationToken);

            Logger.LogInformation($"StopMonitorAsync, After StopCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

    }
}
