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
using Assistant.Core;

namespace Assistant.Gumps.Internal
{
    public sealed class CooldownGump : Gump
    {
        public CooldownGump() : base(500, 250, -1)
        {
            X = 300;
            Y = 200;

            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            
            AddPage(0);
            //background +26 per cooldown
            AddBackground(96, 64, 138, 68 + (CooldownManager.Cooldowns.Count * 26), 9270);
            AddHtml(110, 77, 106, 24, $"<CENTER><BIG><BASEFONT COLOR=#E0E70B>Cooldowns</BASEFONT></BIG></CENTER>", false, false);

            int labelY = 100;
            
            foreach (KeyValuePair<string, Cooldown> cooldown in CooldownManager.Cooldowns)
            {
                //get the number of seconds left
                TimeSpan diff = cooldown.Value.EndTime - DateTime.UtcNow;
                int timeLeft = (int)diff.TotalSeconds;
                double percent = (diff.TotalSeconds / cooldown.Value.Seconds) * 100;
                
                //based on seconds, use a different style bar
                if (percent > 25) //green
                {
                    int hue = 62;
                    if (cooldown.Value.Hue > 0)
                    {
                        hue = cooldown.Value.Hue;
                    }
                    
                    AddProgressBar(110, labelY, 110, 18, $"{cooldown.Key}", timeLeft, cooldown.Value.Seconds, Color.Black, Color.Black, Color.Green);
                    AddLabelCropped(120, labelY, 100, 18, hue, $"{cooldown.Key} ({timeLeft}s) ");
                }
                else if (percent > 10) //yellow
                {
                    int hue = 54;
                    if (cooldown.Value.Hue > 0)
                    {
                        hue = cooldown.Value.Hue;
                    }
                    
                    AddProgressBar(110, labelY, 110, 18, $"{cooldown.Key}", timeLeft, cooldown.Value.Seconds, Color.Black, Color.Black, Color.Yellow);
                    AddLabelCropped(120, labelY, 100, 18, hue, $"{cooldown.Key} ({timeLeft}s)");
                    
                }
                else //red
                {
                    int hue = 32;
                    if (cooldown.Value.Hue > 0)
                    {
                        hue = cooldown.Value.Hue;
                    }
                    
                    AddProgressBar(110, labelY, 110, 18, $"{cooldown.Key}", timeLeft, cooldown.Value.Seconds, Color.Black, Color.Black, Color.Red);
                    AddLabelCropped(120, labelY, 100, 18, hue, $"{cooldown.Key} ({timeLeft}s)");
                }
                
                //AddHtml(110, labelY - 4, 100, 18, $"<CENTER><BASEFONT COLOR=#FFFFFF>{cooldown.Key} ({timeLeft}s)</BASEFONT></CENTER>", false, false);
                //AddLabel(110, labelY - 4, 1001, $"{cooldown.Key} ({timeLeft}s)");
                
                labelY += 26;
            }
        }

        public void AddProgressBar(int x, int y, int w, int h, string text, double min, double max, Color color, Color back, Color fore)
        {
            string bg = $"<BODYBGCOLOR=#{back.ToArgb():X}>";
            string fg = $"<BODYBGCOLOR=#{fore.ToArgb():X}>";

            //var label = $"<BASEFONT COLOR=#{color.ToArgb():X}><CENTER>{text} {(min / max) * 100:#,0.0}%";
            string label = $"<BASEFONT COLOR=#{color.ToArgb():X}>";

            AddHtml(x, y, w, h, bg, false, false);
            AddHtml(x + 2, y + 2, (int) Math.Ceiling((w - 4) * (min / max)), h - 4, fg, false, false);
            AddHtml(x + 2, y + 2, w - 4, h - 4, label, false, false);
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            //World.Player.OverheadMessage($"Button {buttonId}");

            if (buttonId == 0)
            {
                World.Player?.SendMessage("All cooldowns have been stopped");
                CooldownManager.Stop();
            }
            
            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}