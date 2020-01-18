using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UOSteam;

namespace Assistant.Macros.Scripts
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

        private static int Backpack(ref ASTNode node)
        {
            if (World.Player == null || World.Player.Backpack == null)
                return 0;

            return World.Player.Backpack.Serial;
        }

        private static int Bank(ref ASTNode node)
        {
            // unsupported?  I can't find a reference to the bankbox in the player
            return -1;
        }

        private static int Enemy(ref ASTNode node)
        {
            // we will need to modify the PlayerData class to keep track of the current enemy to make this work
            return -1;
        }

        private static int Friend(ref ASTNode node)
        {
            // we will need to modify the PlayerData class to keep track of the current enemy to make this work
            return -1;
        }

        private static int Ground(ref ASTNode node)
        {
            // not sure how to return the serial of the ground at your current position
            return -1;
        }

        private static int Last(ref ASTNode node)
        {
            if (Targeting.LastTargetInfo == null)
                return 0;

            return Targeting.LastTargetInfo.Serial;
        }

        private static int LastObject(ref ASTNode node)
        {
            if (World.Player.LastObject != null)
                return World.Player.LastObject;

            return 0;
        }

        private static int LeftHand(ref ASTNode node)
        {
            if (World.Player == null)
                return 0;

            Item i = World.Player.GetItemOnLayer(Layer.LeftHand);

            if (i == null)
                return 0;

            return i.Serial;
        }

        private static int Mount(ref ASTNode node)
        {
            // not sure how to support this
            return -1;
        }

        private static int RightHand(ref ASTNode node)
        {
            if (World.Player == null)
                return 0;

            Item i = World.Player.GetItemOnLayer(Layer.RightHand);

            if (i == null)
                return 0;

            return i.Serial;
        }

        private static int Self(ref ASTNode node)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Serial;
        }
    }
}
