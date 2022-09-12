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

using System.IO;
using System.Collections.Generic;

namespace Assistant
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

        private Serial _serial;
        private Point3D _pos;
        private ushort _hue;
        private bool _deleted;
        private ContextMenuList _contextMenu = new ContextMenuList();
        protected ObjectPropertyList _objPropList = null;

        public ObjectPropertyList ObjPropList
        {
            get { return _objPropList; }
        }

        public virtual void AfterLoad()
        {
        }

        public UOEntity(Serial ser)
        {
            _objPropList = new ObjectPropertyList(this);

            _serial = ser;
            _deleted = false;
        }

        public Serial Serial
        {
            get { return _serial; }
        }

        public virtual Point3D Position
        {
            get { return _pos; }
            set
            {
                if (value != _pos)
                {
                    var oldPos = _pos;
                    _pos = value;
                    OnPositionChanging(oldPos);
                }
            }
        }

        public bool Deleted
        {
            get { return _deleted; }
        }

        public ContextMenuList ContextMenu
        {
            get { return _contextMenu; }
        }

        public virtual ushort Hue
        {
            get { return _hue; }
            set { _hue = value; }
        }

        public virtual void Remove()
        {
            _deleted = true;
        }

        public virtual void OnPositionChanging(Point3D oldPos)
        {
        }

        public override int GetHashCode()
        {
            return _serial.GetHashCode();
        }

        public int OPLHash
        {
            get
            {
                if (_objPropList != null)
                    return _objPropList.Hash;
                else
                    return 0;
            }
            set
            {
                if (_objPropList != null)
                    _objPropList.Hash = value;
            }
        }

        public bool ModifiedOPL
        {
            get { return _objPropList.Customized; }
        }

        public void ReadPropertyList(PacketReader p)
        {
            _objPropList.Read(p);
        }

        /*public Packet BuildOPLPacket()
        { 
            return m_ObjPropList.BuildPacket();
        }*/

        public void OPLChanged()
        {
            //Client.Instance.SendToClient( m_ObjPropList.BuildPacket() );
            Client.Instance.SendToClient(new OPLInfo(Serial, OPLHash));
        }
    }
}