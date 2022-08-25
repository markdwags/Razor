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
using Microsoft.Win32.SafeHandles;

namespace Ultima
{
    public class ClientWindowHandle : CriticalHandleZeroOrMinusOneIsInvalid
    {
        public static ClientWindowHandle Invalid = new ClientWindowHandle(new IntPtr(-1));

        public ClientWindowHandle()
        {
        }

        public ClientWindowHandle(IntPtr value)
        {
            handle = value;
        }

        protected override bool ReleaseHandle()
        {
            if (!this.IsClosed)
                return ReleaseHandle();
            return true;
        }
    }

    public class ClientProcessHandle : CriticalHandleZeroOrMinusOneIsInvalid
    {
        public static ClientProcessHandle Invalid = new ClientProcessHandle(new IntPtr(-1));

        public ClientProcessHandle()
            : base()
        {
        }

        public ClientProcessHandle(IntPtr value)
            : base()
        {
            handle = value;
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(this) == 0;
        }
    }
}