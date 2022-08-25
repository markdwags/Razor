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
using System.Xml;
using System.Windows.Forms;
using Assistant.Scripts;
using Assistant.UI;

namespace Assistant
{
    public class DressList
    {
        private static readonly List<DressList> m_List = new List<DressList>();
        private static ListBox _dressList;
        private static ListBox _dressItems;

        public static void SetControls(ListBox dressList, ListBox dressItems)
        {
            _dressList = dressList;
            _dressItems = dressItems;
        }

        public static void Redraw()
        {
            _dressList.SafeAction(s => s.Items.Clear());
            _dressItems.SafeAction(s => s.Items.Clear());

            _dressList.SafeAction(s => s.Items.AddRange(m_List.ToArray()));
        }

        public static void ClearAll()
        {
            _dressList?.SafeAction(s => s.Items.Clear());
            _dressItems?.SafeAction(s => s.Items.Clear());

            while (m_List.Count > 0)
                Remove(m_List[0]);
        }

        public static DressList Find(string name)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                DressList list = m_List[i];
                if (list.Name == name)
                    return list;
            }

            return null;
        }

        public static Item FindUndressBag(Item item)
        {
            Item undressBag = World.Player.Backpack;
            for (int i = 0; i < m_List.Count; i++)
            {
                DressList list = m_List[i];
                if (list != null && (list.Items.Contains(item.Serial) || list.Items.Contains(item.ItemID)))
                {
                    if (list.m_UndressBag.IsValid)
                    {
                        Item bag = World.FindItem(list.m_UndressBag);
                        if (bag != null && (bag.RootContainer == World.Player ||
                                            (bag.RootContainer == null && Utility.InRange(bag.GetWorldPosition(),
                                                 World.Player.Position, 2))))
                            undressBag = bag;
                    }

                    break;
                }
            }

            return undressBag;
        }

        public static void Load(XmlElement xml)
        {
            ClearAll();

            if (xml == null)
                return;

            foreach (XmlElement el in xml.GetElementsByTagName("list"))
            {
                try
                {
                    string name = el.GetAttribute("name");
                    DressList list = new DressList(name);
                    Add(list);

                    try
                    {
                        list.m_UndressBag = Serial.Parse(el.GetAttribute("undressbag"));
                    }
                    catch
                    {
                        list.m_UndressBag = Serial.Zero;
                    }

                    foreach (XmlElement el2 in el.GetElementsByTagName("item"))
                    {
                        try
                        {
                            string ser = el2.GetAttribute("serial");
                            list.Items.Add((Serial) Convert.ToUInt32(ser));
                        }
                        catch
                        {
                            try
                            {
                                string iid = el2.GetAttribute("id");
                                list.Items.Add((ItemID) Convert.ToUInt16(iid));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static void Save(XmlTextWriter xml)
        {
            foreach (DressList list in m_List)
            {
                xml.WriteStartElement("list");
                xml.WriteAttributeString("name", list.Name);

                if (list.m_UndressBag.IsItem)
                    xml.WriteAttributeString("undressbag", list.m_UndressBag.ToString());

                foreach (object o in list.Items)
                {
                    xml.WriteStartElement("item");
                    if (o is Serial)
                        xml.WriteAttributeString("serial", ((Serial) o).Value.ToString());
                    else if (o is ItemID)
                        xml.WriteAttributeString("id", ((ItemID) o).Value.ToString());
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
        }

        public static void Add(DressList list)
        {
            if (Engine.MainWindow == null)
            {
                m_List.Add(list);
                HotKey.Add(HKCategory.Dress, HKSubCat.None, $"Dress: {list.Name}",
                    new HotKeyCallback(list.Dress));
                HotKey.Add(HKCategory.Dress, HKSubCat.None, $"Undress: {list.Name}",
                    new HotKeyCallback(list.Undress));
                HotKey.Add(HKCategory.Dress, HKSubCat.None, $"Toggle: {list.Name}",
                    new HotKeyCallback(list.Toggle));
            }
            else
            {
                Engine.MainWindow.SafeAction(s =>
                {
                    m_List.Add(list);
                    HotKey.Add(HKCategory.Dress, HKSubCat.None, $"Dress: {list.Name}",
                        new HotKeyCallback(list.Dress));
                    HotKey.Add(HKCategory.Dress, HKSubCat.None, $"Undress: {list.Name}",
                        new HotKeyCallback(list.Undress));
                    HotKey.Add(HKCategory.Dress, HKSubCat.None, $"Toggle: {list.Name}",
                        new HotKeyCallback(list.Toggle));
                });
            }
        }

        public static void Remove(DressList list)
        {
            if (Engine.MainWindow == null)
            {
                m_List.Remove(list);
                HotKey.Remove($"Dress: {list.Name}");
                HotKey.Remove($"Undress: {list.Name}");
                HotKey.Remove($"Toggle: {list.Name}");
            }
            else
            {
                Engine.MainWindow.SafeAction(s =>
                {
                    m_List.Remove(list);
                    HotKey.Remove($"Dress: {list.Name}");
                    HotKey.Remove($"Undress: {list.Name}");
                    HotKey.Remove($"Toggle: {list.Name}");
                });
            }
        }

        private Serial m_UndressBag;

        public DressList(string name)
        {
            Name = name;
            Items = new List<object>();
            m_UndressBag = Serial.Zero;
        }

        public DressList(XmlElement xml)
        {
            Name = xml.GetAttribute("name");
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; }
        public List<object> Items { get; }

        public void SetUndressBag(Serial serial)
        {
            if (serial == World.Player.Backpack.Serial || !serial.IsItem)
                m_UndressBag = Serial.Zero;
            else
                m_UndressBag = serial;
        }

        public void Toggle()
        {
            if (World.Player == null)
                return;

            int worn = 0;
            int total = Items.Count;

            for (int i = 0; i < Items.Count; i++)
            {
                Item item = null;
                if (Items[i] is Serial)
                    item = World.FindItem((Serial) Items[i]);
                else if (Items[i] is ItemID)
                    item = World.Player.FindItemByID((ItemID) Items[i]);

                if (item == null)
                    total--;
                else if (item.Container == World.Player)
                    worn++;
            }

            if (Items.Count == 1)
            {
                if (worn != 0)
                    Undress();
                else
                    Dress();
            }
            else
            {
                if (worn > total / 2)
                    Undress();
                else
                    Dress();
            }
        }

        public void Undress()
        {
            if (World.Player == null)
                return;

            int count = 0;
            Item undressBag = World.Player.Backpack;
            if (undressBag == null)
            {
                World.Player.SendMessage(LocString.NoBackpack);
                return;
            }

            if (Macros.MacroManager.AcceptActions)
                Macros.MacroManager.Action(new Macros.UnDressAction(Name));

            if (ScriptManager.Recording)
                ScriptManager.AddToScript($"undress '{Name}'");

            if (m_UndressBag.IsValid)
            {
                Item bag = World.FindItem(m_UndressBag);
                if (bag != null && (bag.RootContainer == World.Player ||
                                    (bag.RootContainer == null && Utility.InRange(bag.GetWorldPosition(),
                                         World.Player.Position, 2))))
                    undressBag = bag;
                else
                    World.Player.SendMessage(LocString.UndressBagRange);
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Item item = null;
                if (Items[i] is Serial)
                    item = World.FindItem((Serial) Items[i]);
                else if (Items[i] is ItemID)
                    item = World.Player.FindItemByID((ItemID) Items[i]);

                if (item == null || DragDropManager.CancelDragFor(item.Serial) || item.Container != World.Player)
                {
                    continue;
                }
                else
                {
                    DragDropManager.DragDrop(item, undressBag);
                    count++;
                }
            }

            World.Player.SendMessage(LocString.UndressQueued, count);
        }

        public static Layer GetLayerFor(Item item)
        {
            Layer layer = item.Layer;
            if (layer == Layer.Invalid || layer > Layer.LastUserValid)
                layer = (Layer) item.ItemID.ItemData.Quality;

            return layer;
        }

        public void Dress()
        {
            if (World.Player == null)
                return;

            int skipped = 0, gone = 0, done = 0;
            List<Item> list = new List<Item>();
            bool remConflicts = Config.GetBool("UndressConflicts");

            if (World.Player.Backpack == null)
            {
                World.Player.SendMessage(LocString.NoBackpack);
                return;
            }

            if (Macros.MacroManager.AcceptActions)
                Macros.MacroManager.Action(new Macros.DressAction(Name));

            ScriptManager.AddToScript($"dress '{Name}'");

            for (int i = 0; i < Items.Count; i++)
            {
                Item item = null;
                if (Items[i] is Serial)
                {
                    item = World.FindItem((Serial) Items[i]);
                    if (item == null)
                        gone++;
                    else
                        list.Add(item);
                }
                else if (Items[i] is ItemID)
                {
                    ItemID id = (ItemID) Items[i];

                    // search to make sure they are not already wearing this...
                    item = World.Player.FindItemByID(id);
                    if (item != null)
                    {
                        skipped++;
                    }
                    else
                    {
                        item = World.Player.Backpack.FindItemById(id);
                        if (item == null)
                            gone++;
                        else
                            list.Add(item);
                    }
                }
            }

            foreach (Item item in list)
            {
                if (item.Container == World.Player)
                {
                    skipped++;
                }
                else if (item.IsChildOf(World.Player.Backpack) || item.RootContainer == null)
                {
                    Layer layer = GetLayerFor(item);
                    if (layer == Layer.Invalid || layer > Layer.LastUserValid)
                        continue;

                    if (remConflicts)
                    {
                        Item conflict = World.Player.GetItemOnLayer(layer);
                        if (conflict != null)
                            DragDropManager.DragDrop(conflict, FindUndressBag(conflict));

                        // try to also undress conflicting hand(s)
                        if (layer == Layer.RightHand)
                            conflict = World.Player.GetItemOnLayer(Layer.LeftHand);
                        else if (layer == Layer.LeftHand)
                            conflict = World.Player.GetItemOnLayer(Layer.RightHand);
                        else
                            conflict = null;

                        if (conflict != null && (conflict.IsTwoHanded || item.IsTwoHanded))
                            DragDropManager.DragDrop(conflict, FindUndressBag(conflict));
                    }

                    DragDropManager.DragDrop(item, World.Player, layer);
                    done++;
                }
            }

            if (done > 0)
                World.Player.SendMessage(LocString.DressQueued, done);
            if (skipped > 0)
                World.Player.SendMessage(LocString.AlreadyDressed, skipped);
            if (gone > 0)
                World.Player.SendMessage(LocString.ItemsNotFound, gone);
        }
    }
}