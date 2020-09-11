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
using System.Windows.Forms;

namespace Assistant.Gumps.Internal
{
    public sealed class SystemMessagesGump : Gump
    {
        public SystemMessagesGump() : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            Resend = true;

            int pageCount = 1;

            AddPage(pageCount);
            AddBackground(0, 0, 527, 201, 9270);
            AddLabel(18, 13, 252, $"System Messages ({PacketHandlers.SysMessages.Count})");
            AddButton(460, 15, 9909, 9911, 1234, GumpButtonType.Page, pageCount);
            AddButton(483, 14, 9903, 9905, 1235, GumpButtonType.Page, pageCount + 1);

            int textY = 40;
            int buttonY = 42;
            int id = 0;
            int count = 0;

            foreach (string message in PacketHandlers.SysMessages.ToArray().Reverse())
            {
                AddButton(18, buttonY, 1209, 1210, id, GumpButtonType.Reply, 0);
                AddTextEntry(44, textY, 463, 20, 2954, id, PacketHandlers.SysMessages[count]);

                id++;
                count++;
                buttonY += 30;
                textY += 30;

                if (count % 5 == 0)
                {
                    AddButton(483, 14, 9903, 9905, pageCount + 2, GumpButtonType.Page, pageCount + 1);

                    pageCount++;
                    textY = 40;
                    buttonY = 42;

                    AddPage(pageCount);
                    AddBackground(0, 0, 527, 201, 9270);
                    AddLabel(18, 13, 252, $"System Messages ({PacketHandlers.SysMessages.Count})");
                    AddButton(460, 15, 9909, 9911, pageCount + 1, GumpButtonType.Page, pageCount - 1);                    
                }
            }
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonId == 0)
            {
                Resend = false;
                CloseGump();
            }

            if (buttonId > PacketHandlers.SysMessages.Count)
                return;

            Clipboard.SetText(PacketHandlers.SysMessages[buttonId]);
            World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, PacketHandlers.SysMessages[buttonId]), false);
        }
    }
}
