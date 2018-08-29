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

        private List<BoatCommandKeywords> _boatCommandKw = new List<BoatCommandKeywords>();

        private class BoatCommandKeywords
        {
            public string command { get; set; }
            public ArrayList keywords { get; set; }
        }

        public BoatWindow()
        {
            InitializeComponent();
            this.MouseDown += BoatWindow_MouseDown;
            this.boatBorder.MouseDown += BoatWindow_MouseDown;

            LoadKeywords();
        }

        private void LoadKeywords()
        {
            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "stop",
                keywords = new ArrayList(5) {(ushort) 48, (byte) 54, (byte) 4, (byte) 241, (byte) 97}
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "forward left",
                keywords = new ArrayList(2) { (ushort)16, (byte)75 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "forward",
                keywords = new ArrayList(2) { (ushort)16, (byte)69 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "forward right",
                keywords = new ArrayList(2) { (ushort)16, (byte)76 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "right",
                keywords = new ArrayList(2) { (ushort)16, (byte)72 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "back right",
                keywords = new ArrayList(2) { (ushort)16, (byte)78 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "back",
                keywords = new ArrayList(2) { (ushort)16, (byte)70 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "back left",
                keywords = new ArrayList(2) { (ushort)16, (byte)77 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "left",
                keywords = new ArrayList(2) { (ushort)16, (byte)71 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow forward left",
                keywords = new ArrayList(2) { (ushort)16, (byte)84 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow forward",
                keywords = new ArrayList(2) { (ushort)16, (byte)82 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow forward right",
                keywords = new ArrayList(2) { (ushort)16, (byte)85 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow right",
                keywords = new ArrayList(2) { (ushort)16, (byte)81 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow back right",
                keywords = new ArrayList(2) { (ushort)16, (byte)86 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow back",
                keywords = new ArrayList(2) { (ushort)16, (byte)83 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "slow back left",
                keywords = new ArrayList(2) { (ushort)16, (byte)87 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "forward left one",
                keywords = new ArrayList(2) { (ushort)16, (byte)92 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "forward one",
                keywords = new ArrayList(2) { (ushort)16, (byte)90 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "forward right one",
                keywords = new ArrayList(2) { (ushort)16, (byte)93 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "right one",
                keywords = new ArrayList(2) { (ushort)16, (byte)89 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "back right one",
                keywords = new ArrayList(2) { (ushort)16, (byte)94 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "back one",
                keywords = new ArrayList(2) { (ushort)16, (byte)91 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "back left one",
                keywords = new ArrayList(2) { (ushort)16, (byte)95 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "left one",
                keywords = new ArrayList(2) { (ushort)16, (byte)88 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "turn right",
                keywords = new ArrayList(2) { (ushort)16, (byte)101 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "turn left",
                keywords = new ArrayList(2) { (ushort)16, (byte)102 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "raise anchor",
                keywords = new ArrayList(4) { (ushort)32, (byte)44, (byte)6, (byte)176 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "drop anchor",
                keywords = new ArrayList(4) { (ushort)32, (byte)44, (byte)6, (byte)160 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "turn around",
                keywords = new ArrayList(2) { (ushort)16, (byte)103 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "doracron",
                keywords = new ArrayList(0)
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "sueacron",
                keywords = new ArrayList(0)
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 1",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 2",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 3",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 4",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 5",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 6",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 7",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 8",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "goto 9",
                keywords = new ArrayList(2) { (ushort)16, (byte)99 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "start",
                keywords = new ArrayList(2) { (ushort)16, (byte)97 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "continue",
                keywords = new ArrayList(2) { (ushort)16, (byte)98 }
            });

            _boatCommandKw.Add(new BoatCommandKeywords
            {
                command = "nav",
                keywords = new ArrayList(2) { (ushort)16, (byte)96 }
            });
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

            ArrayList kw = new ArrayList();

            foreach (BoatCommandKeywords boatCommandKw in _boatCommandKw)
            {
                if (boatCommandKw.command.Equals(boatCommand.ToLower()))
                {
                    kw = boatCommandKw.keywords;
                    break;
                }
            }

            ClientCommunication.SendToServer(new ClientUniMessage(MessageType.Regular, World.Player.SpeechHue, Config.GetInt("SysColor"), "ENU",kw, boatCommand));
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

        private void boatStop_Click(object sender, EventArgs e)
        {
            SendBoatCommand("Stop", false);
        }

        private void boatClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}