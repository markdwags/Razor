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

namespace Ultima
{
    public class WindowProcessStream : ProcessStream
    {
        private ClientWindowHandle m_Window;
        private ClientProcessHandle m_ProcessID;

        public ClientWindowHandle Window
        {
            get { return m_Window; }
            set { m_Window = value; }
        }

        public WindowProcessStream(ClientWindowHandle window)
        {
            m_Window = window;
            m_ProcessID = ClientProcessHandle.Invalid;
        }

        public override ClientProcessHandle ProcessID
        {
            get
            {
                if (NativeMethods.IsWindow(m_Window) != 0 && !m_ProcessID.IsInvalid)
                    return m_ProcessID;

                NativeMethods.GetWindowThreadProcessId(m_Window, ref m_ProcessID);

                return m_ProcessID;
            }
        }
    }
}