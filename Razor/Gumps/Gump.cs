/* Copyright (C) 2009 Matthew Geyer
 * 
 * This file is part of UO Machine.
 * 
 * UO Machine is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * UO Machine is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assistant;
using Assistant.Core.Gumps;
using Assistant.Network;

namespace Assistant.Gumps
{
    public class Gump
    {
        private static readonly Random _random = new Random();

        private static readonly byte[] BeginLayout = StringToBuffer("{ ");
        private static readonly byte[] EndLayout = StringToBuffer(" }");

        private static readonly byte[] NoMove = StringToBuffer("{ nomove }");
        private static readonly byte[] NoClose = StringToBuffer("{ noclose }");
        private static readonly byte[] NoDispose = StringToBuffer("{ nodispose }");
        private static readonly byte[] NoResize = StringToBuffer("{ noresize }");

        private static readonly byte[] True = StringToBuffer(" 1");
        private static readonly byte[] False = StringToBuffer(" 0");

        private static readonly byte[] BeginTextSeparator = StringToBuffer(" @");
        private static readonly byte[] EndTextSeparator = StringToBuffer("@");

        public readonly string Layout;
        public readonly GumpPage[] Pages;

        private List<GumpElement> _elements;
        private List<string> _strings;
        public int Serial;
        public int X;
        public int Y;

        public Gump(int x, int y, uint id, int serial, string layout, string[] strings, GumpElement[] elements,
            GumpPage[] pages)
        {
            X = x;
            Y = y;
            ID = id;
            Serial = serial;
            Layout = layout;
            Strings = strings;
            GumpElements = elements;
            Pages = pages;

            foreach (GumpPage gp in pages)
            {
                gp.ParentGump = this;
            }
        }

        public Gump(int x, int y, int serial = 0, uint id = 0)
        {
            X = x;
            Y = y;

            Serial = serial;
            ID = id;

            string fullName = GetType().FullName;

            if (ID == 0 && fullName != null)
            {
                ID = (uint) fullName.GetHashCode();
            }

            if (Serial == 0 && fullName != null)
            {
                uint hashCode = (uint) fullName.GetHashCode();

                hashCode &= 0x0000FFFF;
                hashCode <<= 8;
                hashCode |= (uint) 0xFF << 24;

                Serial = (int) (hashCode + _random.Next(0, 0xFF));
            }

            _elements = new List<GumpElement>();
            _strings = new List<string>();
        }

        public bool Closable { get; set; }

        public bool Disposable { get; set; }

        public GumpElement[] GumpElements
        {
            get => _elements.ToArray();
            set
            {
                _elements = new List<GumpElement>();

                for (int i = 0; i < value.Length; i++)
                {
                    _elements.Add(value[i]);
                }
            }
        }

        public uint ID { get; }

        public bool Movable { get; set; }

        public bool Resizable { get; set; }

        public string[] Strings
        {
            get => _strings.ToArray();
            set
            {
                _strings = new List<string>();

                for (int i = 0; i < value.Length; i++)
                {
                    _strings.Add(value[i]);
                }
            }
        }

        /// <summary>
        ///     Get array of GumpElements which match the specified ElementType from all pages.
        /// </summary>
        public GumpElement[] GetElementsByType(ElementType type)
        {
            List<GumpElement> elementList = new List<GumpElement>();

            if (GumpElements != null)
            {
                elementList.AddRange(GumpElements.Where(ge => ge.Type == type));
            }

            if (Pages != null)
            {
                elementList.AddRange(from p in Pages from ge in p.GumpElements where ge.Type == type select ge);
            }

            return elementList.ToArray();
        }

        /// <summary>
        ///     Get the GumpElement with the specified ID.  Searches all pages/elements.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetElementByID(int id, out GumpElement element)
        {
            if (GumpElements != null)
            {
                foreach (GumpElement ge in GumpElements)
                {
                    if (ge.ElementID == id)
                    {
                        element = ge;
                        return true;
                    }
                }
            }

            if (Pages != null)
            {
                foreach (GumpPage p in Pages)
                {
                    foreach (GumpElement ge in p.GumpElements)
                    {
                        if (ge.ElementID == id)
                        {
                            element = ge;
                            return true;
                        }
                    }
                }
            }

            element = null;
            return false;
        }

        /// <summary>
        ///     Get the GumpElement nearest to the specified GumpElement.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(GumpElement source, out GumpElement element)
        {
            GumpElement nearest = null;
            double closest = 0;

            if (source.ParentPage != null)
            {
                return source.ParentPage.GetNearestElement(source, out element);
            }

            foreach (GumpElement ge in GumpElements)
            {
                if (ge == source)
                {
                    continue;
                }

                double distance = Utility.Distance(source.X, source.Y, ge.X, ge.Y);

                if (nearest == null)
                {
                    closest = distance;
                    nearest = ge;
                }
                else
                {
                    if (distance < closest)
                    {
                        closest = distance;
                        nearest = ge;
                    }
                }
            }

            element = nearest;
            return nearest != null;
        }

        /// <summary>
        ///     Get nearest GumpElement to source, but only if it's ElementType is contained in the include list.
        /// </summary>
        /// <param name="source">Source GumpElement</param>
        /// <param name="includeTypes">Array of ElementTypes which specifies valid GumpElements to search.</param>
        /// <param name="element">GumpElement (out).</param>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(GumpElement source, ElementType[] includeTypes, out GumpElement element)
        {
            GumpElement nearest = null;
            double closest = 0;

            if (source.ParentPage != null)
            {
                return source.ParentPage.GetNearestElement(source, includeTypes, out element);
            }

            foreach (GumpElement ge in GumpElements)
            {
                if (ge == source)
                {
                    continue;
                }

                bool found = includeTypes.Any(et => ge.Type == et);

                if (!found)
                {
                    continue;
                }

                double distance = Utility.Distance(source.X, source.Y, ge.X, ge.Y);

                if (nearest == null)
                {
                    closest = distance;
                    nearest = ge;
                }
                else
                {
                    if (distance < closest)
                    {
                        closest = distance;
                        nearest = ge;
                    }
                }
            }

            element = nearest;
            return nearest != null;
        }

        public bool GetElementByXY(int x, int y, out GumpElement gumpElement)
        {
            gumpElement = null;

            if (GumpElements == null)
            {
                return false;
            }

            GumpElement element = GumpElements.FirstOrDefault(m => m.X == x && m.Y == y);

            if (element != null)
            {
                gumpElement = element;
            }

            return gumpElement != null;
        }

        public GumpElement GetElementByXY(int x, int y)
        {
            return GetElementByXY(x, y, out GumpElement element) ? element : null;
        }

        public bool GetElementByCliloc(int cliloc, out GumpElement gumpElement)
        {
            gumpElement = null;

            if (GumpElements == null)
            {
                return false;
            }

            GumpElement element = GumpElements.FirstOrDefault(m => m.Cliloc == cliloc);

            if (element != null)
            {
                gumpElement = element;
            }

            return gumpElement != null;
        }

        public GumpElement GetElementByCliloc(int cliloc)
        {
            return GetElementByCliloc(cliloc, out GumpElement element) ? element : null;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ Layout?.GetHashCode() ?? 0;
        }

        public virtual void OnResponse(int buttonID, int[] switches, GumpTextEntry[] textEntries = null)
        {
        }

        public virtual void SendGump()
        {
            byte[] bytes = Compile();

            World.Player.InternalGumps.Add(this);

            Client.Instance.SendPacketToClient(bytes, bytes.Length);
        }

        public void CloseGump()
        {
            Client.Instance.SendToClient(new CloseGump(ID));
        }

        public void Add(GumpElement e)
        {
            e.ParentGump = this;
            _elements.Add(e);
        }

        public void AddPage(int page)
        {
            GumpElement ge = new GumpElement {Type = ElementType.page, PageNumber = page};
            Add(ge);
        }

        public void AddAlphaRegion(int x, int y, int width, int height)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.checkertrans,
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            Add(ge);
        }

        public void AddBackground(int x, int y, int width, int height, int gumpID)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.resizepic,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                ElementID = gumpID
            };
            Add(ge);
        }

        public void AddButton(int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type, int param)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.button,
                X = x,
                Y = y,
                InactiveID = normalID,
                ActiveID = pressedID,
                ButtonType = (int) type,
                ElementID = buttonID,
                Param = param
            };
            Add(ge);
        }

        public void AddCheck(int x, int y, int inactiveID, int activeID, bool initialState, int switchID)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.checkbox,
                X = x,
                Y = y,
                InactiveID = inactiveID,
                ActiveID = activeID,
                InitialState = initialState,
                ElementID = switchID
            };
            Add(ge);
        }

        public void AddGroup(int group)
        {
            GumpElement ge = new GumpElement {Type = ElementType.group, Group = group};
            Add(ge);
        }

        public void AddTooltip(int number)
        {
            GumpElement ge = new GumpElement {Type = ElementType.tooltip, Cliloc = number};
            Add(ge);
        }

        public void AddHtml(int x, int y, int width, int height, string text, bool background, bool scrollbar)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.htmlgump,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Text = text,
                ScrollBar = scrollbar,
                Background = background
            };
            Add(ge);
        }

        public void AddHtmlLocalized(int x, int y, int width, int height, int number, bool background, bool scrollbar)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.xmfhtmlgump,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Cliloc = number,
                Background = background,
                ScrollBar = scrollbar
            };
            Add(ge);
        }

        public void AddHtmlLocalized(int x, int y, int width, int height, int number, int color, bool background,
            bool scrollbar)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.xmfhtmlgumpcolor,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Cliloc = number,
                Hue = color,
                Background = background,
                ScrollBar = scrollbar
            };
            Add(ge);
        }

        public void AddHtmlLocalized(int x, int y, int width, int height, int number, string args, int color,
            bool background, bool scrollbar)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.xmfhtmltok,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Cliloc = number,
                Args = args,
                Hue = color,
                Background = background,
                ScrollBar = scrollbar
            };
            Add(ge);
        }

        public void AddImage(int x, int y, int gumpID)
        {
            AddImage(x, y, gumpID, 0);
        }

        public void AddImage(int x, int y, int gumpID, int hue)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.gumppic,
                X = x,
                Y = y,
                ElementID = gumpID,
                Hue = hue
            };
            Add(ge);
        }

        public void AddImageTiled(int x, int y, int width, int height, int gumpID)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.gumppictiled,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                ElementID = gumpID
            };
            Add(ge);
        }

        public void AddImageTiledButton(int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type,
            int param, int itemID, int hue, int width, int height)
        {
            AddImageTiledButton(x, y, normalID, pressedID, buttonID, type, param, itemID, hue, width, height, -1);
        }

        public void AddImageTiledButton(int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type,
            int param, int itemID, int hue, int width, int height, int localizedTooltip)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.buttontileart,
                X = x,
                Y = y,
                InactiveID = normalID,
                ActiveID = pressedID,
                ElementID = buttonID,
                ButtonType = (int) type,
                Param = param,
                ItemID = itemID,
                Hue = hue,
                Height = height,
                Width = width,
                Cliloc = localizedTooltip
            };
            Add(ge);
        }

        public void AddItem(int x, int y, int itemID)
        {
            GumpElement ge = new GumpElement {Type = ElementType.tilepic, X = x, Y = y, ItemID = itemID};
            Add(ge);
        }

        public void AddItem(int x, int y, int itemID, int hue)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.tilepic,
                X = x,
                Y = y,
                ItemID = itemID,
                Hue = hue
            };
            Add(ge);
        }

        public void AddLabel(int x, int y, int hue, string text)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.text,
                X = x,
                Y = y,
                Hue = hue,
                Text = text
            };
            Add(ge);
        }

        public void AddLabelCropped(int x, int y, int width, int height, int hue, string text)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.croppedtext,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Hue = hue,
                Text = text
            };
            Add(ge);
        }

        public void AddRadio(int x, int y, int inactiveID, int activeID, bool initialState, int switchID)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.radio,
                X = x,
                Y = y,
                InactiveID = inactiveID,
                ActiveID = activeID,
                InitialState = initialState,
                ElementID = switchID
            };
            Add(ge);
        }

        public void AddTextEntry(int x, int y, int width, int height, int hue, int entryID, string initialText)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.textentry,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Hue = hue,
                ElementID = entryID,
                Text = initialText
            };
            Add(ge);
        }

        public void AddTextEntry(int x, int y, int width, int height, int hue, int entryID, string initialText,
            int size)
        {
            GumpElement ge = new GumpElement
            {
                Type = ElementType.textentrylimited,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Hue = hue,
                ElementID = entryID,
                Text = initialText,
                Size = size
            };
            Add(ge);
        }

        public void AddItemProperty(int serial)
        {
            GumpElement ge = new GumpElement {Type = ElementType.itemproperty, Serial = serial};
            Add(ge);
        }

        public int Intern(string value)
        {
            int indexOf = _strings.IndexOf(value);

            if (indexOf >= 0)
            {
                return indexOf;
            }

            _strings.Add(value);
            return _strings.Count - 1;
        }

        internal static byte[] StringToBuffer(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private byte[] Compile()
        {
            IGumpWriter disp = new GumpWriter(this);

            if (!Movable)
            {
                disp.AppendLayout(NoMove);
            }

            if (!Closable)
            {
                disp.AppendLayout(NoClose);
            }

            if (!Disposable)
            {
                disp.AppendLayout(NoDispose);
            }

            if (!Resizable)
            {
                disp.AppendLayout(NoResize);
            }

            int count = GumpElements.Length;

            for (int i = 0; i < count; ++i)
            {
                GumpElement e = GumpElements[i];

                disp.AppendLayout(BeginLayout);
                e.AppendTo(disp);
                disp.AppendLayout(EndLayout);
            }

            List<string> strings = new List<string>();

            if (Strings != null)
            {
                for (int i = 0; i < Strings.Length; i++)
                {
                    strings.Add(Strings[i]);
                }
            }

            disp.WriteStrings(strings);

            disp.Flush();

            return disp.ToArray();
        }

        internal interface IGumpWriter
        {
            int Switches { get; set; }
            int TextEntries { get; set; }

            void AppendLayout(bool val);
            void AppendLayout(int val);
            void AppendLayoutNS(int val);
            void AppendLayout(string text);
            void AppendLayout(byte[] buffer);
            void WriteStrings(List<string> strings);
            void Flush();
            byte[] ToArray();
        }

        internal class GumpWriter : IGumpWriter, IDisposable
        {
            private readonly byte[] _buffer = new byte[48];

            private readonly PacketWriter _packet;

            private int m_LayoutLength;
            private int m_PacketLength;
            private int m_StringsLength;

            public GumpWriter(Gump g)
            {
                /*_packet = new Packet(0xB0);

                _packet.EnsureCapacity(4096);

                //_packet.Write((short)0);
                _packet.Write( g.Serial );
                _packet.Write( g.ID );
                _packet.Write( g.X );
                _packet.Write( g.Y );
                _packet.Write( (ushort) 0xFFFF );*/

                _packet = new PacketWriter(4096);
                _buffer[0] = (byte) ' ';
                _packet.Write((byte) 0xB0);
                _packet.Write((short) 0);
                _packet.Write(g.Serial);
                _packet.Write(g.ID);
                _packet.Write(g.X);
                _packet.Write(g.Y);
                _packet.Write((ushort) 0xFFFF);
            }

            public PacketWriter GetPacket()
            {
                return _packet;
            }

            public int Switches { get; set; }
            public int TextEntries { get; set; }

            public void AppendLayout(byte[] buffer)
            {
                int length = buffer.Length;
                _packet.Write(buffer, 0, length);
                m_LayoutLength += length;
            }

            public void AppendLayout(string text)
            {
                AppendLayout(BeginTextSeparator);

                int length = text.Length;
                _packet.WriteAsciiFixed(text, length);
                m_LayoutLength += length;

                AppendLayout(EndTextSeparator);
            }

            public void AppendLayout(int val)
            {
                string toString = val.ToString();
                int bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, _buffer, 1) + 1;

                _packet.Write(_buffer, 0, bytes);
                m_LayoutLength += bytes;
            }

            public void AppendLayout(bool val)
            {
                AppendLayout(val ? True : False);
            }

            public void AppendLayoutNS(int val)
            {
                string toString = val.ToString();
                int bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, _buffer, 1);

                _packet.Write(_buffer, 1, bytes);
                m_LayoutLength += bytes;
            }

            public void Flush()
            {
                int length = 23 + m_LayoutLength + m_StringsLength;
                _packet.Seek(1, SeekOrigin.Begin);
                _packet.Write((short) length);
                m_PacketLength = length;
            }

            public void WriteStrings(List<string> text)
            {
                _packet.Seek(19, SeekOrigin.Begin);
                _packet.Write((ushort) m_LayoutLength);
                _packet.Seek(0, SeekOrigin.End);

                _packet.Write((ushort) text.Count);

                for (int i = 0; i < text.Count; ++i)
                {
                    string v = text[i] ?? string.Empty;

                    int length = (ushort) v.Length;
                    m_StringsLength += length * 2 + 2;

                    _packet.Write((ushort) length);
                    _packet.WriteBigUniFixed(v, length);
                }
            }

            public byte[] ToArray()
            {
                byte[] packet = new byte[m_PacketLength];
                Buffer.BlockCopy(_packet.ToArray(), 0, packet, 0, m_PacketLength);
                return packet;
            }

            #region IDisposable Support

            private bool disposedValue; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (disposedValue)
                {
                    return;
                }

                if (disposing)
                {
                    //_packet.Dispose();
                }

                disposedValue = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }
    }
}