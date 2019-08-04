using System;

namespace Assistant
{
    public class StealthSteps
    {
        private static int m_Count;
        private static bool m_Hidden = false;

        public static int Count
        {
            get { return m_Count; }
        }

        public static bool Counting
        {
            get { return m_Hidden; }
        }

        public static bool Hidden
        {
            get { return m_Hidden; }
        }

        public static void OnMove()
        {
            if (m_Hidden && Config.GetBool("CountStealthSteps") && World.Player != null)
            {
                m_Count++;

                string message = Config.GetString("StealthStepsFormat").Replace("{step}", m_Count.ToString());

                if (string.IsNullOrEmpty(message))
                    message = $"Steps: {m_Count}";

                if (Config.GetBool("StealthOverhead"))
                {
                    World.Player.OverheadMessage(message);
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Error, message);
                }

                if (m_Count > 30)
                    Unhide();
            }
        }

        public static void Hide()
        {
            m_Hidden = true;
            m_Count = 0;
        }

        public static void Unhide()
        {
            m_Hidden = false;
            m_Count = 0;
        }
    }
}