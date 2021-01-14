using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistant.Scripts.Helpers
{
    public static class CommandHelper
    {
        /// <summary>
        /// Common logic for dclicktype and targettype to find items by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="backpack"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Item> GetItemsByName(string name, bool backpack, bool inRange)
        {
            List<Item> items = new List<Item>();

            if (backpack && World.Player.Backpack != null) // search backpack only
            {
                Item i = World.Player.Backpack.FindItemByName(name, true);

                if (i != null)
                    items.Add(i);
            }
            else if (inRange) // inrange includes both backpack and within 2 tiles
            {
                items.AddRange(World.FindItemsByName(name).Where(item =>
                    !item.IsInBank && (Utility.InRange(World.Player.Position, item.Position, 2) ||
                                       item.RootContainer == World.Player)).ToList());
            }
            else
            {
                items.AddRange(World.FindItemsByName(name).Where(item => !item.IsInBank).ToList());
            }

            return items;
        }

        /// <summary>
        /// Common logic for dclicktype and targettype to find items by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="backpack"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Item> GetItemsById(ushort id, bool backpack, bool inRange)
        {
            List<Item> items = new List<Item>();

            if (backpack && World.Player.Backpack != null)
            {
                Item i = World.Player.Backpack.FindItemByID(id);

                if (i != null)
                    items.Add(i);
            } 
            else
            {
                foreach (Item i in World.Items.Values)
                {
                    if (i.ItemID == id && i.RootContainer == null)
                    {
                        if (inRange)
                        {
                            if (Utility.InRange(World.Player.Position, i.Position, 2))
                                items.Add(i);
                        }
                        else
                        {
                            items.Add(i);
                        }
                    }
                }

                if (items.Count == 0)
                {
                    foreach (Item i in World.Items.Values)
                    {
                        if (i.ItemID == id && !i.IsInBank)
                        {
                            if (inRange)
                            {
                                if (Utility.InRange(World.Player.Position, i.Position, 2))
                                    items.Add(i);
                            }
                            else
                            {
                                items.Add(i);
                            }
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Common logic for dclicktype and targettype to find mobiles by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Mobile> GetMobilesByName(string name, bool inRange)
        {
            List<Mobile> mobiles;

            if (inRange)
            {
                mobiles = World.FindMobilesByName(name)
                    .Where(m => Utility.InRange(World.Player.Position, m.Position, 2)).ToList();
            }
            else
            {
                mobiles = World.FindMobilesByName(name);
            }

            return mobiles;
        }

        /// <summary>
        /// Common logic for dclicktype and targettype to find mobiles by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inRange"></param>
        /// <returns></returns>
        public static List<Mobile> GetMobilesById(ushort id, bool inRange)
        {
            List<Mobile> mobiles;

            if (inRange)
            {
                mobiles = World.MobilesInRange()
                    .Where(m => Utility.InRange(World.Player.Position, m.Position, 2) && m.Body == id).ToList();
            }
            else
            {
                mobiles = World.MobilesInRange().Where(m => m.Body == id).ToList();
            }

            return mobiles;
        }
    }
}
