#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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
using System.IO;

namespace Assistant.Core
{
    public static class MessageManager
    {
        public static Action<Packet, PacketHandlerEventArgs, Serial, ushort, MessageType, ushort, ushort, string, string, string> OnSpellMessage { get; set; }
        public static Action<Packet, PacketHandlerEventArgs, Serial, ushort, MessageType, ushort, ushort, string, string, string> OnLabelMessage { get; set; }
        public static Action<Packet, PacketHandlerEventArgs, Serial, ushort, MessageType, ushort, ushort, string, string, string> OnSystemMessage { get; set; }
        public static Action<Packet, PacketHandlerEventArgs, Serial, ushort, MessageType, ushort, ushort, string, string, string> OnMobileMessage { get; set; }

        public static bool GetLabelCommand { get; set; } = false;

        public static void Initialize()
        {
            OnMobileMessage += HandleMobileMessage;
        }

        public static void HandleMessage(Packet p, PacketHandlerEventArgs args, Serial source, ushort graphic,
                                         MessageType type, ushort hue, ushort font, string lang, string sourceName,
                                         string text)
        {
            if (World.Player == null)
            {
                return;
            }

            switch (type)
            {
                case MessageType.Spell:
                    OnSpellMessage?.Invoke(p, args, source, graphic, type, hue, font, lang, sourceName, text);
                    break;
                case MessageType.Label:
                    if (source.IsMobile)
                    {
                        Mobile m = World.FindMobile(source);
                        if (m != null)
                        {
                            m.Name = sourceName;
                        }
                    }

                    OnLabelMessage?.Invoke(p, args, source, graphic, type, hue, font, lang, sourceName, text);
                    break;
                case MessageType.Emote:

                    if (Config.GetBool("PlayEmoteSound") && source.IsMobile)
                    {
                        Mobile m = World.FindMobile(source);

                        if (m != null)
                        {
                            text = text.Trim('*');

                            if (m.Female)
                            {
                                if (Enum.TryParse(text, true, out FemaleSounds sound))
                                {
                                    if (sound != 0)
                                    {
                                        Client.Instance.SendToClient(new PlaySound((int)sound));
                                    }
                                }
                            }
                            else
                            {
                                if (Enum.TryParse(text, true, out MaleSounds sound))
                                {
                                    if (sound != 0)
                                    {
                                        Client.Instance.SendToClient(new PlaySound((int) sound));
                                    }
                                }
                            }
                        }
                    }

                    break;
                default:
                    if (source == Serial.MinusOne && sourceName == "System")
                    {
                        OnSystemMessage?.Invoke(p, args, source, graphic, type, hue, font, lang, sourceName, text);
                    }

                    if (source.IsMobile && source != World.Player.Serial)
                    {
                        if (GetLabelCommand)
                        {
                            OnLabelMessage?.Invoke(p, args, source, graphic, type, hue, font, lang, sourceName, text);
                        }
                        else
                        {
                            OnMobileMessage?.Invoke(p, args, source, graphic, type, hue, font, lang, sourceName, text);
                        }
                    }
                    
                    break;
            }
        }

        public static void HandleMobileMessage(Packet p, PacketHandlerEventArgs args, Serial source, ushort graphic,
                                 MessageType type, ushort hue, ushort font, string lang, string sourceName,
                                 string text)
        {
            if (Config.GetBool("ForceSpeechHue"))
            {
                p.Seek(10, SeekOrigin.Begin);
                p.Write((ushort)Config.GetInt("SpeechHue"));
            }
        }
    }
}