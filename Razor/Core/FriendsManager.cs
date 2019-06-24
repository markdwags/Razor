using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Core
{
    public static class FriendsManager
    {
        private static ComboBox _friendGroups;
        private static ListBox _friendList;

        public static void SetControls(ComboBox friendsGroup, ListBox friendsList)
        {
            _friendGroups = friendsGroup;
            _friendList = friendsList;
        }

        public static void OnTargetAddFriend()
        {
            World.Player.SendMessage(MsgLevel.Friend, LocString.TargFriendAdd);
            Targeting.OneTimeTarget(OnAddTarget);
        }

        public class Friend
        {
            public string Name { get; set; }
            public Serial Serial { get; set; }

            public override string ToString()
            {
                return $"{Name} ({Serial})";
            }
        }

        public class FriendGroup
        {
            public string GroupName { get; set; }
            public bool Enabled { get; set; }
            public List<Friend> Friends { get; set; }

            public FriendGroup()
            {
                Friends = new List<Friend>();
            }

            public override string ToString()
            {
                return $"{GroupName}";
            }
        }

        public static List<FriendGroup> FriendGroups = new List<FriendGroup>();

        public static bool IsFriend(Serial serial)
        {
            bool isFriend = false;

            foreach (var friendGroup in FriendGroups)
            {
                if (friendGroup.Enabled)
                {
                    if (friendGroup.Friends.Any(f => f.Serial == serial) ||
                        (Config.GetBool("AutoFriend") && PacketHandlers.Party.Contains(serial)))
                    {
                        isFriend = true;
                        break;
                    }
                }
            }

            return isFriend;
        }

        public static void EnableFriendsGroup(string name, bool enabled)
        {
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                if (friendGroup.GroupName.Equals(name))
                {
                    friendGroup.Enabled = enabled;
                    return;
                }
            }
        }

        public static bool IsFriendsGroup(string name)
        {
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                if (friendGroup.GroupName.ToLower().Equals(name.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AddFriend(string group, string friendName, Serial friendSerial)
        {
            foreach (var friendGroup in FriendGroups)
            {
                if (friendGroup.GroupName.Equals(group) && IsFriend(friendSerial) == false)
                {
                    Friend newFriend = new Friend
                    {
                        Name = friendName,
                        Serial = friendSerial
                    };

                    friendGroup.Friends.Add(newFriend);

                    _friendList.Items.Add(newFriend);

                    return true;
                }
            }

            return false;
        }

        public static void AddAllMobileAsFriends(string group)
        {
            List<Mobile> mobiles = World.MobilesInRange(12);

            foreach (Mobile mobile in mobiles)
            {
                if (!IsFriend(mobile.Serial) && mobile.Serial.IsMobile && mobile.Serial != World.Player.Serial)
                {
                    if (AddFriend(_friendGroups.Text, mobile.Name, mobile.Serial))
                    {
                        World.Player.SendMessage(MsgLevel.Friend, $"Added '{mobile.Name}' to '{group}'");

                        mobile.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                        mobile.OPLChanged();
                    }
                }
            }
        }

        public static void AddAllHumanoidsAsFriends(string group)
        {
            List<Mobile> mobiles = World.MobilesInRange(12);

            foreach (Mobile mobile in mobiles)
            {
                if (!IsFriend(mobile.Serial) && mobile.Serial.IsMobile && mobile.Serial != World.Player.Serial &&
                    mobile.IsHuman)
                {
                    if (AddFriend(_friendGroups.Text, mobile.Name, mobile.Serial))
                    {
                        World.Player.SendMessage(MsgLevel.Friend, $"Added {mobile.Name} to '{group}'");

                        mobile.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                        mobile.OPLChanged();
                    }
                }
            }
        }

        private static void OnAddTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (!location && serial.IsMobile && serial != World.Player.Serial)
            {
                Mobile m = World.FindMobile(serial);

                if (m == null)
                    return;

                if (AddFriend(_friendGroups.Text, m.Name, serial))
                {
                    World.Player.SendMessage(MsgLevel.Friend, $"Added {m.Name} to '{_friendGroups.Text}'");

                    m.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                    m.OPLChanged();
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Warning, $"'{m.Name}' is already on a friends list");
                }
            }
        }

        public static bool RemoveFriend(int index)
        {
            foreach (var friendGroup in FriendGroups)
            {
                if (friendGroup.GroupName.Equals(_friendGroups.Text))
                {
                    friendGroup.Friends.RemoveAt(index);

                    return true;
                }
            }

            return false;
        }

        public static void ClearFriendGroup(string group)
        {
            foreach (var friendGroup in FriendGroups)
            {
                if (friendGroup.GroupName.Equals(group))
                {
                    friendGroup.Friends.Clear();
                    return;
                }
            }
        }

        public static bool DeleteFriendGroup(string group)
        {
            if (FriendGroups.RemoveAll(g => g.GroupName.Equals(group)) > 0)
            {
                return true;
            }

            return false;
        }

        public static void AddFriendGroup(string group)
        {
            FriendGroup friendGroup = new FriendGroup
            {
                Enabled = true,
                GroupName = group,
                Friends = new List<Friend>()
            };

            _friendGroups.Items.Add(group);

            FriendGroups.Add(friendGroup);
        }

        public static void ReloadFriendGroups()
        {
            _friendGroups.Items.Clear();

            foreach (var friendGroup in FriendGroups)
            {
                _friendGroups.Items.Add(friendGroup.GroupName);
            }

            _friendGroups.SelectedIndex = 0;
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (var friendGroup in FriendGroups)
            {
                xml.WriteStartElement("group");
                xml.WriteAttributeString("name", friendGroup.GroupName);
                xml.WriteAttributeString("enabled", friendGroup.Enabled.ToString());

                foreach (var friend in friendGroup.Friends)
                {
                    xml.WriteStartElement("friend");
                    xml.WriteAttributeString("name", friend.Name);
                    xml.WriteAttributeString("serial", friend.Serial.ToString());
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
        }

        public static void Load(XmlElement node)
        {
            ClearAll();

            try
            {
                foreach (XmlElement el in node.GetElementsByTagName("group"))
                {
                    FriendGroup friendGroup = new FriendGroup
                    {
                        GroupName = el.GetAttribute("name"),
                        Enabled = Convert.ToBoolean(el.GetAttribute("enabled"))
                    };

                    foreach (XmlElement friendEl in el.GetElementsByTagName("friend"))
                    {
                        Friend friend = new Friend
                        {
                            Name = friendEl.GetAttribute("name"),
                            Serial = Serial.Parse(friendEl.GetAttribute("serial"))
                        };

                        friendGroup.Friends.Add(friend);
                    }

                    FriendGroups.Add(friendGroup);
                }
            }
            catch
            {
            }
        }

        public static void ClearAll()
        {
            if (FriendGroups == null)
            {
                FriendGroups = new List<FriendGroup>();
            }

            FriendGroups.Clear();
        }

        public static void Redraw()
        {
            _friendGroups.BeginUpdate();
            _friendGroups.Items.Clear();

            foreach (FriendGroup friendGroup in FriendGroups)
            {
                _friendGroups.Items.Add(friendGroup);
            }

            _friendGroups.EndUpdate();
        }
    }
}