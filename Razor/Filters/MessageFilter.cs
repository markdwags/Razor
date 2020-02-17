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

namespace Assistant.Filters
{
    public class AsciiMessageFilter : Filter
    {
        private LocString m_Name;
        private string[] m_Strings;
        private MessageType m_Type;

        private AsciiMessageFilter(LocString name, MessageType type, string[] msgs)
        {
            m_Name = name;
            m_Strings = msgs;
            m_Type = type;
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0x1C}; }
        }

        public override LocString Name
        {
            get { return m_Name; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            if (args.Block)
                return;

            // 0, 1, 2
            Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
            ushort body = p.ReadUInt16(); // 7, 8
            MessageType type = (MessageType) p.ReadByte(); // 9

            if (type != m_Type)
                return;

            ushort hue = p.ReadUInt16(); // 10, 11
            ushort font = p.ReadUInt16();
            string name = p.ReadStringSafe(30);
            string text = p.ReadStringSafe();

            for (int i = 0; i < m_Strings.Length; i++)
            {
                if (text.IndexOf(m_Strings[i]) != -1)
                {
                    args.Block = true;
                    return;
                }
            }
        }
    }

    public class LocMessageFilter : Filter
    {
        public static void Initialize()
        {
        }

        private LocString m_Name;
        private int[] m_Nums;
        private MessageType m_Type;

        private LocMessageFilter(LocString name, MessageType type, int[] msgs)
        {
            m_Name = name;
            m_Nums = msgs;
            m_Type = type;
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0xC1}; }
        }

        public override LocString Name
        {
            get { return m_Name; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            if (args.Block)
                return;

            Serial serial = p.ReadUInt32();
            ushort body = p.ReadUInt16();
            MessageType type = (MessageType) p.ReadByte();
            ushort hue = p.ReadUInt16();
            ushort font = p.ReadUInt16();
            int num = p.ReadInt32();

            // paladin spells
            if (num >= 1060718 && num <= 1060727)
                type = MessageType.Spell;
            if (type != m_Type)
                return;

            for (int i = 0; i < m_Nums.Length; i++)
            {
                if (num == m_Nums[i])
                {
                    args.Block = true;
                    return;
                }
            }
        }
    }
}