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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Assistant.Core;
using Ultima;

namespace Assistant.Gumps.Internal
{
    public sealed class CooldownGump : Gump
    {
        public CooldownGump() : base(100, 100, -1)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            
            AddPage(0);
            AddHtml(110, 77, 106, 24, "<CENTER><BIG><BASEFONT COLOR=#E0E70B>Cooldowns</BASEFONT></BIG></CENTER>", false, false);

            int currentY = 100;
            
            foreach (KeyValuePair<string, Cooldown> cooldown in CooldownManager.Cooldowns.OrderByDescending(c => (c.Value.EndTime - DateTime.UtcNow).TotalSeconds))
            {
                //get the number of seconds left
                TimeSpan diff = cooldown.Value.EndTime - DateTime.UtcNow;
                int timeLeft = (int)diff.TotalSeconds;

                if (timeLeft < 0)
                {
                    timeLeft = 0;
                }

                Color foreColor;
                int labelHue;
                
                if (timeLeft > 10) //green
                {
                    foreColor = Color.Green;
                    labelHue = 62;

                    if (cooldown.Value.Hue > 0)
                    {
                        labelHue = cooldown.Value.Hue;
                    }
                }
                else if (timeLeft > 5) //yellow
                {
                    foreColor = Color.FromArgb(253, 216, 53);
                    labelHue = 52;

                    if (cooldown.Value.Hue > 0)
                    {
                        labelHue = cooldown.Value.Hue;
                    }
                }
                else //red
                {
                    foreColor = Color.DarkRed;
                    labelHue = 32;

                    if (cooldown.Value.Hue > 0)
                    {
                        labelHue = cooldown.Value.Hue;
                    }
                }

                if (cooldown.Value.Icon > 0)
                {
                    AddImage(80, currentY, cooldown.Value.Icon);
                }

                AddProgressBar(110, currentY, 110, 28, timeLeft, cooldown.Value.Seconds, Color.Black, Color.Black, foreColor);
                AddLabelCropped(120, currentY, 100, 28, labelHue, $"{cooldown.Key} ({timeLeft}s)");

                if (timeLeft == 0)
                {
                    AddImage(224, currentY + 4, 9009);
                }

                currentY += 30;
            }
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonId == 0)
            {
                World.Player?.SendMessage("All cooldowns have been stopped");
                CooldownManager.Stop();
            }
            
            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}