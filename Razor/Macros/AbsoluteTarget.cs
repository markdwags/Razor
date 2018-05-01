using System;
using System.Text;
using System.Windows.Forms;

namespace Assistant.Macros
{
    public class AbsoluteTarget
    {
        public TargetInfo TargetInfo { get; set; }
        public string TargetVariableName { get; set; }
        public string TargetVariableProfile { get; set; }

        public AbsoluteTarget(string[] args)
        {
            TargetInfo = new TargetInfo
            {
                Type = Convert.ToByte(args[2]),
                Flags = Convert.ToByte(args[3]),
                Serial = Convert.ToUInt32(args[4]),
                X = Convert.ToUInt16(args[5]),
                Y = Convert.ToUInt16(args[6]),
                Z = Convert.ToInt16(args[7]),
                Gfx = Convert.ToUInt16(args[8])
            };

            TargetVariableName = args[0];
            TargetVariableProfile = args[1];
        }

        public AbsoluteTarget(string name, string profile, TargetInfo info)
        {
            TargetInfo = new TargetInfo
            {
                Type = info.Type,
                Flags = info.Flags,
                Serial = info.Serial,
                X = info.X,
                Y = info.Y,
                Z = info.Z,
                Gfx = info.Gfx
            };

            TargetVariableName = name;
            TargetVariableProfile = profile;
        }

        public string Serialize()
        {

            object[] serialString = new object[]
            {
                TargetVariableName, TargetVariableProfile, TargetInfo.Type, TargetInfo.Flags, TargetInfo.Serial.Value, TargetInfo.X, TargetInfo.Y,
                TargetInfo.Z, TargetInfo.Gfx
            };

            StringBuilder sb = new StringBuilder();

            foreach (var serial in serialString)
            {
                sb.AppendFormat("{0}|", serial);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return $"${TargetVariableName} ({TargetInfo.Serial})";
        }

        /*private MenuItem[] _targetMenuItems;
        public MenuItem[] GetContextMenuItems()
        {
            if (_targetMenuItems == null)
            {
                _targetMenuItems = new MacroMenuItem[]
                {
                    new MacroMenuItem( LocString.ReTarget, new MacroMenuCallback( ReTarget ) )
                };
            }

            return _targetMenuItems;
        }

        private void ReTarget(object[] args)
        {
            Targeting.OneTimeTarget(!TargetInfo.Serial.IsValid, new Targeting.TargetResponseCallback(ReTargetResponse));
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void ReTargetResponse(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo.Gfx = gfx;
            TargetInfo.Serial = serial;
            TargetInfo.Type = (byte)(ground ? 1 : 0);
            TargetInfo.X = pt.X;
            TargetInfo.Y = pt.Y;
            TargetInfo.Z = pt.Z;

            Engine.MainWindow.ShowMe();
            
        }*/
    }
}
