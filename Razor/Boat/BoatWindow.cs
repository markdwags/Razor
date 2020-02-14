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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistant.Boat
{
    public partial class BoatWindow : Form
    {
        private enum Orientation
        {
            Normal,
            Northwest,
            Southwest,
            Southeast,
            Northeast
        }

        private int _orientation = 0;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private bool _inT2A = false;
        private bool _ancherUp = false;

        public BoatWindow()
        {
            InitializeComponent();
            this.MouseDown += BoatWindow_MouseDown;
            this.boatBorder.MouseDown += BoatWindow_MouseDown;
        }

        private void BoatWindow_Load(object sender, EventArgs e)
        {
            alwaysOnTop.Checked = true;
        }

        private void BoatWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void SendBoatCommand(string command, bool directionCommand = true)
        {
            string boatCommand = command;

            if (boatSpeedOne.Checked && directionCommand)
            {
                boatCommand = $"{command} One";
            }
            else if (boatSpeedSlow.Checked && directionCommand)
            {
                boatCommand = $"Slow {command}";
            }

            if (World.Player != null)
            {
                World.Player.Say(boatCommand);
            }
        }

        private string[] _boatDirCommands =
            {"Forward", "Forward Right", "Right", "Back Right", "Back", "Back Left", "Left", "Forward Left"};
        //        0             1            2           3          4        5          6           7

        private void boatNorth_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:

                    switch (_orientation)
                    {
                        case (int) Orientation.Northwest:
                            SendBoatCommand(_boatDirCommands[1]);
                            break;
                        case (int) Orientation.Southwest:
                            SendBoatCommand(_boatDirCommands[3]);
                            break;
                        case (int) Orientation.Southeast:
                            SendBoatCommand(_boatDirCommands[5]);
                            break;
                        case (int) Orientation.Northeast:
                            SendBoatCommand(_boatDirCommands[7]);
                            break;
                        default:
                            SendBoatCommand(_boatDirCommands[0]);
                            break;
                    }

                    //SendBoatCommand(_boatDirCommands[1]); //NW orientation
                    //SendBoatCommand(_boatDirCommands[3]); //SW orientation
                    //SendBoatCommand(_boatDirCommands[5]); //SE orientation
                    //SendBoatCommand(_boatDirCommands[7]); //NE orientation
                    break;
                case MouseButtons.Right:
                    _orientation = (int) Orientation.Normal;

                    boatNorth.BorderStyle = BorderStyle.FixedSingle;
                    boatNortheast.BorderStyle = BorderStyle.None;
                    boatNorthwest.BorderStyle = BorderStyle.None;
                    boatSouthwest.BorderStyle = BorderStyle.None;
                    boatSoutheast.BorderStyle = BorderStyle.None;

                    if (World.Player != null)
                        World.Player.SendMessage(MsgLevel.Force, "Set Boat Orientation: Normal");

                    break;
            }
        }

        private void boatNorthwest_Click(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (_orientation)
                    {
                        case (int) Orientation.Northwest:
                            SendBoatCommand(_boatDirCommands[0]);
                            break;
                        case (int) Orientation.Southwest:
                            SendBoatCommand(_boatDirCommands[2]);
                            break;
                        case (int) Orientation.Southeast:
                            SendBoatCommand(_boatDirCommands[4]);
                            break;
                        case (int) Orientation.Northeast:
                            SendBoatCommand(_boatDirCommands[6]);
                            break;
                        default:
                            SendBoatCommand(_boatDirCommands[7]);
                            break;
                    }

                    break;
                case MouseButtons.Right:
                    _orientation = (int) Orientation.Northwest;

                    boatNorth.BorderStyle = BorderStyle.None;
                    boatNortheast.BorderStyle = BorderStyle.None;
                    boatNorthwest.BorderStyle = BorderStyle.FixedSingle;
                    boatSouthwest.BorderStyle = BorderStyle.None;
                    boatSoutheast.BorderStyle = BorderStyle.None;

                    if (World.Player != null)
                        World.Player.SendMessage(MsgLevel.Force, "Set Boat Orientation: Northwest");


                    break;
            }
        }

        private void boatSouth_Click(object sender, EventArgs e)
        {
            switch (_orientation)
            {
                case (int) Orientation.Northwest:
                    SendBoatCommand(_boatDirCommands[5]);
                    break;
                case (int) Orientation.Southwest:
                    SendBoatCommand(_boatDirCommands[7]);
                    break;
                case (int) Orientation.Southeast:
                    SendBoatCommand(_boatDirCommands[1]);
                    break;
                case (int) Orientation.Northeast:
                    SendBoatCommand(_boatDirCommands[3]);
                    break;
                default:
                    SendBoatCommand(_boatDirCommands[4]);
                    break;
            }
        }

        private void boatSoutheast_Click(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (_orientation)
                    {
                        case (int) Orientation.Northwest:
                            SendBoatCommand(_boatDirCommands[4]);
                            break;
                        case (int) Orientation.Southwest:
                            SendBoatCommand(_boatDirCommands[6]);
                            break;
                        case (int) Orientation.Southeast:
                            SendBoatCommand(_boatDirCommands[0]);
                            break;
                        case (int) Orientation.Northeast:
                            SendBoatCommand(_boatDirCommands[2]);
                            break;
                        default:
                            SendBoatCommand(_boatDirCommands[3]);
                            break;
                    }

                    break;
                case MouseButtons.Right:
                    _orientation = (int) Orientation.Southeast;

                    boatNorth.BorderStyle = BorderStyle.None;
                    boatNortheast.BorderStyle = BorderStyle.None;
                    boatNorthwest.BorderStyle = BorderStyle.None;
                    boatSouthwest.BorderStyle = BorderStyle.None;
                    boatSoutheast.BorderStyle = BorderStyle.FixedSingle;

                    if (World.Player != null)
                        World.Player.SendMessage(MsgLevel.Force, "Set Boat Orientation: Southeast");

                    break;
            }
        }

        private void boatSouthwest_Click(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (_orientation)
                    {
                        case (int) Orientation.Northwest:
                            SendBoatCommand(_boatDirCommands[6]);
                            break;
                        case (int) Orientation.Southwest:
                            SendBoatCommand(_boatDirCommands[0]);
                            break;
                        case (int) Orientation.Southeast:
                            SendBoatCommand(_boatDirCommands[2]);
                            break;
                        case (int) Orientation.Northeast:
                            SendBoatCommand(_boatDirCommands[4]);
                            break;
                        default:
                            SendBoatCommand(_boatDirCommands[5]);
                            break;
                    }

                    break;
                case MouseButtons.Right:
                    _orientation = (int) Orientation.Southwest;

                    boatNorth.BorderStyle = BorderStyle.None;
                    boatNortheast.BorderStyle = BorderStyle.None;
                    boatNorthwest.BorderStyle = BorderStyle.None;
                    boatSouthwest.BorderStyle = BorderStyle.FixedSingle;
                    boatSoutheast.BorderStyle = BorderStyle.None;

                    if (World.Player != null)
                        World.Player.SendMessage(MsgLevel.Force, "Set Boat Orientation: Southwest");

                    break;
            }
        }

        private void boatWest_Click(object sender, EventArgs e)
        {
            switch (_orientation)
            {
                case (int) Orientation.Northwest:
                    SendBoatCommand(_boatDirCommands[7]);
                    break;
                case (int) Orientation.Southwest:
                    SendBoatCommand(_boatDirCommands[1]);
                    break;
                case (int) Orientation.Southeast:
                    SendBoatCommand(_boatDirCommands[3]);
                    break;
                case (int) Orientation.Northeast:
                    SendBoatCommand(_boatDirCommands[5]);
                    break;
                default:
                    SendBoatCommand(_boatDirCommands[6]);
                    break;
            }
        }

        private void boatNortheast_Click(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (_orientation)
                    {
                        case (int) Orientation.Northwest:
                            SendBoatCommand(_boatDirCommands[2]);
                            break;
                        case (int) Orientation.Southwest:
                            SendBoatCommand(_boatDirCommands[4]);
                            break;
                        case (int) Orientation.Southeast:
                            SendBoatCommand(_boatDirCommands[6]);
                            break;
                        case (int) Orientation.Northeast:
                            SendBoatCommand(_boatDirCommands[0]);
                            break;
                        default:
                            SendBoatCommand(_boatDirCommands[1]);
                            break;
                    }

                    break;
                case MouseButtons.Right:
                    _orientation = (int) Orientation.Northeast;

                    boatNorth.BorderStyle = BorderStyle.None;
                    boatNortheast.BorderStyle = BorderStyle.FixedSingle;
                    boatNorthwest.BorderStyle = BorderStyle.None;
                    boatSouthwest.BorderStyle = BorderStyle.None;
                    boatSoutheast.BorderStyle = BorderStyle.None;

                    if (World.Player != null)
                        World.Player.SendMessage(MsgLevel.Force, "Set Boat Orientation: Northeast");

                    break;
            }
        }

        private void boatEast_Click(object sender, EventArgs e)
        {
            switch (_orientation)
            {
                case (int) Orientation.Northwest:
                    SendBoatCommand(_boatDirCommands[3]);
                    break;
                case (int) Orientation.Southwest:
                    SendBoatCommand(_boatDirCommands[5]);
                    break;
                case (int) Orientation.Southeast:
                    SendBoatCommand(_boatDirCommands[7]);
                    break;
                case (int) Orientation.Northeast:
                    SendBoatCommand(_boatDirCommands[1]);
                    break;
                default:
                    SendBoatCommand(_boatDirCommands[2]);
                    break;
            }
        }

        private void boatTurnRight_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Turn Right", false);

            switch (_orientation)
            {
                case (int) Orientation.Northwest:
                    boatNortheast_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Southwest:
                    boatNorthwest_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Southeast:
                    boatSouthwest_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Northeast:
                    boatSoutheast_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
            }
        }

        private void boatTurnLeft_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Turn Left", false);

            switch (_orientation)
            {
                case (int) Orientation.Northwest:
                    boatSouthwest_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Southwest:
                    boatSoutheast_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Southeast:
                    boatNortheast_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Northeast:
                    boatNorthwest_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
            }
        }

        private void boatTurnAround_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Turn Around", false);

            switch (_orientation)
            {
                case (int) Orientation.Northwest:
                    boatSoutheast_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Southwest:
                    boatNortheast_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Southeast:
                    boatNorthwest_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
                case (int) Orientation.Northeast:
                    boatSouthwest_Click(null, new MouseEventArgs(MouseButtons.Right, 1, 1, 1, 1));
                    break;
            }
        }

        private void boatAnchor_Click(object sender, EventArgs e)
        {
            if (_ancherUp)
            {
                SendBoatCommand("Drop Anchor", false);
                _ancherUp = false;
            }
            else
            {
                SendBoatCommand("Raise Anchor", false);
                _ancherUp = true;
            }
        }

        private void boatT2a_Click(object sender, EventArgs e)
        {
            if (_inT2A)
            {
                SendBoatCommand("Sueacron", false);
                _inT2A = false;
            }
            else
            {
                SendBoatCommand("Doracron", false);
                _inT2A = true;
            }
        }

        private void alwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = alwaysOnTop.Checked;
        }

        private void boatGoto1_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 1", false);
        }

        private void boatGoto2_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 2", false);
        }

        private void boatGoto3_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 3", false);
        }

        private void boatGoto4_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 4", false);
        }

        private void boatGoto5_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 5", false);
        }

        private void boatGoto6_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 6", false);
        }

        private void boatGoto7_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 7", false);
        }

        private void boatGoto8_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 8", false);
        }

        private void boatGoto9_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Goto 9", false);
        }

        private void boatStart_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Start", false);
        }

        private void boatContinue_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Continue", false);
        }

        private void boatNav_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Nav", false);
        }

        private void boatWindowsBorder_CheckedChanged(object sender, EventArgs e)
        {
            boatBorder.Visible = boatWindowsBorder.Checked;
        }

        private void boatStop_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Stop", false);
        }

        private void boatClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}