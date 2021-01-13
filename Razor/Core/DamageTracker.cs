#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistant.Gumps.Internal;

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

        private static DamageTrackerGump _gump;

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
            Client.Instance.RequestTitlebarUpdate();

            if (World.Player != null)
                World.Player.SendMessage(MsgLevel.Force, "-- [Damage Tracking Started] ---");
        }

        public static void Stop()
        {
            if (!DamageTrackerTimer.Running)
                return;

            _gump.CloseGump();

            DamageTrackerTimer.Stop();
            Client.Instance.RequestTitlebarUpdate();

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
            World.Player.SendMessage(MsgLevel.Force,
                $"Max Single Damage Dealt: {MaxSingleDamageDealt} on '{MaxSingleDamageDealtName}'");
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

        public static List<KeyValuePair<string, int>> GetTotalDamageList()
        {
            List<KeyValuePair<string, int>> sortedList =
                (from mob in TotalDamageDealtByName orderby mob.Value descending select mob)
                .ToDictionary(pair => pair.Key, pair => pair.Value).ToList();

            return sortedList;
        }

        private class InternalTimer : Timer
        {
            private StringBuilder _damageInfo = new StringBuilder();

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

                _gump?.CloseGump();

                TimeSpan span = DateTime.UtcNow.Subtract(StartTime);

                DamagePerSecond = span.Seconds > 0 ? TotalDamageDealt / span.TotalSeconds : 0;

                if (DamagePerSecond > MaxDamagePerSecond)
                    MaxDamagePerSecond = DamagePerSecond;

                //Client.Instance.RequestTitlebarUpdate();

                _damageInfo.AppendLine($"Total Damage Dealt: {TotalDamageDealt}");
                _damageInfo.AppendLine($"Total Damage Taken: {TotalDamageTaken}");
                _damageInfo.AppendLine($"Max Damage Dealt: {MaxSingleDamageDealt} on '{MaxSingleDamageDealtName}'");
                _damageInfo.AppendLine($"Max Damage Taken: {MaxSingleDamageTaken}");

                _gump = new DamageTrackerGump(_damageInfo.ToString());
                _gump.SendGump();

                _damageInfo.Clear();
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