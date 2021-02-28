﻿using System;
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
        private readonly ILogger<ZPMonitorService> m_logger;
        private readonly IConfiguration m_configuration;
        private readonly IServiceProvider m_serviceProvider;
        private readonly ZwiftPacketMonitor.Monitor m_zpMonitor;

        private int m_trackedPlayerId;
        private bool m_debugMode;
        private int m_targetHR;
        private int m_targetPower;
        private bool m_isStarted;
        private int m_eventsProcessed;
        private DateTime m_lastPlayerStateUpdate;
        //private long m_lastWorldTime;
        //private PlayerStateEventArgs m_lastPlayerStateEventArgs;
        
        private string m_zpMonitorNetwork;

        public event EventHandler<PlayerStateEventArgs> PlayerStateEvent;

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

        private Dictionary<int, Zwifter> m_zwifters;



        public ZPMonitorService(ILogger<ZPMonitorService> logger, IServiceProvider serviceProvider, IConfiguration configuration, ZwiftPacketMonitor.Monitor zpMonitor)
        {
            m_logger = logger;
            m_configuration = configuration;
            m_serviceProvider = serviceProvider;
            m_zpMonitor = zpMonitor;

            m_zpMonitorNetwork = m_configuration["ZwiftPacketMonitor:Network"];

            m_zwifters = new Dictionary<int, Zwifter>();

            m_logger.LogInformation($"Class {this.GetType()} initialized.");
        }

        public void StartMonitor(bool debugMode, int targetHR, int targetPower)
        {
            if (m_isStarted)
            {
                m_logger.LogWarning($"ZwiftPacketMonitor is already running.");
                return;
            }

            m_debugMode = debugMode;
            m_targetHR = targetHR;
            m_targetPower = targetPower;

            m_lastPlayerStateUpdate = DateTime.Now;
            //m_lastWorldTime = 0;
            m_eventsProcessed = 0;

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

            //m_zpMonitor.IncomingPlayerEnteredWorldEvent += this.PlayerEnteredWorldEventHandler;

            // Here we launch our own task to start monitoring.  It's not actually necessary
            // but it does allow some extra logging of threads used for startup, shutdown, etc.
            // You cannot wait for it to complete because it doesn't, at least not until stop is called.
            // The alternate way is commented out below.
            Task.Run(() => { StartMonitorAsync(); });
            
            //m_zpMonitor.StartCaptureAsync(m_zpMonitorNetwork); // alternate way

            m_isStarted = true;

            Thread.Sleep(1000); // just to make sure startup has had enough time

            m_logger.LogInformation($"ZwiftPacketMonitor started.");

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


            m_logger.LogInformation($"ZwiftPacketMonitor stopped.");

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
                //    m_logger.LogInformation($"Found Rider: {e.PlayerUpdate.RiderId} FirstName: {e.PlayerUpdate.FirstName} LastName: {e.PlayerUpdate.LastName}");

                //}
                m_zwifters.Add(e.PlayerUpdate.RiderId, new Zwifter(e.PlayerUpdate));

                if (m_zwifters.Count % 100 == 0)
                {
                    m_logger.LogInformation($"Rider count: {m_zwifters.Count}");
                }
            }
            else
            {
                //m_logger.LogInformation($"Duplicate Rider: {e.PlayerUpdate.RiderId} FirstName: {e.PlayerUpdate.FirstName} LastName: {e.PlayerUpdate.LastName}");
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
                                m_logger.LogInformation($"Monitoring player: {m_trackedPlayerId}");
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
                    //m_logger.LogInformation($"TRACING-OUTGOING: {e.PlayerState}");
                }

                //if (e.PlayerState.WorldTime == m_lastWorldTime)
                //{
                //    m_logger.LogInformation($"1-DUP WORLDTIME: {m_lastPlayerStateEventArgs.PlayerState}");
                //    m_logger.LogInformation($"2-DUP WORLDTIME: {e.PlayerState}");
                //    m_trackedPlayerId = 0;
                //    return;
                //}

                //m_lastWorldTime = e.PlayerState.WorldTime;
                //m_lastPlayerStateEventArgs = e;


                if (m_debugMode)
                {
                    if ((DateTime.Now - m_lastPlayerStateUpdate).TotalMilliseconds < 1000)
                    {
                        return;
                    }
                    m_lastPlayerStateUpdate = DateTime.Now;
                    m_logger.LogInformation($"TRACING-INCOMING: PlayerId: {e.PlayerState.Id}, Power: {e.PlayerState.Power}, HeartRate: {e.PlayerState.Heartrate}");
                }

                m_eventsProcessed++;

                // Do work here
                OnPlayerStateEvent(e);
            }
            catch (Exception ex)
            {
                m_logger.LogError($"Exception occurred: {ex}");
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
            m_logger.LogInformation($"1-Thread: {Thread.CurrentThread.ManagedThreadId}");

            await m_zpMonitor.StartCaptureAsync(m_zpMonitorNetwork, cancellationToken);

            m_logger.LogInformation($"2-Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        private async Task StopMonitorAsync(CancellationToken cancellationToken = default)
        {
            m_logger.LogInformation($"1-Thread: {Thread.CurrentThread.ManagedThreadId}");

            await m_zpMonitor.StopCaptureAsync(cancellationToken);

            m_logger.LogInformation($"2-Thread: {Thread.CurrentThread.ManagedThreadId}");
        }

    }
}
