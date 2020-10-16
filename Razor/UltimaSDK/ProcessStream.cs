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
using System.IO;

namespace Assistant.UltimaSDK
{
    public unsafe abstract class ProcessStream : Stream
    {
        private const int ProcessAllAccess = 0x1F0FFF;

        protected bool m_Open;
        protected ClientProcessHandle m_Process;

        protected int m_Position;

        public abstract ClientProcessHandle ProcessID { get; }

        public ProcessStream()
        {
        }

        public virtual bool BeginAccess()
        {
            if (m_Open)
                return false;

            m_Process = NativeMethods.OpenProcess(ProcessAllAccess, 0, ProcessID);
            m_Open = true;

            return true;
        }

        public virtual void EndAccess()
        {
            if (!m_Open)
                return;

            m_Process.Close();
            m_Open = false;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            bool end = !BeginAccess();

            int res = 0;

            fixed (byte* p = buffer)
                NativeMethods.ReadProcessMemory(m_Process, m_Position, p + offset, count, ref res);

            m_Position += count;

            if (end)
                EndAccess();

            return res;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            bool end = !BeginAccess();

            fixed (byte* p = buffer)
                NativeMethods.WriteProcessMemory(m_Process, m_Position, p + offset, count, 0);

            m_Position += count;

            if (end)
                EndAccess();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { return m_Position; }
            set { m_Position = (int) value; }
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    m_Position = (int) offset;
                    break;
                case SeekOrigin.Current:
                    m_Position += (int) offset;
                    break;
                case SeekOrigin.End: throw new NotSupportedException();
            }

            return m_Position;
        }
    }
}