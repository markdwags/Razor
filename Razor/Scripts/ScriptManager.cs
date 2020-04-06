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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Assistant.Macros;
using Assistant.Scripts.Engine;
using Assistant.UI;
using FastColoredTextBoxNS;

namespace Assistant.Scripts
{
    public static class ScriptManager
    {
        public static bool Recording { get; set; }

        public static bool Running => ScriptRunning;

        private static bool ScriptRunning { get; set; }

        public static DateTime LastWalk { get; set; }

        public static bool SetLastTargetActive { get; set; }

        public static bool SetVariableActive { get; set; }

        public static string ScriptPath => Config.GetUserDirectory("Scripts");

        private static FastColoredTextBox ScriptEditor { get; set; }

        private static ListBox ScriptList { get; set; }

        private static Script _queuedScript;

        private class ScriptTimer : Timer
        {
            // Only run scripts once every 25ms to avoid spamming.
            public ScriptTimer() : base(TimeSpan.FromMilliseconds(25), TimeSpan.FromMilliseconds(25))
            {
            }

            protected override void OnTick()
            {
                try
                {
                    if (!Client.Instance.ClientRunning)
                    {
                        if (ScriptRunning)
                        {
                            ScriptRunning = false;
                            Interpreter.StopScript();
                        }
                        return;
                    }

                    bool running;

                    if (_queuedScript != null)
                    {
                        // Starting a new script. This relies on the atomicity for references in CLR
                        var script = _queuedScript;

                        running = Interpreter.StartScript(script);

                        _queuedScript = null;
                    }
                    else
                    {
                        running = Interpreter.ExecuteScript();
                    }


                    if (running)
                    {
                        if (ScriptManager.Running == false)
                        {
                            World.Player?.SendMessage(LocString.ScriptPlaying);
                            Assistant.Engine.MainWindow.LockScriptUI(true);
                            ScriptRunning = true;
                        }
                    }
                    else
                    {
                        if (ScriptManager.Running)
                        {
                            World.Player?.SendMessage(LocString.ScriptFinished);
                            Assistant.Engine.MainWindow.LockScriptUI(false);
                            ScriptRunning = false;
                        }
                    }
                }
                catch (RunTimeError ex)
                {

                    if (ex.Node != null)
                    {
                        World.Player?.SendMessage(MsgLevel.Error, $"Script Error: {ex.Message} (Line: {ex.Node.LineNumber + 1})");

                        SetHighlightLine(ex.Node.LineNumber, Color.Red);
                    }
                    else
                    {
                        World.Player?.SendMessage(MsgLevel.Error, $"Script Error: {ex.Message}");
                    }

                    StopScript();
                }
                catch (Exception ex)
                {
                    World.Player?.SendMessage(MsgLevel.Error, $"Script Error: {ex.Message}");
                    StopScript();
                }
            }
        }

        private static readonly HotKeyCallbackState HotkeyCallback = OnHotKey;

        /// <summary>
        /// This is called via reflection when the application starts up
        /// </summary>
        public static void Initialize()
        {
            HotKey.Add(HKCategory.Scripts, LocString.StopScript, new HotKeyCallback(HotkeyStopScript));

            Scripts = new List<RazorScript>();

            LoadScripts();

            foreach (RazorScript script in Scripts)
            {
                AddHotkey(script.Name);
            }
        }

        private static void HotkeyStopScript()
        {
            StopScript();
        }

        public static void AddHotkey(string script)
        {
            HotKey.Add(HKCategory.Scripts, HKSubCat.None, Language.Format(LocString.PlayScript, script), HotkeyCallback,
                script);
        }

        public static void RemoveHotkey(string script)
        {
            HotKey.Remove(Language.Format(LocString.PlayScript, script));
        }

        public static void OnHotKey(ref object state)
        {
            PlayScript((string) state);
        }

        public static void StopScript()
        {
            _queuedScript = null;

            Interpreter.StopScript();
        }

        public static void PlayScript(string scriptName)
        {
            foreach (RazorScript razorScript in Scripts)
            {
                if (razorScript.Name.Equals(scriptName, StringComparison.OrdinalIgnoreCase))
                {
                    PlayScript(razorScript.Lines);
                    break;
                }
            }
        }

        public static void PlayScript(string[] lines)
        {
            if (World.Player == null || ScriptEditor == null || lines == null)
                return;

            if (MacroManager.Playing || MacroManager.StepThrough)
                MacroManager.Stop();

            StopScript(); // be sure nothing is running

            SetLastTargetActive = false;
            SetVariableActive = false;
            
            if (_queuedScript != null)
                return;

            if (!Client.Instance.ClientRunning)
                return;

            if (World.Player == null)
                return;

            ClearHighlightLine();

            Script script = new Script(Lexer.Lex(lines));

            _queuedScript = script;
        }

        private static ScriptTimer Timer { get; }

        static ScriptManager()
        {
            Timer = new ScriptTimer();
        }

        public static void SetControls(FastColoredTextBox scriptEditor, ListBox scriptList)
        {
            ScriptEditor = scriptEditor;
            ScriptList = scriptList;
        }

        public static void OnLogin()
        {
            Commands.Register();
            AgentCommands.Register();
            Aliases.Register();
            Expressions.Register();

            Timer.Start();
        }

        public static void OnLogout()
        {
            Timer.Stop();
        }

        public static void StartEngine()
        {
            Timer.Start();
        }

        public static void StopEngine()
        {
            Timer.Stop();
        }

        public class RazorScript
        {
            public string Path { get; set; }
            public string[] Lines { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public static List<RazorScript> Scripts { get; set; }

        public static void LoadScripts()
        {
            Scripts.Clear();

            foreach (string file in Directory.GetFiles(ScriptPath, "*.razor"))
            {
                Scripts.Add(new RazorScript
                {
                    Lines = File.ReadAllLines(file),
                    Name = Path.GetFileNameWithoutExtension(file),
                    Path = file
                });
            }
        }

        public static void DisplayScriptVariables(ListBox list)
        {
            list.SafeAction(s =>
            {
                s.BeginUpdate();
                s.Items.Clear();

                foreach (ScriptVariables.ScriptVariable at in ScriptVariables.ScriptVariableList)
                {
                    s.Items.Add($"'{at.Name}' ({at.TargetInfo.Serial})");
                }

                s.EndUpdate();
                s.Refresh();
                s.Update();
            });
        }

        public static bool AddToScript(string command)
        {
            if (Recording)
            {
                ScriptEditor?.AppendText(command + Environment.NewLine);

                return true;
            }

            return false;
        }

        private static int GetScriptIndex(string script)
        {
            for (int i = 0; i < Scripts.Count; i++)
            {
                if (Scripts[i].Name.Equals(script, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        public static void Error(bool quiet, string statement, string message, bool throwError = false)
        {
            if (quiet)
                return;

            World.Player?.SendMessage(MsgLevel.Error, $"{statement}: {message}");
        }

        public static List<ASTNode> ParseArguments(ref ASTNode node)
        {
            List<ASTNode> args = new List<ASTNode>();
            while (node != null)
            {
                args.Add(node);
                node = node.Next();
            }

            return args;
        }

        private delegate void SetHighlightLineDelegate(int iline, Color color);

        private static void SetHighlightLine(int iline, Color background)
        {
            for (int i = 0; i < ScriptEditor.LinesCount; i++)
            {
                ScriptEditor[i].BackgroundBrush = ScriptEditor.BackBrush;
            }

            ScriptEditor[iline].BackgroundBrush = new SolidBrush(background);
            ScriptEditor.Invalidate();
        }

        public static void ClearHighlightLine()
        {
            for (int i = 0; i < ScriptEditor.LinesCount; i++)
            {
                ScriptEditor[i].BackgroundBrush = ScriptEditor.BackBrush;
            }

            ScriptEditor.Invalidate();
        }

        private static FastColoredTextBoxNS.AutocompleteMenu _autoCompleteMenu;

        public static void InitScriptEditor()
        {
            _autoCompleteMenu = new AutocompleteMenu(ScriptEditor);
            //_autoCompleteMenu.Items.ImageList = imageList2;
            _autoCompleteMenu.SearchPattern = @"[\w\.:=!<>]";
            _autoCompleteMenu.AllowTabKey = true;
            _autoCompleteMenu.ToolTipDuration = 5000;
            _autoCompleteMenu.AppearInterval = 100;

            #region Keywords

            string[] keywords =
            {
                "if", "elseif", "else", "endif", "while", "endwhile", "for", "endfor", "break", "continue", "stop",
                "replay", "not", "and", "or"
            };

            #endregion

            #region Commands auto-complete

            string[] commands =
            {
                "cast", "dress", "undress", "dressconfig", "target", "targettype", "targetrelloc", "dress", "drop",
                "waitfortarget", "wft", "dclick", "dclicktype", "dclickvar", "usetype", "useobject", "droprelloc",
                "lift", "lifttype", "waitforgump", "gumpresponse", "gumpclose", "menu", "menuresponse", "waitformenu",
                "promptresponse", "waitforprompt", "hotkey", "say", "msg", "overhead", "sysmsg", "wait", "pause",
                "waitforstat", "setability", "setlasttarget", "lasttarget", "setvar", "skill", "useskill", "walk",
                "script", "attack", "useonce", "organizer", "org", "organize", "restock", "scav", "scavenger", "potion"
            };

            #endregion

            Dictionary<string, ToolTipDescriptions> descriptionCommands = new Dictionary<string, ToolTipDescriptions>();

            #region DropTips

            ToolTipDescriptions tooltip = new ToolTipDescriptions("drop", new[] {"drop (serial) (x y z/layername)"},
                "N/A", "Drop a specific item on the location or on you", "drop 0x42ABD 234 521 0\ndrop 0x42ABD Helm");
            descriptionCommands.Add("drop", tooltip);

            #endregion

            #region DressTips

            #endregion

            #region TargetTips

            tooltip = new ToolTipDescriptions("target", new[] {"target (serial) [x] [y] [z]"}, "N/A",
                "Target a specific item or mobile OR target a specific x, y, z location",
                "target 0x345A\ntarget 1563 2452 0");
            descriptionCommands.Add("target", tooltip);

            tooltip = new ToolTipDescriptions("targettype", new[] {"targettype (isMobile) (graphic)"}, "N/A",
                "Target a specific type of item/mobile", "targettype true 0x43");
            descriptionCommands.Add("targettype", tooltip);

            tooltip = new ToolTipDescriptions("targetrelloc", new[] {"targetrelloc (x-offset) (y-offset)"}, "N/A",
                "Target a relative location based on your location", "targetrelloc -4 6");
            descriptionCommands.Add("targetrelloc", tooltip);

            #endregion

            #region DClickTips

            #endregion

            #region MovingTips

            #endregion

            #region GumpTips

            #endregion

            #region MenuTips

            #endregion

            #region PromptTips

            #endregion

            #region HotKeyTips

            #endregion

            #region MessageTips

            #endregion

            #region WaitPauseTips

            #endregion

            #region DressTips

            #endregion

            #region MiscTips

            #endregion


            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));

            foreach (var item in commands)
            {
                descriptionCommands.TryGetValue(item, out ToolTipDescriptions element);

                if (element != null)
                {
                    items.Add(new MethodAutocompleteItemAdvance(item)
                    {
                        ImageIndex = 2,
                        ToolTipTitle = element.Title,
                        ToolTipText = element.ToolTipDescription()
                    });
                }
                else
                {
                    items.Add(new MethodAutocompleteItemAdvance(item)
                    {
                        ImageIndex = 2
                    });
                }
            }

            _autoCompleteMenu.Items.SetAutocompleteItems(items);
            _autoCompleteMenu.Items.MaximumSize =
                new Size(_autoCompleteMenu.Items.Width + 20, _autoCompleteMenu.Items.Height);
            _autoCompleteMenu.Items.Width = _autoCompleteMenu.Items.Width + 20;

            ScriptEditor.Language = FastColoredTextBoxNS.Language.Razor;
        }

        public class ToolTipDescriptions
        {
            public string Title;
            public string[] Parameters;
            public string Returns;
            public string Description;
            public string Example;

            public ToolTipDescriptions(string title, string[] parameter, string returns, string description,
                string example)
            {
                Title = title;
                Parameters = parameter;
                Returns = returns;
                Description = description;
                Example = example;
            }

            public string ToolTipDescription()
            {
                string complete_description = string.Empty;

                complete_description += "Parameter(s): ";

                foreach (string parameter in Parameters)
                    complete_description += "\n\t" + parameter;

                complete_description += "\nReturns: " + Returns;

                complete_description += "\nDescription:";

                complete_description += "\n\t" + Description;

                complete_description += "\nExample(s):";

                complete_description += "\n\t" + Example;

                return complete_description;
            }
        }

        public class MethodAutocompleteItemAdvance : MethodAutocompleteItem
        {
            string firstPart;
            string lastPart;

            public MethodAutocompleteItemAdvance(string text)
                : base(text)
            {
                var i = text.LastIndexOf(' ');
                if (i < 0)
                    firstPart = text;
                else
                {
                    firstPart = text.Substring(0, i);
                    lastPart = text.Substring(i + 1);
                }
            }

            public override CompareResult Compare(string fragmentText)
            {
                int i = fragmentText.LastIndexOf(' ');

                if (i < 0)
                {
                    if (firstPart.StartsWith(fragmentText) && string.IsNullOrEmpty(lastPart))
                        return CompareResult.VisibleAndSelected;
                    //if (firstPart.ToLower().Contains(fragmentText.ToLower()))
                    //  return CompareResult.Visible;
                }
                else
                {
                    var fragmentFirstPart = fragmentText.Substring(0, i);
                    var fragmentLastPart = fragmentText.Substring(i + 1);


                    if (firstPart != fragmentFirstPart)
                        return CompareResult.Hidden;

                    if (lastPart != null && lastPart.StartsWith(fragmentLastPart))
                        return CompareResult.VisibleAndSelected;

                    if (lastPart != null && lastPart.ToLower().Contains(fragmentLastPart.ToLower()))
                        return CompareResult.Visible;
                }

                return CompareResult.Hidden;
            }

            public override string GetTextForReplace()
            {
                if (lastPart == null)
                    return firstPart;

                return firstPart + " " + lastPart;
            }

            public override string ToString()
            {
                if (lastPart == null)
                    return firstPart;

                return lastPart;
            }
        }

        public static void RedrawScripts()
        {
            ScriptList.SafeAction(s =>
            {
                int curIndex = 0;

                if (s.SelectedIndex > -1)
                    curIndex = s.SelectedIndex;

                s.BeginUpdate();
                s.Items.Clear();

                LoadScripts();

                foreach (RazorScript script in Scripts)
                {
                    if (script != null)
                        s.Items.Add(script);
                }

                if (s.Items.Count > 0 && (curIndex - 1 != -1))
                    s.SelectedIndex = curIndex - 1;

                s.EndUpdate();
            });
        }
    }
}