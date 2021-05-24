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
using System.Text.RegularExpressions;

namespace Ultima
{
    public sealed class StringEntry
    {
        [Flags]
        public enum CliLocFlag
        {
            Original = 0x0,
            Custom = 0x1,
            Modified = 0x2
        }

        private string m_Text;

        public int Number { get; private set; }

        public string Text
        {
            get { return m_Text; }
            set
            {
                if (value == null)
                    m_Text = "";
                else
                    m_Text = value;
            }
        }

        public CliLocFlag Flag { get; set; }

        public StringEntry(int number, string text, byte flag)
        {
            Number = number;
            m_Text = text;
            Flag = (CliLocFlag) flag;
        }

        public StringEntry(int number, string text, CliLocFlag flag)
        {
            Number = number;
            m_Text = text;
            Flag = flag;
        }

        // Razor
        private static Regex m_RegEx = new Regex(@"~(\d+)[_\w]+~",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        private string m_FmtTxt;
        private static object[] m_Args = new object[] {"", "", "", "", "", "", "", "", "", "", ""};

        public string Format(params object[] args)
        {
            if (m_FmtTxt == null)
                m_FmtTxt = m_RegEx.Replace(m_Text, @"{$1}");
            for (int i = 0; i < args.Length && i < 10; i++)
                m_Args[i + 1] = args[i];
            return String.Format(m_FmtTxt, m_Args);
        }

        public string SplitFormat(string argstr)
        {
            if (m_FmtTxt == null)
                m_FmtTxt = m_RegEx.Replace(m_Text, @"{$1}");
            string[] args = argstr.Split('\t'); // adds an extra on to the args array
            for (int i = 0; i < args.Length && i < 10; i++)
                m_Args[i + 1] = args[i];
            return String.Format(m_FmtTxt, m_Args);
            /*
            {
                 StringBuilder sb = new StringBuilder();
                 sb.Append( m_FmtTxt );
                 for(int i=0;i<args.Length;i++)
                 {
                      sb.Append( "|" );
                      sb.Append( args[i] == null ? "-null-" : args[i] );
                 }
                 throw new Exception( sb.ToString() );
            }*/
        }
    }
}