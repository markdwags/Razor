#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Assistant.Scripts;
using FastColoredTextBoxNS;

namespace Assistant.UI
{
    public partial class RazorScriptEditor : Form
    {
        private bool _savedCurrentScript = true;
        private RazorScript _currentScript;

        public RazorScriptEditor()
        {
            InitializeComponent();
        }

        private void RazorScriptEditor_Load(object sender, EventArgs e)
        {
            InitScriptEditorPopout();
        }

        private void InitScriptEditorPopout()
        {
            ScriptManager.SetEditor(scriptEditor, true);

            scriptDocMap.Target = scriptEditor;

            if (ScriptManager.SelectedScript != null)
            {
                scriptEditor.SafeAction(s =>
                {
                    ScriptManager.SetEditorText(ScriptManager.SelectedScript);
                    UpdateScriptWindowTitle();
                });
            }
        }

        private void RazorScriptEditor_VisibleChanged(object sender, EventArgs e)
        {
            this.SafeAction(s =>
            {
                if (s.Visible)
                {
                    InitScriptEditorPopout();
                }
            });
        }

        private void UpdateScriptWindowTitle()
        {
            if (ScriptManager.SelectedScript != null)
            {
                Text = _savedCurrentScript ? $"[{ScriptManager.SelectedScript.Name}] - Razor Script Editor" : $"[*{ScriptManager.SelectedScript.Name}] - Razor Script Editor";
            }
        }

        private void scriptPlay_Click(object sender, EventArgs e)
        {
            PlayScript();
        }

        private void PlayScript()
        {
            if (ScriptManager.Running)
            {
                ScriptManager.StopScript();
                return;
            }

            if (string.IsNullOrEmpty(scriptEditor.Text))
            {
                return;
            }

            if (Config.GetBool("AutoSaveScriptPlay"))
            {
                SaveScript();
            }

            // We want to play the contents of the script editor
            ScriptManager.PlayScript(scriptEditor.Lines.ToArray());
        }

        private void SaveScript()
        {
            if (ScriptManager.SelectedScript != null)
            {
                File.WriteAllText(ScriptManager.SelectedScript.Path, scriptEditor.Text);
                ScriptManager.SelectedScript.Lines = File.ReadAllLines(ScriptManager.SelectedScript.Path);

                _savedCurrentScript = true;

                UpdateScriptWindowTitle();
            }
        }

        private void scriptRecord_Click(object sender, EventArgs e)
        {
            if (World.Player == null)
                return;

            if (ScriptManager.Recording) // stop recording
            {
                scriptRecord.Text = "Record";
                ScriptManager.Recording = false;
                scriptEditor.Enabled = true;
                scriptPlay.Enabled = true;
            }
            else //start recording
            {
                scriptRecord.Text = "Stop";
                ScriptManager.Recording = true;
                scriptEditor.Enabled = false;
                scriptPlay.Enabled = false;
            }
        }
        
        private void scriptSave_Click(object sender, EventArgs e)
        {
            SaveScript();
        }

        private void scriptEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.S))
            {
                BeginInvoke(new Action(SaveScript));
            } 
            else if (e.KeyData == (Keys.F5))
            {
                BeginInvoke(new Action(PlayScript));
            }

            if (_savedCurrentScript)
            {
                _savedCurrentScript = false;

                UpdateScriptWindowTitle();
            }
        }

        private void alwaysOnTop_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;

            alwaysOnTop.Checked = TopMost;
        }

        private void scriptHelp_Click(object sender, EventArgs e)
        {
            Utility.LaunchBrowser("http://www.razorce.com/guide/");
        }

        private void scriptEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                ContextMenuStrip menu = new ContextMenuStrip();

                if (!string.IsNullOrEmpty(scriptEditor.SelectedText))
                {
                    menu.Items.Add("Comment", null, OnScriptComment);
                    menu.Items.Add("Uncomment", null, OnScriptUncomment);

                    if (!string.IsNullOrEmpty(scriptEditor.SelectedText) && !ScriptManager.Running && !ScriptManager.Recording && World.Player != null)
                    {
                        menu.Items.Add("-");
                        menu.Items.Add("Play selected script code", null, OnScriptPlaySelected);

                        int space = scriptEditor.SelectedText.IndexOf(" ", StringComparison.Ordinal);

                        if (space > -1)
                        {
                            string command = scriptEditor.SelectedText.Substring(0, space);

                            if (command.Equals("dclick"))
                            {
                                menu.Items.Add("-");
                                menu.Items.Add("Convert to 'dclicktype' by gfxid", null, OnScriptDclickTypeId);
                                menu.Items.Add("Convert to 'dclicktype' by name", null, OnScriptDclickTypeName);
                            }
                        }
                    }

                    menu.Items.Add("-");
                }

                menu.Show(scriptEditor, new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                    return;
            }
        }

        private void OnScriptComment(object sender, System.EventArgs e)
        {
            string[] lines = scriptEditor.SelectedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            scriptEditor.SelectedText = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                scriptEditor.SelectedText += "#" + lines[i];
                if (i < lines.Count() - 1)
                    scriptEditor.SelectedText += "\r\n";
            }
        }

        private void OnScriptUncomment(object sender, System.EventArgs e)
        {
            string[] lines = scriptEditor.SelectedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            scriptEditor.SelectedText = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                scriptEditor.SelectedText += lines[i].TrimStart('#');
                if (i < lines.Count() - 1)
                    scriptEditor.SelectedText += "\r\n";
            }
        }

        private void OnScriptDclickTypeId(object sender, System.EventArgs e)
        {
            Serial itemId = Serial.Zero;

            try
            {
                itemId = Serial.Parse(scriptEditor.SelectedText.Split(' ')[1]);
            }
            catch
            {
                return;
            }

            Item item = World.FindItem(itemId);

            if (item == null)
                return;

            scriptEditor.SelectedText = "";
            scriptEditor.SelectedText = $"dclicktype '{item.ItemID.Value}'";
        }

        private void OnScriptDclickTypeName(object sender, System.EventArgs e)
        {
            Serial gfxid = Serial.Zero;

            try
            {
                gfxid = Serial.Parse(scriptEditor.SelectedText.Split(' ')[1]);
            }
            catch
            {
                return;
            }


            Item item = World.FindItem(gfxid);

            if (item == null)
                return;

            scriptEditor.SelectedText = "";
            scriptEditor.SelectedText = $"dclicktype '{item.ItemID.ItemData.Name}'";
        }

        private void OnScriptPlaySelected(object sender, System.EventArgs e)
        {
            if (ScriptManager.Running)
            {
                ScriptManager.StopScript();
                return;
            }

            if (string.IsNullOrEmpty(scriptEditor.SelectedText))
                return;

            string[] lines = scriptEditor.SelectedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            ScriptManager.PlayScript(lines);
        }

        private void scriptEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentScript != ScriptManager.SelectedScript)
            {
                _currentScript = ScriptManager.SelectedScript;

                _savedCurrentScript = true;

                UpdateScriptWindowTitle();
            }
        }

        public void LockScriptUI(bool enabled)
        {
            Engine.RazorScriptEditorWindow.SafeAction(s =>
            {
                scriptEditor.Enabled = !enabled;
                scriptRecord.Enabled = !enabled;
                scriptSave.Enabled = !enabled;

                scriptPlay.Text = !enabled ? "Play" : "Stop";

                scriptRecord.Enabled = !enabled;
                scriptInfo.Enabled = !enabled;
            });
        }

        private void scriptInfo_Click(object sender, EventArgs e)
        {
            ScriptManager.GetGumpInfo(null);
        }
    }
}
