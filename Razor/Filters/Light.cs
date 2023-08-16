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

namespace Assistant.Filters
{
    public class LightFilter : Filter
    {
        public static void Initialize()
        {
            Filter.Register(new LightFilter());
        }

        private LightFilter()
        {
        }

        public override LocString Name
        {
            get { return LocString.LightFilter; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            /*
            if (Client.Instance.AllowBit(FeatureBit.LightFilter))
            {
                args.Block = true;
                if (World.Player != null)
                {
                    World.Player.LocalLightLevel = 0;
                    World.Player.GlobalLightLevel = 0;
                }
            }
            */
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] { /* 0x4E, 0x4F */ }; }
        }

        /*
        
        public override void OnEnable()
        {
            base.OnEnable();

            if (Client.Instance.AllowBit(FeatureBit.LightFilter) && World.Player != null)
            {
                World.Player.LocalLightLevel = 0;
                World.Player.GlobalLightLevel = 0;

                Client.Instance.SendToClient(new GlobalLightLevel(0));
                Client.Instance.SendToClient(new PersonalLightLevel(World.Player));
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (Client.Instance.AllowBit(FeatureBit.LightFilter) && World.Player != null)
            {
                World.Player.LocalLightLevel = 6;
                World.Player.GlobalLightLevel = 2;

                Client.Instance.SendToClient(new GlobalLightLevel(26));
                Client.Instance.SendToClient(new PersonalLightLevel(World.Player));
            }
        }
        */
    }
}