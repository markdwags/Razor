#pragma once

class Compression
{
public:
	static int Compress( char *out, const char *in, int inlen );
	static int Decompress( char *out, const char *in, int inlen );

	static void Reset();

protected:
	static int bit_num, treepos;
	static int value, mask;

	static int CompressionTable[];
	static int DecompressionTree[];
};

