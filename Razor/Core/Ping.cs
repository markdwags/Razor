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
    public class Ping
    {
        private static DateTime m_Start;
        private static byte m_Seq;
        private static double m_Time, m_Min, m_Max;
        private static int m_Total;
        private static int m_Count;

        public static bool Response(byte seq)
        {
            if (seq == m_Seq && m_Start != DateTime.MinValue)
            {
                double ms = (DateTime.UtcNow - m_Start).TotalMilliseconds;

                if (ms < m_Min)
                    m_Min = ms;
                if (ms > m_Max)
                    m_Max = ms;

                if (m_Count-- > 0)
                {
                    m_Time += ms;
                    World.Player.SendMessage(MsgLevel.Force, $"Response: {ms:F1}ms");
                    DoPing();
                }
                else
                {
                    m_Start = DateTime.MinValue;
                    World.Player.SendMessage(MsgLevel.Force, "Ping Result:");
                    World.Player.SendMessage(MsgLevel.Force, "Min: {0:F1}ms  Max: {1:F1}ms  Avg: {2:F1}ms", m_Min,
                        m_Max, m_Time / ((double) m_Total));
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void StartPing(int count)
        {
            if (count <= 0 || count > 20)
                m_Count = 5;
            else
                m_Count = count;

            m_Total = m_Count;
            m_Time = 0;
            m_Min = double.MaxValue;
            m_Max = 0;

            World.Player.SendMessage(MsgLevel.Force, "Pinging server with {0} packets ({1} bytes)...", m_Count,
                m_Count * 2);
            DoPing();
        }

        private static void DoPing()
        {
            m_Seq = (byte) Utility.Random(256);
            m_Start = DateTime.UtcNow;
            Client.Instance.SendToServer(new PingPacket(m_Seq));
        }
    }
}