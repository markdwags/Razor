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

using System.Reflection;

namespace Assistant.Gumps.Internal
{
    public sealed class BoatControlGump : Gump
    {
        public BoatControlGump(int speed) : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Movable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(48, 28, 182, 195, 9270);

            AddButton(130, 69, 5600, 5604, (int) Buttons.OneForward, GumpButtonType.Reply, 0);
            AddButton(130, 109, 5602, 5606, (int) Buttons.OneBackwards, GumpButtonType.Reply, 0);
            AddButton(108, 89, 5603, 5607, (int) Buttons.OneLeft, GumpButtonType.Reply, 0);
            AddButton(150, 89, 5601, 5605, (int) Buttons.OneRight, GumpButtonType.Reply, 0);
            AddButton(173, 86, 9903, 9905, (int) Buttons.Right, GumpButtonType.Reply, 0);
            AddButton(81, 86, 9909, 9911, (int) Buttons.Left, GumpButtonType.Reply, 0);
            AddButton(128, 47, 9900, 9902, (int) Buttons.Forward, GumpButtonType.Reply, 0);
            AddButton(128, 129, 9906, 9908, (int) Buttons.Back, GumpButtonType.Reply, 0);
            AddButton(130, 90, 2117, 2118, (int) Buttons.Stop, GumpButtonType.Reply, 0);
            AddButton(69, 118, 22406, 22407, (int) Buttons.TurnLeft, GumpButtonType.Reply, 0);
            AddButton(189, 118, 22400, 22401, (int) Buttons.TurnRight, GumpButtonType.Reply, 0);
            AddButton(162, 66, 5106, 5106, (int) Buttons.ForwardRight, GumpButtonType.Reply, 0);
            AddButton(162, 120, 5100, 5100, (int) Buttons.BackRight, GumpButtonType.Reply, 0);
            AddButton(106, 120, 5102, 5102, (int) Buttons.BackLeft, GumpButtonType.Reply, 0);
            AddButton(106, 66, 5108, 5108, (int) Buttons.ForwardLeft, GumpButtonType.Reply, 0);

            AddCheck(64, 188, 210, 211, speed == 0, (int) Buttons.Reg);
            AddLabel(87, 189, 990, "Reg");
            AddCheck(115, 189, 210, 211, speed == 1, (int) Buttons.Slow);
            AddLabel(135, 189, 990, "Slow");
            AddCheck(169, 189, 210, 211, speed == 2, (int) Buttons.One);
            AddLabel(189, 189, 990, "One");

            AddButton(66, 157, 5600, 5604, (int) Buttons.AnchorUp, GumpButtonType.Reply, 0);
            AddButton(83, 157, 5602, 5606, (int) Buttons.AnchorDown, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(int buttonID, int[] switches, GumpTextEntry[] textEntries = null)
        {
            int speed = 0;

            foreach (int check in switches)
            {
                if (check == (int) Buttons.Reg)
                {
                    speed = 0;
                    break;
                }

                if (check == (int)Buttons.Slow)
                {
                    speed = 1;
                    break;
                }
                
                if (check == (int)Buttons.One)
                {
                    speed = 2;
                    break;
                }
            }

            switch (buttonID)
            {
                case (int) Buttons.AnchorDown:
                    SendBoatCommand("Lower Anchor");
                    break;
                case (int) Buttons.AnchorUp:
                    SendBoatCommand("Raise Anchor");
                    break;
                case (int) Buttons.Back:
                    SendBoatCommand("Back", speed);
                    break;
                case (int) Buttons.BackLeft:
                    SendBoatCommand("Back Left", speed);
                    break;
                case (int) Buttons.BackRight:
                    SendBoatCommand("Back Right", speed);
                    break;
                case (int) Buttons.Forward:
                    SendBoatCommand("Forward", speed);
                    break;
                case (int) Buttons.ForwardLeft:
                    SendBoatCommand("Forward Left", speed);
                    break;
                case (int) Buttons.ForwardRight:
                    SendBoatCommand("Forward Right", speed);
                    break;
                case (int) Buttons.Left:
                    SendBoatCommand("Left", speed);
                    break;
                case (int) Buttons.OneBackwards:
                    SendBoatCommand("Back", 2);
                    break;
                case (int) Buttons.OneForward:
                    SendBoatCommand("Forward", 2);
                    break;
                case (int) Buttons.OneLeft:
                    SendBoatCommand("Left", 2);
                    break;
                case (int) Buttons.OneRight:
                    SendBoatCommand("Right", 2);
                    break;
                case (int) Buttons.Right:
                    SendBoatCommand("Right", speed);
                    break;
                case (int) Buttons.TurnLeft:
                    SendBoatCommand("Turn Left");
                    break;
                case (int) Buttons.TurnRight:
                    SendBoatCommand("Turn Right");
                    break;
                case (int) Buttons.Stop:
                    SendBoatCommand("Stop");
                    break;
            }

            if (buttonID != 0)
            {
                CloseGump();

                BoatControlGump gump = new BoatControlGump(speed);
                gump.SendGump();
            }
        }

        /// <summary>
        /// Handles basic logic on sending boat commands
        /// </summary>
        /// <param name="command"></param>
        /// <param name="speed">0 = reg, 1 = slow, 2 = one</param>
        /// <param name="directionCommand"></param>
        private void SendBoatCommand(string command, int speed = 0, bool directionCommand = true)
        {
            string boatCommand = command;

            if (speed == 2 && directionCommand)
            {
                boatCommand = $"{command} One";
            }
            else if (speed == 1 && directionCommand)
            {
                boatCommand = $"Slow {command}";
            }

            if (World.Player != null)
            {
                World.Player.Say(boatCommand);
            }
        }

        public enum Buttons
        {
            OneForward = 1,
            OneBackwards,
            OneLeft,
            OneRight,
            Right,
            Left,
            Forward,
            Back,
            Stop,
            TurnLeft,
            TurnRight,
            ForwardRight,
            BackRight,
            BackLeft,
            ForwardLeft,
            Reg,
            Slow,
            One,
            AnchorUp,
            AnchorDown,
        }
    }
}