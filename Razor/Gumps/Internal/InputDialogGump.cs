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

using System;
using Assistant.Agents;

namespace Assistant.Gumps.Internal
{
    public sealed class InputDialogGump : Gump
    {
        private enum Controls
        {
            Okay,
            InputEntry,
        }
        
        private int _inputReference { get; set; }
        //private InputDialogTypes _type { get; set; }
        private readonly Func<int, string, bool> _inputCallback;

        /// <summary>
        /// Simple input dialog to replace UI based input boxes
        /// </summary>
        /// <param name="inputCallback">Method to execute when Okay is pressed</param>
        /// <param name="inputReference">Optional int to be used as a reference on the callback</param>
        /// <param name="inputRequest">The text in the gump asking the user to enter something specific</param>
        /// <param name="defaultInput">Optional string to pre-populate the gump textbox input with</param>
        public InputDialogGump(Func<int, string, bool> inputCallback, int inputReference, string inputRequest, string defaultInput = "") : base(100, 100)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            Resend = false;

            _inputReference = inputReference;
            _inputCallback = inputCallback;

            AddBackground(105, 70, 253, 121, 3600);
            AddBackground(129, 115, 209, 22, 9200);
            AddLabel(125, 90, 2954, inputRequest);
            AddButton(273, 145, 247, 248, (int)Controls.Okay, GumpButtonType.Reply, 0);
            AddTextEntry(133, 117, 204, 20, 2954, (int)Controls.InputEntry, defaultInput);

        }

        public override void OnResponse(int buttonID, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonID == (int)Controls.Okay)
            {
                if (!string.IsNullOrEmpty(textEntries?[0]?.Text))
                {
                    _inputCallback(_inputReference, textEntries[0].Text);

                    CloseGump();
                }
            }
        }
    }
}
