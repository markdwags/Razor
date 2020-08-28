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

using System.Collections.Generic;
using System.Text;
using Assistant.Core;

namespace Assistant.Gumps.Internal
{
    public sealed class DamageTrackerGump : Gump
    {
        public DamageTrackerGump(string message) : base(100, 100)
        {
            Movable = true;
            Closable = false;
            Disposable = false;

            AddPage(0);
            AddBackground(0, 0, 253, 203, 9270);

            AddLabel(20, 20, 2954, "DPS:");
            AddLabel(60, 20, 67, $"{DamageTracker.DamagePerSecond:N2}");

            AddLabel(120, 20, 2954, "Max DPS:");
            AddLabel(190, 20, 67, $"{DamageTracker.MaxDamagePerSecond:N2}");

            AddHtml(20, 50, 213, 100, message, true, true);

            AddButton(20, 160, 4008, 4010, 1, GumpButtonType.Reply, 0);
            
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonId == 1)
            {
                StringBuilder sb = new StringBuilder();

                int x = 1;
                foreach (KeyValuePair<string, int> dmg in DamageTracker.GetTotalDamageList())
                {
                    sb.AppendLine($"{x}) {dmg.Key} [{dmg.Value:N2}]");
                    x++;
                }

                DamageTrackerListGump dmgList = new DamageTrackerListGump(sb.ToString());
                dmgList.SendGump();
            }

            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}