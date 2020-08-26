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

using System.Linq;

namespace Assistant.Core.Gumps
{
    public sealed class GumpPage
    {
        public GumpElement[] GumpElements { get; internal set; }
        public int Page { get; internal set; }
        public Gump ParentGump { get; internal set; }

        /// <summary>
        ///     Get array of GumpElements which match the specified ElementType.
        /// </summary>
        public GumpElement[] GetElementsByType( ElementType type )
        {
            return GumpElements.Where( ge => ge.Type == type ).ToArray();
        }

        /// <summary>
        ///     Get nearest GumpElement to source, but only if it's ElementType is contained in the include list.
        /// </summary>
        /// <param name="source">Source element.</param>
        /// <param name="includeTypes">Array of ElementTypes which specifies valid GumpElements to search.</param>
        /// <param name="element">GumpElement (out).</param>
        /// <returns>True on success.</returns>
        public bool GetNearestElement( GumpElement source, ElementType[] includeTypes, out GumpElement element )
        {
            GumpElement nearest = null;
            double closest = 0;

            foreach ( GumpElement ge in GumpElements )
            {
                if ( ge == source )
                {
                    continue;
                }

                bool found = includeTypes.Any( et => ge.Type == et );

                if ( !found )
                {
                    continue;
                }

                double distance = Utility.Distance( source.X, source.Y, ge.X, ge.Y );

                if ( nearest == null )
                {
                    closest = distance;
                    nearest = ge;
                }
                else
                {
                    if ( !( distance < closest ) )
                    {
                        continue;
                    }

                    closest = distance;
                    nearest = ge;
                }
            }

            element = nearest;
            return nearest != null;
        }

        /// <summary>
        ///     Get nearest GumpElement from source.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool GetNearestElement( GumpElement source, out GumpElement element )
        {
            GumpElement nearest = null;
            double closest = 0;

            foreach ( GumpElement ge in GumpElements )
            {
                if ( ge == source )
                {
                    continue;
                }

                double distance = Utility.Distance( source.X, source.Y, ge.X, ge.Y );

                if ( nearest == null )
                {
                    closest = distance;
                    nearest = ge;
                }
                else
                {
                    if ( !( distance < closest ) )
                    {
                        continue;
                    }

                    closest = distance;
                    nearest = ge;
                }
            }

            element = nearest;
            return nearest != null;
        }

        public bool GetElementByXY( int x, int y, out GumpElement gumpElement )
        {
            gumpElement = null;

            if ( GumpElements == null )
            {
                return false;
            }

            GumpElement element = GumpElements.FirstOrDefault( m => m.X == x && m.Y == y );

            if ( element != null )
            {
                gumpElement = element;
            }

            return gumpElement != null;
        }

        public GumpElement GetElementByXY( int x, int y )
        {
            if ( GetElementByXY( x, y, out GumpElement element ) )
            {
                return element;
            }

            return null;
        }

        public bool GetElementByCliloc( int cliloc, out GumpElement gumpElement )
        {
            gumpElement = null;

            if ( GumpElements == null )
            {
                return false;
            }

            GumpElement element = GumpElements.FirstOrDefault( m => m.Cliloc == cliloc );

            if ( element != null )
            {
                gumpElement = element;
            }

            return gumpElement != null;
        }

        public GumpElement GetElementByCliloc( int cliloc )
        {
            return GetElementByCliloc( cliloc, out GumpElement element ) ? element : null;
        }
    }
}