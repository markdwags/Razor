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
using System.Security.Cryptography;
using Assistant.Core;
using Ultima;

namespace Assistant.Gumps.Internal
{
    public sealed class BuffGump : Gump
    {
        public BuffGump() : base(50, 50, -1)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;
            
            AddPage(0);
            
            int currentY = 100;

            int barHeight = Config.GetInt("ShowBuffDebuffHeight");
            int barWidth = Config.GetInt("ShowBuffDebuffWidth");
            bool showIcons = Config.GetBool("ShowBuffDebuffIcons");

            IOrderedEnumerable<BuffDebuff> buffDebuffs;

            switch (Config.GetInt("ShowBuffDebuffSort"))
            {
                case 0:
                    buffDebuffs = World.Player.BuffsDebuffs.OrderBy(buff => buff.ClilocMessage1);
                    break;
                case 1:
                    buffDebuffs = World.Player.BuffsDebuffs.OrderBy(buff => (buff.Duration - (DateTime.UtcNow - buff.Timestamp).TotalSeconds));
                    break;
                case 2:
                    buffDebuffs = World.Player.BuffsDebuffs.OrderByDescending(buff => (buff.Duration - (DateTime.UtcNow - buff.Timestamp).TotalSeconds));
                    break;
                default:
                    buffDebuffs = World.Player.BuffsDebuffs.OrderByDescending(buff => (buff.Duration - (DateTime.UtcNow - buff.Timestamp).TotalSeconds));
                    break;
            }

            foreach (BuffDebuff buff in buffDebuffs)
            {
                TimeSpan diff = DateTime.UtcNow - buff.Timestamp;
                var timeLeft = buff.Duration - (int)diff.TotalSeconds;

                string timeLeftDisplay;

                if (timeLeft < 0)
                {
                    timeLeft = 0; 
                    timeLeftDisplay = "-";
                }
                else
                {
                    timeLeftDisplay = $"{timeLeft}s";
                }

                Color barColor;
                Color bgColor;
                int labelHue;

                if (BuffDebuffManager.IsBuff(buff.IconId))
                {
                    barColor = Color.Green;
                    bgColor = Config.GetBool("UseBlackBuffDebuffBg") ? Color.Black : Color.DarkGreen; 

                    labelHue = 62;
                }
                else if (BuffDebuffManager.IsDebuff(buff.IconId))
                {
                    barColor = Color.Red;
                    bgColor = Config.GetBool("UseBlackBuffDebuffBg") ? Color.Black : Color.DarkRed;

                    labelHue = 32;
                } 
                else if (BuffDebuffManager.IsState(buff.IconId))
                {
                    barColor = Color.Blue;
                    bgColor = Config.GetBool("UseBlackBuffDebuffBg") ? Color.Black : Color.DarkBlue;

                    labelHue = 102;
                }
                else
                {
                    barColor = Color.Gray;
                    bgColor = Config.GetBool("UseBlackBuffDebuffBg") ? Color.Black : Color.DarkGray;

                    labelHue = 922;
                }

                if (showIcons)
                {
                    AddImage(80, currentY, buff.IconId);
                }

                AddProgressBar(110, currentY, barWidth, barHeight, timeLeft, buff.Duration == 0 ? 100 : buff.Duration,
                    Color.Black, bgColor, barColor);

                switch (Config.GetInt("ShowBuffDebuffTimeType"))
                {
                    case 0: //next to name
                        AddLabelCropped(114, currentY, barWidth, barHeight, labelHue, $"{buff} ({timeLeftDisplay})");
                        break;
                    case 1: // outside of bar (left)
                        AddLabelCropped(114, currentY, barWidth, barHeight, labelHue, $"{buff}");
                        AddHtml(114 + barWidth, currentY, 50, barHeight, $"<BIG><BASEFONT COLOR=#FFFFFF>{timeLeftDisplay}</BASEFONT></BIG>", false, false);
                        break;
                    case 2: // right side of bar (inside)
                        AddLabelCropped(114, currentY, barWidth, barHeight, labelHue, $"{buff}");
                        AddHtml(114, currentY, barWidth, barHeight, $"<DIV ALIGN=\"RIGHT\"><BIG><BASEFONT COLOR=#FFFFFF>{timeLeftDisplay}</BASEFONT></BIG></DIV>", false, false);
                        break;
                    case 3: // under icon (28x28 icon)
                        AddLabelCropped(114, currentY, barWidth, barHeight, labelHue, $"{buff}");
                        AddHtml(80, currentY + 32, 50, barHeight, $"<BIG><BASEFONT COLOR=#FFFFFF>{timeLeftDisplay}</BASEFONT></BIG>", false, false);
                        break;
                    case 4: //none
                        AddLabelCropped(114, currentY, barWidth, barHeight, labelHue, $"{buff}");
                        break;
                    default:
                        AddLabelCropped(114, currentY, barWidth, barHeight, labelHue, $"{buff} ({timeLeftDisplay})");
                        break;
                }

                if (barHeight > 28)
                {
                    currentY += barHeight + 4;
                }
                else
                {
                    currentY += 32;
                }
            }
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            /*if (buttonId == 0)
            {
                World.Player?.SendMessage("All cooldowns have been stopped");
                CooldownManager.Stop();
            }*/
            
            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}