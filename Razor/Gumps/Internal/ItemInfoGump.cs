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

using System.Windows.Forms;

namespace Assistant.Gumps.Internal
{
    public sealed class ItemInfoGump : Gump
    {
        private enum ItemInfoButtons
        {
            Okay,
            CopyItemName,
            CopySerial,
            CopyId,
            CopyHue,
            ItemName,
            Serial,
            Id,
            Hue
        }

        private Item _item { get; }

        public ItemInfoGump(Item item) : base(271, 130, -1)
        {
            _item = item;

            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            Resend = true;

            AddPage(0);
            
            AddBackground(106, 70, 253, 189, 3600);

            AddLabel(197, 87, 154, "Item Data");
            AddItem(290, 98, item.ItemID);

            AddLabel(143, 115, 900, "Item Name:");
            AddLabel(142, 140, 900, "Serial:");
            AddLabel(142, 164, 900, "Id:");
            AddLabel(142, 188, 900, "Hue:");

            AddButton(274, 217, 247, 248, (int)ItemInfoButtons.Okay, GumpButtonType.Reply, 0);
            AddButton(124, 120, 2103, 2104, (int)ItemInfoButtons.CopyItemName, GumpButtonType.Reply, 0);
            AddButton(124, 143, 2103, 2104, (int)ItemInfoButtons.CopySerial, GumpButtonType.Reply, 0);
            AddButton(124, 168, 2103, 2104, (int)ItemInfoButtons.CopyId, GumpButtonType.Reply, 0);
            AddButton(124, 191, 2103, 2104, (int)ItemInfoButtons.CopyHue, GumpButtonType.Reply, 0);
            
            AddTextEntry(219, 115, 116, 20, 62, (int)ItemInfoButtons.ItemName, $"{item.ItemID.ItemData.Name}");
            AddTextEntry(219, 141, 116, 20, 62, (int)ItemInfoButtons.Serial, $"{item.Serial}");
            AddTextEntry(219, 165, 116, 20, 62, (int)ItemInfoButtons.Id, $"{item.ItemID.Value}");

            if (item.Hue == 0)
                AddTextEntry(219, 188, 116, 20, 62, (int)ItemInfoButtons.Hue, "Default (0)");
            else
                AddTextEntry(219, 188, 116, 20, item.Hue - 1, (int)ItemInfoButtons.Hue, $"{item.Hue}");

        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            switch (buttonId)
            {
                case (int)ItemInfoButtons.CopyItemName:
                    Clipboard.SetText(_item.ItemID.ItemData.Name);
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _item.ItemID.ItemData.Name), false);
                    break;
                case (int)ItemInfoButtons.CopySerial:
                    Clipboard.SetText(_item.Serial.ToString());
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _item.Serial.ToString()), false);
                    break;
                case (int)ItemInfoButtons.CopyId:
                    Clipboard.SetText(_item.ItemID.Value.ToString());
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _item.ItemID.Value.ToString()), false);
                    break;
                case (int)ItemInfoButtons.CopyHue:
                    Clipboard.SetText(_item.Hue.ToString());
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _item.Hue.ToString()), false);
                    break;
                case (int)ItemInfoButtons.Okay:
                    Resend = false;
                    break;
            }

            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}