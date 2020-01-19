using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormsSyntaxHighlighter.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var syntaxHighlighter = new SyntaxHighlighter(rtbInput);

            // multi-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.DarkSeaGreen, false, true));
            // singlie-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"//.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.Green, false, true));
            // numbers
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\d+\.\d+|\d+"), new SyntaxStyle(Color.Purple));
            // double quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\""([^""]|\""\"")+\"""), new SyntaxStyle(Color.Red));
            // single quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\'([^']|\'\')+\'"), new SyntaxStyle(Color.Salmon));
            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("for", "foreach", "int", "var"), new SyntaxStyle(Color.Blue));
            // keywords2
            syntaxHighlighter.AddPattern(new CaseInsensitivePatternDefinition("public", "partial", "class", "void"), new SyntaxStyle(Color.Navy, true, false));
            // operators
            syntaxHighlighter.AddPattern(new PatternDefinition("+", "-", ">", "<", "&", "|"), new SyntaxStyle(Color.Brown));
        }
    }
}
