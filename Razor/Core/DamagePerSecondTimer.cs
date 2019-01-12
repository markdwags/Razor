using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Assistant
{
    public class DamagePerSecondTimer
    {
        private static Timer DpsTimer;
        private static DateTime StartTime;

        public static double DamagePerSecond { get; set; }
        public static double MaxDamagePerSecond { get; set; }
        public static int TotalDamage { get; set; }
        public static int MaxSingleDamage { get; set; }

        public static ConcurrentDictionary<string, int> TotalDamageByType;

        static DamagePerSecondTimer()
        {
            DpsTimer = new InternalTimer();
            StartTime = DateTime.UtcNow;
        }

        public static bool Running
        {
            get { return DpsTimer.Running; }
        }

        public static void Start()
        {
            TotalDamage = 0;
            MaxSingleDamage = 0;
            DamagePerSecond = 0;
            MaxDamagePerSecond = 0;

            TotalDamageByType = new ConcurrentDictionary<string, int>();

            StartTime = DateTime.UtcNow;

            if (DpsTimer.Running)
            {
                DpsTimer.Stop();
            }

            DpsTimer.Start();
            ClientCommunication.RequestTitlebarUpdate();

            if (World.Player != null)
                World.Player.SendMessage(MsgLevel.Force, "-- [Damage Tracking Started] ---");
        }

        public static void Stop()
        {
            if (World.Player != null)
            {
                World.Player.SendMessage(MsgLevel.Force, "-- [Damage Tracking Stopped] ---");
                World.Player.SendMessage(MsgLevel.Force, $"Total Damage: {TotalDamage}");
                World.Player.SendMessage(MsgLevel.Force, $"Max Single Damage: {MaxSingleDamage}");
                World.Player.SendMessage(MsgLevel.Force, $"Final DPS: {DamagePerSecond:N2}");
                World.Player.SendMessage(MsgLevel.Force, $"Max DPS: {MaxDamagePerSecond:N2}");

                List<KeyValuePair<string, int>> topFive =
                    (from mob in TotalDamageByType orderby mob.Value descending select mob)
                    .ToDictionary(pair => pair.Key, pair => pair.Value).Take(5).ToList();

                World.Player.SendMessage(MsgLevel.Force, "-- [Top 5 Damaged] ---");
                int x = 1;
                foreach (KeyValuePair<string, int> top in topFive)
                {
                    World.Player.SendMessage(MsgLevel.Force, $"{x}) {top.Key} [{top.Value:N2}]");
                    x++;
                }

                TotalDamageByType.Clear();
            }

            DpsTimer.Stop();
            ClientCommunication.RequestTitlebarUpdate();
        }

        private class InternalTimer : Timer
        {
            public InternalTimer() : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
            }

            protected override void OnTick()
            {
                if (World.Player == null)
                {
                    DamagePerSecondTimer.Stop();
                    return;
                }

                TimeSpan span = DateTime.UtcNow.Subtract(StartTime);

                DamagePerSecond = span.Seconds > 0 ? TotalDamage / span.TotalSeconds : 0;

                if (DamagePerSecond > MaxDamagePerSecond)
                    MaxDamagePerSecond = DamagePerSecond;

                ClientCommunication.RequestTitlebarUpdate();
            }
        }

        public static void AddDamage(uint serial, ushort damage)
        {
            if (damage > MaxSingleDamage)
                MaxSingleDamage = damage;

            TotalDamage += damage;
            
            Mobile mob = World.FindMobile(serial);

            if (mob == null)
                return;

            if (TotalDamageByType.ContainsKey(mob.Name))
            {
                TotalDamageByType[mob.Name] += damage;
            }
            else
            {
                TotalDamageByType.TryAdd(mob.Name, damage);
            }
        }
    }
}