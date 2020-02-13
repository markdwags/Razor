using UOSteam;

namespace Assistant.Scripts
{
    public class Aliases
    {
        public static void Register()
        {
            Interpreter.RegisterAliasHandler("backpack", Backpack);
            Interpreter.RegisterAliasHandler("last", Last);
            Interpreter.RegisterAliasHandler("lasttarget", Last);
            Interpreter.RegisterAliasHandler("lastobject", LastObject);
            Interpreter.RegisterAliasHandler("self", Self);


            Interpreter.RegisterAliasHandler("stam", Stam);
            Interpreter.RegisterAliasHandler("stamina", Stam);
            Interpreter.RegisterAliasHandler("maxstam", MaxStam);
            Interpreter.RegisterAliasHandler("health", Hp);
            Interpreter.RegisterAliasHandler("hits", Hp);
            Interpreter.RegisterAliasHandler("hp", Hp);
            Interpreter.RegisterAliasHandler("maxhealth", MaxHp);
            Interpreter.RegisterAliasHandler("maxhits", MaxHp);
            Interpreter.RegisterAliasHandler("maxhp", MaxHp);
            Interpreter.RegisterAliasHandler("mana", Mana);
            Interpreter.RegisterAliasHandler("maxmana", MaxMana);

            Interpreter.RegisterAliasHandler("str", Str);
            Interpreter.RegisterAliasHandler("int", Int);
            Interpreter.RegisterAliasHandler("dex", Dex);

            Interpreter.RegisterAliasHandler("poisoned", Poisoned);
            Interpreter.RegisterAliasHandler("weight", Weight);
            Interpreter.RegisterAliasHandler("mount", Mounted);
            Interpreter.RegisterAliasHandler("righthand", RHandEmpty);
            Interpreter.RegisterAliasHandler("lefthand", LHandEmpty);
        }

        private static uint Dummy(string alias)
        {
            // not sure how to return the serial of the ground at your current position
            return uint.MaxValue;
        }

        private static uint Stam(string alias)
        {
            return World.Player == null ? (uint) 0 : World.Player.Stam;
        }

        private static uint MaxStam(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.StamMax;
        }

        private static uint Hp(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.Hits;
        }

        private static uint MaxHp(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.HitsMax;
        }

        private static uint Mana(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.Mana;
        }

        private static uint MaxMana(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.ManaMax;
        }

        private static uint Str(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.Str;
        }

        private static uint Dex(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.Dex;
        }

        private static uint Int(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.Int;
        }

        private static uint Poisoned(string alias)
        {
            return World.Player != null && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned) &&
                   World.Player.Poisoned
                ? (uint) 1
                : (uint) 0;
        }

        private static uint Weight(string alias)
        {
            return World.Player == null ? (uint)0 : World.Player.Weight;
        }

        private static uint Mounted(string alias)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.Mount) != null
                ? (uint) 1
                : (uint) 0;
        }

        private static uint RHandEmpty(string alias)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.RightHand) != null
                ? (uint)1
                : (uint)0;
        }

        private static uint LHandEmpty(string alias)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.LeftHand) != null
                ? (uint)1
                : (uint)0;
        }

        private static uint Backpack(string alias)
        {
            if (World.Player == null || World.Player.Backpack == null)
                return 0;

            return World.Player.Backpack.Serial;
        }

        private static uint Ground(string alias)
        {
            // not sure how to return the serial of the ground at your current position
            return uint.MaxValue;
        }

        private static uint Last(string alias)
        {
            /*if (Targeting.LastTargetInfo == null)
                return 0;

            return Targeting.LastTargetInfo.Serial;*/

            if (!Targeting.DoLastTarget()) //Targeting.LastTarget( true );
                Targeting.ResendTarget();

            return 0;
        }

        private static uint LastObject(string alias)
        {
            if (World.Player.LastObject != null)
                return World.Player.LastObject;

            return 0;
        }

        private static uint Self(string alias)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Serial;
        }
    }
}