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

namespace Assistant.Core
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
                    }

                    Equip(m_Right, DressList.GetLayerFor(m_Right));
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.MustDisarm);
                }
            }
            else
            {
                Unequip(DressList.GetLayerFor(item));
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
                    }

                    Equip(m_Left, DressList.GetLayerFor(m_Left));
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Force, LocString.MustDisarm);
                }
            }
            else
            {
                Unequip(DressList.GetLayerFor(item));
                m_Left = item;
            }
        }

        public static bool Equip(Item item, Layer layer)
        {
            if (layer == Layer.Invalid || layer > Layer.LastUserValid || item == null || item.Layer == Layer.Invalid ||
                item.Layer > Layer.LastUserValid)
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