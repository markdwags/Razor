#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assistant
{
    public class ObjectPropertyList
    {
        private class OPLEntry
        {
            public int Number = 0;
            public string Args = null;

            public OPLEntry(int num) : this(num, null)
            {
            }

            public OPLEntry(int num, string args)
            {
                Number = num;
                Args = args;
            }
        }

        private List<int> m_StringNums = new List<int>();

        private int m_Hash = 0;
        private List<OPLEntry> m_Content = new List<OPLEntry>();

        private int m_CustomHash = 0;
        private List<OPLEntry> m_CustomContent = new List<OPLEntry>();

        private static Regex m_RegEx = new Regex(@"~(\d+)[_\w]+~", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        private UOEntity m_Owner = null;

        public int Hash
        {
            get { return m_Hash ^ m_CustomHash; }
            set { m_Hash = value; }
        }

        public int ServerHash
        {
            get { return m_Hash; }
        }

        public bool Customized
        {
            get { return m_CustomHash != 0; }
        }

        public ObjectPropertyList(UOEntity owner)
        {
            m_Owner = owner;

            m_StringNums.AddRange(m_DefaultStringNums);
        }

        public UOEntity Owner
        {
            get { return m_Owner; }
        }

        public void Read(PacketReader p)
        {
            m_Content.Clear();

            p.Seek(5, System.IO.SeekOrigin.Begin); // seek to packet data

            p.ReadUInt32(); // serial
            p.ReadByte(); // 0
            p.ReadByte(); // 0
            m_Hash = p.ReadInt32();

            m_StringNums.Clear();
            m_StringNums.AddRange(m_DefaultStringNums);

            while (p.Position < p.Length)
            {
                int num = p.ReadInt32();
                if (num == 0)
                    break;

                m_StringNums.Remove(num);

                short bytes = p.ReadInt16();
                string args = null;
                if (bytes > 0)
                    args = p.ReadUnicodeStringBE(bytes >> 1);

                m_Content.Add(new OPLEntry(num, args));
            }

            for (int i = 0; i < m_CustomContent.Count; i++)
            {
                OPLEntry ent = (OPLEntry) m_CustomContent[i];
                if (m_StringNums.Contains(ent.Number))
                {
                    m_StringNums.Remove(ent.Number);
                }
                else
                {
                    for (int s = 0; s < m_DefaultStringNums.Length; s++)
                    {
                        if (ent.Number == m_DefaultStringNums[s])
                        {
                            ent.Number = GetStringNumber();
                            break;
                        }
                    }
                }
            }
        }

        public void Add(int number)
        {
            if (number == 0)
                return;

            AddHash(number);

            m_CustomContent.Add(new OPLEntry(number));
        }

        private static byte[] m_Buffer = new byte[0];

        public void AddHash(int val)
        {
            m_CustomHash ^= (val & 0x3FFFFFF);
            m_CustomHash ^= (val >> 26) & 0x3F;
        }

        public void Add(int number, string arguments)
        {
            if (number == 0)
                return;

            AddHash(number);
            m_CustomContent.Add(new OPLEntry(number, arguments));
        }

        public void Add(int number, string format, object arg0)
        {
            Add(number, String.Format(format, arg0));
        }

        public void Add(int number, string format, object arg0, object arg1)
        {
            Add(number, String.Format(format, arg0, arg1));
        }

        public void Add(int number, string format, object arg0, object arg1, object arg2)
        {
            Add(number, String.Format(format, arg0, arg1, arg2));
        }

        public void Add(int number, string format, params object[] args)
        {
            Add(number, String.Format(format, args));
        }

        private static int[] m_DefaultStringNums = new int[]
        {
            1042971, // ~1_NOTHING~
            1070722, // ~1_NOTHING~
            1063483, // ~1_MATERIAL~ ~2_ITEMNAME~
            1076228, // ~1_DUMMY~ ~2_DUMMY~
            1060847, // ~1_val~ ~2_val~
            1050039 // ~1_NUMBER~ ~2_ITEMNAME~
            // these are ugly:
            //1062613, // "~1_NAME~" (orange)
            //1049644, // [~1_stuff~]
        };

        private int GetStringNumber()
        {
            if (m_StringNums.Count > 0)
            {
                int num = (int) m_StringNums[0];
                m_StringNums.RemoveAt(0);
                return num;
            }
            else
            {
                return 1049644;
            }
        }

        private const string RazorHTMLFormat = " <CENTER><BASEFONT COLOR=#FF0000>{0}</BASEFONT></CENTER> ";

        public void Add(string text)
        {
            Add(GetStringNumber(), String.Format(RazorHTMLFormat, text));
        }

        public void Add(string format, string arg0)
        {
            Add(GetStringNumber(), String.Format(format, arg0));
        }

        public void Add(string format, string arg0, string arg1)
        {
            Add(GetStringNumber(), String.Format(format, arg0, arg1));
        }

        public void Add(string format, string arg0, string arg1, string arg2)
        {
            Add(GetStringNumber(), String.Format(format, arg0, arg1, arg2));
        }

        public void Add(string format, params object[] args)
        {
            Add(GetStringNumber(), String.Format(format, args));
        }

        public bool Remove(int number)
        {
            for (int i = 0; i < m_Content.Count; i++)
            {
                OPLEntry ent = (OPLEntry) m_Content[i];
                if (ent == null)
                    continue;

                if (ent.Number == number)
                {
                    for (int s = 0; s < m_DefaultStringNums.Length; s++)
                    {
                        if (m_DefaultStringNums[s] == ent.Number)
                        {
                            m_StringNums.Insert(0, ent.Number);
                            break;
                        }
                    }

                    m_Content.RemoveAt(i);
                    AddHash(ent.Number);
                    if (!string.IsNullOrEmpty(ent.Args))
                        AddHash(ent.Args.GetHashCode());

                    return true;
                }
            }

            for (int i = 0; i < m_CustomContent.Count; i++)
            {
                OPLEntry ent = (OPLEntry) m_CustomContent[i];
                if (ent == null)
                    continue;

                if (ent.Number == number)
                {
                    for (int s = 0; s < m_DefaultStringNums.Length; s++)
                    {
                        if (m_DefaultStringNums[s] == ent.Number)
                        {
                            m_StringNums.Insert(0, ent.Number);
                            break;
                        }
                    }

                    m_CustomContent.RemoveAt(i);
                    AddHash(ent.Number);
                    if (!string.IsNullOrEmpty(ent.Args))
                        AddHash(ent.Args.GetHashCode());
                    if (m_CustomContent.Count == 0)
                        m_CustomHash = 0;
                    return true;
                }
            }

            return false;
        }

        public bool Remove(string str)
        {
            string htmlStr = String.Format(RazorHTMLFormat, str);

            /*for ( int i = 0; i < m_Content.Count; i++ )
            {
                 OPLEntry ent = (OPLEntry)m_Content[i];
                 if ( ent == null )
                      continue;

                 for (int s=0;s<m_DefaultStringNums.Length;s++)
                 {
                      if ( ent.Number == m_DefaultStringNums[s] && ( ent.Args == htmlStr || ent.Args == str ) )
                      {
                           m_StringNums.Insert( 0, ent.Number );

                           m_Content.RemoveAt( i );

                           AddHash( ent.Number );
                           if ( ent.Args != null && ent.Args != "" )
                                AddHash( ent.Args.GetHashCode() );
                           return true;
                      }
                 }
            }*/

            for (int i = 0; i < m_CustomContent.Count; i++)
            {
                OPLEntry ent = (OPLEntry) m_CustomContent[i];
                if (ent == null)
                    continue;

                for (int s = 0; s < m_DefaultStringNums.Length; s++)
                {
                    if (ent.Number == m_DefaultStringNums[s] && (ent.Args == htmlStr || ent.Args == str))
                    {
                        m_StringNums.Insert(0, ent.Number);

                        m_CustomContent.RemoveAt(i);

                        AddHash(ent.Number);
                        if (!string.IsNullOrEmpty(ent.Args))
                            AddHash(ent.Args.GetHashCode());
                        return true;
                    }
                }
            }

            return false;
        }

        public Packet BuildPacket()
        {
            Packet p = new Packet(0xD6);

            p.EnsureCapacity(128);

            p.Write((short) 0x01);
            p.Write((uint) (m_Owner != null ? m_Owner.Serial : Serial.Zero));
            p.Write((byte) 0);
            p.Write((byte) 0);
            p.Write((uint) (m_Hash ^ m_CustomHash));

            foreach (OPLEntry ent in m_Content)
            {
                if (ent != null && ent.Number != 0)
                {
                    p.Write((int) ent.Number);
                    if (!string.IsNullOrEmpty(ent.Args))
                    {
                        int byteCount = Encoding.Unicode.GetByteCount(ent.Args);

                        if (byteCount > m_Buffer.Length)
                            m_Buffer = new byte[byteCount];

                        byteCount = Encoding.Unicode.GetBytes(ent.Args, 0, ent.Args.Length, m_Buffer, 0);

                        p.Write((short) byteCount);
                        p.Write(m_Buffer, 0, byteCount);
                    }
                    else
                    {
                        p.Write((short) 0);
                    }
                }
            }
            
            foreach (OPLEntry ent in m_CustomContent)
            {
                try
                {
                    if (ent != null && ent.Number != 0)
                    {
                        string arguments = ent.Args;

                        p.Write((int) ent.Number);

                        if (string.IsNullOrEmpty(arguments))
                            arguments = " ";
                        arguments += "\t ";

                        if (!string.IsNullOrEmpty(arguments))
                        {
                            int byteCount = Encoding.Unicode.GetByteCount(arguments);

                            if (byteCount > m_Buffer.Length)
                                m_Buffer = new byte[byteCount];

                            byteCount = Encoding.Unicode.GetBytes(arguments, 0, arguments.Length, m_Buffer, 0);

                            p.Write((short) byteCount);
                            p.Write(m_Buffer, 0, byteCount);
                        }
                        else
                        {
                            p.Write((short) 0);
                        }
                    }
                }
                catch
                {
                }
            }

            p.Write((int) 0);

            return p;
        }

        public Dictionary<string, string> ExportProperties()
        {
            var values = new Dictionary<string, string>();
            for (int i = 0; i < m_Content.Count; i++)
            {
                OPLEntry ent = m_Content[i];
                if (ent == null) continue;

                var response = ConvertContentToKeyValuePair(ent.Number, ent.Args);
                if (response.key == null) continue;

                response.key = response.key?.Trim();
                response.value = response.value?.Trim();

                // Assume the first returned value is the "name"
                if (i == 0)
                {
                    values.Add("$name", response.key);

                    // Handle special case. Ex: {amount} {name}
                    if (!string.IsNullOrWhiteSpace(response.value) && int.TryParse(response.value, out _))
                    {
                        var parsedName = UnfoldArgClilocNumbers(ent.Args);
                        values.Add("$amount", response.value);
                    }

                    continue;
                }

                // Special case: Containers are being passed in.
                if (response.key?.Contains("stones") == true) continue;

                // Clean up the property value
                response.key = response.key.Trim(':', ' ');

                // Move the % to the property if it exists
                if (response.value?.EndsWith("%") == true)
                {
                    response.value = response.value.TrimEnd('%');
                    response.key += " %";
                }

                // If a duplicate would be created, include the cliloc number instead
                var key = values.TryGetValue(response.key, out var existing)
                    ? $"{response.key} ({ent.Number}" // Hope this never happens ... but worth at least getting it out
                    : response.key;

                values.Add(key, response.value);
            }

            return values;
        }

        private string UnfoldArgClilocNumbers(string args)
        {
            if (string.IsNullOrWhiteSpace(args)) return null;

            // Replace all Cliloc numbers that are passed in.
            // Ex: Exceptional #{resource_cliloc} #{armor_cliloc} -- Exceptional {barbed} {leather tunic}

            var builder = new StringBuilder();
            var isNumericRun = false;
            var isCharRun = false;
            int startIndex = 0;
            for (var i = 0; i < args.Length; i++)
            {
                var c = args[i];

                if (isNumericRun)
                {
                    if (char.IsNumber(c)) continue;

                    // Look up value
                    var intValue = int.Parse(args.Substring(startIndex + 1, i - startIndex));
                    var clilocValue = Language.GetClilocUnformatted(intValue);

                    // Copy value
                    builder.Append(clilocValue);
                    builder.Append(c);

                    isCharRun = isNumericRun = false;

                    continue;
                }

                if (isCharRun)
                {
                    // Multiple args are separated by '\t'. Break into a new run.
                    if (c != '\t') continue;

                    // Copy value
                    builder.Append(args.Substring(startIndex, i - startIndex));
                    builder.Append(c);

                    isCharRun = isNumericRun = false;

                    continue;
                }

                // A '#' indicates it's a Cliloc number
                isNumericRun = c == '#';
                isCharRun = !isNumericRun;
                startIndex = i;
            }

            // Flush final pass
            if (isNumericRun)
            {
                var intValue = int.Parse(args.Substring(startIndex + 1));
                var clilocValue = Language.GetClilocUnformatted(intValue);

                builder.Append(clilocValue);
            }
            else if (isCharRun)
            {
                builder.Append(args.Substring(startIndex));
            }

            return builder.ToString();
        }

        private (string key, string value) ConvertContentToKeyValuePair(int initialClilocNumber, string args)
        {
            var unformattedClilocValue = Language.GetClilocUnformatted(initialClilocNumber);
            if (string.IsNullOrWhiteSpace(unformattedClilocValue)) return (null, null); // Unknown value

            // Ex: {Axe Of The Heavens}, {Mage Armor}
            // If there are no variables, return the whole value
            if (string.IsNullOrWhiteSpace(args)) return (unformattedClilocValue, null);

            var matches = m_RegEx.Matches(unformattedClilocValue);
            if (matches.Count == 0) return (unformattedClilocValue, null); // Weird case, we have Args but nowhere to put them.

            // Unfold
            args = UnfoldArgClilocNumbers(args);

            // Physical resist: {number}
            // Crafted by {string}
            if (matches.Count == 1) return (unformattedClilocValue.Replace(matches[0].Value, ""), args);

            var multipleArgs = args.Split('\t');
            var formattedClilocValue = unformattedClilocValue;
            for (int i = 0; i < matches.Count; i++)
            {
                // {skill} +{number} -- Number removed
                // {number} {resource} -- Number removed
                // Exceptional #{resource_cliloc} #{armor_cliloc} -- Exceptional {barbed} {leather tunic} -- Nothing removed

                // Rip out numeric args and replace the rest with their values
                // var replacementValue = int.TryParse(multipleArgs[i], out _) ? "" : multipleArgs[i];
                formattedClilocValue = formattedClilocValue.Replace(matches[i].Value, multipleArgs[i]); // AKA "PropertyValue"
            }

            (var flippingIndex, var firstValueNumeric) = ParseFormattedClilocValue(formattedClilocValue);

            // Extract property/value strings
            string value = null;
            string property = null;
            if (firstValueNumeric)
            {
                value = formattedClilocValue.Substring(0, flippingIndex);
                property = formattedClilocValue.Substring(flippingIndex);
            }
            else
            {
                value = formattedClilocValue.Substring(flippingIndex);
                property = formattedClilocValue.Substring(0, flippingIndex);
            }

            return (property, value);
        }

        private (int flippingIndex, bool firstValueNumeric) ParseFormattedClilocValue(string value)
        {
            /* Example cases
                 {amount} {name} - value / label
                 {skill} +{value} - label / value
                 {property}: {value}
                 {property} {value_1} / {value_2}
            */

            bool isNumericRun = false;
            bool isCharRun = false;
            int i = 0;

            for (i = 0; i < value.Length; i++)
            {
                var c = value[i];
                var isNumber = char.IsNumber(c);

                if (isNumericRun)
                {
                    if (isNumber) continue;
                    break;
                }

                if (isCharRun)
                {
                    if (!isNumber) continue;
                    break;
                }

                // Figure out which comes first
                isNumericRun = isNumber;
                isCharRun = !isNumericRun;
            }

            return (i, isNumericRun);
        }
 }

    public class OPLInfo : Packet
    {
        public OPLInfo(Serial ser, int hash) : base(0xDC, 9)
        {
            Write((uint) ser);
            Write((int) hash);
        }
    }
}