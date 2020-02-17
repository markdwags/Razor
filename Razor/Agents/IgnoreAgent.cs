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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Agents
{
    public class IgnoreAgent : Agent
    {
        public static IgnoreAgent Instance { get; private set; }

        public static void Initialize()
        {
            Agent.Add(Instance = new IgnoreAgent());
        }

        public static bool IsIgnored(Serial ser)
        {
            return Instance?.IsSerialIgnored(ser) ?? false;
        }

        private ListBox m_SubList;
        private readonly List<Serial> m_Chars;
        private readonly Dictionary<Serial, string> m_Names;
        private static bool m_Enabled;
        private Button m_EnableBTN;

        public IgnoreAgent()
        {
            m_Chars = new List<Serial>();
            m_Names = new Dictionary<Serial, string>();

            HotKey.Add(HKCategory.Targets, LocString.AddToIgnore, new HotKeyCallback(AddToIgnoreList));
            HotKey.Add(HKCategory.Targets, LocString.RemoveFromIgnore, new HotKeyCallback(RemoveFromIgnoreList));

            Agent.OnMobileCreated += new MobileCreatedEventHandler(OPLCheckIgnore);
        }

        public override void Clear()
        {
            m_Chars.Clear();
            m_Names.Clear();
        }

        public static bool IsEnabled()
        {
            return m_Enabled;
        }

        public bool IsSerialIgnored(Serial ser)
        {
            if (m_Enabled)
            {
                return m_Chars.Contains(ser);
            }
            else
            {
                return false;
            }
        }

        public override string Name
        {
            get { return Language.GetString(LocString.IgnoreAgent); }
        }

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            m_EnableBTN = buttons[4];

            buttons[0].Text = Language.GetString(LocString.AddTarg);
            buttons[0].Visible = true;
            buttons[1].Text = Language.GetString(LocString.Remove);
            buttons[1].Visible = true;
            buttons[2].Text = Language.GetString(LocString.RemoveTarg);
            buttons[2].Visible = true;
            buttons[3].Text = Language.GetString(LocString.ClearList);
            buttons[3].Visible = true;
            buttons[4].Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
            buttons[4].Visible = true;

            m_SubList = subList;
            subList.BeginUpdate();
            subList.Items.Clear();
            for (int i = 0; i < m_Chars.Count; i++)
            {
                Add2List((Serial) m_Chars[i]);
            }

            subList.EndUpdate();
        }

        public void AddToIgnoreList()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.AddToIgnore);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnAddTarget));
        }

        public void RemoveFromIgnoreList()
        {
            World.Player.SendMessage(MsgLevel.Force, LocString.RemoveFromIgnore);
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnRemoveTarget));
        }

        public override void OnButtonPress(int num)
        {
            switch (num)
            {
                case 1:
                {
                    AddToIgnoreList();
                    break;
                }

                case 2:
                {
                    if (m_SubList.SelectedIndex >= 0 && m_SubList.SelectedIndex < m_Chars.Count)
                    {
                        try
                        {
                            m_Names.Remove(m_Chars[m_SubList.SelectedIndex]);
                        }
                        catch
                        {
                        }

                        m_Chars.RemoveAt(m_SubList.SelectedIndex);
                        m_SubList.Items.RemoveAt(m_SubList.SelectedIndex);
                    }

                    break;
                }

                case 3:
                {
                    RemoveFromIgnoreList();
                    break;
                }

                case 4:
                {
                    if (MessageBox.Show(Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (Serial s in m_Chars)
                        {
                            Mobile m = World.FindMobile(s);
                            if (m != null)
                            {
                                if (m.ObjPropList.Remove(Language.GetString(LocString.RazorIgnored)))
                                {
                                    m.OPLChanged();
                                }
                            }
                        }

                        m_Chars.Clear();
                        m_SubList.Items.Clear();
                    }

                    break;
                }

                case 5:
                {
                    m_Enabled = !m_Enabled;
                    m_EnableBTN.Text = Language.GetString(m_Enabled ? LocString.PushDisable : LocString.PushEnable);
                    break;
                }
            }
        }

        private void OPLCheckIgnore(Mobile m)
        {
            if (IsIgnored(m.Serial))
            {
                m.ObjPropList.Add(Language.GetString(LocString.RazorIgnored));
            }
        }

        private void OnAddTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (!location && serial.IsMobile && serial != World.Player.Serial)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.AddToIgnore);
                if (!m_Chars.Contains(serial))
                {
                    m_Chars.Add(serial);

                    Add2List(serial);

                    Mobile m = World.FindMobile(serial);
                    if (m != null)
                    {
                        m.ObjPropList.Add(Language.GetString(LocString.RazorIgnored));
                        m.OPLChanged();
                    }
                }
            }
        }

        private void Add2List(Serial s)
        {
            Mobile m = World.FindMobile(s);
            string name = null;

            if (m_Names.ContainsKey(s))
            {
                name = m_Names[s] as string;
            }

            if (m != null && m.Name != null && m.Name != "")
            {
                name = m.Name;
            }

            if (name == null)
            {
                name = "(Name Unknown)";
            }

            m_Names[s] = name;

            if (m_SubList != null)
            {
                m_SubList.Items.Add($"\"{name}\" {s}");
            }
        }

        private void OnRemoveTarget(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (!location && serial.IsMobile && serial != World.Player.Serial)
            {
                m_Chars.Remove(serial);
                m_Names.Remove(serial);

                World.Player.SendMessage(MsgLevel.Force, LocString.RemoveFromIgnore);

                m_SubList.BeginUpdate();
                m_SubList.Items.Clear();
                for (int i = 0; i < m_Chars.Count; i++)
                {
                    Add2List((Serial) m_Chars[i]);
                }

                m_SubList.EndUpdate();

                Mobile m = World.FindMobile(serial);
                if (m != null)
                {
                    if (m.ObjPropList.Remove(Language.GetString(LocString.RazorIgnored)))
                    {
                        m.OPLChanged();
                    }
                }
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            xml.WriteAttributeString("enabled", m_Enabled.ToString());
            for (int i = 0; i < m_Chars.Count; i++)
            {
                xml.WriteStartElement("ignore");
                xml.WriteAttributeString("serial", m_Chars[i].ToString());
                try
                {
                    if (m_Names.ContainsKey((Serial) m_Chars[i]))
                    {
                        xml.WriteAttributeString("name", m_Names[(Serial) m_Chars[i]].ToString());
                    }
                }
                catch
                {
                }

                xml.WriteEndElement();
            }
        }

        public override void Load(XmlElement node)
        {
            try
            {
                m_Enabled = Convert.ToBoolean(node.GetAttribute("enabled"));
            }
            catch
            {
            }

            foreach (XmlElement el in node.GetElementsByTagName("ignore"))
            {
                try
                {
                    Serial toAdd = Serial.Parse(el.GetAttribute("serial"));

                    if (!m_Chars.Contains(toAdd))
                    {
                        m_Chars.Add(toAdd);
                    }

                    string name = el.GetAttribute("name");
                    if (name != null && name != "")
                    {
                        m_Names.Add(toAdd, name.Trim());
                    }
                }
                catch
                {
                }
            }
        }
    }
}