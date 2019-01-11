using System;
using System.Collections.Concurrent;

namespace Assistant
{
    public class DamagePerSecondTimer
    {
        public class DamageData
        {
            private string Name { get; }
            public int TotalDamage { get; set; }

            public DamageData(string name, int damage)
            {
                Name = name;
                TotalDamage = damage;
            }
        }

        private static Timer DpsTimer;
        private static DateTime StartTime;

        public static double DamagePerSecond { get; set; }
        public static double MaxDamagePerSecond { get; set; }
        public static int TotalDamage { get; set; }
        public static int MaxSingleDamage { get; set; }

        public static ConcurrentDictionary<uint, DamageData> TotalDamageData { get; set; }

        static DamagePerSecondTimer()
        {
            DpsTimer = new InternalTimer();
            StartTime = DateTime.UtcNow;
        }
        
        public static bool Running
        {
            get { return DpsTimer.Running; }
        }

        public static void Start()
        {
            TotalDamage = 0;
            MaxSingleDamage = 0;
            DamagePerSecond = 0;
            MaxDamagePerSecond = 0;

            TotalDamageData = new ConcurrentDictionary<uint, DamageData>();

            StartTime = DateTime.UtcNow;

            if (DpsTimer.Running)
            {
                DpsTimer.Stop();
            }

            DpsTimer.Start();
            ClientCommunication.RequestTitlebarUpdate();

            World.Player.OverheadMessage("Damage tracking started");
        }

        public static void Stop()
        {
            DpsTimer.Stop();
            ClientCommunication.RequestTitlebarUpdate();

            World.Player.OverheadMessage("Damage tracking stopped");
        }

        private int _previousDamageTotal;

        private class InternalTimer : Timer
        {
            public InternalTimer() : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
            }

            protected override void OnTick()
            {
                if (World.Player == null)
                {
                    DamagePerSecondTimer.Stop();
                    return;
                }
             
                TimeSpan span = DateTime.UtcNow.Subtract(StartTime);

                DamagePerSecond = span.Seconds > 0 ? TotalDamage / span.TotalSeconds : 0;

                if (DamagePerSecond > MaxDamagePerSecond)
                    MaxDamagePerSecond = DamagePerSecond;

                ClientCommunication.RequestTitlebarUpdate();
            }
        }

        public static void AddDamage(uint serial, ushort damage)
        {
            if (damage > MaxSingleDamage)
                MaxSingleDamage = damage;

            TotalDamage += damage;

            if (TotalDamageData.ContainsKey(serial))
            {
                TotalDamageData[serial].TotalDamage += damage;
            }
            else
            {
                Mobile mob = World.FindMobile(serial);

                if (mob == null)
                    return;

                DamageData data = new DamageData(World.FindMobile(serial).Name, damage);
                TotalDamageData.TryAdd(serial, data);
            }
        }

    }
}