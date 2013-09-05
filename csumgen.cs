using System;
using System.IO;
using System.Security.Cryptography;

namespace csumgen
{
	public class csumgen
	{
		public static void Main()
		{
			using ( StreamWriter sw = new StreamWriter( "Crypt\\Checksum.h", false ) ) {
				byte[] data = File.ReadAllBytes("Output\\Razor.exe");

				MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
				byte[] hash = x.ComputeHash(data);
				Buffer.BlockCopy(hash, 0, data, 0, 16);

				for (int a = 0; a < 16; a++)
					data[a] ^= data[0x1717 + a];

				sw.Write("unsigned char RAZOR_CHECKSUM[16] = { ");
				for (int b = 0; b < 16; b++)
				{
					if ( b != 15 )
						sw.Write("0x{0:x2}, ", data[b]);
					else
						sw.Write("0x{0:x2} ", data[b]);
				}

				sw.WriteLine( "};" );

				byte[] data2 = File.ReadAllBytes( "Output\\Ultima.dll" );

				MD5CryptoServiceProvider x2 = new MD5CryptoServiceProvider();
				byte[] hash2 = x2.ComputeHash(data2);
				Buffer.BlockCopy(hash2, 0, data2, 0, 16);

				for (int c = 0; c < 16; c++)
					data2[c] ^= data2[0x1717 + c];

				sw.Write("unsigned char ULTIMA_CHECKSUM[16] = { ");
				for (int d = 0; d < 16; d++)
				{
					if ( d != 15 )
						sw.Write("0x{0:x2}, ", data2[d]);
					else
						sw.Write("0x{0:x2} ", data2[d]);
				}

				sw.WriteLine( "};" );
			}
		}
	}
}