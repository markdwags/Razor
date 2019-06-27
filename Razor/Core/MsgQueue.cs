using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Assistant
{
    public class MessageQueue
    {
        private class MsgInfo
        {
            public MsgInfo(Serial ser, Mobile m, ushort body, MessageType type, ushort hue, ushort font, string lang, string name)
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
                                    Client.Instance.SendToClient(new AsciiMessage(msg.Serial, msg.Body, msg.Type,
                                        msg.Hue, msg.Font, msg.Name,
                                        msg.Count > 1 ? $"{txt} [{msg.Count}]" : txt));
                                    break;
                                default:
                                    Client.Instance.SendToClient(new UnicodeMessage(msg.Serial, msg.Body, msg.Type,
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

        public static bool Enqueue(Serial ser, Mobile mobile, ushort body, MessageType type, ushort hue, ushort font, string lang,
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