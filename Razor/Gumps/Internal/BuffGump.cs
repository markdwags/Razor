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
                    buffDebuffs = World.Player.BuffsDebuffs.OrderBy(buff => buff);
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
                //get the number of seconds left
                TimeSpan diff = DateTime.UtcNow - buff.Timestamp;
                int timeLeft = buff.Duration - (int)diff.TotalSeconds;

                if (timeLeft < 0)
                {
                    timeLeft = 0;
                }

                Color barColor = Color.Green;
                int hue;

                //based on seconds, use a different style bar
                if (timeLeft > 10) //green
                {
                    hue = 62;
                }
                else if (timeLeft > 5) //yellow
                {
                    hue = 52;
                }
                else //red
                {
                    hue = 32;
                }

                if (showIcons)
                {
                    AddImage(80, currentY - 4, BuffDebuffManager.BuffTable[buff.IconId]);
                }

                AddProgressBar(110, currentY, barWidth + 110, barHeight, timeLeft, buff.Duration, Color.Black, Color.Black, barColor);
                AddLabelCropped(120, currentY, barWidth, barHeight, hue, $"{buff} ({timeLeft}s)");

                currentY += 26;
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