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

            ClientCommunication.SendToServer(new ClientUniMessage(MessageType.Regular, 68, 3, "ENU",
                new ArrayList {(ushort) 16, (byte) 59}, boatCommand));
        }

        private void boatNorth_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Forward");
        }

        private void boatSouth_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Back");
        }

        private void boatSoutheast_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Back Right");
        }

        private void boatSouthwest_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Back Left");
        }

        private void boatWest_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Left");
        }

        private void boatNorthwest_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Forward Left");
        }

        private void boatNortheast_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Forward Right");
        }

        private void boatEast_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Right");
        }

        private void boatTurnRight_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Turn Right", false);
        }

        private void boatTurnLeft_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Turn Left", false);
        }

        private void boatTurnAround_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Turn Around", false);
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
    }
}