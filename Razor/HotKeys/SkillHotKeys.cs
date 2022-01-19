#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

using System.Collections.Generic;
using Ultima;

namespace Assistant.HotKeys
{
    public class SkillHotKeys
    {
        private static HotKeyCallbackState _callback;
        
        public static void Initialize()
        {
            _callback = new HotKeyCallbackState(OnHotKey);
            //1044060 = Alchemy in UO cliloc
            
            foreach (int t in Skills.GetUsableSkillIndexes())
            {
                HotKey.Add(HKCategory.Skills, (1044060 + t), _callback, t);
            }
        }

        private static void OnHotKey(ref object state)
        {
            int sk = (int) state;
            Client.Instance.SendToServer(new UseSkill(sk));
            if (World.Player != null)
            {
                World.Player.LastSkill = sk;
            }

            if (sk == Skills.StealthIndex && !World.Player.Visible)
            {
                StealthSteps.Hide();
            }

            SkillTimer.Start();
        }
    }
}