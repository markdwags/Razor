﻿#region license

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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Assistant.Gumps.Internal;
using Assistant.Macros;
using Assistant.Scripts.Engine;
using Assistant.UI;
using FastColoredTextBoxNS;

namespace Assistant.Scripts
{
    public static class ScriptManager
    {
        public static bool Recording { get; set; }

        public static bool Paused => ScriptPaused;

        private static bool ScriptPaused { get; set; }

        public static bool Running => ScriptRunning;

        private static bool ScriptRunning { get; set; }

        public static DateTime LastWalk { get; set; }

        public static bool SetLastTargetActive { get; set; }

        public static bool SetVariableActive { get; set; }
        
        public static bool TargetFound { get; set; }

        public static string ScriptPath => Config.GetUserDirectory("Scripts");

        private static FastColoredTextBox ScriptEditor { get; set; }

        private static TreeView ScriptTree { get; set; }

        private static ListBox ScriptVariableList { get; set; }

        private static Script _queuedScript;

        public static bool BlockPopupMenu { get; set; }

        public enum HighlightType
        {
            Error,
            Execution
        }

        private static Dictionary<HighlightType, List<int>> HighlightLines { get; } = new Dictionary<HighlightType, List<int>>();

        private static Dictionary<HighlightType, Brush> HighlightLineColors { get; } = new Dictionary<HighlightType, Brush>()
        {
            { HighlightType.Error, new SolidBrush(Color.Red) },
            { HighlightType.Execution, new SolidBrush(Color.Blue) }
        };

        private static HighlightType[] GetHighlightTypes()
        {
            return (HighlightType[])Enum.GetValues(typeof(HighlightType));
        }

        public static RazorScript SelectedScript { get; set; }

        public static bool PopoutEditor { get; set; }

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
                        Script script = _queuedScript;

                        running = Interpreter.StartScript(script);
                        UpdateLineNumber(Interpreter.CurrentLine);

                        _queuedScript = null;
                    }
                    else
                    {
                        running = Interpreter.ExecuteScript();

                        if (running)
                        {
                            UpdateLineNumber(Interpreter.CurrentLine);
                        }
                    }


                    if (running)
                    {
                        if (ScriptManager.Running == false)
                        {
                            if (Config.GetBool("ScriptDisablePlayFinish"))
                                World.Player?.SendMessage(LocString.ScriptPlaying);

                            Assistant.Engine.MainWindow.LockScriptUI(true);
                            Assistant.Engine.RazorScriptEditorWindow?.LockScriptUI(true);
                            ScriptRunning = true;
                        }
                    }
                    else
                    {
                        if (ScriptManager.Running)
                        {
                            if (Config.GetBool("ScriptDisablePlayFinish"))
                                World.Player?.SendMessage(LocString.ScriptFinished);

                            Assistant.Engine.MainWindow.LockScriptUI(false);
                            Assistant.Engine.RazorScriptEditorWindow?.LockScriptUI(false);
                            ScriptRunning = false;

                            ClearHighlightLine(HighlightType.Execution);
                        }
                    }
                }
                catch (Exception ex)
                {
                    World.Player?.SendMessage(MsgLevel.Error, $"Script Error: {ex.Message} (Line: {Interpreter.CurrentLine + 1})");

                    SetHighlightLine(Interpreter.CurrentLine, HighlightType.Error);

                    StopScript();
                }
            }
        }

        /// <summary>
        /// This is called via reflection when the application starts up
        /// </summary>
        public static void Initialize()
        {
            HotKey.Add(HKCategory.Scripts, HKSubCat.None, LocString.StopScript, HotkeyStopScript);
            HotKey.Add(HKCategory.Scripts, HKSubCat.None, LocString.PauseScript, HotkeyPauseScript);
            HotKey.Add(HKCategory.Scripts, HKSubCat.None, LocString.ScriptDClickType, HotkeyDClickTypeScript);
            HotKey.Add(HKCategory.Scripts, HKSubCat.None, LocString.ScriptTargetType, HotkeyTargetTypeScript);


            _scriptList = new List<RazorScript>();

            Recurse(null, Config.GetUserDirectory("Scripts"));
            
            foreach (HighlightType type in GetHighlightTypes())
            {
                HighlightLines[type] = new List<int>();
            }
        }

        private static void HotkeyTargetTypeScript()
        {
            if (World.Player != null)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.ScriptTargetType);
                Targeting.OneTimeTarget(OnTargetTypeScript);
            }
        }

        private static void OnTargetTypeScript(bool loc, Serial serial, Point3D pt, ushort itemId)
        {
            Item item = World.FindItem(serial);

            if (item != null && item.Serial.IsItem && item.Movable && item.Visible)
            {
                string cmd = $"targettype '{item.ItemID.ItemData.Name}'";

                Clipboard.SetDataObject(cmd);
                World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, cmd), false);
            }
            else
            {
                Mobile m = World.FindMobile(serial);

                if (m != null)
                {
                    string cmd = $"targettype '{m.Body}'";

                    Clipboard.SetDataObject(cmd);
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, cmd), false);
                }
            }
        }

        private static void HotkeyDClickTypeScript()
        {
            if (World.Player != null)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.ScriptTargetType);
                Targeting.OneTimeTarget(OnDClickTypeScript);
            }
        }

        private static void OnDClickTypeScript(bool loc, Serial serial, Point3D pt, ushort itemId)
        {
            Item item = World.FindItem(serial);

            if (item != null && item.Serial.IsItem && item.Movable && item.Visible)
            {
                string cmd = $"dclicktype '{item.ItemID.ItemData.Name}'";

                Clipboard.SetDataObject(cmd);
                World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, cmd), false);
            }
            else
            {
                Mobile m = World.FindMobile(serial);

                if (m != null)
                {
                    string cmd = $"dclicktype '{m.Body}'";

                    Clipboard.SetDataObject(cmd);
                    World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ScriptCopied, cmd), false);
                }
            }
        }

        private static void HotkeyStopScript()
        {
            StopScript();
        }

        private static void HotkeyPauseScript()
        {
            if (!ScriptRunning)
            {
                return;
            }

            if (ScriptPaused)
            {
                ResumeScript();
                World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.ResumeScriptMessage, Interpreter.CurrentLine), false);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Force, Language.Format(LocString.PauseScriptMessage, Interpreter.CurrentLine), false);
                PauseScript();
            }
        }

        private static void AddHotkey(RazorScript script)
        {
            HotKey.Add(HKCategory.Scripts, HKSubCat.None, Language.Format(LocString.PlayScript, script), OnHotKey,
                script);
        }

        private static void RemoveHotkey(RazorScript script)
        {
            HotKey.Remove(Language.Format(LocString.PlayScript, script.ToString()));
        }

        public static void OnHotKey(ref object state)
        {
            RazorScript script = (RazorScript) state;

            PlayScript(script.Lines);
        }

        public static void StopScript()
        {
            _queuedScript = null;
            ScriptPaused = false;

            Interpreter.StopScript();
        }

        public static void PauseScript()
        {
            ScriptPaused = true;
            Interpreter.PauseScript();
        }

        public static void ResumeScript()
        {
            ScriptPaused = false;
            Interpreter.ResumeScript();
        }

        public static RazorScript AddScript(string file)
        {
            RazorScript script = new RazorScript
            {
                Lines = File.ReadAllLines(file),
                Name = Path.GetFileNameWithoutExtension(file),
                Path = file
            };

            if (Path.GetDirectoryName(script.Path).Equals(Config.GetUserDirectory("Scripts")))
            {
                script.Category = string.Empty;
            }
            else
            {
                string cat = file.Replace(Config.GetUserDirectory("Scripts"), "").Substring(1);
                script.Category = Path.GetDirectoryName(cat).Replace("/", "\\");
            }

            AddHotkey(script);

            _scriptList.Add(script);

            return script;
        }

        public static void RemoveScript(RazorScript script)
        {
            RemoveHotkey(script);

            _scriptList.Remove(script);
        }
        
        public static void PlayScript(string scriptName)
        {
            foreach (RazorScript razorScript in _scriptList)
            {
                if (razorScript.ToString().IndexOf(scriptName, StringComparison.OrdinalIgnoreCase) != -1)
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

            ClearAllHighlightLines();

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

            Script script = new Script(Lexer.Lex(lines));

            _queuedScript = script;
        }

        private static void ActiveScriptStatementExecuted(ASTNode statement)
        {
            if (statement != null && PopoutEditor)
            {
                var lineNum = statement.LineNumber;

                SetHighlightLine(lineNum, HighlightType.Execution);
                // Scrolls to relevant line, per this suggestion: https://github.com/PavelTorgashov/FastColoredTextBox/issues/115
                ScriptEditor.Selection.Start = new Place(0, lineNum);
                ScriptEditor.DoSelectionVisible();
            }
        }

        private static ScriptTimer Timer { get; }

        static ScriptManager()
        {
            Timer = new ScriptTimer();
        }

        private static void UpdateLineNumber(int lineNum)
        {
            if (PopoutEditor)
            {
                SetHighlightLine(lineNum, HighlightType.Execution);
                // Scrolls to relevant line, per this suggestion: https://github.com/PavelTorgashov/FastColoredTextBox/issues/115
                ScriptEditor.Selection.Start = new Place(0, lineNum);
                ScriptEditor.DoSelectionVisible();
            }
        }

        public static void SetEditor(FastColoredTextBox scriptEditor, bool popoutEditor)
        {
            ScriptEditor = scriptEditor;
            ScriptEditor.Visible = true;

            PopoutEditor = popoutEditor;

            InitScriptEditor();

            if (SelectedScript != null)
            {
                SetEditorText(SelectedScript);
            }
        }

        public static void SetEditorText(RazorScript selectedScript)
        {
            SelectedScript = selectedScript;

            ScriptEditor.Text = string.Join("\n", SelectedScript.Lines);
        }

        public static void SetControls(FastColoredTextBox scriptEditor, TreeView scriptTree, ListBox scriptVariables)
        {
            ScriptEditor = scriptEditor;
            ScriptTree = scriptTree;
            ScriptVariableList = scriptVariables;
        }

        public static void OnLogin()
        {
            Commands.Register();
            AgentCommands.Register();
            SpeechCommands.Register();
            TargetCommands.Register();

            Aliases.Register();
            Expressions.Register();

            Timer.Start();
        }

        public static void OnLogout()
        {
            StopScript();
            Timer.Stop();
            Assistant.Engine.MainWindow.LockScriptUI(false);
            Assistant.Engine.RazorScriptEditorWindow?.LockScriptUI(false);
        }

        public static void StartEngine()
        {
            Timer.Start();
        }

        private static List<RazorScript> _scriptList { get; set; }

        public static void RedrawScriptVariables()
        {
            ScriptVariableList?.SafeAction(s =>
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
            for (int i = 0; i < _scriptList.Count; i++)
            {
                if (_scriptList[i].Name.Equals(script, StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// Adds a new highlight of specified type
        /// </summary>
        /// <param name="iline">Line number to highlight</param>
        /// <param name="type">Type of highlight to set</param>
        private static void AddHighlightLine(int iline, HighlightType type)
        {
            HighlightLines[type].Add(iline);
            RefreshHighlightLines();
        }

        /// <summary>
        /// Clears existing highlight lines of this type, and adds a new one at specified line number
        /// </summary>
        /// <param name="iline">Line number to highlight</param>
        /// <param name="type">Type of highlight to set</param>
        private static void SetHighlightLine(int iline, HighlightType type)
        {
            if (!PopoutEditor)
                return;

            ClearHighlightLine(type);
            AddHighlightLine(iline, type);
        }

        public static void ClearHighlightLine(HighlightType type)
        {
            if (!PopoutEditor)
                return;

            HighlightLines[type].Clear();
            RefreshHighlightLines();
        }

        public static void ClearAllHighlightLines()
        {
            if (!PopoutEditor)
                return;

            foreach (HighlightType type in GetHighlightTypes())
            {
                HighlightLines[type].Clear();
            }

            RefreshHighlightLines();
        }

        private static void RefreshHighlightLines()
        {
            for (int i = 0; i < ScriptEditor.LinesCount; i++)
            {
                ScriptEditor[i].BackgroundBrush = ScriptEditor.BackBrush;
            }

            foreach (HighlightType type in GetHighlightTypes())
            {
                foreach (int lineNum in HighlightLines[type])
                {
                    ScriptEditor[lineNum].BackgroundBrush = HighlightLineColors[type];
                }
            }

            ScriptEditor.Invalidate();
        }

        private static FastColoredTextBoxNS.AutocompleteMenu _autoCompleteMenu;

        public static void InitScriptEditor()
        {
            _autoCompleteMenu = new AutocompleteMenu(ScriptEditor)
            {
                SearchPattern = @"[\w\.:=!<>]",
                AllowTabKey = true,
                ToolTipDuration = 5000,
                AppearInterval = 100
            };

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
                "alliance", "clearall", "cleardragdrop", "clearhands", "emote", "guild", "attack", "interrupt", "virtue", "yell",
                "cast", "dclick", "dclicktype",
                "dress", "drop", "droprelloc", "gumpresponse", "gumpclose", "hotkey", "lasttarget", "lift", "lifttype",
                "menu", "menuresponse", "organizer", "overhead", "potion", "promptresponse", "restock", "say",
                "whisper", "yell", "emote", "script", "scavenger", "sell", "setability", "setlasttarget", "setvar",
                "skill", "sysmsg", "target", "targettype", "targetrelloc", "undress", "useonce", "walk", "wait",
                "pause", "waitforgump", "waitformenu", "waitforprompt", "waitfortarget", "waitforsysmsg", "clearsysmsg", "clearjournal",
                "waitforsysmsg", "random"
            };

            #endregion

            Dictionary<string, ToolTipDescriptions> descriptionCommands = new Dictionary<string, ToolTipDescriptions>();

            #region CommandToolTips

            var tooltip = new ToolTipDescriptions("attack", new[] { "attack (serial) or attack ('variablename')" },
                "N/A", "Attack a specific serial or variable tied to a serial.", "attack 0x2AB4\n\tattack 'attackdummy'");
            descriptionCommands.Add("attack", tooltip);

            tooltip = new ToolTipDescriptions("clearall", new[] { "clearall" }, "N/A", "Clear target, clear queues, drop anything you're holding",
                "clearall");
            descriptionCommands.Add("clearall", tooltip);

            tooltip = new ToolTipDescriptions("clearhands", new[] { "clearhands ('right'/'left'/'hands')" }, "N/A", "Use the item in your hands",
                "clearhands");
            descriptionCommands.Add("clearhands", tooltip);

            tooltip = new ToolTipDescriptions("virtue", new[] { "virtue ('honor'/'sacrifice'/'valor')" }, "N/A", "Invoke a specific virtue",
                "virtue 'honor'");
            descriptionCommands.Add("virtue", tooltip);

            tooltip = new ToolTipDescriptions("cast", new[] { "cast ('name of spell')" }, "N/A", "Cast a spell by name",
                "cast 'blade spirits'");
            descriptionCommands.Add("cast", tooltip);

            tooltip = new ToolTipDescriptions("dclick", new[] { "dclick (serial) or useobject (serial)" }, "N/A",
                "This command will use (double-click) a specific item or mobile.", "dclick 0x34AB");
            descriptionCommands.Add("dclick", tooltip);

            tooltip = new ToolTipDescriptions("dclicktype",
                new[]
                {
                        "dclicktype ('name of item') OR (graphicID) [inrange] or usetype ('name of item') OR (graphicID) [inrange/backpack] [hue]"
                }, "N/A",
                "This command will use (double-click) an item type either provided by the name or the graphic ID.\n\t\tIf you include the optional true parameter, items within range (2 tiles) will only be considered.",
                "dclicktype 'dagger'\n\t\twaitfortarget\n\t\ttargettype 'robe'");
            descriptionCommands.Add("dclicktype", tooltip);

            tooltip = new ToolTipDescriptions("dress", new[] { "dress ('name of dress list')" }, "N/A",
                "This command will execute a spec dress list you have defined in Razor.", "dress 'My Sunday Best'");
            descriptionCommands.Add("dress", tooltip);

            tooltip = new ToolTipDescriptions("drop", new[] { "drop (serial) (x/y/z/layername)" }, "N/A",
                "This command will drop the item you are holding either at your feet,\n\t\ton a specific layer or at a specific X / Y / Z location.",
                "lift 0x400D54A7 1\n\t\tdrop 0x6311 InnerTorso");
            descriptionCommands.Add("drop", tooltip);

            tooltip = new ToolTipDescriptions("", new[] { "" }, "N/A", "",
                "lift 0x400D54A7 1\n\twait 5000\n\tdrop 0xFFFFFFFF 5926 1148 0");
            descriptionCommands.Add("", tooltip);

            tooltip = new ToolTipDescriptions("droprelloc", new[] { "droprelloc (x) (y)" }, "N/A",
                "This command will drop the item you're holding to a location relative to your position.",
                "lift 0x400EED2A 1\n\twait 1000\n\tdroprelloc 1 1");
            descriptionCommands.Add("droprelloc", tooltip);

            tooltip = new ToolTipDescriptions("gumpresponse", new[] { "gumpresponse (buttonID)" }, "N/A",
                "Responds to a specific gump button", "gumpresponse 4");
            descriptionCommands.Add("gumpresponse", tooltip);

            tooltip = new ToolTipDescriptions("gumpclose", new[] { "gumpclose" }, "N/A",
                "This command will close the last gump that opened.", "gumpclose");
            descriptionCommands.Add("gumpclose", tooltip);

            tooltip = new ToolTipDescriptions("hotkey", new[] { "hotkey ('name of hotkey')" }, "N/A",
                "This command will execute any Razor hotkey by name.",
                "skill 'detect hidden'\n\twaitfortarget\n\thotkey 'target self'");
            descriptionCommands.Add("hotkey", tooltip);

            tooltip = new ToolTipDescriptions("lasttarget", new[] { "lasttarget" }, "N/A",
                "This command will target your last target set in Razor.",
                "cast 'magic arrow'\n\twaitfortarget\n\tlasttarget");
            descriptionCommands.Add("lasttarget", tooltip);

            tooltip = new ToolTipDescriptions("lift", new[] { "lift (serial) [amount]" }, "N/A",
                "This command will lift a specific item and amount. If no amount is provided, 1 is defaulted.",
                "lift 0x400EED2A 1\n\twait 1000\n\tdroprelloc 1 1 0");
            descriptionCommands.Add("lift", tooltip);

            tooltip = new ToolTipDescriptions("lifttype",
                new[] { "lifttype (gfx) [amount] or lifttype ('name of item') [amount] [hue]" }, "N/A",
                "This command will lift a specific item by type either by the graphic id or by the name.\n\tIf no amount is provided, 1 is defaulted.",
                "lifttype 'robe'\n\twait 1000\n\tdroprelloc 1 1 0\n\tlifttype 0x1FCD\n\twait 1000\n\tdroprelloc 1 1");
            descriptionCommands.Add("lifttype", tooltip);

            tooltip = new ToolTipDescriptions("menu", new[] { "menu (serial) (index) [false]" }, "N/A",
                "Selects a specific index within a context menu", "# open backpack\n\tmenu 0 1");
            descriptionCommands.Add("menu", tooltip);

            tooltip = new ToolTipDescriptions("menuresponse", new[] { "menuresponse (index) (menuId) [hue]" }, "N/A",
                "Responds to a specific menu and menu ID (not a context menu)", "menuresponse 3 4");
            descriptionCommands.Add("menuresponse", tooltip);

            tooltip = new ToolTipDescriptions("organizer", new[] { "organizer (number) ['set']" }, "N/A",
                "This command will execute a specific organizer agent. If the set parameter is included,\n\tyou will instead be prompted to set the organizer agent's hotbag.",
                "organizer 1\n\torganizer 4 'set'");
            descriptionCommands.Add("organizer", tooltip);

            tooltip = new ToolTipDescriptions("overhead", new[] { "overhead ('text') [color] [serial]" }, "N/A",
                "This command will display a message over your head. Only you can see this.",
                "if stam = 100\n\t    overhead 'ready to go!'\n\tendif");
            descriptionCommands.Add("overhead", tooltip);

            tooltip = new ToolTipDescriptions("potion", new[] { "potion ('potion type')" }, "N/A",
                "This command will use a specific potion based on the type.", "potion 'agility'\n\tpotion 'heal'");
            descriptionCommands.Add("potion", tooltip);

            tooltip = new ToolTipDescriptions("promptresponse", new[] { "promptresponse ('prompt response')" }, "N/A",
                "This command will respond to a prompt triggered from actions such as renaming runes or giving a guild title.",
                "dclicktype 'rune'\n\twaitforprompt\n\tpromptresponse 'to home'");
            descriptionCommands.Add("promptresponse", tooltip);

            tooltip = new ToolTipDescriptions("restock", new[] { "restock (number) ['set']" }, "N/A",
                "This command will execute a specific restock agent.\n\tIf the set parameter is included, you will instead be prompted to set the restock agent's hotbag.",
                "restock 1\n\trestock 4 'set'");
            descriptionCommands.Add("restock", tooltip);

            tooltip = new ToolTipDescriptions("say",
                new[] { "say ('message to send') [hue] or msg ('message to send') [hue]" }, "N/A",
                "This command will force your character to say the message passed as the parameter.",
                "say 'Hello world!'\n\tsay 'Hello world!' 454");
            descriptionCommands.Add("say", tooltip);

            tooltip = new ToolTipDescriptions("whisper",
                new[] { "whisper ('message to send') [hue]" }, "N/A",
                "This command will force your character to whisper the message passed as the parameter.",
                "whisper 'Hello world!'\n\twhisper 'Hello world!' 454");
            descriptionCommands.Add("whisper", tooltip);

            tooltip = new ToolTipDescriptions("yell",
                new[] { "yell ('message to send') [hue]" }, "N/A",
                "This command will force your character to yell the message passed as the parameter.",
                "yell 'Hello world!'\n\tyell 'Hello world!' 454");
            descriptionCommands.Add("yell", tooltip);

            tooltip = new ToolTipDescriptions("emote",
                new[] { "emote ('message to send') [hue]" }, "N/A",
                "This command will force your character to emote the message passed as the parameter.",
                "emote 'Hello world!'\n\temote 'Hello world!' 454");
            descriptionCommands.Add("emote", tooltip);

            tooltip = new ToolTipDescriptions("script", new[] { "script 'name'" }, "N/A",
                "This command will call another script.", "if hp = 40\n\t   script 'healself'\n\tendif");
            descriptionCommands.Add("script", tooltip);

            tooltip = new ToolTipDescriptions("scavenger", new[] { "scavenger ['clear'/'add'/'on'/'off'/'set']" },
                "N/A", "This command will control the scavenger agent.", "scavenger 'off'");
            descriptionCommands.Add("scavenger", tooltip);

            tooltip = new ToolTipDescriptions("sell", new[] { "sell" }, "N/A",
                "This command will set the Sell agent's hotbag.", "sell");
            descriptionCommands.Add("sell", tooltip);

            tooltip = new ToolTipDescriptions("setability",
                new[] { "setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']" }, "N/A",
                "This will set a specific ability on or off. If on or off is missing, on is defaulted.",
                "setability stun");
            descriptionCommands.Add("setability", tooltip);

            tooltip = new ToolTipDescriptions("setlasttarget", new[] { "setlasttarget" }, "N/A",
                "This command will pause the script until you select a target to be set as Last Target.",
                "overhead 'set last target'\n\tsetlasttarget\n\toverhead 'set!'\n\tcast 'magic arrow'\n\twaitfortarget\n\ttarget 'last'");
            descriptionCommands.Add("setlasttarget", tooltip);

            tooltip = new ToolTipDescriptions("setvar", new[] { "setvar ('variable') or setvariable ('variable')" },
                "N/A",
                "This command will pause the script until you select a target to be assigned a variable.\n\tPlease note, the variable must exist before you can assign values to it.",
                "setvar 'dummy'\n\tcast 'magic arrow'\n\twaitfortarget\n\ttarget 'dummy'");
            descriptionCommands.Add("setvar", tooltip);

            tooltip = new ToolTipDescriptions("skill", new[] { "skill 'name of skill' or skill last" }, "N/A",
                "This command will use a specific skill (assuming it's a usable skill).",
                "while mana < maxmana\n\t    say 'mediation!'\n\t    skill 'meditation'\n\t    wait 11000\n\tendwhile");
            descriptionCommands.Add("skill", tooltip);

            tooltip = new ToolTipDescriptions("sysmsg", new[] { "sysmsg ('message to display in system message')" },
                "N/A", "This command will display a message in the lower-left of the client.",
                "if stam = 100\n\t    sysmsg 'ready to go!'\n\tendif");
            descriptionCommands.Add("sysmsg", tooltip);

            tooltip = new ToolTipDescriptions("target", new[] { "target (serial) or target (x) (y) (z)" }, "N/A",
                "This command will target a specific mobile or item or target a specific location based on X/Y/Z coordinates.",
                "cast 'lightning'\n\twaitfortarget\n\ttarget 0xBB3\n\tcast 'fire field'\n\twaitfortarget\n\ttarget 5923 1145 0");
            descriptionCommands.Add("target", tooltip);

            tooltip = new ToolTipDescriptions("targettype",
                new[] { "targettype (graphic) or targettype ('name of item or mobile type') [inrangecheck/backpack] [hue]" }, "N/A",
                "This command will target a specific type of mobile or item based on the graphic id or based on\n\tthe name of the item or mobile. If the optional parameter is passed\n\tin as true only items within the range of 2 tiles will be considered.",
                "usetype 'dagger'\n\twaitfortarget\n\ttargettype 'robe'\n\tuseobject 0x4005ECAF\n\twaitfortarget\n\ttargettype 0x1f03\n\tuseobject 0x4005ECAF\n\twaitfortarget\n\ttargettype 0x1f03 true");
            descriptionCommands.Add("targettype", tooltip);

            tooltip = new ToolTipDescriptions("targetrelloc", new[] { "targetrelloc (x-offset) (y-offset)" }, "N/A",
                "This command will target a specific location on the map relative to your position.",
                "cast 'fire field'\n\twaitfortarget\n\ttargetrelloc 1 1");
            descriptionCommands.Add("targetrelloc", tooltip);

            tooltip = new ToolTipDescriptions("undress",
                new[] { "undress ['name of dress list']' or undress 'LayerName'" }, "N/A",
                "This command will either undress you completely if no dress list is provided.\n\tIf you provide a dress list, only those specific items will be undressed. Lastly, you can define a layer name to undress.",
                "undress\n\tundress 'My Sunday Best'\n\tundress 'Shirt'\n\tundrsss 'Pants'");
            descriptionCommands.Add("undress", tooltip);

            tooltip = new ToolTipDescriptions("useonce", new[] { "useonce ['add'/'addcontainer']" }, "N/A",
                "This command will execute the UseOnce agent. If the add parameter is included, you can add items to your UseOnce list.\n\tIf the addcontainer parameter is included, you can add all items in a container to your UseOnce list.",
                "useonce\n\tuseonce 'add'\n\tuseonce 'addcontainer'");
            descriptionCommands.Add("useonce", tooltip);

            tooltip = new ToolTipDescriptions("walk", new[] { "walk ('direction')" }, "N/A",
                "This command will turn and/or walk your player in a certain direction.",
                "walk 'North'\n\twalk 'Up'\n\twalk 'West'\n\twalk 'Left'\n\twalk 'South'\n\twalk 'Down'\n\twalk 'East'\n\twalk 'Right'");
            descriptionCommands.Add("walk", tooltip);

            tooltip = new ToolTipDescriptions("wait",
                new[] { "wait [time in milliseconds or pause [time in milliseconds]" }, "N/A",
                "This command will pause the execution of a script for a given time.",
                "while stam < 100\n\t    wait 5000\n\tendwhile");
            descriptionCommands.Add("wait", tooltip);

            tooltip = new ToolTipDescriptions("pause",
                new[] { "pause [time in milliseconds or pause [time in milliseconds]" }, "N/A",
                "This command will pause the execution of a script for a given time.",
                "while stam < 100\n\t    wait 5000\n\tendwhile");
            descriptionCommands.Add("pause", tooltip);

            tooltip = new ToolTipDescriptions("waitforgump", new[] { "waitforgump [gump id]" }, "N/A",
                "This command will wait for a gump. If no gump id is provided, it will wait for **any * *gump.",
                "waitforgump\n\twaitforgump 4");
            descriptionCommands.Add("waitforgump", tooltip);

            tooltip = new ToolTipDescriptions("waitformenu", new[] { "waitformenu [menu id]" }, "N/A",
                "This command will wait for menu (not a context menu). If no menu id is provided, it will wait for **any * *menu.",
                "waitformenu\n\twaitformenu 4");
            descriptionCommands.Add("waitformenu", tooltip);

            tooltip = new ToolTipDescriptions("waitforprompt", new[] { "waitforprompt" }, "N/A",
                "This command will wait for a prompt before continuing.",
                "dclicktype 'rune'\n\twaitforprompt\n\tpromptresponse 'to home'");
            descriptionCommands.Add("waitforprompt", tooltip);

            tooltip = new ToolTipDescriptions("waitfortarget",
                new[] { "waitfortarget [pause in milliseconds] or wft [pause in milliseconds]" }, "N/A",
                "This command will cause the script to pause until you have a target cursor.\n\tBy default it will wait 30 seconds but you can define a specific wait time if you prefer.",
                "cast 'energy bolt'\n\twaitfortarget\n\thotkey 'Target Closest Enemy'");
            descriptionCommands.Add("waitfortarget", tooltip);

            tooltip = new ToolTipDescriptions("clearsysmsg",
                new[] { "clearsysmsg" }, "N/A",
                "This command will clear the internal system message queue used with insysmsg.",
                "clearsysmsg\n");
            descriptionCommands.Add("clearsysmsg", tooltip);

            tooltip = new ToolTipDescriptions("clearjournal",
                new[] { "clearjournal" }, "N/A",
                "This command (same as clearjournal) will clear the internal system message queue used with insysmsg.",
                "clearjournal\n");
            descriptionCommands.Add("clearjournal", tooltip);

            tooltip = new ToolTipDescriptions("waitforsysmsg",
                new[] { "waitforsysmsg" }, "N/A",
                "This command will pause the script until the message defined is in the system message queue.",
                "waitforsysmsg 'message here'\n");
            descriptionCommands.Add("waitforsysmsg", tooltip);

            tooltip = new ToolTipDescriptions("random",
                new[] { "random [max number]" }, "N/A",
                "This command output a random number between 1 and the max number provided.",
                "random '15'\n");
            descriptionCommands.Add("random", tooltip);

            #endregion

            if (!Config.GetBool("DisableScriptTooltips"))
            {
                List<AutocompleteItem> items = new List<AutocompleteItem>();

                foreach (var item in keywords)
                {
                    items.Add(new AutocompleteItem(item));
                }

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
            }
            else
            {
                _autoCompleteMenu.Items.SetAutocompleteItems(new List<AutocompleteItem>());
            }

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
            ScriptTree.SafeAction(s =>
            {
                s.BeginUpdate();
                s.Nodes.Clear();
                Recurse(s.Nodes, Config.GetUserDirectory("Scripts"));
                s.EndUpdate();
                s.Refresh();
                s.Update();
            });

            RedrawScriptVariables();
        }

        public static TreeNode GetScriptDirNode()
        {
            if (ScriptTree.SelectedNode == null)
            {
                return null;
            }

            if (ScriptTree.SelectedNode.Tag is string)
                return ScriptTree.SelectedNode;
                
            if (!(ScriptTree.SelectedNode.Parent?.Tag is string))
                return null;
                
            return ScriptTree.SelectedNode.Parent;
        }

        public static void AddScriptNode(TreeNode node)
        {
            if (node == null)
            {
                ScriptTree.Nodes.Add(node);
            }
            else
            {
                node.Nodes.Add(node);
            }

            ScriptTree.SelectedNode = node;
        }

        private static void Recurse(TreeNodeCollection nodes, string path)
        {
            try
            {
                var razorFiles = Directory.GetFiles(path, "*.razor");
                razorFiles = razorFiles.OrderBy(fileName => fileName).ToArray();

                foreach (var file in razorFiles)
                {
                    RazorScript script = null;

                    foreach (RazorScript razorScript in _scriptList)
                    {
                        if (razorScript.Path.Equals(file))
                        {
                            script = razorScript;
                        }
                    }

                    if (script == null)
                    {
                        script = AddScript(file);
                    }

                    if (nodes != null)
                    {
                        TreeNode node = new TreeNode(script.Name)
                        {
                            Tag = script
                        };

                        nodes.Add(node);
                    }
                }
            }
            catch
            {
                // ignored
            }

            try
            {

                foreach (string directory in Directory.GetDirectories(path))
                {
                    if (!string.IsNullOrEmpty(directory) && !directory.Equals(".") && !directory.Equals(".."))
                    {
                        if (nodes != null)
                        {
                            TreeNode node = new TreeNode($"[{Path.GetFileName(directory)}]")
                            {
                                Tag = directory
                            };

                            nodes.Add(node);

                            Recurse(node.Nodes, directory);
                        }
                        else
                        {
                            Recurse(null, directory);
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void GetGumpInfo(string[] param)
        {
            Targeting.OneTimeTarget(OnGetItemInfoTarget);
            Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x3B2, 3,
                Language.CliLocName, "System", "Select an item or mobile to view/inspect"));
        }

        private static void OnGetItemInfoTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            Item item = World.FindItem(serial);

            if (item == null)
            {
                Mobile mobile = World.FindMobile(serial);

                if (mobile == null)
                    return;

                MobileInfoGump gump = new MobileInfoGump(mobile);
                gump.SendGump();
            }
            else
            {
                ItemInfoGump gump = new ItemInfoGump(item);
                gump.SendGump();
            }
        }
    }
}