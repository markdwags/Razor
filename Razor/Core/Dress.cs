using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistant
{
    public static class Dress
    {
        private static Item m_Right, m_Left;
        public static void ToggleRight()
        {
            if (World.Player == null)
                return;

            Item item = World.Player.GetItemOnLayer(Layer.RightHand);
            if (item == null)
            {
                if (m_Right != null)
                    m_Right = World.FindItem(m_Right.Serial);

                if (m_Right != null && m_Right.IsChildOf(World.Player.Backpack))
                {
                    // try to also undress conflicting hand(s)
                    Item conflict = World.Player.GetItemOnLayer(Layer.LeftHand);
                    if (conflict != null && (conflict.IsTwoHanded || m_Right.IsTwoHanded))
                    {
                        Unequip(DressList.GetLayerFor(conflict));
                        /*
                        Item ub = DressList.FindUndressBag(conflict);
                        if (ub != null)
                            DragDropManager.DragDrop(conflict, ub);
                        */
                    }

                    Equip(m_Right, DressList.GetLayerFor(m_Right));

                    // DragDropManager.DragDrop(m_Right, World.Player, DressList.GetLayerFor(m_Right));
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.MustDisarm);
                }
            }
            else
            {
                Unequip(DressList.GetLayerFor(m_Left));
                /*
                Item ub = DressList.FindUndressBag(item);
                if (ub != null)
                    DragDropManager.DragDrop(item, ub);
                */
                m_Right = item;
            }
        }
        public static void ToggleLeft()
        {
            if (World.Player == null || World.Player.Backpack == null)
                return;

            Item item = World.Player.GetItemOnLayer(Layer.LeftHand);
            if (item == null)
            {
                if (m_Left != null)
                    m_Left = World.FindItem(m_Left.Serial);

                if (m_Left != null && m_Left.IsChildOf(World.Player.Backpack))
                {
                    Item conflict = World.Player.GetItemOnLayer(Layer.RightHand);
                    if (conflict != null && (conflict.IsTwoHanded || m_Left.IsTwoHanded))
                    {
                        Unequip(DressList.GetLayerFor(conflict));
                        /*
                        Item ub = DressList.FindUndressBag(conflict);
                        if (ub != null)
                            DragDropManager.DragDrop(conflict, ub);
                        */
                    }

                    Equip(m_Left, DressList.GetLayerFor(m_Left));
                    // DragDropManager.DragDrop(m_Left, World.Player, DressList.GetLayerFor(m_Left));
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.MustDisarm);
                }
            }
            else
            {
                Unequip(DressList.GetLayerFor(m_Left));
                /*
                Item ub = DressList.FindUndressBag(item);
                if (ub != null)
                    DragDropManager.DragDrop(item, ub);
                */
                m_Left = item;
            }
        }
        public static bool Equip(Item item, Layer layer)
        {
            if (layer == Layer.Invalid || layer > Layer.LastUserValid || item.Layer == Layer.Invalid || item.Layer > Layer.LastUserValid)
                return false;

            if (item != null && World.Player != null && item.IsChildOf(World.Player.Backpack))
            {
                DragDropManager.DragDrop(item, World.Player, layer);
                return true;
            }

            return false;
        }
        public static bool Unequip(Layer layer)
        {
            if (layer == Layer.Invalid || layer > Layer.LastUserValid)
                return false;

            Item item = World.Player.GetItemOnLayer(layer);
            if (item != null)
            {
                Item pack = DressList.FindUndressBag(item);
                if (pack != null)
                {
                    DragDropManager.DragDrop(item, pack);
                    return true;
                }
            }

            return false;
        }
    }
}
