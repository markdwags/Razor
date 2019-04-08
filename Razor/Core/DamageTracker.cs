using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Assistant.Core
{
    public class DamageTracker
    {
        private static readonly Timer DamageTrackerTimer;
        private static DateTime StartTime;

        public static double DamagePerSecond { get; set; }
        public static double MaxDamagePerSecond { get; set; }
        public static int TotalDamageDealt { get; set; }
        public static int TotalDamageTaken { get; set; }

        public static int MaxSingleDamageDealt { get; set; }
        public static string MaxSingleDamageDealtName { get; set; }

        public static int MaxSingleDamageTaken { get; set; }

        public static ConcurrentDictionary<string, int> TotalDamageDealtByName;

        static DamageTracker()
        {
            DamageTrackerTimer = new InternalTimer();
            StartTime = DateTime.UtcNow;
        }

        public static bool Running => DamageTrackerTimer.Running;

        public static void Start()
        {
            TotalDamageDealt = 0;
            MaxSingleDamageDealt = 0;
            DamagePerSecond = 0;
            MaxDamagePerSecond = 0;

            TotalDamageDealtByName = new ConcurrentDictionary<string, int>();

            StartTime = DateTime.UtcNow;

            if (DamageTrackerTimer.Running)
            {
                DamageTrackerTimer.Stop();
            }

            DamageTrackerTimer.Start();
            Client.RequestTitlebarUpdate();

            if (World.Player != null)
                World.Player.SendMessage(MsgLevel.Force, "-- [Damage Tracking Started] ---");
        }

        public static void Stop()
        {
            if (!DamageTrackerTimer.Running)
                return;

            DamageTrackerTimer.Stop();
            Client.RequestTitlebarUpdate();

            if (World.Player != null)
            {
                SendReport();
            }
        }

        public static void SendReport()
        {
            if (World.Player == null)
                return;

            World.Player.SendMessage(MsgLevel.Force, "-- [Damage Tracking Stopped] ---");
            World.Player.SendMessage(MsgLevel.Force, $"Total Damage Dealt: {TotalDamageDealt}");
            World.Player.SendMessage(MsgLevel.Force, $"Total Damage Taken: {TotalDamageTaken}");
            World.Player.SendMessage(MsgLevel.Force, $"Max Single Damage Dealt: {MaxSingleDamageDealt} on '{MaxSingleDamageDealtName}'");
            World.Player.SendMessage(MsgLevel.Force, $"Max Single Damage Taken: {MaxSingleDamageTaken}");
            World.Player.SendMessage(MsgLevel.Force, $"Final DPS: {DamagePerSecond:N2}");
            World.Player.SendMessage(MsgLevel.Force, $"Max DPS: {MaxDamagePerSecond:N2}");

            List<KeyValuePair<string, int>> topFive =
                (from mob in TotalDamageDealtByName orderby mob.Value descending select mob)
                .ToDictionary(pair => pair.Key, pair => pair.Value).Take(5).ToList();

            if (topFive.Count > 0)
            {
                World.Player.SendMessage(MsgLevel.Force, "-- [Top 5 Damage Dealt By Name] ---");

                int x = 1;
                foreach (KeyValuePair<string, int> top in topFive)
                {
                    World.Player.SendMessage(MsgLevel.Force, $"{x}) {top.Key} [{top.Value:N2}]");
                    x++;
                }
            }
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
                    DamageTracker.Stop();
                    return;
                }

                TimeSpan span = DateTime.UtcNow.Subtract(StartTime);

                DamagePerSecond = span.Seconds > 0 ? TotalDamageDealt / span.TotalSeconds : 0;

                if (DamagePerSecond > MaxDamagePerSecond)
                    MaxDamagePerSecond = DamagePerSecond;

                Client.RequestTitlebarUpdate();
            }
        }

        public static void AddDamage(uint serial, ushort damage)
        {
            Mobile mob = World.FindMobile(serial);

            if (mob == null)
                return;

            if (World.Player.Serial == serial) // Track damage you taken
            {
                if (damage > MaxSingleDamageTaken)
                {
                    MaxSingleDamageTaken = damage;
                }

                TotalDamageTaken += damage;
            }
            else // track damage you deal out
            {
                if (damage > MaxSingleDamageDealt)
                {
                    MaxSingleDamageDealt = damage;
                    MaxSingleDamageDealtName = mob.Name;
                }

                TotalDamageDealt += damage;

                if (TotalDamageDealtByName.ContainsKey(mob.Name))
                {
                    TotalDamageDealtByName[mob.Name] += damage;
                }
                else
                {
                    TotalDamageDealtByName.TryAdd(mob.Name, damage);
                }
            }
        }
    }
}