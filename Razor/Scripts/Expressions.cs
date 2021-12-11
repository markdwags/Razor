#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2021 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.Collections.Generic;
using Assistant.Core;
using Assistant.Scripts.Engine;
using Assistant.Scripts.Helpers;
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
            Interpreter.RegisterExpressionHandler("hidden", Hidden);

            Interpreter.RegisterExpressionHandler("mounted", Mounted);
            Interpreter.RegisterExpressionHandler("rhandempty", RHandEmpty);
            Interpreter.RegisterExpressionHandler("lhandempty", LHandEmpty);

            Interpreter.RegisterExpressionHandler("dead", Dead);

            Interpreter.RegisterExpressionHandler("str", Str);
            Interpreter.RegisterExpressionHandler("int", Int);
            Interpreter.RegisterExpressionHandler("dex", Dex);

            Interpreter.RegisterExpressionHandler("weight", Weight);
            Interpreter.RegisterExpressionHandler("maxweight", MaxWeight);

            Interpreter.RegisterExpressionHandler("skill", SkillExpression);
            Interpreter.RegisterExpressionHandler("count", CountExpression);
            Interpreter.RegisterExpressionHandler("counter", CountExpression);

            Interpreter.RegisterExpressionHandler("insysmsg", InSysMessage);
            Interpreter.RegisterExpressionHandler("insysmessage", InSysMessage);

            Interpreter.RegisterExpressionHandler("findtype", FindType);

            Interpreter.RegisterExpressionHandler("findbuff", FindBuffDebuff);
            Interpreter.RegisterExpressionHandler("finddebuff", FindBuffDebuff);

            Interpreter.RegisterExpressionHandler("position", Position);

            Interpreter.RegisterExpressionHandler("queued", Queued);

            Interpreter.RegisterExpressionHandler("varexist", VarExist);
            Interpreter.RegisterExpressionHandler("varexists", VarExist);
        }

        private static bool VarExist(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length != 1)
            {
                throw new RunTimeError("Usage: varexist ('name')");
            }

            string varName = vars[0].AsString(false);
            
            return Interpreter.AliasHandlerExist(varName);
        }

        private static bool Queued(string expression, Variable[] vars, bool quiet, bool force)
        {
            return !ActionQueue.Empty;
        }

        private static bool FindBuffDebuff(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: findbuff/finddebuff ('name of buff')");
            }

            string name = vars[0].AsString();

            foreach (BuffDebuff buff in World.Player.BuffsDebuffs)
            {
                if (buff.ClilocMessage1.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private static uint FindType(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: findtype ('name of item') OR (graphicID) [inrangecheck (true/false)/backpack]");
            }

            string gfxStr = vars[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);
            List<Item> items;
            List<Mobile> mobiles;

            bool inRangeCheck = false;
            bool backpack = false;

            if (vars.Length == 2)
            {
                if (vars[1].AsString().IndexOf("pack", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    backpack = true;
                }
                else
                {
                    inRangeCheck = vars[1].AsBool();
                }
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                items = CommandHelper.GetItemsByName(gfxStr, backpack, inRangeCheck);

                if (items.Count == 0) // no item found, search mobile by name
                {
                    mobiles = CommandHelper.GetMobilesByName(gfxStr, inRangeCheck);

                    if (mobiles.Count > 0)
                    {
                        return mobiles[Utility.Random(mobiles.Count)].Serial;
                    }
                }
                else
                {
                    return items[Utility.Random(items.Count)].Serial;
                }
            }
            else // Provided graphic id for type, check backpack first (same behavior as DoubleClickAction in macros
            {
                ushort id = Utility.ToUInt16(gfxStr, 0);

                items = CommandHelper.GetItemsById(id, backpack, inRangeCheck);

                // Still no item? Mobile check!
                if (items.Count == 0)
                {
                    mobiles = CommandHelper.GetMobilesById(id, inRangeCheck);

                    if (mobiles.Count > 0)
                    {
                        return mobiles[Utility.Random(mobiles.Count)].Serial;
                    }
                }
                else
                {
                    return items[Utility.Random(items.Count)].Serial;
                }
            }

            return Serial.Zero;
        }

        private static bool Mounted(string expression, Variable[] vars, bool quiet, bool force)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.Mount) != null;
        }

        private static bool RHandEmpty(string expression, Variable[] vars, bool quiet, bool force)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.RightHand) == null;
        }

        private static bool LHandEmpty(string expression, Variable[] vars, bool quiet, bool force)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.LeftHand) == null;
        }

        private static bool Dead(string expression, Variable[] vars, bool quiet, bool force)
        {
            return World.Player != null && World.Player.IsGhost;
        }

        private static bool InSysMessage(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length == 0)
            {
                throw new RunTimeError("Usage: insysmsg ('text')");
            }

            string text = vars[0].AsString();

            return SystemMessages.Exists(text);
        }

        private static int Mana(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Mana;
        }

        private static int MaxMana(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.ManaMax;
        }

        private static bool Poisoned(string expression, Variable[] vars, bool quiet, bool force)
        {
            return World.Player != null && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned) &&
                   World.Player.Poisoned;
        }

        private static bool Hidden(string expression, Variable[] vars, bool quiet, bool force)
        {
            return World.Player != null && !World.Player.Visible;
        }

        private static int Hp(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Hits;
        }

        private static int MaxHp(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.HitsMax;
        }

        private static int Stam(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Stam;
        }

        private static int MaxStam(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.StamMax;
        }

        private static int Str(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Str;
        }

        private static int Dex(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Dex;
        }

        private static int Int(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Int;
        }

        private static int Weight(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Weight;
        }

        private static int MaxWeight(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return 0;

            return World.Player.MaxWeight;
        }

        private static double SkillExpression(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
                throw new RunTimeError("Usage: skill ('name of skill')");

            if (World.Player == null)
                return 0;

            foreach (SkillInfo skill in Skills.SkillEntries)
            {
                if (skill.Name.IndexOf(vars[0].AsString(), StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return World.Player.Skills[skill.Index].Value;
                }
            }

            return 0;
        }

        private static int CountExpression(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (vars.Length < 1)
                throw new RunTimeError("Usage: count ('name of counter') OR count ('name of item' OR graphicID) [hue]");

            if (World.Player == null)
                return 0;

            var counter = Counter.FindCounter(vars[0].AsString());
            if (counter != null)
            {
                return counter.Amount;
            }

            string gfxStr = vars[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);
            ushort hue = 0xFFFF;

            if (vars.Length == 2)
            {
                hue = Utility.ToUInt16(vars[1].AsString(), 0xFFFF);
            }

            // No graphic id, maybe searching by name?
            if (gfx == 0)
            {
                var items = CommandHelper.GetItemsByName(gfxStr, true, false);
                if (items.Count == 0) // no item found
                {
                    return 0;
                }
                else
                {
                    return Counter.GetCount(items[0].ItemID, hue);
                }
            }

            return Counter.GetCount(new ItemID((ushort)gfx.Value), hue);
        }

        private static bool Position(string expression, Variable[] vars, bool quiet, bool force)
        {
            if (World.Player == null)
                return false;

            if (vars.Length < 2)
            {
                throw new RunTimeError("Usage: position (x, y) or position (x, y, z)");
            } 

            int x = vars[0].AsInt();
            int y = vars[1].AsInt();
            int z = (vars.Length > 2) 
                ? vars[2].AsInt() 
                : World.Player.Position.Z;

            return World.Player.Position.X == x
                && World.Player.Position.Y == y
                && World.Player.Position.Z == z;
        }
    }
}