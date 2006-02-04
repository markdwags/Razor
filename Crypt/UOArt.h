#pragma once

#define Color16to32(c16) (((c16) & 0x7C00) >> 7) | (((c16) & 0x3E0) << 6) | (((c16) & 0x1F) << 19)

struct UOItem
{
	int RealWidth;
	int RealHeight;

	int Left;
	int Top;
	int Bottom;
	int Right;

	int GetWidth() const { return Right - Left; }
	int GetHeight() const { return Bottom - Top; }

	unsigned short **Data; // [ReadlWidth][RealHeight] 32bit color data

	int ItemID;
	UOItem *pNext;
};

struct ArtIdx
{
	long FilePos;
	long Length;
	long Unused;
};

struct ArtHeader
{
	long Unknown;
	short Width;
	short Height;
	//followed by short LookupTable
};

extern char *DataPath;
extern HANDLE CommMutex;
