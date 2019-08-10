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
    public partial class OverheadMessages : Form
    {
        private List<Core.OverheadMessages.OverheadMessage> NewOverheadEntries =
            new List<Core.OverheadMessages.OverheadMessage>();

        public OverheadMessages()
        {
            InitializeComponent();
        }

        private void OverheadMessages_Load(object sender, EventArgs e)
        {
            foreach (Core.OverheadMessages.OverheadMessage message in Core.OverheadMessages.OverheadMessageList)
            {
                ListViewItem item = new ListViewItem($"{message.SearchMessage}");
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message.MessageOverhead));

                int hueIdx = message.Hue;

                if (hueIdx > 0 && hueIdx < 3000)
                    item.SubItems[1].BackColor = Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
                else
                    item.SubItems[1].BackColor = SystemColors.Control;

                item.SubItems[1].ForeColor =
                    (item.SubItems[1].BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);

                item.UseItemStyleForSubItems = false;

                cliLocOverheadView.SafeAction(s => s.Items.Add(item));
            }

            overheadFormat.SafeAction(s => s.Text = Config.GetString("OverheadFormat"));

            if (Config.GetInt("OverheadStyle") == 0)
            {
                asciiStyle.SafeAction(s => s.Checked = true);
            }
            else
            {
                unicodeStyle.SafeAction(s => s.Checked = true);
            }
        }

        private void cliLocSearch_Click(object sender, EventArgs e)
        {
            cliLocSearchView.SafeAction(s => s.Items.Clear());

            if (string.IsNullOrEmpty(cliLocTextSearch.Text) || cliLocTextSearch.Text.Length < 4)
                return;

            foreach (StringEntry entry in Language.CliLoc.Entries)
            {
                if (entry.Text.IndexOf(cliLocTextSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ListViewItem item = new ListViewItem($"{entry.Number}");
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, $"{entry.Text}"));

                    cliLocSearchView.SafeAction(s => s.Items.Add(item));
                }
            }

            if (cliLocSearchView.Items.Count == 0)
            {
                if (MessageBox.Show(this,
                        $"Unable to find that message. Would you still like to setup an overhead message with '{cliLocTextSearch.Text}' anyway?",
                        "Overhead Messages",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    setOverheadMessage.PerformClick();
                }
            }
        }

        private void setOverheadMessage_Click(object sender, EventArgs e)
        {
            int hueIdx = Config.GetInt("SysColor");
            string newItemText = string.Empty;

            if (string.IsNullOrEmpty(cliLocTextSearch.Text))
                return;

            if (cliLocSearchView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = cliLocSearchView.SelectedItems[0];
                newItemText = selectedItem.SubItems[1].Text;
            }
            else
            {
                newItemText = cliLocTextSearch.Text;
            }

            ListViewItem item = new ListViewItem(newItemText);

            if (InputBox.Show(this,
                "Enter text to display overhead",
                newItemText))
            {
                string overheadMessage = InputBox.GetString();

                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, overheadMessage));

                if (hueIdx > 0 && hueIdx < 3000)
                    item.SubItems[1].BackColor = Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
                else
                    item.SubItems[1].BackColor = SystemColors.Control;

                item.SubItems[1].ForeColor =
                    (item.SubItems[1].BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);
                item.UseItemStyleForSubItems = false;

                cliLocOverheadView.SafeAction(s => s.Items.Add(item));

                NewOverheadEntries.Add(new Core.OverheadMessages.OverheadMessage
                {
                    SearchMessage = newItemText,
                    Hue = hueIdx,
                    MessageOverhead = overheadMessage
                });
            }
        }

        private void removeOverheadMessage_Click(object sender, EventArgs e)
        {
            if (cliLocOverheadView.SelectedItems.Count > 0)
            {
                cliLocOverheadView.SafeAction(s => s.Items.Remove(s.SelectedItems[0]));
            }
        }

        private void saveOverheadMessages_Click(object sender, EventArgs e)
        {
            //Core.OverheadMessages.ClearAll();
            List<Core.OverheadMessages.OverheadMessage> newOverheadMessageList =
                new List<Core.OverheadMessages.OverheadMessage>();


            // Keep it simple, reset to default if it isn't what we like
            if (string.IsNullOrEmpty(overheadFormat.Text) || !overheadFormat.Text.Contains("{msg}"))
            {
                overheadFormat.Text = @"[{msg}]";
            }

            Config.SetProperty("OverheadFormat", overheadFormat.Text);

            if (asciiStyle.Checked)
            {
                Config.SetProperty("OverheadStyle", 0);
            }
            else
            {
                Config.SetProperty("OverheadStyle", 1);
            }

            foreach (ListViewItem item in cliLocOverheadView.Items)
            {
                Core.OverheadMessages.OverheadMessage message = new Core.OverheadMessages.OverheadMessage
                {
                    SearchMessage = item.SubItems[0].Text,
                    MessageOverhead = item.SubItems[1].Text,
                    Hue = GetHueFromListView(item.SubItems[1].Text)
                };

                newOverheadMessageList.Add(message);
            }

            Core.OverheadMessages.OverheadMessageList =
                new List<Core.OverheadMessages.OverheadMessage>(newOverheadMessageList);

            Config.Save();

            this.SafeAction(s => s.Hide());
        }

        private void cancelOverheadMessages_Click(object sender, EventArgs e)
        {
            this.SafeAction(s => s.Hide());
        }

        private void setColorHue_Click(object sender, EventArgs e)
        {
            if (cliLocOverheadView.SelectedItems.Count > 0)
            {
                SetContainerLabelHue();
            }
        }

        private void SetContainerLabelHue()
        {
            ListViewItem selectedItem = cliLocOverheadView.Items[cliLocOverheadView.SelectedIndices[0]];

            HueEntry h = new HueEntry(GetHueFromListView(selectedItem.SubItems[1].Text));

            // TODO: BREAKING DRY!
            if (h.ShowDialog(this) == DialogResult.OK)
            {
                int hueIdx = h.Hue;

                if (hueIdx > 0 && hueIdx < 3000)
                    selectedItem.SubItems[1].BackColor = Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
                else
                    selectedItem.SubItems[1].BackColor = Color.White;

                selectedItem.SubItems[1].ForeColor = (selectedItem.SubItems[1].BackColor.GetBrightness() < 0.35
                    ? Color.White
                    : Color.Black);

                foreach (Core.OverheadMessages.OverheadMessage list in Core.OverheadMessages.OverheadMessageList)
                {
                    if (list.SearchMessage.Equals(selectedItem.Text))
                    {
                        list.Hue = hueIdx;
                        break;
                    }
                }

                foreach (Core.OverheadMessages.OverheadMessage list in NewOverheadEntries)
                {
                    if (list.SearchMessage.Equals(selectedItem.Text))
                    {
                        list.Hue = hueIdx;
                        break;
                    }
                }
            }
        }

        public int GetHueFromListView(string id)
        {
            int hue = 0;

            foreach (Core.OverheadMessages.OverheadMessage list in Core.OverheadMessages.OverheadMessageList)
            {
                if (list.MessageOverhead.Equals(id))
                {
                    return list.Hue;
                }
            }

            foreach (Core.OverheadMessages.OverheadMessage list in NewOverheadEntries)
            {
                if (list.MessageOverhead.Equals(id))
                {
                    return list.Hue;
                }
            }

            return hue;
        }

        private void editOverheadMessage_Click(object sender, EventArgs e)
        {
            if (cliLocOverheadView.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = cliLocOverheadView.SelectedItems[0];
            string oldMessage = selectedItem.SubItems[1].Text;

            //ListViewItem item = new ListViewItem(newItemText);

            if (InputBox.Show(this, "Enter Overhead Text", "Enter text to display overhead", oldMessage))
            {
                string newMessage = InputBox.GetString();

                if (string.IsNullOrEmpty(newMessage))
                    return;

                selectedItem.SubItems[1].Text = newMessage;

                foreach (Core.OverheadMessages.OverheadMessage list in Core.OverheadMessages.OverheadMessageList)
                {
                    if (list.MessageOverhead.Equals(oldMessage))
                    {
                        list.MessageOverhead = newMessage;
                        break;
                    }
                }
            }
        }

        private void OverheadMessages_Closing(object sender, CancelEventArgs e)
        {
            this.SafeAction(s =>
            {
                e.Cancel = true;
                s.Hide();
            });
        }
    }
}