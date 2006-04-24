#include "stdafx.h"
#include "UOArt.h"
#include "Crypt.h"

UOItem *ArtCache = NULL;
unsigned short **Hues = NULL;
int NumHues = 0;

inline int Round( float n )
{
	int i = (int)n;
	return i + ( n-i >= 0.5 ? 1 : 0 );
}

unsigned short *GetHue( int index )
{
	if ( Hues == NULL )
	{
		if ( !pShared )
			return NULL;

		char str[512];
		int length, blockCount, index;
		FILE *huesMul = NULL;

		WaitForSingleObject( CommMutex, INFINITE );
		sprintf( str, "%s/hues.mul", pShared->DataPath );
		ReleaseMutex( CommMutex );
		
		huesMul = fopen( str, "rb" );
		if ( !huesMul )
		{
			Hues = new unsigned short *[1];
			Hues[0] = new unsigned short[34];
			memset( Hues[0], 0, 34*2 );
			NumHues = 1;
			return NULL;
		}
		
		fseek( huesMul, 0, SEEK_END );
		length = (int)ftell( huesMul );
		fseek( huesMul, 0, SEEK_SET );
		blockCount = length / 708;
		if ( blockCount > 375 )
			blockCount = 375;
		NumHues = blockCount*8;
		
		Hues = new unsigned short *[NumHues];

		index = 0;
		for (int b=0;b<blockCount;b++)
		{
			fseek( huesMul, 4, SEEK_CUR );

			for (int i=0;i<8;i++, index++)
			{
				Hues[index] = new unsigned short[34];
				for (int c=0;c<34;c++)
				{
					unsigned short color;
					fread( &color, 2, 1, huesMul );
					Hues[index][c] = color | 0x8000;
				}

				fseek( huesMul, 20, SEEK_CUR );// ignore name
			}
		}
		fclose ( huesMul );
	}

	if ( index > 0 && index <= NumHues )
		return Hues[index-1];
	else
		return NULL;
}

unsigned short ApplyHueToPixel( unsigned short *hue, unsigned short pix )
{
	if ( hue )
		return hue[(pix>>10)&31];
	else
		return pix;
}

UOItem *ReadUOItem( int item, int bh )
{
	if ( item == 0 || item >= 0xFFFF || !pShared )
		return NULL;

	char str[512];
	short *Lookup;
	unsigned short *Run;

	FILE *idxMul, *artMul;
	ArtIdx idx;
	ArtHeader header;
	memset( &header, 0, sizeof(ArtHeader) );

	WaitForSingleObject( CommMutex, INFINITE );
	sprintf( str, "%s/artidx.mul", pShared->DataPath );
	ReleaseMutex( CommMutex );
	
	idxMul = fopen( str, "rb" );
	if ( !idxMul )
		return NULL;
	fseek( idxMul, item*sizeof(ArtIdx), SEEK_SET );
	fread( &idx, sizeof(ArtIdx), 1, idxMul );
	fclose( idxMul );
	if ( idx.FilePos == -1 || idx.Length == -1 )
		return NULL;

	WaitForSingleObject( CommMutex, INFINITE );
	sprintf( str, "%s/art.mul", pShared->DataPath );
	ReleaseMutex( CommMutex );

	artMul = fopen( str, "rb" );
	if ( !artMul )
	{
		fclose( idxMul );
		return NULL;
	}
	fseek( artMul, idx.FilePos, SEEK_SET );
	fread( &header, sizeof(ArtHeader), 1, artMul );
	if ( header.Height <= 0 || header.Width <= 0 || header.Height >= 1024 || header.Width >= 1024 || header.Unknown > 0xFFFF || header.Unknown == 0 )
	{
		fclose( artMul );
		return NULL;
	}

	Run = new unsigned short[header.Width]; // it should never be wider than the whole image!
	Lookup = new short[header.Height];
	fread( Lookup, header.Height * 2, 1, artMul );
	long dataStart = ftell( artMul );

	UOItem *pNew = new UOItem;
	pNew->ItemID = item;
	pNew->pNext = ArtCache;
	ArtCache = pNew;

	unsigned short **Image = new unsigned short*[header.Width];
	for(int i=0;i<header.Width;i++)
	{
		Image[i] = new unsigned short[header.Height];
		memset( Image[i], 0, header.Height*2 );
	}

	pNew->Left = pNew->Top = 0x7FFFFFFF;
	pNew->Right = pNew->Bottom = 0;
	for (int y=0;y<header.Height;y++)
	{
		int x = 0;

		fseek( artMul, dataStart + Lookup[y] * 2, SEEK_SET );
		do {
			short RunOffset = 0, RunLength = 0;

			fread( &RunOffset, 2, 1, artMul );
			fread( &RunLength, 2, 1, artMul );
			
			if ( RunLength <= 0 || RunOffset < 0 || RunOffset + RunLength >= 2048 || RunLength > header.Width ) 
				break;

			if ( y > pNew->Bottom )
				pNew->Bottom = y;
			if ( y < pNew->Top )
				pNew->Top = y;

			x += RunOffset;
			if ( x < pNew->Left )
				pNew->Left = x;

			fread( Run, RunLength*2, 1, artMul );
			for (int o=0;o<RunLength;o++,x++)
				Image[x][y] = Run[o];

			if ( x > pNew->Right )
				pNew->Right = x;
		} while ( true );
	}
	fclose( artMul );

	delete[] Run;
	delete[] Lookup;

	float scale = float(bh) / float(pNew->GetHeight());
	if ( scale > 1 || scale <= 0 )
		scale = 1;

	pNew->RealHeight = (int)(header.Height * scale + 1);
	pNew->RealWidth = (int)(header.Width * scale + 1);
	pNew->Data = new unsigned short *[pNew->RealWidth];
	for(int x=0;x<pNew->RealWidth;x++)
	{
		pNew->Data[x] = new unsigned short[pNew->RealHeight];
		memset( pNew->Data[x], 0, 2*pNew->RealHeight );
	}

	for(int x=0;x<header.Width;x++)
	{
		for(int y=0;y<header.Height;y++)
			pNew->Data[(int)(x * scale)][(int)(y * scale)] |= Image[x][y];
	}

	pNew->Top = (int)(pNew->Top * scale);
	pNew->Left = (int)(pNew->Left * scale);
	pNew->Bottom = (int)(pNew->Bottom * scale);
	pNew->Right = (int)(pNew->Right * scale);

	for(int x=0;x<header.Width;x++)
		delete[] Image[x];
	delete[] Image;

	return pNew;
}

UOItem *FindItem( int item )
{
	UOItem *node = ArtCache;
	while ( node != NULL )
	{
		if ( node->ItemID == item )
			return node;
		else
			node = node->pNext;
	}

	return NULL;
}

inline COLORREF Brightness( int shift, COLORREF c )
{
	return RGB( min(255,GetRValue(c)+shift), min(255,GetGValue(c)+shift), min(255,GetBValue(c)+shift) );
}

int DrawUOItem( HDC hDC, RECT rect, int item, int hueIdx )
{
	item |= 0x4000;

	rect.top ++;
	rect.bottom --;
	int maxHeight = rect.bottom - rect.top;

	UOItem *i = FindItem( item );
	if ( i == NULL )
		i = ReadUOItem( item, maxHeight );
	
	if ( i == NULL )
		return 0;

	if ( i->GetHeight() < maxHeight )
		rect.top += ( maxHeight - i->GetHeight() ) / 2;

	unsigned short *hue = GetHue( hueIdx );
	for(int x=i->Left;x<=i->Right;x++)
	{
		for (int y=i->Top;y<=i->Bottom;y++)
		{
			if ( i->Data[x][y] != 0 )
				SetPixel( hDC, rect.left + x - i->Left, rect.top + y - i->Top, Brightness( 0x30, Color16to32( ApplyHueToPixel( hue, i->Data[x][y] ) ) ) );
		}
	}

	return i->GetWidth() + 3;
}

void FreeItem( UOItem *node )
{
	if ( node != NULL )
	{
		FreeItem( node->pNext );
		for(int i=0;i<node->RealWidth;i++)
			delete[] node->Data[i];
		delete[] node->Data;
		delete node;
	}
}

void FreeArt()
{
	FreeItem( ArtCache );
	if ( Hues && NumHues > 0 )
	{
		for (int i=0;i<NumHues;i++)
			delete[] Hues[i];
		delete[] Hues;
	}
}
