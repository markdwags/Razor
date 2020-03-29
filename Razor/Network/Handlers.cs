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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assistant.Agents;
using Assistant.Core;
using Assistant.Filters;
using Assistant.Macros;
using Assistant.Scripts;
using Assistant.UI;
using OverheadMessages = Assistant.Core.OverheadMessages;
using ContainerLabels = Assistant.Core.ContainerLabels;

namespace Assistant
{
    public class PacketHandlers
    {
        private static List<Item> m_IgnoreGumps = new List<Item>();

        public static List<Item> IgnoreGumps
        {
            get { return m_IgnoreGumps; }
        }

        public static void Initialize()
        {
            //Client -> Server handlers
            PacketHandler.RegisterClientToServerViewer(0x00, new PacketViewerCallback(CreateCharacter));
            //PacketHandler.RegisterClientToServerViewer(0x01, new PacketViewerCallback(Disconnect));
            PacketHandler.RegisterClientToServerFilter(0x02, new PacketFilterCallback(MovementRequest));
            PacketHandler.RegisterClientToServerFilter(0x05, new PacketFilterCallback(AttackRequest));
            PacketHandler.RegisterClientToServerViewer(0x06, new PacketViewerCallback(ClientDoubleClick));
            PacketHandler.RegisterClientToServerViewer(0x07, new PacketViewerCallback(LiftRequest));
            PacketHandler.RegisterClientToServerViewer(0x08, new PacketViewerCallback(DropRequest));
            PacketHandler.RegisterClientToServerViewer(0x09, new PacketViewerCallback(ClientSingleClick));
            PacketHandler.RegisterClientToServerViewer(0x12, new PacketViewerCallback(ClientTextCommand));
            PacketHandler.RegisterClientToServerViewer(0x13, new PacketViewerCallback(EquipRequest));
            // 0x29 - UOKR confirm drop.  0 bytes payload (just a single byte, 0x29, no length or data)
            PacketHandler.RegisterClientToServerViewer(0x3A, new PacketViewerCallback(SetSkillLock));
            PacketHandler.RegisterClientToServerViewer(0x5D, new PacketViewerCallback(PlayCharacter));
            PacketHandler.RegisterClientToServerViewer(0x7D, new PacketViewerCallback(MenuResponse));
            PacketHandler.RegisterClientToServerFilter(0x80, new PacketFilterCallback(ServerListLogin));
            PacketHandler.RegisterClientToServerFilter(0x91, new PacketFilterCallback(GameLogin));
            PacketHandler.RegisterClientToServerViewer(0x95, new PacketViewerCallback(HueResponse));
            PacketHandler.RegisterClientToServerViewer(0xA0, new PacketViewerCallback(PlayServer));
            PacketHandler.RegisterClientToServerViewer(0xB1, new PacketViewerCallback(ClientGumpResponse));
            PacketHandler.RegisterClientToServerFilter(0xBF, new PacketFilterCallback(ExtendedClientCommand));
            //PacketHandler.RegisterClientToServerViewer( 0xD6, new PacketViewerCallback( BatchQueryProperties ) );
            PacketHandler.RegisterClientToServerFilter(0xC2, new PacketFilterCallback(UnicodePromptSend));
            PacketHandler.RegisterClientToServerViewer(0xD7, new PacketViewerCallback(ClientEncodedPacket));
            PacketHandler.RegisterClientToServerViewer(0xF8, new PacketViewerCallback(CreateCharacter));

            //Server -> Client handlers
            PacketHandler.RegisterServerToClientViewer(0x0B, new PacketViewerCallback(Damage));
            PacketHandler.RegisterServerToClientViewer(0x11, new PacketViewerCallback(MobileStatus));
            PacketHandler.RegisterServerToClientViewer(0x17, new PacketViewerCallback(NewMobileStatus));
            PacketHandler.RegisterServerToClientViewer(0x1A, new PacketViewerCallback(WorldItem));
            PacketHandler.RegisterServerToClientViewer(0x1B, new PacketViewerCallback(LoginConfirm));
            PacketHandler.RegisterServerToClientViewer(0x55, new PacketViewerCallback(CompleteLogin));
            PacketHandler.RegisterServerToClientFilter(0x1C, new PacketFilterCallback(AsciiSpeech));
            PacketHandler.RegisterServerToClientViewer(0x1D, new PacketViewerCallback(RemoveObject));
            PacketHandler.RegisterServerToClientFilter(0x20, new PacketFilterCallback(MobileUpdate));
            PacketHandler.RegisterServerToClientViewer(0x24, new PacketViewerCallback(BeginContainerContent));
            PacketHandler.RegisterServerToClientFilter(0x25, new PacketFilterCallback(ContainerContentUpdate));
            PacketHandler.RegisterServerToClientViewer(0x27, new PacketViewerCallback(LiftReject));
            PacketHandler.RegisterServerToClientViewer(0x2D, new PacketViewerCallback(MobileStatInfo));
            PacketHandler.RegisterServerToClientFilter(0x2E, new PacketFilterCallback(EquipmentUpdate));
            PacketHandler.RegisterServerToClientViewer(0x3A, new PacketViewerCallback(Skills));
            PacketHandler.RegisterServerToClientFilter(0x3C, new PacketFilterCallback(ContainerContent));
            PacketHandler.RegisterServerToClientViewer(0x4E, new PacketViewerCallback(PersonalLight));
            PacketHandler.RegisterServerToClientViewer(0x4F, new PacketViewerCallback(GlobalLight));
            PacketHandler.RegisterServerToClientViewer(0x6F, new PacketViewerCallback(TradeRequest));
            PacketHandler.RegisterServerToClientViewer(0x72, new PacketViewerCallback(ServerSetWarMode));
            PacketHandler.RegisterServerToClientViewer(0x73, new PacketViewerCallback(PingResponse));
            PacketHandler.RegisterServerToClientViewer(0x76, new PacketViewerCallback(ServerChange));
            PacketHandler.RegisterServerToClientFilter(0x77, new PacketFilterCallback(MobileMoving));
            PacketHandler.RegisterServerToClientFilter(0x78, new PacketFilterCallback(MobileIncoming));
            PacketHandler.RegisterServerToClientViewer(0x7C, new PacketViewerCallback(SendMenu));
            PacketHandler.RegisterServerToClientFilter(0x8C, new PacketFilterCallback(ServerAddress));
            PacketHandler.RegisterServerToClientViewer(0xA1, new PacketViewerCallback(HitsUpdate));
            PacketHandler.RegisterServerToClientViewer(0xA2, new PacketViewerCallback(ManaUpdate));
            PacketHandler.RegisterServerToClientViewer(0xA3, new PacketViewerCallback(StamUpdate));
            PacketHandler.RegisterServerToClientViewer(0xA8, new PacketViewerCallback(ServerList));
            PacketHandler.RegisterServerToClientViewer(0xAB, new PacketViewerCallback(DisplayStringQuery));
            PacketHandler.RegisterServerToClientViewer(0xAF, new PacketViewerCallback(DeathAnimation));
            PacketHandler.RegisterServerToClientFilter(0xAE, new PacketFilterCallback(UnicodeSpeech));
            PacketHandler.RegisterServerToClientViewer(0xB0, new PacketViewerCallback(SendGump));
            PacketHandler.RegisterServerToClientViewer(0xB9, new PacketViewerCallback(Features));
            PacketHandler.RegisterServerToClientViewer(0xBC, new PacketViewerCallback(ChangeSeason));
            PacketHandler.RegisterServerToClientFilter(0xBE, new PacketFilterCallback(OnAssistVersion));
            PacketHandler.RegisterServerToClientViewer(0xBF, new PacketViewerCallback(ExtendedPacket));
            PacketHandler.RegisterServerToClientFilter(0xC1, new PacketFilterCallback(OnLocalizedMessage));
            PacketHandler.RegisterServerToClientFilter(0xC2, new PacketFilterCallback(UnicodePromptReceived));
            PacketHandler.RegisterServerToClientFilter(0xC8, new PacketFilterCallback(SetUpdateRange));
            PacketHandler.RegisterServerToClientFilter(0xCC, new PacketFilterCallback(OnLocalizedMessageAffix));
            PacketHandler.RegisterServerToClientViewer(0xD6,
                new PacketViewerCallback(EncodedPacket)); //0xD6 "encoded" packets
            PacketHandler.RegisterServerToClientViewer(0xD8, new PacketViewerCallback(CustomHouseInfo));
            //PacketHandler.RegisterServerToClientFilter( 0xDC, new PacketFilterCallback( ServOPLHash ) );
            PacketHandler.RegisterServerToClientViewer(0xDD, new PacketViewerCallback(CompressedGump));
            PacketHandler.RegisterServerToClientViewer(0xF0,
                new PacketViewerCallback(RunUOProtocolExtention)); // Special RunUO protocol extentions (for KUOC/Razor)

            PacketHandler.RegisterServerToClientViewer(0xF3, new PacketViewerCallback(SAWorldItem));

            PacketHandler.RegisterServerToClientViewer(0x2C, new PacketViewerCallback(ResurrectionGump));

            PacketHandler.RegisterServerToClientViewer(0xDF, new PacketViewerCallback(BuffDebuff));

            PacketHandler.RegisterServerToClientFilter(0x54, new PacketFilterCallback(PlaySoundEffect));
            PacketHandler.RegisterServerToClientFilter(0x6D, new PacketFilterCallback(PlayMusic));
        }

        private static void DisplayStringQuery(PacketReader p, PacketHandlerEventArgs args)
        {
            // See also Packets.cs: StringQueryResponse
            /*if ( MacroManager.AcceptActions )
            {
                 int serial = p.ReadInt32();
                 byte type = p.ReadByte();
                 byte index = p.ReadByte();

                 MacroManager.Action( new WaitForTextEntryAction( serial, type, index ) );
            }*/
        }

        private static void SetUpdateRange(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player != null)
                World.Player.VisRange = p.ReadByte();
        }

        private static void EncodedPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            /*ushort id = p.ReadUInt16();

            switch ( id )
            {
                 case 1: // object property list
                 {
                      Serial s = p.ReadUInt32();

                      if ( s.IsItem )
                      {
                           Item item = World.FindItem( s );
                           if ( item == null )
                                World.AddItem( item=new Item( s ) );

                           item.ReadPropertyList( p );
                           if ( item.ModifiedOPL )
                           {
                                args.Block = true;
                                Client.Instance.SendToClient( item.ObjPropList.BuildPacket() );
                           }
                      }
                      else if ( s.IsMobile )
                      {
                           Mobile m = World.FindMobile( s );
                           if ( m == null )
                                World.AddMobile( m=new Mobile( s ) );

                           m.ReadPropertyList( p );
                           if ( m.ModifiedOPL )
                           {
                                args.Block = true;
                                Client.Instance.SendToClient( m.ObjPropList.BuildPacket() );
                           }
                      }
                      break;
                 }
            }*/
        }

        private static void ServOPLHash(Packet p, PacketHandlerEventArgs args)
        {
            /*Serial s = p.ReadUInt32();
            int hash = p.ReadInt32();

            if ( s.IsItem )
            {
                 Item item = World.FindItem( s );
                 if ( item != null && item.OPLHash != hash )
                 {
                      item.OPLHash = hash;
                      p.Seek( -4, SeekOrigin.Current );
                      p.Write( (uint)item.OPLHash );
                 }
            }
            else if ( s.IsMobile )
            {
                 Mobile m = World.FindMobile( s );
                 if ( m != null && m.OPLHash != hash )
                 {
                      m.OPLHash = hash;
                      p.Seek( -4, SeekOrigin.Current );
                      p.Write( (uint)m.OPLHash );
                 }
            }*/
        }

        private static void ClientSingleClick(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial ser = p.ReadUInt32();

            Mobile m = World.FindMobile(ser);

            if (m == null)
                return;

            // if you modify this, don't forget to modify the allnames hotkey
            if (Config.GetBool("LastTargTextFlags"))
                Targeting.CheckTextFlags(m);

            if (Config.GetBool("ShowFriendOverhead") && FriendsManager.IsFriend(m.Serial))
            {
                m.OverheadMessage(Config.GetInt("FriendOverheadFormatHue"),
                    $"{Config.GetString("FriendOverheadFormat")}");
            }
        }

        private static void ClientDoubleClick(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial ser = p.ReadUInt32();
            if (Config.GetBool("BlockDismount") && World.Player != null && ser == World.Player.Serial &&
                World.Player.Warmode && World.Player.GetItemOnLayer(Layer.Mount) != null)
            {
                // mount layer = 0x19
                World.Player.SendMessage(LocString.DismountBlocked);
                args.Block = true;
                return;
            }

            if (Config.GetBool("QueueActions"))
                args.Block = !PlayerData.DoubleClick(ser, false);

            if (ser.IsItem && World.Player != null)
                World.Player.LastObject = ser;

            if (Macros.MacroManager.AcceptActions || ScriptManager.Recording)
            {
                ushort gfx = 0;
                if (ser.IsItem)
                {
                    Item i = World.FindItem(ser);
                    if (i != null)
                        gfx = i.ItemID;
                }
                else
                {
                    Mobile m = World.FindMobile(ser);
                    if (m != null)
                        gfx = m.Body;
                }

                if (gfx != 0)
                {
                    MacroManager.Action(new DoubleClickAction(ser, gfx));

                    ScriptManager.AddToScript($"dclick {ser}");
                }
            }
        }

        private static void DeathAnimation(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial killed = p.ReadUInt32();
            Mobile m = World.FindMobile(killed);

            if (Config.GetBool("CaptureOthersDeath"))
            {
                if (m != null && ((m.Body >= 0x0190 && m.Body <= 0x0193) || (m.Body >= 0x025D && m.Body <= 0x0260)) &&
                    Utility.Distance(World.Player.Position, m.Position) <= 12)
                {
                    if (!string.IsNullOrEmpty(m.Name))
                        ScreenCapManager.LastMobileDeathName = m.Name;

                    ScreenCapManager.DeathCapture(Config.GetDouble("CaptureOthersDeathDelay"));
                }
            }
        }

        private static void ExtendedClientCommand(Packet p, PacketHandlerEventArgs args)
        {
            ushort ext = p.ReadUInt16();
            switch (ext)
            {
                case 0x10: // query object properties
                {
                    break;
                }

                case 0x15: // context menu response
                {
                    UOEntity ent = null;
                    Serial ser = p.ReadUInt32();
                    ushort idx = p.ReadUInt16();

                    if (ser.IsMobile)
                        ent = World.FindMobile(ser);
                    else if (ser.IsItem)
                        ent = World.FindItem(ser);

                    if (ent != null && ent.ContextMenu != null)
                    {
                        if (MacroManager.AcceptActions)
                        {
                            try
                            {
                                MacroManager.Action(new ContextMenuAction(ent, idx, ent.ContextMenu[idx].Value));
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        if (ScriptManager.Recording)
                        {
                            try
                            {
                                ScriptManager.AddToScript(ent.Serial == World.Player.Serial
                                    ? $"menu 'self' {idx}"
                                    : $"menu {ent.Serial} {idx}");
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }

                    break;
                }

                case 0x1C: // cast spell
                {
                    Serial ser = Serial.MinusOne;
                    if (p.ReadUInt16() == 1)
                        ser = p.ReadUInt32();
                    ushort sid = p.ReadUInt16();
                    Spell s = Spell.Get(sid);
                    if (s != null)
                    {
                        s.OnCast(p);
                        args.Block = true;

                        if (Macros.MacroManager.AcceptActions)
                            MacroManager.Action(new ExtCastSpellAction(s, ser));

                        ScriptManager.AddToScript($"cast '{Spell.GetName(sid - 1)}'");
                    }

                    break;
                }

                case 0x24:
                {
                    // for the cheatx0r part 2...  anything outside this range indicates some haxing, just hide it with 0x30s
                    byte b = p.ReadByte();
                    if (b < 0x25 || b >= 0x5E + 0x25)
                    {
                        p.Seek(-1, SeekOrigin.Current);
                        p.Write((byte) 0x30);
                    }

                    //using ( StreamWriter w = new StreamWriter( "bf24.txt", true ) )
                    //	w.WriteLine( "{0} : 0x{1:X2}", Engine.MistedDateTime.ToString( "HH:mm:ss.ffff" ), b );
                    break;
                }
            }
        }

        private static void ClientTextCommand(PacketReader p, PacketHandlerEventArgs args)
        {
            int type = p.ReadByte();
            string command = p.ReadString();

            switch (type)
            {
                case 0x24: // Use skill
                {
                    int skillIndex;

                    try
                    {
                        skillIndex = Convert.ToInt32(command.Split(' ')[0]);
                    }
                    catch
                    {
                        break;
                    }

                    if (World.Player != null)
                        World.Player.LastSkill = skillIndex;

                    if (Macros.MacroManager.AcceptActions)
                        MacroManager.Action(new UseSkillAction(skillIndex));

                    ScriptManager.AddToScript($"useskill '{Ultima.Skills.GetSkill(skillIndex).Name}'");

                    if (skillIndex == (int) SkillName.Stealth && !World.Player.Visible)
                        StealthSteps.Hide();

                    SkillTimer.Start();
                    break;
                }

                case 0x27: // Cast spell from book
                {
                    try
                    {
                        string[] split = command.Split(' ');

                        if (split.Length > 0)
                        {
                            ushort spellID = Convert.ToUInt16(split[0]);
                            Serial serial = Convert.ToUInt32(split.Length > 1 ? Utility.ToInt32(split[1], -1) : -1);
                            Spell s = Spell.Get(spellID);
                            if (s != null)
                            {
                                s.OnCast(p);
                                args.Block = true;
                                if (Macros.MacroManager.AcceptActions)
                                    MacroManager.Action(new BookCastSpellAction(s, serial));

                                ScriptManager.AddToScript($"cast '{Spell.GetName(spellID - 1)}'");
                            }
                        }
                    }
                    catch
                    {
                    }

                    break;
                }

                case 0x56: // Cast spell from macro
                {
                    try
                    {
                        ushort spellID = Convert.ToUInt16(command);
                        Spell s = Spell.Get(spellID);
                        if (s != null)
                        {
                            s.OnCast(p);
                            args.Block = true;
                            if (Macros.MacroManager.AcceptActions)
                                MacroManager.Action(new MacroCastSpellAction(s));

                            ScriptManager.AddToScript($"cast '{Spell.GetName(spellID - 1)}'");
                        }
                    }
                    catch
                    {
                    }

                    break;
                }
            }
        }

        public static DateTime PlayCharTime = DateTime.MinValue;

        private static void CreateCharacter(PacketReader p, PacketHandlerEventArgs args)
        {
            p.Seek(1 + 4 + 4 + 1, SeekOrigin.Begin); // skip begining crap
            World.OrigPlayerName = p.ReadStringSafe(30);

            PlayCharTime = DateTime.UtcNow;

            if (Engine.MainWindow != null)
                Engine.MainWindow.SafeAction(s => s.UpdateControlLocks());
        }

        private static void PlayCharacter(PacketReader p, PacketHandlerEventArgs args)
        {
            p.ReadUInt32(); //0xedededed
            World.OrigPlayerName = p.ReadStringSafe(30);

            PlayCharTime = DateTime.UtcNow;

            if (Engine.MainWindow != null)
                Engine.MainWindow.SafeAction(s => s.UpdateControlLocks());

            //Client.TranslateLogin( World.OrigPlayerName, World.ShardName );
        }

        private static void ServerList(PacketReader p, PacketHandlerEventArgs args)
        {
            p.ReadByte(); //unknown
            ushort numServers = p.ReadUInt16();

            for (int i = 0; i < numServers; ++i)
            {
                ushort num = p.ReadUInt16();
                World.Servers[num] = p.ReadString(32);
                p.ReadByte(); // full %
                p.ReadSByte(); // time zone
                p.ReadUInt32(); // ip
            }
        }

        private static void PlayServer(PacketReader p, PacketHandlerEventArgs args)
        {
            ushort index = p.ReadUInt16();
            string name;
            if (World.Servers.TryGetValue(index, out name) && !string.IsNullOrEmpty(name))
                World.ShardName = name;
            else
                World.ShardName = "[Unknown]";
        }

        private static void LiftRequest(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();
            ushort amount = p.ReadUInt16();

            Item item = World.FindItem(serial);
            ushort iid = 0;

            if (item != null)
                iid = item.ItemID.Value;

            if (Config.GetBool("QueueActions"))
            {
                if (item == null)
                {
                    World.AddItem(item = new Item(serial));
                    item.Amount = amount;
                }

                DragDropManager.Drag(item, amount, true);
                //Client.Instance.SendToClient( new RemoveObject( serial ) ); // remove the object from the client view
                args.Block = true;
            }

            if (Macros.MacroManager.AcceptActions)
            {
                MacroManager.Action(new LiftAction(serial, amount, iid));
                //MacroManager.Action( new PauseAction( TimeSpan.FromMilliseconds( Config.GetInt( "ObjectDelay" ) ) ) );
            }

            ScriptManager.AddToScript($"lift {serial} {amount}");
        }

        private static void LiftReject(PacketReader p, PacketHandlerEventArgs args)
        {
            /*
            if ( ActionQueue.FilterLiftReject() )
                 args.Block = true;
            */
            int reason = p.ReadByte();

            if (!DragDropManager.LiftReject())
                args.Block = true;
            //MacroManager.PlayError( MacroError.LiftRej );
        }

        private static void EquipRequest(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial iser = p.ReadUInt32(); // item being dropped serial
            Layer layer = (Layer) p.ReadByte();
            Serial mser = p.ReadUInt32();

            Item item = World.FindItem(iser);

            if (MacroManager.AcceptActions || ScriptManager.Recording)
            {
                if (layer == Layer.Invalid || layer > Layer.LastValid)
                {
                    if (item != null)
                    {
                        layer = item.Layer;
                        if (layer == Layer.Invalid || layer > Layer.LastValid)
                            layer = (Layer) item.ItemID.ItemData.Quality;
                    }
                }

                if (layer > Layer.Invalid && layer <= Layer.LastUserValid)
                {
                    if (MacroManager.AcceptActions)
                        MacroManager.Action(new DropAction(mser, Point3D.Zero, layer));

                    ScriptManager.AddToScript($"drop {mser} {layer}");
                }
            }

            if (item == null)
                return;

            Mobile m = World.FindMobile(mser);
            if (m == null)
                return;

            if (Config.GetBool("QueueActions"))
                args.Block = DragDropManager.Drop(item, m, layer);
        }

        private static void DropRequest(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial iser = p.ReadUInt32();
            int x = p.ReadInt16();
            int y = p.ReadInt16();
            int z = p.ReadSByte();
            if (Engine.UsePostKRPackets)
                /* grid num */
                p.ReadByte();
            Point3D newPos = new Point3D(x, y, z);
            Serial dser = p.ReadUInt32();

            if (Macros.MacroManager.AcceptActions)
                MacroManager.Action(new DropAction(dser, newPos));

            ScriptManager.AddToScript($"drop {dser} {newPos.X} {newPos.Y} {newPos.Z}");

            Item i = World.FindItem(iser);
            if (i == null)
                return;

            Item dest = World.FindItem(dser);
            if (dest != null && dest.IsContainer && World.Player != null &&
                (dest.IsChildOf(World.Player.Backpack) || dest.IsChildOf(World.Player.Quiver)))
                i.IsNew = true;

            if (Config.GetBool("QueueActions"))
                args.Block = DragDropManager.Drop(i, dser, newPos);
        }

        private static void MovementRequest(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player != null)
            {
                Direction dir = (Direction) p.ReadByte();
                byte seq = p.ReadByte();

                World.Player.Direction = (dir & Direction.Mask);

                WalkAction.LastWalkTime = DateTime.UtcNow;
                if (MacroManager.AcceptActions)
                    MacroManager.Action(new WalkAction(dir));

                ScriptManager.AddToScript($"walk '{dir}'");
            }
        }

        private static void ContainerContentUpdate(Packet p, PacketHandlerEventArgs args)
        {
            // This function will ignore the item if the container item has not been sent to the client yet.
            // We can do this because we can't really count on getting all of the container info anyway.
            // (So we'd need to request the container be updated, so why bother with the extra stuff required to find the container once its been sent?)
            Serial serial = p.ReadUInt32();
            ushort itemid = p.ReadUInt16();
            itemid = (ushort) (itemid + p.ReadSByte()); // signed, itemID offset
            ushort amount = p.ReadUInt16();
            if (amount == 0)
                amount = 1;
            Point3D pos = new Point3D(p.ReadUInt16(), p.ReadUInt16(), 0);
            byte gridPos = 0;
            if (Engine.UsePostKRPackets)
                gridPos = p.ReadByte();
            Serial cser = p.ReadUInt32();
            ushort hue = p.ReadUInt16();

            Item i = World.FindItem(serial);
            if (i == null)
            {
                if (!serial.IsItem)
                    return;

                World.AddItem(i = new Item(serial));
                i.IsNew = i.AutoStack = true;
            }
            else
            {
                i.CancelRemove();
            }

            if (serial != DragDropManager.Pending)
            {
                if (!DragDropManager.EndHolding(serial))
                    return;
            }

            i.ItemID = itemid;
            i.Amount = amount;
            i.Position = pos;
            i.GridNum = gridPos;
            i.Hue = hue;

            if (SearchExemptionAgent.Contains(i))
            {
                p.Seek(-2, System.IO.SeekOrigin.Current);
                p.Write((short) Config.GetInt("ExemptColor"));
            }

            i.Container = cser;
            if (i.IsNew)
                Item.UpdateContainers();
            if (!SearchExemptionAgent.IsExempt(i) &&
                (i.IsChildOf(World.Player.Backpack) || i.IsChildOf(World.Player.Quiver)))
                Counter.Count(i);
        }

        private static void BeginContainerContent(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial ser = p.ReadUInt32();
            if (!ser.IsItem)
                return;
            Item item = World.FindItem(ser);
            if (item != null)
            {
                if (m_IgnoreGumps.Contains(item))
                {
                    m_IgnoreGumps.Remove(item);
                    args.Block = true;
                }
            }
            else
            {
                World.AddItem(new Item(ser));
                Item.UpdateContainers();
            }
        }

        private static void ContainerContent(Packet p, PacketHandlerEventArgs args)
        {
            int count = p.ReadUInt16();

            for (int i = 0; i < count; i++)
            {
                Serial serial = p.ReadUInt32();
                // serial is purposely not checked to be valid, sometimes buy lists dont have "valid" item serials (and we are okay with that).
                Item item = World.FindItem(serial);
                if (item == null)
                {
                    World.AddItem(item = new Item(serial));
                    item.IsNew = true;
                    item.AutoStack = false;
                }
                else
                {
                    item.CancelRemove();
                }

                if (!DragDropManager.EndHolding(serial))
                    continue;

                item.ItemID = p.ReadUInt16();
                item.ItemID = (ushort) (item.ItemID + p.ReadSByte()); // signed, itemID offset
                item.Amount = p.ReadUInt16();
                if (item.Amount == 0)
                    item.Amount = 1;
                item.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), 0);
                if (Engine.UsePostKRPackets)
                    item.GridNum = p.ReadByte();
                Serial cont = p.ReadUInt32();
                item.Hue = p.ReadUInt16();
                if (SearchExemptionAgent.Contains(item))
                {
                    p.Seek(-2, System.IO.SeekOrigin.Current);
                    p.Write((short) Config.GetInt("ExemptColor"));
                }

                item.Container = cont; // must be done after hue is set (for counters)
                if (!SearchExemptionAgent.IsExempt(item) &&
                    (item.IsChildOf(World.Player.Backpack) || item.IsChildOf(World.Player.Quiver)))
                    Counter.Count(item);
            }

            Item.UpdateContainers();
        }

        private static void EquipmentUpdate(Packet p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();

            Item i = World.FindItem(serial);
            bool isNew = false;
            if (i == null)
            {
                World.AddItem(i = new Item(serial));
                isNew = true;
                Item.UpdateContainers();
            }
            else
            {
                i.CancelRemove();
            }

            if (!DragDropManager.EndHolding(serial))
                return;

            ushort iid = p.ReadUInt16();
            i.ItemID = (ushort) (iid + p.ReadSByte()); // signed, itemID offset
            i.Layer = (Layer) p.ReadByte();
            Serial ser = p.ReadUInt32(); // cont must be set after hue (for counters)
            i.Hue = p.ReadUInt16();

            i.Container = ser;

            int ltHue = Config.GetInt("LTHilight");
            if (ltHue != 0 && Targeting.IsLastTarget(i.Container as Mobile))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((ushort) (ltHue & 0x3FFF));
            }

            if (i.Layer == Layer.Backpack && isNew && Config.GetBool("AutoSearch") && ser == World.Player.Serial)
            {
                m_IgnoreGumps.Add(i);
                PlayerData.DoubleClick(i);
            }
        }

        private static void SetSkillLock(PacketReader p, PacketHandlerEventArgs args)
        {
            int i = p.ReadUInt16();

            if (i >= 0 && i < Skill.Count)
            {
                Skill skill = World.Player.Skills[i];

                skill.Lock = (LockType) p.ReadByte();
                Engine.MainWindow.SafeAction(s => s.UpdateSkill(skill));
            }
        }

        private static void Skills(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player == null || World.Player.Skills == null || Engine.MainWindow == null)
                return;
            byte type = p.ReadByte();

            switch (type)
            {
                case 0x02: //list (with caps, 3.0.8 and up)
                {
                    int i;
                    while ((i = p.ReadUInt16()) > 0)
                    {
                        if (i > 0 && i <= Skill.Count)
                        {
                            Skill skill = World.Player.Skills[i - 1];

                            if (skill == null)
                                continue;

                            skill.FixedValue = p.ReadUInt16();
                            skill.FixedBase = p.ReadUInt16();
                            skill.Lock = (LockType) p.ReadByte();
                            skill.FixedCap = p.ReadUInt16();
                            if (!World.Player.SkillsSent)
                                skill.Delta = 0;

                            UOAssist.PostSkillUpdate(i - 1, skill.FixedBase);
                        }
                        else
                        {
                            p.Seek(7, SeekOrigin.Current);
                        }
                    }

                    World.Player.SkillsSent = true;
                    Engine.MainWindow.SafeAction(s => s.RedrawSkills());
                    break;
                }

                case 0x00: // list (without caps, older clients)
                {
                    int i;
                    while ((i = p.ReadUInt16()) > 0)
                    {
                        if (i > 0 && i <= Skill.Count)
                        {
                            Skill skill = World.Player.Skills[i - 1];

                            if (skill == null)
                                continue;

                            skill.FixedValue = p.ReadUInt16();
                            skill.FixedBase = p.ReadUInt16();
                            skill.Lock = (LockType) p.ReadByte();
                            skill.FixedCap = 100; //p.ReadUInt16();
                            if (!World.Player.SkillsSent)
                                skill.Delta = 0;

                            UOAssist.PostSkillUpdate(i - 1, skill.FixedBase);
                        }
                        else
                        {
                            p.Seek(5, SeekOrigin.Current);
                        }
                    }

                    World.Player.SkillsSent = true;
                    Engine.MainWindow.SafeAction(s => s.RedrawSkills());
                    break;
                }

                case 0xDF: //change (with cap, new clients)
                {
                    int i = p.ReadUInt16();

                    if (i >= 0 && i < Skill.Count)
                    {
                        Skill skill = World.Player.Skills[i];

                        if (skill == null)
                            break;

                        ushort old = skill.FixedBase;
                        skill.FixedValue = p.ReadUInt16();
                        skill.FixedBase = p.ReadUInt16();
                        skill.Lock = (LockType) p.ReadByte();
                        skill.FixedCap = p.ReadUInt16();
                        Engine.MainWindow.SafeAction(s => s.UpdateSkill(skill));

                        //Your skill in {0} has changed by {4}{5:F1}, it is now {3:F1} ({1}{2:F1}).
                        if (Config.GetBool("DisplaySkillChanges") && skill.FixedBase != old)
                        {
                            if (Config.GetBool("DisplaySkillChangesOverhead"))
                            {
                                World.Player.OverheadMessage(LocString.SkillChangeOverhead, (SkillName) i,
                                    skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value,
                                    skill.FixedBase - old > 0 ? "+" : "",
                                    (skill.FixedBase - old) / 10.0);
                            }
                            else
                            {
                                //                                                                    0                    1                    2            3                        4                                        5
                                World.Player.SendMessage(MsgLevel.Force, LocString.SkillChanged, (SkillName) i,
                                    skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value,
                                    skill.FixedBase - old > 0 ? "+" : "", ((double) (skill.FixedBase - old)) / 10.0);
                            }
                        }

                        UOAssist.PostSkillUpdate(i, skill.FixedBase);
                    }

                    break;
                }

                case 0xFF: //change (without cap, older clients)
                {
                    int i = p.ReadUInt16();

                    if (i >= 0 && i < Skill.Count)
                    {
                        Skill skill = World.Player.Skills[i];

                        if (skill == null)
                            break;

                        ushort old = skill.FixedBase;
                        skill.FixedValue = p.ReadUInt16();
                        skill.FixedBase = p.ReadUInt16();
                        skill.Lock = (LockType) p.ReadByte();
                        skill.FixedCap = 100;
                        Engine.MainWindow.SafeAction(s => s.UpdateSkill(skill));
                        if (Config.GetBool("DisplaySkillChanges") && skill.FixedBase != old)
                        {
                            if (Config.GetBool("DisplaySkillChangesOverhead"))
                            {
                                World.Player.OverheadMessage(LocString.SkillChangeOverhead, (SkillName) i,
                                    skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value,
                                    skill.FixedBase - old > 0 ? "+" : "",
                                    (skill.FixedBase - old) / 10.0);
                            }
                            else
                            {
                                World.Player.SendMessage(MsgLevel.Force, LocString.SkillChanged, (SkillName) i,
                                    skill.Delta > 0 ? "+" : "", skill.Delta, skill.Value,
                                    ((double) (skill.FixedBase - old)) / 10.0, skill.FixedBase - old > 0 ? "+" : "");
                            }
                        }

                        UOAssist.PostSkillUpdate(i, skill.FixedBase);
                    }

                    break;
                }
            }
        }

        private static void LoginConfirm(PacketReader p, PacketHandlerEventArgs args)
        {
            World.Items.Clear();
            World.Mobiles.Clear();

            UseNewStatus = false;

            Serial serial = p.ReadUInt32();

            PlayerData m = new PlayerData(serial);
            m.Name = World.OrigPlayerName;

            Mobile test = World.FindMobile(serial);
            if (test != null)
                test.Remove();

            World.AddMobile(World.Player = m);
            Config.LoadProfileFor(World.Player);

            p.ReadUInt32(); // always 0?
            m.Body = p.ReadUInt16();
            m.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16());
            m.Direction = (Direction) p.ReadByte();

            Client.Instance.RequestTitlebarUpdate();
            UOAssist.PostLogin((int) serial.Value);
            Engine.MainWindow.SafeAction(s => s.UpdateTitle()); // update player name & shard name

            Client.Instance.SetPosition((uint) m.Position.X, (uint) m.Position.Y, (uint) m.Position.Z,
                (byte) m.Direction);

            if (World.Player != null)
                World.Player.SetSeason();
        }

        private static void CompleteLogin(PacketReader p, PacketHandlerEventArgs args)
        {
            // Set the default min light level if they have limits set
            if (World.Player != null)
                EnforceLightLevels(-1);
        }

        private static void MobileMoving(Packet p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();
            Mobile m = World.FindMobile(serial);

            if (m == null)
            {
                World.AddMobile(m = new Mobile(serial));
                World.RequestMobileStatus(m);
            }

            if (m != null)
            {
                m.Body = p.ReadUInt16();

                MobileFilter.ApplyDragonFilter(p, m);
                MobileFilter.ApplyDrakeFilter(p, m);
                MobileFilter.ApplyDaemonFilter(p, m);

                m.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte());

                if (World.Player != null && !Utility.InRange(World.Player.Position, m.Position, World.Player.VisRange))
                {
                    m.Remove();
                    return;
                }

                Targeting.CheckLastTargetRange(m);

                m.Direction = (Direction) p.ReadByte();
                m.Hue = p.ReadUInt16();
                int ltHue = Config.GetInt("LTHilight");
                if (ltHue != 0 && Targeting.IsLastTarget(m))
                {
                    p.Seek(-2, SeekOrigin.Current);
                    p.Write((short) (ltHue | 0x8000));
                }

                bool wasPoisoned = m.Poisoned;
                m.ProcessPacketFlags(p.ReadByte());
                byte oldNoto = m.Notoriety;
                m.Notoriety = p.ReadByte();

                if (m == World.Player)
                {
                    Client.Instance.SetPosition((uint) m.Position.X, (uint) m.Position.Y, (uint) m.Position.Z,
                        (byte) m.Direction);

                    if (wasPoisoned != m.Poisoned || (oldNoto != m.Notoriety && Config.GetBool("ShowNotoHue")))
                        Client.Instance.RequestTitlebarUpdate();
                }
            }
        }

        private static readonly int[] HealthHues = new int[] {428, 333, 37, 44, 49, 53, 158, 263, 368, 473, 578};

        private static void HitsUpdate(PacketReader p, PacketHandlerEventArgs args)
        {
            Mobile m = World.FindMobile(p.ReadUInt32());

            if (m != null)
            {
                int oldPercent = (int) (m.Hits * 100 / (m.HitsMax == 0 ? (ushort) 1 : m.HitsMax));

                m.HitsMax = p.ReadUInt16();
                m.Hits = p.ReadUInt16();

                if (m == World.Player)
                {
                    Client.Instance.RequestTitlebarUpdate();
                    UOAssist.PostHitsUpdate();
                }

                if (Client.Instance.AllowBit(FeatureBit.OverheadHealth) && Config.GetBool("ShowHealth"))
                {
                    int percent = (int) (m.Hits * 100 / (m.HitsMax == 0 ? (ushort) 1 : m.HitsMax));

                    // Limit to people who are on screen and check the previous value so we dont get spammed.
                    if (oldPercent != percent && World.Player != null &&
                        Utility.Distance(World.Player.Position, m.Position) <= 12)
                    {
                        try
                        {
                            m.OverheadMessageFrom(HealthHues[((percent + 5) / 10) % HealthHues.Length],
                                m.Name ?? string.Empty,
                                Config.GetString("HealthFmt"), percent);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private static void StamUpdate(PacketReader p, PacketHandlerEventArgs args)
        {
            Mobile m = World.FindMobile(p.ReadUInt32());

            if (m != null)
            {
                int oldPercent = (int) (m.Stam * 100 / (m.StamMax == 0 ? (ushort) 1 : m.StamMax));

                m.StamMax = p.ReadUInt16();
                m.Stam = p.ReadUInt16();

                if (m == World.Player)
                {
                    Client.Instance.RequestTitlebarUpdate();
                    UOAssist.PostStamUpdate();
                }

                if (m != World.Player && Client.Instance.AllowBit(FeatureBit.OverheadHealth) &&
                    Config.GetBool("ShowPartyStats"))
                {
                    int stamPercent = (int) (m.Stam * 100 / (m.StamMax == 0 ? (ushort) 1 : m.StamMax));
                    int manaPercent = (int) (m.Mana * 100 / (m.ManaMax == 0 ? (ushort) 1 : m.ManaMax));

                    // Limit to people who are on screen and check the previous value so we dont get spammed.
                    if (oldPercent != stamPercent && World.Player != null &&
                        Utility.Distance(World.Player.Position, m.Position) <= 12)
                    {
                        try
                        {
                            m.OverheadMessageFrom(0x63,
                                m.Name ?? string.Empty,
                                Config.GetString("PartyStatFmt"), manaPercent, stamPercent);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private static void ManaUpdate(PacketReader p, PacketHandlerEventArgs args)
        {
            Mobile m = World.FindMobile(p.ReadUInt32());

            if (m != null)
            {
                int oldPercent = (int) (m.Mana * 100 / (m.ManaMax == 0 ? (ushort) 1 : m.ManaMax));

                m.ManaMax = p.ReadUInt16();
                m.Mana = p.ReadUInt16();

                if (m == World.Player)
                {
                    Client.Instance.RequestTitlebarUpdate();
                    UOAssist.PostManaUpdate();
                }

                if (m != World.Player && Client.Instance.AllowBit(FeatureBit.OverheadHealth) &&
                    Config.GetBool("ShowPartyStats"))
                {
                    int stamPercent = (int) (m.Stam * 100 / (m.StamMax == 0 ? (ushort) 1 : m.StamMax));
                    int manaPercent = (int) (m.Mana * 100 / (m.ManaMax == 0 ? (ushort) 1 : m.ManaMax));

                    // Limit to people who are on screen and check the previous value so we dont get spammed.
                    if (oldPercent != manaPercent && World.Player != null &&
                        Utility.Distance(World.Player.Position, m.Position) <= 12)
                    {
                        try
                        {
                            m.OverheadMessageFrom(0x63,
                                Language.Format(LocString.sStatsA1, m.Name),
                                Config.GetString("PartyStatFmt"), manaPercent, stamPercent);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private static void MobileStatInfo(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            Mobile m = World.FindMobile(pvSrc.ReadUInt32());
            if (m == null)
                return;
            PlayerData p = World.Player;

            m.HitsMax = pvSrc.ReadUInt16();
            m.Hits = pvSrc.ReadUInt16();

            m.ManaMax = pvSrc.ReadUInt16();
            m.Mana = pvSrc.ReadUInt16();

            m.StamMax = pvSrc.ReadUInt16();
            m.Stam = pvSrc.ReadUInt16();

            if (m == World.Player)
            {
                Client.Instance.RequestTitlebarUpdate();
                UOAssist.PostHitsUpdate();
                UOAssist.PostStamUpdate();
                UOAssist.PostManaUpdate();
            }
        }

        public static bool UseNewStatus = false;

        private static void NewMobileStatus(PacketReader p, PacketHandlerEventArgs args)
        {
            Mobile m = World.FindMobile((Serial) p.ReadUInt32());

            if (m == null)
                return;

            UseNewStatus = true;

            // 00 01
            p.ReadUInt16();

            // 00 01 Poison
            // 00 02 Yellow Health Bar

            ushort id = p.ReadUInt16();

            // 00 Off
            // 01 On
            // For Poison: Poison Level + 1

            byte flag = p.ReadByte();

            if (id == 1)
            {
                bool wasPoisoned = m.Poisoned;
                m.Poisoned = (flag != 0);

                if (m == World.Player && wasPoisoned != m.Poisoned)
                    Client.Instance.RequestTitlebarUpdate();
            }
        }

        private static void Damage(PacketReader p, PacketHandlerEventArgs args)
        {
            uint serial = p.ReadUInt32();
            ushort damage = p.ReadUInt16();

            if (Config.GetBool("ShowDamageTaken") || Config.GetBool("ShowDamageDealt"))
            {
                Mobile m = World.FindMobile(serial);

                if (m == null)
                    return;

                if (serial == World.Player.Serial && Config.GetBool("ShowDamageTaken"))
                {
                    if (Config.GetBool("ShowDamageTakenOverhead"))
                    {
                        World.Player.OverheadMessage(38, $"[{damage}]", true);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, $"{damage} dmg->{m.Name}");
                    }
                }

                if (Config.GetBool("ShowDamageDealt"))
                {
                    if (Config.GetBool("ShowDamageDealtOverhead"))
                    {
                        m.OverheadMessageFrom(38, m.Name, $"[{damage}]", true);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, $"{World.Player.Name}->{m.Name}: {damage} damage");
                    }
                }
            }

            if (DamageTracker.Running)
                DamageTracker.AddDamage(serial, damage);
        }

        private static void MobileStatus(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();
            Mobile m = World.FindMobile(serial);
            if (m == null)
                World.AddMobile(m = new Mobile(serial));

            m.Name = p.ReadString(30);

            m.Hits = p.ReadUInt16();
            m.HitsMax = p.ReadUInt16();

            //p.ReadBoolean();//CanBeRenamed
            if (p.ReadBoolean())
                m.CanRename = true;

            byte type = p.ReadByte();

            if (m == World.Player && type != 0x00)
            {
                PlayerData player = (PlayerData) m;

                player.Female = p.ReadBoolean();

                int oStr = player.Str, oDex = player.Dex, oInt = player.Int;

                player.Str = p.ReadUInt16();
                player.Dex = p.ReadUInt16();
                player.Int = p.ReadUInt16();

                //Your strength has changed by {0}{1}, it is now {2}.
                if (player.Str != oStr && oStr != 0 && Config.GetBool("DisplaySkillChanges"))
                {
                    if (Config.GetBool("DisplaySkillChangesOverhead"))
                    {
                        World.Player.OverheadMessage(LocString.StrChangeOverhead, player.Str - oStr > 0 ? "+" : "",
                            player.Str - oStr, player.Str);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, LocString.StrChanged, player.Str - oStr > 0 ? "+" : "",
                            player.Str - oStr, player.Str);
                    }
                }

                if (player.Dex != oDex && oDex != 0 && Config.GetBool("DisplaySkillChanges"))
                {
                    if (Config.GetBool("DisplaySkillChangesOverhead"))
                    {
                        World.Player.OverheadMessage(LocString.DexChangeOverhead, player.Dex - oDex > 0 ? "+" : "",
                            player.Dex - oDex, player.Dex);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, LocString.DexChanged, player.Dex - oDex > 0 ? "+" : "",
                            player.Dex - oDex, player.Dex);
                    }
                }

                if (player.Int != oInt && oInt != 0 && Config.GetBool("DisplaySkillChanges"))
                {
                    if (Config.GetBool("DisplaySkillChangesOverhead"))
                    {
                        World.Player.OverheadMessage(LocString.IntChangeOverhead, player.Int - oInt > 0 ? "+" : "",
                            player.Int - oInt, player.Int);
                    }
                    else
                    {
                        World.Player.SendMessage(MsgLevel.Force, LocString.IntChanged, player.Int - oInt > 0 ? "+" : "",
                            player.Int - oInt, player.Int);
                    }
                }

                player.Stam = p.ReadUInt16();
                player.StamMax = p.ReadUInt16();
                player.Mana = p.ReadUInt16();
                player.ManaMax = p.ReadUInt16();

                player.Gold = p.ReadUInt32();
                player.AR = p.ReadUInt16(); // ar / physical resist
                player.Weight = p.ReadUInt16();

                if (type >= 0x03)
                {
                    if (type > 0x04)
                    {
                        player.MaxWeight = p.ReadUInt16();

                        p.ReadByte(); // race?
                    }

                    player.StatCap = p.ReadUInt16();

                    player.Followers = p.ReadByte();
                    player.FollowersMax = p.ReadByte();

                    if (type > 0x03)
                    {
                        player.FireResistance = p.ReadInt16();
                        player.ColdResistance = p.ReadInt16();
                        player.PoisonResistance = p.ReadInt16();
                        player.EnergyResistance = p.ReadInt16();

                        player.Luck = p.ReadInt16();

                        player.DamageMin = p.ReadUInt16();
                        player.DamageMin = p.ReadUInt16();
                        player.DamageMax = p.ReadUInt16();

                        player.Tithe = p.ReadInt32();
                    }
                }

                Client.Instance.RequestTitlebarUpdate();

                UOAssist.PostHitsUpdate();
                UOAssist.PostStamUpdate();
                UOAssist.PostManaUpdate();

                Engine.MainWindow.SafeAction(s => s.UpdateTitle()); // update player name
            }
        }

        private static void MobileUpdate(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            Serial serial = p.ReadUInt32();
            Mobile m = World.FindMobile(serial);
            if (m == null)
                World.AddMobile(m = new Mobile(serial));

            bool wasHidden = !m.Visible;

            m.Body = (ushort) (p.ReadUInt16() + p.ReadSByte());
            m.Hue = p.ReadUInt16();
            int ltHue = Config.GetInt("LTHilight");
            if (ltHue != 0 && Targeting.IsLastTarget(m))
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((ushort) (ltHue | 0x8000));
            }

            bool wasPoisoned = m.Poisoned;
            m.ProcessPacketFlags(p.ReadByte());

            ushort x = p.ReadUInt16();
            ushort y = p.ReadUInt16();
            p.ReadUInt16(); //always 0?
            m.Direction = (Direction) p.ReadByte();
            m.Position = new Point3D(x, y, p.ReadSByte());

            if (m == World.Player)
            {
                Client.Instance.SetPosition((uint) m.Position.X, (uint) m.Position.Y, (uint) m.Position.Z,
                    (byte) m.Direction);

                if (!wasHidden && !m.Visible)
                {
                    if (Config.GetBool("CountStealthSteps"))
                        StealthSteps.Hide();
                }
                else if (wasHidden && m.Visible)
                {
                    StealthSteps.Unhide();
                }

                if (wasPoisoned != m.Poisoned)
                    Client.Instance.RequestTitlebarUpdate();
            }

            Item.UpdateContainers();
        }

        private static void MobileIncoming(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            Serial serial = p.ReadUInt32();
            ushort body = p.ReadUInt16();

            Mobile m = World.FindMobile(serial);

            if (m == null)
                World.AddMobile(m = new Mobile(serial));

            MobileFilter.ApplyDragonFilter(p, m);
            MobileFilter.ApplyDrakeFilter(p, m);
            MobileFilter.ApplyDaemonFilter(p, m);

            Point3D position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadSByte());

            bool wasHidden = !m.Visible;

            if (m != World.Player && Config.GetBool("ShowMobNames"))
                Client.Instance.SendToServer(new SingleClick(m));
            if (Config.GetBool("LastTargTextFlags"))
                Targeting.CheckTextFlags(m);

            if (Config.GetBool("ShowFriendOverhead") && FriendsManager.IsFriend(m.Serial))
            {
                m.OverheadMessage(Config.GetInt("FriendOverheadFormatHue"),
                    $"{Config.GetString("FriendOverheadFormat")}");
            }

            int ltHue = Config.GetInt("LTHilight");
            bool isLT;
            if (ltHue != 0)
                isLT = Targeting.IsLastTarget(m);
            else
                isLT = false;

            m.Body = body;
            if (m != World.Player)
                m.Position = position;
            m.Direction = (Direction) p.ReadByte();
            m.Hue = p.ReadUInt16();
            if (isLT)
            {
                p.Seek(-2, SeekOrigin.Current);
                p.Write((short) (ltHue | 0x8000));
            }

            bool wasPoisoned = m.Poisoned;
            m.ProcessPacketFlags(p.ReadByte());
            byte oldNoto = m.Notoriety;
            m.Notoriety = p.ReadByte();

            if (m == World.Player)
            {
                if (!wasHidden && !m.Visible)
                {
                    if (Config.GetBool("CountStealthSteps"))
                        StealthSteps.Hide();
                }
                else if (wasHidden && m.Visible)
                {
                    StealthSteps.Unhide();
                }

                if (wasPoisoned != m.Poisoned || (oldNoto != m.Notoriety && Config.GetBool("ShowNotoHue")))
                    Client.Instance.RequestTitlebarUpdate();
            }

            while (true)
            {
                serial = p.ReadUInt32();
                if (!serial.IsItem)
                    break;

                Item item = World.FindItem(serial);
                bool isNew = false;
                if (item == null)
                {
                    isNew = true;
                    World.AddItem(item = new Item(serial));
                }

                if (!DragDropManager.EndHolding(serial))
                    continue;

                item.Container = m;

                ushort id = p.ReadUInt16();

                if (Engine.UseNewMobileIncoming)
                    item.ItemID = (ushort) (id & 0xFFFF);
                else if (Engine.UsePostSAChanges)
                    item.ItemID = (ushort) (id & 0x7FFF);
                else
                    item.ItemID = (ushort) (id & 0x3FFF);

                item.Layer = (Layer) p.ReadByte();

                if (Engine.UseNewMobileIncoming)
                {
                    item.Hue = p.ReadUInt16();
                    if (isLT)
                    {
                        p.Seek(-2, SeekOrigin.Current);
                        p.Write((short) (ltHue & 0x3FFF));
                    }
                }
                else
                {
                    if ((id & 0x8000) != 0)
                    {
                        item.Hue = p.ReadUInt16();
                        if (isLT)
                        {
                            p.Seek(-2, SeekOrigin.Current);
                            p.Write((short) (ltHue & 0x3FFF));
                        }
                    }
                    else
                    {
                        item.Hue = 0;
                        if (isLT)
                            Client.Instance.SendToClient(new EquipmentItem(item, (ushort) (ltHue & 0x3FFF), m.Serial));
                    }
                }

                if (item.Layer == Layer.Backpack && isNew && Config.GetBool("AutoSearch") && m == World.Player &&
                    m != null)
                {
                    m_IgnoreGumps.Add(item);
                    PlayerData.DoubleClick(item);
                }
            }

            Item.UpdateContainers();
        }

        private static void RemoveObject(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();

            if (serial.IsMobile)
            {
                Mobile m = World.FindMobile(serial);
                if (m != null && m != World.Player)
                    m.Remove();
            }
            else if (serial.IsItem)
            {
                Item i = World.FindItem(serial);
                if (i != null)
                {
                    if (DragDropManager.Holding == i)
                    {
                        Counter.SupressWarnings = true;
                        i.Container = null;
                        Counter.SupressWarnings = false;
                    }
                    else
                    {
                        i.RemoveRequest();
                    }
                }
            }
        }

        private static void ServerChange(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player != null)
                World.Player.Position = new Point3D(p.ReadUInt16(), p.ReadUInt16(), p.ReadInt16());
        }

        private static void WorldItem(PacketReader p, PacketHandlerEventArgs args)
        {
            Item item;
            uint serial = p.ReadUInt32();
            item = World.FindItem(serial & 0x7FFFFFFF);
            bool isNew = false;
            if (item == null)
            {
                World.AddItem(item = new Item(serial & 0x7FFFFFFF));
                isNew = true;
            }
            else
            {
                item.CancelRemove();
            }

            if (!DragDropManager.EndHolding(serial))
                return;

            item.Container = null;
            Counter.Uncount(item);

            ushort itemID = p.ReadUInt16();
            item.ItemID = (ushort) (itemID & 0x7FFF);

            if ((serial & 0x80000000) != 0)
                item.Amount = p.ReadUInt16();
            else
                item.Amount = 1;

            if ((itemID & 0x8000) != 0)
                item.ItemID = (ushort) (item.ItemID + p.ReadSByte());

            ushort x = p.ReadUInt16();
            ushort y = p.ReadUInt16();

            if ((x & 0x8000) != 0)
                item.Direction = p.ReadByte();
            else
                item.Direction = 0;

            short z = p.ReadSByte();

            item.Position = new Point3D(x & 0x7FFF, y & 0x3FFF, z);

            if ((y & 0x8000) != 0)
                item.Hue = p.ReadUInt16();
            else
                item.Hue = 0;

            byte flags = 0;
            if ((y & 0x4000) != 0)
                flags = p.ReadByte();

            item.ProcessPacketFlags(flags);

            if (isNew && World.Player != null)
            {
                if (item.ItemID == 0x2006) // corpse itemid = 0x2006
                {
                    if (Config.GetBool("ShowCorpseNames"))
                        Client.Instance.SendToServer(new SingleClick(item));

                    if (Config.GetBool("AutoOpenCorpses") &&
                        Utility.InRange(item.Position, World.Player.Position, Config.GetInt("CorpseRange")) &&
                        World.Player != null && World.Player.Visible)
                    {
                        if (Config.GetBool("BlockOpenCorpsesTwice"))
                        {
                            bool blockOpen = false;
                            foreach (uint openedCorse in World.Player.OpenedCorpses)
                            {
                                if (openedCorse == serial)
                                {
                                    blockOpen = true;
                                    break;
                                }
                            }

                            if (World.Player.OpenedCorpses.Count > 2000)
                            {
                                World.Player.OpenedCorpses.RemoveRange(0, 500);
                            }

                            if (!blockOpen)
                            {
                                PlayerData.DoubleClick(item);
                            }

                            if (!World.Player.OpenedCorpses.Contains(serial))
                            {
                                World.Player.OpenedCorpses.Add(serial);
                            }
                        }
                        else
                        {
                            PlayerData.DoubleClick(item);
                        }
                    }
                }
                else if (item.IsMulti)
                {
                    UOAssist.PostAddMulti(item.ItemID, item.Position);
                }
                else
                {
                    ScavengerAgent s = ScavengerAgent.Instance;
                    int dist = Utility.Distance(item.GetWorldPosition(), World.Player.Position);
                    if (!World.Player.IsGhost && World.Player.Visible && dist <= 2 && s.Enabled && item.Movable)
                        s.Scavenge(item);
                }
            }

            Item.UpdateContainers();

            if (Config.GetBool("ShowStaticWalls"))
                args.Block = WallStaticFilter.MakeWallStatic(item);
        }

        private static void SAWorldItem(PacketReader p, PacketHandlerEventArgs args)
        {
            /*
            New World Item Packet
            PacketID: 0xF3
            PacketLen: 24
            Format:

                 BYTE - 0xF3 packetId
                 WORD - 0x01
                 BYTE - ArtDataID: 0x00 if the item uses art from TileData table, 0x02 if the item uses art from MultiData table)
                 DWORD - item Serial
                 WORD - item ID
                 BYTE - item direction (same as old)
                 WORD - amount
                 WORD - amount
                 WORD - X
                 WORD - Y
                 SBYTE - Z
                 BYTE - item light
                 WORD - item Hue
                 BYTE - item flags (same as old packet)
            */

            // Post-7.0.9.0
            /*
            New World Item Packet
            PacketID: 0xF3
            PacketLen: 26
            Format:

                 BYTE - 0xF3 packetId
                 WORD - 0x01
                 BYTE - ArtDataID: 0x00 if the item uses art from TileData table, 0x02 if the item uses art from MultiData table)
                 DWORD - item Serial
                 WORD - item ID
                 BYTE - item direction (same as old)
                 WORD - amount
                 WORD - amount
                 WORD - X
                 WORD - Y
                 SBYTE - Z
                 BYTE - item light
                 WORD - item Hue
                 BYTE - item flags (same as old packet)
                 WORD ???
            */

            ushort _unk1 = p.ReadUInt16();

            byte _artDataID = p.ReadByte();

            Item item;
            uint serial = p.ReadUInt32();
            item = World.FindItem(serial);
            bool isNew = false;
            if (item == null)
            {
                World.AddItem(item = new Item(serial));
                isNew = true;
            }
            else
            {
                item.CancelRemove();
            }

            if (!DragDropManager.EndHolding(serial))
                return;

            item.Container = null;
            Counter.Uncount(item);

            ushort itemID = p.ReadUInt16();
            item.ItemID = (ushort) (_artDataID == 0x02 ? itemID | 0x4000 : itemID);

            item.Direction = p.ReadByte();

            ushort _amount = p.ReadUInt16();
            item.Amount = _amount = p.ReadUInt16();

            ushort x = p.ReadUInt16();
            ushort y = p.ReadUInt16();
            short z = p.ReadSByte();

            item.Position = new Point3D(x, y, z);

            byte _light = p.ReadByte();

            item.Hue = p.ReadUInt16();

            byte flags = p.ReadByte();

            item.ProcessPacketFlags(flags);

            if (Engine.UsePostHSChanges)
            {
                p.ReadUInt16();
            }

            if (isNew && World.Player != null)
            {
                if (item.ItemID == 0x2006) // corpse itemid = 0x2006
                {
                    if (Config.GetBool("ShowCorpseNames"))
                        Client.Instance.SendToServer(new SingleClick(item));
                    if (Config.GetBool("AutoOpenCorpses") &&
                        Utility.InRange(item.Position, World.Player.Position, Config.GetInt("CorpseRange")) &&
                        World.Player != null && World.Player.Visible)
                        PlayerData.DoubleClick(item);
                }
                else if (item.IsMulti)
                {
                    UOAssist.PostAddMulti(item.ItemID, item.Position);
                }
                else
                {
                    ScavengerAgent s = ScavengerAgent.Instance;
                    int dist = Utility.Distance(item.GetWorldPosition(), World.Player.Position);
                    if (!World.Player.IsGhost && World.Player.Visible && dist <= 2 && s.Enabled && item.Movable)
                        s.Scavenge(item);
                }
            }

            Item.UpdateContainers();

            if (Config.GetBool("ShowStaticWalls"))
                args.Block = WallStaticFilter.MakeWallStatic(item);
        }

        public static List<string> SysMessages = new List<string>();

        public static void HandleSpeech(Packet p, PacketHandlerEventArgs args, Serial ser, ushort body,
            MessageType type, ushort hue, ushort font, string lang, string name, string text)
        {
            if (World.Player == null)
                return;

            if (type == MessageType.Spell)
            {
                Spell s = Spell.Get(text.Trim());
                bool replaced = false;
                if (s != null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(Config.GetString("SpellFormat"));
                    sb.Replace(@"{power}", s.WordsOfPower);
                    string spell = Language.GetString(s.Name);
                    sb.Replace(@"{spell}", spell);
                    sb.Replace(@"{name}", spell);
                    sb.Replace(@"{circle}", s.Circle.ToString());

                    string newText = sb.ToString();

                    if (newText != null && newText != "" && newText != text)
                    {
                        Client.Instance.SendToClient(new AsciiMessage(ser, body, MessageType.Spell, s.GetHue(hue), font,
                            name, newText));
                        //Client.Instance.SendToClient( new UnicodeMessage( ser, body, MessageType.Spell, s.GetHue( hue ), font, Language.CliLocName, name, newText ) );
                        replaced = true;
                        args.Block = true;
                    }
                }

                if (!replaced && Config.GetBool("ForceSpellHue"))
                {
                    p.Seek(10, SeekOrigin.Begin);
                    if (s != null)
                        p.Write((ushort) s.GetHue(hue));
                    else
                        p.Write((ushort) Config.GetInt("NeutralSpellHue"));
                }
            }
            else if (ser.IsMobile && type == MessageType.Label)
            {
                Mobile m = World.FindMobile(ser);
                if (m != null /*&& ( m.Name == null || m.Name == "" || m.Name == "(Not Seen)" )*/ &&
                    m.Name.IndexOf(text) != 5 && m != World.Player && !(text.StartsWith("(") && text.EndsWith(")")))
                    m.Name = text;
            }
            /*else if ( Spell.Get( text.Trim() ) != null )
            { // send fake spells to bottom left
                 p.Seek( 3, SeekOrigin.Begin );
                 p.Write( (uint)0xFFFFFFFF );
            }*/
            else
            {
                if (ser == Serial.MinusOne && name == "System")
                {
                    if (Config.GetBool("FilterSnoopMsg") && text.IndexOf(World.Player.Name) == -1 &&
                        text.StartsWith("You notice") && text.IndexOf("attempting to peek into") != -1 &&
                        text.IndexOf("belongings") != -1)
                    {
                        args.Block = true;
                        return;
                    }

                    if (text.StartsWith("You've committed a criminal act") || text.StartsWith("You are now a criminal"))
                    {
                        World.Player.ResetCriminalTimer();
                    }

                    // Overhead message override
                    OverheadMessages.DisplayOverheadMessage(text);
                }

                if (Config.GetBool("ShowContainerLabels") && ser.IsItem)
                {
                    Item item = World.FindItem(ser);

                    if (item == null || !item.IsContainer)
                        return;

                    foreach (ContainerLabels.ContainerLabel label in ContainerLabels.ContainerLabelList)
                    {
                        // Check if its the serial match and if the text matches the name (since we override that for the label)
                        if (Serial.Parse(label.Id) == ser &&
                            (item.ItemID.ItemData.Name.Equals(text) ||
                             label.Alias.Equals(text, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            string labelDisplay =
                                $"{Config.GetString("ContainerLabelFormat").Replace("{label}", label.Label).Replace("{type}", text)}";

                            //ContainerLabelStyle
                            if (Config.GetInt("ContainerLabelStyle") == 0)
                            {
                                Client.Instance.SendToClient(new AsciiMessage(ser, item.ItemID.Value, MessageType.Label,
                                    label.Hue, 3, Language.CliLocName, labelDisplay));
                            }
                            else
                            {
                                Client.Instance.SendToClient(new UnicodeMessage(ser, item.ItemID.Value,
                                    MessageType.Label, label.Hue, 3, Language.CliLocName, "", labelDisplay));
                            }

                            // block the actual message from coming through since we have it in the label
                            args.Block = true;

                            ContainerLabels.LastContainerLabelDisplayed = ser;

                            break;
                        }
                    }
                }

                if ((type == MessageType.Emote || type == MessageType.Regular || type == MessageType.Whisper ||
                     type == MessageType.Yell) && ser.IsMobile && ser != World.Player.Serial)
                {
                    if (ser.IsMobile && IgnoreAgent.IsIgnored(ser))
                    {
                        args.Block = true;
                        return;
                    }

                    if (Config.GetBool("ForceSpeechHue"))
                    {
                        p.Seek(10, SeekOrigin.Begin);
                        p.Write((ushort) Config.GetInt("SpeechHue"));
                    }
                }

                if (!ser.IsValid || ser == World.Player.Serial || ser.IsItem)
                {
                    SysMessages.Add(text);

                    if (SysMessages.Count >= 25)
                        SysMessages.RemoveRange(0, 10);
                }

                if (Config.GetBool("FilterSystemMessages") && ser == Serial.MinusOne || ser == Serial.Zero)
                {
                    if (!MessageQueue.Enqueue(ser, null, body, type, hue, font, lang, name, text))
                    {
                        args.Block = true;
                    }
                }
            }
        }

        public static void AsciiSpeech(Packet p, PacketHandlerEventArgs args)
        {
            // 0, 1, 2
            Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
            ushort body = p.ReadUInt16(); // 7, 8
            MessageType type = (MessageType) p.ReadByte(); // 9
            ushort hue = p.ReadUInt16(); // 10, 11
            ushort font = p.ReadUInt16();
            string name = p.ReadStringSafe(30);
            string text = p.ReadStringSafe();

            if (World.Player != null && serial == Serial.Zero && body == 0 && type == MessageType.Regular &&
                hue == 0xFFFF && font == 0xFFFF && name == "SYSTEM")
            {
                args.Block = true;

                Client.Instance.SendToServer(new ACKTalk());
            }
            else
            {
                HandleSpeech(p, args, serial, body, type, hue, font, "A", name, text);

                if (!serial.IsValid)
                {
                    BandageTimer.OnAsciiMessage(text);
                }

                GateTimer.OnAsciiMessage(text);
            }
        }

        public static void UnicodeSpeech(Packet p, PacketHandlerEventArgs args)
        {
            // 0, 1, 2
            Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
            ushort body = p.ReadUInt16(); // 7, 8
            MessageType type = (MessageType) p.ReadByte(); // 9
            ushort hue = p.ReadUInt16(); // 10, 11
            ushort font = p.ReadUInt16();
            string lang = p.ReadStringSafe(4);
            string name = p.ReadStringSafe(30);
            string text = p.ReadUnicodeStringSafe();

            HandleSpeech(p, args, serial, body, type, hue, font, lang, name, text);

            if (!serial.IsValid)
            {
                BandageTimer.OnAsciiMessage(text);
            }
        }

        private static void OnLocalizedMessage(Packet p, PacketHandlerEventArgs args)
        {
            // 0, 1, 2
            Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
            ushort body = p.ReadUInt16(); // 7, 8
            MessageType type = (MessageType) p.ReadByte(); // 9
            ushort hue = p.ReadUInt16(); // 10, 11
            ushort font = p.ReadUInt16();
            int num = p.ReadInt32();
            string name = p.ReadStringSafe(30);
            string ext_str = p.ReadUnicodeStringLESafe();

            if ((num >= 3002011 && num < 3002011 + 64) || // reg spells
                (num >= 1060509 && num < 1060509 + 16) || // necro
                (num >= 1060585 && num < 1060585 + 10) || // chiv
                (num >= 1060493 && num < 1060493 + 10) || // chiv
                (num >= 1060595 && num < 1060595 + 6) || // bush
                (num >= 1060610 && num < 1060610 + 8)) // ninj
            {
                type = MessageType.Spell;
            }

            BandageTimer.OnLocalizedMessage(num);

            try
            {
                string text = Language.ClilocFormat(num, ext_str);
                HandleSpeech(p, args, serial, body, type, hue, font, Language.CliLocName.ToUpper(), name, text);
            }
            catch (Exception e)
            {
                Engine.LogCrash(new Exception(String.Format("Exception in Ultima.dll cliloc: {0}, {1}", num, ext_str),
                    e));
            }
        }

        private static void OnLocalizedMessageAffix(Packet p, PacketHandlerEventArgs phea)
        {
            // 0, 1, 2
            Serial serial = p.ReadUInt32(); // 3, 4, 5, 6
            ushort body = p.ReadUInt16(); // 7, 8
            MessageType type = (MessageType) p.ReadByte(); // 9
            ushort hue = p.ReadUInt16(); // 10, 11
            ushort font = p.ReadUInt16();
            int num = p.ReadInt32();
            byte affixType = p.ReadByte();
            string name = p.ReadStringSafe(30);
            string affix = p.ReadStringSafe();
            string args = p.ReadUnicodeStringSafe();

            if ((num >= 3002011 && num < 3002011 + 64) || // reg spells
                (num >= 1060509 && num < 1060509 + 16) || // necro
                (num >= 1060585 && num < 1060585 + 10) || // chiv
                (num >= 1060493 && num < 1060493 + 10) || // chiv
                (num >= 1060595 && num < 1060595 + 6) || // bush
                (num >= 1060610 && num < 1060610 + 8) // ninj
            )
            {
                type = MessageType.Spell;
            }

            string text;
            if ((affixType & 1) != 0) // prepend
                text = String.Format("{0}{1}", affix, Language.ClilocFormat(num, args));
            else // 0 == append, 2 = system
                text = String.Format("{0}{1}", Language.ClilocFormat(num, args), affix);
            HandleSpeech(p, phea, serial, body, type, hue, font, Language.CliLocName.ToUpper(), name, text);
        }

        private static void SendGump(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            World.Player.CurrentGumpS = p.ReadUInt32();
            World.Player.CurrentGumpI = p.ReadUInt32();
            World.Player.HasGump = true;
            //byte[] data = p.CopyBytes( 11, p.Length - 11 );

            if (Macros.MacroManager.AcceptActions &&
                MacroManager.Action(new WaitForGumpAction(World.Player.CurrentGumpI)))
                args.Block = true;

            ScriptManager.AddToScript($"waitforgump {World.Player.CurrentGumpI} 10000");
        }

        private static void ClientGumpResponse(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            Serial ser = p.ReadUInt32();
            uint tid = p.ReadUInt32();
            int bid = p.ReadInt32();

            World.Player.HasGump = false;
            World.Player.HasCompressedGump = false;

            int sc = p.ReadInt32();
            if (sc < 0 || sc > 2000)
                return;
            int[] switches = new int[sc];
            for (int i = 0; i < sc; i++)
                switches[i] = p.ReadInt32();

            int ec = p.ReadInt32();
            if (ec < 0 || ec > 2000)
                return;
            GumpTextEntry[] entries = new GumpTextEntry[ec];
            for (int i = 0; i < ec; i++)
            {
                ushort id = p.ReadUInt16();
                ushort len = p.ReadUInt16();
                if (len >= 240)
                    return;
                string text = p.ReadUnicodeStringSafe(len);
                entries[i] = new GumpTextEntry(id, text);
            }

            if (Macros.MacroManager.AcceptActions)
                MacroManager.Action(new GumpResponseAction(bid, switches, entries));

            ScriptManager.AddToScript(bid == 0 ? "closegump" : $"gumpresponse {bid}");


            World.Player.LastGumpResponseAction = new GumpResponseAction(bid, switches, entries);
        }

        private static void ChangeSeason(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player != null)
            {
                byte season = p.ReadByte();
                World.Player.SetSeason(season);
            }
        }

        private static void OnAssistVersion(Packet p, PacketHandlerEventArgs args)
        {
            args.Block = true;

            Client.Instance.ForceSendToServer(new AssistVersion());
        }

        private static void ExtendedPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            ushort type = p.ReadUInt16();

            switch (type)
            {
                case 0x04: // close gump
                {
                    // int serial, int tid
                    if (World.Player != null)
                    {
                        World.Player.HasGump = false;
                        World.Player.HasCompressedGump = false;
                    }

                    break;
                }

                case 0x06: // party messages
                {
                    OnPartyMessage(p, args);
                    break;
                }

                case 0x08: // map change
                {
                    if (World.Player != null)
                        World.Player.Map = p.ReadByte();
                    break;
                }

                case 0x14: // context menu
                {
                    p.ReadInt16(); // 0x01
                    UOEntity ent = null;
                    Serial ser = p.ReadUInt32();
                    if (ser.IsMobile)
                        ent = World.FindMobile(ser);
                    else if (ser.IsItem)
                        ent = World.FindItem(ser);

                    if (ent != null)
                    {
                        byte count = p.ReadByte();

                        try
                        {
                            ent.ContextMenu.Clear();

                            for (int i = 0; i < count; i++)
                            {
                                ushort idx = p.ReadUInt16();
                                ushort num = p.ReadUInt16();
                                ushort flags = p.ReadUInt16();
                                ushort color = 0;

                                if ((flags & 0x02) != 0)
                                    color = p.ReadUInt16();

                                ent.ContextMenu.Add(idx, num);
                            }
                        }
                        catch
                        {
                        }
                    }

                    break;
                }

                case 0x18: // map patches
                {
                    if (World.Player != null)
                    {
                        int count = p.ReadInt32() * 2;
                        try
                        {
                            World.Player.MapPatches = new int[count];
                            for (int i = 0; i < count; i++)
                                World.Player.MapPatches[i] = p.ReadInt32();
                        }
                        catch
                        {
                        }
                    }

                    break;
                }

                case 0x19: //  stat locks
                {
                    if (p.ReadByte() == 0x02)
                    {
                        Mobile m = World.FindMobile(p.ReadUInt32());
                        if (World.Player == m && m != null)
                        {
                            p.ReadByte(); // 0?

                            byte locks = p.ReadByte();

                            World.Player.StrLock = (LockType) ((locks >> 4) & 3);
                            World.Player.DexLock = (LockType) ((locks >> 2) & 3);
                            World.Player.IntLock = (LockType) (locks & 3);
                        }
                    }

                    break;
                }

                case 0x1D: // Custom House "General Info"
                {
                    Item i = World.FindItem(p.ReadUInt32());
                    if (i != null)
                        i.HouseRevision = p.ReadInt32();
                    break;
                }
            }
        }

        public static int SpecialPartySent = 0;
        public static int SpecialPartyReceived = 0;

        private static void RunUOProtocolExtention(PacketReader p, PacketHandlerEventArgs args)
        {
            // Only block this packet if they are using the OSI client
            // since ClassicUO actually has support for it now
            args.Block = Client.IsOSI;

            switch (p.ReadByte())
            {
                case 1: // Custom Party information
                {
                    Serial serial;

                    PacketHandlers.SpecialPartyReceived++;

                    while ((serial = p.ReadUInt32()) > 0)
                    {
                        Mobile mobile = World.FindMobile(serial);

                        short x = p.ReadInt16();
                        short y = p.ReadInt16();
                        byte map = p.ReadByte();

                        if (mobile == null)
                        {
                            World.AddMobile(mobile = new Mobile(serial));
                            mobile.Visible = false;
                        }

                        if (mobile.Name == null || mobile.Name.Length <= 0)
                            mobile.Name = "(Not Seen)";

                        if (!m_Party.Contains(serial))
                            m_Party.Add(serial);

                        if (map == World.Player.Map)
                            mobile.Position = new Point3D(x, y, mobile.Position.Z);
                        else
                            mobile.Position = Point3D.Zero;
                    }

                    if (Engine.MainWindow.MapWindow != null)
                        Engine.MainWindow.SafeAction(s => s.MapWindow.UpdateMap());

                    break;
                }

                case 0xFE: // Begin Handshake/Features Negotiation
                {
                    ulong features = ((ulong) p.ReadUInt32() << 32) | (ulong) p.ReadUInt32();

                    Client.Instance.SetFeatures(features);
                    Client.Instance.SendToServer(new RazorNegotiateResponse());
                    Engine.MainWindow.SafeAction(s => s.UpdateControlLocks());

                    break;
                }
            }
        }

        private static List<Serial> m_Party = new List<Serial>();

        public static List<Serial> Party
        {
            get { return m_Party; }
        }

        private static Timer m_PartyDeclineTimer = null;
        public static Serial PartyLeader = Serial.Zero;

        private static void OnPartyMessage(PacketReader p, PacketHandlerEventArgs args)
        {
            switch (p.ReadByte())
            {
                case 0x01: // List
                {
                    m_Party.Clear();

                    int count = p.ReadByte();
                    for (int i = 0; i < count; i++)
                    {
                        Serial s = p.ReadUInt32();
                        if (World.Player == null || s != World.Player.Serial)
                            m_Party.Add(s);
                    }

                    break;
                }

                case 0x02: // Remove Member/Re-list
                {
                    m_Party.Clear();
                    int count = p.ReadByte();
                    Serial remSerial = p.ReadUInt32(); // the serial of who was removed

                    if (World.Player != null)
                    {
                        Mobile rem = World.FindMobile(remSerial);
                        if (rem != null && !Utility.InRange(World.Player.Position, rem.Position, World.Player.VisRange))
                            rem.Remove();
                    }

                    for (int i = 0; i < count; i++)
                    {
                        Serial s = p.ReadUInt32();
                        if (World.Player == null || s != World.Player.Serial)
                            m_Party.Add(s);
                    }

                    break;
                }

                case 0x03: // text message

                case 0x04: // 3 = private, 4 = public
                {
                    //Serial from = p.ReadUInt32();
                    //string text = p.ReadUnicodeStringSafe();
                    break;
                }

                case 0x07: // party invite
                {
                    //Serial leader = p.ReadUInt32();
                    PartyLeader = p.ReadUInt32();

                    if (Config.GetBool("BlockPartyInvites"))
                    {
                        Client.Instance.SendToServer(new DeclineParty(PacketHandlers.PartyLeader));
                    }

                    if (Config.GetBool("AutoAcceptParty"))
                    {
                        Mobile leaderMobile = World.FindMobile(PartyLeader);
                        if (leaderMobile != null && FriendsManager.IsFriend(leaderMobile.Serial))
                        {
                            if (PartyLeader != Serial.Zero)
                            {
                                World.Player.SendMessage($"Auto accepted party invite from: {leaderMobile.Name}");

                                Client.Instance.SendToServer(new AcceptParty(PartyLeader));
                                PartyLeader = Serial.Zero;
                            }
                        }
                    }
                    else
                    {
                        if (m_PartyDeclineTimer == null)
                            m_PartyDeclineTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(10.0),
                                new TimerCallback(PartyAutoDecline));
                        m_PartyDeclineTimer.Start();
                    }

                    break;
                }
            }


            if (Engine.MainWindow.MapWindow != null)
                Engine.MainWindow.SafeAction(s => s.MapWindow.UpdateMap());
        }

        private static void PartyAutoDecline()
        {
            PartyLeader = Serial.Zero;
        }

        private static void PingResponse(PacketReader p, PacketHandlerEventArgs args)
        {
            if (Ping.Response(p.ReadByte()))
                args.Block = true;
        }

        private static void ClientEncodedPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();
            ushort packetID = p.ReadUInt16();
            switch (packetID)
            {
                case 0x19: // set ability
                {
                    int ability = 0;
                    if (p.ReadByte() == 0)
                        ability = p.ReadInt32();

                    if (ability >= 0 && ability < (int) AOSAbility.Invalid && Macros.MacroManager.AcceptActions)
                        MacroManager.Action(new SetAbilityAction((AOSAbility) ability));
                    else if (ability >= 0 && ability < (int) AOSAbility.Invalid)
                        ScriptManager.AddToScript($"setability '{ability}'");

                    break;
                }
            }
        }

        private static string m_LastPW = "";

        private static void ServerListLogin(Packet p, PacketHandlerEventArgs args)
        {
            m_LastPW = "";
            if (!Config.GetBool("RememberPwds"))
                return;

            World.AccountName = p.ReadStringSafe(30);
            string pass = p.ReadStringSafe(30);

            if (pass == "")
            {
                pass = PasswordMemory.Find(World.AccountName, Client.Instance.LastConnection);
                if (pass != null && pass != "")
                {
                    p.Seek(31, SeekOrigin.Begin);
                    p.WriteAsciiFixed(pass, 30);
                    m_LastPW = pass;
                }
            }
            else
            {
                PasswordMemory.Add(World.AccountName, pass, Client.Instance.LastConnection);
            }
        }

        private static void GameLogin(Packet p, PacketHandlerEventArgs args)
        {
            int authID = p.ReadInt32();

            World.AccountName = p.ReadString(30);
            string password = p.ReadString(30);

            if (password == "" && m_LastPW != "" && Config.GetBool("RememberPwds"))
            {
                p.Seek(35, SeekOrigin.Begin);
                p.WriteAsciiFixed(m_LastPW, 30);
                m_LastPW = "";
            }

            ScriptManager.OnLogin();
        }

        private static void MenuResponse(PacketReader pvSrc, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            uint serial = pvSrc.ReadUInt32();
            ushort menuID = pvSrc.ReadUInt16();
            ushort index = pvSrc.ReadUInt16();
            ushort itemID = pvSrc.ReadUInt16();
            ushort hue = pvSrc.ReadUInt16();

            World.Player.HasMenu = false;
            if (MacroManager.AcceptActions)
                MacroManager.Action(new MenuResponseAction(index, itemID, hue));

            ScriptManager.AddToScript($"menuresponse {index} {itemID} {hue}");
        }

        private static void SendMenu(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            World.Player.CurrentMenuS = p.ReadUInt32();
            World.Player.CurrentMenuI = p.ReadUInt16();
            World.Player.HasMenu = true;
            if (MacroManager.AcceptActions && MacroManager.Action(new WaitForMenuAction(World.Player.CurrentMenuI)))
                args.Block = true;

            if (ScriptManager.AddToScript($"replymenu {World.Player.CurrentMenuI}"))
                args.Block = true;
        }

        private static void HueResponse(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial serial = p.ReadUInt32();
            ushort iid = p.ReadUInt16();
            ushort hue = p.ReadUInt16();

            if (serial == Serial.MinusOne)
            {
                if (HueEntry.Callback != null)
                    HueEntry.Callback(hue);
                args.Block = true;
            }
        }

        private static void ServerAddress(Packet p, PacketHandlerEventArgs args)
        {
            int port = Config.GetInt("ForcePort");
            if (port != 0)
            {
                try
                {
                    string[] parts = Config.GetString("ForceIP").Split('.');
                    p.Write((byte) Convert.ToInt16(parts[0]));
                    p.Write((byte) Convert.ToInt16(parts[1]));
                    p.Write((byte) Convert.ToInt16(parts[2]));
                    p.Write((byte) Convert.ToInt16(parts[3]));

                    p.Write((ushort) port);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(Engine.MainWindow, "Error parsing Proxy Settings.",
                        "Force Proxy Error.");
                }
            }
        }

        private static void Features(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player != null)
                World.Player.Features = p.ReadUInt16();
        }

        private static void PersonalLight(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player != null && !args.Block)
            {
                p.ReadUInt32(); // serial

                World.Player.LocalLightLevel = p.ReadSByte();

                if (EnforceLightLevels(World.Player.LocalLightLevel))
                    args.Block = true;
            }
        }

        private static void GlobalLight(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player != null && !args.Block)
            {
                World.Player.GlobalLightLevel = p.ReadByte();

                if (EnforceLightLevels(World.Player.GlobalLightLevel))
                    args.Block = true;
            }
        }

        private static bool EnforceLightLevels(int lightLevel)
        {
            if (Config.GetBool("MinMaxLightLevelEnabled"))
            {
                // 0 bright, 30 is dark
                if (lightLevel == -1)
                {
                    lightLevel = Convert.ToByte(Config.GetInt("MinLightLevel"));
                }
                else if (lightLevel < Config.GetInt("MaxLightLevel"))
                {
                    lightLevel = Convert.ToByte(Config.GetInt("MaxLightLevel")); // light level is too light
                }
                else if (lightLevel > Config.GetInt("MinLightLevel")) // light level is too dark
                {
                    lightLevel = Convert.ToByte(Config.GetInt("MinLightLevel"));
                }
                else // No need to block or do anything special
                {
                    return false;
                }

                World.Player.LocalLightLevel = 0;
                World.Player.GlobalLightLevel = (byte) lightLevel;

                Client.Instance.SendToClient(new GlobalLightLevel(lightLevel));
                Client.Instance.SendToClient(new PersonalLightLevel(World.Player));

                return true;
            }

            return false;
        }

        private static void ServerSetWarMode(PacketReader p, PacketHandlerEventArgs args)
        {
            World.Player.Warmode = p.ReadBoolean();
        }

        private static void CustomHouseInfo(PacketReader p, PacketHandlerEventArgs args)
        {
            p.ReadByte(); // compression
            p.ReadByte(); // Unknown

            Item i = World.FindItem(p.ReadUInt32());
            if (i != null)
            {
                i.HouseRevision = p.ReadInt32();
                i.HousePacket = p.CopyBytes(0, p.Length);
            }
        }

        /*
        Packet Build
        1.  BYTE[1] Cmd
        2.  BYTE[2] len
        3.  BYTE[4] Player Serial
        4.  BYTE[4] Gump ID
        5.  BYTE[4] x
        6.  BYTE[4] y
        7.  BYTE[4] Compressed Gump Layout Length (CLen)
        8.  BYTE[4] Decompressed Gump Layout Length (DLen)
        9.  BYTE[CLen-4] Gump Data, zlib compressed
        10. BYTE[4] Number of text lines
        11. BYTE[4] Compressed Text Line Length (CTxtLen)
        12. BYTE[4] Decompressed Text Line Length (DTxtLen)
        13. BYTE[CTxtLen-4] Gump's Compressed Text data, zlib compressed
         */
        private static void CompressedGump(PacketReader p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            World.Player.CurrentGumpS = p.ReadUInt32();
            World.Player.CurrentGumpI = p.ReadUInt32();
            World.Player.HasCompressedGump = true;

            if (Macros.MacroManager.AcceptActions &&
                MacroManager.Action(new WaitForGumpAction(World.Player.CurrentGumpI)))
                args.Block = true;

            ScriptManager.AddToScript($"waitforgump {World.Player.CurrentGumpI}");

            List<string> gumpStrings = new List<string>();

            try
            {
                int x = p.ReadInt32(), y = p.ReadInt32();

                string layout = p.GetCompressedReader().ReadString();

                int numStrings = p.ReadInt32();
                if (numStrings < 0 || numStrings > 256)
                    numStrings = 0;

                // Split on one or more non-digit characters.
                World.Player.CurrentGumpStrings.Clear();

                string[] numbers = Regex.Split(layout, @"\D+");

                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int i = int.Parse(value);
                        if ((i >= 500000 && i <= 503405) || (i >= 1000000 && i <= 1155584) ||
                            (i >= 3000000 && i <= 3011032))
                            gumpStrings.Add(Language.GetString(i));
                    }
                }

                PacketReader pComp = p.GetCompressedReader();
                int len = 0;
                int x1 = 0;
                string[] stringlistparse = new string[numStrings];

                while (!pComp.AtEnd && (len = pComp.ReadInt16()) > 0)
                {
                    string tempString = pComp.ReadUnicodeString(len);
                    stringlistparse[x1] = tempString;
                    x1++;
                }

                if (TryParseGump(layout, out string[] gumpPieces))
                {
                    gumpStrings.AddRange(ParseGumpString(gumpPieces, stringlistparse));
                }

                if (Config.GetBool("CaptureMibs") && MessageInBottleCapture.IsMibGump(layout))
                {
                    switch (gumpStrings.Count)
                    {
                        //Classic, non-custom MIB
                        case 3:
                            MessageInBottleCapture.CaptureMibCoordinates(gumpStrings[2], false);
                            break;
                        case 4:
                            MessageInBottleCapture.CaptureMibCoordinates(gumpStrings[2], true);
                            break;
                    }
                }

                World.Player.CurrentGumpStrings.AddRange(gumpStrings);
                World.Player.CurrentGumpRawData = layout; // Get raw data of current gump
            }
            catch
            {
            }
        }

        private static bool TryParseGump(string gumpData, out string[] pieces)
        {
            List<string> i = new List<string>();
            int dataIndex = 0;
            while (dataIndex < gumpData.Length)
            {
                if (gumpData.Substring(dataIndex) == "\0")
                {
                    break;
                }
                else
                {
                    int begin = gumpData.IndexOf("{", dataIndex);
                    int end = gumpData.IndexOf("}", dataIndex + 1);
                    if ((begin != -1) && (end != -1))
                    {
                        string sub = gumpData.Substring(begin + 1, end - begin - 1).Trim();
                        i.Add(sub);
                        dataIndex = end;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            pieces = i.ToArray();
            return (pieces.Length > 0);
        }

        private static List<string> ParseGumpString(string[] gumpPieces, string[] gumpLines)
        {
            List<string> gumpText = new List<string>();
            for (int i = 0; i < gumpPieces.Length; i++)
            {
                string[] gumpParams = Regex.Split(gumpPieces[i], @"\s+");
                switch (gumpParams[0].ToLower())
                {
                    case "croppedtext":
                        gumpText.Add(gumpLines[int.Parse(gumpParams[6])]);
                        // CroppedText [x] [y] [width] [height] [color] [text-id]
                        // Adds a text field to the gump. gump is similar to the text command, but the text is cropped to the defined area.
                        //gump.AddControl(new CroppedText(gump, gumpParams, gumpLines), currentGUMPPage);
                        //(gump.LastControl as CroppedText).Hue = 1;
                        break;

                    case "htmlgump":
                        gumpText.Add(gumpLines[int.Parse(gumpParams[5])]);
                        // HtmlGump [x] [y] [width] [height] [text-id] [background] [scrollbar]
                        // Defines a text-area where Html-commands are allowed.
                        // [background] and [scrollbar] can be 0 or 1 and define whether the background is transparent and a scrollbar is displayed.
                        //	gump.AddControl(new HtmlGumpling(gump, gumpParams, gumpLines), currentGUMPPage);
                        break;

                    case "text":
                        gumpText.Add(gumpLines[int.Parse(gumpParams[4])]);
                        // Text [x] [y] [color] [text-id]
                        // Defines the position and color of a text (data) entry.
                        //gump.AddControl(new TextLabel(gump, gumpParams, gumpLines), currentGUMPPage);
                        break;
                }
            }

            return gumpText;
        }

        private static DateTime _lastSelfDeathScreenshot = DateTime.MinValue;

        private static void ResurrectionGump(PacketReader p, PacketHandlerEventArgs args)
        {
            if (Config.GetBool("CaptureOwnDeath") &&
                _lastSelfDeathScreenshot + TimeSpan.FromMilliseconds(500) < DateTime.UtcNow)
            {
                ScreenCapManager.DeathCapture(Config.GetDouble("CaptureOwnDeathDelay"));
                _lastSelfDeathScreenshot = DateTime.UtcNow;
            }
        }

        private static void BuffDebuff(PacketReader p, PacketHandlerEventArgs args)
        {
            Serial ser = p.ReadUInt32();
            ushort icon = p.ReadUInt16();
            ushort action = p.ReadUInt16();

            if (Enum.IsDefined(typeof(BuffIcon), icon))
            {
                BuffIcon buff = (BuffIcon) icon;

                string format = Config.GetString("BuffDebuffFormat");
                if (string.IsNullOrEmpty(format))
                {
                    format = "[{action}{name}]";
                }

                switch (action)
                {
                    case 0x01: // show

                        p.ReadUInt32(); //0x000
                        p.ReadUInt16(); //icon # again..?
                        p.ReadUInt16(); //0x1 = show
                        p.ReadUInt32(); //0x000
                        ushort duration = p.ReadUInt16();
                        p.ReadUInt16(); //0x0000
                        p.ReadByte(); //0x0

                        BuffsDebuffs buffInfo = new BuffsDebuffs
                        {
                            IconNumber = icon,
                            BuffIcon = (BuffIcon) icon,
                            ClilocMessage1 = Language.GetCliloc((int) p.ReadUInt32()),
                            ClilocMessage2 = Language.GetCliloc((int) p.ReadUInt32()),
                            Duration = duration,
                            Timestamp = DateTime.UtcNow
                        };

                        if (World.Player != null && World.Player.BuffsDebuffs.All(b => b.BuffIcon != buff))
                        {
                            World.Player.BuffsDebuffs.Add(buffInfo);

                            if (Config.GetBool("ShowBuffDebuffOverhead") &&
                                !BuffsTimer.IsFiltered(buffInfo.ClilocMessage1))
                            {
                                World.Player.OverheadMessage(Config.GetInt("BuffHue"),
                                    format.Replace("{action}", "+").Replace("{name}", buffInfo.ClilocMessage1)
                                        .Replace("{duration}", buffInfo.Duration.ToString()));
                            }
                        }

                        break;

                    case 0x0: // remove
                        if (World.Player != null) // && World.Player.BuffsDebuffs.Any(b => b.BuffIcon == buff))
                        {
                            if (Config.GetBool("ShowBuffDebuffOverhead"))
                            {
                                string buffRemoveInfo = World.Player.BuffsDebuffs.Where(b => b.BuffIcon == buff)
                                    .Select(x => x.ClilocMessage1).FirstOrDefault();

                                if (!BuffsTimer.IsFiltered(buffRemoveInfo))
                                    World.Player.OverheadMessage(Config.GetInt("DebuffHue"),
                                        format.Replace("{action}", "-").Replace("{name}", buffRemoveInfo)
                                            .Replace("{duration}", string.Empty));
                            }

                            World.Player.BuffsDebuffs.RemoveAll(b => b.BuffIcon == buff);
                        }

                        break;
                }

                Client.Instance.RequestTitlebarUpdate();
            }

            if (World.Player != null && World.Player.BuffsDebuffs.Count > 0)
            {
                BuffsTimer.Start();
            }
            else
            {
                BuffsTimer.Stop();
            }
        }

        private static void AttackRequest(Packet p, PacketHandlerEventArgs args)
        {
            if (Config.GetBool("ShowAttackTargetOverhead"))
            {
                uint serial = p.ReadUInt32();

                Mobile m = World.FindMobile(serial);

                if (m == null) return;

                World.Player.OverheadMessage(FriendsManager.IsFriend(m.Serial) ? 63 : m.GetNotorietyColorInt(),
                    $"Attack: {m.Name}");
            }
        }

        private static void TradeRequest(PacketReader p, PacketHandlerEventArgs args)
        {
            if (Config.GetBool("BlockTradeRequests"))
            {
                args.Block = true;
            }
        }

        private static void UnicodePromptSend(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            //uint serial = p.ReadUInt32();
            //uint id = p.ReadUInt32();
            //uint type = p.ReadUInt32();

            uint serial = p.ReadUInt32();
            uint id = p.ReadUInt32();
            uint type = p.ReadUInt32();

            string lang = p.ReadStringSafe(4);
            string text = p.ReadUnicodeStringLESafe();

            World.Player.HasPrompt = false;
            World.Player.PromptSenderSerial = serial;
            World.Player.PromptID = id;
            World.Player.PromptType = type;
            World.Player.PromptInputText = text;

            if (MacroManager.AcceptActions && !string.IsNullOrEmpty(World.Player.PromptInputText))
            {
                MacroManager.Action(new PromptAction(World.Player.PromptInputText));
            }

            if (!string.IsNullOrEmpty(World.Player.PromptInputText))
                ScriptManager.AddToScript($"promptresponse '{World.Player.PromptInputText}'");
        }

        private static void UnicodePromptReceived(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            uint serial = p.ReadUInt32();
            uint id = p.ReadUInt32();
            uint type = p.ReadUInt32();

            World.Player.HasPrompt = true;
            World.Player.PromptSenderSerial = serial;
            World.Player.PromptID = id;
            World.Player.PromptType = type;

            if (MacroManager.AcceptActions)
            {
                MacroManager.Action(new WaitForPromptAction(id));
            }

            ScriptManager.AddToScript($"waitforprompt {id}");

            //args.Block = true;

            //string lang = p.ReadStringSafe(4);
            //string message = p.ReadUnicodeStringSafe();
        }

        private static void PlaySoundEffect(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            byte type = p.ReadByte();
            ushort sound = p.ReadUInt16();

            if (Config.GetBool("SoundFilterEnabled"))
            {
                bool blockSound = SoundMusicManager.IsFilteredSound(sound, out string name);

                if (blockSound)
                {
                    args.Block = true;

                    if (Config.GetBool("ShowFilteredSound"))
                    {
                        World.Player.SendMessage(MsgLevel.Warning, $"Blocked Sound: '{name}' (0x{sound:X})");
                        return;
                    }
                }
            }

            if (Config.GetBool("ShowPlayingSoundInfo"))
            {
                World.Player.SendMessage(MsgLevel.Info,
                    $"Playing Sound: '{SoundMusicManager.GetSoundName(sound)}' (0x{sound:X})");
            }
        }

        private static void PlayMusic(Packet p, PacketHandlerEventArgs args)
        {
            if (World.Player == null)
                return;

            if (!Config.GetBool("ShowMusicInfo"))
                return;

            ushort musicId = p.ReadUInt16();

            World.Player.SendMessage(
                $"Playing Music: '{SoundMusicManager.GetMusicName(musicId, out bool loop)}' (id: '{musicId}' loop: '{loop}')");
        }
    }
}