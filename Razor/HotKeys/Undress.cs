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

using Assistant.Core;

namespace Assistant.HotKeys
{
    public class UndressHotKeys
    {
        public static void Initialize()
        {
            HotKey.Add(HKCategory.Dress, LocString.ArmDisarmRight, new HotKeyCallback(Dress.ToggleRight));
            HotKey.Add(HKCategory.Dress, LocString.ArmDisarmLeft, new HotKeyCallback(Dress.ToggleLeft));

            HotKey.Add(HKCategory.Dress, LocString.UndressAll, new HotKeyCallback(OnUndressAll));
            HotKey.Add(HKCategory.Dress, LocString.UndressHands, new HotKeyCallback(OnUnequipBothHands));
            HotKey.Add(HKCategory.Dress, LocString.UndressLeft, new HotKeyCallback(OnUnequipLeft));
            HotKey.Add(HKCategory.Dress, LocString.UndressRight, new HotKeyCallback(OnUnequipRight));
            HotKey.Add(HKCategory.Dress, LocString.UndressHat, new HotKeyCallback(OnUnequipHat));
            HotKey.Add(HKCategory.Dress, LocString.UndressJewels, new HotKeyCallback(OnUnequipJewelry));
        }

        public static void OnUndressAll()
        {
            for (int i = 0; i < World.Player.Contains.Count; i++)
            {
                Item item = (Item) World.Player.Contains[i];
                if (item.Layer <= Layer.LastUserValid && item.Layer != Layer.Backpack && item.Layer != Layer.Hair &&
                    item.Layer != Layer.FacialHair)
                {
                    Item pack = DressList.FindUndressBag(item);
                    if (pack != null)
                        DragDropManager.DragDrop(item, pack);
                }
            }

            //if ( Macros.MacroManager.AcceptActions )
            //	MacroManager.Action( new Macros.UnDressAction( (byte)0 ) );
        }

        public static void OnUnequipJewelry()
        {
            Dress.Unequip(Layer.Ring); // ring
            Dress.Unequip(Layer.Bracelet); // bracelet
            Dress.Unequip(Layer.Earrings); // earrings
        }

        public static void OnUnequipHat()
        {
            Dress.Unequip(Layer.Head);
        }

        public static void OnUnequipBothHands()
        {
            Dress.Unequip(Layer.RightHand);
            Dress.Unequip(Layer.LeftHand);
        }

        public static void OnUnequipRight()
        {
            Dress.Unequip(Layer.RightHand);
        }

        public static void OnUnequipLeft()
        {
            Dress.Unequip(Layer.LeftHand);
        }
    }
}