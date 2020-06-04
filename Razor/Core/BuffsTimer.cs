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
using Assistant.Core;

namespace Assistant
{
    public class BuffsTimer
    {
        //private static int m_Count;
        private static Timer m_Timer;


        static BuffsTimer()
        {
            m_Timer = new InternalTimer();
        }

        public static bool Running
        {
            get { return m_Timer.Running; }
        }

        public static void Start()
        {
            if (m_Timer.Running)
                m_Timer.Stop();
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
                Client.Instance.RequestTitlebarUpdate();

                if (!Config.GetBool("BuffDebuffEveryXSeconds") || !Config.GetBool("ShowBuffDebuffOverhead"))
                    return;

                foreach (BuffsDebuffs buffsDebuff in World.Player.BuffsDebuffs)
                {
                    TimeSpan diff = DateTime.UtcNow - buffsDebuff.Timestamp;
                    int timeLeft = buffsDebuff.Duration - (int) diff.TotalSeconds;

                    if (!IsFiltered(buffsDebuff.ClilocMessage1) && timeLeft % Config.GetInt("BuffDebuffSeconds") == 0)
                    {
                        World.Player.OverheadMessage(Config.GetInt("BuffHue"),
                            Config.GetString("BuffDebuffFormat").Replace("{action}", string.Empty)
                                .Replace("{name}", buffsDebuff.ClilocMessage1)
                                .Replace("{duration}", timeLeft.ToString()));
                    }
                }
            }
        }

        public static bool IsFiltered(string name)
        {
            if (string.IsNullOrEmpty(Config.GetString("BuffDebuffFilter")))
                return false;

            if (string.IsNullOrEmpty(name))
                return false;

            foreach (string filter in Config.GetString("BuffDebuffFilter").ToLower().Split(','))
            {
                if (string.IsNullOrEmpty(filter))
                    continue;

                if (name.ToLower().Contains(filter))
                    return true;
            }

            return false;
        }
    }
}