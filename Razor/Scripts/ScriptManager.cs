using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using UOSteam;

namespace Assistant.Scripts
{
    public static class ScriptManager
    {
        public static bool Recording { get; set; }

        public static string ScriptPath
        {
            get { return $"{Config.GetInstallDirectory()}\\Scripts"; }
        }

        private static FastColoredTextBox _scriptEditor { get; set; }

        private class ScriptTimer : Timer
        {
            // Only run scripts once every 25ms to avoid spamming.
            public ScriptTimer() : base(TimeSpan.FromMilliseconds(25), TimeSpan.FromMilliseconds(25))
            {
            }

            protected override void OnTick()
            {
                Interpreter.ExecuteScripts();

                /*if (Interpreter.ScriptCount > 0)
                {
                    int highlightLine = _scriptEditor.LinesCount - (Interpreter.ScriptCount - 1);

                    if (highlightLine < _scriptEditor.LinesCount - 1)
                        SetHighlightLine(highlightLine, Color.Yellow);
                }*/
            }
        }

        private static ScriptTimer _timer { get; }

        static ScriptManager()
        {
            _timer = new ScriptTimer();
        }

        public static void SetControls(FastColoredTextBox scriptEditor)
        {
            _scriptEditor = scriptEditor;
        }

        public static void OnLogin()
        {
            Commands.Register();
            Aliases.Register();
            Expressions.Register();

            _timer.Start();
        }

        public static void OnLogout()
        {
            _timer.Stop();
        }

        public static void StartEngine()
        {
            _timer.Start();
        }

        public static void StopEngine()
        {
            _timer.Stop();
        }

        public static string[] GetScripts()
        {
            return Directory.GetFiles(ScriptPath, "*.razor");
        }

        public static bool AddToScript(string command)
        {
            if (Recording)
            {
                _scriptEditor?.AppendText(command + Environment.NewLine);

                return true;
            }

            return false;
        }

        public static void Error(string message, string scriptname = "")
        {
            World.Player?.SendMessage(MsgLevel.Error, $"Script '{scriptname}' error => {message}");
        }

        private delegate void SetHighlightLineDelegate(int iline, Color color);

        private static void SetHighlightLine(int iline, Color background)
        {
            for (int i = 0; i < _scriptEditor.LinesCount; i++)
            {
                _scriptEditor[i].BackgroundBrush = _scriptEditor.BackBrush;
            }

            _scriptEditor[iline].BackgroundBrush = new SolidBrush(background);
            _scriptEditor.Invalidate();
        }

        private static FastColoredTextBoxNS.AutocompleteMenu _autoCompleteMenu;

        public static void InitScriptEditor()
        {
            _autoCompleteMenu = new AutocompleteMenu(_scriptEditor);
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
                "waitforstat", "setability", "setlasttarget", "lasttarget", "setvar", "skill", "useskill", "walk"
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

            _scriptEditor.Language = FastColoredTextBoxNS.Language.Razor;
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


        private static TimeSpan _pauseDuration;
        private static DateTime _startPause = DateTime.MaxValue;

        public static bool PauseComplete(int ms = 30000)
        {
            if (_startPause == DateTime.MaxValue) // no timer set
            {
                _startPause = DateTime.UtcNow;
                _pauseDuration = TimeSpan.FromMilliseconds(ms);

                return false; // we want to start pausing
            }

            if (_startPause + _pauseDuration < DateTime.UtcNow) // timer is set, has it elapsed?
            {
                _startPause = DateTime.MaxValue;
                _pauseDuration = TimeSpan.FromMilliseconds(ms);
                return true; //pause limit succeeded
            }

            return false; // keep on pausing
        }
    }
}