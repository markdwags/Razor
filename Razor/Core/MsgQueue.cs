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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Assistant.Network;
using Assistant.UI;

namespace Assistant.Core
{
    public class MessageQueue
    {
        private class MsgInfo
        {
            public MsgInfo(Serial ser, Mobile m, ushort body, MessageType type, ushort hue, ushort font, string lang,
                string name)
            {
                Serial = ser;
                Body = body;
                Type = type;
                Hue = hue;
                Font = font;
                Lang = lang;
                Name = name;
                Mobile = m;
            }

            public TimeSpan Delay;
            public DateTime NextSend;
            public int Count;

            public Mobile Mobile;

            //ser, body, type, hue, font, lang, name
            public Serial Serial;
            public ushort Body, Hue, Font;
            public MessageType Type;
            public string Lang, Name;
        }

        private class MessageTimer : Timer
        {
            public MessageTimer() : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            {
            }

            protected override void OnTick()
            {
                if (m_Table.Count <= 0)
                    return;

                List<string> toremove = new List<string>();
                foreach (KeyValuePair<string, MsgInfo> de in m_Table)
                {
                    string txt = de.Key;
                    MsgInfo msg = de.Value;

                    if (msg.NextSend <= DateTime.UtcNow)
                    {
                        if (msg.Count > 0)
                        {
                            switch (msg.Lang)
                            {
                                case "O":
                                    msg.Mobile.OverheadMessage(msg.Hue, msg.Count > 1 ? $"{txt} [{msg.Count}]" : txt);

                                    break;
                                case "A":
                                    Client.Client.Instance.SendToClient(new AsciiMessage(msg.Serial, msg.Body, msg.Type,
                                        msg.Hue, msg.Font, msg.Name,
                                        msg.Count > 1 ? $"{txt} [{msg.Count}]" : txt));
                                    break;
                                default:
                                    Client.Client.Instance.SendToClient(new UnicodeMessage(msg.Serial, msg.Body, msg.Type,
                                        msg.Hue, msg.Font, msg.Lang, msg.Name,
                                        msg.Count > 1 ? $"{txt} [{msg.Count}]" : txt));
                                    break;
                            }

                            msg.Count = 0;
                            msg.NextSend = DateTime.UtcNow + msg.Delay;
                        }
                        else
                        {
                            if (txt != null)
                                toremove.Add(txt);
                        }
                    }
                }

                for (int i = toremove.Count - 1; i >= 0; --i)
                {
                    m_Table.TryRemove(toremove[i], out var msg);
                }
            }
        }

        private static Timer m_Timer = new MessageTimer();
        private static ConcurrentDictionary<string, MsgInfo> m_Table = new ConcurrentDictionary<string, MsgInfo>();

        static MessageQueue()
        {
            m_Timer.Start();
        }

        public static bool Enqueue(Mobile m, int hue, string lang, string text)
        {
            return Enqueue(0xFFFFFFFF, m, 0, MessageType.Regular, (ushort) hue, 3, lang, "System", text);
        }

        public static bool Enqueue(Serial ser, Mobile mobile, ushort body, MessageType type, ushort hue, ushort font,
            string lang,
            string name, string text)
        {
            MsgInfo m;

            if (!m_Table.TryGetValue(text, out m) || m == null)
            {
                m_Table[text] = m = new MsgInfo(ser, mobile, body, type, hue, font, lang, name);

                m.Count = 0;

                m.Delay = TimeSpan.FromSeconds((text.Length / 50 + 1) * Config.GetDouble("FilterDelay"));

                m.NextSend = DateTime.UtcNow + m.Delay;

                return true;
            }

            m.Count++;

            return false;
        }
    }
}