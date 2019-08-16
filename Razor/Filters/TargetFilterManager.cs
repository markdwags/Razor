using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Assistant.Core;
using Assistant.UI;

namespace Assistant.Filters
{
    public static class TargetFilterManager
    {
        private static ListBox _targetFilterList;

        private static List<TargetFilter> TargetFilters = new List<TargetFilter>();

        public static void SetControls(ListBox targetFilterList)
        {
            _targetFilterList = targetFilterList;
        }

        public static void OnTargetAddTargetFilter()
        {
            World.Player.SendMessage(MsgLevel.Friend, "Target mobile to be filtered");
            Targeting.OneTimeTarget(OnAddFriendTargetFilter);
        }

        public static void OnAddFriendTargetFilter(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (!location && serial.IsMobile && serial != World.Player.Serial)
            {
                Mobile m = World.FindMobile(serial);

                if (m == null)
                    return;

                AddTargetFilter(m.Name, serial);

                RedrawList();
            }
        }

        public class TargetFilter
        {
            public string Name { get; set; }
            public Serial Serial { get; set; }

            public override string ToString()
            {
                return $"{Name} ({Serial})";
            }
        }

        public static void AddTargetFilter(string name, Serial serial)
        {
            if (TargetFilters.Any(f => f.Serial == serial) == false)
            {
                if (FriendsManager.IsFriend(serial))
                {
                    World.Player.SendMessage(MsgLevel.Warning, $"'{name}' ({serial}) is a friend, not adding to target filter list");
                    return;
                }

                TargetFilter newFilter = new TargetFilter
                {
                    Name = name,
                    Serial = serial
                };

                TargetFilters.Add(newFilter);

                World.Player.SendMessage(MsgLevel.Info, $"Added '{name}' ({serial}) to the target filter list");
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, $"'{name}' ({serial}) is already on the target filter");
            }
        }

        public static void AddAllMobileAsTargetFilters()
        {
            List<Mobile> mobiles = World.MobilesInRange(12);

            foreach (Mobile mobile in mobiles)
            {
                if (!FriendsManager.IsFriend(mobile.Serial) && mobile.Serial.IsMobile && mobile.Serial != World.Player.Serial)
                {
                    AddTargetFilter(mobile.Name, mobile.Serial);
                    /*if (AddTargetFilter(mobile.Name, mobile.Serial))
                    {
                        mobile.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                        mobile.OPLChanged();
                    }*/
                }
            }
        }

        public static void AddAllHumanoidsAsTargetFilters()
        {
            List<Mobile> mobiles = World.MobilesInRange(12);

            foreach (Mobile mobile in mobiles)
            {
                if (!FriendsManager.IsFriend(mobile.Serial) && mobile.Serial.IsMobile && mobile.Serial != World.Player.Serial &&
                    mobile.IsHuman)
                {
                    AddTargetFilter(mobile.Name, mobile.Serial);
                    /*if (AddFriend(mobile.Name, mobile.Serial))
                    {
                        mobile.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                        mobile.OPLChanged();
                    }*/
                }
            }
        }

        public static bool IsFilteredTarget(Serial serial)
        {
            if (!Config.GetBool("TargetFilterEnabled"))
                return false;
            
            foreach (var filter in TargetFilters)
            {
                if (filter.Serial == serial)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool RemoveTargetFilter(int index)
        {
            TargetFilters.RemoveAt(index);
            RedrawList();

            return true;
        }

        public static void ClearTargetFilters()
        {
            TargetFilters.Clear();
            RedrawList();
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (var filter in TargetFilters)
            {
                xml.WriteStartElement("targetfilter");
                xml.WriteAttributeString("name", filter.Name);
                xml.WriteAttributeString("serial", filter.Serial.ToString());
                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("targetfilter"))
                {
                    try
                    {
                        TargetFilter filter = new TargetFilter
                        {
                            Name = el.GetAttribute("name"),
                            Serial = Serial.Parse(el.GetAttribute("serial"))
                        };

                        TargetFilters.Add(filter);
                    }
                    catch
                    {
                        // bad entry, ignore
                    }
                }
            }
            catch
            {
                // must not be in the profile, move on
            }

            RedrawList();
        }

        public static void ClearAll()
        {
            TargetFilters.Clear();
        }

        public static void RedrawList()
        {
            _targetFilterList?.SafeAction(s =>
            {
                s.BeginUpdate();
                s.Items.Clear();

                foreach (var target in TargetFilters)
                {
                    s.Items.Add(target);
                }

                s.EndUpdate();
            });
        }
    }
}