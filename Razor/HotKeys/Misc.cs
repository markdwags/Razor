using System;
using Assistant;
using Assistant.Core;

namespace Assistant.HotKeys
{
    public class UseHotKeys
    {
        public static void Initialize()
        {
            HotKey.Add(HKCategory.Misc, LocString.Resync, new HotKeyCallback(Resync));

            HotKey.Add(HKCategory.Misc, LocString.GoldPerHotkey, new HotKeyCallback(ToggleGoldPer));
            HotKey.Add(HKCategory.Misc, LocString.DamageTracker, new HotKeyCallback(ToggleDamage));

            HotKey.Add(HKCategory.Misc, LocString.CaptureBod, new HotKeyCallback(CaptureBod));

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

            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllCome, new HotKeyCallback(PetAllCome));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllFollowMe, new HotKeyCallback(PetAllFollowMe));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllFollow, new HotKeyCallback(PetAllFollow));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllGuardMe, new HotKeyCallback(PetAllGuardMe));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllGuard, new HotKeyCallback(PetAllGuard));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllKill, new HotKeyCallback(PetAllKill));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllStay, new HotKeyCallback(PetAllStay));
            HotKey.Add(HKCategory.Misc, HKSubCat.PetCommands, LocString.AllStop, new HotKeyCallback(PetAllStop));

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

        private static void ToggleGoldPer()
        {
            if (GoldPerHourTimer.Running)
            {
                World.Player.SendMessage(MsgLevel.Force, "Stopping 'GoldPer Timer'");
                GoldPerHourTimer.Stop();
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Force, "Starting 'GoldPer Timer' when you loot your first gold");
                GoldPerHourTimer.Start();
            }
        }

        private static void ToggleDamage()
        {
            Engine.MainWindow.ToggleDamageTracker(!DamageTracker.Running);

        }

        private enum PetCommands
        {
            AllCome,
            AllFollowMe,
            AllFollow,
            AllGuardMe,
            AllGuard,
            AllKill,
            AllStay,
            AllStop
        }

        private static void PetAllCome()
        {
            World.Player.Say(Language.GetString(LocString.AllCome));
        }

        private static void PetAllFollowMe()
        {
            World.Player.Say(Language.GetString(LocString.AllFollowMe));
        }
        private static void PetAllFollow()
        {
            World.Player.Say(Language.GetString(LocString.AllFollow));
        }
        private static void PetAllGuardMe()
        {
            World.Player.Say(Language.GetString(LocString.AllGuardMe));
        }
        private static void PetAllGuard()
        {
            World.Player.Say(Language.GetString(LocString.AllGuard));
        }
        private static void PetAllKill()
        {
            World.Player.Say(Language.GetString(LocString.AllKill));
        }
        private static void PetAllStay()
        {
            World.Player.Say(Language.GetString(LocString.AllStay));
        }
        private static void PetAllStop()
        {
            World.Player.Say(Language.GetString(LocString.AllStop));
        }
       
        private static void CaptureBod()
        {
            try
            {
                if (BodCapture.IsBodGump(World.Player.CurrentGumpI))
                {
                    BodCapture.CaptureBod(World.Player.CurrentGumpStrings);

                    World.Player.SendMessage(MsgLevel.Force, "BOD has been captured and saved to BODs.csv");
                }
                else
                {
                    World.Player.SendMessage(MsgLevel.Force, "The last gump you had open doesn't appear to be a BOD");
                }
            }
            catch
            {
                World.Player.SendMessage(MsgLevel.Force, "Unable to capture BOD, probably unknown format");
            }

            
        }


        private static void PartyAccept()
        {
            if (PacketHandlers.PartyLeader != Serial.Zero)
            {
                Client.Instance.SendToServer(new AcceptParty(PacketHandlers.PartyLeader));
                PacketHandlers.PartyLeader = Serial.Zero;
            }
        }

        private static void PartyDecline()
        {
            if (PacketHandlers.PartyLeader != Serial.Zero)
            {
                Client.Instance.SendToServer(new DeclineParty(PacketHandlers.PartyLeader));
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
                    Client.Instance.SendToServer(new SingleClick(m));

                if (textFlags)
                    Targeting.CheckTextFlags(m);
            }

            foreach (Item i in World.Items.Values)
            {
                if (i.IsCorpse)
                    Client.Instance.SendToServer(new SingleClick(i));
            }
        }

        private static void AllCorpses()
        {
            foreach (Item i in World.Items.Values)
            {
                if (i.IsCorpse)
                    Client.Instance.SendToServer(new SingleClick(i));
            }
        }

        private static void AllMobiles()
        {
            bool textFlags = Config.GetBool("LastTargTextFlags");

            foreach (Mobile m in World.MobilesInRange())
            {
                if (m != World.Player)
                    Client.Instance.SendToServer(new SingleClick(m));

                if (textFlags)
                    Targeting.CheckTextFlags(m);
            }
        }

        private static void LastSkill()
        {
            if (World.Player != null && World.Player.LastSkill != -1)
                Client.Instance.SendToServer(new UseSkill(World.Player.LastSkill));
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

                Client.Instance.SendToServer(new ResyncReq());
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
                Platform.AllowBit(FeatureBit.BlockHealPoisoned))
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
            if (!Platform.AllowBit(FeatureBit.PotionHotkeys))
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