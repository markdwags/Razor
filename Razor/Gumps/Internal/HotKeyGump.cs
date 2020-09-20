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
    public sealed class HotKeyGump : Gump
    {
        public HotKeyGump(bool alt, bool ctrl, bool shift) : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;

            int pageCount = 1;

            AddPage(pageCount);

            AddBackground(0, 0, 326, 450, 9270);
            AddLabel(16, 16, 52, "Assigned Razor Hot Keys");

            AddCheck(16, 38, 210, 211, alt, 1);
            AddLabel(40, 38, 52, "Alt");
            AddCheck(68, 38, 210, 211, ctrl, 2);
            AddLabel(92, 38, 52, "Ctrl");
            AddCheck(127, 38, 210, 211, shift, 3);
            AddLabel(151, 38, 52, "Shift");
            AddButton(197, 38, 4011, 4012, 1, GumpButtonType.Reply, 0);

            int labelY = 40;
            int count = 0;

            foreach (KeyData data in HotKey.List.Where(hk => hk.Key != 0).OrderBy(hk => hk.DispName))
            {
                AddHtml(20, labelY, 500, 20, $"<BASEFONT COLOR=#FFFFFF>{data.DispName}</BASEFONT> <BASEFONT COLOR=#E0E70B>({data.KeyString()})</BASEFONT>", false, false);
                
                labelY += 20;
                count++;

                if (count % 20 == 0)
                {
                    AddButton(292, 17, 9903, 9905, pageCount + 2, GumpButtonType.Page, pageCount + 1);

                    pageCount++;
                    labelY = 40;

                    AddPage(pageCount);

                    AddBackground(0, 0, 326, 480, 9270);
                    AddLabel(16, 16, 52, "Assigned Razor Hot Keys");
                    AddButton(270, 18, 9909, 9911, pageCount + 1, GumpButtonType.Page, pageCount - 1);
                }
            }
        }

        public override void OnResponse(int buttonID, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonID == 1)
            {
                bool alt = false;
                bool ctrl = false;
                bool shift = false;

                foreach (int check in switches)
                {
                    switch (check)
                    {
                        case 1:
                            alt = true;
                            break;
                        case 2:
                            ctrl = true;
                            break;
                        case 3:
                            shift = true;
                            break;
                    }
                }

                CloseGump();
                HotKeyGump gump = new HotKeyGump(alt, ctrl, shift);
                gump.SendGump();

            }

            base.OnResponse(buttonID, switches, textEntries);
        }
    }
}
