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
                        GoldSinceStart = GoldSinceStart + ((int)World.Player.Gold - m_PrevGoldAmount);

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

                GoldPerSecond = span.Seconds > 0 ? (double)GoldSinceStart / span.TotalSeconds : 0;
                GoldPerMinute = span.Seconds > 0 ? (double)GoldSinceStart / (span.TotalSeconds / 60.0) : 0;
                GoldPerHour = span.Seconds > 0 ? (double)GoldSinceStart / (span.TotalSeconds / 3600.0) : 0;

                TotalMinutes = span.TotalMinutes;

                Client.Instance.RequestTitlebarUpdate();

                m_PrevGoldAmount = (int) World.Player.Gold;
            }
        }
    }
}