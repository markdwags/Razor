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

namespace Assistant.Gumps.Internal
{
    public sealed class DamageTrackerListGump : Gump
    {
        public DamageTrackerListGump(string message) : base(100, 100)
        {
            Movable = true;
            Closable = true;
            Disposable = false;

            AddPage(0);
            AddBackground(0, 0, 273, 318, 9380);
            AddLabel(69, 5, 2954, "Damage Dealt (by name)");
			AddHtml(30, 40, 213, 239, message, true, true);
        }
    }
}