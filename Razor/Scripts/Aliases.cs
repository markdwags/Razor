using Assistant.Scripts.Engine;

namespace Assistant.Scripts
{
    public static class Aliases
    {
        public static void Register()
        {
            Interpreter.RegisterAliasHandler("backpack", Backpack);
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
            if (World.Player == null)
                return 0;

            Item i = World.Player.GetItemOnLayer(Layer.Mount);

            if (i == null)
                return 0;

            return i.Serial;
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