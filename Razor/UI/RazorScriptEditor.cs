using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistant.Scripts;

namespace Assistant.UI
{
    public partial class RazorScriptEditor : Form
    {
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
            ScriptManager.SetEditor(scriptEditor);

            scriptDocMap.Target = scriptEditor;

            if (ScriptManager.SelectedScript != null)
            {
                scriptEditor.SafeAction(s =>
                {
                    ScriptManager.SetEditorText(ScriptManager.SelectedScript);
                    UpdateScriptWindowTitle(true);
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

        private void UpdateScriptWindowTitle(bool saved)
        {
            if (ScriptManager.SelectedScript != null)
            {
                Text = saved ? $"[{ScriptManager.SelectedScript.Name}] - Razor Script Editor" : $"[*{ScriptManager.SelectedScript.Name}] - Razor Script Editor";
            }
        }

        private void scriptPlay_Click(object sender, EventArgs e)
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
            if (ScriptManager.SelectedScript == null)
            {
                string filePath = $"{ScriptManager.ScriptPath}\\auto-{Guid.NewGuid().ToString().Substring(0, 4)}.razor";

                File.WriteAllText(filePath, scriptEditor.Text);

                RazorScript script = new RazorScript
                {
                    Lines = File.ReadAllLines(filePath),
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    Path = filePath
                };

                TreeNode node = ScriptManager.GetScriptDirNode();

                ScriptManager.RedrawScripts();

                TreeNode newNode = new TreeNode(script.Name)
                {
                    Tag = script
                };

                ScriptManager.AddScriptNode(newNode);
            }
            else
            {
                File.WriteAllText(ScriptManager.SelectedScript.Path, scriptEditor.Text);
                ScriptManager.SelectedScript.Lines = File.ReadAllLines(ScriptManager.SelectedScript.Path);
            }

            UpdateScriptWindowTitle(true);
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

        private static readonly char[] _invalidNameChars = { '/', '\\', ';', '?', ':', '*' };

        private void scriptNew_Click(object sender, EventArgs e)
        {
            if (InputBox.Show(this, "New Razor Script", "Enter the name of the script"))
            {
                string name = InputBox.GetString();
                if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    name.IndexOfAny(_invalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TreeNode node = ScriptManager.GetScriptDirNode();

                string path = (node == null || !(node.Tag is string))
                    ? Config.GetUserDirectory("Scripts")
                    : (string)node.Tag;
                path = Path.Combine(path, $"{name}.razor");

                if (File.Exists(path))
                {
                    MessageBox.Show(this, Language.GetString(LocString.MacroExists),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                File.CreateText(path).Close();

                RazorScript script = ScriptManager.AddScript(path);

                TreeNode newNode = new TreeNode(script.Name)
                {
                    Tag = script
                };

                ScriptManager.AddScriptNode(newNode);
            }
        }

        private void scriptSave_Click(object sender, EventArgs e)
        {
            SaveScript();
        }
    }
}
