﻿#region license

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
                ScriptManager.Error("Usage: findtype ('name of item') OR (graphicID) [inrangecheck (true/false)]");
                return 0;
            }

            string gfxStr = args[0].AsString();
            Serial gfx = Utility.ToUInt16(gfxStr, 0);

            bool inRangeCheck = false;
            ScriptVariables.ScriptVariable foundVar = ScriptVariables.GetVariable("found");

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
                            if (foundVar != null)
                            {
                                foundVar.TargetInfo = new TargetInfo
                                {
                                    Gfx = 0,
                                    Serial = item.Serial,
                                    Type = (byte)(item.OnGround ? 1 : 0),
                                    X = item.Position.X,
                                    Y = item.Position.Y,
                                    Z = item.Position.Z
                                };
                                Assistant.Engine.MainWindow.SaveScriptVariables();
                            }
                            return 1;
                        }
                    }
                    else
                    {
                        if (foundVar != null)
                        {
                            foundVar.TargetInfo = new TargetInfo
                            {
                                Gfx = 0,
                                Serial = item.Serial,
                                Type = (byte)(item.OnGround ? 1 : 0),
                                X = item.Position.X,
                                Y = item.Position.Y,
                                Z = item.Position.Z
                            };
                            Assistant.Engine.MainWindow.SaveScriptVariables();
                        }
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
                    {
                        if (foundVar != null)
                        {
                            foundVar.TargetInfo = new TargetInfo
                            {
                                Gfx = (ushort)gfx.Value,
                                Serial = i.Serial,
                                Type = (byte)(i.OnGround ? 1 : 0),
                                X = i.Position.X,
                                Y = i.Position.Y,
                                Z = i.Position.Z
                            };
                            Assistant.Engine.MainWindow.SaveScriptVariables();
                        }
                        return 1;
                    }
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
                        {
                            if (foundVar != null)
                            {
                                foundVar.TargetInfo = new TargetInfo
                                {
                                    Gfx = (ushort)gfx.Value,
                                    Serial = i.Serial,
                                    Type = (byte)(i.OnGround ? 1 : 0),
                                    X = i.Position.X,
                                    Y = i.Position.Y,
                                    Z = i.Position.Z
                                };
                                Assistant.Engine.MainWindow.SaveScriptVariables();
                            }
                            return 1;
                        }

                    }
                    else
                    {
                        if (foundVar != null)
                        {
                            foundVar.TargetInfo = new TargetInfo
                            {
                                Gfx = (ushort)gfx.Value,
                                Serial = i.Serial,
                                Type = (byte)(i.OnGround ? 1 : 0),
                                X = i.Position.X,
                                Y = i.Position.Y,
                                Z = i.Position.Z
                            };
                            Assistant.Engine.MainWindow.SaveScriptVariables();
                        }
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
                        {
                            if (foundVar != null)
                            {
                                foundVar.TargetInfo = new TargetInfo
                                {
                                    Gfx = (ushort)gfx.Value,
                                    Serial = i.Serial,
                                    Type = (byte)(i.OnGround ? 1 : 0),
                                    X = i.Position.X,
                                    Y = i.Position.Y,
                                    Z = i.Position.Z
                                };
                                Assistant.Engine.MainWindow.SaveScriptVariables();
                            }
                            return 1;
                        }

                    }
                    else
                    {
                        if (foundVar != null)
                        {
                            foundVar.TargetInfo = new TargetInfo
                            {
                                Gfx = (ushort)gfx.Value,
                                Serial = i.Serial,
                                Type = (byte)(i.OnGround ? 1 : 0),
                                X = i.Position.X,
                                Y = i.Position.Y,
                                Z = i.Position.Z
                            };
                            Assistant.Engine.MainWindow.SaveScriptVariables();
                        }
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
                        {
                            if (foundVar != null)
                            {
                                foundVar.TargetInfo = new TargetInfo
                                {
                                    Gfx = (ushort)gfx.Value,
                                    Serial = m.Serial,
                                    Type = 1,
                                    X = m.Position.X,
                                    Y = m.Position.Y,
                                    Z = m.Position.Z
                                };
                                Assistant.Engine.MainWindow.SaveScriptVariables();
                            }
                        }
                        return 1;
                    }
                    else
                    {
                        if (foundVar != null)
                        {
                            foundVar.TargetInfo = new TargetInfo
                            {
                                Gfx = (ushort)gfx.Value,
                                Serial = m.Serial,
                                Type = 1,
                                X = m.Position.X,
                                Y = m.Position.Y,
                                Z = m.Position.Z
                            };
                            Assistant.Engine.MainWindow.SaveScriptVariables();
                        }
                        return 1;
                    }
                }
            }

            return 0;
        }

        private static double Mounted(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.Mount) != null
                ? 1
                : 0;
        }

        private static double RHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.RightHand) != null
                ? 1
                : 0;
        }

        private static double LHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.GetItemOnLayer(Layer.LeftHand) != null
                ? 1
                : 0;
        }

        private static double Dead(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.IsGhost
                ? 1
                : 0;
        }

        private static double InSysMessage(string expression, Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                ScriptManager.Error("Usage: insysmsg ('text')");
                return 0;
            }

            string text = args[0].AsString();

            for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
            {
                string sys = PacketHandlers.SysMessages[i];

                if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    PacketHandlers.SysMessages.RemoveRange(0, i + 1);
                    return 1;
                }
            }

            return 0;
        }

        private static double Mana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Mana;
        }

        private static double MaxMana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.ManaMax;
        }

        private static double Poisoned(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned) &&
                   World.Player.Poisoned
                ? 1
                : 0;
        }

        private static double Hp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Hits;
        }

        private static double MaxHp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.HitsMax;
        }

        private static double Stam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Stam;
        }

        private static double MaxStam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.StamMax;
        }

        private static double Str(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Str;
        }

        private static double Dex(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Dex;
        }

        private static double Int(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Int;
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

        private static double CountExpression(string expression, Argument[] args, bool quiet)
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