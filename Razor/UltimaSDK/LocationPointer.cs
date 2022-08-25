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

namespace Ultima
{
    public sealed class LocationPointer
    {
        public int PointerX { get; set; }
        public int PointerY { get; set; }
        public int PointerZ { get; set; }
        public int PointerF { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int SizeZ { get; set; }
        public int SizeF { get; set; }

        public LocationPointer(int ptrX, int ptrY, int ptrZ, int ptrF, int sizeX, int sizeY, int sizeZ, int sizeF)
        {
            PointerX = ptrX;
            PointerY = ptrY;
            PointerZ = ptrZ;
            PointerF = ptrF;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            SizeF = sizeF;
        }
    }
}