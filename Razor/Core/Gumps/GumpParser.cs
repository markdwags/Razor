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
using System.Text;

namespace Assistant.Core.Gumps
{
    public static class GumpParser
    {
        // Gump command reference: http://docs.polserver.com/pol098/guides.php?guidefile=gumpcmdlist#xmfhtmltok
        private static void OnError( string elementName, string layout )
        {
            throw new InvalidOperationException(
                $"Error parsing gump element {elementName} in layout:\r\n\r\n{layout}" );
        }

        private static string[] GetTokens( string input )
        {
            //@0@1@ = {"0","1"}
            if ( input.Contains( "@" ) )
            {
                input = input.Trim( '@' );

                if ( input.Contains( "@" ) )
                {
                    return input.Split( '@' );
                }

                string[] output = new string[1];
                output[0] = input;
                return output;
            }

            return null;
        }

        private static bool GetInt( string input, out int output )
        {
            if ( input.Contains( "=" ) )
            {
                string[] strArray = input.Split( '=' );
                return int.TryParse( strArray[1].Trim(), out output );
            }

            output = -1;
            return false;
        }

        public static Gump Parse( int serial, uint ID, int x, int y, string layout, string[] text )
        {
            bool closable = true, movable = true, disposable = true, resizable = true;

            List<GumpPage> pageList = new List<GumpPage>();
            List<GumpElement> gumpElementList = new List<GumpElement>();
            GumpElement lastGumpElement = null;
            GumpPage currentPage = null;

            if ( string.IsNullOrEmpty( layout ) )
            {
                return null;
            }

            string[] split = layout.Substring( layout.IndexOf( '{' ) ).TrimEnd( '}', ' ' )
                .Split( new[] { '}' }, StringSplitOptions.RemoveEmptyEntries );

            for ( int i = 0; i < split.Length; i++ )
            {
                split[i] = split[i].TrimStart( '{', ' ' ).Trim();
                string[] formatted = split[i].Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

                for ( int j = 0; j < formatted.Length; j++ )
                {
                    formatted[j] = formatted[j].Trim();
                }

                switch ( formatted[0] )
                {
                    case "noclose":
                        closable = false;
                        break;
                    case "nodispose":
                        disposable = false;
                        break;
                    case "nomove":
                        movable = false;
                        break;
                    case "noresize":
                        resizable = false;
                        break;
                    case "kr_button":
                    case "button":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int gbX ) &&
                                 int.TryParse( formatted[2], out int gbY ) &&
                                 int.TryParse( formatted[3], out int gbNormalID ) &&
                                 int.TryParse( formatted[4], out int gbPressedID ) &&
                                 int.TryParse( formatted[5], out int gbType ) &&
                                 int.TryParse( formatted[6], out int gbParam ) &&
                                 int.TryParse( formatted[7], out int gbID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.button,
                                    ParentPage = currentPage,
                                    X = gbX,
                                    Y = gbY,
                                    InactiveID = gbNormalID,
                                    ActiveID = gbPressedID,
                                    ElementID = gbID,
                                    ButtonType = gbType,
                                    Param = gbParam
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "button", layout );
                            }
                        }
                        catch
                        {
                            OnError( "button", layout );
                        }

                        break;
                    case "buttontileart":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int btX ) &&
                                 int.TryParse( formatted[2], out int btY ) &&
                                 int.TryParse( formatted[3], out int btNormalID ) &&
                                 int.TryParse( formatted[4], out int btPressedID ) &&
                                 int.TryParse( formatted[5], out int btType ) &&
                                 int.TryParse( formatted[6], out int btParam ) &&
                                 int.TryParse( formatted[7], out int btID ) &&
                                 int.TryParse( formatted[8], out int btItemID ) &&
                                 int.TryParse( formatted[9], out int btHue ) &&
                                 int.TryParse( formatted[10], out int btWidth ) &&
                                 int.TryParse( formatted[11], out int btHeight ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.buttontileart,
                                    ParentPage = currentPage,
                                    X = btX,
                                    Y = btY,
                                    ElementID = btID,
                                    ButtonType = btType,
                                    Height = btHeight,
                                    Width = btWidth,
                                    Hue = btHue,
                                    ItemID = btItemID,
                                    InactiveID = btNormalID,
                                    Param = btParam,
                                    ActiveID = btPressedID
                                };
                                gumpElementList.Add( ge );
                                lastGumpElement = ge;
                            }
                            else
                            {
                                OnError( "buttontileart", layout );
                            }
                        }
                        catch
                        {
                            OnError( "buttontileart", layout );
                        }

                        break;
                    case "checkertrans":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int ctX ) &&
                                 int.TryParse( formatted[2], out int ctY ) &&
                                 int.TryParse( formatted[3], out int ctWidth ) &&
                                 int.TryParse( formatted[4], out int ctHeight ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.checkertrans,
                                    ParentPage = currentPage,
                                    X = ctX,
                                    Y = ctY,
                                    Width = ctWidth,
                                    Height = ctHeight
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "checkertrans", layout );
                            }
                        }
                        catch
                        {
                            OnError( "checkertrans", layout );
                        }

                        break;
                    case "croppedtext":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int crX ) &&
                                 int.TryParse( formatted[2], out int crY ) &&
                                 int.TryParse( formatted[3], out int crWidth ) &&
                                 int.TryParse( formatted[4], out int crHeight ) &&
                                 int.TryParse( formatted[5], out int crHue ) &&
                                 int.TryParse( formatted[6], out int crText ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.croppedtext,
                                    ParentPage = currentPage,
                                    X = crX,
                                    Y = crY,
                                    Height = crHeight,
                                    Width = crWidth,
                                    Hue = crHue,
                                    Text = text[crText]
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "croppedtext", layout );
                            }
                        }
                        catch
                        {
                            OnError( "croppedtext", layout );
                        }

                        break;
                    case "checkbox":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int cbX ) &&
                                 int.TryParse( formatted[2], out int cbY ) &&
                                 int.TryParse( formatted[3], out int cbInactiveID ) &&
                                 int.TryParse( formatted[4], out int cbActiveID ) &&
                                 int.TryParse( formatted[5], out int cbState ) &&
                                 int.TryParse( formatted[6], out int cbID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.checkbox,
                                    ParentPage = currentPage,
                                    X = cbX,
                                    Y = cbY,
                                    InactiveID = cbInactiveID,
                                    ActiveID = cbActiveID,
                                    ElementID = cbID,
                                    InitialState = cbState == 1
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "checkbox", layout );
                            }
                        }
                        catch
                        {
                            OnError( "checkbox", layout );
                        }

                        break;
                    case "page":
                        if ( currentPage == null )
                        {
                            currentPage = new GumpPage();
                        }
                        else
                        {
                            currentPage.GumpElements = gumpElementList.ToArray();
                            pageList.Add( currentPage );
                            currentPage = new GumpPage();
                            gumpElementList = new List<GumpElement>();
                        }

                        if ( int.TryParse( formatted[1], out int page ) )
                        {
                            currentPage.Page = page;
                        }
                        else
                        {
                            OnError( "page", layout );
                        }

                        break;
                    case "kr_gumppic":
                    case "gumppic":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int gpX ) &&
                                 int.TryParse( formatted[2], out int gpY ) &&
                                 int.TryParse( formatted[3], out int gpID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.gumppic, ParentPage = currentPage
                                };

                                if ( formatted.Length > 4 )
                                {
                                    for ( int gp = 4; gp < formatted.Length; gp++ )
                                    {
                                        if ( formatted[gp].Contains( "hue" ) )
                                        {
                                            if ( GetInt( formatted[gp], out int gpHue ) )
                                            {
                                                ge.Hue = gpHue;
                                            }
                                            else
                                            {
                                                OnError( "gumppic", layout );
                                            }
                                        }
                                        else
                                        {
                                            ge.Args = formatted[gp];
                                        }
                                    }
                                }

                                ge.X = gpX;
                                ge.Y = gpY;
                                ge.ElementID = gpID;
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "gumppic", layout );
                            }
                        }
                        catch
                        {
                            OnError( "gumppic", layout );
                        }

                        break;
                    case "gumppictiled":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int gtX ) &&
                                 int.TryParse( formatted[2], out int gtY ) &&
                                 int.TryParse( formatted[3], out int gtWidth ) &&
                                 int.TryParse( formatted[4], out int gtHeight ) &&
                                 int.TryParse( formatted[5], out int gtID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.gumppictiled,
                                    ParentPage = currentPage,
                                    X = gtX,
                                    Y = gtY,
                                    Width = gtWidth,
                                    Height = gtHeight,
                                    ElementID = gtID
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "gumppictiled", layout );
                            }
                        }
                        catch
                        {
                            OnError( "gumppictiled", layout );
                        }

                        break;
                    case "kr_xmfhtmlgump":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int xgX ) &&
                                 int.TryParse( formatted[2], out int xgY ) &&
                                 int.TryParse( formatted[3], out int xgWidth ) &&
                                 int.TryParse( formatted[4], out int xgHeight ) &&
                                 int.TryParse( formatted[5], out int xgCliloc ) &&
                                 int.TryParse( formatted[6], out int xgBackground ) &&
                                 int.TryParse( formatted[7], out int xgScrollbar ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.kr_xmfhtmlgump,
                                    ParentPage = currentPage,
                                    X = xgX,
                                    Y = xgY,
                                    Width = xgWidth,
                                    Height = xgHeight,
                                    Cliloc = xgCliloc,
                                    Background = xgBackground == 1,
                                    ScrollBar = xgScrollbar == 1
                                };

                                if ( xgCliloc != 0 )
                                {
                                    ge.Text = Language.GetCliloc( xgCliloc );
                                }

                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "kr_xmfhtmlgump", layout );
                            }
                        }
                        catch
                        {
                            OnError( "kr_xmfhtmlgump", layout );
                        }

                        break;
                    case "mastergump":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int mgID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.mastergump, ParentPage = currentPage, ElementID = mgID
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "mastergump", layout );
                            }
                        }
                        catch
                        {
                            OnError( "mastergump", layout );
                        }

                        break;
                    case "radio":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int rX ) && int.TryParse( formatted[2], out int rY ) &&
                                 int.TryParse( formatted[3], out int rInactiveID ) &&
                                 int.TryParse( formatted[4], out int rActiveID ) &&
                                 int.TryParse( formatted[5], out int rState ) &&
                                 int.TryParse( formatted[6], out int rID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.radio,
                                    ParentPage = currentPage,
                                    X = rX,
                                    Y = rY,
                                    InactiveID = rInactiveID,
                                    ActiveID = rActiveID,
                                    ElementID = rID,
                                    InitialState = rState == 1
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "radio", layout );
                            }
                        }
                        catch
                        {
                            OnError( "radio", layout );
                        }

                        break;
                    case "resizepic":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int rpX ) &&
                                 int.TryParse( formatted[2], out int rpY ) &&
                                 int.TryParse( formatted[3], out int rpID ) &&
                                 int.TryParse( formatted[4], out int rpWidth ) &&
                                 int.TryParse( formatted[5], out int _ ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.resizepic,
                                    ParentPage = currentPage,
                                    X = rpX,
                                    Y = rpY,
                                    Width = rpWidth,
                                    Height = rpWidth,
                                    ElementID = rpID
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "resizepic", layout );
                            }
                        }
                        catch
                        {
                            OnError( "resizepic", layout );
                        }

                        break;
                    case "text":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int tX ) && int.TryParse( formatted[2], out int tY ) &&
                                 int.TryParse( formatted[3], out int tHue ) &&
                                 int.TryParse( formatted[4], out int tText ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.text,
                                    ParentPage = currentPage,
                                    X = tX,
                                    Y = tY,
                                    Hue = tHue,
                                    Text = text[tText]
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "text", layout );
                            }
                        }
                        catch
                        {
                            OnError( "text", layout );
                        }

                        break;
                    case "tooltip":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int tooltip ) /* && lastGumpElement != null */ )
                            {
                                if ( lastGumpElement != null )
                                {
                                    lastGumpElement.Tooltip = tooltip;
                                    lastGumpElement.Text = Language.GetCliloc( tooltip );
                                }
                            }
                            else
                            {
                                OnError( "tooltip", layout );
                            }
                        }
                        catch
                        {
                            OnError( "tooltip", layout );
                        }

                        break;
                    case "htmlgump":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int hgX ) &&
                                 int.TryParse( formatted[2], out int hgY ) &&
                                 int.TryParse( formatted[3], out int hgWidth ) &&
                                 int.TryParse( formatted[4], out int hgHeight ) &&
                                 int.TryParse( formatted[5], out int hgText ) &&
                                 int.TryParse( formatted[6], out int hgBackground ) &&
                                 int.TryParse( formatted[7], out int hgScrollbar ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.htmlgump,
                                    ParentPage = currentPage,
                                    X = hgX,
                                    Y = hgY,
                                    Width = hgWidth,
                                    Height = hgHeight,
                                    Text = text[hgText],
                                    ScrollBar = hgScrollbar == 1,
                                    Background = hgBackground == 1
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "htmlgump", layout );
                            }
                        }
                        catch
                        {
                            OnError( "htmlgump", layout );
                        }

                        break;
                    case "textentry":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int teX ) &&
                                 int.TryParse( formatted[2], out int teY ) &&
                                 int.TryParse( formatted[3], out int teWidth ) &&
                                 int.TryParse( formatted[4], out int teHeight ) &&
                                 int.TryParse( formatted[5], out int teHue ) &&
                                 int.TryParse( formatted[6], out int teID ) &&
                                 int.TryParse( formatted[7], out int teText ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.textentry,
                                    ParentPage = currentPage,
                                    X = teX,
                                    Y = teY,
                                    Height = teHeight,
                                    Width = teWidth,
                                    Hue = teHue,
                                    ElementID = teID,
                                    Text = text[teText]
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "textentry", layout );
                            }
                        }
                        catch
                        {
                            OnError( "textentry", layout );
                        }

                        break;
                    case "textentrylimited":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int tlX ) &&
                                 int.TryParse( formatted[2], out int tlY ) &&
                                 int.TryParse( formatted[3], out int tlWidth ) &&
                                 int.TryParse( formatted[4], out int tlHeight ) &&
                                 int.TryParse( formatted[5], out int tlHue ) &&
                                 int.TryParse( formatted[6], out int tlID ) &&
                                 int.TryParse( formatted[7], out int tlText ) &&
                                 int.TryParse( formatted[8], out int tlSize ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.textentrylimited,
                                    ParentPage = currentPage,
                                    X = tlX,
                                    Y = tlY,
                                    Height = tlHeight,
                                    Width = tlWidth,
                                    Hue = tlHue,
                                    ElementID = tlID,
                                    Text = text[tlText],
                                    Size = tlSize
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "textentrylimited", layout );
                            }
                        }
                        catch
                        {
                            OnError( "textentrylimited", layout );
                        }

                        break;
                    case "tilepic":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int tpX ) &&
                                 int.TryParse( formatted[2], out int tpY ) && int.TryParse(
                                     formatted[3].Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries )?[0] ??
                                     "0", out int tpID ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.tilepic,
                                    ParentPage = currentPage,
                                    X = tpX,
                                    Y = tpY,
                                    ElementID = tpID
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "tilepic", layout );
                            }
                        }
                        catch
                        {
                            OnError( "tilepic", layout );
                        }

                        break;
                    case "tilepichue":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int tpX ) &&
                                 int.TryParse( formatted[2], out int tpY ) &&
                                 int.TryParse( formatted[3], out int tpID ) &&
                                 int.TryParse( formatted[4], out int tpHue ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.tilepichue,
                                    ParentPage = currentPage,
                                    X = tpX,
                                    Y = tpY,
                                    ElementID = tpID,
                                    Hue = tpHue
                                };
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "tilepichue", layout );
                            }
                        }
                        catch
                        {
                            OnError( "tilepichue", layout );
                        }

                        break;
                    case "xmfhtmlgump":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int xgX ) &&
                                 int.TryParse( formatted[2], out int xgY ) &&
                                 int.TryParse( formatted[3], out int xgWidth ) &&
                                 int.TryParse( formatted[4], out int xgHeight ) &&
                                 int.TryParse( formatted[5], out int xgCliloc ) &&
                                 int.TryParse( formatted[6], out int xgBackground ) &&
                                 int.TryParse( formatted[7], out int xgScrollbar ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.xmfhtmlgump,
                                    ParentPage = currentPage,
                                    X = xgX,
                                    Y = xgY,
                                    Width = xgWidth,
                                    Height = xgHeight,
                                    Cliloc = xgCliloc,
                                    Background = xgBackground == 1,
                                    ScrollBar = xgScrollbar == 1
                                };

                                if ( xgCliloc != 0 )
                                {
                                    ge.Text = Language.GetCliloc( xgCliloc );
                                }

                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "xmfhtmlgump", layout );
                            }
                        }
                        catch
                        {
                            OnError( "xmfhtmlgump", layout );
                        }

                        break;
                    case "xmfhtmlgumpcolor":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int xcX ) &&
                                 int.TryParse( formatted[2], out int xcY ) &&
                                 int.TryParse( formatted[3], out int xcWidth ) &&
                                 int.TryParse( formatted[4], out int xcHeight ) &&
                                 int.TryParse( formatted[5], out int xcCliloc ) &&
                                 int.TryParse( formatted[6], out int xcBackground ) &&
                                 int.TryParse( formatted[7], out int xcScrollbar ) &&
                                 int.TryParse( formatted[8], out int xcHue ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.xmfhtmlgumpcolor,
                                    ParentPage = currentPage,
                                    X = xcX,
                                    Y = xcY,
                                    Width = xcWidth,
                                    Height = xcHeight,
                                    Cliloc = xcCliloc,
                                    Background = xcBackground == 1,
                                    ScrollBar = xcScrollbar == 1,
                                    Hue = xcHue
                                };

                                if ( xcCliloc != 0 )
                                {
                                    ge.Text = Language.GetCliloc( xcCliloc );
                                }

                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "xmfhtmlgumpcolor", layout );
                            }
                        }
                        catch
                        {
                            OnError( "xmfhtmlgumpcolor", layout );
                        }

                        break;
                    case "xmfhtmltok":
                        try
                        {
                            if ( int.TryParse( formatted[1], out int xtX ) &&
                                 int.TryParse( formatted[2], out int xtY ) &&
                                 int.TryParse( formatted[3], out int xtWidth ) &&
                                 int.TryParse( formatted[4], out int xtHeight ) &&
                                 int.TryParse( formatted[5], out int xtBackground ) &&
                                 int.TryParse( formatted[6], out int xtScrollbar ) &&
                                 int.TryParse( formatted[7], out int xtHue ) &&
                                 int.TryParse( formatted[8], out int xtCliloc ) )
                            {
                                GumpElement ge = new GumpElement
                                {
                                    Type = ElementType.xmfhtmltok, ParentPage = currentPage
                                };

                                string[] args = null;
                                StringBuilder sb = new StringBuilder();

                                if ( formatted.Length > 9 )
                                {
                                    sb.Append( formatted[9] );

                                    for ( int a = 10; a < formatted.Length; a++ )
                                    {
                                        sb.Append( ' ' );
                                        sb.Append( formatted[a] );
                                    }

                                    args = GetTokens( sb.ToString() );
                                }

                                ge.Text = Language.GetCliloc( xtCliloc );
                                ge.Args = args != null ? string.Join( "\t", args ) : string.Empty;
                                ge.X = xtX;
                                ge.Y = xtY;
                                ge.Width = xtWidth;
                                ge.Height = xtHeight;
                                ge.Hue = xtHue;
                                ge.Cliloc = xtCliloc;
                                ge.ScrollBar = xtScrollbar == 1;
                                ge.Background = xtBackground == 1;
                                gumpElementList.Add( ge );
                            }
                            else
                            {
                                OnError( "xmfhtmltok", layout );
                            }
                        }
                        catch
                        {
                            OnError( "xmfhtmltok", layout );
                        }

                        break;
                    case "itemproperty":
                        int itemserial;

                        if ( int.TryParse( formatted[1], out itemserial ) )
                        {
                            GumpElement ge = new GumpElement { Type = ElementType.itemproperty, Serial = itemserial };
                            gumpElementList.Add( ge );
                        }

                        break;
                    case "echandleinput":
                        //TODO
                        break;
                    //default:
                    //    throw new InvalidOperationException(
                    //        $"Unknown element \"{formatted[0]}\" in custom gump layout:\r\n\r\n{layout}" );
                }
            }

            if ( currentPage != null )
            {
                currentPage.GumpElements = gumpElementList.ToArray();
                pageList.Add( currentPage );
            }

            Gump g = new Gump( x, y, ID, serial, layout, text, gumpElementList.ToArray(), pageList.ToArray() )
            {
                Closable = closable, Disposable = disposable, Movable = movable, Resizable = resizable
            };

            return g;
        }
    }
}