using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Assistant.Core;
using UOSteam;

namespace Assistant.Macros.Scripts
{
    public class Commands
    {
        private static List<ASTNode> ParseArguments(ref ASTNode node)
        {
            List<ASTNode> args = new List<ASTNode>();
            while (node != null)
            {
                args.Add(node);
                node = node.Next();
            }

            return args;
        }

        private static Serial GetSerial(ref ASTNode target)
        {
            Serial targetSerial = Serial.MinusOne;
            if (target.Type == ASTNodeType.STRING)
                targetSerial = (uint) Interpreter.GetAlias(ref target);
            else if (target.Type == ASTNodeType.SERIAL)
                targetSerial = Utility.ToUInt32(target.Lexeme, Serial.MinusOne);

            return targetSerial;
        }

        private static bool DummyCommand(ref ASTNode node, bool quiet, bool force)
        {
            Console.WriteLine("Executing command {0} {1}", node.Type, node.Lexeme);

            node = null;

            return true;
        }

        private static bool UseItem(Item cont, ushort find)
        {
            for (int i = 0; i < cont.Contains.Count; i++)
            {
                Item item = cont.Contains[i];

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

        public static void Register()
        {
            // Commands based on Actions.cs
            Interpreter.RegisterCommandHandler("cast", Cast); //BookcastAction, etc

            Interpreter.RegisterCommandHandler("dress", DressCommand); //DressAction
            Interpreter.RegisterCommandHandler("undress", UnDressCommand); //UndressAction
            Interpreter.RegisterCommandHandler("dressconfig", DressConfig);

            Interpreter.RegisterCommandHandler("target", Target); //Absolute Target
            Interpreter.RegisterCommandHandler("waitfortarget", WaitForTarget); //Absolute Target

            Interpreter.RegisterCommandHandler("menu", DummyCommand); //ContextMenuAction

            Interpreter.RegisterCommandHandler("dclicktype", UseType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("dclick", UseObject); //DoubleClickAction
            Interpreter.RegisterCommandHandler("usetype", UseType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("useobject", UseObject); //DoubleClickAction

            Interpreter.RegisterCommandHandler("drop", MoveItem); //DropAction
            Interpreter.RegisterCommandHandler("waitforgump", DummyCommand); // WaitForGumpAction
            Interpreter.RegisterCommandHandler("waitformenu", DummyCommand); // WaitForMenuAction
            Interpreter.RegisterCommandHandler("replygump", DummyCommand); // GumpResponseAction
            Interpreter.RegisterCommandHandler("closegump", DummyCommand);
            Interpreter.RegisterCommandHandler("hotkey", DummyCommand); //HotKeyAction
            //Interpreter.RegisterCommandHandler("lasttarget", DummyCommand); //LastTargetAction -- alias?
            Interpreter.RegisterCommandHandler("lift", MoveItem); //LiftAction
            Interpreter.RegisterCommandHandler("lifttype", MoveItem); //LiftTypeAction
            Interpreter.RegisterCommandHandler("say", Msg); //SpeechAction
            Interpreter.RegisterCommandHandler("msg", Msg); //SpeechAction
            Interpreter.RegisterCommandHandler("overhead", HeadMsg); //OverheadMessageAction
            Interpreter.RegisterCommandHandler("sysmsg", SysMsg); //SystemMessageAction
            Interpreter.RegisterCommandHandler("wait", Pause); //PauseAction
            Interpreter.RegisterCommandHandler("pause", Pause); //PauseAction
            Interpreter.RegisterCommandHandler("setability", SetAbility); //SetAbilityAction
            Interpreter.RegisterCommandHandler("setlasttarget", DummyCommand); //SetLastTargetAction
            Interpreter.RegisterCommandHandler("skill", UseSkill); //SkillAction
            Interpreter.RegisterCommandHandler("walk", Walk); //Move/WalkAction


            /*Interpreter.RegisterCommandHandler("attack", Attack);
            Interpreter.RegisterCommandHandler("clearhands", ClearHands);
            Interpreter.RegisterCommandHandler("bandageself", BandageSelf);
            Interpreter.RegisterCommandHandler("useonce", UseOnce);
            //Interpreter.RegisterCommandHandler("cleanusequeue", CleanUseQueue);
            /*Interpreter.RegisterCommandHandler("moveitem", MoveItem);
            Interpreter.RegisterCommandHandler("moveitemoffset", DummyCommand);
            Interpreter.RegisterCommandHandler("movetype", DummyCommand);
            Interpreter.RegisterCommandHandler("movetypeoffset", DummyCommand);#1#
            Interpreter.RegisterCommandHandler("turn", Turn);
            Interpreter.RegisterCommandHandler("useskill", UseSkill);
            Interpreter.RegisterCommandHandler("togglehands", ToggleHands);
            Interpreter.RegisterCommandHandler("equipitem", EquipItem);
            //Interpreter.RegisterCommandHandler("togglemounted", DummyCommand);
            //Interpreter.RegisterCommandHandler("equipwand", DummyCommand);
            //Interpreter.RegisterCommandHandler("buy", DummyCommand);
            //Interpreter.RegisterCommandHandler("sell", DummyCommand);
            //Interpreter.RegisterCommandHandler("clearbuy", DummyCommand);
            //Interpreter.RegisterCommandHandler("clearsell", DummyCommand);
            //Interpreter.RegisterCommandHandler("organizer", DummyCommand);
            //Interpreter.RegisterCommandHandler("dressconfig", DummyCommand);
            Interpreter.RegisterCommandHandler("togglescavenger", ToggleScavenger);
            //Interpreter.RegisterCommandHandler("counter", DummyCommand);
            Interpreter.RegisterCommandHandler("unsetalias", UnsetAlias);
            Interpreter.RegisterCommandHandler("setalias", SetAlias);
            Interpreter.RegisterCommandHandler("promptalias", DummyCommand);
            
            //Interpreter.RegisterCommandHandler("clearjournal", DummyCommand);
            //Interpreter.RegisterCommandHandler("waitforjournal", DummyCommand);
            //Interpreter.RegisterCommandHandler("poplist", DummyCommand);
            //Interpreter.RegisterCommandHandler("pushlist", DummyCommand);
            //Interpreter.RegisterCommandHandler("removelist", DummyCommand);
            //Interpreter.RegisterCommandHandler("createlist", DummyCommand);
            //Interpreter.RegisterCommandHandler("clearlist", DummyCommand);
            //Interpreter.RegisterCommandHandler("info", DummyCommand);
            Interpreter.RegisterCommandHandler("pause", Pause);
            //Interpreter.RegisterCommandHandler("ping", Ping);
            Interpreter.RegisterCommandHandler("playmacro", DummyCommand);
            Interpreter.RegisterCommandHandler("playsound", DummyCommand);
            Interpreter.RegisterCommandHandler("resync", Resync);
            Interpreter.RegisterCommandHandler("snapshot", DummyCommand);
            
            Interpreter.RegisterCommandHandler("where", DummyCommand);*/
            //Interpreter.RegisterCommandHandler("messagebox", MessageBox);
            //Interpreter.RegisterCommandHandler("mapuo", DummyCommand);
            //Interpreter.RegisterCommandHandler("clickscreen", DummyCommand);
            //Interpreter.RegisterCommandHandler("paperdoll", DummyCommand);
            //Interpreter.RegisterCommandHandler("helpbutton", DummyCommand);
            //Interpreter.RegisterCommandHandler("guildbutton", DummyCommand);
            //Interpreter.RegisterCommandHandler("questsbutton", DummyCommand);
            //Interpreter.RegisterCommandHandler("logoutbutton", DummyCommand);
            //Interpreter.RegisterCommandHandler("virtue", DummyCommand);
        }

        private static bool Target(ref ASTNode node, bool quiet, bool force)
        {
            List<ASTNode> args = ParseArguments(ref node);

            ASTNode target = args[1];
            Serial serial = Serial.Parse(target.Lexeme);

            if (serial != Serial.Zero) // Target a specific item or mobile
            {
                Item item = World.FindItem(serial);

                if (item != null)
                {
                    Targeting.Target(item);
                    return true;
                }

                Mobile mobile = World.FindMobile(serial);

                if (mobile != null)
                {
                    Targeting.Target(mobile);
                }
            }
            else if (args.Count == 6) // target a specific x/y/z
            {
                ASTNode x = args[2];
                ASTNode y = args[3];
                ASTNode z = args[4];
                ASTNode gfx = args[5];

                TargetInfo targetInfo = new TargetInfo
                {
                    Serial = Serial.Zero,
                    X = Utility.ToInt32(x.Lexeme, 0),
                    Y = Utility.ToInt32(y.Lexeme, 0),
                    Z = Utility.ToInt32(z.Lexeme, 0),
                    Gfx = Utility.ToUInt16(gfx.Lexeme, 0)
                };

                Targeting.Target(targetInfo);
            }

            

            return true;
        }

        private static bool WaitForTarget(ref ASTNode node, bool quiet, bool force)
        {
            List<ASTNode> args = ParseArguments(ref node);

            ASTNode waitTime = args[0];

            //Stopwatch sw = Stopwatch.StartNew();

            while (!Targeting.HasTarget)
            {
                return false;
            }

            //sw.Stop();

            return true;
        }

        private static string[] abilities = new string[4] {"primary", "secondary", "stun", "disarm"};

        private static bool SetAbility(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next(); // walk past COMMAND

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count < 1 || !abilities.Contains(args[0].Lexeme))
            {
                ScriptErrorMsg("Usage: setability ('primary'/'secondary'/'stun'/'disarm') ['on'/'off']");
                return true;
            }

            if (args.Count == 2 && args[1].Lexeme == "on" || args.Count == 1)
            {
                switch (args[0].Lexeme)
                {
                    case "primary":
                        SpecialMoves.SetPrimaryAbility();
                        break;
                    case "secondary":
                        SpecialMoves.SetSecondaryAbility();
                        break;
                    case "stun":
                        Client.Instance.SendToServer(new StunRequest());
                        break;
                    case "disarm":
                        Client.Instance.SendToServer(new DisarmRequest());
                        break;
                    default:
                        break;
                }
            }
            else if (args.Count == 2 && args[1].Lexeme == "off")
            {
                Client.Instance.SendToServer(new UseAbility(AOSAbility.Clear));
                Client.Instance.SendToClient(ClearAbility.Instance);
            }

            return true;
        }

        private static bool Attack(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            ParseArguments(ref node);

            return true;
        }

        private static string[] hands = new string[3] {"left", "right", "both"};

        private static bool ClearHands(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next(); // walk past COMMAND

            // expect one STRING node

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0 || !hands.Contains(args[0].Lexeme))
            {
                ScriptErrorMsg("Usage: clearhands ('left'/'right'/'both')");
                return true;
            }

            switch (args[0].Lexeme)
            {
                case "left":
                    Dress.Unequip(Layer.LeftHand);
                    break;
                case "right":
                    Dress.Unequip(Layer.RightHand);
                    break;
                default:
                    Dress.Unequip(Layer.LeftHand);
                    Dress.Unequip(Layer.RightHand);
                    break;
            }

            return true;
        }
        
        private static bool UseType(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            // expect a SERIAL containing graphic ID

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0)
                throw new ArgumentException("Usage: dclicktype|usetype (graphic)");

            ASTNode obj = args[0];
            int itemId = Utility.ToInt32(obj.Lexeme, -1);

            Item item = World.FindItemByType(itemId);

            if (item != null)
                Client.Instance.SendToServer(new DoubleClick(item.Serial));

            return true;
        }

        private static bool UseObject(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            // expect a SERIAL node

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: dclick|useobject (serial)");
                return true;
            }

            ASTNode obj = args[0];
            Serial serial = Serial.Parse(obj.Lexeme);

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: dclick|useobject (serial)");
                return true;
            }

            Client.Instance.SendToServer(new DoubleClick(serial));

            return true;
        }

        private static bool UseOnce(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            ParseArguments(ref node);

            return true;
        }

        private static bool MoveItem(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count < 2)
            {
                ScriptErrorMsg("Usage: moveitem (serial) (destination) [(x, y, z)] [amount]");
                return true;
            }

            ASTNode serialNode = args[0];
            ASTNode destinationNode = args[1];

            int serial = GetSerial(ref serialNode);
            int destination = GetSerial(ref destinationNode);

            if (args.Count == 2)
                DragDropManager.DragDrop(World.FindItem((uint) serial), World.FindItem((uint) destination));
            else if (args.Count == 5)
                return true;
            else if (args.Count == 6)
                return true;

            return true;
        }

        private static bool Walk(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            ParseArguments(ref node);

            return true;
        }

        private static Dictionary<string, int> UsableSkills = new Dictionary<string, int>()
        {
            {"anatomy", 1}, // anatomy
            {"animallore", 2}, // animal lore
            {"itemidentification", 3}, // item identification
            {"armslore", 4}, // arms lore
            {"begging", 6}, // begging
            {"peacemaking", 9}, // peacemaking
            {"cartography", 12}, // cartography
            {"detectinghidden", 14}, // detect hidden
            {"discordance", 15}, // Discordance
            {"evaluatingintelligence", 16}, // evaluate intelligence
            {"forensicevaluation", 19}, // forensic evaluation
            {"hiding", 21}, // hiding
            {"provocation", 22}, // provocation
            {"inscription", 23}, // inscription
            {"poisoning", 30}, // poisoning
            {"spiritspeak", 32}, // spirit speak
            {"stealing", 33}, // stealing
            {"taming", 35}, // taming
            {"tasteidentification", 36}, // taste id
            {"tracking", 38}, // tracking
            {"meditation", 46}, // Meditation
            {"stealth", 47}, // Stealth
            {"removetrap", 48} // RemoveTrap
        };

        private static bool UseSkill(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            // expect one string node or "last"

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: useskill ('skill name'/'last')");
                return true;
            }

            if (args[0].Lexeme == "last")
                Client.Instance.SendToServer(new UseSkill(World.Player.LastSkill));
            else if (UsableSkills.TryGetValue(node.Lexeme, out int skillId))
                Client.Instance.SendToServer(new UseSkill(skillId));

            return true;
        }

        private static bool SetAlias(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count != 2)
            {
                ScriptErrorMsg("Usage: setalias ('name') [serial]");
                return true;
            }

            ASTNode value = args[1]; // can't pass ref to this

            int serial = GetSerial(ref value);

            if (serial == Serial.MinusOne)
                return true;

            Interpreter.SetAlias(args[0].Lexeme, serial);

            return true;
        }

        private static bool UnsetAlias(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: unsetalias (string)");
                return true;
            }

            Interpreter.SetAlias(args[0].Lexeme, 0);

            return true;
        }

        public static bool EquipItem(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count < 2)
            {
                ScriptErrorMsg("Usage: equipitem (serial) (layer)");
                return true;
            }

            ASTNode item = args[0];

            Item equip = World.FindItem((uint) GetSerial(ref item));
            byte layer = (byte) Utility.ToInt32(args[1].Lexeme, 0);

            if (equip != null && (Layer) layer != Layer.Invalid)
                Dress.Equip(equip, (Layer) layer);

            return true;
        }

        private static bool Pause(ref ASTNode node, bool quiet, bool force)
        {
            return true;
        }

        public static bool Msg(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: msg ('text') [color]");
                return true;
            }

            if (!Client.Instance.ClientRunning)
                return true;

            if (args.Count == 1)
                World.Player.Say(Config.GetInt("SysColor"), args[0].Lexeme);
            else
                World.Player.Say(Utility.ToInt32(args[1].Lexeme, 0), args[0].Lexeme);

            return true;
        }

        public static bool Cast(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (!Client.Instance.ClientRunning)
                return true;

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: cast 'spell' [serial]");
                return true;
            }

            Spell spell;

            if (int.TryParse(args[0].Lexeme, out int spellnum))
                spell = Spell.Get(spellnum);
            else
                spell = Spell.GetByName(args[0].Lexeme);
            if (spell != null)
            {
                if (args.Count > 1)
                {
                    ASTNode n = args[1];
                    Serial s = GetSerial(ref n);
                    if (force)
                        Targeting.ClearQueue();
                    if (s > Serial.Zero && s != Serial.MinusOne)
                    {
                        spell.OnCast(new CastSpellFromMacro((ushort)spell.GetID()));
                        Targeting.Target(s);
                    }
                    else if (!quiet)
                        ScriptErrorMsg("cast - invalid serial or alias");
                }
            }
            else if (!quiet)
                ScriptErrorMsg("cast - spell name or number not valid");

            return true;
        }

        public static bool HeadMsg(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);
            
            if (!Client.Instance.ClientRunning)
                return true;

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: headmsg ('text') [color] [serial]");
                return true;
            }

            if (args.Count == 1)
                World.Player.OverheadMessage(Config.GetInt("SysColor"), args[0].Lexeme);
            else
            {
                int hue = Utility.ToInt32(args[1].Lexeme, 0);

                if (args.Count == 3)
                {
                    ASTNode target = args[2];
                    int serial = GetSerial(ref target);

                    Mobile m = World.FindMobile((uint) serial);

                    if (m != null)
                        m.OverheadMessage(hue, args[0].Lexeme);
                }
                else
                    World.Player.OverheadMessage(hue, args[0].Lexeme);
            }

            return true;
        }

        public static bool SysMsg(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (args.Count == 0)
            {
                ScriptErrorMsg("Usage: sysmsg ('text') [color]");
                return true;
            }

            if (!Client.Instance.ClientRunning)
                return true;

            if (args.Count == 1)
                World.Player.SendMessage(Config.GetInt("SysColor"), args[0].Lexeme);
            else if (args.Count == 2)
                World.Player.SendMessage(Utility.ToInt32(args[1].Lexeme, 0), args[0].Lexeme);

            return true;
        }

        public static bool DressCommand(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (!Client.Instance.ClientRunning)
                return true;
            //we're using a named dresslist or a temporary dresslist?
            if (args.Count == 0)
            {
                if (DressList._Temporary != null)
                    DressList._Temporary.Dress();
                else if (!quiet)
                    ScriptErrorMsg("No dresslist specified and no temporary dressconfig present - usage: dress ['dresslist']");
            }
            else
            {
                var d = DressList.Find(args[0].Lexeme);
                if (d != null)
                    d.Dress();
                else if (!quiet)
                    ScriptErrorMsg($"dresslist {args[0].Lexeme} not found");
            }

            return true;
        }

        public static bool UnDressCommand(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            List<ASTNode> args = ParseArguments(ref node);

            if (!Client.Instance.ClientRunning)
                return true;
            //we're using a named dresslist or a temporary dresslist?
            if (args.Count == 0)
            {
                if (DressList._Temporary != null)
                    DressList._Temporary.Undress();
                else if (!quiet)
                    ScriptErrorMsg("No dresslist specified and no temporary dressconfig present - usage: undress ['dresslist']");
            }
            else
            {
                var d = DressList.Find(args[0].Lexeme);
                if (d != null)
                    d.Undress();
                else if (!quiet)
                    ScriptErrorMsg($"dresslist {args[0].Lexeme} not found");
            }

            return true;
        }

        public static bool DressConfig(ref ASTNode node, bool quiet, bool force)
        {
            node = node.Next();

            if (!Client.Instance.ClientRunning)
                return true;

            if (DressList._Temporary == null)
                DressList._Temporary = new DressList("dressconfig");

            DressList._Temporary.Items.Clear();
            for (int i = 0; i < World.Player.Contains.Count; i++)
            {
                Item item = World.Player.Contains[i];
                if (item.Layer <= Layer.LastUserValid && item.Layer != Layer.Backpack && item.Layer != Layer.Hair &&
                    item.Layer != Layer.FacialHair)
                    DressList._Temporary.Items.Add(item.Serial);
            }

            return true;
        }
        
        private static void ScriptErrorMsg(string message, string scriptname = "")
        {
            World.Player?.SendMessage(MsgLevel.Error, $"Script {scriptname} error => {message}");_
        }
    }
}