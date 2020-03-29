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

using System.Collections.Generic;

namespace Assistant
{
    public class World
    {
        private static Dictionary<Serial, Item> m_Items;
        private static Dictionary<Serial, Mobile> m_Mobiles;
        private static PlayerData m_Player;
        private static string m_ShardName, m_PlayerName, m_AccountName;
        private static Dictionary<ushort, string> m_Servers;

        static World()
        {
            m_Servers = new Dictionary<ushort, string>();
            m_Items = new Dictionary<Serial, Item>();
            m_Mobiles = new Dictionary<Serial, Mobile>();
            m_ShardName = "[None]";
        }

        internal static Dictionary<ushort, string> Servers
        {
            get { return m_Servers; }
        }

        internal static Dictionary<Serial, Item> Items
        {
            get { return m_Items; }
        }

        internal static Dictionary<Serial, Mobile> Mobiles
        {
            get { return m_Mobiles; }
        }

        internal static Item FindItem(Serial serial)
        {
            m_Items.TryGetValue(serial, out Item it);
            return it;
        }

        internal static Item FindItemByType(int itemId)
        {
            foreach (KeyValuePair<Serial, Item> item in m_Items)
            {
                if (item.Value.ItemID.Value == itemId)
                    return item.Value;
            }

            return null;
        }

        internal static List<Item> FindItemsByName(string name)
        {
            List<Item> items = new List<Item>();

            foreach (KeyValuePair<Serial, Item> item in m_Items)
            {
                if (item.Value.ItemID.ItemData.Name.ToLower().StartsWith(name.ToLower()))
                    items.Add(item.Value);
            }

            return items;
        }

        internal static Mobile FindMobile(Serial serial)
        {
            m_Mobiles.TryGetValue(serial, out Mobile m);
            return m;
        }

        internal static List<Mobile> FindMobilesByName(string name)
        {
            List<Mobile> mobiles = new List<Mobile>();
            foreach (KeyValuePair<Serial, Mobile> item in m_Mobiles)
            {
                if (item.Value.Name.ToLower().Equals(name.ToLower()))
                    mobiles.Add(item.Value);
            }

            return mobiles;
        }

        internal static List<Mobile> MobilesInRange(int range)
        {
            List<Mobile> list = new List<Mobile>();

            if (World.Player == null)
                return list;

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (Utility.InRange(World.Player.Position, m.Position, World.Player.VisRange))
                    list.Add(m);
            }

            return list;
        }

        internal static List<Mobile> MobilesInRange()
        {
            if (Player == null)
                return MobilesInRange(18);
            else
                return MobilesInRange(Player.VisRange);
        }

        internal static void AddItem(Item item)
        {
            m_Items[item.Serial] = item;
        }

        internal static void AddMobile(Mobile mob)
        {
            m_Mobiles[mob.Serial] = mob;
        }

        internal static void RequestMobileStatus(Mobile m)
        {
            Client.Instance.SendToServer(new StatusQuery(m));
        }

        internal static void RemoveMobile(Mobile mob)
        {
            m_Mobiles.Remove(mob.Serial);
        }

        internal static void RemoveItem(Item item)
        {
            m_Items.Remove(item.Serial);
        }

        internal static PlayerData Player
        {
            get { return m_Player; }
            set { m_Player = value; }
        }

        internal static string ShardName
        {
            get { return m_ShardName; }
            set { m_ShardName = value; }
        }

        internal static string OrigPlayerName
        {
            get { return m_PlayerName; }
            set { m_PlayerName = value; }
        }

        internal static string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
    }
}