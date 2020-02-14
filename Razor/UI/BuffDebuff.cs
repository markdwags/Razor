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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistant.UI
{
    public partial class BuffDebuff : Form
    {
        public BuffDebuff()
        {
            InitializeComponent();
        }

        private void SetBuffHue_Click(object sender, EventArgs e)
        {
            lblBuffHue.SafeAction(s => { SetHue(s, "BuffHue"); });
        }

        private void SetDebuffHue_Click(object sender, EventArgs e)
        {
            lblDebuffHue.SafeAction(s => { SetHue(s, "DebuffHue"); });
        }

        private void SetHue(Control ctrl, string cfg)
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
            }
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

        private void BuffDebuffFormat_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(buffDebuffFormat.Text))
            {
                Config.SetProperty("BuffDebuffFormat", "[{action}{name} ({duration}s)]");
            }
            else
            {
                Config.SetProperty("BuffDebuffFormat", buffDebuffFormat.Text);
            }
        }

        private void BuffDebuffSeconds_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("BuffDebuffSeconds", Utility.ToInt32(buffDebuffSeconds.Text.Trim(), 20));

            if (Config.GetInt("BuffDebuffSeconds") < 1)
                Config.SetProperty("BuffDebuffSeconds", 20);
        }

        private void DisplayBuffDebuffEvery_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("BuffDebuffEveryXSeconds", displayBuffDebuffEvery.Checked);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void AddFilter_Click(object sender, EventArgs e)
        {
            if (InputBox.Show(this, "Filter Buff/Debuff",
                "Enter part or the whole name of the buff to filter it from showing overhead"))
            {
                string name = InputBox.GetString();

                if (!string.IsNullOrEmpty(name))
                {
                    buffDebuffFilters.Items.Add(name);
                }
            }

            SaveFilter();
        }

        private void BuffDebuff_Load(object sender, EventArgs e)
        {
            buffDebuffFilters.Items.Clear();

            foreach (string filter in Config.GetString("BuffDebuffFilter").Split(','))
            {
                if (string.IsNullOrEmpty(filter))
                    continue;

                buffDebuffFilters.Items.Add(filter);
            }

            buffDebuffFormat.SafeAction(s => s.Text = Config.GetString("BuffDebuffFormat"));
            buffDebuffSeconds.SafeAction(s => s.Text = Convert.ToString(Config.GetInt("BuffDebuffSeconds")));
            displayBuffDebuffEvery.SafeAction(s => s.Checked = Config.GetBool("BuffDebuffEveryXSeconds"));

            lblBuffHue.SafeAction(s => { InitPreviewHue(s, "BuffHue"); });
            lblDebuffHue.SafeAction(s => { InitPreviewHue(s, "DebuffHue"); });
        }

        private void RemoveFilter_Click(object sender, EventArgs e)
        {
            if (buffDebuffFilters.SelectedIndex < 0)
                return;

            buffDebuffFilters.Items.RemoveAt(buffDebuffFilters.SelectedIndex);

            SaveFilter();
        }

        private void BuffFormatKey_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You can insert these variables into the buff/debuff format box.");
            sb.AppendLine(string.Empty);
            sb.AppendLine("{name} - The name of the buff/debuff");
            sb.AppendLine("{action} - + or - depending if you gain or lose the buff/debuff");
            sb.AppendLine("{duration} - Time in seconds left for the buff/debuff");

            MessageBox.Show(this, sb.ToString(), "Buff/Debuff Format", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void SaveFilter()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in buffDebuffFilters.Items)
            {
                sb.Append($"{item},");
            }

            Config.SetProperty("BuffDebuffFilter", sb.ToString());
        }

        private void OkClose_Click(object sender, EventArgs e)
        {
            this.SafeAction(s => s.Hide());
        }

        private void BuffDebuff_Closing(object sender, CancelEventArgs e)
        {
            this.SafeAction(s =>
            {
                e.Cancel = true;
                s.Hide();
            });
        }
    }
}