using System;
using System.Collections;
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
            Item it;
            m_Items.TryGetValue(serial, out it);
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


        internal static Mobile FindMobile(Serial serial)
        {
            Mobile m;
            m_Mobiles.TryGetValue(serial, out m);
            return m;
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