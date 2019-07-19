using System;
using System.Linq;
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