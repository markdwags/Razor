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
        public OverheadMessages()
        {
            InitializeComponent();
        }

        private void OverheadMessages_Load(object sender, EventArgs e)
        {
            /*foreach (StringEntry entry in Language.CliLoc.Entries)
            {
                //var cliItem = new ListViewItem(new[] { $"{entry.Number}", $"{entry.Text}", "" });

                ListViewItem item = new ListViewItem($"{entry.Number}");
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, $"{entry.Text}"));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "None"));
                cliLocSearchView.Items.Add(item);
            }*/

            foreach (Core.OverheadMessages.OverheadMessage message in Core.OverheadMessages.OverheadMessageList)
            {
                ListViewItem item = new ListViewItem($"{message.SearchMessage}");
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message.MessageOverhead));
                cliLocOverheadView.Items.Add(item);
            }

            overheadFormat.Text = Config.GetString("OverheadFormat");
        }

        private void cliLocSearch_Click(object sender, EventArgs e)
        {
            cliLocSearchView.Items.Clear();

            if (string.IsNullOrEmpty(cliLocTextSearch.Text) || cliLocTextSearch.Text.Length < 4)
                return;

            foreach (StringEntry entry in Language.CliLoc.Entries)
            {
                if (entry.Text.IndexOf(cliLocTextSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ListViewItem item = new ListViewItem($"{entry.Number}");
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, $"{entry.Text}"));
                    cliLocSearchView.Items.Add(item);
                }
            }
        }

        private void setOverheadMessage_Click(object sender, EventArgs e)
        {
            if (cliLocSearchView.SelectedItems.Count > 0)
            {
                //if (InputBox.Show(this, Language.GetString(LocString.NewMacro), Language.GetString(LocString.EnterAName)))
                if (InputBox.Show(this, "Enter Overhead Text", "Enter text to display overhead"))
                {
                    string overheadMessage = InputBox.GetString();

                    ListViewItem selectedItem = cliLocSearchView.SelectedItems[0];

                    ListViewItem item = new ListViewItem($"{selectedItem.SubItems[1].Text}");
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, overheadMessage));
                    cliLocOverheadView.Items.Add(item);
                }
            }
            else
            {
                if (InputBox.Show(this, "Enter Overhead Text", "Enter text to display overhead"))
                {
                    string overheadMessage = InputBox.GetString();

                    ListViewItem item = new ListViewItem($"{cliLocTextSearch.Text}");
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, overheadMessage));
                    cliLocOverheadView.Items.Add(item);
                }
            }
        }

        private void removeOverheadMessage_Click(object sender, EventArgs e)
        {
            if (cliLocOverheadView.SelectedItems.Count > 0)
            {
                cliLocOverheadView.Items.Remove(cliLocOverheadView.SelectedItems[0]);
            }
        }

        private void saveOverheadMessages_Click(object sender, EventArgs e)
        {
            Core.OverheadMessages.ClearAll();

            // Keep it simple, reset to default if it isn't what we like
            if (string.IsNullOrEmpty(overheadFormat.Text) || !overheadFormat.Text.Contains("{msg}"))
            {
                overheadFormat.Text = @"[{msg}]";
            }

            Config.SetProperty("OverheadFormat", overheadFormat.Text);

            foreach (ListViewItem item in cliLocOverheadView.Items)
            {  
                Core.OverheadMessages.OverheadMessage message = new Core.OverheadMessages.OverheadMessage
                {
                    SearchMessage = item.SubItems[0].Text,
                    MessageOverhead = item.SubItems[1].Text
                };

                Core.OverheadMessages.OverheadMessageList.Add(message);
            }

            Config.Save();

            /*// Apply the format to the message after its been saved
            foreach (Core.OverheadMessages.OverheadMessage message in Core.OverheadMessages.OverheadMessageList)
            {
                message.MessageOverhead = overheadFormat.Text.Replace("{msg}", message.MessageOverhead);
            }*/

            Close();
        }

        private void cancelOverheadMessages_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
