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
    public class WeatherFilter : Filter
    {
        public static void Initialize()
        {
            Filter.Register(new WeatherFilter());
        }

        private WeatherFilter()
        {
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0x65}; }
        }

        public override LocString Name
        {
            get { return LocString.Weather; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            if (Client.Instance.AllowBit(FeatureBit.WeatherFilter))
                args.Block = true;
        }
    }
}