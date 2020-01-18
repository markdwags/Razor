using System;
using Assistant;
using Assistant.Core;
using Assistant.Macros;

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