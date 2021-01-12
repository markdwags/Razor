using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace FastColoredTextBoxNS
{
    public class SyntaxHighlighter : IDisposable
    {
        //styles
        protected static readonly Platform platformType = PlatformType.GetOperationSystemPlatform();
        public readonly Style BlueBoldStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        public readonly Style BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        public readonly Style BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
        public readonly Style BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        public readonly Style GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public readonly Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        public readonly Style MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        public readonly Style MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
        public readonly Style RedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
        public readonly Style BlackStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        public readonly Style CrimsonStyle = new TextStyle(Brushes.Crimson, null, FontStyle.Regular);
        public readonly Style DarkOrangeStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Regular);
        public readonly Style DodgerBlueStyle = new TextStyle(Brushes.DodgerBlue, null, FontStyle.Regular);

        public readonly Style DarkOrangeBoldStyle =
            new TextStyle(Brushes.DarkOrange, null, FontStyle.Bold | FontStyle.Regular);

        public readonly Style RazorDarkKeywords =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 253, 134, 30)), null, FontStyle.Regular);

        public readonly Style RazorDarkCommands =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 43, 144, 175)), null, FontStyle.Regular);

        public readonly Style RazorDarkStrings =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 181, 241, 9)), null, FontStyle.Regular);

        public readonly Style RazorDarkOperators =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 253, 134, 30)), null, FontStyle.Regular);

        public readonly Style RazorDarkNumbers =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 174, 129, 255)), null, FontStyle.Regular);

        public readonly Style RazorDarkComments =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 150, 150, 150)), null, FontStyle.Regular);

        public readonly Style RazorDarkSerial =
            new TextStyle(new SolidBrush(Color.FromArgb(255, 249, 37, 77)), null, FontStyle.Regular);


        //
        protected readonly Dictionary<string, SyntaxDescriptor> descByXMLfileNames =
            new Dictionary<string, SyntaxDescriptor>();

        protected readonly List<Style> resilientStyles = new List<Style>(5);

        protected Regex RazorCommentRegex;
        protected Regex RazorSerialRegex;
        protected Regex RazorOperatorRegex;
        protected Regex RazorLayerRegex;
        protected Regex RazorExpressionRegex;
        protected Regex RazorCommandRegex;
        protected Regex RazorKeywordRegex;
        protected Regex RazorNumberRegex;
        protected Regex RazorStringRegex1;
        protected Regex RazorStringRegex2;

        protected FastColoredTextBox currentTb;

        public static RegexOptions RegexCompiledOption
        {
            get
            {
                if (platformType == Platform.X86)
                    return RegexOptions.Compiled;
                else
                    return RegexOptions.None;
            }
        }

        public SyntaxHighlighter(FastColoredTextBox currentTb)
        {
            this.currentTb = currentTb;
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (SyntaxDescriptor desc in descByXMLfileNames.Values)
                desc.Dispose();
        }

        #endregion

        /// <summary>
        /// Highlights syntax for given language
        /// </summary>
        public virtual void HighlightSyntax(Language language, Range range)
        {
            RazorSyntaxHighlight(range);
        }

        /// <summary>
        /// Highlights syntax for given XML description file
        /// </summary>
        public virtual void HighlightSyntax(string XMLdescriptionFile, Range range)
        {
            SyntaxDescriptor desc = null;
            if (!descByXMLfileNames.TryGetValue(XMLdescriptionFile, out desc))
            {
                var doc = new XmlDocument();
                string file = XMLdescriptionFile;
                if (!File.Exists(file))
                    file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));

                doc.LoadXml(File.ReadAllText(file));
                desc = ParseXmlDescription(doc);
                descByXMLfileNames[XMLdescriptionFile] = desc;
            }

            HighlightSyntax(desc, range);
        }

        public virtual void AutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            RazorAutoIndentNeeded(sender, args);
        }

        /// <summary>
        /// Uses the given <paramref name="doc"/> to parse a XML description and adds it as syntax descriptor. 
        /// The syntax descriptor is used for highlighting when 
        /// <list type="bullet">
        ///     <item>Language property of FCTB is set to <see cref="Language.Custom"/></item>
        ///     <item>DescriptionFile property of FCTB has the same value as the method parameter <paramref name="descriptionFileName"/></item>
        /// </list>
        /// </summary>
        /// <param name="descriptionFileName">Name of the description file</param>
        /// <param name="doc">XmlDocument to parse</param>
        public virtual void AddXmlDescription(string descriptionFileName, XmlDocument doc)
        {
            SyntaxDescriptor desc = ParseXmlDescription(doc);
            descByXMLfileNames[descriptionFileName] = desc;
        }

        /// <summary>
        /// Adds the given <paramref name="style"/> as resilient style. A resilient style is additionally available when highlighting is 
        /// based on a syntax descriptor that has been derived from a XML description file. In the run of the highlighting routine 
        /// the styles used by the FCTB are always dropped and replaced with the (initial) ones from the syntax descriptor. Resilient styles are 
        /// added afterwards and can be used anyway. 
        /// </summary>
        /// <param name="style">Style to add</param>
        public virtual void AddResilientStyle(Style style)
        {
            if (resilientStyles.Contains(style)) return;
            currentTb.CheckStylesBufferSize(); // Prevent buffer overflow
            resilientStyles.Add(style);
        }

        public static SyntaxDescriptor ParseXmlDescription(XmlDocument doc)
        {
            var desc = new SyntaxDescriptor();
            XmlNode brackets = doc.SelectSingleNode("doc/brackets");
            if (brackets != null)
            {
                if (brackets.Attributes["left"] == null || brackets.Attributes["right"] == null ||
                    brackets.Attributes["left"].Value == "" || brackets.Attributes["right"].Value == "")
                {
                    desc.leftBracket = '\x0';
                    desc.rightBracket = '\x0';
                }
                else
                {
                    desc.leftBracket = brackets.Attributes["left"].Value[0];
                    desc.rightBracket = brackets.Attributes["right"].Value[0];
                }

                if (brackets.Attributes["left2"] == null || brackets.Attributes["right2"] == null ||
                    brackets.Attributes["left2"].Value == "" || brackets.Attributes["right2"].Value == "")
                {
                    desc.leftBracket2 = '\x0';
                    desc.rightBracket2 = '\x0';
                }
                else
                {
                    desc.leftBracket2 = brackets.Attributes["left2"].Value[0];
                    desc.rightBracket2 = brackets.Attributes["right2"].Value[0];
                }

                if (brackets.Attributes["strategy"] == null || brackets.Attributes["strategy"].Value == "")
                    desc.bracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2;
                else
                    desc.bracketsHighlightStrategy =
                        (BracketsHighlightStrategy) Enum.Parse(typeof(BracketsHighlightStrategy),
                            brackets.Attributes["strategy"].Value);
            }

            var styleByName = new Dictionary<string, Style>();

            foreach (XmlNode style in doc.SelectNodes("doc/style"))
            {
                Style s = ParseStyle(style);
                styleByName[style.Attributes["name"].Value] = s;
                desc.styles.Add(s);
            }

            foreach (XmlNode rule in doc.SelectNodes("doc/rule"))
                desc.rules.Add(ParseRule(rule, styleByName));
            foreach (XmlNode folding in doc.SelectNodes("doc/folding"))
                desc.foldings.Add(ParseFolding(folding));

            return desc;
        }

        protected static FoldingDesc ParseFolding(XmlNode foldingNode)
        {
            var folding = new FoldingDesc();
            //regex
            folding.startMarkerRegex = foldingNode.Attributes["start"].Value;
            folding.finishMarkerRegex = foldingNode.Attributes["finish"].Value;
            //options
            XmlAttribute optionsA = foldingNode.Attributes["options"];
            if (optionsA != null)
                folding.options = (RegexOptions) Enum.Parse(typeof(RegexOptions), optionsA.Value);

            return folding;
        }

        protected static RuleDesc ParseRule(XmlNode ruleNode, Dictionary<string, Style> styles)
        {
            var rule = new RuleDesc();
            rule.pattern = ruleNode.InnerText;
            //
            XmlAttribute styleA = ruleNode.Attributes["style"];
            XmlAttribute optionsA = ruleNode.Attributes["options"];
            //Style
            if (styleA == null)
                throw new Exception("Rule must contain style name.");
            if (!styles.ContainsKey(styleA.Value))
                throw new Exception("Style '" + styleA.Value + "' is not found.");
            rule.style = styles[styleA.Value];
            //options
            if (optionsA != null)
                rule.options = (RegexOptions) Enum.Parse(typeof(RegexOptions), optionsA.Value);

            return rule;
        }

        protected static Style ParseStyle(XmlNode styleNode)
        {
            XmlAttribute typeA = styleNode.Attributes["type"];
            XmlAttribute colorA = styleNode.Attributes["color"];
            XmlAttribute backColorA = styleNode.Attributes["backColor"];
            XmlAttribute fontStyleA = styleNode.Attributes["fontStyle"];
            XmlAttribute nameA = styleNode.Attributes["name"];
            //colors
            SolidBrush foreBrush = null;
            if (colorA != null)
                foreBrush = new SolidBrush(ParseColor(colorA.Value));
            SolidBrush backBrush = null;
            if (backColorA != null)
                backBrush = new SolidBrush(ParseColor(backColorA.Value));
            //fontStyle
            FontStyle fontStyle = FontStyle.Regular;
            if (fontStyleA != null)
                fontStyle = (FontStyle) Enum.Parse(typeof(FontStyle), fontStyleA.Value);

            return new TextStyle(foreBrush, backBrush, fontStyle);
        }

        protected static Color ParseColor(string s)
        {
            if (s.StartsWith("#"))
            {
                if (s.Length <= 7)
                    return Color.FromArgb(255,
                        Color.FromArgb(Int32.Parse(s.Substring(1), NumberStyles.AllowHexSpecifier)));
                else
                    return Color.FromArgb(Int32.Parse(s.Substring(1), NumberStyles.AllowHexSpecifier));
            }
            else
                return Color.FromName(s);
        }

        public void HighlightSyntax(SyntaxDescriptor desc, Range range)
        {
            //set style order
            range.tb.ClearStylesBuffer();
            for (int i = 0; i < desc.styles.Count; i++)
                range.tb.Styles[i] = desc.styles[i];
            // add resilient styles
            int l = desc.styles.Count;
            for (int i = 0; i < resilientStyles.Count; i++)
                range.tb.Styles[l + i] = resilientStyles[i];
            //brackets
            char[] oldBrackets = RememberBrackets(range.tb);
            range.tb.LeftBracket = desc.leftBracket;
            range.tb.RightBracket = desc.rightBracket;
            range.tb.LeftBracket2 = desc.leftBracket2;
            range.tb.RightBracket2 = desc.rightBracket2;
            //clear styles of range
            range.ClearStyle(desc.styles.ToArray());
            //highlight syntax
            foreach (RuleDesc rule in desc.rules)
                range.SetStyle(rule.style, rule.Regex);
            //clear folding
            range.ClearFoldingMarkers();
            //folding markers
            foreach (FoldingDesc folding in desc.foldings)
                range.SetFoldingMarkers(folding.startMarkerRegex, folding.finishMarkerRegex, folding.options);

            //
            RestoreBrackets(range.tb, oldBrackets);
        }

        protected void RestoreBrackets(FastColoredTextBox tb, char[] oldBrackets)
        {
            tb.LeftBracket = oldBrackets[0];
            tb.RightBracket = oldBrackets[1];
            tb.LeftBracket2 = oldBrackets[2];
            tb.RightBracket2 = oldBrackets[3];
        }

        protected char[] RememberBrackets(FastColoredTextBox tb)
        {
            return new[] {tb.LeftBracket, tb.RightBracket, tb.LeftBracket2, tb.RightBracket2};
        }

        public void InitStyleSchema(Language lang)
        {
            CommentStyle = RazorDarkComments;
            StringStyle = RazorDarkStrings;
            NumberStyle = RazorDarkNumbers;
            KeywordStyle = RazorDarkKeywords;
            FunctionsStyle = RedStyle;
            RazorCommandStyle = RazorDarkCommands;
            RazorSerialStyle = RazorDarkSerial;
            RazorLayerStyle = RazorDarkNumbers;
            RazorExpressionStyle = RazorDarkSerial;
        }

        protected void InitRazorRegex()
        {
            RazorStringRegex1 = new Regex("\"[^\"\\\\]*(\\\\.[^\"\\\\]*)*\"", RegexCompiledOption);
            RazorStringRegex2 = new Regex("'[^'\\\\]*(\\\\.[^'\\\\]*)*'", RegexCompiledOption);

            RazorCommentRegex = new Regex("(//.*$|#.*$)", RegexOptions.Multiline | RegexCompiledOption);

            RazorSerialRegex = new Regex(@"0x[\da-fA-F]*");

            RazorNumberRegex = new Regex(@"\b[+-]?[0-9]+(?:\.[0-9]+)?(?:[eE][+-]?[0-9]+)?\b", RegexCompiledOption);
            RazorKeywordRegex =
                new Regex(
                    @"\b(if|elseif|else|endif|while|endwhile|for|endfor|break|continue|not|and|or|stop|replay|loop)\b",
                    RegexCompiledOption);

            RazorCommandRegex =
                new Regex(
                    @"\b(attack|cast|dress|undress|dressconfig|target|targettype|targetrelloc|dress|drop|waitfortarget|wft|dclick|dclicktype|dclickvar|usetype|useobject|droprelloc|lift|lifttype|waitforgump|gumpresponse|gumpclose|menu|menuresponse|waitformenu|promptresponse|waitforprompt|hotkey|say|msg|overhead|sysmsg|wait|pause|waitforstat|setability|setlasttarget|lasttarget|setvar|skill|useskill|walk|script|useonce|organizer|organize|org|restock|scav|scavenger|potion|clearsysmsg|clearjournal|whisper|yell|guild|alliance|emote|waitforsysmsg|wfsysmsg)\b",
                    RegexCompiledOption);

            RazorLayerRegex =
                new Regex(
                    @"\b(RightHand|LeftHand|Shoes|Pants|Shirt|Head|Gloves|Ring|Talisman|Neck|Hair|Waist|InnerTorso|Bracelet|FacialHair|MiddleTorso|Earrings|Arms|Cloak|Backpack|OuterTorso|OuterLegs|InnerLegs|backpack|true|false)\b",
                    RegexCompiledOption);

            RazorExpressionRegex =
                new Regex(
                    @"\b(queued|position|insysmsg|insysmessage|findtype|findbuff|finddebuff|stam|maxstam|hp|maxhp|maxhits|hits|mana|maxmana|str|dex|int|poisoned|hidden|mounted|rhandempty|lhandempty|skill|count|counter|weight|dead)\b",
                    RegexCompiledOption);
        }

        public virtual void RazorSyntaxHighlight(Range range)
        {
            range.tb.CommentPrefix = "//";

            range.tb.LeftBracket = '(';
            range.tb.RightBracket = ')';
            range.tb.LeftBracket2 = '[';
            range.tb.RightBracket2 = ']';
            range.tb.BracketsHighlightStrategy = BracketsHighlightStrategy.Strategy1;

            //range.tb.AutoIndentCharsPatterns = @"^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>.+)";

            //clear style of changed range
            range.ClearStyle(StringStyle, CommentStyle, NumberStyle, KeywordStyle, FunctionsStyle);

            InitRazorRegex();

            //string highlighting
            range.SetStyle(StringStyle, RazorStringRegex1);
            range.SetStyle(StringStyle, RazorStringRegex2);

            //comment highlighting
            range.SetStyle(CommentStyle, RazorCommentRegex);

            //number highlighting
            range.SetStyle(NumberStyle, RazorNumberRegex);

            //keyword highlighting
            range.SetStyle(KeywordStyle, RazorKeywordRegex);

            // Razor highlight
            range.SetStyle(RazorCommandStyle, RazorCommandRegex);
            range.SetStyle(RazorSerialStyle, RazorSerialRegex);
            range.SetStyle(RazorLayerStyle, RazorLayerRegex);
            range.SetStyle(RazorExpressionStyle, RazorExpressionRegex);
            //range.SetStyle(RazorOperatorStyle, RazorOperatorsRegEx);

            //clear folding markers
            range.ClearFoldingMarkers();
        }

        protected void RazorAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            if (Regex.IsMatch(args.LineText, @"^[^""']*\:"))
            {
                args.ShiftNextLines = args.TabLength;
            }

            if (Regex.IsMatch(args.PrevLineText, @"^\s*(if|for|while|elseif|[\}\s]*else)\b[^{]*$"))
                if (!Regex.IsMatch(args.PrevLineText, @"(;\s*$)|(;\s*//)")) //operator is unclosed
                {
                    args.Shift = args.TabLength;
                }
        }

        #region Styles

        /// <summary>
        /// String style
        /// </summary>
        public Style StringStyle { get; set; }

        /// <summary>
        /// Comment style
        /// </summary>
        public Style CommentStyle { get; set; }

        /// <summary>
        /// Number style
        /// </summary>
        public Style NumberStyle { get; set; }

        /// <summary>
        /// Keyword style
        /// </summary>
        public Style KeywordStyle { get; set; }

      
        /// <summary>
        /// SQL Functions style
        /// </summary>
        public Style FunctionsStyle { get; set; }

        public Style RazorCommandStyle { get; set; }
        public Style RazorSerialStyle { get; set; }
        public Style RazorLayerStyle { get; set; }
        public Style RazorExpressionStyle { get; set; }
        public Style RazorOperatorStyle { get; set; }

        #endregion
    }

    /// <summary>
    /// Language
    /// </summary>
    public enum Language
    {
        Razor
    }
}