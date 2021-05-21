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

using Assistant.Core;
using System;

namespace Assistant
{
    public class BandageTimer
    {
        private static int m_Count;
        private static Timer m_Timer;

        private static int[] m_ClilocNums = new int[]
        {
            500955,
            500962,
            500963,
            500964,
            500965,
            500966,
            500967,
            500968,
            500969,
            503252,
            503253,
            503254,
            503255,
            503256,
            503257,
            503258,
            503259,
            503260,
            503261,
            1010058,
            1010648,
            1010650,
            1060088,
            1060167
        };

        static BandageTimer()
        {
            m_Timer = new InternalTimer();

            MessageManager.OnSystemMessage += OnSystemMessage;
        }

        public static void OnLocalizedMessage(int num)
        {
            if (Running)
            {
                if (num == 500955 || (num >= 500962 && num <= 500969) || (num >= 503252 && num <= 503261) ||
                    num == 1010058 || num == 1010648 || num == 1010650 || num == 1060088 || num == 1060167)
                {
                    Stop();

                    if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageEnd"))
                        ShowBandagingStatusMessage(Config.GetString("BandageEndMessage"));

                    return;
                }

                // Check if they are re-healing before the timer ends
                if (num == 500956)
                {
                    Start();

                    if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageStart"))
                        ShowBandagingStatusMessage(Config.GetString("BandageStartMessage"));
                }
            }
            else
            {
                // Start timer as soon as there is the "You begin applying the bandages." message
                if (num == 500956)
                {
                    Start();

                    if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageStart"))
                        ShowBandagingStatusMessage(Config.GetString("BandageStartMessage"));
                }
            }
        }

        private static void OnSystemMessage(Packet p, PacketHandlerEventArgs args, Serial source, ushort graphic,
                                            MessageType type, ushort hue, ushort font, string lang, string sourceName,
                                            string msg)
        {
            if (Running)
            {
                if (msg == "You heal what little damage you had." ||
                    msg == "You heal what little damage the patient had." ||
                    msg == "You did not stay close enough to heal your target.")
                {
                    Stop();

                    if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageEnd"))
                        ShowBandagingStatusMessage(Config.GetString("BandageEndMessage"));

                    return;
                }

                if (msg == "You begin applying the bandages.") // Timer is running and they start a new bandage
                {
                    Start();

                    if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageStart"))
                        ShowBandagingStatusMessage(Config.GetString("BandageStartMessage"));

                    return;
                }

                foreach (var t in m_ClilocNums)
                {
                    if (Language.GetCliloc(t) == msg)
                    {
                        Stop();

                        if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageEnd"))
                            ShowBandagingStatusMessage(Config.GetString("BandageEndMessage"));

                        break;
                    }
                }
            }
            else
            {
                // Start timer as soon as there is the "You begin applying the bandages." message
                if (msg == "You begin applying the bandages.")
                {
                    Start();

                    if (Config.GetBool("ShowBandageTimer") && Config.GetBool("ShowBandageStart"))
                        ShowBandagingStatusMessage(Config.GetString("BandageStartMessage"));
                }
            }
        }

        public static int Count
        {
            get { return m_Count; }
        }

        public static bool Running
        {
            get { return m_Timer.Running; }
        }

        public static void Start()
        {
            m_Count = 0;

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

        public static void ShowBandagingStatusMessage(string msg)
        {
            if (Config.GetInt("ShowBandageTimerLocation") == 0)
            {
                World.Player.OverheadMessage(Config.GetInt("ShowBandageTimerHue"), msg);
            }
            else
            {
                World.Player.SendMessage(Config.GetInt("ShowBandageTimerHue"), msg);
            }
        }

        private class InternalTimer : Timer
        {
            public InternalTimer() : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
            }

            protected override void OnTick()
            {
                if (World.Player != null && World.Player.IsGhost)
                {
                    BandageTimer.Stop();
                    return;
                }

                m_Count++;

                if (Config.GetBool("ShowBandageTimer"))
                {
                    bool showMessage = !(Config.GetBool("OnlyShowBandageTimerEvery") &&
                                         m_Count % Config.GetInt("OnlyShowBandageTimerSeconds") != 0);

                    if (showMessage)
                        ShowBandagingStatusMessage(Config.GetString("ShowBandageTimerFormat")
                            .Replace("{count}", m_Count.ToString()));
                }

                if (m_Count > 30)
                    Stop();

                Client.Instance.RequestTitlebarUpdate();
            }
        }
    }
}