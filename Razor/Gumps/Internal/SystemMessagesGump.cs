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

using System.Linq;

namespace Assistant.Gumps.Internal
{
    public sealed class SystemMessagesGump : Gump
    {
        public SystemMessagesGump(string messages) : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 450, 286, 9270);
            AddLabel(162, 17, 2954, "System Messages");
            AddHtml(19, 45, 412, 222, messages, true, true);
            AddButton(401, 16, 4014, 4015, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonId == 1)
            {
                CloseGump();

                SystemMessagesGump gump = new SystemMessagesGump(string.Join("\n", PacketHandlers.SysMessages.ToArray().Reverse()));
                gump.SendGump();
            }            
        }
    }
}