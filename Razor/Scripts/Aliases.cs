using UOSteam;

namespace Assistant.Scripts
{
    public class Aliases
    {
         public static void Register()
        {
            Interpreter.RegisterAliasHandler("backpack", Backpack);
            Interpreter.RegisterAliasHandler("bank", Bank);
            Interpreter.RegisterAliasHandler("enemy", Enemy);
            Interpreter.RegisterAliasHandler("last", Last);
            Interpreter.RegisterAliasHandler("lasttarget", Last);
            Interpreter.RegisterAliasHandler("lastobject", LastObject);
            Interpreter.RegisterAliasHandler("lefthand", LeftHand);
            Interpreter.RegisterAliasHandler("mount", Mount);
            Interpreter.RegisterAliasHandler("righthand", RightHand);
            Interpreter.RegisterAliasHandler("self", Self);
        }

        private static uint Backpack(string alias)
        {
            if (World.Player == null || World.Player.Backpack == null)
                return 0;

            return World.Player.Backpack.Serial;
        }

        private static uint Bank(string alias)
        {
            // unsupported?  I can't find a reference to the bankbox in the player
            return uint.MaxValue;
        }

        private static uint Enemy(string alias)
        {
            // we will need to modify the PlayerData class to keep track of the current enemy to make this work
            return uint.MaxValue;
        }

        private static uint Friend(string alias)
        {
            // we will need to modify the PlayerData class to keep track of the current enemy to make this work
            return uint.MaxValue;
        }

        private static uint Ground(string alias)
        {
            // not sure how to return the serial of the ground at your current position
            return uint.MaxValue;
        }

        private static uint Last(string alias)
        {
            if (Targeting.LastTargetInfo == null)
                return 0;

            return Targeting.LastTargetInfo.Serial;
        }

        private static uint LastObject(string alias)
        {
            if (World.Player.LastObject != null)
                return World.Player.LastObject;

            return 0;
        }

        private static uint LeftHand(string alias)
        {
            if (World.Player == null)
                return 0;

            Item i = World.Player.GetItemOnLayer(Layer.LeftHand);

            if (i == null)
                return 0;

            return i.Serial;
        }

        private static uint Mount(string alias)
        {
            // not sure how to support this
            return uint.MaxValue;
        }

        private static uint RightHand(string alias)
        {
            if (World.Player == null)
                return 0;

            Item i = World.Player.GetItemOnLayer(Layer.RightHand);

            if (i == null)
                return 0;

            return i.Serial;
        }

        private static uint Self(string alias)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Serial;
        }
    }
}
