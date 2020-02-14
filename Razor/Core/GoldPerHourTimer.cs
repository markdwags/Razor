#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

namespace Assistant
{
    public class GoldPerHourTimer
    {
        private static Timer m_Timer;
        private static DateTime m_StartTime;
        private static int m_PrevGoldAmount;

        private static bool m_PickedUpGold;

        public static int GoldSinceStart { get; set; }
        public static double GoldPerSecond { get; set; }
        public static double GoldPerMinute { get; set; }
        public static double GoldPerHour { get; set; }

        static GoldPerHourTimer()
        {
            m_Timer = new InternalTimer();
            m_StartTime = DateTime.UtcNow;
        }

        public static bool Running
        {
            get { return m_Timer.Running; }
        }

        public static double TotalMinutes { get; set; }

        public static void Start()
        {
            GoldSinceStart = 0;
            m_PrevGoldAmount = (int) World.Player.Gold;
            m_StartTime = DateTime.UtcNow;

            m_PickedUpGold = false;

            if (m_Timer.Running)
            {
                m_Timer.Stop();
            }

            m_Timer.Start();
            Client.Instance.RequestTitlebarUpdate();
        }

        public static void Stop()
        {
            m_Timer.Stop();
            Client.Instance.RequestTitlebarUpdate();
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
                    GoldPerHourTimer.Stop();
                    return;
                }

                if (m_PrevGoldAmount != World.Player.Gold)
                {
                    if (GoldSinceStart + ((int) World.Player.Gold - m_PrevGoldAmount) > m_PrevGoldAmount)
                    {
                        GoldSinceStart = GoldSinceStart + ((int) World.Player.Gold - m_PrevGoldAmount);

                        m_PickedUpGold = true;
                    }
                }

                // Let's not start calculating until we've picked up gold
                if (m_PickedUpGold == false)
                {
                    m_StartTime = DateTime.UtcNow;
                    return;
                }

                TimeSpan span = DateTime.UtcNow.Subtract(m_StartTime);

                GoldPerSecond = span.Seconds > 0 ? (double) GoldSinceStart / span.TotalSeconds : 0;
                GoldPerMinute = span.Seconds > 0 ? (double) GoldSinceStart / (span.TotalSeconds / 60.0) : 0;
                GoldPerHour = span.Seconds > 0 ? (double) GoldSinceStart / (span.TotalSeconds / 3600.0) : 0;

                TotalMinutes = span.TotalMinutes;

                Client.Instance.RequestTitlebarUpdate();

                m_PrevGoldAmount = (int) World.Player.Gold;
            }
        }
    }
}