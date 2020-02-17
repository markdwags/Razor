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