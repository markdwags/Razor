#include "stdafx.h"
#include "MemFinder.h"

DWORD MemFinder::Find( const void *data, int length, DWORD addressHint, DWORD addressMax )
{
	for( DWORD addr = addressHint; addr < addressMax && !IsBadReadPtr( (void*)addr, length ) ; addr++ )
	{
		if ( memcmp( (const void*)addr, data, length ) == 0 )
			return addr;
	}

	return 0;
}

MemFinder::MemFinder()
{
	Clear();
}

MemFinder::~MemFinder()
{
	Clear();
}

void MemFinder::Clear()
{
	_Executed = false; 
	_StartPos = 0xFFFFFFFF;

	for(unsigned int i=0;i<_Entries.size();i++)
		delete[] _Entries[i].Data;

	_Entries.clear();
}


void MemFinder::AddEntry( const void *data, int length, unsigned int maxResults, DWORD hint )
{
	Entry ent;

	ent.Data = new char[length];
	memcpy( ent.Data, data, length );

	ent.Length = length;
	ent.MaxResults = maxResults;
	ent.PositionHint = hint;

	if ( hint < _StartPos )
		_StartPos = hint;

	_Entries.push_back( ent );
}

DWORD MemFinder::GetAddress( const void *data, int length, unsigned int idx )
{
	if ( !_Executed )
		return 0;

	for(unsigned int i=0;i<_Entries.size();i++)
	{
		if ( _Entries[i].Length != length )
			continue;

		if ( !memcmp( data, _Entries[i].Data, length ) )
		{
			if ( idx < _Entries[i].Results.size() )
				return _Entries[i].Results[idx];
			else
				return 0;
		}
	}

	return 0;
}

void MemFinder::Execute()
{
	bool allDone = false;
	for( DWORD pos = _StartPos; pos < 0x02000000 && !allDone; pos++ )
	{
		allDone = true;
		for(unsigned int i=0;i<_Entries.size();i++)
		{
			Entry &e = _Entries[i];

			if ( e.Results.size() >= e.MaxResults )
				continue;

			if ( IsBadReadPtr( (void*)pos, e.Length ) )
				continue;

			allDone = false;

			if ( e.PositionHint > pos )
				continue;

			if ( !memcmp( (void*)pos, e.Data, e.Length ) )
				e.Results.push_back( pos );
		}
	}

	_Executed = true;
}

