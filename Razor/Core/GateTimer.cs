#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Linq;
using Assistant.Core;

namespace Assistant
{
    public class GateTimer
    {
        private static int m_Count;
        private static Timer m_Timer;

        private static readonly int[] m_ClilocsStop = {502632};

        private static readonly int[] m_ClilocsRestart = {501024};

        static GateTimer()
        {
            m_Timer = new InternalTimer();
            
            MessageManager.OnSystemMessage += OnSystemMessage;
        }

        public static int Count
        {
            get { return m_Count; }
        }
        
        private static void OnSystemMessage(Packet p, PacketHandlerEventArgs args, Serial source, ushort graphic,
            MessageType type, ushort hue, ushort font, string lang, string sourceName,
            string msg)
        {
            if (Running)
            {
                if (m_ClilocsStop.Any(t => Language.GetCliloc(t) == msg))
                {
                    Stop();
                }

                if (m_ClilocsRestart.Any(t => Language.GetCliloc(t) == msg))
                {
                    Start();
                }
            }
        }

        public static bool Running
        {
            get { return m_Timer.Running; }
        }

        public static void Start()
        {
            m_Count = 0;

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
                m_Count++;
                if (m_Count > 30)
                {
                    Stop();
                }

                Client.Instance.RequestTitlebarUpdate();
            }
        }
    }
}