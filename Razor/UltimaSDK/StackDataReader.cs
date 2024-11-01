#region license

// Copyright (c) 2024, andreakarasho
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by andreakarasho - https://github.com/andreakarasho
// 4. Neither the name of the copyright holder nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion
using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultima
{
    unsafe ref struct StackDataReader
    {
        private readonly ReadOnlySpan<byte> _data;
        public StackDataReader(ReadOnlySpan<byte> data)
        {
            _data = data;
            Length = data.Length;
            Position = 0;
        }

        public int Position { get; private set; }
        public long Length { get; }
        public int Remaining => (int)(Length - Position);

        public byte this[int index] => _data[0];

        public ReadOnlySpan<byte> Buffer => _data;


        public ref byte GetPinnableReference()
        {
            return ref MemoryMarshal.GetReference(_data);
        }

        public void Release()
        {
            // do nothing right now.
        }

        public void Seek(long p)
        {
            Position = (int)p;
        }

        public void Skip(int count)
        {
            Position += count;
        }

        public byte ReadUInt8()
        {
            if (Position + 1 > Length)
            {
                return 0;
            }

            return _data[Position++];
        }

        public sbyte ReadInt8()
        {
            if (Position + 1 > Length)
            {
                return 0;
            }

            return (sbyte)_data[Position++];
        }

        public bool ReadBool() => ReadUInt8() != 0;

        public ushort ReadUInt16LE()
        {
            if (Position + 2 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadUInt16LittleEndian(_data.Slice(Position), out ushort v);

            Skip(2);

            return v;
        }

        public short ReadInt16LE()
        {
            if (Position + 2 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadInt16LittleEndian(_data.Slice(Position), out short v);

            Skip(2);

            return v;
        }

        public uint ReadUInt32LE()
        {
            if (Position + 4 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadUInt32LittleEndian(_data.Slice(Position), out uint v);

            Skip(4);

            return v;
        }

        public int ReadInt32LE()
        {
            if (Position + 4 > Length)
            {
                return 0;
            }

            int v = BinaryPrimitives.ReadInt32LittleEndian(_data.Slice(Position));

            Skip(4);

            return v;
        }

        public ulong ReadUInt64LE()
        {
            if (Position + 8 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadUInt64LittleEndian(_data.Slice(Position), out ulong v);

            Skip(8);

            return v;
        }

        public long ReadInt64LE()
        {
            if (Position + 8 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadInt64LittleEndian(_data.Slice(Position), out long v);

            Skip(8);

            return v;
        }





        public ushort ReadUInt16BE()
        {
            if (Position + 2 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadUInt16BigEndian(_data.Slice(Position), out ushort v);

            Skip(2);

            return v;
        }

        public short ReadInt16BE()
        {
            if (Position + 2 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadInt16BigEndian(_data.Slice(Position), out short v);

            Skip(2);

            return v;
        }

        public uint ReadUInt32BE()
        {
            if (Position + 4 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadUInt32BigEndian(_data.Slice(Position), out uint v);

            Skip(4);

            return v;
        }

        public int ReadInt32BE()
        {
            if (Position + 4 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadInt32BigEndian(_data.Slice(Position), out int v);

            Skip(4);

            return v;
        }

        public ulong ReadUInt64BE()
        {
            if (Position + 8 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadUInt64BigEndian(_data.Slice(Position), out ulong v);

            Skip(8);

            return v;
        }

        public long ReadInt64BE()
        {
            if (Position + 8 > Length)
            {
                return 0;
            }

            BinaryPrimitives.TryReadInt64BigEndian(_data.Slice(Position), out long v);

            Skip(8);

            return v;
        }

        private string ReadRawString(int length, int sizeT, bool safe)
        {
            if (length == 0 || Position + sizeT > Length)
            {
                return string.Empty;
            }

            bool fixedLength = length > 0;
            int remaining = Remaining;
            int size;

            if (fixedLength)
            {
                size = length * sizeT;

                if (size > remaining)
                {
                    size = remaining;
                }
            }
            else
            {
                size = remaining - (remaining & (sizeT - 1));
            }

            ReadOnlySpan<byte> slice = _data.Slice(Position, size);

            int index = GetIndexOfZero(slice, sizeT);
            size = index < 0 ? size : index;

            string result;

            if (size <= 0)
            {
                result = String.Empty;
            }
            else
            {
                result = StringHelper.Cp1252ToString(slice.Slice(0, size));

                if (safe)
                {
                    Span<char> buff = stackalloc char[256];
                    ReadOnlySpan<char> chars = result.AsSpan();

                    ValueStringBuilder sb = new ValueStringBuilder(buff);

                    bool hasDoneAnyReplacements = false;
                    int last = 0;
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (!StringHelper.IsSafeChar(chars[i]))
                        {
                            hasDoneAnyReplacements = true;
                            sb.Append(chars.Slice(last, i - last));
                            last = i + 1; // Skip the unsafe char
                        }
                    }

                    if (hasDoneAnyReplacements)
                    {
                        // append the rest of the string
                        if (last < chars.Length)
                        {
                            sb.Append(chars.Slice(last, chars.Length - last));
                        }

                        result = sb.ToString();
                    }

                    sb.Dispose();
                }
            }

            Position += Math.Max(size + (!fixedLength && index >= 0 ? sizeT : 0), length * sizeT);

            return result;
        }

        public string ReadASCII(bool safe = false)
        {
            return ReadRawString(-1, 1, safe);
            //return ReadString(StringHelper.Cp1252Encoding, -1, 1, safe);
        }

        public string ReadASCII(int length, bool safe = false)
        {
            return ReadRawString(length, 1, safe);

            //return ReadString(StringHelper.Cp1252Encoding, length, 1, safe);
        }

        public string ReadUnicodeBE(bool safe = false)
        {
            return ReadString(Encoding.BigEndianUnicode, -1, 2, safe);
        }

        public string ReadUnicodeBE(int length, bool safe = false)
        {
            return ReadString(Encoding.BigEndianUnicode, length, 2, safe);
        }

        public string ReadUnicodeLE(bool safe = false)
        {
            return ReadString(Encoding.Unicode, -1, 2, safe);
        }

        public string ReadUnicodeLE(int length, bool safe = false)
        {
            return ReadString(Encoding.Unicode, length, 2, safe);
        }

        public string ReadUTF8(bool safe = false)
        {
            return ReadString(Encoding.UTF8, -1, 1, safe);
        }

        public string ReadUTF8(int length, bool safe = false)
        {
            return ReadString(Encoding.UTF8, length, 1, safe);
        }

        public void Read(Span<byte> data, int offset, int count)
        {
            _data.Slice(Position + offset, count).CopyTo(data);
        }

        // from modernuo <3
        private string ReadString(Encoding encoding, int length, int sizeT, bool safe)
        {
            if (length == 0 || Position + sizeT > Length)
            {
                return string.Empty;
            }

            bool fixedLength = length > 0;
            int remaining = Remaining;
            int size;

            if (fixedLength)
            {
                size = length * sizeT;

                if (size > remaining)
                {
                    size = remaining;
                }
            }
            else
            {
                size = remaining - (remaining & (sizeT - 1));
            }

            ReadOnlySpan<byte> slice = _data.Slice(Position, size);

            int index = GetIndexOfZero(slice, sizeT);
            size = index < 0 ? size : index;

            string result;

            fixed (byte* ptr = slice)
            {
                result = encoding.GetString(ptr, size);
            }

            if (safe)
            {
                Span<char> buff = stackalloc char[256];
                ReadOnlySpan<char> chars = result.AsSpan();

                ValueStringBuilder sb = new ValueStringBuilder(buff);

                bool hasDoneAnyReplacements = false;
                int last = 0;
                for (int i = 0; i < chars.Length; i++)
                {
                    if (!StringHelper.IsSafeChar(chars[i]))
                    {
                        hasDoneAnyReplacements = true;
                        sb.Append(chars.Slice(last, i - last));
                        last = i + 1; // Skip the unsafe char
                    }
                }

                if (hasDoneAnyReplacements)
                {
                    // append the rest of the string
                    if (last < chars.Length)
                    {
                        sb.Append(chars.Slice(last, chars.Length - last));
                    }

                    result = sb.ToString();
                }

                sb.Dispose();
            }

            Position += Math.Max(size + (!fixedLength && index >= 0 ? sizeT : 0), length * sizeT);

            return result;
        }

        private static int GetIndexOfZero(ReadOnlySpan<byte> span, int sizeT)
        {
            switch (sizeT)
            {
                case 2: return MemoryMarshal.Cast<byte, char>(span).IndexOf('\0') * 2;
                case 4: return MemoryMarshal.Cast<byte, uint>(span).IndexOf((uint)0) * 4;
                default: return span.IndexOf((byte)0);
            }
        }
    }
}
