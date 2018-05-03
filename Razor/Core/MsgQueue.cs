using System;
using System.Collections;
using System.Collections.Generic;

namespace Assistant
{
    public class MessageQueue
    {
        private class MsgInfo
        {
            public MsgInfo(Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang, string name)
            {
                Serial = ser;
                Body = body;
                Type = type;
                Hue = hue;
                Font = font;
                Lang = lang;
                Name = name;
            }

            public TimeSpan Delay;
            public DateTime NextSend;
            public int Count;

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
                            if (msg.Lang == "A")
                                ClientCommunication.SendToClient(new AsciiMessage(msg.Serial, msg.Body, msg.Type,
                                    msg.Hue, msg.Font, msg.Name,
                                    msg.Count > 1 ? String.Format("{0} [{1}]", txt, msg.Count) : txt));
                            else
                                ClientCommunication.SendToClient(new UnicodeMessage(msg.Serial, msg.Body, msg.Type,
                                    msg.Hue, msg.Font, msg.Lang, msg.Name,
                                    msg.Count > 1 ? String.Format("{0} [{1}]", txt, msg.Count) : txt));
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
                    m_Table.Remove(toremove[i]);
                }
            }
        }

        private static Timer m_Timer = new MessageTimer();
        private static Dictionary<string, MsgInfo> m_Table = new Dictionary<string, MsgInfo>();

        static MessageQueue()
        {
            m_Timer.Start();
        }

        public static bool Enqueue(Serial ser, ushort body, MessageType type, ushort hue, ushort font, string lang,
            string name, string text)
        {
            MsgInfo m;

            if (!m_Table.TryGetValue(text, out m) || m == null)
            {
                m_Table[text] = m = new MsgInfo(ser, body, type, hue, font, lang, name);

                m.Count = 0;

                m.Delay = TimeSpan.FromSeconds((((int) (text.Length / 50)) + 1) * 3.5);

                m.NextSend = DateTime.UtcNow + m.Delay;

                return true;
            }

            m.Count++;

            return false;
        }
    }
}