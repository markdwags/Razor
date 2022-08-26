#region license
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
#endregion

using System;
using Assistant.Core.Gumps;

namespace Assistant.Gumps
{
    public class GumpElement
    {
        private Gump _parentGump;
        public int ActiveID { get; set; }
        public string Args { get; set; }
        public bool Background { get; set; }
        public int ButtonType { get; set; }
        public int Cliloc { get; set; }
        public int ElementID { get; set; }
        public int Group { get; set; }
        public int Height { get; set; }
        public int Hue { get; set; }
        public int InactiveID { get; set; }
        public bool InitialState { get; set; }
        public int ItemID { get; set; }
        public int PageNumber { get; set; }
        public int Param { get; set; }

        public Gump ParentGump
        {
            get => _parentGump ?? ParentPage?.ParentGump;
            set => _parentGump = value;
        }

        public GumpPage ParentPage { get; set; }
        public bool ScrollBar { get; set; }
        public int Serial { get; set; }
        public int Size { get; set; }
        public string Text { get; set; }
        public int Tooltip { get; set; }
        public ElementType Type { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        ///     Get nearest GumpElement.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(out GumpElement element)
        {
            if (ParentPage != null)
            {
                return ParentPage.GetNearestElement(this, out element);
            }

            element = null;
            return false;
        }

        /// <summary>
        ///     Get nearest GumpElement, but only if it's ElementType is contained in the include list.
        /// </summary>
        /// <param name="includeTypes">Array of ElementTypes which specifies valid GumpElements to search.</param>
        /// <param name="element">GumpElement (out).</param>
        /// <returns>True on success.</returns>
        public bool GetNearestElement(ElementType[] includeTypes, out GumpElement element)
        {
            if (ParentPage != null)
            {
                return ParentPage.GetNearestElement(this, includeTypes, out element);
            }

            element = null;
            return false;
        }

        public void Click()
        {
            Gump g = ParentGump;

            if (g != null && g.ID != 461 && Type == ElementType.button)
            {
                //TODO
                //Macros.MacroEx.CloseClientGump( g.Client, g.ID );
                //Macros.MacroEx.GumpButtonClick( g.Client, g.ID, g.Serial, this.ElementID );
            }
        }

        internal void AppendTo(Gump.IGumpWriter disp)
        {
            switch (Type)
            {
                case ElementType.textentrylimited:
                {
                    disp.AppendLayout(Gump.StringToBuffer("textentrylimited"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(Hue);
                    disp.AppendLayout(ElementID);
                    disp.AppendLayout(ParentGump.Intern(Text));
                    disp.AppendLayout(Size);
                    disp.TextEntries++;
                    break;
                }
                case ElementType.textentry:
                {
                    disp.AppendLayout(Gump.StringToBuffer("textentry"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(Hue);
                    disp.AppendLayout(ElementID);
                    disp.AppendLayout(ParentGump.Intern(Text));
                    disp.TextEntries++;
                    break;
                }
                case ElementType.radio:
                {
                    disp.AppendLayout(Gump.StringToBuffer("radio"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(InactiveID);
                    disp.AppendLayout(ActiveID);
                    disp.AppendLayout(InitialState);
                    disp.AppendLayout(ElementID);
                    disp.Switches++;
                    break;
                }
                case ElementType.croppedtext:
                {
                    disp.AppendLayout(Gump.StringToBuffer("croppedtext"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(Hue);
                    disp.AppendLayout(ParentGump.Intern(Text));
                    break;
                }
                case ElementType.buttontileart:
                {
                    disp.AppendLayout(Gump.StringToBuffer("buttontileart"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(InactiveID);
                    disp.AppendLayout(ActiveID);
                    disp.AppendLayout(ButtonType);
                    disp.AppendLayout(Param);
                    disp.AppendLayout(ElementID);
                    disp.AppendLayout(ItemID);
                    disp.AppendLayout(Hue);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);

                    if (Cliloc != -1)
                    {
                        disp.AppendLayout(Gump.StringToBuffer(" }{ tooltip"));
                        disp.AppendLayout(Cliloc);
                    }

                    break;
                }
                case ElementType.tilepic:
                case ElementType.tilepichue:
                {
                    disp.AppendLayout(Gump.StringToBuffer(Hue == 0 ? "tilepic" : "tilepichue"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(ItemID);

                    if (Hue != 0)
                    {
                        disp.AppendLayout(Hue);
                    }

                    break;
                }
                case ElementType.itemproperty:
                {
                    disp.AppendLayout(Gump.StringToBuffer("itemproperty"));
                    disp.AppendLayout(Serial);
                    break;
                }
                case ElementType.gumppictiled:
                {
                    disp.AppendLayout(Gump.StringToBuffer("gumppictiled"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(ElementID);
                    break;
                }
                case ElementType.gumppic:
                {
                    disp.AppendLayout(Gump.StringToBuffer("gumppic"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(ElementID);

                    if (Hue != 0)
                    {
                        disp.AppendLayout(Gump.StringToBuffer(" hue="));
                        disp.AppendLayoutNS(Hue);
                    }

                    break;
                }
                case ElementType.xmfhtmlgump:
                {
                    disp.AppendLayout(Gump.StringToBuffer("xmfhtmlgump"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(Cliloc);
                    disp.AppendLayout(Background);
                    disp.AppendLayout(ScrollBar);
                    break;
                }
                case ElementType.xmfhtmlgumpcolor:
                {
                    disp.AppendLayout(Gump.StringToBuffer("xmfhtmlgumpcolor"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(Cliloc);
                    disp.AppendLayout(Background);
                    disp.AppendLayout(ScrollBar);
                    disp.AppendLayout(Hue);
                    break;
                }
                case ElementType.xmfhtmltok:
                {
                    disp.AppendLayout(Gump.StringToBuffer("xmfhtmltok"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(Background);
                    disp.AppendLayout(ScrollBar);
                    disp.AppendLayout(Hue);
                    disp.AppendLayout(Cliloc);
                    disp.AppendLayout(Args);
                    break;
                }
                case ElementType.htmlgump:
                {
                    disp.AppendLayout(Gump.StringToBuffer("htmlgump"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    disp.AppendLayout(ParentGump.Intern(Text));
                    disp.AppendLayout(Background);
                    disp.AppendLayout(ScrollBar);
                    break;
                }
                case ElementType.tooltip:
                {
                    disp.AppendLayout(Gump.StringToBuffer("tooltip"));
                    disp.AppendLayout(Cliloc);
                    break;
                }
                case ElementType.group:
                {
                    disp.AppendLayout(Gump.StringToBuffer("group"));
                    disp.AppendLayout(Group);
                    break;
                }
                case ElementType.resizepic:
                {
                    disp.AppendLayout(Gump.StringToBuffer("resizepic"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(ElementID);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    break;
                }
                case ElementType.checkertrans:
                {
                    disp.AppendLayout(Gump.StringToBuffer("checkertrans"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Width);
                    disp.AppendLayout(Height);
                    break;
                }
                case ElementType.page:
                {
                    disp.AppendLayout(Gump.StringToBuffer("page"));
                    disp.AppendLayout(PageNumber);
                    break;
                }
                case ElementType.button:
                {
                    disp.AppendLayout(Gump.StringToBuffer("button"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(InactiveID);
                    disp.AppendLayout(ActiveID);
                    disp.AppendLayout(ButtonType);
                    disp.AppendLayout(Param);
                    disp.AppendLayout(ElementID);
                    break;
                }
                case ElementType.text:
                {
                    disp.AppendLayout(Gump.StringToBuffer("text"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(Hue);
                    disp.AppendLayout(ParentGump.Intern(Text));
                    break;
                }
                case ElementType.invalid:
                    break;
                case ElementType.checkbox:
                {
                    disp.AppendLayout(Gump.StringToBuffer("checkbox"));
                    disp.AppendLayout(X);
                    disp.AppendLayout(Y);
                    disp.AppendLayout(InactiveID);
                    disp.AppendLayout(ActiveID);
                    disp.AppendLayout(InitialState);
                    disp.AppendLayout(ElementID);

                    disp.Switches++;
                    break;
                }
                case ElementType.kr_xmfhtmlgump:
                    break;
                case ElementType.mastergump:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}