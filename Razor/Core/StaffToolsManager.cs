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
using System.Windows.Forms;
using System.Xml;
using Ultima;

namespace Assistant.Core
{
    public static class StaffToolsManager
    {
        public static void Initialize()
        {
           
        }

        public static void LoadItems(TreeView tree)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Path.Combine(Config.GetInstallDirectory(), "items.xml"));

            tree.BeginUpdate();

            tree.Nodes.Clear();
            tree.Nodes.Add(new TreeNode(xDoc.DocumentElement.Name));

            TreeNode tNode = tree.Nodes[0];
            AddItemTreeNode(xDoc.DocumentElement, tNode);

            tree.Nodes[0].Expand();

            tree.EndUpdate();
        }
        
        public static void LoadDoors(TreeView tree)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Path.Combine(Config.GetInstallDirectory(), "doors.xml"));

            tree.BeginUpdate();

            tree.Nodes.Clear();
            tree.Nodes.Add(new TreeNode(xDoc.DocumentElement.Name));

            TreeNode tNode = tree.Nodes[0];
            AddDoorTreeNode(xDoc.DocumentElement, tNode);

            tree.Nodes[0].Expand();

            tree.EndUpdate();
        }

        private static void AddItemTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            if (xmlNode.HasChildNodes) //The current node has children
            {
                XmlNodeList xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    XmlNode xNode = xmlNode.ChildNodes[x];

                    if (xNode.Attributes != null)
                    {
                        XmlAttribute name = xNode.Attributes["name"];
                        //XmlAttribute id = xNode.Attributes["ID"];

                        if (name != null)
                        {
                            treeNode.Nodes.Add(new TreeNode(name.Value));
                        }
                        /*else if (id != null)
                        {
                            treeNode.Nodes.Add(new TreeNode(id.Value));
                        }*/
                        else
                        {
                            treeNode.Nodes.Add(new TreeNode(xNode.Name));
                        }
                    }
                    else
                    {
                        treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    }

                    TreeNode tNode = treeNode.Nodes[x];
                    AddItemTreeNode(xNode, tNode);
                }
            }
            else
            {
                //No children, so add the outer xml (trimming off whitespace)
                XmlAttribute id = xmlNode.Attributes?["ID"];

                if (id != null)
                {
                    int itemId = Utility.ToInt32(id.Value, 0x0);
                    string itemName = TileData.ItemTable[itemId].Name;

                    if (string.IsNullOrEmpty(itemName))
                    {
                        treeNode.Text = $"{id.Value}";
                    }
                    else
                    {
                        treeNode.Text = $"{itemName} ({id.Value})";
                    }

                    treeNode.Tag = itemId;

                }
                else
                {
                    xmlNode.OuterXml.Trim();
                }
            }
        }


        private static void AddDoorTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            if (xmlNode.HasChildNodes) //The current node has children
            {
                XmlNodeList xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    XmlNode xNode = xmlNode.ChildNodes[x];

                    if (xNode.Attributes != null)
                    {
                        DoorInfo doorInfo = DoorInfo.FromXmlNode(xNode);

                        treeNode.Nodes.Add(new TreeNode(doorInfo.Name));
                    }
                    else
                    {
                        treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    }

                    TreeNode tNode = treeNode.Nodes[x];
                    AddDoorTreeNode(xNode, tNode);
                }
            }
            else
            {
                //No children, so add the outer xml (trimming off whitespace)
                XmlAttribute id = xmlNode.Attributes?["base"];

                if (id != null)
                {
                    //<Door name="Stone 1" base="232" item="SecretStoneDoor1"></Door>
                    DoorInfo doorInfo = DoorInfo.FromXmlNode(xmlNode);
                    treeNode.Tag = doorInfo;

                }
                else
                {
                    xmlNode.OuterXml.Trim();
                }
            }
        }

        public static void AddDoor(DoorInfo door, string facing)
        {
            World.Player.Say($"[add {door.Item} {facing}");
        }

        public static int GetDoorOffset(string facing)
        {
            var offset = 0;

            if (facing.Contains("WestCW"))
            {
                offset = 0;
            }
            else if (facing.Contains("EastCCW"))
            {
                offset = 2;
            }
            else if (facing.Contains("WestCCW"))
            {
                offset = 4;
            }
            else if (facing.Contains("EastCW"))
            {
                offset = 6;
            }
            else if (facing.Contains("NorthCW"))
            {
                offset = 14;
            }
            else if (facing.Contains("SouthCCW"))
            {
                offset = 8;
            }
            else if (facing.Contains("NorthCCW"))
            {
                offset = 10;
            }
            else if (facing.Contains("SouthCW"))
            {
                offset = 12;
            }

            return offset;
        }
    }

    public class DoorInfo
    {
        /// <summary>
        ///     Gets or sets the door name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets or sets the door's item name
        /// </summary>
        public string Item { get; private set; }

        /// <summary>
        ///     Gets or sets the door's base ID
        /// </summary>
        public int BaseId { get; private set; }

        /// <summary>
        ///     Creates a new DoorInfo object
        /// </summary>
        private DoorInfo()
        { }

        /// <summary>
        ///     Creates a DoorInfo from an Xml node
        /// </summary>
        /// <param name="xNode">The XmlNode to convert to a door info</param>
        /// <returns>A Door Info object</returns>
        public static DoorInfo FromXmlNode(XmlNode xNode)
        {
            var door = new DoorInfo
            {
                Name = xNode.Attributes["name"].Value,
                Item = xNode.Attributes["item"]?.Value,
                BaseId = xNode.Attributes["base"]?.Value != null ? int.Parse(xNode.Attributes["base"].Value) : 0
            };

            return door;
        }
    }
}