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

using System.Collections.Generic;
using System.IO;
using Assistant.Network;

namespace Assistant.Core
{
    public class UOEntity
    {
        public class ContextMenuList : List<KeyValuePair<ushort, ushort>>
        {
            public void Add(ushort key, ushort value)
            {
                var element = new KeyValuePair<ushort, ushort>(key, value);
                Add(element);
            }
        }

        private Serial m_Serial;
        private Point3D m_Pos;
        private ushort m_Hue;
        private bool m_Deleted;
        private ContextMenuList m_ContextMenu = new ContextMenuList();
        protected ObjectPropertyList m_ObjPropList = null;

        public ObjectPropertyList ObjPropList
        {
            get { return m_ObjPropList; }
        }

        public virtual void SaveState(BinaryWriter writer)
        {
            writer.Write((uint) m_Serial);
            writer.Write((int) m_Pos.X);
            writer.Write((int) m_Pos.Y);
            writer.Write((int) m_Pos.Z);
            writer.Write((ushort) m_Hue);
        }

        public UOEntity(BinaryReader reader, int version)
        {
            m_Serial = reader.ReadUInt32();
            m_Pos = new Point3D(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            m_Hue = reader.ReadUInt16();
            m_Deleted = false;

            m_ObjPropList = new ObjectPropertyList(this);
        }

        public virtual void AfterLoad()
        {
        }

        public UOEntity(Serial ser)
        {
            m_ObjPropList = new ObjectPropertyList(this);

            m_Serial = ser;
            m_Deleted = false;
        }

        public Serial Serial
        {
            get { return m_Serial; }
        }

        public virtual Point3D Position
        {
            get { return m_Pos; }
            set
            {
                if (value != m_Pos)
                {
                    var oldPos = m_Pos;
                    m_Pos = value;
                    OnPositionChanging(oldPos);
                }
            }
        }

        public bool Deleted
        {
            get { return m_Deleted; }
        }

        public ContextMenuList ContextMenu
        {
            get { return m_ContextMenu; }
        }

        public virtual ushort Hue
        {
            get { return m_Hue; }
            set { m_Hue = value; }
        }

        public virtual void Remove()
        {
            m_Deleted = true;
        }

        public virtual void OnPositionChanging(Point3D oldPos)
        {
        }

        public override int GetHashCode()
        {
            return m_Serial.GetHashCode();
        }

        public int OPLHash
        {
            get
            {
                if (m_ObjPropList != null)
                    return m_ObjPropList.Hash;
                else
                    return 0;
            }
            set
            {
                if (m_ObjPropList != null)
                    m_ObjPropList.Hash = value;
            }
        }

        public bool ModifiedOPL
        {
            get { return m_ObjPropList.Customized; }
        }

        public void ReadPropertyList(PacketReader p)
        {
            m_ObjPropList.Read(p);
        }

        /*public Packet BuildOPLPacket()
        { 
            return m_ObjPropList.BuildPacket();
        }*/

        public void OPLChanged()
        {
            //Client.Instance.SendToClient( m_ObjPropList.BuildPacket() );
            Client.Client.Instance.SendToClient(new OPLInfo(Serial, OPLHash));
        }
    }
}