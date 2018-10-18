using Assistant.Macros;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace Assistant.UI
{
    public partial class ContainerLabels : Form
    {
        public ContainerLabels()
        {
            InitializeComponent();
        }

        private void ContainerLabels_Load(object sender, EventArgs e)
        {
            foreach (Core.ContainerLabels.ContainerLabel list in Core.ContainerLabels.ContainerLabelList)
            {
                ListViewItem item = new ListViewItem($"{list.Id}");
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, list.Type));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, list.Label));
                containerView.Items.Add(item);
            }

            containerLabelFormat.Text = Config.GetString("ContainerLabelFormat");
            InitPreviewHue(lblContainerHue, "ContainerLabelColor");
        }

        private void InitPreviewHue(Control ctrl, string cfg)
        {
            int hueIdx = Config.GetInt(cfg);
            if (hueIdx > 0 && hueIdx < 3000)
                ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
            else
                ctrl.BackColor = SystemColors.Control;
            ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);
        }

        private void saveContainerLabels_Click(object sender, EventArgs e)
        {
            Core.ContainerLabels.ClearAll();

            // Keep it simple, reset to default if it isn't what we like
            if (string.IsNullOrEmpty(containerLabelFormat.Text) || !containerLabelFormat.Text.Contains("{label}"))
            {
                containerLabelFormat.Text = @"[{label}]";
            }

            Config.SetProperty("ContainerLabelFormat", containerLabelFormat.Text);

            foreach (ListViewItem item in containerView.Items)
            {
                Core.ContainerLabels.ContainerLabel label = new Core.ContainerLabels.ContainerLabel
                {
                    Id = item.SubItems[0].Text,
                    Type = item.SubItems[1].Text,
                    Label = item.SubItems[2].Text,
                };

                Core.ContainerLabels.ContainerLabelList.Add(label);
            }

            Config.Save();

            Close();
        }

        private void removeOverheadMessage_Click(object sender, EventArgs e)
        {
            if (containerView.SelectedItems.Count > 0)
            {
                containerView.Items.Remove(containerView.SelectedItems[0]);
            }
        }

        private void cancelOverheadMessages_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addContainLabel_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            Targeting.OneTimeTarget(OnContainerLabelAddTarget);
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void OnContainerLabelAddTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte)(ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            if (t != null && t.Serial.IsItem)
            {
                Item item = World.FindItem(t.Serial);

                if (!item.IsContainer)
                {
                    // must be a container
                    World.Player.SendMessage(MsgLevel.Force, "You must select a container");
                }
                else
                {
                    // add it

                    if (InputBox.Show(this, Language.GetString(LocString.SetContainerLabel), Language.GetString(LocString.EnterAName)))
                    {
                        string name = InputBox.GetString();

                        ListViewItem lvItem = new ListViewItem($"{t.Serial.Value}");
                        lvItem.SubItems.Add(new ListViewItem.ListViewSubItem(lvItem, item.Name));
                        lvItem.SubItems.Add(new ListViewItem.ListViewSubItem(lvItem, name));
                        containerView.Items.Add(lvItem);

                        World.Player.SendMessage(MsgLevel.Force, $"Container {item} labeled as '{name}'");
                    }

                    Engine.MainWindow.ShowMe();
                }
            }
            else if (t != null && t.Serial.IsMobile)
            {
                World.Player.SendMessage(MsgLevel.Force, "You shouldn't label other people");
            }
        }

        private void setExHue_Click(object sender, EventArgs e)
        {
            SetHue(lblContainerHue, "ContainerLabelColor");
        }

        private bool SetHue(Control ctrl, string cfg)
        {
            HueEntry h = new HueEntry(Config.GetInt(cfg));

            if (h.ShowDialog(this) == DialogResult.OK)
            {
                int hueIdx = h.Hue;
                Config.SetProperty(cfg, hueIdx);
                if (hueIdx > 0 && hueIdx < 3000)
                    ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
                else
                    ctrl.BackColor = Color.White;
                ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
