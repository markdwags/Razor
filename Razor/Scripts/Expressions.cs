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
using Assistant.Scripts.Engine;
using Ultima;

namespace Assistant.Scripts
{
    public static class Expressions
    {
        public static void Register()
        {
            Interpreter.RegisterExpressionHandler("stam", Stam);
            Interpreter.RegisterExpressionHandler("maxstam", MaxStam);
            Interpreter.RegisterExpressionHandler("hp", Hp);
            Interpreter.RegisterExpressionHandler("hits", Hp);
            Interpreter.RegisterExpressionHandler("maxhp", MaxHp);
            Interpreter.RegisterExpressionHandler("maxhits", MaxHp);
            Interpreter.RegisterExpressionHandler("mana", Mana);
            Interpreter.RegisterExpressionHandler("maxmana", MaxMana);
            Interpreter.RegisterExpressionHandler("poisoned", Poisoned);

            Interpreter.RegisterExpressionHandler("mounted", Mounted);
            Interpreter.RegisterExpressionHandler("rhandempty", RHandEmpty);
            Interpreter.RegisterExpressionHandler("lhandempty", LHandEmpty);

            Interpreter.RegisterExpressionHandler("dead", Dead);

            Interpreter.RegisterExpressionHandler("str", Str);
            Interpreter.RegisterExpressionHandler("int", Int);
            Interpreter.RegisterExpressionHandler("dex", Dex);

            Interpreter.RegisterExpressionHandler("weight", Weight);

            Interpreter.RegisterExpressionHandler("skill", SkillExpression);
            Interpreter.RegisterExpressionHandler("count", CountExpression);
            Interpreter.RegisterExpressionHandler("counter", CountExpression);

            Interpreter.RegisterExpressionHandler("insysmsg", InSysMessage);
            Interpreter.RegisterExpressionHandler("insysmessage", InSysMessage);

            Interpreter.RegisterExpressionHandler("findtype", FindType);
        }

        private static double FindType(string expression, Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: findtype ('name of item') OR (graphicID) [inrangecheck (true/false)]");
            }

            string gfxStr = args[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);

            bool inRangeCheck = false;

            if (args.Length == 2)
            {
                inRangeCheck = args[1].AsBool();
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                foreach (Item item in World.FindItemsByName(gfxStr))
                {
                    if (inRangeCheck)
                    {
                        if (Utility.InRange(World.Player.Position, item.Position, 2))
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else // Check backpack first
            {
                if (World.Player.Backpack != null)
                {
                    Item i = World.Player.Backpack.FindItemByID(Utility.ToUInt16(gfxStr, 0));

                    if (i != null)
                        return 1;
                }
            }

            // Not in backpack? Lets check the world
            foreach (Item i in World.Items.Values)
            {
                if (i.ItemID == gfx && i.RootContainer == null)
                {
                    if (inRangeCheck)
                    {
                        if (Utility.InRange(World.Player.Position, i.Position, 2))
                            return 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            foreach (Item i in World.Items.Values)
            {
                if (i.ItemID == gfx && !i.IsInBank)
                {
                    if (inRangeCheck)
                    {
                        if (Utility.InRange(World.Player.Position, i.Position, 2))
                            return 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            foreach (Mobile m in World.MobilesInRange())
            {
                if (m.Body == gfx)
                {
                    if (inRangeCheck)
                    {
                        if (Utility.InRange(World.Player.Position, m.Position, 2))
                            return 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        private static bool Mounted(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.Mount) != null;
        }

        private static bool RHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.RightHand) == null;
        }

        private static bool LHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.LeftHand) == null;
        }

        private static bool Dead(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.IsGhost;
        }

        private static bool InSysMessage(string expression, Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: insysmsg ('text')");
            }

            string text = args[0].AsString();

            for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
            {
                string sys = PacketHandlers.SysMessages[i];

                if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    PacketHandlers.SysMessages.RemoveRange(0, i + 1);
                    return true;
                }
            }

            return false;
        }

        private static int Mana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Mana;
        }

        private static int MaxMana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.ManaMax;
        }

        private static bool Poisoned(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned) &&
                   World.Player.Poisoned;
        }

        private static int Hp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Hits;
        }

        private static int MaxHp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.HitsMax;
        }

        private static int Stam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Stam;
        }

        private static int MaxStam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.StamMax;
        }

        private static int Str(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Str;
        }

        private static int Dex(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Dex;
        }

        private static int Int(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Int;
        }

        private static int Weight(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Weight;
        }

        private static double SkillExpression(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                throw new ArgumentException("Usage: skill ('name of skill')");

            if (World.Player == null)
                return 0;

            foreach (SkillInfo skill in Skills.SkillEntries)
            {
                if (skill.Name.IndexOf(args[0].AsString(), StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    return World.Player.Skills[skill.Index].Value;
                }
            }

            return 0;
        }

        private static int CountExpression(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                throw new ArgumentException("Usage: count ('name of counter item')");

            if (World.Player == null)
                return 0;

            foreach (Counter c in Counter.List)
            {
                if (c.Name.Equals(args[0].AsString(), StringComparison.OrdinalIgnoreCase))
                {
                    return c.Enabled ? c.Amount : 0;
                }
            }

            return 0;
        }
    }
}