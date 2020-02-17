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

using System;

namespace Assistant
{
    public class MemoryHelperThinggie : Timer
    {
        private static TimeSpan Frequency = TimeSpan.FromMinutes(2.5);

        public static readonly MemoryHelperThinggie Instance = new MemoryHelperThinggie();

        public static void Initialize()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                Instance.Start();
        }

        private MemoryHelperThinggie() : base(TimeSpan.Zero, Frequency)
        {
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private static extern uint SetProcessWorkingSetSize(IntPtr hProc, int minSize, int maxSize);

        protected override void OnTick()
        {
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }
    }
}