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
using Assistant.Agents;

namespace Assistant
{
    public enum MessageType
    {
        Regular = 0x00,
        System = 0x01,
        Emote = 0x02,
        Label = 0x06,
        Focus = 0x07,
        Whisper = 0x08,
        Yell = 0x09,
        Spell = 0x0A,
        Guild = 0x0D,
        Alliance = 0x0E,
        Encoded = 0xC0,

        Special = 0x20
    }

    public sealed class QueryPartyLocs : Packet
    {
        public QueryPartyLocs() : base(0xF0)
        {
            EnsureCapacity(4);
            Write((byte) 0x00);
        }
    }

    public sealed class SendPartyMessage : Packet
    {
        public SendPartyMessage(string message) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort) 0x06); // party command
            Write((byte) 0x04); // tell party
            WriteBigUniNull(message);
        }
    }

    public sealed class AcceptParty : Packet
    {
        public AcceptParty(Serial leader) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort) 0x06); // party command
            Write((byte) 0x08); // accept
            Write((uint) leader);
        }
    }

    public sealed class DeclineParty : Packet
    {
        public DeclineParty(Serial leader) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort) 0x06); // party command
            Write((byte) 0x09); // decline
            Write((uint) leader);
        }
    }

    public sealed class AddParty : Packet
    {
        public AddParty() : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort)0x06); // party command
            Write((byte)0x01); // add party
            Write(0);
        }
    }

    public sealed class SendMessageParty : Packet
    {
        public SendMessageParty(string message) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 1 + 4);

            Write((ushort) 0x06); // party command
            Write((byte) 0x04); // tell party
            WriteBigUniNull(message);
        }
    }

    public sealed class ContainerContent : Packet
    {
        public ContainerContent(List<Item> items) : this(items, Engine.UsePostKRPackets)
        {
        }

        public ContainerContent(List<Item> items, bool useKR) : base(0x3C)
        {
            Write((ushort) items.Count);

            foreach (Item item in items)
            {
                Write((uint) item.Serial);
                Write((ushort) item.ItemID);
                Write((sbyte) 0);
                Write((ushort) item.Amount);
                Write((ushort) item.Position.X);
                Write((ushort) item.Position.Y);

                if (useKR)
                    Write((byte) item.GridNum);

                if (item.Container is Item)
                    Write((uint) ((Item) item.Container).Serial);
                else
                    Write((uint) 0);
                Write((ushort) item.Hue);
            }
        }
    }

    public sealed class ContainerItem : Packet
    {
        public ContainerItem(Item item) : this(item, Engine.UsePostKRPackets)
        {
        }

        public ContainerItem(Item item, bool isKR) : base(0x25, 20)
        {
            if (isKR)
                EnsureCapacity(21);

            Write(item.Serial);

            Write(item.ItemID);
            Write((byte) 0);
            Write(item.Amount);
            Write((ushort) item.Position.X);
            Write((ushort) item.Position.Y);

            if (isKR)
                Write(item.GridNum);

            object cont = item.Container;
            if (cont is UOEntity)
                Write((uint) ((UOEntity) item.Container).Serial);
            else if (cont is uint)
                Write((uint) cont);
            else if (cont is Serial)
                Write((Serial) item.Container);
            else
                Write((uint) 0x7FFFFFFF);

            if (SearchExemptionAgent.Contains(item))
                Write((ushort) Config.GetInt("ExemptColor"));
            else
                Write(item.Hue);
        }
    }

    public sealed class SingleClick : Packet
    {
        public SingleClick(object clicked) : base(0x09, 5)
        {
            if (clicked is Mobile)
                Write(((Mobile) clicked).Serial);
            else if (clicked is Item)
                Write(((Item) clicked).Serial);
            else if (clicked is Serial)
                Write(((Serial) clicked).Value);
            else
                Write((uint) 0);
        }
    }

    public sealed class DoubleClick : Packet
    {
        public DoubleClick(Serial clicked) : base(0x06, 5)
        {
            Write((uint) clicked.Value);
        }
    }

    public sealed class Target : Packet
    {
        public Target(uint tid) : this(tid, false, 0)
        {
        }

        public Target(uint tid, byte flags) : this(tid, false, flags)
        {
        }

        public Target(uint tid, bool ground) : this(tid, ground, 0)
        {
        }

        public Target(uint tid, bool ground, byte flags) : base(0x6C, 19)
        {
            Write(ground);
            Write(tid);
            Write(flags);
            Fill();
        }
    }

    public sealed class TargetResponse : Packet
    {
        public TargetResponse(TargetInfo info) : base(0x6C, 19)
        {
            Write((byte) info.Type);
            Write((uint) info.TargID);
            Write((byte) info.Flags);
            Write((uint) info.Serial);
            Write((ushort) info.X);
            Write((ushort) info.Y);
            Write((short) info.Z);
            Write((ushort) info.Gfx);
        }

        public TargetResponse(uint id, Mobile m) : base(0x6C, 19)
        {
            Write((byte) 0x00); // target object
            Write((uint) id);
            Write((byte) 0); // flags
            Write((uint) m.Serial);
            Write((ushort) m.Position.X);
            Write((ushort) m.Position.Y);
            Write((short) m.Position.Z);
            Write((ushort) m.Body);
        }

        public TargetResponse(uint id, Item item) : base(0x6C, 19)
        {
            Write((byte) 0x00); // target object
            Write((uint) id);
            Write((byte) 0); // flags
            Write((uint) item.Serial);
            Write((ushort) item.Position.X);
            Write((ushort) item.Position.Y);
            Write((short) item.Position.Z);
            Write((ushort) item.ItemID);
        }
    }

    public sealed class TargetCancelResponse : Packet
    {
        public TargetCancelResponse(uint id) : base(0x6C, 19)
        {
            Write((byte) 0);
            Write((uint) id);
            Write((byte) 0);
            Write((uint) 0);
            Write((ushort) 0xFFFF);
            Write((ushort) 0xFFFF);
            Write((short) 0);
            Write((ushort) 0);
        }
    }

    public sealed class AttackReq : Packet
    {
        public AttackReq(Serial serial) : base(0x05, 5)
        {
            Write((uint) serial);
        }
    }

    public sealed class SetWeather : Packet
    {
        public SetWeather(int type, int num) : base(0x65, 4)
        {
            Write((byte) type); //types: 0x00 - "It starts to rain", 0x01 - "A fierce storm approaches.", 0x02 - "It begins to snow", 0x03 - "A storm is brewing.", 0xFF - None (turns off sound effects), 0xFE (no effect?? Set temperature?) 
            Write((byte) num); //number of weather effects on screen
            Write((byte) 0xFE);
        }
    }

    public sealed class PlayMusic : Packet
    {
        public PlayMusic(ushort num) : base(0x6D, 3)
        {
            Write(num);
        }
    }

    public sealed class CancelTarget : Packet
    {
        public CancelTarget(uint id) : base(0x6C, 19)
        {
            Write((byte) 0);
            Write((uint) id);
            Write((byte) 3);
            Fill();
        }
    }

    public sealed class SkillsQuery : Packet
    {
        public SkillsQuery(Mobile m) : base(0x34, 10)
        {
            Write((uint) 0xEDEDEDED); // que el fuck, osi
            Write((byte) 0x05);
            Write(m.Serial);
        }
    }

    public sealed class StatusQuery : Packet
    {
        public StatusQuery(Mobile m) : base(0x34, 10)
        {
            Write((uint) 0xEDEDEDED);
            Write((byte) 0x04);
            Write(m.Serial);
        }
    }

    public sealed class StatLockInfo : Packet
    {
        public StatLockInfo(PlayerData m) : base(0xBF)
        {
            this.EnsureCapacity(12);

            Write((short) 0x19);
            Write((byte) 2);
            Write((int) m.Serial);
            Write((byte) 0);

            int lockBits = 0;

            lockBits |= (int) m.StrLock << 4;
            lockBits |= (int) m.DexLock << 2;
            lockBits |= (int) m.IntLock;

            Write((byte) lockBits);
        }
    }

    public sealed class SkillsList : Packet
    {
        public SkillsList() : base(0x3A)
        {
            EnsureCapacity(3 + 1 + Skill.Count * 9 + 2);

            Write((byte) 0x02);
            for (int i = 0; i < Skill.Count; i++)
            {
                Write((short) (i + 1));
                Write(World.Player.Skills[i].FixedValue);
                Write(World.Player.Skills[i].FixedBase);
                Write((byte) World.Player.Skills[i].Lock);
                Write(World.Player.Skills[i].FixedCap);
            }

            Write((short) 0);
        }
    }

    public sealed class SkillUpdate : Packet
    {
        public SkillUpdate(Skill s) : base(0x3A)
        {
            EnsureCapacity(3 + 1 + 9);

            Write((byte) 0xDF);

            Write((short) s.Index);
            Write((ushort) s.FixedValue);
            Write((ushort) s.FixedBase);
            Write((byte) s.Lock);
            Write((ushort) s.FixedCap);
        }
    }

    public sealed class SetSkillLock : Packet
    {
        public SetSkillLock(int skill, LockType type) : base(0x3A)
        {
            EnsureCapacity(6);
            Write((short) skill);
            Write((byte) type);
        }
    }

    public sealed class AsciiMessage : Packet
    {
        public AsciiMessage(Serial serial, int graphic, MessageType type, int hue, int font, string name,
            string text) : base(0x1C)
        {
            if (name == null) name = "";
            if (text == null) text = "";

            if (hue == 0)
                hue = 0x3B2;

            this.EnsureCapacity(45 + text.Length);

            Write((uint) serial);
            Write((short) graphic);
            Write((byte) type);
            Write((short) hue);
            Write((short) font);
            WriteAsciiFixed(name, 30);
            WriteAsciiNull(text);
        }
    }

    public sealed class ClientAsciiMessage : Packet
    {
        public ClientAsciiMessage(MessageType type, int hue, int font, string str) : base(0x03)
        {
            EnsureCapacity(1 + 2 + 1 + 2 + 2 + str.Length + 1);

            Write((byte) type);
            Write((short) hue);
            Write((short) font);
            WriteAsciiNull(str);
        }
    }

    public sealed class UnicodeMessage : Packet
    {
        public UnicodeMessage(Serial serial, int graphic, MessageType type, int hue, int font, string lang, string name,
            string text) : base(0xAE)
        {
            if (lang == null || lang == "") lang = "ENU";
            if (name == null) name = "";
            if (text == null) text = "";

            if (hue == 0)
                hue = 0x3B2;

            this.EnsureCapacity(50 + (text.Length * 2));

            Write((uint) serial);
            Write((ushort) graphic);
            Write((byte) type);
            Write((ushort) hue);
            Write((ushort) font);
            WriteAsciiFixed(lang.ToUpper(), 4);
            WriteAsciiFixed(name, 30);
            WriteBigUniNull(text);
        }
    }

    public sealed class ClientUniMessage : Packet
    {
        public ClientUniMessage(MessageType type, int hue, int font, string lang, ArrayList keys, string text) :
            base(0xAD)
        {
            if (lang == null || lang == "") lang = "ENU";
            if (text == null) text = "";

            this.EnsureCapacity(50 + (text.Length * 2) + (keys == null ? 0 : keys.Count + 1));
            if (keys == null || keys.Count <= 1)
                Write((byte) type);
            else
                Write((byte) (type | MessageType.Encoded));
            Write((short) hue);
            Write((short) font);
            WriteAsciiFixed(lang, 4);
            if (keys != null && keys.Count > 1)
            {
                Write((ushort) keys[0]);
                for (int i = 1; i < keys.Count; i++)
                    Write((byte) keys[i]);
                WriteUTF8Null(text);
            }
            else
            {
                WriteBigUniNull(text);
            }
        }
    }

    public sealed class ClientUniEncodedCommandMessage : Packet
    {
        public ClientUniEncodedCommandMessage(MessageType type, int hue, int font, string lang, List<ushort> keys,
            string text) : base(0xAD)
        {
            if (lang == null || lang == "") lang = "ENU";
            if (text == null) text = "";

            this.EnsureCapacity(50 + (text.Length * 2) + (keys == null ? 0 : keys.Count + 1));
            if (keys == null || keys.Count <= 1)
                Write((byte) type);
            else
                Write((byte) (type | MessageType.Encoded));
            Write((short) hue);
            Write((short) font);
            WriteAsciiFixed(lang, 4);
            if (keys != null && keys.Count > 1)
            {
                Write(keys[0]);
                for (int i = 1; i < keys.Count; i++)
                    Write((byte) keys[i]);
                WriteUTF8Null(text);
            }
            else
            {
                WriteBigUniNull(text);
            }
        }
    }

    public sealed class LiftRequest : Packet
    {
        public LiftRequest(Serial ser, int amount) : base(0x07, 7)
        {
            this.Write(ser.Value);
            this.Write((ushort) amount);
        }

        public LiftRequest(Item i, int amount) : this(i.Serial, amount)
        {
        }

        public LiftRequest(Item i) : this(i.Serial, i.Amount)
        {
        }
    }

    public sealed class LiftRej : Packet
    {
        public LiftRej() : this(5) // reason = Inspecific
        {
        }

        public LiftRej(byte reason) : base(0x27, 2)
        {
            Write(reason);
        }
    }

    public sealed class EquipRequest : Packet
    {
        public EquipRequest(Serial item, Mobile to, Layer layer) : base(0x13, 10)
        {
            Write(item);
            Write((byte) layer);
            Write(to.Serial);
        }

        public EquipRequest(Serial item, Serial to, Layer layer) : base(0x13, 10)
        {
            Write(item);
            Write((byte) layer);
            Write(to);
        }
    }

    public sealed class DropRequest : Packet
    {
        public DropRequest(Item item, Serial destSer) : base(0x08, 14)
        {
            if (Engine.UsePostKRPackets)
                EnsureCapacity(15);

            Write(item.Serial);
            Write((short) (-1));
            Write((short) (-1));
            Write((sbyte) 0);
            if (Engine.UsePostKRPackets)
                Write((byte) 0);
            Write(destSer);
        }

        public DropRequest(Item item, Item to) : this(item, to.Serial)
        {
        }

        public DropRequest(Serial item, Point3D pt, Serial dest) : base(0x08, 14)
        {
            if (Engine.UsePostKRPackets)
                EnsureCapacity(15);

            Write(item);
            Write((ushort) pt.X);
            Write((ushort) pt.Y);
            Write((sbyte) pt.Z);
            if (Engine.UsePostKRPackets)
                Write((byte) 0);
            Write(dest);
        }

        public DropRequest(Item item, Point3D pt, Serial destSer) : this(item.Serial, pt, destSer)
        {
        }
    }

    public class SellListItem
    {
        public Serial Serial;
        public ushort Amount;

        public SellListItem(Serial s, ushort a)
        {
            Serial = s;
            Amount = a;
        }
    }

    public sealed class VendorSellResponse : Packet
    {
        public VendorSellResponse(Mobile vendor, List<SellListItem> list) : base(0x9F)
        {
            EnsureCapacity(1 + 2 + 4 + 2 + list.Count * 6);

            Write((uint) vendor.Serial);
            Write((ushort) list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                SellListItem sli = list[i];
                Write((uint) sli.Serial);
                Write((ushort) sli.Amount);
            }
        }
    }

    public sealed class MobileStatusExtended : Packet
    {
        public MobileStatusExtended(PlayerData m) : base(0x11)
        {
            string name = m.Name;
            if (name == null) name = "";

            this.EnsureCapacity(88);

            Write((uint) m.Serial);
            WriteAsciiFixed(name, 30);

            Write((short) m.Hits);
            Write((short) m.HitsMax);

            Write(false); // cannot edit name

            Write((byte) 0x03); // no aos info

            Write(m.Female);

            Write((short) m.Str);
            Write((short) m.Dex);
            Write((short) m.Int);

            Write((short) m.Stam);
            Write((short) m.StamMax);

            Write((short) m.Mana);
            Write((short) m.ManaMax);

            Write((int) m.Gold);
            Write((short) m.AR);
            Write((short) m.Weight);
            Write((short) m.StatCap);
            Write((byte) m.Followers);
            Write((byte) m.FollowersMax);
        }
    }

    public sealed class MobileStatusCompact : Packet
    {
        public MobileStatusCompact(Mobile m) : base(0x11)
        {
            string name = m.Name;
            if (name == null) name = "";

            this.EnsureCapacity(88);

            Write((uint) m.Serial);
            WriteAsciiFixed(name, 30);

            Write((short) m.Hits);
            Write((short) m.HitsMax);

            Write(false); // cannot edit name

            Write((byte) 0x00); // no aos info
        }
    }

    public sealed class GumpTextEntry
    {
        public GumpTextEntry(ushort id, string s)
        {
            EntryID = id;
            Text = s;
        }

        public ushort EntryID;
        public string Text;
    }

    public sealed class GumpResponse : Packet
    {
        public GumpResponse(uint serial, uint tid, int bid, int[] switches, GumpTextEntry[] entries) : base(0xB1)
        {
            EnsureCapacity(3 + 4 + 4 + 4 + 4 + switches.Length * 4 + 4 + entries.Length * 4);

            Write((uint) serial);
            Write((uint) tid);

            Write((int) bid);

            Write((int) switches.Length);
            for (int i = 0; i < switches.Length; i++)
                Write((int) switches[i]);
            Write((int) entries.Length);
            for (int i = 0; i < entries.Length; i++)
            {
                GumpTextEntry gte = (GumpTextEntry) entries[i];
                Write((ushort) gte.EntryID);
                Write((ushort) (gte.Text.Length * 2));
                WriteBigUniFixed(gte.Text, gte.Text.Length);
            }
        }
    }

    public sealed class CompressedGump : Packet
    {
        public CompressedGump(uint serial, uint tid, int bid, int[] switches, GumpTextEntry[] entries) : base(0xDD)
        {
            EnsureCapacity(3 + 4 + 4 + 4 + 4 + switches.Length * 4 + 4 + entries.Length * 4);

            Write((uint) serial);
            Write((uint) tid);

            Write((int) bid);

            Write((int) switches.Length);
            for (int i = 0; i < switches.Length; i++)
                Write((int) switches[i]);
            Write((int) entries.Length);
            for (int i = 0; i < entries.Length; i++)
            {
                GumpTextEntry gte = (GumpTextEntry) entries[i];
                Write((ushort) gte.EntryID);
                Write((ushort) (gte.Text.Length * 2));
                WriteBigUniFixed(gte.Text, gte.Text.Length);
            }
        }
    }

    public sealed class UseSkill : Packet
    {
        public UseSkill(int sk) : base(0x12)
        {
            string cmd = String.Format("{0} 0", sk);
            EnsureCapacity(4 + cmd.Length + 1);
            Write((byte) 0x24);
            WriteAsciiNull(cmd);
        }

        public UseSkill(SkillName sk) : this((int) sk)
        {
        }
    }

    public sealed class ExtCastSpell : Packet
    {
        public ExtCastSpell(Serial book, ushort spell) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 2 + 4 + 2);
            Write((short) 0x1C);
            Write((short) (book.IsItem ? 1 : 2));
            if (book.IsItem)
                Write((uint) book);
            Write((short) spell);
        }
    }

    public sealed class CastSpellFromBook : Packet
    {
        public CastSpellFromBook(Serial book, ushort spell) : base(0x12)
        {
            string cmd;
            if (book.IsItem)
                cmd = String.Format("{0} {1}", spell, book.Value);
            else
                cmd = String.Format("{0}", spell);
            EnsureCapacity(3 + 1 + cmd.Length + 1);
            Write((byte) 0x27);
            WriteAsciiNull(cmd);
        }
    }

    public sealed class CastSpellFromMacro : Packet
    {
        public CastSpellFromMacro(ushort spell) : base(0x12)
        {
            string cmd = spell.ToString();
            EnsureCapacity(3 + 1 + cmd.Length + 1);
            Write((byte) 0x56);
            WriteAsciiNull(cmd);
        }
    }

    public sealed class DisarmRequest : Packet
    {
        public DisarmRequest() : base(0xBF)
        {
            EnsureCapacity(3);
            Write((ushort) 0x09);
        }
    }

    public sealed class StunRequest : Packet
    {
        public StunRequest() : base(0xBF)
        {
            EnsureCapacity(3);
            Write((ushort) 0x0A);
        }
    }

    public sealed class CloseGump : Packet
    {
        public CloseGump(uint typeID, uint buttonID) : base(0xBF)
        {
            EnsureCapacity(13);

            Write((short) 0x04);
            Write((int) typeID);
            Write((int) buttonID);
        }

        public CloseGump(uint typeID) : base(0xBF)
        {
            EnsureCapacity(13);

            Write((short) 0x04);
            Write((int) typeID);
            Write((int) 0);
        }
    }

    public sealed class ChangeCombatant : Packet
    {
        public ChangeCombatant(Serial ser) : base(0xAA, 5)
        {
            Write((uint) ser);
        }

        public ChangeCombatant(Mobile m) : this(m.Serial)
        {
        }
    }

    public sealed class UseAbility : Packet
    {
        // ints are 'encoded' with a leading bool, if true then the number is 0, if flase then followed by all 4 bytes (lame :-)
        public UseAbility(AOSAbility a) : base(0xD7)
        {
            EnsureCapacity(1 + 2 + 4 + 2 + 4);

            Write((uint) World.Player.Serial);
            Write((ushort) 0x19);
            if (a == AOSAbility.Clear)
            {
                Write(true);
            }
            else
            {
                Write(false);
                Write((int) a);
            }
        }
    }

    public sealed class ClearAbility : Packet
    {
        public static readonly Packet Instance = new ClearAbility();

        public ClearAbility() : base(0xBF)
        {
            EnsureCapacity(5);

            Write((short) 0x21);
        }
    }

    public sealed class PingPacket : Packet
    {
        public PingPacket(byte seq) : base(0x73, 2)
        {
            Write(seq);
        }
    }

    public sealed class MobileUpdate : Packet
    {
        public MobileUpdate(Mobile m) : base(0x20, 19)
        {
            Write((int) m.Serial);
            Write((short) m.Body);
            Write((byte) 0);
            int ltHue = Config.GetInt("LTHilight");
            if (ltHue != 0 && Targeting.IsLastTarget(m))
                Write((short) (ltHue | 0x8000));
            else
                Write((short) m.Hue);
            Write((byte) m.GetPacketFlags());
            Write((short) m.Position.X);
            Write((short) m.Position.Y);
            Write((short) 0);
            Write((byte) m.Direction);
            Write((sbyte) m.Position.Z);
        }
    }

    public sealed class MobileIncoming : Packet
    {
        public MobileIncoming(Mobile m) : base(0x78)
        {
            int count = m.Contains.Count;
            int ltHue = Config.GetInt("LTHilight");
            bool isLT;
            if (ltHue != 0)
                isLT = Targeting.IsLastTarget(m);
            else
                isLT = false;

            EnsureCapacity(3 + 4 + 2 + 2 + 2 + 1 + 1 + 2 + 1 + 1 + 4 + count * (4 + 2 + 1 + 2));
            Write((uint) m.Serial);
            Write((ushort) m.Body);
            Write((ushort) m.Position.X);
            Write((ushort) m.Position.Y);
            Write((sbyte) m.Position.Z);
            Write((byte) m.Direction);
            Write((ushort) (isLT ? ltHue | 0x8000 : m.Hue));
            Write((byte) m.GetPacketFlags());
            Write((byte) m.Notoriety);

            for (int i = 0; i < count; ++i)
            {
                Item item = (Item) m.Contains[i];

                int itemID = item.ItemID & 0x3FFF;
                bool writeHue = (item.Hue != 0);
                if (writeHue || isLT)
                    itemID |= 0x8000;

                Write((uint) item.Serial);
                Write((ushort) itemID);
                Write((byte) item.Layer);
                if (isLT)
                    Write((ushort) (ltHue & 0x3FFF));
                else if (writeHue)
                    Write((ushort) item.Hue);
            }

            Write((uint) 0); // terminate
        }
    }

    public class VendorBuyItem
    {
        public VendorBuyItem(Serial ser, int amount, int price)
        {
            Serial = ser;
            Amount = amount;
            Price = price;
        }

        public readonly Serial Serial;
        public int Amount;
        public int Price;

        public int TotalCost
        {
            get { return Amount * Price; }
        }
    }

    public sealed class VendorBuyResponse : Packet
    {
        public VendorBuyResponse(Serial vendor, IList<VendorBuyItem> list) : base(0x3B)
        {
            EnsureCapacity(1 + 2 + 4 + 1 + list.Count * 7);

            Write(vendor);
            Write((byte) 0x02); // flag

            for (int i = 0; i < list.Count; i++)
            {
                VendorBuyItem vbi = list[i];
                Write((byte) 0x1A); // layer?
                Write(vbi.Serial);
                Write((ushort) vbi.Amount);
            }
        }
    }

    public sealed class MenuResponse : Packet
    {
        public MenuResponse(uint serial, ushort menuid, ushort index, ushort itemid, ushort hue) : base(0x7D, 13)
        {
            Write((uint) serial);
            Write(menuid);
            Write(index);
            Write(itemid);
            Write(hue);
        }
    }

    public sealed class HuePicker : Packet
    {
        public HuePicker() : this(Serial.MinusOne, 0x0FAB)
        {
        }

        public HuePicker(ItemID itemid) : this(Serial.MinusOne, itemid)
        {
        }

        public HuePicker(Serial serial, ItemID itemid) : base(0x95, 9)
        {
            Write((uint) serial);
            Write((ushort) 0);
            Write((ushort) itemid);
        }
    }

    public sealed class WalkRequest : Packet
    {
        public WalkRequest(Direction dir, byte seq) : base(0x02, 7)
        {
            Write((byte) dir);
            Write(seq);
            Write((int) -1); // key
        }
    }

    public sealed class ResyncReq : Packet
    {
        public ResyncReq() : base(0x22, 3)
        {
            Write((ushort) 0);
        }
    }

    public sealed class ACKTalk : Packet
    {
        public ACKTalk() : base(0x03)
        {
            EnsureCapacity(40);
            Write((byte) 0x20);
            Write((byte) 0x00);
            Write((byte) 0x34);
            Write((byte) 0x00);
            Write((byte) 0x03);
            Write((byte) 0xdb);
            Write((byte) 0x13);
            Write((byte) 0x14);
            Write((byte) 0x3f);
            Write((byte) 0x45);
            Write((byte) 0x2c);
            Write((byte) 0x58);
            Write((byte) 0x0f);
            Write((byte) 0x5d);
            Write((byte) 0x44);
            Write((byte) 0x2e);
            Write((byte) 0x50);
            Write((byte) 0x11);
            Write((byte) 0xdf);
            Write((byte) 0x75);
            Write((byte) 0x5c);
            Write((byte) 0xe0);
            Write((byte) 0x3e);
            Write((byte) 0x71);
            Write((byte) 0x4f);
            Write((byte) 0x31);
            Write((byte) 0x34);
            Write((byte) 0x05);
            Write((byte) 0x4e);
            Write((byte) 0x18);
            Write((byte) 0x1e);
            Write((byte) 0x72);
            Write((byte) 0x0f);
            Write((byte) 0x59);
            Write((byte) 0xad);
            Write((byte) 0xf5);
            Write((byte) 0x00);
        }
    }

    public sealed class WorldItem : Packet
    {
        public WorldItem(Item item) : base(0x1A)
        {
            this.EnsureCapacity(20);

            // 14 base length
            // +2 - Amount
            // +2 - Hue
            // +1 - Flags

            uint serial = (uint) item.Serial;
            ushort itemID = item.ItemID;
            ushort amount = item.Amount;
            int x = item.Position.X;
            int y = item.Position.Y;
            ushort hue = item.Hue;
            byte flags = item.GetPacketFlags();
            byte direction = item.Direction;

            if (amount != 0)
                serial |= 0x80000000;
            else
                serial &= 0x7FFFFFFF;
            Write((uint) serial);
            Write((ushort) (itemID & 0x7FFF));
            if (amount != 0)
                Write((ushort) amount);

            x &= 0x7FFF;
            if (direction != 0)
                x |= 0x8000;
            Write((ushort) x);

            y &= 0x3FFF;
            if (hue != 0)
                y |= 0x8000;
            if (flags != 0)
                y |= 0x4000;

            Write((ushort) y);
            if (direction != 0)
                Write((byte) direction);
            Write((sbyte) item.Position.Z);
            if (hue != 0)
                Write((ushort) hue);
            if (flags != 0)
                Write((byte) flags);
        }
    }

    public sealed class EquipmentItem : Packet
    {
        public EquipmentItem(Item item, Serial owner) : this(item, item.Hue, owner)
        {
        }

        public EquipmentItem(Item item, ushort hue, Serial owner) : base(0x2E, 15)
        {
            Write((uint) item.Serial);
            Write((ushort) item.ItemID);
            Write((sbyte) 0);
            Write((byte) item.Layer);
            Write((uint) owner);
            Write((ushort) hue);
        }
    }

    public sealed class ForceWalk : Packet
    {
        public ForceWalk(Direction d) : base(0x97, 2)
        {
            Write((byte) d);
        }
    }

    public sealed class PathFindTo : Packet
    {
        public PathFindTo(Point3D loc) : base(0x38, 7 * 20)
        {
            for (int i = 0; i < 20; i++)
            {
                if (i != 0)
                    Write((byte) 0x38);
                Write((ushort) loc.X);
                Write((ushort) loc.Y);
                Write((short) loc.Z);
            }
        }
    }

    public sealed class LoginConfirm : Packet
    {
        public LoginConfirm(Mobile m) : base(0x1B, 37)
        {
            Write((int) m.Serial);
            Write((int) 0);
            Write((short) m.Body);
            Write((short) m.Position.X);
            Write((short) m.Position.Y);
            Write((short) m.Position.Z);
            Write((byte) m.Direction);
            Write((byte) 0);
            Write((int) -1);

            Write((short) 0);
            Write((short) 0);
            Write((short) 6144);
            Write((short) 4096);
        }
    }

    public sealed class LoginComplete : Packet
    {
        public LoginComplete() : base(0x55, 1)
        {
        }
    }

    public sealed class DeathStatus : Packet
    {
        public DeathStatus(bool dead) : base(0x2C, 2)
        {
            Write((byte) (dead ? 0 : 2));
        }
    }

    public sealed class CurrentTime : Packet
    {
        public CurrentTime() : base(0x5B, 4)
        {
            DateTime now = DateTime.UtcNow;

            Write((byte) now.Hour);
            Write((byte) now.Minute);
            Write((byte) now.Second);
        }
    }

    public sealed class MapChange : Packet
    {
        public MapChange(byte map) : base(0xBF)
        {
            this.EnsureCapacity(6);

            Write((short) 0x08);
            Write((byte) map);
        }
    }

    public sealed class SeasonChange : Packet
    {
        public SeasonChange(int season, bool playSound) : base(0xBC, 3)
        {
            Write((byte) season);
            Write((bool) playSound);
        }
    }

    public sealed class SupportedFeatures : Packet
    {
        //private static int m_Value = 0x801F;
        public SupportedFeatures(ushort val) : base(0xB9, 3)
        {
            Write((ushort) val); // 0x01 = T2A, 0x02 = LBR
        }
    }

    public sealed class MapPatches : Packet
    {
        public MapPatches(int[] patches) : base(0xBF)
        {
            EnsureCapacity(9 + (4 * patches.Length));

            Write((short) 0x0018);

            Write((int) (patches.Length / 2));

            for (int i = 0; i < patches.Length; i++)
                Write((int) patches[i]);
            /*Write( (int) Ultima.Map.Felucca.Tiles.Patch.StaticBlocks );
            Write( (int) Ultima.Map.Felucca.Tiles.Patch.LandBlocks );

            Write( (int) Ultima.Map.Trammel.Tiles.Patch.StaticBlocks );
            Write( (int) Ultima.Map.Trammel.Tiles.Patch.LandBlocks );

            Write( (int) Ultima.Map.Ilshenar.Tiles.Patch.StaticBlocks );
            Write( (int) Ultima.Map.Ilshenar.Tiles.Patch.LandBlocks );

            Write( (int) Ultima.Map.Malas.Tiles.Patch.StaticBlocks );
            Write( (int) Ultima.Map.Malas.Tiles.Patch.LandBlocks );*/
        }
    }

    public sealed class MobileAttributes : Packet
    {
        public MobileAttributes(PlayerData m) : base(0x2D, 17)
        {
            Write(m.Serial);

            Write((short) m.HitsMax);
            Write((short) m.Hits);

            Write((short) m.ManaMax);
            Write((short) m.Mana);

            Write((short) m.StamMax);
            Write((short) m.Stam);
        }
    }

    public sealed class SetWarMode : Packet
    {
        public SetWarMode(bool mode) : base(0x72, 5)
        {
            Write(mode);
            Write((byte) 0x00);
            Write((byte) 0x32);
            Write((byte) 0x00);
            //Fill();
        }
    }

    public sealed class OpenDoorMacro : Packet
    {
        public OpenDoorMacro() : base(0x12)
        {
            EnsureCapacity(5);
            Write((byte) 0x58);
            Write((byte) 0);
        }
    }

    public sealed class PersonalLightLevel : Packet
    {
        public PersonalLightLevel(PlayerData m) : base(0x4E, 6)
        {
            Write((int) m.Serial);
            Write((sbyte) m.LocalLightLevel);
        }
    }

    public sealed class GlobalLightLevel : Packet
    {
        public GlobalLightLevel(int level) : base(0x4F, 2)
        {
            Write((sbyte) level);
        }
    }

    public sealed class DisplayPaperdoll : Packet
    {
        public DisplayPaperdoll(Mobile m, string text) : base(0x88, 66)
        {
            Write((int) m.Serial);
            WriteAsciiFixed(text, 60);
            Write((byte) (m.Warmode ? 1 : 0));
        }
    }

    public sealed class RemoveObject : Packet
    {
        public RemoveObject(UOEntity ent) : base(0x1D, 5)
        {
            Write((uint) ent.Serial);
        }

        public RemoveObject(Serial s) : base(0x1D, 5)
        {
            Write((uint) s);
        }
    }

    public sealed class ContextMenuRequest : Packet
    {
        public ContextMenuRequest(Serial entity) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4);
            Write((ushort) 0x13);
            Write((uint) entity);
        }
    }

    public sealed class ContextMenuResponse : Packet
    {
        public ContextMenuResponse(Serial entity, ushort idx) : base(0xBF)
        {
            EnsureCapacity(1 + 2 + 2 + 4 + 2);

            Write((ushort) 0x15);
            Write((uint) entity);
            Write((ushort) idx);
        }
    }

    public sealed class SetUpdateRange : Packet
    {
        public SetUpdateRange(int range) : base(0xC8, 2)
        {
            Write((byte) range);
        }
    }

    public sealed class PlaySound : Packet
    {
        public PlaySound(int sound) : base(0x54, 12)
        {
            Write((byte) 0x01); //(0x00=quiet, repeating, 0x01=single normally played sound effect)
            Write((ushort) sound);
            Write((ushort) 0);
            Write((ushort) World.Player.Position.X);
            Write((ushort) World.Player.Position.Y);
            Write((ushort) World.Player.Position.Z);
        }
    }

    public sealed class RazorNegotiateResponse : Packet
    {
        public RazorNegotiateResponse() : base(0xF0)
        {
            EnsureCapacity(1 + 2 + 1);

            Write((byte) 0xFF);
        }
    }

    public sealed class DesignStateGeneral : Packet
    {
        public DesignStateGeneral(Item house) : base(0xBF)
        {
            EnsureCapacity(13);

            Write((ushort) 0x1D);
            Write((uint) house.Serial);
            Write((int) house.HouseRevision);
        }
    }

    public sealed class StringQueryResponse : Packet
    {
        public StringQueryResponse(int serial, byte type, byte index, bool ok, string resp) : base(0xAC)
        {
            if (resp == null)
                resp = String.Empty;

            this.EnsureCapacity(1 + 2 + 4 + 1 + 1 + 1 + 2 + resp.Length + 1);

            Write((int) serial);
            Write((byte) type);
            Write((byte) index);
            Write((bool) ok);
            Write((short) (resp.Length + 1));
            WriteAsciiNull(resp);
        }
    }

    public class DesignStateDetailed : Packet
    {
        public const int MaxItemsPerStairBuffer = 750;

        private static byte[][] m_PlaneBuffers;
        private static bool[] m_PlaneUsed;

        private static byte[][] m_StairBuffers;

        private static byte[] m_InflatedBuffer = new byte[0x2000];
        private static byte[] m_DeflatedBuffer = new byte[0x2000];

        /*private static byte[] m_PrimBuffer = new byte[4];

        public override void Write( int value )
        {
             m_PrimBuffer[0] = (byte)(value >> 24);
             m_PrimBuffer[1] = (byte)(value >> 16);
             m_PrimBuffer[2] = (byte)(value >>  8);
             m_PrimBuffer[3] = (byte) value;

             UnderlyingStream.Write( m_PrimBuffer, 0, 4 );
        }

        public override void Write( short value )
        {
             m_PrimBuffer[0] = (byte)(value >> 8);
             m_PrimBuffer[1] = (byte) value;

             UnderlyingStream.Write( m_PrimBuffer, 0, 2 );
        }

        public override void Write( byte value )
        {
             UnderlyingStream.WriteByte( value );
        }

        public void Write( byte[] buffer, int offset, int size )
        {
             UnderlyingStream.Write( buffer, offset, size );
        }*/

        public static void Clear(byte[] buffer, int size)
        {
            for (int i = 0; i < size; ++i)
                buffer[i] = 0;
        }

        public DesignStateDetailed(Serial serial, int revision, int xMin, int yMin, int xMax, int yMax,
            MultiTileEntry[] tiles) : base(0xD8)
        {
            EnsureCapacity(17 + (tiles.Length * 5));

            Write((byte) 0x03); // Compression Type
            Write((byte) 0x00); // Unknown
            Write((uint) serial);
            Write((int) revision);
            Write((short) tiles.Length);
            Write((short) 0); // Buffer length : reserved
            Write((byte) 0); // Plane count : reserved

            int totalLength = 1; // includes plane count

            int width = (xMax - xMin) + 1;
            int height = (yMax - yMin) + 1;

            if (m_PlaneBuffers == null)
            {
                m_PlaneBuffers = new byte[9][];
                m_PlaneUsed = new bool[9];

                for (int i = 0; i < m_PlaneBuffers.Length; ++i)
                    m_PlaneBuffers[i] = new byte[0x400];

                m_StairBuffers = new byte[6][];

                for (int i = 0; i < m_StairBuffers.Length; ++i)
                    m_StairBuffers[i] = new byte[MaxItemsPerStairBuffer * 5];
            }
            else
            {
                for (int i = 0; i < m_PlaneUsed.Length; ++i)
                    m_PlaneUsed[i] = false;

                Clear(m_PlaneBuffers[0], width * height * 2);

                for (int i = 0; i < 4; ++i)
                {
                    Clear(m_PlaneBuffers[1 + i], (width - 1) * (height - 2) * 2);
                    Clear(m_PlaneBuffers[5 + i], width * (height - 1) * 2);
                }
            }

            int totalStairsUsed = 0;

            for (int i = 0; i < tiles.Length; ++i)
            {
                MultiTileEntry mte = tiles[i];
                int x = mte.m_OffsetX - xMin;
                int y = mte.m_OffsetY - yMin;
                int z = mte.m_OffsetZ;
                int plane, size;
                bool floor = false;
                try
                {
                    floor = (Ultima.TileData.ItemTable[mte.m_ItemID & 0x3FFF].Height <= 0);
                }
                catch
                {
                }

                switch (z)
                {
                    case 0:
                        plane = 0;
                        break;
                    case 7:
                        plane = 1;
                        break;
                    case 27:
                        plane = 2;
                        break;
                    case 47:
                        plane = 3;
                        break;
                    case 67:
                        plane = 4;
                        break;
                    default:
                    {
                        int stairBufferIndex = (totalStairsUsed / MaxItemsPerStairBuffer);
                        byte[] stairBuffer = m_StairBuffers[stairBufferIndex];

                        int byteIndex = (totalStairsUsed % MaxItemsPerStairBuffer) * 5;

                        stairBuffer[byteIndex++] = (byte) ((mte.m_ItemID >> 8) & 0x3F);
                        stairBuffer[byteIndex++] = (byte) mte.m_ItemID;

                        stairBuffer[byteIndex++] = (byte) mte.m_OffsetX;
                        stairBuffer[byteIndex++] = (byte) mte.m_OffsetY;
                        stairBuffer[byteIndex++] = (byte) mte.m_OffsetZ;

                        ++totalStairsUsed;

                        continue;
                    }
                }

                if (plane == 0)
                {
                    size = height;
                }
                else if (floor)
                {
                    size = height - 2;
                    x -= 1;
                    y -= 1;
                }
                else
                {
                    size = height - 1;
                    plane += 4;
                }

                int index = ((x * size) + y) * 2;

                m_PlaneUsed[plane] = true;
                m_PlaneBuffers[plane][index] = (byte) ((mte.m_ItemID >> 8) & 0x3F);
                m_PlaneBuffers[plane][index + 1] = (byte) mte.m_ItemID;
            }

            int planeCount = 0;

            for (int i = 0; i < m_PlaneBuffers.Length; ++i)
            {
                if (!m_PlaneUsed[i])
                    continue;

                ++planeCount;

                int size = 0;

                if (i == 0)
                    size = width * height * 2;
                else if (i < 5)
                    size = (width - 1) * (height - 2) * 2;
                else
                    size = width * (height - 1) * 2;

                byte[] inflatedBuffer = m_PlaneBuffers[i];

                int deflatedLength = m_DeflatedBuffer.Length;
                ZLibError ce = ZLib.compress2(m_DeflatedBuffer, ref deflatedLength, inflatedBuffer, size,
                    ZLibCompressionLevel.Z_DEFAULT_COMPRESSION);

                if (ce != ZLibError.Z_OK)
                {
                    Console.WriteLine("ZLib error: {0} (#{1})", ce, (int) ce);
                    deflatedLength = 0;
                    size = 0;
                }

                Write((byte) (0x20 | i));
                Write((byte) size);
                Write((byte) deflatedLength);
                Write((byte) (((size >> 4) & 0xF0) | ((deflatedLength >> 8) & 0xF)));
                Write(m_DeflatedBuffer, 0, deflatedLength);

                totalLength += 4 + deflatedLength;
            }

            int totalStairBuffersUsed = (totalStairsUsed + (MaxItemsPerStairBuffer - 1)) / MaxItemsPerStairBuffer;

            for (int i = 0; i < totalStairBuffersUsed; ++i)
            {
                ++planeCount;

                int count = (totalStairsUsed - (i * MaxItemsPerStairBuffer));

                if (count > MaxItemsPerStairBuffer)
                    count = MaxItemsPerStairBuffer;

                int size = count * 5;

                byte[] inflatedBuffer = m_StairBuffers[i];

                int deflatedLength = m_DeflatedBuffer.Length;
                ZLibError ce = ZLib.compress2(m_DeflatedBuffer, ref deflatedLength, inflatedBuffer, size,
                    ZLibCompressionLevel.Z_DEFAULT_COMPRESSION);

                if (ce != ZLibError.Z_OK)
                {
                    Console.WriteLine("ZLib error: {0} (#{1})", ce, (int) ce);
                    deflatedLength = 0;
                    size = 0;
                }

                Write((byte) (9 + i));
                Write((byte) size);
                Write((byte) deflatedLength);
                Write((byte) (((size >> 4) & 0xF0) | ((deflatedLength >> 8) & 0xF)));
                Write(m_DeflatedBuffer, 0, deflatedLength);

                totalLength += 4 + deflatedLength;
            }

            Seek(15, System.IO.SeekOrigin.Begin);

            Write((short) totalLength); // Buffer length
            Write((byte) planeCount); // Plane count

            /*int planes = (tiles.Length + (MaxItemsPerPlane - 1)) / MaxItemsPerPlane;

            if ( planes > 255 )
                 planes = 255;

            int totalLength = 0;

            Write( (byte) planes );
            ++totalLength;

            int itemIndex = 0;

            for ( int i = 0; i < planes; ++i )
            {
                 int byteIndex = 0;

                 for ( int j = 0; j < MaxItemsPerPlane && itemIndex < tiles.Length; ++j, ++itemIndex )
                 {
                      MultiTileEntry e = tiles[itemIndex];

                      m_InflatedBuffer[byteIndex++] = (byte)((e.m_ItemID >> 8) & 0x3F);
                      m_InflatedBuffer[byteIndex++] = (byte)e.m_ItemID;
                      m_InflatedBuffer[byteIndex++] = (byte)e.m_OffsetX;
                      m_InflatedBuffer[byteIndex++] = (byte)e.m_OffsetY;
                      m_InflatedBuffer[byteIndex++] = (byte)e.m_OffsetZ;
                 }

                 int deflatedLength = m_DeflatedBuffer.Length;
                 ZLibError ce = ZLib.compress2( m_DeflatedBuffer, ref deflatedLength, m_InflatedBuffer, byteIndex, ZLibCompressionLevel.Z_DEFAULT_COMPRESSION );

                 if ( ce != ZLibError.Z_OK )
                 {
                      Console.WriteLine( "ZLib error: {0} (#{1})", ce, (int)ce );
                      deflatedLength = 0;
                      byteIndex = 0;
                 }

                 Write( (byte) 0x00 );
                 Write( (byte) byteIndex );
                 Write( (byte) deflatedLength );
                 Write( (byte) (((byteIndex >> 4) & 0xF0) | ((deflatedLength >> 8) & 0xF)) );
                 Write( m_DeflatedBuffer, 0, deflatedLength );

                 totalLength += 4 + deflatedLength;
            }

            Seek( 15, System.IO.SeekOrigin.Begin );
            Write( (short) totalLength ); // Buffer length*/
        }
    }

    internal sealed class PromptResponse : Packet
    {
        internal PromptResponse(uint serial, uint promptid, uint operation, string lang, string text)
            : base(0xC2)
        {
            if (text != "")
                EnsureCapacity(2 + 4 + 4 + 4 + 4 + (text.Length * 2));
            else
            {
                EnsureCapacity(18);
            }

            Write((uint) serial);
            Write((uint) promptid);
            Write((uint) operation);

            if (string.IsNullOrEmpty(lang))
                lang = "ENU";

            WriteAsciiFixed(lang.ToUpper(), 4);

            if (text != "")
                WriteLittleUniNull(text);
        }
    }

    internal sealed class AssistVersion : Packet
    {
        internal AssistVersion()
            : base(0xBE)
        {
            var version = typeof(AssistVersion).Assembly.GetName().Version.ToString();

            EnsureCapacity(3 + version.Length);

            WriteAsciiNull(version);
        }
    }
}