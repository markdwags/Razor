using System;
using System.Collections.Generic;

namespace Assistant
{
    public delegate void PacketViewerCallback(PacketReader p, PacketHandlerEventArgs args);

    public delegate void PacketFilterCallback(Packet p, PacketHandlerEventArgs args);

    public class PacketHandlerEventArgs
    {
        private bool m_Block;

        public bool Block
        {
            get { return m_Block; }
            set { m_Block = value; }
        }

        public PacketHandlerEventArgs()
        {
            Reinit();
        }

        public void Reinit()
        {
            m_Block = false;
        }
    }

    public class PacketHandler
    {
        private static Dictionary<int, List<PacketViewerCallback>> m_ClientViewers;
        private static Dictionary<int, List<PacketViewerCallback>> m_ServerViewers;

        private static Dictionary<int, List<PacketFilterCallback>> m_ClientFilters;
        private static Dictionary<int, List<PacketFilterCallback>> m_ServerFilters;

        static PacketHandler()
        {
            m_ClientViewers = new Dictionary<int, List<PacketViewerCallback>>();
            m_ServerViewers = new Dictionary<int, List<PacketViewerCallback>>();

            m_ClientFilters = new Dictionary<int, List<PacketFilterCallback>>();
            m_ServerFilters = new Dictionary<int, List<PacketFilterCallback>>();
        }

        internal static void RegisterClientToServerViewer(int packetID, PacketViewerCallback callback)
        {
            List<PacketViewerCallback> list;
            if (!m_ClientViewers.TryGetValue(packetID, out list) || list == null)
                m_ClientViewers[packetID] = list = new List<PacketViewerCallback>();
            list.Add(callback);
        }

        internal static void RegisterServerToClientViewer(int packetID, PacketViewerCallback callback)
        {
            List<PacketViewerCallback> list;
            if (!m_ServerViewers.TryGetValue(packetID, out list) || list == null)
                m_ServerViewers[packetID] = list = new List<PacketViewerCallback>();
            list.Add(callback);
        }

        internal static void RemoveClientToServerViewer(int packetID, PacketViewerCallback callback)
        {
            List<PacketViewerCallback> list;
            if (m_ClientViewers.TryGetValue(packetID, out list) && list != null)
                list.Remove(callback);
        }

        internal static void RemoveServerToClientViewer(int packetID, PacketViewerCallback callback)
        {
            List<PacketViewerCallback> list;
            if (m_ServerViewers.TryGetValue(packetID, out list) && list != null)
                list.Remove(callback);
        }

        internal static void RegisterClientToServerFilter(int packetID, PacketFilterCallback callback)
        {
            List<PacketFilterCallback> list;
            if (!m_ClientFilters.TryGetValue(packetID, out list) || list == null)
                m_ClientFilters[packetID] = list = new List<PacketFilterCallback>();
            list.Add(callback);
        }

        internal static void RegisterServerToClientFilter(int packetID, PacketFilterCallback callback)
        {
            List<PacketFilterCallback> list;
            if (!m_ServerFilters.TryGetValue(packetID, out list) || list == null)
                m_ServerFilters[packetID] = list = new List<PacketFilterCallback>();
            list.Add(callback);
        }

        internal static void RemoveClientToServerFilter(int packetID, PacketFilterCallback callback)
        {
            List<PacketFilterCallback> list;
            if (m_ClientFilters.TryGetValue(packetID, out list) && list != null)
                list.Remove(callback);
        }

        internal static void RemoveServerToClientFilter(int packetID, PacketFilterCallback callback)
        {
            List<PacketFilterCallback> list;
            if (m_ServerFilters.TryGetValue(packetID, out list) && list != null)
                list.Remove(callback);
        }

        public static bool OnServerPacket(int id, PacketReader pr, Packet p)
        {
            bool result = false;
            if (pr != null)
            {
                List<PacketViewerCallback> list;
                if (m_ServerViewers.TryGetValue(id, out list) && list != null && list.Count > 0)
                    result = ProcessViewers(list, pr);
            }

            if (p != null)
            {
                List<PacketFilterCallback> list;
                if (m_ServerFilters.TryGetValue(id, out list) && list != null && list.Count > 0)
                    result |= ProcessFilters(list, p);
            }

            return result;
        }


        public static bool OnClientPacket(int id, PacketReader pr, Packet p)
        {
            bool result = false;
            if (pr != null)
            {
                List<PacketViewerCallback> list;
                if (m_ClientViewers.TryGetValue(id, out list) && list != null && list.Count > 0)
                    result = ProcessViewers(list, pr);
            }

            if (p != null)
            {
                List<PacketFilterCallback> list;
                if (m_ClientFilters.TryGetValue(id, out list) && list != null && list.Count > 0)
                    result |= ProcessFilters(list, p);
            }

            return result;
        }

        public static bool HasClientViewer(int packetID)
        {
            List<PacketViewerCallback> list;
            return m_ClientViewers.TryGetValue(packetID, out list) && list != null && list.Count > 0;
        }

        public static bool HasServerViewer(int packetID)
        {
            List<PacketViewerCallback> list;
            return m_ServerViewers.TryGetValue(packetID, out list) && list != null && list.Count > 0;
        }

        public static bool HasClientFilter(int packetID)
        {
            List<PacketFilterCallback> list;
            return (m_ClientFilters.TryGetValue(packetID, out list) && list != null && list.Count > 0);
        }

        public static bool HasServerFilter(int packetID)
        {
            List<PacketFilterCallback> list;
            return (m_ServerFilters.TryGetValue(packetID, out list) && list != null && list.Count > 0);
        }

        private static PacketHandlerEventArgs m_Args = new PacketHandlerEventArgs();

        private static bool ProcessViewers(List<PacketViewerCallback> list, PacketReader p)
        {
            m_Args.Reinit();

            if (list != null)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    p.MoveToData();

                    try
                    {
                        list[i](p, m_Args);
                    }
                    catch (Exception e)
                    {
                        Engine.LogCrash(e);
                        new MessageDialog("WARNING: Packet viewer exception!", true, e.ToString()).Show();
                    }
                }
            }

            return m_Args.Block;
        }

        private static bool ProcessFilters(List<PacketFilterCallback> list, Packet p)
        {
            m_Args.Reinit();

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    p.MoveToData();

                    try
                    {
                        list[i](p, m_Args);
                    }
                    catch (Exception e)
                    {
                        Engine.LogCrash(e);
                        new MessageDialog("WARNING: Packet filter exception!", true, e.ToString()).Show();
                    }
                }
            }

            return m_Args.Block;
        }
    }
}