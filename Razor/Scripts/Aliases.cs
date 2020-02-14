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

using Assistant.Scripts.Engine;

namespace Assistant.Scripts
{
    public static class Aliases
    {
        public static void Register()
        {
            Interpreter.RegisterAliasHandler("backpack", Backpack);
            Interpreter.RegisterAliasHandler("last", Last);
            Interpreter.RegisterAliasHandler("lasttarget", Last);
            Interpreter.RegisterAliasHandler("lastobject", LastObject);
            Interpreter.RegisterAliasHandler("self", Self);
            Interpreter.RegisterAliasHandler("mount", Mounted);
            Interpreter.RegisterAliasHandler("righthand", RHandEmpty);
            Interpreter.RegisterAliasHandler("lefthand", LHandEmpty);
        }

        private static uint Mounted(string alias)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.Mount) != null
                ? (uint) 1
                : (uint) 0;
        }

        private static uint RHandEmpty(string alias)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.RightHand) != null
                ? (uint) 1
                : (uint) 0;
        }

        private static uint LHandEmpty(string alias)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.LeftHand) != null
                ? (uint) 1
                : (uint) 0;
        }

        private static uint Backpack(string alias)
        {
            if (World.Player == null || World.Player.Backpack == null)
                return 0;

            return World.Player.Backpack.Serial;
        }

        private static uint Last(string alias)
        {
            if (!Targeting.DoLastTarget())
                Targeting.ResendTarget();

            return 0;
        }

        private static uint LastObject(string alias)
        {
            if (World.Player.LastObject != null)
                return World.Player.LastObject;

            return 0;
        }

        private static uint Self(string alias)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Serial;
        }
    }
}