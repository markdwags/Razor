#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using Assistant.Scripts;
using Assistant.UI;

namespace Assistant
{
    [Flags]
    public enum ModKeys : int
    {
        None = 0,
        Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004
    }

    public delegate void HotKeyCallback();

    public delegate void HotKeyCallbackState(ref object state);

    public enum HKCategory
    {
        None = 0,
        Items,
        Targets,
        Agents,
        Dress,
        Macros,
        Spells,
        Skills,
        Misc,
        Friends,
        Scripts
    }

    public enum HKSubCat
    {
        None = 0,

        // spells: (these all are # oriented, so be careful)
        SpellOffset,
        FirstC,
        SecondC,
        ThirdC,
        FourthC,
        FifthC,
        SixthC,
        SeventhC,
        EighthC,
        NecroC = SpellOffset + 10,
        PaladinC = SpellOffset + 20,

        //items
        Potions,

        // misc:
        SpecialMoves,

        // more # oriented, CAUTION!
        BushidoC = SpellOffset + 40,
        NinjisuC = SpellOffset + 50,
        SpellWeaveC = SpellOffset + 60,
        MystC = SpellOffset + 65,
        MasteriesC = SpellOffset + 70,

        // pet commands
        PetCommands = 780, //1749

        SubTargetCriminal = 796,
        SubTargetEnemy,
        SubTargetFriendly,
        SubTargetGrey,
        SubTargetInnocent,
        SubTargetMurderer,
        SubTargetNonFriendly,
    }

    public class KeyData
    {
        private int _name;
        private string _sName;
        private HotKeyCallback _callback;
        private HotKeyCallbackState _callbackState;
        private bool _sendToUO;
        private int _key;
        private ModKeys _mod;
        private TreeNode _node;
        private object _state;
        private string _command;

        public object pState => _state;

        private DateTime _lastTriggerTime = DateTime.MinValue;

        public string DisplayName => _name != 0 ? Language.GetString(_name) : _sName;

        public int LocName => _name;

        public string StrName => _sName;

        public string Command
        {
            get => _command;
            set => _command = value;
        }

        public KeyData(int name, HKCategory cat, HKSubCat sub, HotKeyCallback call)
        {
            _name = name;
            _sName = null;
            _callback = call;
            _callbackState = null;
            _node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public KeyData(int name, HKCategory cat, HKSubCat sub, HotKeyCallbackState call, object state)
        {
            _name = name;
            _sName = null;
            _callback = null;
            _callbackState = call;
            _state = state;
            _node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public KeyData(string name, HKCategory cat, HKSubCat sub, HotKeyCallback call)
        {
            _name = 0;
            _sName = name;
            _callback = call;
            _callbackState = null;
            _node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public KeyData(string name, HKCategory cat, HKSubCat sub, HotKeyCallbackState call, object state)
        {
            _name = 0;
            _sName = name;
            _callback = null;
            _callbackState = call;
            _state = state;
            _node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public void Callback()
        {
            // protect again weird keyboard oddities which "double press" the keys when the user only wanted to do an action once
            if (_lastTriggerTime + TimeSpan.FromMilliseconds(20) <= DateTime.UtcNow)
            {
                _lastTriggerTime = DateTime.UtcNow;

                if (_callback != null)
                    _callback();
                else if (_callbackState != null)
                    _callbackState(ref _state);
            }
        }

        public void Remove()
        {
            _node.Remove();
            _node.Text = "removed";
        }

        public bool SendToUO
        {
            get => _sendToUO;
            set => _sendToUO = value;
        }

        public int Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    _node.Text = ToString();
                }
            }
        }

        public ModKeys Mod
        {
            get => _mod;
            set
            {
                if (_mod != value)
                {
                    _mod = value;
                    _node.Text = ToString();
                }
            }
        }

        public string KeyString(bool smallNames = false)
        {
            if (Key != 0)
            {
                StringBuilder sb = new StringBuilder();
                bool np = false;

                if ((Mod & ModKeys.Control) != 0)
                {
                    sb.Append(smallNames ? "Ctrl" : "Control");

                    np = true;
                }

                if ((Mod & ModKeys.Alt) != 0)
                {
                    if (np)
                        sb.Append("+");
                    sb.Append("Alt");
                    np = true;
                }

                if ((Mod & ModKeys.Shift) != 0)
                {
                    if (np)
                        sb.Append("+");
                    sb.Append(smallNames ? "Sft" : "Shift");
                    np = true;
                }

                if (np)
                    sb.Append("+");

                switch (Key)
                {
                    case -1:
                        sb.Append("Wheel UP");
                        break;
                    case -2:
                        sb.Append("Wheel DOWN");
                        break;
                    case -3:
                        sb.Append("Middle Button");
                        break;
                    case -4:
                        sb.Append("XButton 1");
                        break;
                    case -5:
                        sb.Append("XButton 2");
                        break;
                    default:
                        sb.Append(((Keys) Key).ToString());
                        break;
                }

                return sb.ToString();
            }
            else
            {
                return null;
            }
        }


        public override string ToString()
        {
            if (Key != 0)
                return $"{this.DisplayName} ({KeyString()})";
            
            return $"{this.DisplayName} ({Language.GetString(LocString.NotAssigned)})";
        }
    }

    public class HotKey
    {
        private static List<KeyData> m_List = new List<KeyData>();
        private static TreeNode m_Root = new TreeNode("Hot Keys");
        private static bool m_Enabled;
        private static System.Windows.Forms.Label m_Status;
        private static KeyData m_HK_En;

        private static KeyData _hotKeyEnableToggle;
        private static KeyData _hotKeyDisableToggle;

        public static List<KeyData> List
        {
            get { return m_List; }
        }

        public static TreeNode RootNode
        {
            get { return m_Root; }
        }

        public static System.Windows.Forms.Label Status
        {
            get { return m_Status; }
            set
            {
                m_Status = value;
                UpdateStatus();
            }
        }


        public static bool KeyDown(Keys k)
        {
            return (Platform.GetAsyncKeyState((int) k) & 0xFF00) != 0; //|| Client.IsKeyDown( (int)k );
        }

        public static void Initialize()
        {
            m_HK_En = Add(HKCategory.None, LocString.ToggleHKEnable, new HotKeyCallback(OnToggleEnableDisable));

            _hotKeyEnableToggle = Add(HKCategory.None, LocString.EnableHotkeys, OnToggleEnable);
            _hotKeyDisableToggle = Add(HKCategory.None, LocString.DisableHotkeys, OnToggleDisable);
        }

        private static void OnToggleEnableDisable()
        {
            m_Enabled = !m_Enabled;

            MsgLevel type = MsgLevel.Warning;

            if (m_Enabled)
            {
                type = MsgLevel.Info;
            }

            if (World.Player != null)
                World.Player.SendMessage(type, UpdateStatus());
        }

        private static void OnToggleEnable()
        {
            if (m_Enabled)
            {
                if (World.Player != null)
                    World.Player.SendMessage(MsgLevel.Warning, "HotKeys are already enabled");
            }
            else
            {
                m_Enabled = !m_Enabled;
                if (World.Player != null)
                    World.Player.SendMessage(MsgLevel.Info, UpdateStatus());
            }
        }

        private static void OnToggleDisable()
        {
            if (!m_Enabled)
            {
                if (World.Player != null)
                    World.Player.SendMessage(MsgLevel.Warning, "HotKeys are already disabled");
            }
            else
            {
                m_Enabled = !m_Enabled;
                if (World.Player != null)
                    World.Player.SendMessage(MsgLevel.Warning, UpdateStatus());
            }
        }


        public static string UpdateStatus()
        {
            KeyData kd = Get((int) LocString.ToggleHKEnable);
            string ks = null;
            if (kd != null)
                ks = kd.KeyString();

            string msg;
            if (ks != null)
            {
                if (m_Enabled)
                    msg = Language.Format(LocString.HKEnabledPress, ks);
                else
                    msg = Language.Format(LocString.HKDisabledPress, ks);
            }
            else
            {
                if (m_Enabled)
                    msg = Language.GetString(LocString.HKEnabled);
                else
                    msg = Language.GetString(LocString.HKDisabled);
            }

            m_Status?.SafeAction(s => { s.Text = msg; });

            return msg;
        }

        public static bool ValidateCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return false;
            }

            foreach (KeyData keyData in List)
            {
                if (!string.IsNullOrEmpty(keyData.Command) && keyData.Command.Equals(command, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            foreach (string cmd in Command.GetCommands)
            {
                if (!string.IsNullOrEmpty(cmd) && cmd.Equals(command, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ExecuteCommand(string command)
        {
            foreach (KeyData keyData in List)
            {
                if (!string.IsNullOrEmpty(keyData.Command) && keyData.Command.Equals(command, StringComparison.OrdinalIgnoreCase))
                {
                    keyData.Callback();
                    return true;
                }
            }

            return false;
        }

        static HotKey()
        {
            m_Enabled = true;

            m_Root.Tag = (int) LocString.HotKeys;

            TreeNode items = MakeNode("Items", HKCategory.Items);

            MakeNode(items, "Potions", HKSubCat.Potions);

            TreeNode targets = MakeNode("Targets", HKCategory.Targets);

            MakeNode(targets, "Criminal Targets", HKSubCat.SubTargetCriminal);
            MakeNode(targets, "Enemy Targets", HKSubCat.SubTargetEnemy);
            MakeNode(targets, "Friendly Targets", HKSubCat.SubTargetFriendly);
            MakeNode(targets, "Grey Targets", HKSubCat.SubTargetGrey);
            MakeNode(targets, "Innocent Targets", HKSubCat.SubTargetInnocent);
            MakeNode(targets, "Murderer Targets", HKSubCat.SubTargetMurderer);
            MakeNode(targets, "Non-Friendly Targets", HKSubCat.SubTargetNonFriendly);

            MakeNode("Agents", HKCategory.Agents);

            MakeNode("Dress", HKCategory.Dress);

            MakeNode("Macros", HKCategory.Macros);

            TreeNode spells = MakeNode("Spells", HKCategory.Spells);
            MakeNode(spells, "1st", HKSubCat.FirstC);
            MakeNode(spells, "2nd", HKSubCat.SecondC);
            MakeNode(spells, "3rd", HKSubCat.ThirdC);
            MakeNode(spells, "4th", HKSubCat.FourthC);
            MakeNode(spells, "5th", HKSubCat.FifthC);
            MakeNode(spells, "6th", HKSubCat.SixthC);
            MakeNode(spells, "7th", HKSubCat.SeventhC);
            MakeNode(spells, "8th", HKSubCat.EighthC);
            MakeNode(spells, "Necro", HKSubCat.NecroC);
            MakeNode(spells, "Paladin", HKSubCat.PaladinC);
            MakeNode(spells, "Bushido", HKSubCat.BushidoC);
            MakeNode(spells, "Ninjisu", HKSubCat.NinjisuC);
            MakeNode(spells, "Spellweaving", HKSubCat.SpellWeaveC);
            MakeNode(spells, "Mysticism", HKSubCat.MystC);
            MakeNode(spells, "Masteries", HKSubCat.MasteriesC);

            MakeNode("Skills", HKCategory.Skills);

            TreeNode misc = MakeNode("Misc", HKCategory.Misc);
            MakeNode(misc, "Special Moves", HKSubCat.SpecialMoves);
            MakeNode(misc, "Pet Commands", HKSubCat.PetCommands);

            MakeNode("Friends", HKCategory.Friends);
            MakeNode("Scripts", HKCategory.Scripts);
        }

        public static void RebuildList(TreeView tree)
        {
            tree.BeginUpdate();
            UpdateNode(m_Root);

            tree.Nodes.Clear();
            tree.Nodes.Add(m_Root);
            m_Root.Expand();
            tree.EndUpdate();
        }

        private static void UpdateNode(TreeNode node)
        {
            if (node != null)
            {
                if (node.Tag is KeyData)
                    node.Text = ((KeyData) node.Tag).ToString();
                else if (node.Tag is HKCategory)
                    node.Text = Language.GetString((int) LocString.HotKeys + (int) ((HKCategory) node.Tag));
                else if (node.Tag is HKSubCat)
                    node.Text = Language.GetString(GetHKSubCatLangKey((HKSubCat) node.Tag));
                else if (node.Tag is Int32)
                    node.Text = Language.GetString((int) node.Tag);

                if (node != m_Root && node.NextNode != null)
                    UpdateNode(node.NextNode);
                if (node.GetNodeCount(true) > 0)
                    UpdateNode(node.FirstNode);
            }
        }

        private static int GetHKSubCatLangKey(HKSubCat tag)
        {
            switch (tag)
            {
                case HKSubCat.MystC:
                    return 2103;
                case HKSubCat.MasteriesC:
                    return 2104;
                default:
                    return (int)LocString.HKSubOffset + (int) tag;
            }
        }

        public static void ClearAll()
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                KeyData kd = m_List[i];
                kd.Key = 0;
                kd.Mod = ModKeys.None;
                kd.SendToUO = false;
            }
        }

        public static KeyData GetFromObj(object other)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                if (m_List[i].pState == other)
                    return m_List[i];
            }

            return null;
        }

        public static KeyData Get(int name)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                if (((KeyData) m_List[i]).LocName == name)
                    return (KeyData) m_List[i];
            }

            return null;
        }

        public static KeyData Get(string name)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                if (((KeyData) m_List[i]).StrName == name)
                    return (KeyData) m_List[i];
            }

            return null;
        }

        public static KeyData GetByNameOrId(string query)
        {
            foreach (KeyData key in m_List)
            {
                if (key.DisplayName.ToLower().Equals(query.ToLower()))
                    return key;
            }

            int hkIndex = Utility.ToInt32(query, -1);

            return hkIndex != -1 ? m_List[hkIndex] : null;
        }

        public static KeyData Get(int key, ModKeys mod)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                KeyData hk = (KeyData) m_List[i];
                if (hk.Key == key && hk.Mod == mod && hk.Key != 0)
                    return hk;
            }

            return null;
        }

        public static TreeNode FindParent(TreeNode root, HKCategory cat, HKSubCat sub)
        {
            TreeNode parent = root;
            if (cat != HKCategory.None)
            {
                parent = FindNode(root, cat);
                if (sub != HKSubCat.None && parent != null)
                {
                    TreeNode subNode = FindNode(parent, sub);
                    if (subNode != null)
                        parent = subNode;
                }
            }

            return parent;
        }

        public static TreeNode FindParent(HKCategory cat, HKSubCat sub)
        {
            return FindParent(m_Root, cat, sub);
        }

        public static TreeNode MakeNode(TreeNode parent, string text, object tag)
        {
            TreeNode n = new TreeNode(text);
            n.Tag = tag;
            parent.Nodes.Add(n);
            return n;
        }

        public static TreeNode MakeNode(string text, object tag)
        {
            return MakeNode(m_Root, text, tag);
        }

        public static TreeNode FindNode(TreeNode parent, object tag, bool recurse)
        {
            if (tag == null)
                return null;

            TreeNode n = parent.FirstNode;
            while (n != null)
            {
                if (tag.ToString() == n.Tag.ToString())
                    break;

                if (recurse)
                {
                    TreeNode r = FindNode(n, tag, true);
                    if (r != null)
                        return r;
                }

                n = n.NextNode;
            }

            return n;
        }

        public static TreeNode FindNode(TreeNode parent, object tag)
        {
            return FindNode(parent, tag, false);
        }

        public static bool OnKeyDown(int key, ModKeys mod)
        {
            if (World.Player == null)
                return true;
            ModKeys cur = mod;
            if (KeyDown(Keys.ControlKey))
                cur |= ModKeys.Control;
            if (KeyDown(Keys.Menu))
                cur |= ModKeys.Alt;
            if (KeyDown(Keys.ShiftKey))
                cur |= ModKeys.Shift;

            if (m_HK_En != null && m_HK_En.Key > 0 && m_HK_En.Mod == cur &&
                (m_HK_En.Key == key || KeyDown((Keys) m_HK_En.Key)))
            {
                m_HK_En.Callback();
                return m_HK_En.SendToUO;
            }

            if (_hotKeyEnableToggle != null && _hotKeyEnableToggle.Key > 0 && _hotKeyEnableToggle.Mod == cur &&
                (_hotKeyEnableToggle.Key == key || KeyDown((Keys) _hotKeyEnableToggle.Key)))
            {
                _hotKeyEnableToggle.Callback();
                return _hotKeyEnableToggle.SendToUO;
            }

            if (_hotKeyDisableToggle != null && _hotKeyDisableToggle.Key > 0 && _hotKeyDisableToggle.Mod == cur &&
                (_hotKeyDisableToggle.Key == key || KeyDown((Keys) _hotKeyDisableToggle.Key)))
            {
                _hotKeyDisableToggle.Callback();
                return _hotKeyDisableToggle.SendToUO;
            }

            if (m_Enabled)
            {
                for (int i = 0; i < m_List.Count; i++)
                {
                    KeyData hk = (KeyData) m_List[i];
                    if (hk.Mod == cur && hk.Key > 0)
                    {
                        if (hk.Key == key)
                        {
                            if (Macros.MacroManager.AcceptActions)
                                Macros.MacroManager.Action(new Macros.HotKeyAction(hk));

                            ScriptManager.AddToScript($"hotkey '{hk.DisplayName}'");

                            hk.Callback();
                            return hk.SendToUO;
                        }
                    }
                }

                // if pressed key didn't match a hotkey, check for any key currently down
                for (int i = 0; i < m_List.Count; i++)
                {
                    KeyData hk = (KeyData) m_List[i];
                    if (hk.Mod == cur && hk.Key > 0)
                    {
                        if (KeyDown((Keys) hk.Key))
                        {
                            if (Macros.MacroManager.AcceptActions)
                                Macros.MacroManager.Action(new Macros.HotKeyAction(hk));

                            ScriptManager.AddToScript($"hotkey '{hk.DisplayName}'");

                            hk.Callback();
                            return hk.SendToUO;
                        }
                    }
                }
            }

            return true;
        }

        public static void OnMouse(int button, int wheel)
        {
            if (World.Player == null || !m_Enabled)
                return;

            ModKeys cur = ModKeys.None;
            int key = 0;
            switch (button)
            {
                case 0:
                    key = wheel > 0 ? -1 : -2;
                    break;
                case 1:
                    key = -3;
                    break;
                case 2:
                    key = -4;
                    break;
                case 3:
                    key = -5;
                    break;
            }

            if (KeyDown(Keys.ControlKey))
                cur |= ModKeys.Control;
            if (KeyDown(Keys.Menu))
                cur |= ModKeys.Alt;
            if (KeyDown(Keys.ShiftKey))
                cur |= ModKeys.Shift;

            KeyData hk = Get(key, cur);
            if (hk != null)
                hk.Callback();
        }

        public static KeyData Add(HKCategory cat, int name, HotKeyCallback callback)
        {
            return Add(cat, HKSubCat.None, name, callback);
        }

        public static KeyData Add(HKCategory cat, LocString name, HotKeyCallback callback)
        {
            return Add(cat, HKSubCat.None, (int) name, callback);
        }

        public static KeyData Add(HKCategory cat, HKSubCat sub, int name, HotKeyCallback callback)
        {
            KeyData kd = new KeyData(name, cat, sub, callback);
            m_List.Add(kd);
            return kd;
        }

        public static KeyData Add(HKCategory cat, HKSubCat sub, LocString name, HotKeyCallback callback)
        {
            KeyData kd = new KeyData((int) name, cat, sub, callback);
            m_List.Add(kd);
            return kd;
        }

        public static KeyData Add(HKCategory cat, HKSubCat sub, string name, HotKeyCallback callback)
        {
            KeyData kd = new KeyData(name, cat, sub, callback);
            m_List.Add(kd);
            return kd;
        }

        public static KeyData Add(HKCategory cat, int name, HotKeyCallbackState callback, object state)
        {
            return Add(cat, HKSubCat.None, name, callback, state);
        }

        public static KeyData Add(HKCategory cat, LocString name, HotKeyCallbackState callback, object state)
        {
            return Add(cat, HKSubCat.None, (int) name, callback, state);
        }

        public static KeyData Add(HKCategory cat, HKSubCat sub, int name, HotKeyCallbackState callback, object state)
        {
            KeyData kd = new KeyData(name, cat, sub, callback, state);
            m_List.Add(kd);
            return kd;
        }

        public static KeyData Add(HKCategory cat, HKSubCat sub, LocString name, HotKeyCallbackState callback,
            object state)
        {
            KeyData kd = new KeyData((int) name, cat, sub, callback, state);
            m_List.Add(kd);
            return kd;
        }

        public static KeyData Add(HKCategory cat, HKSubCat sub, string name, HotKeyCallbackState callback, object state)
        {
            KeyData kd = new KeyData(name, cat, sub, callback, state);
            m_List.Add(kd);
            return kd;
        }

        public static bool Remove(int name)
        {
            if (name == 0)
                return false;

            for (int i = 0; i < m_List.Count; i++)
            {
                KeyData hk = (KeyData) m_List[i];
                if (hk.LocName == name)
                {
                    hk.Remove();
                    m_List.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public static bool Remove(string name)
        {
            if (name == null)
                return false;

            for (int i = 0; i < m_List.Count; i++)
            {
                KeyData hk = (KeyData) m_List[i];
                if (hk.StrName == name)
                {
                    hk.Remove();
                    m_List.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public static void Load(XmlElement xml)
        {
            if (xml == null)
                return;

            ClearAll();

            foreach (XmlElement el in xml.GetElementsByTagName("key"))
            {
                try
                {
                    string name = el.GetAttribute("name");
                    string loc = el.GetAttribute("loc");
                    string mod = el.GetAttribute("mod");
                    string key = el.GetAttribute("key");
                    string send = el.GetAttribute("send");
                    string command = el.GetAttribute("command");

                    if (!ValidateCommand(command))
                    {
                        command = string.Empty;
                    }

                    KeyData k = null;

                    if (loc != null && loc.Length > 0)
                    {
                        k = Get(Convert.ToInt32(loc));
                    }
                    else if (name != null && name.Length > 0)
                    {
                        k = Get(name);
                    }
                    else if (el.InnerText != null && el.InnerText.Length > 1)
                    {
                        if (el.InnerText[0] == 'L' && el.InnerText[1] == ':')
                        {
                            try
                            {
                                k = Get(Convert.ToInt32(el.InnerText.Substring(2)));
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        else
                        {
                            k = Get(el.InnerText);
                        }
                    }

                    if (k != null)
                    {
                        k.Mod = (ModKeys) Convert.ToInt32(mod);
                        k.Key = Convert.ToInt32(key);
                        k.SendToUO = Convert.ToBoolean(send);
                        k.Command = command;
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void Save(XmlTextWriter xml)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                KeyData k = (KeyData) m_List[i];

                if (k.Key != 0)
                {
                    xml.WriteStartElement("key");
                    xml.WriteAttributeString("mod", ((int) k.Mod).ToString());
                    xml.WriteAttributeString("key", k.Key.ToString());
                    xml.WriteAttributeString("send", k.SendToUO.ToString());
                    xml.WriteAttributeString("command", string.IsNullOrEmpty(k.Command) ? string.Empty : k.Command);

                    //xml.WriteAttributeString( "name", ((int)k.LocName).ToString() );
                    if (k.LocName != 0)
                        xml.WriteString("L:" + k.LocName.ToString());
                    else
                        xml.WriteString(k.StrName); //xml.WriteAttributeString( "name", k.StrName );

                    xml.WriteEndElement();
                }
            }
        }
    }
}
