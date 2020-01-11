using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using System.Xml;
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
        Friends
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
        private int m_Name;
        private string m_SName;
        private HotKeyCallback m_Callback;
        private HotKeyCallbackState m_CallbackState;
        private bool m_SendToUO;
        private int m_Key;
        private ModKeys m_Mod;
        private TreeNode m_Node;
        private object m_State;

        public object pState
        {
            get { return m_State; }
        }

        private DateTime m_LastTriggerTime = DateTime.MinValue;

        public string DispName
        {
            get { return m_Name != 0 ? Language.GetString(m_Name) : m_SName; }
        }

        public int LocName
        {
            get { return m_Name; }
        }

        public string StrName
        {
            get { return m_SName; }
        }
        //public HotKeyCallback Callback{ get{ return m_Callback; } }

        public KeyData(int name, HKCategory cat, HKSubCat sub, HotKeyCallback call)
        {
            m_Name = name;
            m_SName = null;
            m_Callback = call;
            m_CallbackState = null;
            m_Node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public KeyData(int name, HKCategory cat, HKSubCat sub, HotKeyCallbackState call, object state)
        {
            m_Name = name;
            m_SName = null;
            m_Callback = null;
            m_CallbackState = call;
            m_State = state;
            m_Node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public KeyData(string name, HKCategory cat, HKSubCat sub, HotKeyCallback call)
        {
            m_Name = 0;
            m_SName = name;
            m_Callback = call;
            m_CallbackState = null;
            m_Node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public KeyData(string name, HKCategory cat, HKSubCat sub, HotKeyCallbackState call, object state)
        {
            m_Name = 0;
            m_SName = name;
            m_Callback = null;
            m_CallbackState = call;
            m_State = state;
            m_Node = HotKey.MakeNode(HotKey.FindParent(cat, sub), ToString(), this);
        }

        public void Callback()
        {
            // protect again weird keyboard oddities which "double press" the keys when the user only wanted to do an action once
            if (m_LastTriggerTime + TimeSpan.FromMilliseconds(20) <= DateTime.UtcNow)
            {
                m_LastTriggerTime = DateTime.UtcNow;

                if (m_Callback != null)
                    m_Callback();
                else if (m_CallbackState != null)
                    m_CallbackState(ref m_State);
            }
        }

        public void Remove()
        {
            m_Node.Remove();
            m_Node.Text = "removed";
        }

        public bool SendToUO
        {
            get { return m_SendToUO; }
            set { m_SendToUO = value; }
        }

        public int Key
        {
            get { return m_Key; }
            set
            {
                if (m_Key != value)
                {
                    m_Key = value;
                    m_Node.Text = ToString();
                }
            }
        }

        public ModKeys Mod
        {
            get { return m_Mod; }
            set
            {
                if (m_Mod != value)
                {
                    m_Mod = value;
                    m_Node.Text = ToString();
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
                return String.Format("{0} ({1})", this.DispName, KeyString());
            else
                return String.Format("{0} ({1})", this.DispName, Language.GetString(LocString.NotAssigned));
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

            MakeNode("Skills", HKCategory.Skills);

            TreeNode misc = MakeNode("Misc", HKCategory.Misc);
            MakeNode(misc, "Special Moves", HKSubCat.SpecialMoves);
            MakeNode(misc, "Pet Commands", HKSubCat.PetCommands);

            MakeNode("Friends", HKCategory.Friends);
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
                    node.Text = Language.GetString((int) LocString.HKSubOffset + (int) ((HKSubCat) node.Tag));
                else if (node.Tag is Int32)
                    node.Text = Language.GetString((int) node.Tag);

                if (node != m_Root && node.NextNode != null)
                    UpdateNode(node.NextNode);
                if (node.GetNodeCount(true) > 0)
                    UpdateNode(node.FirstNode);
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

        private static int GetSpellID(int circle, int num)
        {
            if (circle <= 8) // Mage
                return 3002011 + ((circle - 1) * 8) + (num - 1);
            else if (circle == 10) // Necr
                return 1060509 + num - 1;
            else if (circle == 20) // Chiv
                return 1060585 + num - 1;
            else if (circle == 40) // Bush
                return 1060595 + num - 1;
            else if (circle == 50) // Ninj
                return 1060610 + num - 1;
            else if (circle == 60) // Elfs
                return 1071026 + num - 1;
            else
                return -1;
        }

        private static Dictionary<string, LocString> SteamActionMappings = new Dictionary<string, LocString>()
        {
            // main.pingserver
            { "main.resynchronize", LocString.Resync },
            // main.snapshot
            { "actions.grabitem", LocString.GrabItem },
            // main.togglemounted
            { "actions.use.lastobject", LocString.LastObj },
            { "actions.use.lefthand", LocString.UseLeftHand },
            { "actions.use.righthand", LocString.UseRightHand },
            { "actions.shownames.all", LocString.AllNames },
            { "actions.shownames.corpses", LocString.AllCorpses },
            { "actions.shownames.mobiles", LocString.AllMobiles },
            { "actions.creatures.come", LocString.AllCome },
            { "actions.creatures.follow", LocString.AllFollow },
            { "actions.creatures.guard", LocString.AllGuard },
            { "actions.creatures.kill", LocString.AllKill },
            { "actions.creatures.stay", LocString.AllStay },
            { "actions.creatures.stop", LocString.AllStop },
            { "skills.last", LocString.LastSkill },
            { "skills.anatomy", (LocString)(1044060 + 1) },
            { "skills.animallore", (LocString)(1044060 + 2) },
            { "skills.itemidentification",  (LocString)(1044060 + 3) },
            { "skills.armslore", (LocString)(1044060 + 4) },
            { "skills.begging", (LocString)(1044060 + 6) },
            { "skills.peacemaking", (LocString)(1044060 + 9) },
            { "skills.detectinghidden", (LocString)(1044060 + 14) },
            { "skills.discordance", (LocString)(1044060 + 15) },
            { "skills.evaluatingintelligence", (LocString)(1044060 + 16) },
            { "skills.hiding", (LocString)(1044060 + 21) },
            { "skills.provocation", (LocString)(1044060 + 22) },
            { "skills.inscription", (LocString)(1044060 + 23) },
            { "skills.poisoning", (LocString)(1044060 + 30) },
            { "skills.spiritspeak", (LocString)(1044060 + 32) },
            { "skills.stealing", (LocString)(1044060 + 33) },
            { "skills.animaltaming", (LocString)(1044060 + 35) },
            { "skills.tasteidentification", (LocString)(1044060 + 36) },
            { "skills.tracking", (LocString)(1044060 + 38) },
            { "skills.meditation", (LocString)(1044060 + 46) },
            { "skills.stealth", (LocString)(1044060 + 47) },
            { "skills.removetrap", (LocString)(1044060 + 48) },
            { "spells.last", LocString.LastSpell },
            { "spells.bigheal.self", LocString.GHealOrCureSelf },
            { "spells.miniheal.self", LocString.MiniHealOrCureSelf },
            { "spells.magery.clumsy", (LocString)GetSpellID(1, 1) },
            { "spells.magery.createfood", (LocString)GetSpellID(1, 2) },
            { "spells.magery.feeblemind", (LocString)GetSpellID(1, 3) },
            { "spells.magery.heal", (LocString)GetSpellID(1, 4) },
            { "spells.magery.magicarrow", (LocString)GetSpellID(1, 5) },
            { "spells.magery.nightsight", (LocString)GetSpellID(1, 6) },
            { "spells.magery.reactivearmor", (LocString)GetSpellID(1, 7) },
            { "spells.magery.weaken", (LocString)GetSpellID(1, 8) },
            { "spells.magery.agility", (LocString)GetSpellID(2, 1) },
            { "spells.magery.cunning", (LocString)GetSpellID(2, 2) },
            { "spells.magery.cure", (LocString)GetSpellID(2, 3) },
            { "spells.magery.harm", (LocString)GetSpellID(2, 4) },
            { "spells.magery.magictrap", (LocString)GetSpellID(2, 5) },
            { "spells.magery.magicuntrap", (LocString)GetSpellID(2, 6) },
            { "spells.magery.protection", (LocString)GetSpellID(2, 7) },
            { "spells.magery.strength", (LocString)GetSpellID(2, 8) },            
            { "spells.magery.bless", (LocString)GetSpellID(3, 1) },
            { "spells.magery.fireball", (LocString)GetSpellID(3, 2) },
            { "spells.magery.magiclock", (LocString)GetSpellID(3, 3) },
            { "spells.magery.poison", (LocString)GetSpellID(3, 4) },
            { "spells.magery.telekinesis", (LocString)GetSpellID(3, 5) },
            { "spells.magery.teleport", (LocString)GetSpellID(3, 6) },
            { "spells.magery.unlock", (LocString)GetSpellID(3, 7) },
            { "spells.magery.wallofstone", (LocString)GetSpellID(3, 8) },
            { "spells.magery.archcure", (LocString)GetSpellID(4, 1) },
            { "spells.magery.archprotection", (LocString)GetSpellID(4, 2) },
            { "spells.magery.curse", (LocString)GetSpellID(4, 3) },
            { "spells.magery.firefield", (LocString)GetSpellID(4, 4) },
            { "spells.magery.greaterheal", (LocString)GetSpellID(4, 5) },
            { "spells.magery.lightning", (LocString)GetSpellID(4, 6) },
            { "spells.magery.manadrain", (LocString)GetSpellID(4, 7) },
            { "spells.magery.recall", (LocString)GetSpellID(4, 8) },
            { "spells.magery.bladespirits", (LocString)GetSpellID(5, 1) },
            { "spells.magery.dispelfield", (LocString)GetSpellID(5, 2) },
            { "spells.magery.incognito", (LocString)GetSpellID(5, 3) },
            { "spells.magery.magicreflection", (LocString)GetSpellID(5, 4) },
            { "spells.magery.mindblast", (LocString)GetSpellID(5, 5) },
            { "spells.magery.paralyze", (LocString)GetSpellID(5, 6) },
            { "spells.magery.poisonfield", (LocString)GetSpellID(5, 7) },
            { "spells.magery.summoncreature", (LocString)GetSpellID(5, 8) },
            { "spells.magery.dispel", (LocString)GetSpellID(6, 1) },
            { "spells.magery.energybolt", (LocString)GetSpellID(6, 2) },
            { "spells.magery.explosion", (LocString)GetSpellID(6, 3) },
            { "spells.magery.invisibility", (LocString)GetSpellID(6, 4) },
            { "spells.magery.mark", (LocString)GetSpellID(6, 5) },
            { "spells.magery.masscurse", (LocString)GetSpellID(6, 6) },
            { "spells.magery.paralyzefield", (LocString)GetSpellID(6, 7) },
            { "spells.magery.reveal", (LocString)GetSpellID(6, 8) },
            { "spells.magery.chainlightning", (LocString)GetSpellID(7, 1) },
            { "spells.magery.energyfield", (LocString)GetSpellID(7, 2) },
            { "spells.magery.flamestrike", (LocString)GetSpellID(7, 3) },
            { "spells.magery.gatetravel", (LocString)GetSpellID(7, 4) },
            { "spells.magery.manavampire", (LocString)GetSpellID(7, 5) },
            { "spells.magery.massdispel", (LocString)GetSpellID(7, 6) },
            { "spells.magery.meteorswarm", (LocString)GetSpellID(7, 7) },
            { "spells.magery.polymorph", (LocString)GetSpellID(7, 8) },
            { "spells.magery.summonairelemental", (LocString)GetSpellID(8, 1) },
            { "spells.magery.summonearthelemental", (LocString)GetSpellID(8, 2) },
            { "spells.magery.earthquake", (LocString)GetSpellID(8, 3) },
            { "spells.magery.energyvortex", (LocString)GetSpellID(8, 4) },
            { "spells.magery.summonfireelemental", (LocString)GetSpellID(8, 5) },
            { "spells.magery.resurrection", (LocString)GetSpellID(8, 6) },
            { "spells.magery.summondaemon", (LocString)GetSpellID(8, 7) },
            { "spells.magery.summonwaterelemental", (LocString)GetSpellID(8, 8) },
        };

        public static void ImportSteam(XmlElement xml)
        {
            if (xml == null)
                return;

            ClearAll();

            foreach (XmlElement el in xml.GetElementsByTagName("hotkey"))
            {
                try
                {
                    string action = el.GetAttribute("action");

                    if (!SteamActionMappings.ContainsKey(action))
                        continue;

                    int keyMask = Convert.ToInt32(el.GetAttribute("key"), 16);
                    int key = keyMask & 0xFF;

                    if (key == 4) key = -3;
                    else if (key == 259) key = -4;
                    else if (key == 260) key = -5;

                    ModKeys mod = ModKeys.None;

                    if ((keyMask & 0x200) != 0)
                        mod = ModKeys.Shift;
                    else if ((keyMask & 0x400) != 0)
                        mod = ModKeys.Control;
                    else if ((keyMask & 0x800) != 0)
                        mod = ModKeys.Alt;

                    string pass = el.GetAttribute("pass");

                    KeyData k = Get((int)SteamActionMappings[action]);

                    if (k != null)
                    {
                        k.Mod = mod;
                        k.Key = key;
                        k.SendToUO = Convert.ToBoolean(pass);
                    }
                }
                catch
                {
                }
            }
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
                    }
                }
                catch
                {
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