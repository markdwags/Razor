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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Assistant.UI;

namespace Assistant.Agents
{

    public class ItemInfoExtractorAgent : Agent
    {
        public static ItemInfoExtractorAgent Instance { get; private set; }

        public static void Initialize()
        {
            Agent.Add(Instance = new ItemInfoExtractorAgent());
        }

        public ItemInfoExtractorAgent()
        {
        }

        public override string Name => "Item-Info-Extractor";

        public override string Alias { get; set; }

        public override int Number { get; } = 0;

        public override void OnSelected(ListBox subList, params Button[] buttons)
        {
            buttons[0].Text = "Export Item Info";
            buttons[0].Visible = true;
        }

        public override void OnButtonPress(int num)
        {
            switch (num)
            {
                case 1:
                    World.Player.SendMessage(MsgLevel.Force, "Target a container to export.");
                    Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnTargetBag));
                    break;
            }
        }

        private async void OnTargetBag(bool location, Serial serial, Point3D loc, ushort gfx)
        {
            Engine.MainWindow.SafeAction(s => s.ShowMe());
            if (location || !serial.IsItem) { return; }

            Item item = World.FindItem(serial);
            if (item?.IsContainer != true)
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.InvalidCont);
                return;
            }

            try
            {
                var children = new ConcurrentBag<Item>();
                var listeningTask = Task.Run(async () =>
                {
                    PacketViewerCallback callback = (p, args) =>
                    {
                        ushort id = p.ReadUInt16();
                        if (id != 1) return; // object property list

                        Serial s = p.ReadUInt32();
                        if (!s.IsItem) return;

                        Item returnedItem = new Item(s); // Don't add to the world
                        returnedItem.ReadPropertyList(p);

                        children.Add(returnedItem);
                    };

                    try
                    {
                        World.Player.SendMessage(MsgLevel.Force, $"Processing {item.Contains.Count} items...");
                        PacketHandler.RegisterServerToClientViewer(0xD6, callback); // Register callback - 0xD6 "encoded" packets

                        Client.Instance.SendToServer(new DoubleClick(item.Serial)); // Open the bag

                        // Check every 500ms or until we've built up the expected children
                        var cancellationTokenSource = new CancellationTokenSource(3_000);
                        while (!cancellationTokenSource.IsCancellationRequested && children.Count != item.Contains.Count)
                        {
                            try
                            {
                                await Task.Delay(500, cancellationTokenSource.Token);
                            }
                            catch (OperationCanceledException)
                            {
                                // ignore
                            }
                            catch
                            {
                                // Don't return a partial result set
                                return null;
                            }
                        }

                        if (children.Count != item.Contains.Count)
                        {
                            World.Player.SendMessage(MsgLevel.Warning, $"The operation timed out. Only {children.Count} of {item.Contains.Count} items returned information.");
                        }
                        else
                        {
                            World.Player.SendMessage(MsgLevel.Force, "Retrieved information for all items.");
                        }
                    }
                    finally
                    {
                        PacketHandler.RemoveServerToClientViewer(0xD6, callback); // De-register callback
                    }

                    var containerSerialId = item.Serial.Value.ToString();

                    const string serialIdKey = "$serial_id";
                    const string countainerSerialIdKey = "$container_serial_id";
                    var headers = new HashSet<string>() { countainerSerialIdKey, serialIdKey };
                    var rows = new List<Dictionary<string, string>>();
                    foreach (var containerItem in children)
                    {
                        if (containerItem.IsContainer) continue;
                        if (containerItem.IsResource) continue;

                        var itemPropertiesWithValue = containerItem.ObjPropList.ExportProperties();

                        // Add the Serial
                        itemPropertiesWithValue.Add(serialIdKey, containerItem.Serial.Value.ToString());
                        itemPropertiesWithValue.Add(countainerSerialIdKey, containerSerialId);

                        rows.Add(itemPropertiesWithValue);

                        foreach (var key in itemPropertiesWithValue.Keys)
                        {
                            headers.Add(key);
                        }
                    }

                    var builder = new StringBuilder();
                    var sortedHeaders = headers.ToList();
                    builder.AppendLine(string.Join(",", sortedHeaders));

                    foreach (var row in rows)
                    {
                        builder.AppendLine(string.Join(",", sortedHeaders.Select(column =>
                        {
                            if (!row.TryGetValue(column, out var val)) return "";
                            if (val == null) return "1"; // Assume the property can't have a value
                            if (val.IndexOf(',') < 0) return val; // No comma, return the value directly

                            return $"\"{val.Replace("\"", "\"\"")}\""; // Quote values if necessary
                        })));
                    }

                    return builder.ToString();
                }, CancellationToken.None);

                var response = await listeningTask;
                if (string.IsNullOrWhiteSpace(response))
                {
                    World.Player.SendMessage(MsgLevel.Warning, "No items were extracted.");
                    return;
                }

                Clipboard.SetText(response);
                World.Player.SendMessage(MsgLevel.Force, "Item information has been saved to your clipboard.");
            }
            catch (Exception ex)
            {
                World.Player.SendMessage(MsgLevel.Warning, $"An unhandled error occurred attempting to extract items. {ex.Message}");
            }
        }

        public override void Save(XmlTextWriter xml)
        {
            // Do nothing
        }

        public override void Load(XmlElement node)
        {
            // Do nothing
        }

        public override void Clear()
        {
            // Do nothing
        }
    }
}
