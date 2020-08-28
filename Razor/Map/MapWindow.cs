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
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Assistant.UI;

namespace Assistant.MapUO
{
    /// <summary>
    /// Summary description for MapWindow.
    /// </summary>
    public class MapWindow : System.Windows.Forms.Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private Assistant.MapUO.UOMapControl Map;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public MapWindow()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.Opening += ContextMenu_Popup;
            this.Location = new Point(Config.GetInt("MapX"), Config.GetInt("MapY"));
            this.ClientSize = new Size(Config.GetInt("MapW"), Config.GetInt("MapH"));

            if (this.Location.X < -10 || this.Location.Y < -10)
                this.Location = Point.Empty;

            if (this.Width < 50)
                this.Width = 50;
            if (this.Height < 50)
                this.Height = 50;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            this.Map.FullUpdate();
            Client.Instance.SetMapWndHandle(this);
        }

        public class MapMenuItem : ToolStripMenuItem
        {
            public MapMenuItem(System.String text, System.EventHandler onClick) : base(text, null, onClick)
            {
                Tag = null;
            }
        }

        void ContextMenu_Popup(object sender, EventArgs e)
        {
            ContextMenuStrip cm = this.ContextMenuStrip;
            cm.Items.Clear();
            if (World.Player != null && PacketHandlers.Party.Count > 0)
            {
                MapMenuItem mi = new MapMenuItem("You", new EventHandler(FocusChange));
                mi.Tag = World.Player.Serial;
                cm.Items.Add(mi);
                foreach (Serial s in PacketHandlers.Party)
                {
                    Mobile m = World.FindMobile(s);
                    if (m.Name != null)
                    {
                        mi = new MapMenuItem(m.Name, new EventHandler(FocusChange));
                        mi.Tag = s;
                        if (this.Map.FocusMobile == m)
                            mi.Checked = true;
                        cm.Items.Add(mi);
                    }
                }
            }

            this.ContextMenuStrip = cm;
        }


        private void FocusChange(object sender, System.EventArgs e)
        {
            if (sender != null)
            {
                MapMenuItem mItem = sender as MapMenuItem;

                if (mItem != null)
                {
                    Serial s = (Serial) mItem.Tag;
                    Mobile m = World.FindMobile(s);
                    this.Map.FocusMobile = m;
                    this.Map.FullUpdate();
                }
            }
        }

        public static void Initialize()
        {
            new ReqPartyLocTimer().Start();

            HotKey.Add(HKCategory.Misc, LocString.ToggleMap, new HotKeyCallback(ToggleMap));
        }

        public static void ToggleMap()
        {
            if (World.Player != null && Engine.MainWindow != null)
            {
                if (Engine.MainWindow.MapWindow == null)
                {
                    Engine.MainWindow.SafeAction(s =>
                    {
                        s.MapWindow = new Assistant.MapUO.MapWindow();
                        s.MapWindow.Show();
                        s.MapWindow.BringToFront();
                    });
                }
                else
                {
                    if (Engine.MainWindow.MapWindow.Visible)
                    {
                        Engine.MainWindow.SafeAction(s =>
                        {
                            s.MapWindow.Hide();
                            s.BringToFront();
                        });
                        Platform.BringToFront(Client.Instance.GetWindowHandle());
                    }
                    else
                    {
                        Engine.MainWindow.MapWindow.Show();
                        Engine.MainWindow.MapWindow.BringToFront();
                        Engine.MainWindow.MapWindow.TopMost = true;
                        Client.Instance.SetMapWndHandle(Engine.MainWindow.MapWindow);
                    }
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MapWindow));
            this.Map = new Assistant.MapUO.UOMapControl();
            this.SuspendLayout();
            //
            // Map
            //
            this.Map.Active = true;
            this.Map.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Map.FocusMobile = null;
            this.Map.Location = new System.Drawing.Point(0, 0);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(296, 272);
            this.Map.TabIndex = 0;
            this.Map.TabStop = false;
            this.Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            //
            // MapWindow
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.Map);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MapWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "UO Positioning System";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.MapWindow_Resize);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapWindow_MouseDown);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MapWindow_Closing);
            this.Move += new System.EventHandler(this.MapWindow_Move);
            this.Deactivate += new System.EventHandler(this.MapWindow_Deactivate);
            this.ResumeLayout(false);
        }

        #endregion

        public void CheckLocalUpdate(Mobile mob)
        {
            if (mob.InParty)
                this.Map.FullUpdate();
        }

        private static Font m_RegFont = new Font("Courier New", 8);
        /*private   int ButtonRows;
        protected override void OnPaint(PaintEventArgs e)
        {
        base.OnPaint(e);
        if ( PacketHandlers.Party.Count > 0 )
        {
        //75x15
        int xcount = 0;
        int ycount = 0;
        Point org = new Point(0, (ButtonRows * 15));
        if (this.FormBorderStyle == FormBorderStyle.None)
        {
        org = new Point(0,  (ButtonRows * 15) + 32);
        }

        foreach ( Serial s in PacketHandlers.Party )
        {
        Mobile mob = World.FindMobile( s );
        if ( mob == null )
             continue;

        if (((75 * (xcount+1)) - this.Width) > 0)
        {
             xcount = 0;
             ycount++;
        }
        string name = mob.Name;
        if ( name != null && name.Length > 8)
        {
             name = name.Substring(0, 8);
             name += "...";
        }
        else if ( name == null || name.Length < 1 )
        {
             name = "(Not Seen)";
        }

        Point drawPoint = new Point(org.X + (75 * xcount), org.Y + (15*ycount));
        mob.ButtonPoint = new Point2D( drawPoint.X, drawPoint.Y );
        e.Graphics.FillRectangle( Brushes.Black, drawPoint.X, drawPoint.Y, 75, 15 );
        e.Graphics.DrawRectangle(Pens.Gray, drawPoint.X, drawPoint.Y, 75, 15 );
        e.Graphics.DrawString(name, m_RegFont, Brushes.White, drawPoint);
        xcount++;
        }
        if(ycount > 0)
        ButtonRows = ycount;
        }

        }*/

        private class ReqPartyLocTimer : Timer
        {
            public ReqPartyLocTimer() : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
            }

            protected override void OnTick()
            {
                // never send this packet to encrypted servers (could lead to OSI detecting razor)
                if (Client.Instance.ServerEncrypted)
                {
                    Stop();
                    return;
                }

                if (Engine.MainWindow == null || Engine.MainWindow.MapWindow == null ||
                    !Engine.MainWindow.MapWindow.Visible)
                    return; // don't bother when the map window isnt visible

                if (World.Player != null && PacketHandlers.Party.Count > 0)
                {
                    if (PacketHandlers.SpecialPartySent > PacketHandlers.SpecialPartyReceived)
                    {
                        // If we sent more than we received then the server stopped responding
                        // in that case, wait a long while before trying again
                        PacketHandlers.SpecialPartySent = PacketHandlers.SpecialPartyReceived = 0;
                        this.Interval = TimeSpan.FromSeconds(5.0);
                        return;
                    }
                    else
                    {
                        this.Interval = TimeSpan.FromSeconds(1.0);
                    }

                    bool send = false;
                    foreach (Serial s in PacketHandlers.Party)
                    {
                        Mobile m = World.FindMobile(s);

                        if (m == World.Player)
                            continue;

                        if (m == null || Utility.Distance(World.Player.Position, m.Position) > World.Player.VisRange ||
                            !m.Visible)
                        {
                            send = true;
                            break;
                        }
                    }

                    if (send)
                    {
                        PacketHandlers.SpecialPartySent++;
                        Client.Instance.SendToServer(new QueryPartyLocs());
                    }
                }
                else
                {
                    this.Interval = TimeSpan.FromSeconds(1.0);
                }
            }
        }

        private void RequestPartyLocations()
        {
            if (World.Player != null && PacketHandlers.Party.Count > 0)
                Client.Instance.SendToServer(new QueryPartyLocs());
        }

        public void UpdateMap()
        {
            Client.Instance.SetMapWndHandle(this);
            this.Map.UpdateMap();
        }

        private void MapWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Engine.Running)
            {
                Engine.MainWindow.MapWindow.SafeAction(s =>
                {
                    e.Cancel = true;
                    s.Hide();
                });
            }
        }

        public void PlayerMoved()
        {
            if (this.Visible && this.Map != null)
                this.Map.FullUpdate();
        }

        private void MapWindow_Resize(object sender, System.EventArgs e)
        {
            this.Map.Height = this.Height;
            this.Map.Width = this.Width;

            if (this.Width < 50)
                this.Width = 50;
            if (this.Height < 50)
                this.Height = 50;

            this.Refresh();

            Config.SetProperty("MapX", this.Location.X);
            Config.SetProperty("MapY", this.Location.Y);
            Config.SetProperty("MapW", this.ClientSize.Width);
            Config.SetProperty("MapH", this.ClientSize.Height);
        }

        private void MapWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                else
                    this.FormBorderStyle = FormBorderStyle.None;
            }

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr) HT_CAPTION, IntPtr.Zero);
                /*foreach ( Serial s in PacketHandlers.Party )
                {
                Mobile m = World.FindMobile( s );
                if ( m == null )
                     continue;
                Rectangle rec = new Rectangle( m.ButtonPoint.X, m.ButtonPoint.Y, 75, 15 );
                if ( rec.Contains( e.X, e.Y ) )
                {
                     this.Map.FocusMobile = m;
                     this.Map.Refresh();
                }
                }*/
            }

            this.Map.MapClick(e);
        }

        private void Map_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MapWindow_MouseDown(sender, e);
        }

        private void MapWindow_Move(object sender, System.EventArgs e)
        {
            Config.SetProperty("MapX", this.Location.X);
            Config.SetProperty("MapY", this.Location.Y);
            Config.SetProperty("MapW", this.ClientSize.Width);
            Config.SetProperty("MapH", this.ClientSize.Height);
        }

        private void MapWindow_Deactivate(object sender, System.EventArgs e)
        {
            if (this.TopMost)
                this.TopMost = false;
        }
    }
}