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

        public bool IsStarted { get; internal set; }
        public int EventsProcessed { get; internal set; }
        public string Network { get; internal set; }
        public bool IsDebugMode { get; internal set; }
        public int TargetHeartrate { get; internal set; }
        public int TargetPower { get; internal set; }

        private ILogger<ZPMonitorService> Logger { get; }
        private ZwiftPacketMonitor.Monitor ZPMonitor { get; }

        private int TrackedPlayerId { get; set; }
        private DateTime LastPlayerStateUpdate { get; set; }
        private Timer PacketSmoothingTimer { get; }
        private PlayerStateEventArgs LatestPlayerStateEventArgs { get; set; }


        private TimeSpan m_playerStateTime = new TimeSpan(0); // The PlayerState Time value. This corresponds to the elapsed time the player sees on screen.


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

        // Used if tracking PlayerEnteredWorld events
        private Dictionary<int, Zwifter> m_zwifters;



        public ZPMonitorService(ILogger<ZPMonitorService> logger, ZwiftPacketMonitor.Monitor zpMonitor)
        {
            Logger = logger;
            ZPMonitor = zpMonitor;

            PacketSmoothingTimer = new Timer(OnPacketSmoothingTimerCallback);

            m_zwifters = new Dictionary<int, Zwifter>();

            Logger.LogInformation($"Class {this.GetType()} constructed.");
        }


        public void StartMonitor()
        {
            StartMonitor(false, 0, 0);
        }

        public void StartMonitor(bool debugMode, int targetHR, int targetPower)
        {
            if (IsStarted)
            {
                Logger.LogWarning($"ZwiftPacketMonitor is already running.");
                return;
            }

            Network = ZAMsettings.Settings.Network;

            IsDebugMode = debugMode;
            TargetHeartrate = targetHR;
            TargetPower = targetPower;

            LastPlayerStateUpdate = DateTime.Now;
            EventsProcessed = 0;
            m_playerStateTime = new TimeSpan(0);

            // Here we launch our own task to start monitoring.  It's not actually necessary
            // but it does allow some extra logging of threads used for startup, shutdown, etc.
            // You cannot wait for it to complete because it doesn't, at least not until stop is called.
            // Plus, we want to see if an exception occurs during startup and properly re-throw it.
            Task t = Task.Run(async() => 
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


            //Thread.Sleep(1000); // just to make sure startup has had enough time

            // Debug mode will operate a little differently than the regular game mode.
            // When debug mode is on, we'll either pick the first INCOMING player's data to use, or try to match
            // an INCOMING player's heartrate and power to the targetHR (+-2) and/or targetPower (+-10) parameters.
            // We will then lock onto that PlayerId to filter out subsequent updates. This makes the testing more consistent. 
            // This way it's possible to test event dispatch w/o having to be on the bike with power meter 
            // and heart rate strap actually connected and outputting data.  Idea by Brad W.

            if (IsDebugMode == true)
            {
                TrackedPlayerId = 0; // reset
                ZPMonitor.IncomingPlayerEvent += this.PlayerEventHandler;
            }
            else
            {
                ZPMonitor.OutgoingPlayerEvent += this.PlayerEventHandler;
            }

            PacketSmoothingTimer.Change(0, 1000); // starts the timer
            IsStarted = true;

            

            Logger.LogInformation($"ZwiftPacketMonitor started.");

        }


        public void StopMonitor()
        {
            PacketSmoothingTimer.Change(Timeout.Infinite, Timeout.Infinite); // stops the timer

            if (IsDebugMode == true)
            {
                ZPMonitor.IncomingPlayerEvent -= this.PlayerEventHandler;
            }
            else
            {
                ZPMonitor.OutgoingPlayerEvent -= this.PlayerEventHandler;
            }

            Task.Run(() => { StopMonitorAsync().Wait(); });

            IsStarted = false;
            IsDebugMode = false;
            TargetHeartrate = 0;
            TargetPower = 0;

            //m_zpMonitor.IncomingPlayerEnteredWorldEvent -= this.PlayerEnteredWorldEventHandler;

            Logger.LogInformation($"ZwiftPacketMonitor stopped.");
        }


        // Get the latest PlayerState.Time value. This corresponds to the elapsed time the player sees on screen. 
        public TimeSpan PlayerStateTime
        {
            get
            {
                lock(this)
                {
                    return m_playerStateTime;
                }
            }
        }

        private void PlayerEnteredWorldEventHandler(object sender, PlayerEnteredWorldEventArgs e)
        {
            if (!m_zwifters.ContainsKey(e.PlayerUpdate.RiderId))
            {
                m_zwifters.Add(e.PlayerUpdate.RiderId, new Zwifter(e.PlayerUpdate));

                if (m_zwifters.Count % 100 == 0)
                {
                    Logger.LogInformation($"Rider count: {m_zwifters.Count}");
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
            try
            {
                if (IsDebugMode)
                {
                    if (TrackedPlayerId == 0)
                    {
                        if (TargetHeartrate > 0 || TargetPower > 0) // these will both be zero if randomly choosing a player
                        {
                            if ((TargetHeartrate == 0 || (e.PlayerState.Heartrate >= TargetHeartrate - 2 && e.PlayerState.Heartrate <= TargetHeartrate + 2))
                                && (TargetPower == 0 || e.PlayerState.Power >= TargetPower - 10 && e.PlayerState.Power <= TargetPower + 10))
                            {
                                TrackedPlayerId = e.PlayerState.Id;
                                Logger.LogInformation($"Monitoring player: {TrackedPlayerId}");
                            }
                        }
                        else // randomly choose, not recommended
                        {
                            TrackedPlayerId = e.PlayerState.Id;
                        }
                    }

                    if (TrackedPlayerId != e.PlayerState.Id)
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
                    m_playerStateTime = new TimeSpan(0, 0, e.PlayerState.Time);
                    LatestPlayerStateEventArgs = e;
                }

                // For some applications we might want to see every packet, so provide a high-resolution event.
                OnHighResRiderStateEvent(new RiderStateEventArgs(e));
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
            RiderStateEventArgs riderState = null;

            lock(this)
            {
                if (LatestPlayerStateEventArgs != null)
                {
                    riderState = new RiderStateEventArgs(LatestPlayerStateEventArgs);
                    LatestPlayerStateEventArgs = null;
                }
            }

            if (riderState != null)
            {
                if (IsDebugMode)
                {
                    Logger.LogInformation($"TRACING-INCOMING: PlayerId: {riderState.Id}, Power: {riderState.Power}, HeartRate: {riderState.Heartrate}, Distance: {riderState.Distance}, Time: {riderState.Time}, Course: {riderState.Course}, RoadId: {riderState.RoadId}, IsForward: {riderState.IsForward}, RoadTime: {riderState.RoadTime}");
                }
                else
                {
                    Logger.LogInformation($"TRACING-OUTGOING: Power: {riderState.Power}, HeartRate: {riderState.Heartrate}, Distance: {riderState.Distance}, Time: {riderState.Time}, Course: {riderState.Course}, RoadId: {riderState.RoadId}, IsForward: {riderState.IsForward}, RoadTime: {riderState.RoadTime}");
                }

                EventsProcessed++;

                OnRiderStateEvent(riderState);
            }
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
