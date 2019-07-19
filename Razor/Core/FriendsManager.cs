using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static List<FriendGroup> FriendGroups = new List<FriendGroup>();

        public static void SetControls(ComboBox friendsGroup, ListBox friendsList)
        {
            _friendGroups = friendsGroup;
            _friendList = friendsList;
        }

        public static void OnTargetAddFriend(FriendGroup group)
        {
            World.Player.SendMessage(MsgLevel.Friend, LocString.TargFriendAdd);
            Targeting.OneTimeTarget(group.OnAddFriendTarget);
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

            public void AddHotKeys()
            {
                HotKey.Add(HKCategory.Friends, HKSubCat.None, $"Friend Add Target To: {GroupName}", AddFriendToGroup);
                HotKey.Add(HKCategory.Friends, HKSubCat.None, $"Friend Toggle Group: {GroupName}", ToggleFriendGroup);
                HotKey.Add(HKCategory.Friends, HKSubCat.None, $"Friend Add All Mobiles: {GroupName}", AddAllMobileAsFriends);
                HotKey.Add(HKCategory.Friends, HKSubCat.None, $"Friend Add All Humanoids: {GroupName}", AddAllHumanoidsAsFriends);
            }

            public void RemoveHotKeys()
            {
                HotKey.Remove($"Friend Add Target To: {GroupName}");
                HotKey.Remove($"Friend Toggle Group: {GroupName}");
                HotKey.Remove($"Friend Add All Mobiles: {GroupName}");
                HotKey.Remove($"Friend Add All Humanoids: {GroupName}");
            }

            private void AddFriendToGroup()
            {
                World.Player.SendMessage(MsgLevel.Friend, $"Target friend to add to group '{GroupName}'");
                Targeting.OneTimeTarget(OnAddFriendTarget);
            }

            private void ToggleFriendGroup()
            {
                if (Enabled)
                {
                    World.Player.SendMessage(MsgLevel.Warning, $"Friend group '{GroupName}' ({Friends.Count} friends) has been 'Disabled'");
                    Enabled = false;
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Info, $"Friend group '{GroupName}' ({Friends.Count} friends) has been 'Enabled'");
                    Enabled = true;
                }
            }

            public override string ToString()
            {
                return $"{GroupName}";
            }

            public void OnAddFriendTarget(bool location, Serial serial, Point3D loc, ushort gfx)
            {
                Engine.MainWindow.SafeAction(s => s.ShowMe());

                if (!location && serial.IsMobile && serial != World.Player.Serial)
                {
                    Mobile m = World.FindMobile(serial);

                    if (m == null)
                        return;

                    if (AddFriend(m.Name, serial))
                    {
                        m.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                        m.OPLChanged();
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Warning, $"'{m.Name}' is already in '{GroupName}'");
                    }
                }
            }

            public bool AddFriend(string friendName, Serial friendSerial)
            {
                if (Friends.Any(f => f.Serial == friendSerial) == false)
                {
                    Friend newFriend = new Friend
                    {
                        Name = friendName,
                        Serial = friendSerial
                    };

                    Friends.Add(newFriend);

                    if (_friendGroups.SelectedItem == this)
                    {
                        RedrawList(this);
                    }

                    World.Player.SendMessage(MsgLevel.Friend, $"Added '{friendName}' to '{GroupName}'");

                    return true;
                }
             
                return false;
            }

            public void AddAllMobileAsFriends()
            {
                List<Mobile> mobiles = World.MobilesInRange(12);

                foreach (Mobile mobile in mobiles)
                {
                    if (!IsFriend(mobile.Serial) && mobile.Serial.IsMobile && mobile.Serial != World.Player.Serial)
                    {
                        if (AddFriend(mobile.Name, mobile.Serial))
                        {
                            mobile.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                            mobile.OPLChanged();
                        }
                    }
                }
            }

            public void AddAllHumanoidsAsFriends()
            {
                List<Mobile> mobiles = World.MobilesInRange(12);

                foreach (Mobile mobile in mobiles)
                {
                    if (!IsFriend(mobile.Serial) && mobile.Serial.IsMobile && mobile.Serial != World.Player.Serial &&
                        mobile.IsHuman)
                    {
                        if (AddFriend(mobile.Name, mobile.Serial))
                        {
                            mobile.ObjPropList.Add(Language.GetString(LocString.RazorFriend));
                            mobile.OPLChanged();
                        }
                    }
                }
            }



        }

        public static bool IsFriend(Serial serial)
        {
            // Check if they have treat party as friends enabled and check the party if so
            if (Config.GetBool("AutoFriend") && PacketHandlers.Party.Contains(serial))
                return true;

            bool isFriend = false;

            // Loop through each friends group that is enabled
            foreach (var friendGroup in FriendGroups)
            {
                if (friendGroup.Enabled && friendGroup.Friends.Any(f => f.Serial == serial))
                {
                    isFriend = true;
                    break;
                }
            }

            return isFriend;
        }

        public static void EnableFriendsGroup(FriendGroup group, bool enabled)
        {
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                if (friendGroup == group)
                {
                    friendGroup.Enabled = enabled;
                    return;
                }
            }
        }

        public static bool FriendsGroupExists(string group)
        {
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                if (friendGroup.GroupName.ToLower().Equals(group.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsFriendsGroupEnabled(FriendGroup group)
        {
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                if (friendGroup == group)
                {
                    return friendGroup.Enabled;
                }
            }

            return false;
        }

        public static bool RemoveFriend(FriendGroup group, int index)
        {
            foreach (var friendGroup in FriendGroups)
            {
                if (friendGroup == group)
                {
                    friendGroup.Friends.RemoveAt(index);

                    RedrawList(group);

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

        public static bool DeleteFriendGroup(FriendGroup group)
        {
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                if (friendGroup == group)
                {
                    friendGroup.RemoveHotKeys();
                }
            }

            return FriendGroups.Remove(group);
        }

        public static void AddFriendGroup(string group)
        {
            FriendGroup friendGroup = new FriendGroup
            {
                Enabled = true,
                GroupName = group,
                Friends = new List<Friend>()
            };

            friendGroup.AddHotKeys();

            FriendGroups.Add(friendGroup);

            RedrawGroup();
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

                    friendGroup.AddHotKeys();
                    
                    foreach (XmlElement friendEl in el.GetElementsByTagName("friend"))
                    {
                        try
                        {
                            Friend friend = new Friend
                            {
                                Name = friendEl.GetAttribute("name"),
                                Serial = Serial.Parse(friendEl.GetAttribute("serial"))
                            };

                            friendGroup.Friends.Add(friend);
                        }
                        catch
                        {
                            // ignore this bad record, most likely a bad serial
                        }
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
            foreach (FriendGroup friendGroup in FriendGroups)
            {
                friendGroup.RemoveHotKeys();
            }

            FriendGroups.Clear();
        }

        public static void RedrawGroup()
        {
            _friendGroups.BeginUpdate();
            _friendGroups.Items.Clear();

            foreach (FriendGroup friendGroup in FriendGroups)
            {
                _friendGroups.Items.Add(friendGroup);
            }

            _friendGroups.EndUpdate();

            if (_friendGroups.Items.Count > 0)
            {
                _friendGroups.SelectedIndex = 0;
            }
        }

        public static void RedrawList(FriendGroup group)
        {
            _friendList.BeginUpdate();
            _friendList.Items.Clear();

            foreach (Friend friend in group.Friends)
            {
                _friendList.Items.Add(friend);
            }

            _friendList.EndUpdate();
        }
    }
}