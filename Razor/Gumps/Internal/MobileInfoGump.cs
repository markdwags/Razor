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

using System.Text;
using System.Windows.Forms;

namespace Assistant.Gumps.Internal
{
    public sealed class MobileInfoGump : Gump
    {
        private enum MobileInfoButtons
        {
            Okay,
            CopyName,
            CopySerial,
            CopyHue,
            CopyBody,
            Name,
            Serial,
            Hue,
            Body
        }

        private Mobile _mobile { get; }

        public MobileInfoGump(Mobile mobile) : base(500, 250, -1)
        {
            _mobile = mobile;

            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            Resend = true;

            AddPage(0);

            AddBackground(106, 72, 253, 289, 3600);

            AddLabel(143, 115, 2954, "Name:");
            AddLabel(142, 140, 2954, "Serial:");
            AddLabel(142, 164, 2954, "Hue:");
            AddLabel(142, 188, 2954, "Body:");
            AddLabel(197, 87, 2954, "Mobile Data");

            AddButton(276, 318, 247, 248, (int)MobileInfoButtons.Okay, GumpButtonType.Reply, 0);

            AddButton(124, 121, 2103, 2104, (int)MobileInfoButtons.CopyName, GumpButtonType.Reply, 0);
            AddButton(124, 144, 2103, 2104, (int)MobileInfoButtons.CopySerial, GumpButtonType.Reply, 0);
            AddButton(124, 169, 2103, 2104, (int)MobileInfoButtons.CopyHue, GumpButtonType.Reply, 0);
            AddButton(124, 194, 2103, 2104, (int)MobileInfoButtons.CopyBody, GumpButtonType.Reply, 0);

            AddTextEntry(219, 118, 116, 20, 62, (int)MobileInfoButtons.Name, $"{mobile.Name}");
            AddTextEntry(219, 142, 116, 20, 62, (int)MobileInfoButtons.Serial, $"{mobile.Serial}");

            if (mobile.Hue == 0)
                AddTextEntry(219, 166, 116, 20, 62, (int)MobileInfoButtons.Hue, "Default (0)");
            else
                AddTextEntry(219, 166, 116, 20, mobile.Hue, (int)MobileInfoButtons.Hue, $"{mobile.Hue}");

            AddTextEntry(219, 190, 116, 20, 62, (int) MobileInfoButtons.Body, $"{mobile.Body}");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Position: {_mobile.Position}");
            sb.AppendLine($"Notoriety: {_mobile.Notoriety}");
            
            if (mobile.IsGhost)
                sb.AppendLine("IsGhost: True");
            if (mobile.IsHuman)
                sb.AppendLine("IsHuman: True");
            if (mobile.IsMonster)
                sb.AppendLine("IsMonster: True");

            sb.AppendLine($"Blessed: {mobile.Blessed}");

            AddHtml(124, 223, 211, 88, sb.ToString(), true, true);
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            switch (buttonId)
            {
                case (int)MobileInfoButtons.CopyName:
                    Clipboard.SetText(_mobile.Name);
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _mobile.Name), false);
                    break;
                case (int)MobileInfoButtons.CopySerial:
                    Clipboard.SetText(_mobile.Serial.ToString());
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _mobile.Serial.ToString()), false);
                    break;
                case (int)MobileInfoButtons.CopyHue:
                    Clipboard.SetText(_mobile.Hue.ToString());
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _mobile.Hue.ToString()), false);
                    break;
                case (int)MobileInfoButtons.CopyBody:
                    Clipboard.SetText(_mobile.Body.ToString());
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, _mobile.Body.ToString()), false);
                    break;
                case (int)MobileInfoButtons.Okay:
                    Resend = false;
                    break;
            }

            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}