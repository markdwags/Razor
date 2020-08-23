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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Assistant.Core;
using Assistant.UI;
using Ultima;

namespace Assistant.Macros
{
    public delegate void MacroMenuCallback(object[] Args);

    public class MacroMenuItem : MenuItem
    {
        private MacroMenuCallback m_Call;
        private object[] m_Args;

        public MacroMenuItem(LocString name, MacroMenuCallback call, params object[] args) : base(
            Language.GetString(name))
        {
            base.Click += new EventHandler(OnMenuClick);
            m_Call = call;
            m_Args = args;
        }

        private void OnMenuClick(object sender, System.EventArgs e)
        {
            m_Call(m_Args);
        }
    }

    public abstract class MacroAction
    {
        protected Macro m_Parent;

        public MacroAction()
        {
        }

        public override string ToString()
        {
            return String.Format("?{0}?", GetType().Name);
        }

        public abstract string ToScript();

        public virtual string Serialize()
        {
            return GetType().FullName;
        }

        protected string DoSerialize(params object[] args)
        {
            StringBuilder sb = new StringBuilder(GetType().FullName);
            for (int i = 0; i < args.Length; i++)
                sb.AppendFormat("|{0}", args[i]);
            return sb.ToString();
        }

        public virtual MenuItem[] GetContextMenuItems()
        {
            return null;
        }

        public Macro Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

        public abstract bool Perform();
    }

    public abstract class MacroWaitAction : MacroAction
    {
        protected TimeSpan m_Timeout = TimeSpan.FromMinutes(5);
        private DateTime m_Start;
        private MacroMenuItem m_MenuItem = null;

        public MacroWaitAction()
        {
        }

        public abstract bool PerformWait();

        public TimeSpan Timeout
        {
            get { return m_Timeout; }
        }

        public DateTime StartTime
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        public MacroMenuItem EditTimeoutMenuItem
        {
            get
            {
                if (m_MenuItem == null)
                    m_MenuItem = new MacroMenuItem(LocString.EditTimeout, new MacroMenuCallback(EditTimeout));
                return m_MenuItem;
            }
        }

        private void EditTimeout(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.NewTimeout), Language.GetString(LocString.ChangeTimeout),
                ((int) (m_Timeout.TotalSeconds)).ToString()))
                m_Timeout = TimeSpan.FromSeconds(InputBox.GetInt(60));
        }

        public virtual bool CheckMatch(MacroAction a)
        {
            return false; // a.GetType() == this.GetType();
        }
    }

    public class MacroComment : MacroAction
    {
        private string m_Comment;

        public MacroComment(string comment)
        {
            if (comment == null)
                comment = "";

            m_Comment = comment.Trim();
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToScript()
        {
            return $"# {m_Comment}";
        }

        public override string Serialize()
        {
            return ToString();
        }

        public string Comment
        {
            get { return m_Comment; }
            set { m_Comment = value; }
        }

        public override string ToString()
        {
            if (m_Comment == null)
                m_Comment = "";

            return String.Format("// {0}", m_Comment);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit))
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.InsComment), Language.GetString(LocString.InputReq),
                m_Comment))
            {
                if (m_Comment == null)
                    m_Comment = "";

                m_Comment = InputBox.GetString();

                if (m_Comment == null)
                    m_Comment = "";

                if (m_Parent != null)
                    m_Parent.Update();
            }
        }
    }

    public class ClearSysMessages : MacroAction
    {
        public ClearSysMessages()
        {
        }

        public override bool Perform()
        {
            PacketHandlers.SysMessages.Clear();

            return true;
        }

        public override string ToScript()
        {
            return "clearsysmsg";
        }

        public override string Serialize()
        {
            return DoSerialize();
        }

        public override string ToString()
        {
            return Language.GetString(LocString.ClearSysMsg);
        }
    }

    public class DoubleClickAction : MacroAction
    {
        private Serial m_Serial;
        private ushort m_Gfx;

        public DoubleClickAction(Serial obj, ushort gfx)
        {
            m_Serial = obj;
            m_Gfx = gfx;
        }

        public DoubleClickAction(string[] args)
        {
            m_Serial = Serial.Parse(args[1]);
            m_Gfx = Convert.ToUInt16(args[2]);
        }

        public override bool Perform()
        {
            PlayerData.DoubleClick(m_Serial);
            return true;
        }

        public override string ToScript()
        {
            return $"dclick '{m_Serial.ToString()}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Serial.Value, m_Gfx);
        }

        public override string ToString()
        {
            return Language.Format(LocString.DClickA1, m_Serial);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget)),
                    new MacroMenuItem(LocString.Conv2DCT, new MacroMenuCallback(ConvertToByType))
                };
            }

            return m_MenuItems;
        }

        private void ConvertToByType(object[] args)
        {
            if (m_Gfx != 0 && m_Serial.IsItem && m_Parent != null)
                m_Parent.Convert(this, new DoubleClickTypeAction(m_Gfx, m_Serial.IsItem));
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnReTarget));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void OnReTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            if (serial.IsItem || serial.IsMobile)
            {
                m_Serial = serial;
                m_Gfx = gfx;
            }

            Engine.MainWindow.SafeAction(s => s.ShowMe());

            if (m_Parent != null)
                m_Parent.Update();
        }
    }

    public class DoubleClickTypeAction : MacroAction
    {
        private ushort m_Gfx;
        public bool m_Item;

        public DoubleClickTypeAction(string[] args)
        {
            m_Gfx = Convert.ToUInt16(args[1]);
            try
            {
                m_Item = Convert.ToBoolean(args[2]);
            }
            catch
            {
            }
        }

        public DoubleClickTypeAction(ushort gfx, bool item)
        {
            m_Gfx = gfx;
            m_Item = item;
        }

        public override bool Perform()
        {
            Serial click = Serial.Zero;

            if (m_Item)
            {
                Item item = World.Player.Backpack != null ? World.Player.Backpack.FindItemByID(m_Gfx) : null;
                ArrayList list = new ArrayList();
                if (item == null)
                {
                    foreach (Item i in World.Items.Values)
                    {
                        if (i.ItemID == m_Gfx && i.RootContainer == null)
                        {
                            if (Config.GetBool("RangeCheckDoubleClick"))
                            {
                                if (Utility.InRange(World.Player.Position, i.Position, 2))
                                {
                                    list.Add(i);
                                }
                            }
                            else
                            {
                                list.Add(i);
                            }
                        }
                    }

                    if (list.Count == 0)
                    {
                        foreach (Item i in World.Items.Values)
                        {
                            if (i.ItemID == m_Gfx && !i.IsInBank)
                            {
                                if (Config.GetBool("RangeCheckDoubleClick"))
                                {
                                    if (Utility.InRange(World.Player.Position, i.Position, 2) || i.RootContainer == World.Player.Backpack)
                                    {
                                        list.Add(i);
                                    }
                                }
                                else
                                {
                                    list.Add(i);
                                }
                            }
                        }
                    }

                    if (list.Count > 0)
                        click = ((Item) list[Utility.Random(list.Count)]).Serial;
                }
                else
                {
                    click = item.Serial;
                }
            }
            else
            {
                ArrayList list = new ArrayList();
                foreach (Mobile m in World.MobilesInRange())
                {
                    if (m.Body == m_Gfx)
                    {
                        if (Config.GetBool("RangeCheckDoubleClick"))
                        {
                            if (Utility.InRange(World.Player.Position, m.Position, 2))
                            {
                                list.Add(m);
                            }
                        }
                        else
                        {
                            list.Add(m);
                        }
                    }
                }

                if (list.Count > 0)
                    click = ((Mobile) list[Utility.Random(list.Count)]).Serial;
            }

            if (click != Serial.Zero)
                PlayerData.DoubleClick(click);
            else
                World.Player.SendMessage(MsgLevel.Force, LocString.NoItemOfType,
                    m_Item ? ((ItemID) m_Gfx).ToString() : String.Format("(Character) 0x{0:X}", m_Gfx));
            return true;
        }

        public override string ToScript()
        {
            return $"dclicktype '{m_Gfx}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Gfx, m_Item);
        }

        public override string ToString()
        {
            return Language.Format(LocString.DClickA1,
                m_Item ? ((ItemID) m_Gfx).ToString() : String.Format("(Character) 0x{0:X}", m_Gfx));
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget))
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnReTarget));
            World.Player.SendMessage(LocString.SelTargAct);
        }

        private void OnReTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            m_Gfx = gfx;
            m_Item = serial.IsItem;

            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (m_Parent != null)
                m_Parent.Update();
        }
    }

    public class LiftAction : MacroWaitAction
    {
        private ushort m_Amount;
        private Serial m_Serial;
        private ushort m_Gfx;

        private static Item m_LastLift;

        public static Item LastLift
        {
            get { return m_LastLift; }
            set { m_LastLift = value; }
        }

        public LiftAction(string[] args)
        {
            m_Serial = Serial.Parse(args[1]);
            m_Amount = Convert.ToUInt16(args[2]);
            m_Gfx = Convert.ToUInt16(args[3]);
        }

        public LiftAction(Serial ser, ushort amount, ushort gfx)
        {
            m_Serial = ser;
            m_Amount = amount;
            m_Gfx = gfx;
        }

        private int m_Id;

        public override bool Perform()
        {
            Item item = World.FindItem(m_Serial);
            if (item != null)
            {
                //DragDropManager.Holding = item;
                m_LastLift = item;
                m_Id = DragDropManager.Drag(item, m_Amount <= item.Amount ? m_Amount : item.Amount);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.MacroItemOutRange);
            }

            return false;
        }

        public override bool PerformWait()
        {
            return DragDropManager.LastIDLifted < m_Id;
        }

        public override string ToScript()
        {
            return $"lift '{m_Serial}' {m_Amount}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Serial.Value, m_Amount, m_Gfx);
        }

        public override string ToString()
        {
            return Language.Format(LocString.LiftA10, m_Serial, m_Amount);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget)),
                    new MacroMenuItem(LocString.ConvLiftByType, new MacroMenuCallback(ConvertToByType)),
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(EditAmount))
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(!m_Serial.IsValid, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            m_Serial = serial;
            m_Gfx = gfx;

            Engine.MainWindow.ShowMe();

            m_Parent?.Update();
        }

        private void EditAmount(object[] args)
        {
            if (InputBox.Show(Engine.MainWindow, Language.GetString(LocString.EnterAmount),
                Language.GetString(LocString.InputReq), m_Amount.ToString()))
            {
                m_Amount = (ushort) InputBox.GetInt(m_Amount);

                if (m_Parent != null)
                    m_Parent.Update();
            }
        }

        private void ConvertToByType(object[] args)
        {
            if (m_Gfx != 0 && m_Parent != null)
                m_Parent.Convert(this, new LiftTypeAction(m_Gfx, m_Amount));
        }
    }

    public class LiftTypeAction : MacroWaitAction
    {
        private ushort m_Gfx;
        private ushort m_Amount;

        public LiftTypeAction(string[] args)
        {
            m_Gfx = Convert.ToUInt16(args[1]);
            m_Amount = Convert.ToUInt16(args[2]);
        }

        public LiftTypeAction(ushort gfx, ushort amount)
        {
            m_Gfx = gfx;
            m_Amount = amount;
        }

        private int m_Id;

        public override bool Perform()
        {
            Item item = World.Player.Backpack != null ? World.Player.Backpack.FindItemByID(m_Gfx) : null;
            /*if ( item == null )
            {
                 ArrayList list = new ArrayList();

                 foreach ( Item i in World.Items.Values )
                 {
                      if ( i.ItemID == m_Gfx && ( i.RootContainer == null || i.IsChildOf( World.Player.Quiver ) ) )
                           list.Add( i );
                 }

                 if ( list.Count > 0 )
                      item = (Item)list[ Utility.Random( list.Count ) ];
            }*/

            if (item != null)
            {
                //DragDropManager.Holding = item;
                ushort amount = m_Amount;
                if (item.Amount < amount)
                    amount = item.Amount;
                LiftAction.LastLift = item;
                //ActionQueue.Enqueue( new LiftRequest( item, amount ) );
                m_Id = DragDropManager.Drag(item, amount);
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.NoItemOfType, (ItemID) m_Gfx);
                //MacroManager.Stop();
            }

            return false;
        }

        public override bool PerformWait()
        {
            return DragDropManager.LastIDLifted < m_Id && !DragDropManager.Empty;
        }

        public override string ToScript()
        {
            return $"lifttype '{m_Gfx}' {m_Amount}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Gfx, m_Amount);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget)),
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(EditAmount))
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            m_Gfx = gfx;

            Engine.MainWindow.ShowMe();

            m_Parent?.Update();
        }

        private void EditAmount(object[] args)
        {
            if (InputBox.Show(Engine.MainWindow, Language.GetString(LocString.EnterAmount),
                Language.GetString(LocString.InputReq), m_Amount.ToString()))
            {
                m_Amount = (ushort) InputBox.GetInt(m_Amount);

                if (m_Parent != null)
                    m_Parent.Update();
            }
        }

        public override string ToString()
        {
            return Language.Format(LocString.LiftA10, m_Amount, (ItemID) m_Gfx);
        }
    }

    public class DropAction : MacroAction
    {
        private Serial m_To;
        private Point3D m_At;
        private Layer m_Layer;

        public DropAction(string[] args)
        {
            m_To = Serial.Parse(args[1]);
            m_At = Point3D.Parse(args[2]);
            try
            {
                m_Layer = (Layer) Byte.Parse(args[3]);
            }
            catch
            {
                m_Layer = Layer.Invalid;
            }
        }

        public DropAction(Serial to, Point3D at) : this(to, at, 0)
        {
        }

        public DropAction(Serial to, Point3D at, Layer layer)
        {
            m_To = to;
            m_At = at;
            m_Layer = layer;
        }

        public override bool Perform()
        {
            if (DragDropManager.Holding != null)
            {
                if (m_Layer > Layer.Invalid && m_Layer <= Layer.LastUserValid)
                {
                    Mobile m = World.FindMobile(m_To);
                    if (m != null)
                        DragDropManager.Drop(DragDropManager.Holding, m, m_Layer);
                }
                else
                {
                    DragDropManager.Drop(DragDropManager.Holding, m_To, m_At);
                }
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.MacroNoHold);
            }

            return true;
        }

        public override string ToScript()
        {
            return m_Layer != Layer.Invalid ? $"drop '{m_To}' {m_Layer}" : $"drop '{m_To}' {m_At.X} {m_At.Y} {m_At.X}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_To, m_At, (byte) m_Layer);
        }

        public override string ToString()
        {
            if (m_Layer != Layer.Invalid)
                return Language.Format(LocString.EquipTo, m_To, m_Layer);
            else
                return Language.Format(LocString.DropA2, m_To.IsValid ? m_To.ToString() : "Ground", m_At);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_To.IsValid)
            {
                return null; // Dont allow conversion(s)
            }
            else
            {
                if (m_MenuItems == null)
                {
                    m_MenuItems = new MacroMenuItem[]
                    {
                        new MacroMenuItem(LocString.ConvRelLoc, new MacroMenuCallback(ConvertToRelLoc))
                    };
                }

                return m_MenuItems;
            }
        }

        private void ConvertToRelLoc(object[] args)
        {
            if (!m_To.IsValid && m_Parent != null)
                m_Parent.Convert(this,
                    new DropRelLocAction((sbyte) (m_At.X - World.Player.Position.X),
                        (sbyte) (m_At.Y - World.Player.Position.Y), (sbyte) (m_At.Z - World.Player.Position.Z)));
        }
    }

    public class DropRelLocAction : MacroAction
    {
        private sbyte[] m_Loc;

        public DropRelLocAction(string[] args)
        {
            m_Loc = new sbyte[3]
            {
                Convert.ToSByte(args[1]),
                Convert.ToSByte(args[2]),
                Convert.ToSByte(args[3])
            };
        }

        public DropRelLocAction(sbyte x, sbyte y, sbyte z)
        {
            m_Loc = new sbyte[3] {x, y, z};
        }

        public override bool Perform()
        {
            if (DragDropManager.Holding != null)
                DragDropManager.Drop(DragDropManager.Holding, null,
                    new Point3D((ushort) (World.Player.Position.X + m_Loc[0]),
                        (ushort) (World.Player.Position.Y + m_Loc[1]), (short) (World.Player.Position.Z + m_Loc[2])));
            else
                World.Player.SendMessage(LocString.MacroNoHold);
            return true;
        }

        public override string ToScript()
        {
            return $"droprelloc {m_Loc[0]} {m_Loc[1]}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Loc[0], m_Loc[1], m_Loc[2]);
        }

        public override string ToString()
        {
            return Language.Format(LocString.DropRelA3, m_Loc[0], m_Loc[1], m_Loc[2]);
        }
    }

    public class GumpResponseAction : MacroAction
    {
        private int m_ButtonID;
        private int[] m_Switches;
        private GumpTextEntry[] m_TextEntries;

        public GumpResponseAction(string[] args)
        {
            m_ButtonID = Convert.ToInt32(args[1]);
            m_Switches = new int[Convert.ToInt32(args[2])];
            for (int i = 0; i < m_Switches.Length; i++)
                m_Switches[i] = Convert.ToInt32(args[3 + i]);
            m_TextEntries = new GumpTextEntry[Convert.ToInt32(args[3 + m_Switches.Length])];
            for (int i = 0; i < m_TextEntries.Length; i++)
            {
                string[] split = args[4 + m_Switches.Length + i].Split('&');

                m_TextEntries[i] = new GumpTextEntry(Convert.ToUInt16(split[0]), split[1]);
            }
        }

        public GumpResponseAction(int button, int[] switches, GumpTextEntry[] entries)
        {
            m_ButtonID = button;
            m_Switches = switches;
            m_TextEntries = entries;
        }

        public override bool Perform()
        {
            Client.Instance.SendToClient(new CloseGump(World.Player.CurrentGumpI));
            Client.Instance.SendToServer(new GumpResponse(World.Player.CurrentGumpS, World.Player.CurrentGumpI,
                m_ButtonID, m_Switches, m_TextEntries));
            World.Player.HasGump = false;
            World.Player.HasCompressedGump = false;
            return true;
        }

        public override string ToScript()
        {
            return m_ButtonID == 0 ? "gumpclose" : $"gumpresponse {m_ButtonID}";
        }

        public override string Serialize()
        {
            ArrayList list = new ArrayList(3 + m_Switches.Length + m_TextEntries.Length);
            list.Add(m_ButtonID);
            list.Add(m_Switches.Length);
            list.AddRange(m_Switches);
            list.Add(m_TextEntries.Length);
            for (int i = 0; i < m_TextEntries.Length; i++)
                list.Add(String.Format("{0}&{1}", m_TextEntries[i].EntryID, m_TextEntries[i].Text));
            return DoSerialize((object[]) list.ToArray(typeof(object)));
        }

        public override string ToString()
        {
            if (m_ButtonID != 0)
                return Language.Format(LocString.GumpRespB, m_ButtonID);
            else
                return Language.Format(LocString.CloseGump);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (this.m_MenuItems == null)
                this.m_MenuItems = (MenuItem[]) new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.UseLastGumpResponse, new MacroMenuCallback(this.UseLastResponse),
                        new object[0]),
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(this.Edit), new object[0])
                };
            return this.m_MenuItems;
        }

        private void Edit(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.EnterNewText), "Input Box", this.m_ButtonID.ToString()))
                this.m_ButtonID = InputBox.GetInt();

            Parent?.Update();
        }

        private void UseLastResponse(object[] args)
        {
            m_ButtonID = World.Player.LastGumpResponseAction.m_ButtonID;
            m_Switches = World.Player.LastGumpResponseAction.m_Switches;
            m_TextEntries = World.Player.LastGumpResponseAction.m_TextEntries;

            World.Player.SendMessage(MsgLevel.Force, "Set GumpResponse to last response");

            Parent?.Update();
        }
    }

    public class MenuResponseAction : MacroAction
    {
        private ushort m_Index, m_ItemID, m_Hue;

        public MenuResponseAction(string[] args)
        {
            m_Index = Convert.ToUInt16(args[1]);
            m_ItemID = Convert.ToUInt16(args[2]);
            m_Hue = Convert.ToUInt16(args[3]);
        }

        public MenuResponseAction(ushort idx, ushort iid, ushort hue)
        {
            m_Index = idx;
            m_ItemID = iid;
            m_Hue = hue;
        }

        public override bool Perform()
        {
            Client.Instance.SendToServer(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, m_Index,
                m_ItemID, m_Hue));
            World.Player.HasMenu = false;
            return true;
        }

        public override string ToScript()
        {
            return $"menuresponse {m_Index} {m_ItemID} {m_Hue}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Index, m_ItemID, m_Hue);
        }

        public override string ToString()
        {
            return Language.Format(LocString.MenuRespA1, m_Index);
        }
    }

    public class AbsoluteTargetAction : MacroAction
    {
        private TargetInfo m_Info;

        public AbsoluteTargetAction(string[] args)
        {
            m_Info = new TargetInfo();

            m_Info.Type = Convert.ToByte(args[1]);
            m_Info.Flags = Convert.ToByte(args[2]);
            m_Info.Serial = Convert.ToUInt32(args[3]);
            m_Info.X = Convert.ToUInt16(args[4]);
            m_Info.Y = Convert.ToUInt16(args[5]);
            m_Info.Z = Convert.ToInt16(args[6]);
            m_Info.Gfx = Convert.ToUInt16(args[7]);
        }

        public AbsoluteTargetAction(TargetInfo info)
        {
            m_Info = new TargetInfo();
            m_Info.Type = info.Type;
            m_Info.Flags = info.Flags;
            m_Info.Serial = info.Serial;
            m_Info.X = info.X;
            m_Info.Y = info.Y;
            m_Info.Z = info.Z;
            m_Info.Gfx = info.Gfx;
        }

        public override bool Perform()
        {
            Targeting.Target(m_Info);
            return true;
        }

        public override string ToScript()
        {
            return $"target {m_Info.Serial}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Info.Type, m_Info.Flags, m_Info.Serial.Value, m_Info.X, m_Info.Y, m_Info.Z,
                m_Info.Gfx);
        }

        public override string ToString()
        {
            return Language.GetString(LocString.AbsTarg);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget)),
                    new MacroMenuItem(LocString.ConvLT, new MacroMenuCallback(ConvertToLastTarget)),
                    new MacroMenuItem(LocString.ConvTargType, new MacroMenuCallback(ConvertToByType)),
                    new MacroMenuItem(LocString.ConvRelLoc, new MacroMenuCallback(ConvertToRelLoc))
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(!m_Info.Serial.IsValid, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            m_Info.Gfx = gfx;
            m_Info.Serial = serial;
            m_Info.Type = (byte) (ground ? 1 : 0);
            m_Info.X = pt.X;
            m_Info.Y = pt.Y;
            m_Info.Z = pt.Z;

            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (m_Parent != null)
                m_Parent.Update();
        }

        private void ConvertToLastTarget(object[] args)
        {
            if (m_Parent != null)
                m_Parent.Convert(this, new LastTargetAction());
        }

        private void ConvertToByType(object[] args)
        {
            if (m_Parent != null)
                m_Parent.Convert(this, new TargetTypeAction(m_Info.Serial.IsMobile, m_Info.Gfx));
        }

        private void ConvertToRelLoc(object[] args)
        {
            if (m_Parent != null)
                m_Parent.Convert(this,
                    new TargetRelLocAction((sbyte) (m_Info.X - World.Player.Position.X),
                        (sbyte) (m_Info.Y - World.Player.Position
                                     .Y))); //, (sbyte)(m_Info.Z - World.Player.Position.Z) ) );
        }
    }

    /// <summary>
    /// Action to handle variable macros to alleviate the headache of having multiple macros for the same thing
    ///
    /// This Action does break the pattern that you see in every other action because the data that is stored for this
    /// action exists not in the Macro file, but in a different file that has all the variables in the profile
    /// </summary>
    public class AbsoluteTargetVariableAction : MacroAction
    {
        private TargetInfo _target;
        private readonly string _variableName;

        public AbsoluteTargetVariableAction(string[] args)
        {
            _variableName = args.Length > 1 ? args[1] : args[0];
        }

        public override bool Perform()
        {
            _target = null;

            foreach (MacroVariables.MacroVariable mV in MacroVariables.MacroVariableList)
            {
                if (mV.Name.Equals(_variableName))
                {
                    _target = mV.TargetInfo;
                    break;
                }
            }

            if (_target != null)
            {
                Targeting.Target(_target);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToScript()
        {
            return $"target '{_variableName}'";
        }

        public override string Serialize()
        {
            return DoSerialize(_variableName);
        }

        public override string ToString()
        {
            return $"{Language.GetString(LocString.AbsTarg)} (${_variableName})";
        }

        /*private MenuItem[] m_MenuItems;
        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem( LocString.ReTarget, ReTarget )
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(!_target.Serial.IsValid, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            _target.Gfx = gfx;
            _target.Serial = serial;
            _target.Type = (byte)(ground ? 1 : 0);
            _target.X = pt.X;
            _target.Y = pt.Y;
            _target.Z = pt.Z;

            Engine.MainWindow.SafeAction(s => s.ShowMe());

            m_Parent?.Update();
        }*/
    }

    /// <summary>
    /// Action to handle variable macros to alleviate the headache of having multiple macros for the same thing
    ///
    /// This Action does break the pattern that you see in every other action because the data that is stored for this
    /// action exists not in the Macro file, but in a different file that has all the variables in the profile
    /// </summary>
    public class DoubleClickVariableAction : MacroAction
    {
        private Serial _serial;
        private readonly string _variableName;

        public DoubleClickVariableAction(string[] args)
        {
            _variableName = args.Length > 1 ? args[1] : args[0];
        }

        public override bool Perform()
        {
            _serial = Serial.Zero;

            foreach (MacroVariables.MacroVariable mV in MacroVariables.MacroVariableList)
            {
                if (mV.Name.Equals(_variableName))
                {
                    _serial = mV.TargetInfo.Serial;
                    break;
                }
            }

            if (_serial != null && _serial != Serial.Zero)
            {
                PlayerData.DoubleClick(_serial);
                return true;
            }

            return false;
        }

        public override string ToScript()
        {
            return $"dclick '{_variableName}'";
        }

        public override string Serialize()
        {
            return DoSerialize(_variableName);
        }

        public override string ToString()
        {
            return $"DoubleClick (${_variableName})";
        }
    }

    public class TargetTypeAction : MacroAction
    {
        private bool m_Mobile;
        private ushort m_Gfx;
        private object _previousObject;

        public TargetTypeAction(string[] args)
        {
            m_Mobile = Convert.ToBoolean(args[1]);
            m_Gfx = Convert.ToUInt16(args[2]);
        }

        public TargetTypeAction(bool mobile, ushort gfx)
        {
            m_Mobile = mobile;
            m_Gfx = gfx;
        }

        public override bool Perform()
        {
            if (Targeting.FromGrabHotKey)
                return false;

            ArrayList list = new ArrayList();
            if (m_Mobile)
            {
                foreach (Mobile find in World.MobilesInRange())
                {
                    if (find.Body == m_Gfx)
                    {
                        if (Config.GetBool("RangeCheckTargetByType"))
                        {
                            if (Utility.InRange(World.Player.Position, find.Position, 2))
                            {
                                list.Add(find);
                            }
                        }
                        else
                        {
                            list.Add(find);
                        }
                    }
                }
            }
            else
            {
                foreach (Item i in World.Items.Values)
                {
                    if (i.ItemID == m_Gfx && !i.IsInBank)
                    {
                        if (Config.GetBool("RangeCheckTargetByType"))
                        {
                            if (Utility.InRange(World.Player.Position, i.Position, 2) || i.RootContainer == World.Player.Backpack)
                            {
                                list.Add(i);
                            }
                        }
                        else
                        {
                            list.Add(i);
                        }
                    }
                }
            }

            if (list.Count > 0)
            {
                if (Config.GetBool("DiffTargetByType") && list.Count > 1)
                {
                    object currentObject = list[Utility.Random(list.Count)];

                    while (_previousObject != null && _previousObject == currentObject)
                    {
                        currentObject = list[Utility.Random(list.Count)];
                    }

                    Targeting.Target(currentObject);

                    _previousObject = currentObject;
                }
                else
                {
                    Targeting.Target(list[Utility.Random(list.Count)]);
                }
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Warning, LocString.NoItemOfType,
                    m_Mobile ? String.Format("Character [{0}]", m_Gfx) : ((ItemID) m_Gfx).ToString());
            }

            return true;
        }

        public override string ToScript()
        {
            return $"targettype '{m_Gfx}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Mobile, m_Gfx);
        }

        public override string ToString()
        {
            if (m_Mobile)
                return Language.Format(LocString.TargByType, m_Gfx);
            else
                return Language.Format(LocString.TargByType, (ItemID) m_Gfx);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget)),
                    new MacroMenuItem(LocString.ConvLT, new MacroMenuCallback(ConvertToLastTarget))
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            if (!ground && serial.IsValid)
            {
                m_Mobile = serial.IsMobile;
                m_Gfx = gfx;
            }

            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (m_Parent != null)
                m_Parent.Update();
        }

        private void ConvertToLastTarget(object[] args)
        {
            if (m_Parent != null)
                m_Parent.Convert(this, new LastTargetAction());
        }
    }

    public class TargetRelLocAction : MacroAction
    {
        private sbyte m_X, m_Y;

        public TargetRelLocAction(string[] args)
        {
            m_X = Convert.ToSByte(args[1]);
            m_Y = Convert.ToSByte(args[2]);
        }

        public TargetRelLocAction(sbyte x, sbyte y)
        {
            m_X = x;
            m_Y = y;
        }

        public override bool Perform()
        {
            ushort x = (ushort) (World.Player.Position.X + m_X);
            ushort y = (ushort) (World.Player.Position.Y + m_Y);
            short z = (short) World.Player.Position.Z;
            try
            {
                Ultima.HuedTile tile = Map.GetTileNear(World.Player.Map, x, y, z);
                Targeting.Target(new Point3D(x, y, tile.Z), (ushort) tile.ID);
            }
            catch (Exception e)
            {
                World.Player.SendMessage(MsgLevel.Debug, "Error Executing TargetRelLoc: {0}", e.Message);
            }

            return true;
        }

        public override string ToScript()
        {
            return $"targetrelloc {m_X} {m_Y}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_X, m_Y);
        }

        public override string ToString()
        {
            return Language.Format(LocString.TargRelLocA3, m_X, m_Y, 0);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.ReTarget, new MacroMenuCallback(ReTarget))
                };
            }

            return m_MenuItems;
        }

        private void ReTarget(object[] args)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());

            Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            m_X = (sbyte) (pt.X - World.Player.Position.X);
            m_Y = (sbyte) (pt.Y - World.Player.Position.Y);
            // m_Z = (sbyte)(pt.Z - World.Player.Position.Z);
            if (m_Parent != null)
                m_Parent.Update();
        }
    }

    public class LastTargetAction : MacroAction
    {
        public LastTargetAction()
        {
        }

        public override bool Perform()
        {
            if (Targeting.FromGrabHotKey)
                return false;

            if (!Targeting.DoLastTarget()) //Targeting.LastTarget( true );
                Targeting.ResendTarget();
            return true;
        }

        public override string ToString()
        {
            return String.Format("Exec: {0}", Language.GetString(LocString.LastTarget));
        }

        public override string ToScript()
        {
            return "lasttarget";
        }
    }

    public class SetLastTargetAction : MacroWaitAction
    {
        public SetLastTargetAction()
        {
        }

        public override bool Perform()
        {
            Targeting.TargetSetLastTarget();
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            return !Targeting.LTWasSet;
        }

        public override string ToString()
        {
            return Language.GetString(LocString.SetLT);
        }

        public override string ToScript()
        {
            return "setlasttarget";
        }
    }

    public class SetMacroVariableTargetAction : MacroWaitAction
    {
        private string m_VarName;
        private MacroVariables.MacroVariable m_MacroVariable;

        public SetMacroVariableTargetAction(string[] args)
        {
            m_VarName = args.Length > 1 ? args[1] : args[0];

            FindMacroVariable();
        }

        public SetMacroVariableTargetAction(string varName)
        {
            m_VarName = varName;

            FindMacroVariable();
        }

        private bool FindMacroVariable()
        {
            foreach (MacroVariables.MacroVariable mV in MacroVariables.MacroVariableList)
            {
                if (mV.Name.ToLower().Equals(m_VarName.ToLower()))
                {
                    m_MacroVariable = mV;
                    break;
                }
            }

            if (m_MacroVariable == null)
            {
                m_VarName = $"?{m_VarName}?";
                return false;
            }

            return true;
        }

        public override bool Perform()
        {
            if (m_MacroVariable == null)
            {
                return false;
            }

            m_MacroVariable.TargetSetMacroVariable();

            return !PerformWait();
        }

        public override bool PerformWait()
        {
            if (m_MacroVariable == null)
            {
                return false;
            }

            return !m_MacroVariable.TargetWasSet;
        }

        public override string ToString()
        {
            return $"Set Macro Variable (${m_VarName})";
        }

        public override string ToScript()
        {
            return $"setvar {m_VarName}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_VarName);
        }
    }

    public class SpeechAction : MacroAction
    {
        private MessageType m_Type;
        private ushort m_Font;
        private ushort m_Hue;
        private string m_Lang;
        private ArrayList m_Keywords;
        private string m_Speech;

        public SpeechAction(string[] args)
        {
            m_Type = ((MessageType) Convert.ToInt32(args[1])) & ~MessageType.Encoded;
            m_Hue = Convert.ToUInt16(args[2]);
            m_Font = Convert.ToUInt16(args[3]);
            m_Lang = args[4];

            int count = Convert.ToInt32(args[5]);
            if (count > 0)
            {
                m_Keywords = new ArrayList(count);
                m_Keywords.Add(Convert.ToUInt16(args[6]));

                for (int i = 1; i < count; i++)
                    m_Keywords.Add(Convert.ToByte(args[6 + i]));
            }

            m_Speech = args[6 + count];
        }

        public SpeechAction(MessageType type, ushort hue, ushort font, string lang, ArrayList kw, string speech)
        {
            m_Type = type;
            m_Hue = hue;
            m_Font = font;
            m_Lang = lang;
            m_Keywords = kw;
            m_Speech = speech;
        }

        public override bool Perform()
        {
            if (m_Speech.Length > 1 && m_Speech[0] == '-')
            {
                string text = m_Speech.Substring(1);
                string[] split = text.Split(' ', '\t');
                CommandCallback call = (CommandCallback) Command.List[split[0]];
                if (call == null && text[0] == '-')
                {
                    call = (CommandCallback) Command.List["-"];
                    if (call != null && split.Length > 1 && split[1] != null && split[1].Length > 1)
                        split[1] = split[1].Substring(1);
                }

                if (call != null)
                {
                    ArrayList list = new ArrayList();
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (split[i] != null && split[i].Length > 0)
                            list.Add(split[i]);
                    }

                    call((string[]) list.ToArray(typeof(string)));
                    return true;
                }
            }

            int hue = m_Hue;

            if (m_Type != MessageType.Emote)
            {
                if (World.Player.SpeechHue == 0)
                    World.Player.SpeechHue = m_Hue;
                hue = World.Player.SpeechHue;
            }

            Client.Instance.SendToServer(new ClientUniMessage(m_Type, hue, m_Font, m_Lang, m_Keywords, m_Speech));
            return true;
        }

        public override string ToScript()
        {
            return $"say '{m_Speech}'";
        }

        public override string Serialize()
        {
            ArrayList list = new ArrayList(6);
            list.Add((int) m_Type);
            list.Add(m_Hue);
            list.Add(m_Font);
            list.Add(m_Lang);
            if (m_Keywords != null && m_Keywords.Count > 1)
            {
                list.Add((int) m_Keywords.Count);
                for (int i = 0; i < m_Keywords.Count; i++)
                    list.Add(m_Keywords[i]);
            }
            else
            {
                list.Add("0");
            }

            list.Add(m_Speech);

            return DoSerialize((object[]) list.ToArray(typeof(object)));
        }

        public override string ToString()
        {
            //return Language.Format( LocString.SayQA1, m_Speech );
            StringBuilder sb = new StringBuilder();
            switch (m_Type)
            {
                case MessageType.Emote:
                    sb.Append("Emote: ");
                    break;
                case MessageType.Whisper:
                    sb.Append("Whisper: ");
                    break;
                case MessageType.Yell:
                    sb.Append("Yell: ");
                    break;
                case MessageType.Regular:
                default:
                    sb.Append("Say: ");
                    break;
            }

            sb.Append(m_Speech);
            return sb.ToString();
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (this.m_MenuItems == null)
                this.m_MenuItems = (MenuItem[]) new MacroMenuItem[1]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(this.Edit), new object[0])
                };
            return this.m_MenuItems;
        }

        private void Edit(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.EnterNewText), "Input Box", this.m_Speech))
                this.m_Speech = InputBox.GetString();
            if (this.Parent == null)
                return;
            this.Parent.Update();
        }
    }

    public class OverheadMessageAction : MacroAction
    {
        private ushort _hue;
        private string _message;

        public OverheadMessageAction(string[] args)
        {
            _hue = Convert.ToUInt16(args[1]);

            List<string> message = new List<string>();

            for (int i = 2; i < args.Length; i++)
            {
                message.Add(args[i]);
            }

            _message = string.Join(" ", message);
        }

        public OverheadMessageAction(ushort hue, string message)
        {
            _hue = hue;
            _message = message;
        }

        public override bool Perform()
        {
            if (_message.Length > 0)
            {
                World.Player.OverheadMessage(_hue, _message);
            }

            return true;
        }

        public override string ToScript()
        {
            return $"overhead '{_message}' {_hue}";
        }

        public override string Serialize()
        {
            ArrayList list = new ArrayList(2) {_hue, _message};

            return DoSerialize((object[]) list.ToArray(typeof(object)));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Overhead ({_hue}): ");
            sb.Append(_message);
            return sb.ToString();
        }

        private MenuItem[] _menuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            return _menuItems ?? (_menuItems = new MacroMenuItem[]
            {
                new MacroMenuItem(LocString.Edit, Edit),
                new MacroMenuItem(LocString.SetHue, SetHue)
            });
        }

        private void Edit(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.EnterNewText), "Input Box", _message))
                _message = InputBox.GetString();

            Parent?.Update();
        }

        private void SetHue(object[] args)
        {
            HueEntry h = new HueEntry(_hue);

            if (h.ShowDialog(Engine.MainWindow) == DialogResult.OK)
            {
                _hue = (ushort) h.Hue;
            }

            Parent?.Update();
        }
    }

    public class UseSkillAction : MacroAction
    {
        private int m_Skill;

        public UseSkillAction(string[] args)
        {
            m_Skill = Convert.ToInt32(args[1]);
        }

        public UseSkillAction(int sk)
        {
            m_Skill = sk;
        }

        public override bool Perform()
        {
            Client.Instance.SendToServer(new UseSkill(m_Skill));
            return true;
        }

        public override string ToScript()
        {
            return $"skill '{Language.Skill2Str(m_Skill)}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Skill);
        }

        public override string ToString()
        {
            return Language.Format(LocString.UseSkillA1, Language.Skill2Str(m_Skill));
        }
    }

    public class ExtCastSpellAction : MacroAction
    {
        private Spell m_Spell;
        private Serial m_Book;

        public ExtCastSpellAction(string[] args)
        {
            m_Spell = Spell.Get(Convert.ToInt32(args[1]));
            m_Book = Serial.Parse(args[2]);
        }

        public ExtCastSpellAction(int s, Serial book)
        {
            m_Spell = Spell.Get(s);
            m_Book = book;
        }

        public ExtCastSpellAction(Spell s, Serial book)
        {
            m_Spell = s;
            m_Book = book;
        }

        public override bool Perform()
        {
            m_Spell.OnCast(new ExtCastSpell(m_Book, (ushort) m_Spell.GetID()));
            return true;
        }

        public override string ToScript()
        {
            return $"cast '{m_Spell.GetName()}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Spell.GetID(), m_Book.Value);
        }

        public override string ToString()
        {
            return Language.Format(LocString.CastSpellA1, m_Spell);
        }
    }

    public class BookCastSpellAction : MacroAction
    {
        private Spell m_Spell;
        private Serial m_Book;

        public BookCastSpellAction(string[] args)
        {
            m_Spell = Spell.Get(Convert.ToInt32(args[1]));
            m_Book = Serial.Parse(args[2]);
        }

        public BookCastSpellAction(int s, Serial book)
        {
            m_Spell = Spell.Get(s);
            m_Book = book;
        }

        public BookCastSpellAction(Spell s, Serial book)
        {
            m_Spell = s;
            m_Book = book;
        }

        public override bool Perform()
        {
            m_Spell.OnCast(new CastSpellFromBook(m_Book, (ushort) m_Spell.GetID()));
            return true;
        }

        public override string ToScript()
        {
            return $"cast '{m_Spell.GetName()}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Spell.GetID(), m_Book.Value);
        }

        public override string ToString()
        {
            return Language.Format(LocString.CastSpellA1, m_Spell);
        }
    }

    public class MacroCastSpellAction : MacroAction
    {
        private Spell m_Spell;

        public MacroCastSpellAction(string[] args)
        {
            m_Spell = Spell.Get(Convert.ToInt32(args[1]));
        }

        public MacroCastSpellAction(int s)
        {
            m_Spell = Spell.Get(s);
        }

        public MacroCastSpellAction(Spell s)
        {
            m_Spell = s;
        }

        public override bool Perform()
        {
            m_Spell.OnCast(new CastSpellFromMacro((ushort) m_Spell.GetID()));
            return true;
        }

        public override string ToScript()
        {
            return $"cast '{m_Spell.GetName()}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Spell.GetID());
        }

        public override string ToString()
        {
            return Language.Format(LocString.CastSpellA1, m_Spell);
        }
    }

    public class SetAbilityAction : MacroAction
    {
        private AOSAbility m_Ability;

        public SetAbilityAction(string[] args)
        {
            m_Ability = (AOSAbility) Convert.ToInt32(args[1]);
        }

        public SetAbilityAction(AOSAbility a)
        {
            m_Ability = a;
        }

        public override bool Perform()
        {
            Client.Instance.SendToServer(new UseAbility(m_Ability));
            return true;
        }

        public override string ToScript()
        {
            return $"setability '{m_Ability}'";
        }

        public override string Serialize()
        {
            return DoSerialize((int) m_Ability);
        }

        public override string ToString()
        {
            return Language.Format(LocString.SetAbilityA1, m_Ability);
        }
    }

    public class DressAction : MacroWaitAction
    {
        private string m_Name;

        public DressAction(string[] args)
        {
            m_Name = args[1];
        }

        public DressAction(string name)
        {
            m_Name = name;
        }

        public override bool Perform()
        {
            DressList list = DressList.Find(m_Name);
            if (list != null)
            {
                list.Dress();
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool PerformWait()
        {
            return !ActionQueue.Empty;
        }

        public override string ToScript()
        {
            return $"dress '{m_Name}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Name);
        }

        public override string ToString()
        {
            return Language.Format(LocString.DressA1, m_Name);
        }
    }

    public class UnDressAction : MacroWaitAction
    {
        private string m_Name;
        private byte m_Layer;

        public UnDressAction(string[] args)
        {
            try
            {
                m_Layer = Convert.ToByte(args[2]);
            }
            catch
            {
                m_Layer = 255;
            }

            if (m_Layer == 255)
                m_Name = args[1];
            else
                m_Name = "";
        }

        public UnDressAction(string name)
        {
            m_Name = name;
            m_Layer = 255;
        }

        public UnDressAction(byte layer)
        {
            m_Layer = layer;
            m_Name = "";
        }

        public override bool Perform()
        {
            if (m_Layer == 255)
            {
                DressList list = DressList.Find(m_Name);
                if (list != null)
                {
                    list.Undress();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (m_Layer == 0)
            {
                HotKeys.UndressHotKeys.OnUndressAll();
                return false;
            }
            else
            {
                return !Dress.Unequip((Layer) m_Layer);
            }
        }

        public override bool PerformWait()
        {
            return !ActionQueue.Empty;
        }

        public override string ToScript()
        {
            if (m_Layer == 255)
            {
                return $"undress '{m_Name}'";
            }

            return m_Layer == 0 ? "undress" : $"undress '{m_Layer}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Name, m_Layer);
        }

        public override string ToString()
        {
            if (m_Layer == 255)
                return Language.Format(LocString.UndressA1, m_Name);
            else if (m_Layer == 0)
                return Language.GetString(LocString.UndressAll);
            else
                return Language.Format(LocString.UndressLayerA1, (Layer) m_Layer);
        }
    }

    public class WalkAction : MacroWaitAction
    {
        private Direction m_Dir;
        private static DateTime m_LastWalk = DateTime.MinValue;

        public static DateTime LastWalkTime
        {
            get { return m_LastWalk; }
            set { m_LastWalk = value; }
        }

        public WalkAction(string[] args)
        {
            m_Dir = (Direction) (Convert.ToByte(args[1])) & Direction.Mask;
        }

        public WalkAction(Direction dir)
        {
            m_Dir = dir & Direction.Mask;
        }

        //private static int m_LastSeq = -1;
        public override bool Perform()
        {
            return !PerformWait();
        }

        //public static bool IsMacroWalk(byte seq)
        //{
        //    return m_LastSeq != -1 && m_LastSeq == (int)seq && World.Player.HasWalkEntry((byte)m_LastSeq);
        //}

        public override bool PerformWait()
        {
            if (m_LastWalk + TimeSpan.FromSeconds(0.4) >= DateTime.UtcNow)
            {
                return true;
            }
            else
            {
                //m_LastSeq = World.Player.WalkSequence;
                m_LastWalk = DateTime.UtcNow;

                //Client.Instance.SendToClient(new MobileUpdate(World.Player));
                //Client.Instance.SendToClient(new ForceWalk(m_Dir));
                //Client.Instance.SendToServer(new WalkRequest(m_Dir, World.Player.WalkSequence));
                //World.Player.MoveReq(m_Dir, World.Player.WalkSequence);

                Client.Instance.RequestMove(m_Dir);
                return false;
            }
        }

        public override string ToScript()
        {
            return m_Dir == Direction.Mask ? $"walk 'Up'" : $"walk '{m_Dir}'";
        }

        public override string Serialize()
        {
            return DoSerialize((byte) m_Dir);
        }

        public override string ToString()
        {
            return Language.Format(LocString.WalkA1, m_Dir != Direction.Mask ? m_Dir.ToString() : "Up");
        }
    }

    public class WaitForMenuAction : MacroWaitAction
    {
        private uint m_MenuID;

        public WaitForMenuAction(uint gid)
        {
            m_MenuID = gid;
        }

        public WaitForMenuAction(string[] args)
        {
            if (args.Length > 1)
                m_MenuID = Convert.ToUInt32(args[1]);

            try
            {
                m_Timeout = TimeSpan.FromSeconds(Convert.ToDouble(args[2]));
            }
            catch
            {
            }
        }

        public override bool Perform()
        {
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            return !(World.Player.HasMenu && (World.Player.CurrentGumpI == m_MenuID || m_MenuID == 0));
        }

        public override string ToString()
        {
            if (m_MenuID == 0)
                return Language.GetString(LocString.WaitAnyMenu);
            else
                return Language.Format(LocString.WaitMenuA1, m_MenuID);
        }

        public override string ToScript()
        {
            return $"waitformenu {m_MenuID}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_MenuID, m_Timeout.TotalSeconds);
        }

        public override bool CheckMatch(MacroAction a)
        {
            if (a is WaitForMenuAction)
            {
                if (m_MenuID == 0 || ((WaitForMenuAction) a).m_MenuID == m_MenuID)
                    return true;
            }

            return false;
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit)),
                    this.EditTimeoutMenuItem
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertWait(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class WaitForGumpAction : MacroWaitAction
    {
        private uint m_GumpID;
        private bool m_Strict;

        public WaitForGumpAction()
        {
            m_GumpID = 0;
            m_Strict = false;
        }

        public WaitForGumpAction(uint gid)
        {
            m_GumpID = gid;
            m_Strict = false;
        }

        public WaitForGumpAction(string[] args)
        {
            m_GumpID = Convert.ToUInt32(args[1]);
            try
            {
                m_Strict = Convert.ToBoolean(args[2]);
            }
            catch
            {
                m_Strict = false;
            }

            try
            {
                m_Timeout = TimeSpan.FromSeconds(Convert.ToDouble(args[3]));
            }
            catch
            {
            }
        }

        public override bool Perform()
        {
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            return !((World.Player.HasGump || World.Player.HasCompressedGump) &&
                     (World.Player.CurrentGumpI == m_GumpID || !m_Strict || m_GumpID == 0));

            //if (!World.Player.HasGump) // Does the player even have a gump?
            //    return true;

            //if ((int)World.Player.CurrentGumpI != (int)m_GumpID && m_Strict)
            //    return m_GumpID > 0;

            //return false;
        }

        public override string ToString()
        {
            if (m_GumpID == 0 || !m_Strict)
                return Language.GetString(LocString.WaitAnyGump);
            else
                return Language.Format(LocString.WaitGumpA1, m_GumpID);
        }

        public override string ToScript()
        {
            return $"waitforgump {m_GumpID}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_GumpID, m_Strict, m_Timeout.TotalSeconds);
        }

        public override bool CheckMatch(MacroAction a)
        {
            if (a is WaitForGumpAction)
            {
                if (m_GumpID == 0 || ((WaitForGumpAction) a).m_GumpID == m_GumpID)
                    return true;
            }

            return false;
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit)),
                    new MacroMenuItem(LocString.Null, new MacroMenuCallback(ToggleStrict)),
                    this.EditTimeoutMenuItem
                };
            }

            if (!m_Strict)
                m_MenuItems[1].Text =
                    String.Format("Change to \"{0}\"", Language.Format(LocString.WaitGumpA1, m_GumpID));
            else
                m_MenuItems[1].Text = String.Format("Change to \"{0}\"", Language.GetString(LocString.WaitAnyGump));
            m_MenuItems[1].Enabled = m_GumpID != 0 || m_Strict;

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertWait(this).ShowDialog(Engine.MainWindow);
        }

        private void ToggleStrict(object[] args)
        {
            m_Strict = !m_Strict;
            if (m_Parent != null)
                m_Parent.Update();
        }
    }

    public class WaitForTargetAction : MacroWaitAction
    {
        public WaitForTargetAction()
        {
            m_Timeout = TimeSpan.FromSeconds(30.0);
        }

        public WaitForTargetAction(string[] args)
        {
            try
            {
                m_Timeout = TimeSpan.FromSeconds(Convert.ToDouble(args[1]));
            }
            catch
            {
                m_Timeout = TimeSpan.FromSeconds(30.0);
            }
        }

        public override bool Perform()
        {
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            return !Targeting.HasTarget;
        }

        public override string ToString()
        {
            return Language.GetString(LocString.WaitTarg);
        }

        public override string ToScript()
        {
            return $"waitfortarget";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Timeout.TotalSeconds);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit)),
                    this.EditTimeoutMenuItem
                };
            }

            return m_MenuItems;
        }

        public override bool CheckMatch(MacroAction a)
        {
            return (a is WaitForTargetAction);
        }

        private void Edit(object[] args)
        {
            new MacroInsertWait(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class PauseAction : MacroWaitAction
    {
        public PauseAction(string[] args)
        {
            m_Timeout = TimeSpan.Parse(args[1]);
        }

        public PauseAction(int ms)
        {
            m_Timeout = TimeSpan.FromMilliseconds(ms);
        }

        public PauseAction(TimeSpan time)
        {
            m_Timeout = time;
        }

        public override string ToScript()
        {
            return $"wait {m_Timeout.TotalMilliseconds}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Timeout);
        }

        public override bool Perform()
        {
            this.StartTime = DateTime.UtcNow;
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            return (StartTime + m_Timeout >= DateTime.UtcNow);
        }

        public override string ToString()
        {
            return Language.Format(LocString.PauseA1, m_Timeout.TotalSeconds);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit))
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertWait(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class WaitForStatAction : MacroWaitAction
    {
        private byte m_Direction;
        private int m_Value;
        private IfAction.IfVarType m_Stat;

        public byte Op
        {
            get { return m_Direction; }
        }

        public int Amount
        {
            get { return m_Value; }
        }

        public IfAction.IfVarType Stat
        {
            get { return m_Stat; }
        }

        public WaitForStatAction(string[] args)
        {
            m_Stat = (IfAction.IfVarType) Convert.ToInt32(args[1]);
            m_Direction = Convert.ToByte(args[2]);
            m_Value = Convert.ToInt32(args[3]);

            try
            {
                m_Timeout = TimeSpan.FromSeconds(Convert.ToDouble(args[4]));
            }
            catch
            {
                m_Timeout = TimeSpan.FromMinutes(60.0);
            }
        }

        public WaitForStatAction(IfAction.IfVarType stat, byte dir, int val)
        {
            m_Stat = stat;
            m_Direction = dir;
            m_Value = val;

            m_Timeout = TimeSpan.FromMinutes(60.0);
        }

        public override string ToScript()
        {
            string op = m_Direction > 0 ? ">=" : "<=";
            string stat = "unknown";

            switch (m_Stat)
            {
                case IfAction.IfVarType.Hits:
                    stat = "hits";
                    break;
                case IfAction.IfVarType.Mana:
                    stat = "mana";
                    break;
                case IfAction.IfVarType.Stamina:
                    stat = "stam";
                    break;
            }

            return $"if {stat} {op} {m_Value}";
        }

        public override string Serialize()
        {
            return DoSerialize((int) m_Stat, m_Direction, m_Value, m_Timeout.TotalSeconds);
        }

        public override bool Perform()
        {
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            if (m_Direction > 0)
            {
                // wait for m_Stat >= m_Value
                switch (m_Stat)
                {
                    case IfAction.IfVarType.Hits:
                        return World.Player.Hits < m_Value;
                    case IfAction.IfVarType.Mana:
                        return World.Player.Mana < m_Value;
                    case IfAction.IfVarType.Stamina:
                        return World.Player.Stam < m_Value;
                }
            }
            else
            {
                // wait for m_Stat <= m_Value
                switch (m_Stat)
                {
                    case IfAction.IfVarType.Hits:
                        return World.Player.Hits > m_Value;
                    case IfAction.IfVarType.Mana:
                        return World.Player.Mana > m_Value;
                    case IfAction.IfVarType.Stamina:
                        return World.Player.Stam > m_Value;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return Language.Format(LocString.WaitA3, m_Stat, m_Direction > 0 ? ">=" : "<=", m_Value);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit)),
                    this.EditTimeoutMenuItem
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertWait(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class IfAction : MacroAction
    {
        public enum IfVarType : int
        {
            Hits = 0,
            Mana,
            Stamina,
            Poisoned,
            SysMessage,
            Weight,
            Mounted,
            RHandEmpty,
            LHandEmpty,

            BeginCountersMarker,

            Counter = 50,
            Skill = 100
        }

        // 0 <=,1 >=,2 <,3 >
        private sbyte m_Direction;
        private object m_Value;
        private IfVarType m_Var;
        private string m_Counter;
        private int m_SkillId = -1;
        private Assistant.Counter m_CountObj;

        public sbyte Op
        {
            get { return m_Direction; }
        }

        public object Value
        {
            get { return m_Value; }
        }

        public IfVarType Variable
        {
            get { return m_Var; }
        }

        public string Counter
        {
            get { return m_Counter; }
        }

        public int SkillId
        {
            get { return m_SkillId; }
        }

        public IfAction(string[] args)
        {
            m_Var = (IfVarType) Convert.ToInt32(args[1]);
            try
            {
                m_Direction = Convert.ToSByte(args[2]);
                if (m_Direction > 3)
                    m_Direction = 0;
            }
            catch
            {
                m_Direction = -1;
            }

            if (m_Var == IfVarType.SysMessage)
            {
                m_Value = args[3].ToLower();
            }
            else if (m_Var == IfVarType.Skill)
            {
                if (args[3] is string strVal)
                {
                    m_Value = strVal;
                }
                else
                {
                    m_Value = Convert.ToDouble(args[3]);
                }
            }
            else
            {
                if (args[3] is string strVal)
                {
                    m_Value = strVal;
                }
                else
                {
                    m_Value = Convert.ToInt32(args[3]);
                }
            }

            if (m_Var == IfVarType.Counter)
                m_Counter = args[4];

            if (m_Var == IfVarType.Skill)
                m_SkillId = Convert.ToInt32(args[4]);
        }

        public IfAction(IfVarType var, sbyte dir, int val)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
        }

        public IfAction(IfVarType var, sbyte dir, string val)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
        }

        public IfAction(IfVarType var, sbyte dir, int val, string counter)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
            m_Counter = counter;
        }

        public IfAction(IfVarType var, sbyte dir, double val, int skillId)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
            m_SkillId = skillId;
        }

        public IfAction(IfVarType var, string text)
        {
            m_Var = var;
            m_Value = text.ToLower();
        }

        public override string ToScript()
        {
            string op = "??";
            string expression = "unknown";

            bool useValue = true;

            switch (m_Direction)
            {
                case 0:
                    // if stat <= m_Value
                    op = "<=";
                    break;
                case 1:
                    // if stat >= m_Value
                    op = ">=";
                    break;
                case 2:
                    // if stat < m_Value
                    op = "<";
                    break;
                case 3:
                    // if stat > m_Value
                    op = ">";
                    break;
            }

            switch (m_Var)
            {
                case IfAction.IfVarType.Hits:
                    expression = "hits";
                    break;
                case IfAction.IfVarType.Mana:
                    expression = "mana";
                    break;
                case IfAction.IfVarType.Stamina:
                    expression = "stam";
                    break;
                case IfVarType.Poisoned:
                    expression = "poisoned";
                    break;
                case IfVarType.SysMessage:
                    expression = $"insysmsg '{m_Value}'";
                    useValue = false;
                    break;
                case IfVarType.Weight:
                    expression = "weight";
                    break;
                case IfVarType.Mounted:
                    expression = "mounted";
                    break;
                case IfVarType.RHandEmpty:
                    expression = "rhandempty";
                    break;
                case IfVarType.LHandEmpty:
                    expression = "lhandempty";
                    break;
                case IfVarType.Counter:
                    expression = $"count '{m_Counter}'";
                    break;
                case IfVarType.Skill:
                    expression = $"skill '{Language.Skill2Str(m_SkillId)}'";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return useValue ? $"if {expression} {op} {m_Value}" : $"if {expression}";

        }

        public override string Serialize()
        {
            if (m_Var == IfVarType.Counter && m_Counter != null)
                return DoSerialize((int) m_Var, m_Direction, m_Value, m_Counter);
            else if (m_Var == IfVarType.Skill && m_SkillId != -1)
                return DoSerialize((int) m_Var, m_Direction, m_Value, m_SkillId);
            else
                return DoSerialize((int) m_Var, m_Direction, m_Value);
        }

        public override bool Perform()
        {
            return true;
        }

        public bool Evaluate()
        {
            switch (m_Var)
            {
                case IfVarType.Hits:
                case IfVarType.Mana:
                case IfVarType.Stamina:
                case IfVarType.Weight:
                {
                    bool isNumeric = true;
                    int val;

                    if (m_Value is string value)
                    {
                        isNumeric = int.TryParse(value, out val);
                    }
                    else
                    {
                        val = Convert.ToInt32(m_Value);
                    }

                    if (!isNumeric && m_Value is string strVal)
                    {
                        if (strVal.Equals("{maxhp}"))
                        {
                            val = World.Player.HitsMax;
                        }
                        else if (strVal.Equals("{maxstam}"))
                        {
                            val = World.Player.StamMax;
                        }
                        else if (strVal.Equals("{maxmana}"))
                        {
                            val = World.Player.ManaMax;
                        }
                        else
                        {
                            val = 0;
                        }
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            // if stat <= m_Value
                            switch (m_Var)
                            {
                                case IfVarType.Hits:
                                    return World.Player.Hits <= val;
                                case IfVarType.Mana:
                                    return World.Player.Mana <= val;
                                case IfVarType.Stamina:
                                    return World.Player.Stam <= val;
                                case IfVarType.Weight:
                                    return World.Player.Weight <= val;
                            }

                            break;
                        case 1:
                            // if stat >= m_Value
                            switch (m_Var)
                            {
                                case IfVarType.Hits:
                                    return World.Player.Hits >= val;
                                case IfVarType.Mana:
                                    return World.Player.Mana >= val;
                                case IfVarType.Stamina:
                                    return World.Player.Stam >= val;
                                case IfVarType.Weight:
                                    return World.Player.Weight >= val;
                            }

                            break;
                        case 2:
                            // if stat < m_Value
                            switch (m_Var)
                            {
                                case IfVarType.Hits:
                                    return World.Player.Hits < val;
                                case IfVarType.Mana:
                                    return World.Player.Mana < val;
                                case IfVarType.Stamina:
                                    return World.Player.Stam < val;
                                case IfVarType.Weight:
                                    return World.Player.Weight < val;
                            }

                            break;
                        case 3:
                            // if stat > m_Value
                            switch (m_Var)
                            {
                                case IfVarType.Hits:
                                    return World.Player.Hits > val;
                                case IfVarType.Mana:
                                    return World.Player.Mana > val;
                                case IfVarType.Stamina:
                                    return World.Player.Stam > val;
                                case IfVarType.Weight:
                                    return World.Player.Weight > val;
                            }

                            break;
                    }

                    return false;
                }

                case IfVarType.Poisoned:
                {
                    if (Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
                        return World.Player.Poisoned;
                    else
                        return false;
                }

                case IfVarType.SysMessage:
                {
                    string text = (string) m_Value;
                    for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
                    {
                        string sys = PacketHandlers.SysMessages[i];
                        if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            PacketHandlers.SysMessages.RemoveRange(0, i + 1);
                            return true;
                        }
                    }

                    return false;
                }

                case IfVarType.Mounted:
                {
                    return World.Player.GetItemOnLayer(Layer.Mount) != null;
                }

                case IfVarType.RHandEmpty:
                {
                    return World.Player.GetItemOnLayer(Layer.RightHand) == null;
                }

                case IfVarType.LHandEmpty:
                {
                    return World.Player.GetItemOnLayer(Layer.LeftHand) == null;
                }

                case IfVarType.Skill:

                    double skillValToCompare;

                    if (m_Value is string skillVal)
                    {
                        double.TryParse(skillVal, out skillValToCompare);
                    }
                    else
                    {
                        skillValToCompare = Convert.ToDouble(m_Value);
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            return World.Player.Skills[m_SkillId].Value <= skillValToCompare;
                        case 1:
                            return World.Player.Skills[m_SkillId].Value >= skillValToCompare;
                        case 2:
                            return World.Player.Skills[m_SkillId].Value < skillValToCompare;
                        case 3:
                            return World.Player.Skills[m_SkillId].Value > skillValToCompare;
                        default:
                            return World.Player.Skills[m_SkillId].Value <= skillValToCompare;
                    }

                case IfVarType.Counter:
                {
                    if (m_CountObj == null)
                    {
                        foreach (Assistant.Counter c in Assistant.Counter.List)
                        {
                            if (c.Name == m_Counter)
                            {
                                m_CountObj = c;
                                break;
                            }
                        }
                    }

                    if (m_CountObj == null || !m_CountObj.Enabled)
                        return false;

                    int val;

                    if (m_Value is string value)
                    {
                        int.TryParse(value, out val);
                    }
                    else
                    {
                        val = Convert.ToInt32(m_Value);
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            return m_CountObj.Amount <= val;
                        case 1:
                            return m_CountObj.Amount >= val;
                        case 2:
                            return m_CountObj.Amount < val;
                        case 3:
                            return m_CountObj.Amount > val;
                        default:
                            return m_CountObj.Amount <= val;
                    }
                }

                default:
                    return false;
            }
        }

        private string DirectionString()
        {
            switch (m_Direction)
            {
                case 0:
                    return "<=";
                case 1:
                    return ">=";
                case 2:
                    return "<";
                case 3:
                    return ">";
                default:
                    return "<=";
            }
        }

        public override string ToString()
        {
            switch (m_Var)
            {
                case IfVarType.Hits:
                case IfVarType.Mana:
                case IfVarType.Stamina:
                case IfVarType.Weight:
                    return $"If ( {m_Var} {DirectionString()} {m_Value} )";
                case IfVarType.Poisoned:
                    return "If ( Poisoned )";
                case IfVarType.SysMessage:
                {
                    string str = (string) m_Value;
                    if (str.Length > 10)
                        str = str.Substring(0, 7) + "...";
                    return String.Format("If ( SysMessage \"{0}\" )", str);
                }

                case IfVarType.Skill:
                    return $"If ( \"{Language.Skill2Str(m_SkillId)}\" {DirectionString()} {m_Value})";
                case IfVarType.Mounted:
                    return "If ( Mounted )";
                case IfVarType.RHandEmpty:
                    return "If ( R-Hand Empty )";
                case IfVarType.LHandEmpty:
                    return "If ( L-Hand Empty )";
                case IfVarType.Counter:
                    return $"If ( \"{m_Counter} count\" {DirectionString()} {m_Value} )";
                default:
                    return "If ( ??? )";
            }
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit))
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertIf(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class ElseAction : MacroAction
    {
        public ElseAction()
        {
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToString()
        {
            return "Else";
        }

        public override string ToScript()
        {
            return "else";
        }
    }

    public class EndIfAction : MacroAction
    {
        public EndIfAction()
        {
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToString()
        {
            return "End If";
        }

        public override string ToScript()
        {
            return "endif";
        }
    }

    public class HotKeyAction : MacroAction
    {
        private KeyData m_Key;

        public HotKeyAction(KeyData hk)
        {
            m_Key = hk;
        }

        public HotKeyAction(string[] args)
        {
            try
            {
                int loc = Convert.ToInt32(args[1]);
                if (loc != 0)
                    m_Key = HotKey.Get(loc);
            }
            catch
            {
            }

            if (m_Key == null)
                m_Key = HotKey.Get(args[2]);

            if (m_Key == null)
                throw new Exception("HotKey not found.");
        }

        public override bool Perform()
        {
            if (Client.Instance.AllowBit(FeatureBit.LoopingMacros) ||
                m_Key.DispName.IndexOf(Language.GetString(LocString.PlayA1).Replace(@"{0}", "")) == -1)
                m_Key.Callback();
            return true;
        }

        public override string ToScript()
        {
            return $"hotkey '{m_Key.DispName}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Key.LocName, m_Key.StrName == null ? "" : m_Key.StrName);
        }

        public override string ToString()
        {
            return String.Format("Exec: {0}", m_Key.DispName);
        }
    }

    public class ForAction : MacroAction
    {
        private int m_Max, m_Count;

        public int Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }

        public int Max
        {
            get { return m_Max; }
        }

        public ForAction(string[] args)
        {
            m_Max = Convert.ToInt32(args[1]);
        }

        public ForAction(int max)
        {
            m_Max = max;
        }

        public override string ToScript()
        {
            return $"for {m_Max}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Max);
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToString()
        {
            return String.Format("For ( 1 to {0} )", m_Max);
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit))
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.NumIter), "Input Box", m_Max.ToString()))
                m_Max = InputBox.GetInt(m_Max);
            if (Parent != null)
                Parent.Update();
        }
    }

    public class EndForAction : MacroAction
    {
        public EndForAction()
        {
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToString()
        {
            return "End For";
        }

        public override string ToScript()
        {
            return "endfor";
        }
    }

    public class WhileAction : MacroAction
    {
        public enum WhileVarType : int
        {
            Hits = 0,
            Mana,
            Stamina,
            Poisoned,
            SysMessage,
            Weight,
            Mounted,
            RHandEmpty,
            LHandEmpty,

            BeginCountersMarker,

            Counter = 50,
            Skill = 100
        }

        // 0 <=,1 >=,2 <,3 >
        private sbyte m_Direction;
        private object m_Value;
        private WhileVarType m_Var;
        private string m_Counter;
        private int m_SkillId = -1;
        private Assistant.Counter m_CountObj;

        public sbyte Op
        {
            get { return m_Direction; }
        }

        public object Value
        {
            get { return m_Value; }
        }

        public WhileVarType Variable
        {
            get { return m_Var; }
        }

        public string Counter
        {
            get { return m_Counter; }
        }

        public int SkillId
        {
            get { return m_SkillId; }
        }

        public WhileAction(string[] args)
        {
            m_Var = (WhileVarType) Convert.ToInt32(args[1]);
            try
            {
                m_Direction = Convert.ToSByte(args[2]);
                if (m_Direction > 3)
                    m_Direction = 0;
            }
            catch
            {
                m_Direction = -1;
            }

            if (m_Var == WhileVarType.SysMessage)
            {
                m_Value = args[3].ToLower();
            }
            else if (m_Var == WhileVarType.Skill)
            {
                if (args[3] is string strVal)
                {
                    m_Value = strVal;
                }
                else
                {
                    m_Value = Convert.ToDouble(args[3]);
                }
            }
            else
            {
                if (args[3] is string strVal)
                {
                    m_Value = strVal;
                }
                else
                {
                    m_Value = Convert.ToInt32(args[3]);
                }
            }

            if (m_Var == WhileVarType.Counter)
                m_Counter = args[4];

            if (m_Var == WhileVarType.Skill)
                m_SkillId = Convert.ToInt32(args[4]);
        }

        public WhileAction(WhileVarType var, sbyte dir, int val)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
        }

        public WhileAction(WhileVarType var, sbyte dir, string val)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
        }

        public WhileAction(WhileVarType var, sbyte dir, int val, string counter)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
            m_Counter = counter;
        }

        public WhileAction(WhileVarType var, sbyte dir, double val, int skillId)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
            m_SkillId = skillId;
        }

        public WhileAction(WhileVarType var, string text)
        {
            m_Var = var;
            m_Value = text.ToLower();
        }

        public override string ToScript()
        {
            string op = "??";
            string expression = "unknown";

            bool useValue = true;

            switch (m_Direction)
            {
                case 0:
                    // if stat <= m_Value
                    op = "<=";
                    break;
                case 1:
                    // if stat >= m_Value
                    op = ">=";
                    break;
                case 2:
                    // if stat < m_Value
                    op = "<";
                    break;
                case 3:
                    // if stat > m_Value
                    op = ">";
                    break;
            }

            switch (m_Var)
            {
                case WhileVarType.Hits:
                    expression = "hits";
                    break;
                case WhileVarType.Mana:
                    expression = "mana";
                    break;
                case WhileVarType.Stamina:
                    expression = "stam";
                    break;
                case WhileVarType.Poisoned:
                    expression = "poisoned";
                    break;
                case WhileVarType.SysMessage:
                    expression = $"insysmsg '{m_Value}'";
                    useValue = false;
                    break;
                case WhileVarType.Weight:
                    expression = "weight";
                    break;
                case WhileVarType.Mounted:
                    expression = "mounted";
                    break;
                case WhileVarType.RHandEmpty:
                    expression = "rhandempty";
                    break;
                case WhileVarType.LHandEmpty:
                    expression = "lhandempty";
                    break;
                case WhileVarType.Counter:
                    expression = $"count '{m_Counter}'";
                    break;
                case WhileVarType.Skill:
                    expression = $"skill '{Language.Skill2Str(m_SkillId)}'";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return useValue ? $"while {expression} {op} {m_Value}" : $"while {expression}";
        }

        public override string Serialize()
        {
            if (m_Var == WhileVarType.Counter && m_Counter != null)
                return DoSerialize((int) m_Var, m_Direction, m_Value, m_Counter);
            else if (m_Var == WhileVarType.Skill && m_SkillId != -1)
                return DoSerialize((int) m_Var, m_Direction, m_Value, m_SkillId);
            else
                return DoSerialize((int) m_Var, m_Direction, m_Value);
        }

        public override bool Perform()
        {
            return true;
        }

        public bool Evaluate()
        {
            switch (m_Var)
            {
                case WhileVarType.Hits:
                case WhileVarType.Mana:
                case WhileVarType.Stamina:
                case WhileVarType.Weight:
                {
                    bool isNumeric = true;
                    int val;

                    if (m_Value is string value)
                    {
                        isNumeric = int.TryParse(value, out val);
                    }
                    else
                    {
                        val = Convert.ToInt32(m_Value);
                    }

                    if (!isNumeric && m_Value is string strVal)
                    {
                        if (strVal.Equals("{maxhp}"))
                        {
                            val = World.Player.HitsMax;
                        }
                        else if (strVal.Equals("{maxstam}"))
                        {
                            val = World.Player.StamMax;
                        }
                        else if (strVal.Equals("{maxmana}"))
                        {
                            val = World.Player.ManaMax;
                        }
                        else
                        {
                            val = 0;
                        }
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            // if stat <= m_Value
                            switch (m_Var)
                            {
                                case WhileVarType.Hits:
                                    return World.Player.Hits <= val;
                                case WhileVarType.Mana:
                                    return World.Player.Mana <= val;
                                case WhileVarType.Stamina:
                                    return World.Player.Stam <= val;
                                case WhileVarType.Weight:
                                    return World.Player.Weight <= val;
                            }

                            break;
                        case 1:
                            // if stat >= m_Value
                            switch (m_Var)
                            {
                                case WhileVarType.Hits:
                                    return World.Player.Hits >= val;
                                case WhileVarType.Mana:
                                    return World.Player.Mana >= val;
                                case WhileVarType.Stamina:
                                    return World.Player.Stam >= val;
                                case WhileVarType.Weight:
                                    return World.Player.Weight >= val;
                            }

                            break;
                        case 2:
                            // if stat < m_Value
                            switch (m_Var)
                            {
                                case WhileVarType.Hits:
                                    return World.Player.Hits < val;
                                case WhileVarType.Mana:
                                    return World.Player.Mana < val;
                                case WhileVarType.Stamina:
                                    return World.Player.Stam < val;
                                case WhileVarType.Weight:
                                    return World.Player.Weight < val;
                            }

                            break;
                        case 3:
                            // if stat > m_Value
                            switch (m_Var)
                            {
                                case WhileVarType.Hits:
                                    return World.Player.Hits > val;
                                case WhileVarType.Mana:
                                    return World.Player.Mana > val;
                                case WhileVarType.Stamina:
                                    return World.Player.Stam > val;
                                case WhileVarType.Weight:
                                    return World.Player.Weight > val;
                            }

                            break;
                    }

                    return false;
                }

                case WhileVarType.Poisoned:
                {
                    if (Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
                        return World.Player.Poisoned;
                    else
                        return false;
                }

                case WhileVarType.SysMessage:
                {
                    string text = (string) m_Value;
                    for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
                    {
                        string sys = PacketHandlers.SysMessages[i];
                        if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            PacketHandlers.SysMessages.RemoveRange(0, i + 1);
                            return true;
                        }
                    }

                    return false;
                }

                case WhileVarType.Mounted:
                {
                    return World.Player.GetItemOnLayer(Layer.Mount) != null;
                }

                case WhileVarType.RHandEmpty:
                {
                    return World.Player.GetItemOnLayer(Layer.RightHand) == null;
                }

                case WhileVarType.LHandEmpty:
                {
                    return World.Player.GetItemOnLayer(Layer.LeftHand) == null;
                }

                case WhileVarType.Skill:

                    double skillValToCompare;

                    if (m_Value is string skillVal)
                    {
                        double.TryParse(skillVal, out skillValToCompare);
                    }
                    else
                    {
                        skillValToCompare = Convert.ToDouble(m_Value);
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            return World.Player.Skills[m_SkillId].Value <= skillValToCompare;
                        case 1:
                            return World.Player.Skills[m_SkillId].Value >= skillValToCompare;
                        case 2:
                            return World.Player.Skills[m_SkillId].Value < skillValToCompare;
                        case 3:
                            return World.Player.Skills[m_SkillId].Value > skillValToCompare;
                        default:
                            return World.Player.Skills[m_SkillId].Value <= skillValToCompare;
                    }

                case WhileVarType.Counter:
                {
                    if (m_CountObj == null)
                    {
                        foreach (Assistant.Counter c in Assistant.Counter.List)
                        {
                            if (c.Name == m_Counter)
                            {
                                m_CountObj = c;
                                break;
                            }
                        }
                    }

                    if (m_CountObj == null || !m_CountObj.Enabled)
                        return false;

                    int val;

                    if (m_Value is string value)
                    {
                        int.TryParse(value, out val);
                    }
                    else
                    {
                        val = Convert.ToInt32(m_Value);
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            return m_CountObj.Amount <= val;
                        case 1:
                            return m_CountObj.Amount >= val;
                        case 2:
                            return m_CountObj.Amount < val;
                        case 3:
                            return m_CountObj.Amount > val;
                        default:
                            return m_CountObj.Amount <= val;
                    }
                }

                default:
                    return false;
            }
        }

        private string DirectionString()
        {
            switch (m_Direction)
            {
                case 0:
                    return "<=";
                case 1:
                    return ">=";
                case 2:
                    return "<";
                case 3:
                    return ">";
                default:
                    return "<=";
            }
        }

        public override string ToString()
        {
            switch (m_Var)
            {
                case WhileVarType.Hits:
                case WhileVarType.Mana:
                case WhileVarType.Stamina:
                case WhileVarType.Weight:
                    return $"While ( {m_Var} {DirectionString()} {m_Value} )";
                case WhileVarType.Poisoned:
                    return "While ( Poisoned )";
                case WhileVarType.SysMessage:
                {
                    string str = (string) m_Value;
                    if (str.Length > 10)
                        str = str.Substring(0, 7) + "...";
                    return $"While ( SysMessage \"{str}\" )";
                }

                case WhileVarType.Skill:
                    return $"While ( \"{Language.Skill2Str(m_SkillId)}\" {DirectionString()} {m_Value})";
                case WhileVarType.Mounted:
                    return "While ( Mounted )";
                case WhileVarType.RHandEmpty:
                    return "While ( R-Hand Empty )";
                case WhileVarType.LHandEmpty:
                    return "While ( L-Hand Empty )";
                case WhileVarType.Counter:
                    return $"While ( \"{m_Counter} count\" {DirectionString()} {m_Value} )";
                default:
                    return "While ( ??? )";
            }
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit))
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertWhile(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class EndWhileAction : MacroAction
    {
        public EndWhileAction()
        {
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToString()
        {
            return "End While";
        }

        public override string ToScript()
        {
            return "endwhile";
        }
    }

    public class StartDoWhileAction : MacroAction
    {
        public StartDoWhileAction()
        {
        }

        public override bool Perform()
        {
            return true;
        }

        public override string ToString()
        {
            return "Do";
        }

        public override string ToScript()
        {
            return "# do-while not implemented, use while";
        }
    }

    public class DoWhileAction : MacroAction
    {
        public enum DoWhileVarType : int
        {
            Hits = 0,
            Mana,
            Stamina,
            Poisoned,
            SysMessage,
            Weight,
            Mounted,
            RHandEmpty,
            LHandEmpty,

            BeginCountersMarker,

            Counter = 50,
            Skill = 100
        }

        // 0 <=,1 >=,2 <,3 >
        private sbyte m_Direction;
        private object m_Value;
        private DoWhileVarType m_Var;
        private string m_Counter;
        private int m_SkillId = -1;
        private Assistant.Counter m_CountObj;

        public sbyte Op
        {
            get { return m_Direction; }
        }

        public object Value
        {
            get { return m_Value; }
        }

        public DoWhileVarType Variable
        {
            get { return m_Var; }
        }

        public string Counter
        {
            get { return m_Counter; }
        }

        public int SkillId
        {
            get { return m_SkillId; }
        }

        public DoWhileAction(string[] args)
        {
            m_Var = (DoWhileVarType) Convert.ToInt32(args[1]);
            try
            {
                m_Direction = Convert.ToSByte(args[2]);
                if (m_Direction > 3)
                    m_Direction = 0;
            }
            catch
            {
                m_Direction = -1;
            }

            if (m_Var == DoWhileVarType.SysMessage)
            {
                m_Value = args[3].ToLower();
            }
            else if (m_Var == DoWhileVarType.Skill)
            {
                if (args[3] is string strVal)
                {
                    m_Value = strVal;
                }
                else
                {
                    m_Value = Convert.ToDouble(args[3]);
                }
            }
            else
            {
                if (args[3] is string strVal)
                {
                    m_Value = strVal;
                }
                else
                {
                    m_Value = Convert.ToInt32(args[3]);
                }
            }

            if (m_Var == DoWhileVarType.Counter)
                m_Counter = args[4];

            if (m_Var == DoWhileVarType.Skill)
                m_SkillId = Convert.ToInt32(args[4]);
        }

        public DoWhileAction(DoWhileVarType var, sbyte dir, int val)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
        }

        public DoWhileAction(DoWhileVarType var, sbyte dir, string val)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
        }

        public DoWhileAction(DoWhileVarType var, sbyte dir, int val, string counter)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
            m_Counter = counter;
        }

        public DoWhileAction(DoWhileVarType var, sbyte dir, double val, int skillId)
        {
            m_Var = var;
            m_Direction = dir;
            m_Value = val;
            m_SkillId = skillId;
        }

        public DoWhileAction(DoWhileVarType var, string text)
        {
            m_Var = var;
            m_Value = text.ToLower();
        }

        public override string ToScript()
        {
            return "# do-while not implemented, use while";
        }

        public override string Serialize()
        {
            if (m_Var == DoWhileVarType.Counter && m_Counter != null)
                return DoSerialize((int) m_Var, m_Direction, m_Value, m_Counter);
            else if (m_Var == DoWhileVarType.Skill && m_SkillId != -1)
                return DoSerialize((int) m_Var, m_Direction, m_Value, m_SkillId);
            else
                return DoSerialize((int) m_Var, m_Direction, m_Value);
        }

        public override bool Perform()
        {
            return true;
        }

        public bool Evaluate()
        {
            switch (m_Var)
            {
                case DoWhileVarType.Hits:
                case DoWhileVarType.Mana:
                case DoWhileVarType.Stamina:
                case DoWhileVarType.Weight:
                {
                    bool isNumeric = true;
                    int val;

                    if (m_Value is string value)
                    {
                        isNumeric = int.TryParse(value, out val);
                    }
                    else
                    {
                        val = Convert.ToInt32(m_Value);
                    }

                    if (!isNumeric && m_Value is string strVal)
                    {
                        if (strVal.Equals("{maxhp}"))
                        {
                            val = World.Player.HitsMax;
                        }
                        else if (strVal.Equals("{maxstam}"))
                        {
                            val = World.Player.StamMax;
                        }
                        else if (strVal.Equals("{maxmana}"))
                        {
                            val = World.Player.ManaMax;
                        }
                        else
                        {
                            val = 0;
                        }
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            // if stat <= m_Value
                            switch (m_Var)
                            {
                                case DoWhileVarType.Hits:
                                    return World.Player.Hits <= val;
                                case DoWhileVarType.Mana:
                                    return World.Player.Mana <= val;
                                case DoWhileVarType.Stamina:
                                    return World.Player.Stam <= val;
                                case DoWhileVarType.Weight:
                                    return World.Player.Weight <= val;
                            }

                            break;
                        case 1:
                            // if stat >= m_Value
                            switch (m_Var)
                            {
                                case DoWhileVarType.Hits:
                                    return World.Player.Hits >= val;
                                case DoWhileVarType.Mana:
                                    return World.Player.Mana >= val;
                                case DoWhileVarType.Stamina:
                                    return World.Player.Stam >= val;
                                case DoWhileVarType.Weight:
                                    return World.Player.Weight >= val;
                            }

                            break;
                        case 2:
                            // if stat < m_Value
                            switch (m_Var)
                            {
                                case DoWhileVarType.Hits:
                                    return World.Player.Hits < val;
                                case DoWhileVarType.Mana:
                                    return World.Player.Mana < val;
                                case DoWhileVarType.Stamina:
                                    return World.Player.Stam < val;
                                case DoWhileVarType.Weight:
                                    return World.Player.Weight < val;
                            }

                            break;
                        case 3:
                            // if stat > m_Value
                            switch (m_Var)
                            {
                                case DoWhileVarType.Hits:
                                    return World.Player.Hits > val;
                                case DoWhileVarType.Mana:
                                    return World.Player.Mana > val;
                                case DoWhileVarType.Stamina:
                                    return World.Player.Stam > val;
                                case DoWhileVarType.Weight:
                                    return World.Player.Weight > val;
                            }

                            break;
                    }

                    return false;
                }

                case DoWhileVarType.Poisoned:
                {
                    if (Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
                        return World.Player.Poisoned;
                    else
                        return false;
                }

                case DoWhileVarType.SysMessage:
                {
                    string text = (string) m_Value;
                    for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
                    {
                        string sys = PacketHandlers.SysMessages[i];
                        if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            PacketHandlers.SysMessages.RemoveRange(0, i + 1);
                            return true;
                        }
                    }

                    return false;
                }

                case DoWhileVarType.Mounted:
                {
                    return World.Player.GetItemOnLayer(Layer.Mount) != null;
                }

                case DoWhileVarType.RHandEmpty:
                {
                    return World.Player.GetItemOnLayer(Layer.RightHand) == null;
                }

                case DoWhileVarType.LHandEmpty:
                {
                    return World.Player.GetItemOnLayer(Layer.LeftHand) == null;
                }

                case DoWhileVarType.Skill:

                    double skillValToCompare;

                    if (m_Value is string skillVal)
                    {
                        double.TryParse(skillVal, out skillValToCompare);
                    }
                    else
                    {
                        skillValToCompare = Convert.ToDouble(m_Value);
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            return World.Player.Skills[m_SkillId].Value <= skillValToCompare;
                        case 1:
                            return World.Player.Skills[m_SkillId].Value >= skillValToCompare;
                        case 2:
                            return World.Player.Skills[m_SkillId].Value < skillValToCompare;
                        case 3:
                            return World.Player.Skills[m_SkillId].Value > skillValToCompare;
                        default:
                            return World.Player.Skills[m_SkillId].Value <= skillValToCompare;
                    }

                case DoWhileVarType.Counter:
                {
                    if (m_CountObj == null)
                    {
                        foreach (Assistant.Counter c in Assistant.Counter.List)
                        {
                            if (c.Name == m_Counter)
                            {
                                m_CountObj = c;
                                break;
                            }
                        }
                    }

                    if (m_CountObj == null || !m_CountObj.Enabled)
                        return false;

                    int val;

                    if (m_Value is string value)
                    {
                        int.TryParse(value, out val);
                    }
                    else
                    {
                        val = Convert.ToInt32(m_Value);
                    }

                    switch (m_Direction)
                    {
                        case 0:
                            return m_CountObj.Amount <= val;
                        case 1:
                            return m_CountObj.Amount >= val;
                        case 2:
                            return m_CountObj.Amount < val;
                        case 3:
                            return m_CountObj.Amount > val;
                        default:
                            return m_CountObj.Amount <= val;
                    }
                }

                default:
                    return false;
            }
        }

        private string DirectionString()
        {
            switch (m_Direction)
            {
                case 0:
                    return "<=";
                case 1:
                    return ">=";
                case 2:
                    return "<";
                case 3:
                    return ">";
                default:
                    return "<=";
            }
        }

        public override string ToString()
        {
            switch (m_Var)
            {
                case DoWhileVarType.Hits:
                case DoWhileVarType.Mana:
                case DoWhileVarType.Stamina:
                case DoWhileVarType.Weight:
                    return $"Do While ( {m_Var} {DirectionString()} {m_Value} )";
                case DoWhileVarType.Poisoned:
                    return "Do While ( Poisoned )";
                case DoWhileVarType.SysMessage:
                {
                    string str = (string) m_Value;
                    if (str.Length > 10)
                        str = str.Substring(0, 7) + "...";
                    return String.Format("Do While ( SysMessage \"{0}\" )", str);
                }

                case DoWhileVarType.Skill:
                    return $"Do While ( \"{Language.Skill2Str(m_SkillId)}\" {DirectionString()} {m_Value})";
                case DoWhileVarType.Mounted:
                    return "Do While ( Mounted )";
                case DoWhileVarType.RHandEmpty:
                    return "Do While ( R-Hand Empty )";
                case DoWhileVarType.LHandEmpty:
                    return "Do While ( L-Hand Empty )";
                case DoWhileVarType.Counter:
                    return $"Do While ( \"{m_Counter} count\" {DirectionString()} {m_Value} )";
                default:
                    return "Do While ( ??? )";
            }
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit))
                };
            }

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertDoWhile(this).ShowDialog(Engine.MainWindow);
        }
    }

    public class ContextMenuAction : MacroAction
    {
        private ushort m_CtxName;
        private ushort m_Idx;
        private Serial m_Entity;

        public ContextMenuAction(UOEntity ent, ushort idx, ushort ctxName)
        {
            m_Entity = ent != null ? ent.Serial : Serial.MinusOne;

            if (World.Player != null && World.Player.Serial == m_Entity)
                m_Entity = Serial.Zero;

            m_Idx = idx;
            m_CtxName = ctxName;
        }

        public ContextMenuAction(string[] args)
        {
            m_Entity = Serial.Parse(args[1]);
            m_Idx = Convert.ToUInt16(args[2]);
            try
            {
                m_CtxName = Convert.ToUInt16(args[3]);
            }
            catch
            {
            }
        }

        public override bool Perform()
        {
            Serial s = m_Entity;

            if (s == Serial.Zero && World.Player != null)
                s = World.Player.Serial;

            Client.Instance.SendToServer(new ContextMenuRequest(s));
            Client.Instance.SendToServer(new ContextMenuResponse(s, m_Idx));
            return true;
        }

        public override string ToScript()
        {
            return $"menu {m_Entity} {m_Idx}";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Entity, m_Idx, m_CtxName);
        }

        public override string ToString()
        {
            string ent;

            if (m_Entity == Serial.Zero)
                ent = "(self)";
            else
                ent = m_Entity.ToString();
            return String.Format("ContextMenu: {1} ({0})", ent, m_Idx);
        }
    }

    public class PromptAction : MacroAction
    {
        private string m_Response;

        public PromptAction(string[] args)
        {
            m_Response = args[1];
        }

        public PromptAction(string response)
        {
            m_Response = response;
        }

        public override bool Perform()
        {
            if (m_Response.Length > 1)
            {
                World.Player.ResponsePrompt(m_Response);

                return true;
            }

            return false;
        }

        public override string ToScript()
        {
            return $"promptresponse '{m_Response}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_Response);
        }

        public override string ToString()
        {
            return $"PromptAction: {m_Response}";
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (this.m_MenuItems == null)
                this.m_MenuItems = (MenuItem[]) new MacroMenuItem[1]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(this.Edit), new object[0])
                };
            return this.m_MenuItems;
        }

        private void Edit(object[] args)
        {
            if (InputBox.Show(Language.GetString(LocString.EnterNewText), "Input Box", this.m_Response))
                m_Response = InputBox.GetString();

            Parent?.Update();
        }
    }

    public class WaitForPromptAction : MacroWaitAction
    {
        private uint m_PromptID;
        private bool m_Strict;

        public WaitForPromptAction()
        {
            m_PromptID = 0;
            m_Strict = false;
        }

        public WaitForPromptAction(uint gid)
        {
            m_PromptID = gid;
            m_Strict = false;
        }

        public WaitForPromptAction(string[] args)
        {
            m_PromptID = Convert.ToUInt32(args[1]);
            try
            {
                m_Strict = Convert.ToBoolean(args[2]);
            }
            catch
            {
                m_Strict = false;
            }

            try
            {
                m_Timeout = TimeSpan.FromSeconds(Convert.ToDouble(args[3]));
            }
            catch
            {
            }
        }

        public override bool Perform()
        {
            return !PerformWait();
        }

        public override bool PerformWait()
        {
            return !(World.Player.HasPrompt && (World.Player.PromptID == m_PromptID || !m_Strict || m_PromptID == 0));
        }

        public override string ToString()
        {
            //if (m_PromptID == 0 || !m_Strict)
            //    return Language.GetString(LocString.WaitAnyGump);
            //else
            //    return Language.Format(LocString.WaitGumpA1, m_GumpID);

            if (m_PromptID == 0 || !m_Strict)
                return "Wait For Prompt (Any)";

            return $"Wait For Prompt ({m_PromptID})";
        }

        public override string ToScript()
        {
            return m_PromptID == 0 || !m_Strict ? "waitforprompt" : $"waitforprompt '{m_PromptID}'";
        }

        public override string Serialize()
        {
            return DoSerialize(m_PromptID, m_Strict, m_Timeout.TotalSeconds);
        }

        public override bool CheckMatch(MacroAction a)
        {
            if (a is WaitForGumpAction)
            {
                if (m_PromptID == 0 || ((WaitForPromptAction) a).m_PromptID == m_PromptID)
                    return true;
            }

            return false;
        }

        private MenuItem[] m_MenuItems;

        public override MenuItem[] GetContextMenuItems()
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem(LocString.Edit, new MacroMenuCallback(Edit)),
                    new MacroMenuItem(LocString.Null, new MacroMenuCallback(ToggleStrict)),
                    this.EditTimeoutMenuItem
                };
            }

            if (!m_Strict)
                m_MenuItems[1].Text = $"Change to \"Wait For Prompt ({m_PromptID})\"";
            else
                m_MenuItems[1].Text = $"Change to \"Wait For Prompt (Any)\"";

            m_MenuItems[1].Enabled = m_PromptID != 0 || m_Strict;

            return m_MenuItems;
        }

        private void Edit(object[] args)
        {
            new MacroInsertWait(this).ShowDialog(Engine.MainWindow);
        }

        private void ToggleStrict(object[] args)
        {
            m_Strict = !m_Strict;
            if (m_Parent != null)
                m_Parent.Update();
        }
    }
}