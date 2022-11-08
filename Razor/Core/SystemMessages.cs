#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
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

namespace Assistant.Core
{
    public static class SystemMessages
    {
        public static List<string> Messages { get; } = new List<string>();
        
        public static void Initialize()
        {
            MessageManager.OnSystemMessage += HandleSystemMessage;
        }
        
        private static void HandleSystemMessage(Packet p, PacketHandlerEventArgs args, Serial source, ushort graphic,
            MessageType type, ushort hue, ushort font, string lang, string sourceName,
            string text)
        {
            if (source == Serial.MinusOne && sourceName == "System")
            {
                if (Config.GetBool("FilterSnoopMsg") && text.IndexOf(World.Player.Name) == -1 &&
                    text.StartsWith("You notice") && text.IndexOf("attempting to peek into") != -1 &&
                    text.IndexOf("belongings") != -1)
                {
                    args.Block = true;
                    return;
                }

                if (text.StartsWith("You've committed a criminal act") || text.StartsWith("You are now a criminal"))
                {
                    World.Player.ResetCriminalTimer();
                }

                // Overhead message override
                OverheadManager.DisplayOverheadMessage(text);
            }

            if (!source.IsValid || source == World.Player.Serial || source.IsItem)
            {
                Add(text);
            }

            if (Config.GetBool("FilterSystemMessages"))
            {
                if (!MessageQueue.Enqueue(source, null, graphic, type, hue, font, lang, sourceName, text))
                {
                    args.Block = true;
                }
            }
        }

        public static void Add(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Messages.Add(text);

            if (Messages.Count >= 25)
            {
                Messages.RemoveRange(0, 10);
            }
        }

        public static bool Exists(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            for (int i = Messages.Count - 1; i >= 0; i--)
            {
                if (Messages[i].IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    Messages.RemoveRange(0, i + 1);
                    return true;
                }
            }

            return false;
        }
    }
}
