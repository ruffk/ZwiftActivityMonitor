using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZwiftPacketMonitor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ZwiftActivityMonitor
{
    public class ZPMonitorService
    {
        private readonly ILogger<ZPMonitorService> Logger;
        //private readonly IConfiguration m_configuration;
        //private readonly IServiceProvider m_serviceProvider;
        private readonly ZwiftPacketMonitor.Monitor m_zpMonitor;

        private int m_trackedPlayerId;
        private bool m_debugMode;
        private int m_targetHR;
        private int m_targetPower;
        private bool m_isStarted;
        private int m_eventsProcessed;
        private DateTime m_lastPlayerStateUpdate;
        
        private string m_zpMonitorNetwork;

        public event EventHandler<PlayerStateEventArgs> PlayerStateEvent;

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
            //m_configuration = configuration;
            //m_serviceProvider = serviceProvider;
            m_zpMonitor = zpMonitor;

            //// Determine AutoStart
            //if (!bool.TryParse(m_configuration["ZwiftPacketMonitor:AutoStart"], out m_isAutoStart))
            //{
            //    m_isAutoStart = false;
            //}

            //Logger.LogInformation($"AutoStart of ZwiftPacketMonitor is {m_isAutoStart}");


            //m_zpMonitorNetwork = m_configuration["ZwiftPacketMonitor:Network"];

            m_zwifters = new Dictionary<int, Zwifter>();

            Logger.LogInformation($"Class {this.GetType()} initialized.");
        }


        public void StartMonitor()
        {
            StartMonitor(false, 0, 0);
        }

        public void StartMonitor(bool debugMode, int targetHR, int targetPower)
        {
            if (m_isStarted)
            {
                Logger.LogWarning($"ZwiftPacketMonitor is already running.");
                return;
            }

            m_zpMonitorNetwork = ZAMsettings.Settings.Network;

            m_debugMode = debugMode;
            m_targetHR = targetHR;
            m_targetPower = targetPower;

            m_lastPlayerStateUpdate = DateTime.Now;
            m_eventsProcessed = 0;

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

            if (m_debugMode == true)
            {
                m_trackedPlayerId = 0; // reset
                m_zpMonitor.IncomingPlayerEvent += this.PlayerEventHandler;
            }
            else
            {
                m_zpMonitor.OutgoingPlayerEvent += this.PlayerEventHandler;
            }

            m_isStarted = true;

            Logger.LogInformation($"ZwiftPacketMonitor started.");

        }

        public void StopMonitor()
        {
            Task.Run(() => { StopMonitorAsync().Wait(); });

            m_isStarted = false;

            if (m_debugMode == true)
            {
                m_zpMonitor.IncomingPlayerEvent -= this.PlayerEventHandler;
            }
            else
            {
                m_zpMonitor.OutgoingPlayerEvent -= this.PlayerEventHandler;
            }

            m_debugMode = false;
            m_targetHR = 0;
            m_targetPower = 0;

            //m_zpMonitor.IncomingPlayerEnteredWorldEvent -= this.PlayerEnteredWorldEventHandler;


            Logger.LogInformation($"ZwiftPacketMonitor stopped.");

        }

        public bool IsStarted { get { return m_isStarted; } }

        public int EventsProcessed { get { return m_eventsProcessed; } }

        public string Network { get { return m_zpMonitorNetwork; } }

        public bool IsDebugMode { get { return m_debugMode; } } 

        public int TargetHeartrate { get { return m_targetHR; } }
        public int TargetPower { get { return m_targetPower; } }

        private void PlayerEnteredWorldEventHandler(object sender, PlayerEnteredWorldEventArgs e)
        {

            if (!m_zwifters.ContainsKey(e.PlayerUpdate.RiderId))
            {
                //if  (e.PlayerUpdate.LastName == "Larsen[V]")
                //{
                //    Logger.LogInformation($"Found Rider: {e.PlayerUpdate.RiderId} FirstName: {e.PlayerUpdate.FirstName} LastName: {e.PlayerUpdate.LastName}");

                //}
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
                if (m_debugMode)
                {
                    if (m_trackedPlayerId == 0)
                    {
                        if (m_targetHR > 0 || m_targetPower > 0) // these will both be zero if randomly choosing a player
                        {
                            if ((m_targetHR == 0 || (e.PlayerState.Heartrate >= m_targetHR - 2 && e.PlayerState.Heartrate <= m_targetHR + 2))
                                && (m_targetPower == 0 || e.PlayerState.Power >= m_targetPower - 10 && e.PlayerState.Power <= m_targetPower + 10))
                            {
                                m_trackedPlayerId = e.PlayerState.Id;
                                Logger.LogInformation($"Monitoring player: {m_trackedPlayerId}");
                            }
                        }
                        else // randomly choose, not recommended
                        { 
                            m_trackedPlayerId = e.PlayerState.Id;
                        }
                    }

                    if (m_trackedPlayerId != e.PlayerState.Id)
                    {
                        return; // not our guy
                    }
                }
                else
                {
                    //Logger.LogInformation($"TRACING-OUTGOING: {e.PlayerState}");
                }

                // only receive updates approx once/sec
                if ((DateTime.Now - m_lastPlayerStateUpdate).TotalMilliseconds < 900)
                {
                    return;
                }
                m_lastPlayerStateUpdate = DateTime.Now;


                if (m_debugMode)
                {
                    Logger.LogInformation($"TRACING-INCOMING: PlayerId: {e.PlayerState.Id}, Power: {e.PlayerState.Power}, HeartRate: {e.PlayerState.Heartrate}, Distance: {e.PlayerState.Distance}");
                }
                else
                {
                    //Logger.LogInformation($"TRACING-OUTGOING: PlayerId: {e.PlayerState.Id}, Power: {e.PlayerState.Power}, HeartRate: {e.PlayerState.Heartrate}, Distance: {e.PlayerState.Distance}");
                }

                m_eventsProcessed++;

                // Do work here
                OnPlayerStateEvent(e);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred: {ex}");
            }
        }

        private void OnPlayerStateEvent(PlayerStateEventArgs e)
        {
            EventHandler<PlayerStateEventArgs> handler = PlayerStateEvent;
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
                await m_zpMonitor.StartCaptureAsync(m_zpMonitorNetwork, cancellationToken);
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

            await m_zpMonitor.StopCaptureAsync(cancellationToken);

            Logger.LogInformation($"StopMonitorAsync, After StopCaptureAsync on Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

    }
}
