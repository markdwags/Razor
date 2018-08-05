using System;
using Assistant;

namespace Assistant.HotKeys
{
    public class UseHotKeys
    {
        public static void Initialize()
        {
            HotKey.Add(HKCategory.Misc, LocString.Resync, new HotKeyCallback(Resync));
            HotKey.Add(HKCategory.Misc, LocString.RecStart, new HotKeyCallback(PacketPlayer.Record));
            HotKey.Add(HKCategory.Misc, LocString.VidStop, new HotKeyCallback(PacketPlayer.Stop));

            HotKey.Add(HKCategory.Misc, LocString.ClearDragDropQueue, new HotKeyCallback(DragDropManager.GracefulStop));

            HotKey.Add(HKCategory.Misc, LocString.LastSpell, new HotKeyCallback(LastSpell));
            HotKey.Add(HKCategory.Misc, LocString.LastSkill, new HotKeyCallback(LastSkill));
            HotKey.Add(HKCategory.Misc, LocString.LastObj, new HotKeyCallback(LastObj));
            HotKey.Add(HKCategory.Misc, LocString.AllNames, new HotKeyCallback(AllNames));
            HotKey.Add(HKCategory.Misc, LocString.AllCorpses, new HotKeyCallback(AllCorpses));
            HotKey.Add(HKCategory.Misc, LocString.AllMobiles, new HotKeyCallback(AllMobiles));
            HotKey.Add(HKCategory.Misc, LocString.Dismount, new HotKeyCallback(Dismount));

            HotKey.Add(HKCategory.Items, LocString.BandageSelf, new HotKeyCallback(BandageSelf));
            HotKey.Add(HKCategory.Items, LocString.BandageLT, new HotKeyCallback(BandageLastTarg));
            HotKey.Add(HKCategory.Items, LocString.UseHand, new HotKeyCallback(UseItemInHand));

            HotKey.Add(HKCategory.Misc, LocString.PartyAccept, new HotKeyCallback(PartyAccept));
            HotKey.Add(HKCategory.Misc, LocString.PartyDecline, new HotKeyCallback(PartyDecline));

            HotKeyCallbackState call = new HotKeyCallbackState(OnUseItem);
            HotKey.Add(HKCategory.Items, LocString.UseBandage, call, (ushort) 3617);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkHeal, call, (ushort) 3852);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkCure, call, (ushort) 3847);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkRef, call, (ushort) 3851);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkNS, call, (ushort) 3846);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkExp, call, (ushort) 3853);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkStr, call, (ushort) 3849);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkAg, call, (ushort) 3848);
            HotKey.Add(HKCategory.Items, HKSubCat.Potions, LocString.DrinkApple, new HotKeyCallback(OnDrinkApple));
        }

        private static void PartyAccept()
        {
            if (PacketHandlers.PartyLeader != Serial.Zero)
            {
                ClientCommunication.SendToServer(new AcceptParty(PacketHandlers.PartyLeader));
                PacketHandlers.PartyLeader = Serial.Zero;
            }
        }

        private static void PartyDecline()
        {
            if (PacketHandlers.PartyLeader != Serial.Zero)
            {
                ClientCommunication.SendToServer(new DeclineParty(PacketHandlers.PartyLeader));
                PacketHandlers.PartyLeader = Serial.Zero;
            }
        }

        private static void Dismount()
        {
            if (World.Player.GetItemOnLayer(Layer.Mount) != null)
                ActionQueue.DoubleClick(true, World.Player.Serial);
            else
                World.Player.SendMessage("You are not mounted.");
        }

        private static void AllNames()
        {
            bool textFlags = Config.GetBool("LastTargTextFlags");

            foreach (Mobile m in World.MobilesInRange())
            {
                if (m != World.Player)
                    ClientCommunication.SendToServer(new SingleClick(m));

                if (textFlags)
                    Targeting.CheckTextFlags(m);
            }

            foreach (Item i in World.Items.Values)
            {
                if (i.IsCorpse)
                    ClientCommunication.SendToServer(new SingleClick(i));
            }
        }

        private static void AllCorpses()
        {
            foreach (Item i in World.Items.Values)
            {
                if (i.IsCorpse)
                    ClientCommunication.SendToServer(new SingleClick(i));
            }
        }

        private static void AllMobiles()
        {
            bool textFlags = Config.GetBool("LastTargTextFlags");

            foreach (Mobile m in World.MobilesInRange())
            {
                if (m != World.Player)
                    ClientCommunication.SendToServer(new SingleClick(m));

                if (textFlags)
                    Targeting.CheckTextFlags(m);
            }
        }

        private static void LastSkill()
        {
            if (World.Player != null && World.Player.LastSkill != -1)
                ClientCommunication.SendToServer(new UseSkill(World.Player.LastSkill));
        }

        private static void LastObj()
        {
            if (World.Player != null && World.Player.LastObject != Serial.Zero)
                PlayerData.DoubleClick(World.Player.LastObject);
        }

        private static void LastSpell()
        {
            if (World.Player != null && World.Player.LastSpell != -1)
            {
                ushort id = (ushort) World.Player.LastSpell;
                object o = id;
                Spell.OnHotKey(ref o);
            }
        }

        private static DateTime m_LastSync;

        private static void Resync()
        {
            if (DateTime.UtcNow - m_LastSync > TimeSpan.FromSeconds(1.0))
            {
                m_LastSync = DateTime.UtcNow;

                //ClientCommunication.SendToClient( new MoveReject( World.Player.WalkSequence, World.Player ) );
                ClientCommunication.SendToClient(new MobileUpdate(World.Player));
                ClientCommunication.SendToServer(new ResyncReq());
                World.Player.Resync();
            }
        }

        public static void BandageLastTarg()
        {
            Item pack = World.Player.Backpack;
            if (pack != null)
            {
                if (!UseItem(pack, 3617))
                {
                    World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
                }
                else
                {
                    Targeting.LastTarget(true); //force a targetself to be queued
                    BandageTimer.Start();
                }
            }
        }

        public static void BandageSelf()
        {
            Item pack = World.Player.Backpack;
            if (pack != null)
            {
                if (!UseItem(pack, 3617))
                {
                    World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
                }
                else
                {
                    Targeting.ClearQueue();
                    Targeting.TargetSelf(true); //force a targetself to be queued
                    BandageTimer.Start();
                }
            }
        }

        private static bool DrinkApple(Item cont)
        {
            for (int i = 0; i < cont.Contains.Count; i++)
            {
                Item item = (Item) cont.Contains[i];

                if (item.ItemID == 12248 && item.Hue == 1160)
                {
                    PlayerData.DoubleClick(item);
                    return true;
                }
                else if (item.Contains != null && item.Contains.Count > 0)
                {
                    if (DrinkApple(item))
                        return true;
                }
            }

            return false;
        }

        private static void OnDrinkApple()
        {
            if (World.Player.Backpack == null)
                return;

            if (!DrinkApple(World.Player.Backpack))
                World.Player.SendMessage(LocString.NoItemOfType, (ItemID) 12248);
        }

        private static void OnUseItem(ref object state)
        {
            Item pack = World.Player.Backpack;
            if (pack == null)
                return;

            ushort id = (ushort) state;
            if (id == 3852 && World.Player.Poisoned && Config.GetBool("BlockHealPoison") &&
                ClientCommunication.AllowBit(FeatureBit.BlockHealPoisoned))
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.HealPoisonBlocked);
                return;
            }

            if (!UseItem(pack, id))
                World.Player.SendMessage(LocString.NoItemOfType, (ItemID) id);
        }

        private static void UseItemInHand()
        {
            Item item = World.Player.GetItemOnLayer(Layer.RightHand);
            if (item == null)
                item = World.Player.GetItemOnLayer(Layer.LeftHand);

            if (item != null)
                PlayerData.DoubleClick(item);
        }

        private static bool UseItem(Item cont, ushort find)
        {
            if (!ClientCommunication.AllowBit(FeatureBit.PotionHotkeys))
                return false;

            for (int i = 0; i < cont.Contains.Count; i++)
            {
                Item item = (Item) cont.Contains[i];

                if (item.ItemID == find)
                {
                    PlayerData.DoubleClick(item);
                    return true;
                }
                else if (item.Contains != null && item.Contains.Count > 0)
                {
                    if (UseItem(item, find))
                        return true;
                }
            }

            return false;
        }
    }
}