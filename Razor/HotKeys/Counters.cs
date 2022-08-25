#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

namespace Assistant.HotKeys
{
    public class CounterHotKeys
    {
        public static void Initialize()
        {
            HotKey.Add(HKCategory.Misc, LocString.DispCounters, new HotKeyCallback(DispCounters));
            HotKey.Add(HKCategory.Misc, LocString.RecountCounters, new HotKeyCallback(Recount));
        }

        private static void Recount()
        {
            Counter.FullRecount();
        }

        private static void DispCounters()
        {
            for (int i = 0; i < Counter.List.Count; i++)
            {
                Counter c = (Counter) Counter.List[i];

                if (c.Enabled)
                    World.Player.SendMessage(MsgLevel.Force, "{0}: {1}", c.Name, c.Amount);
            }
        }
    }
}