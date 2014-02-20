#include <windows.h>
#include "RazorReceiver.h"

RazorReceiver::RazorReceiver()
	: _UoPID( 0 ), _hMutex( NULL ), _hMap( NULL ), _pShared( NULL ), _pBuff( NULL )
{
	// no code
}

RazorReceiver::~RazorReceiver()
{
	if ( _pShared )
		UnmapViewOfFile( _pShared );

	if ( _hMap )
		CloseHandle( _hMap );

	if ( _hMutex )
		CloseHandle( _hMutex );
}

bool RazorReceiver::Initialize( DWORD uoProcId )
{
	char name[256];

	_UoPID = uoProcId;

	sprintf( name, "UONetFwd_%x", _UoPID );
	_hMutex = CreateMutex( NULL, FALSE, name );
	if ( !_hMutex )
	{
		return false;
	}

	sprintf( name, "UONetSharedFM_%x", _UoPID );
	_hMap = CreateFileMapping( INVALID_HANDLE_VALUE, NULL, PAGE_READ, 0, sizeof(RazorReceiver::Buffer), name );
	if ( !_hMap )
	{
		CloseHandle( _hMutex );
		return false;
	}

	_pShared = MapViewOfFile( _hMap, FILE_MAP_ALL_ACCESS, 0, 0, 0 );
	if ( !_pShared )
	{
		CloseHandle( _hMap );
		CloseHandle( _hMutex );
		return false;
	}

	_pBuff = ((char*)_pShared) + FWD_BUFF_OFFSET;

	memset( _pBuff, 0, sizeof(RazorReceiver::Buffer) );

	return true;
}

int RazorReceiver::ReceivePacket( void *pData, int maxLen )
{
	unsigned short length = 0;
	unsigned char *data = NULL;

	WaitForSingleObject( _hMutex, INFINITE );

	if ( _pBuff->Length >= 3 )
	{
		data = _pBuff->Buff + _pBuff->Start;
		length = ((data[1]<<8)&0xFF00) | (data[2]&0x00FF);
	
		if ( length > maxLen )
			length = maxLen;

		if ( length > _pBuff->Length )
			length = _pBuff->Length;

		if ( _pBuff->Start + length > SHARED_BUFF_SIZE )
		{
			if ( _pBuff->Start > SHARED_BUFF_SIZE )
			{
				_pBuff->Start = _pBuff->Length = 0;
				length = 0;
			}
			else
			{
				length = _pBuff->Start - SHARED_BUFF_SIZE;
			}
		}

		if ( length > 0 )
			memcpy( pData, _pBuff->Start, length );

		_pBuff->Start += length;
		_pBuff->Length -= length;

		if ( _pBuff->Length <= 0 )
		{
			_pBuff->Start = 0;
			_pBuff->Length = 0;
		}
	}

	ReleaseMutex( _hMutex );

	return length;
}

bool RazorReceiver::PacketAvailable()
{
	bool haveData = false;

	WaitForSingleObject( _hMutex, INFINITE );

	if ( _pBuff->Length >= 3 && _pBuff->Start+_pBuff->Length <= SHARED_BUFF_SIZE )
	{
		haveData = true;
	}

	ReleaseMutex( _hMutex );
	return haveData;
}

HWND RazorReceiver::GetUoHwnd()
{
	DWORD tid = 0, pid = 0;
	HWND hWnd = NULL;

	if ( _UoPID != 0 )
	{
		hWnd = FindWindow( "Ultima Online", NULL );
		while ( hWnd != NULL )
		{
			tid = GetWindowThreadProcessId( hWnd, &pid );
			if ( pid == _UoPID )
				break;
			hWnd = FindWindowEx( NULL, hWnd, "Ultima Online", NULL );
		}

		if ( pid != _UoPID || hWnd == NULL )
		{
			hWnd = FindWindow( "Ultima Online Third Dawn", NULL );
			while ( hWnd != NULL )
			{
				tid = GetWindowThreadProcessId( hWnd, &pid );
				if ( pid == _UoPID )
					break;
				hWnd = FindWindowEx( NULL, hWnd, "Ultima Online Third Dawn", NULL );
			}
		}

		if ( pid != _UoPID )
			hWnd = NULL;
	}

	return hWnd;
}

