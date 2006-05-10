#include "stdafx.h"
#include "Crypt.h"
#include "uo_huffman.h"
#include "PacketInfo.h"
#include "OSIEncryption.h"
#include "LoginEncryption.h"
#include "MemFinder.h"
#include "Checksum.h"

//*************************************************************************************
//**************************************Varaibles**************************************
//*************************************************************************************
HHOOK hWndProcRetHook = NULL;
HHOOK hGetMsgHook = NULL;
HWND hWatchWnd = NULL;
HWND hPostWnd = NULL;
DWORD UOProcId = 0;

HANDLE hFileMap = NULL;
HMODULE hInstance = NULL;
SOCKET CurrentConnection = 0;

HANDLE CommMutex = NULL;

char *tempBuff = NULL;

SharedMemory *pShared = NULL;

LARGE_INTEGER PerfFreq, Counter;

DWORD DeathMsgAddr = 0xFFFFFFFF;
HWND hUOAWnd = NULL;

SIZE DesiredSize = {800,600};

unsigned long OldRecv, OldSend, OldConnect, OldCloseSocket, OldSelect;
unsigned long RecvAddress, SendAddress, ConnectAddress, CloseSocketAddress, SelectAddress;

bool Seeded = false;
bool FirstRecv = true;
bool FirstSend = true;
bool LoginServer = false;
bool Active = true;
bool Disabled = true;
bool SmartCPU = false;
bool ServerNegotiated = false;
bool AllowNegotiate = false;
bool InGame = false;

bool OSICryptEnabled = false;

//**************************************OSI Only Stuff*********************************
OSIEncryption *ClientCrypt = NULL;
OSIEncryption *ServerCrypt = NULL;
LoginEncryption *ClientLogin = NULL;
LoginEncryption *ServerLogin = NULL;
//*************************************************************************************

//*************************************************************************************
//**************************************Functions**************************************
//*************************************************************************************
void RedrawTitleBar( HWND, bool );
void InitThemes();
LRESULT CALLBACK WndProcRetHookFunc( int, WPARAM, LPARAM );
LRESULT CALLBACK GetMsgHookFunc( int, WPARAM, LPARAM );

bool HookFunction( const char *, const char *, int, unsigned long, unsigned long *, unsigned long * );
void MemoryPatch( unsigned long, unsigned long );
void MemoryPatch( unsigned long, int, int );
void MemoryPatch( unsigned long, const void *, int );
void FlushSendData();

bool CreateSharedMemory();
void CloseSharedMemory();
void FreeArt();

//Hooks:
int PASCAL HookRecv( SOCKET, char *, int, int );
int PASCAL HookSend( SOCKET, char *, int, int );
int PASCAL HookConnect( SOCKET, const sockaddr *, int );
int PASCAL HookCloseSocket( SOCKET );
int PASCAL HookSelect( int, fd_set*, fd_set*, fd_set*, const struct timeval * );

typedef int (PASCAL *NetIOFunc)(SOCKET, char *, int, int);
typedef int (PASCAL *ConnFunc)(SOCKET, const sockaddr *, int);
typedef int (PASCAL *CLSFunc)(SOCKET);
typedef int (PASCAL *SelectFunc)( int, fd_set*, fd_set*, fd_set*, const struct timeval* );




BOOL APIENTRY DllMain( HANDLE hModule, DWORD dwReason, LPVOID )
{
	DWORD postID, thisID;

	hInstance = (HMODULE)hModule;
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls( hInstance );
		QueryPerformanceFrequency( &PerfFreq );
		QueryPerformanceCounter( &Counter );
		break;

	case DLL_PROCESS_DETACH:
		postID = 0;
		thisID = GetCurrentProcessId();
		if ( IsWindow( hPostWnd ) )
			GetWindowThreadProcessId( hPostWnd, &postID );

		if ( thisID == postID || thisID == UOProcId )
		{
			if ( IsWindow( hPostWnd ) )
				PostMessage( hPostWnd, WM_UONETEVENT, CLOSE, 0 );

			if ( IsWindow( hWatchWnd ) )
			{
				PostMessage( hWatchWnd, WM_QUIT, 0, 0 );
				SetForegroundWindow( hWatchWnd );
				SetFocus( hWatchWnd );
			}

			CloseSharedMemory();
		}
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}

	return TRUE;
}

DLLFUNCTION DWORD InitializeLibrary( const char *exeVer )
{
	char fileName[256];
	//TODO: addition hack protection?
	if ( !strcmp( exeVer, DLL_VERSION ) )
	{
		GetModuleFileName( NULL, fileName, 256 );

		FILE *file = fopen( fileName, "rb" );

		fseek( file, 0, SEEK_END );
		int len = ftell( file );
		fseek( file, 0, SEEK_SET );

		BYTE *data = new BYTE[len];
		fread( data, len, 1, file );
		fclose( file );

		OSIEncryption::MD5( data, len, data );

		for(int i=0;i<16;i++)
			data[i] ^= data[0x1717+i];

#ifdef _DEBUG
		file = fopen( "razor_checksum.txt", "w" );
		for(int i=0;i<16;i++)
			fprintf( file, "\\x%x", (int)data[i] );
		fprintf( file, "\n" );
		fclose( file );

		Disabled = false;
#else
		Disabled = memcmp( data, CHECKSUM, 16 ) != 0;
#endif
		delete[] data;
	}
	else
	{
		Disabled = true;
	}

	return !Disabled;
}

DLLFUNCTION void *GetSharedAddress()
{
	Log( "Get shared address [0x%x]", pShared );
	return pShared;
}

DLLFUNCTION HWND FindUOWindow( void )
{
	if ( hWatchWnd == NULL || !IsWindow( hWatchWnd ) )
	{
		HWND hWnd = FindWindow( "Ultima Online", NULL );
		if ( hWnd == NULL )
			hWnd = FindWindow( "Ultima Online Third Dawn", NULL );
		return hWnd;
	}
	else
	{
		return hWatchWnd;
	}
}

DLLFUNCTION void SetDataPath( const char *path )
{
	WaitForSingleObject( CommMutex, INFINITE );
	strncpy( pShared->DataPath, path, 256 );
	ReleaseMutex( CommMutex );
}

DLLFUNCTION void SetDeathMsg( const char *msg )
{
	WaitForSingleObject( CommMutex, INFINITE );
	strncpy( pShared->DeathMsg, msg, 16 );
	ReleaseMutex( CommMutex );
	PostMessage( hWatchWnd, WM_UONETEVENT, DEATH_MSG, 0 );
}

void PatchDeathMsg()
{
	if ( DeathMsgAddr == 0xFFFFFFFF )
		DeathMsgAddr = MemFinder::Find( "You are dead.", 14 );

	if ( DeathMsgAddr )
	{
		WaitForSingleObject( CommMutex, INFINITE );
		strncpy( (char*)DeathMsgAddr, pShared->DeathMsg, 16 );
		ReleaseMutex( CommMutex );
	}
}

DLLFUNCTION int InstallLibrary( HWND PostWindow, DWORD pid, int flags )
{
	DWORD UOTId = 0;

	Log( "Initialize library..." );

	if ( Disabled )
		return LIB_DISABLED;

	HWND hWnd = NULL;
	if ( pid != 0 )
	{
		hWnd = FindWindow( "Ultima Online", NULL );
		while ( hWnd != NULL )
		{
			UOTId = GetWindowThreadProcessId( hWnd, &UOProcId );
			if ( UOProcId == pid )
				break;
			hWnd = FindWindowEx( NULL, hWnd, "Ultima Online", NULL );
		}

		if ( UOProcId != pid || hWnd == NULL )
		{
			hWnd = FindWindow( "Ultima Online Third Dawn", NULL );
			while ( hWnd != NULL )
			{
				UOTId = GetWindowThreadProcessId( hWnd, &UOProcId );
				if ( UOProcId == pid )
					break;
				hWnd = FindWindowEx( NULL, hWnd, "Ultima Online Third Dawn", NULL );
			}
		}

		if ( UOProcId != pid )
			return NO_TID;
	}
	else 
	{
		hWnd = FindUOWindow();
		if ( hWnd != NULL )
			UOTId = GetWindowThreadProcessId( hWnd, &UOProcId );
	}

	hWatchWnd = hWnd;
	hPostWnd = PostWindow;

	if ( hWatchWnd == NULL )
		return NO_UOWND;

	if ( !UOTId || !UOProcId )
		return NO_TID;

	if ( !CreateSharedMemory() )
		return NO_SHAREMEM;

	hWndProcRetHook = SetWindowsHookEx( WH_CALLWNDPROCRET, WndProcRetHookFunc, hInstance, UOTId );
	if ( !hWndProcRetHook )
		return NO_HOOK;

	hGetMsgHook = SetWindowsHookEx( WH_GETMESSAGE, GetMsgHookFunc, hInstance, UOTId );
	if ( !hGetMsgHook )
		return NO_HOOK;

	WNDCLASS wc;
	wc.style			= 0;
	wc.lpfnWndProc		= (WNDPROC)UOAWndProc;
	wc.cbClsExtra		= 0;
	wc.cbWndExtra		= 0;
	wc.hInstance		= hInstance;
	wc.hIcon			= LoadIcon(NULL, IDI_WINLOGO);
	wc.hCursor			= LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground	= NULL;	
	wc.lpszMenuName		= NULL;	
	wc.lpszClassName	= "UOASSIST-TP-MSG-WND";
	RegisterClass( &wc );
	DWORD error = GetLastError();

	hUOAWnd = CreateWindow( "UOASSIST-TP-MSG-WND", "UOASSIST-TP-MSG-WND", WS_OVERLAPPEDWINDOW, 0, 0, 50, 50, NULL, NULL, hInstance, 0 );
	if ( hUOAWnd )
		ShowWindow( hUOAWnd, FALSE );

	OSICryptEnabled = (flags&0x2) != 0;

	PostMessage( hWatchWnd, WM_PROCREADY, flags, (LPARAM)hPostWnd );
	return SUCCESS;
}

DLLFUNCTION void WaitForWindow( DWORD pid )
{
	DWORD UOTId = 0;
	DWORD exitCode;
	HANDLE hProc = OpenProcess( PROCESS_QUERY_INFORMATION, FALSE, pid );

	UOProcId = 0;

	do
	{
		Sleep( 25 );
		HWND hWnd = FindWindow( "Ultima Online", NULL );
		while ( hWnd != NULL )
		{
			UOTId = GetWindowThreadProcessId( hWnd, &UOProcId );
			if ( UOProcId == pid )
				break;
			hWnd = FindWindowEx( NULL, hWnd, "Ultima Online", NULL );
		}

		if ( UOProcId != pid || hWnd == NULL )
		{
			hWnd = FindWindow( "Ultima Online Third Dawn", NULL );
			while ( hWnd != NULL )
			{
				UOTId = GetWindowThreadProcessId( hWnd, &UOProcId );
				if ( UOProcId == pid )
					break;
				hWnd = FindWindowEx( NULL, hWnd, "Ultima Online Third Dawn", NULL );
			}
		}

		GetExitCodeProcess( hProc, &exitCode );
	} while ( UOProcId != pid && exitCode == STILL_ACTIVE );

	CloseHandle( hProc );
}

DLLFUNCTION void Shutdown( bool close )
{
	Log( "Shutdown" );

	if ( hUOAWnd && IsWindow( hUOAWnd ) )
	{
		UnregisterClass( "UOASSIST-TP-MSG-WND", hInstance );
		SendMessage( hUOAWnd, WM_CLOSE, 0, 0 );
		hUOAWnd = NULL;
	}
}

DLLFUNCTION int GetUOProcId()
{
	return UOProcId;
}

DLLFUNCTION HANDLE GetCommMutex()
{
	return CommMutex;
}

// totalsend and totalrecv are NOT mutex sync'd
DLLFUNCTION unsigned int TotalOut()
{
	if ( pShared )
		return pShared->TotalSend;
	else
		return 0;
}

DLLFUNCTION unsigned int TotalIn()
{
	if ( pShared )
		return pShared->TotalRecv;
	else
		return 0;
}

DLLFUNCTION bool IsCalibrated()
{
	return pShared && pShared->Position[0] == 0xFFFFFFFF && pShared->Position[1] == 0xDEADBEEF && pShared->Position[2] != 0 && pShared->Position[2] != 0xFFFFFFFF;
}

DLLFUNCTION void CalibratePosition( int x, int y, int z )
{
	pShared->Position[2] = x;
	pShared->Position[1] = y;
	pShared->Position[0] = z;

	PostMessage( hWatchWnd, WM_UONETEVENT, CALIBRATE_POS, 0 );
}

DLLFUNCTION bool GetPosition( int *x, int *y, int *z )
{
	if ( IsCalibrated() )
	{
		int buffer[3];
		DWORD Read = 0;
		HANDLE hProc = OpenProcess( PROCESS_VM_READ, FALSE, UOProcId );
		if ( !hProc )
			return false;

		if ( ReadProcessMemory( hProc, (void*)pShared->Position[2], buffer, sizeof(int)*3, &Read ) )
		{
			if ( Read == sizeof(int)*3 )
			{
				if ( x ) 
					*x = buffer[2];
				if ( y ) 
					*y = buffer[1];
				if ( z ) 
					*z = buffer[0];
			}
			else
			{
				Read = 0;
			}
		}
		else
		{
			Read = 0;
		}

		CloseHandle( hProc );

		if ( Read == sizeof(int)*3 && ( x == NULL || ( *x >= 0 && *x < 8192 ) ) && ( y == NULL || ( *y >= 0 && *y < 8192 ) ) )
		{
			return true;
		}
		else
		{
			memset( pShared->Position, 0, sizeof(int)*3 );
			return false;
		}
	}
	else
	{
		return false;
	}
}

DLLFUNCTION void BringToFront( HWND hWnd )
{
	SetWindowPos( hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE );
	ShowWindow( hWnd, SW_SHOW );
	SetForegroundWindow( hWnd );
	SetFocus( hWnd );
}

#define CHEATPROC_STR "jTjAjHjC"
#define CHEATPROC_LEN 8

DLLFUNCTION void DoFeatures( int features )
{
	char ch;
	int i, size = 9;
	char pkt[256];
	char *str = &pkt[8];
	//(byte)MessageType.Special, 0x02B2, 0x0003, sb
	pkt[0] = 0x03;

	pkt[3] = 0x20;

	pkt[4] = 0x02;
	pkt[5] = 0xB2;

	pkt[6] = 0x00;
	pkt[7] = 0x03;

	size += sprintf( str, "%c%cE%c%c %s 1 %i--\x00", 'C', 'H', 'A', 'T', "UO.exe", features );

	pkt[1] = (size>>8)&0xFF;
	pkt[2] = size&0xFF;

	i = 0;
	do
	{
		ch = str[i];
		str[i] = ch ^ pShared->CheatKey[i & 0xF];
		i++;
	} while ( ch != 0 );

	WaitForSingleObject( CommMutex, 50 );
	memcpy( pShared->OutSend.Buff + pShared->OutSend.Start + pShared->OutSend.Length, pkt, size );
	pShared->OutSend.Length += size;
	ReleaseMutex( CommMutex );
	PostMessage( FindUOWindow(), WM_UONETEVENT, SEND, 0 );
}

DLLFUNCTION bool AllowBit( unsigned int bit )
{
	bit &= 0x0000003F; // limited to 64 bits
	if ( !pShared )
		return true;
	else
		return ( pShared->AuthBits[7-(bit/8)] & (1<<(bit%8)) ) == 0;
}

DLLFUNCTION void AddProperty( const char *propName )
{
	size_t len = strlen( propName );
	int mode = 0;

	// basically search for "IDOC" in the property name to catch Razor hackers
	for(size_t i=0;i<len;i++)
	{
		switch ( propName[i] )
		{
		case 0x17:
			mode ^= 0x04;
			break;

		case 'C':
			if ( mode == 7 )
				mode -= 5;
			else
				mode = 0;
			break;

		case 'D':
			if ( mode == 3 )
				mode -= 6; 
			else
				mode = 0;
			break;

		default:
			if ( mode != 2 && mode != 35 && mode != 83 && mode != 87 )
			{
				mode ^= 0x41783;
				mode ^= 0x91183;
				mode ^= 0x41783;
				mode ^= 0x91183;

				mode *= 0;
			}
			break;

		case 'I':
			if ( mode == 0 )
				mode += 3;
			else
				mode = 0;
			break;

		case 'O':
			if ( mode == -3 )
				mode += 10;
			else
				mode = 0;
			break;

		case 0x04:
			mode ^= 0x17;
			break;
		}
	}

	if ( mode == -1 || mode == 2 || mode == 87 || mode == 0x04171983 )
		pShared->IsHaxed = true;
}

DLLFUNCTION void SetAllowDisconn( bool newVal )
{
	if ( pShared && CommMutex )
	{
		WaitForSingleObject( CommMutex, INFINITE );
		pShared->AllowDisconn = newVal;
		ReleaseMutex( CommMutex );
	}
}

bool CreateSharedMemory()
{
	char name[512];
	Log( "Creating shared mem, proc: %x", UOProcId );

	sprintf( name, "UONetSharedCOMM_%x", UOProcId );
	CommMutex = CreateMutex( NULL, FALSE, name );
	if ( !CommMutex )
		return false;

	sprintf( name, "UONetSharedFM_%x", UOProcId );
	hFileMap = CreateFileMapping( INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SharedMemory), name );
	if ( !hFileMap )
		return false;

	pShared = (SharedMemory*)MapViewOfFile( hFileMap, FILE_MAP_ALL_ACCESS, 0, 0, 0 );
	if ( !pShared )
		return false;

	memset( pShared, 0, sizeof(SharedMemory) );

	pShared->AllowDisconn = true;

	return true;
}

void CloseSharedMemory()
{
	Log( "Close shared memory" );

	if ( hWndProcRetHook )
		UnhookWindowsHookEx( hWndProcRetHook );
	if ( hGetMsgHook )
		UnhookWindowsHookEx( hGetMsgHook );

	if ( CommMutex )
		CloseHandle( CommMutex );
	CommMutex = NULL;
	if ( pShared )
		UnmapViewOfFile( pShared );
	pShared = NULL;
	if ( hFileMap )
		CloseHandle( hFileMap );
	hFileMap = NULL;

	//these are shared vars
	hWndProcRetHook = NULL;
	hGetMsgHook = NULL;

	FreeArt();

	delete ClientCrypt;
	delete ServerCrypt;
	delete ClientLogin;
	delete ServerLogin;
}

bool CreateEncryption()
{
	ClientCrypt = new OSIEncryption();
	ServerCrypt = new OSIEncryption();
	ClientLogin = new LoginEncryption();
	ServerLogin = new LoginEncryption();

	return true;
}

void Maintenance( Buffer &buff )
{
	if ( buff.Length <= 0 )
	{
		buff.Start = 0;
		buff.Length = 0;
	}
	else if ( buff.Start > SHARED_BUFF_SIZE / 2 )
	{
		//shift all the data to the begining of the buffer
		memmove( buff.Buff, &buff.Buff[buff.Start], buff.Length );
		buff.Start = 0;
	}
}

int RecvData()
{
	int len = SHARED_BUFF_SIZE;
	char buff[SHARED_BUFF_SIZE];

	int ackLen = (*(NetIOFunc)OldRecv)(CurrentConnection,buff,len,0);

	if ( ackLen == SOCKET_ERROR )
	{
		if ( WSAGetLastError() != WSAEWOULDBLOCK )
		{
			WaitForSingleObject( CommMutex, INFINITE );
			pShared->ForceDisconn = true;
			ReleaseMutex( CommMutex );
		}
		else
		{
			WSASetLastError( WSAEWOULDBLOCK );
		}

		ackLen = -1;
	}
	else if ( ackLen > 0 )
	{
		if ( FirstRecv )
		{
			Compression::Reset();
			FirstRecv = false;
		}

		WaitForSingleObject( CommMutex, INFINITE );

		pShared->TotalRecv += ackLen;

		if ( LoginServer )
		{
			memcpy( &pShared->InRecv.Buff[pShared->InRecv.Start+pShared->InRecv.Length], buff, ackLen );
			pShared->InRecv.Length += ackLen;
		}
		else
		{
			if ( OSICryptEnabled )
				ServerCrypt->DecryptFromServer( (BYTE*)buff, (BYTE*)buff, ackLen );

			if ( tempBuff == NULL )
				tempBuff = new char[SHARED_BUFF_SIZE];

			int blen = Compression::Decompress( tempBuff, buff, ackLen );
			memcpy( &pShared->InRecv.Buff[pShared->InRecv.Start+pShared->InRecv.Length], tempBuff, blen );
			pShared->InRecv.Length += blen;

			if ( !InGame && AllowNegotiate && !ServerNegotiated && pShared )
			{
				int pos = pShared->InRecv.Start;
				unsigned char *p_buff = &pShared->InRecv.Buff[pos];

				while ( pos < pShared->InRecv.Length )
				{
					int left = pShared->InRecv.Length - pos;
					int p_len = GetPacketLength( p_buff, left );

					if ( *p_buff == 0xA9 && p_len >= 1+2+1+30+30 && p_len <= left )
					{
						unsigned char hash[16], test[16];

						memcpy( pShared->AuthBits, p_buff + 1+2+1+30+1, 8 );
						
						if ( p_buff[3] > 1 )
							memcpy( hash, p_buff + 1+2+1+30+30+30+1, 16 );
						else
							memcpy( hash, p_buff + 1+2+1+30+1+8, 16 );

						for ( int i = 0; i < p_buff[3]; i++ )
							memset( p_buff + 1+2+1+ 30 + 60*i, 0, 30 );

						if ( memcmp( hash, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 16 ) != 0 )
						{
							OSIEncryption::MD5( p_buff, p_len, test );

							ServerNegotiated = memcmp( hash, test, 16 ) == 0;
						}

						if ( !ServerNegotiated )
							memset( pShared->AuthBits, 0, 8 );

						break;
					}
					
					if ( p_len <= 0 )
					{
						break;
					}
					else
					{
						pos += p_len;
						p_buff += p_len;
					}
				}
			}
		}

		ReleaseMutex( CommMutex );

		SendMessage( hPostWnd, WM_UONETEVENT, RECV, 0 );
	}

	return ackLen;
}

int PASCAL HookRecv( SOCKET sock, char *buff, int len, int flags )
{
	int ackLen;

	if ( sock == CurrentConnection && CurrentConnection )
	{
		WaitForSingleObject( CommMutex, INFINITE );
		if ( pShared->ForceDisconn && pShared->AllowDisconn && pShared->OutRecv.Length <= 0 )
		{
			ReleaseMutex( CommMutex );
			WSASetLastError( WSAECONNRESET );
			return -1;
		}

		if ( LoginServer )
		{
			if ( pShared->OutRecv.Length > 0 )
			{
				ackLen = pShared->OutRecv.Length;
				memcpy( buff, &pShared->OutRecv.Buff[pShared->OutRecv.Start], ackLen );

				if ( ((BYTE)buff[0]) == ((BYTE)0x8C) )
					LoginServer = false;

				pShared->OutRecv.Start += ackLen;
				pShared->OutRecv.Length -= ackLen;
			}
			else
			{
				ackLen = 0;
			}
		}
		else
		{
			ackLen = 0;
			while ( pShared->OutRecv.Length > 0 )
			{
				int blen = GetPacketLength( &pShared->OutRecv.Buff[pShared->OutRecv.Start], pShared->OutRecv.Length );

				if ( blen <= 0 || blen > pShared->OutRecv.Length || ackLen+blen > len )
					break;

				if ( tempBuff == NULL )
					tempBuff = new char[SHARED_BUFF_SIZE];
				memcpy( tempBuff, &pShared->OutRecv.Buff[pShared->OutRecv.Start], blen );
				pShared->OutRecv.Start += blen;
				pShared->OutRecv.Length -= blen;

				ackLen += Compression::Compress( &buff[ackLen], tempBuff, blen );
			}

			if ( OSICryptEnabled && ackLen > 0 )
				ClientCrypt->EncryptForClient( (BYTE*)buff, (BYTE*)buff, ackLen );
		}

		Maintenance( pShared->InRecv );
		Maintenance( pShared->OutRecv );

		ReleaseMutex( CommMutex );

		if ( ackLen == 0 )
		{
			WSASetLastError( WSAEWOULDBLOCK );
			return -1;
		}
		else
		{
			return ackLen;
		}
	}
	else
	{
		return (*(NetIOFunc)OldRecv)(sock,buff,len,flags);
	}
}

int PASCAL HookSend( SOCKET sock, char *buff, int len, int flags )
{
	int ackLen;

	if ( sock == CurrentConnection && CurrentConnection )
	{
		if ( !Seeded )
		{
			Seeded = true;

			if ( OSICryptEnabled && len >= 4 )
			{
				ClientCrypt->Initialize( *((DWORD*)buff) );
				ServerCrypt->Initialize( *((DWORD*)buff) );
				ClientLogin->Initialize( (BYTE*)buff );
				ServerLogin->Initialize( (BYTE*)buff );

				Compression::Reset();
			}

			ackLen = (*(NetIOFunc)OldSend)(sock,buff,len,flags);
			pShared->TotalSend += len;
		}
		else
		{
			if ( FirstSend )
			{
				FirstSend = false;

				if ( OSICryptEnabled )
					LoginServer = ClientLogin->Test( (BYTE)buff[0] ) == ((BYTE)0x80);
				else
					LoginServer = ((BYTE)buff[0]) == ((BYTE)0x80);
			}

			WaitForSingleObject( CommMutex, INFINITE );

			memcpy( &pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length], buff, len );

			if ( OSICryptEnabled )
			{
				if ( LoginServer )
					ClientLogin->Decrypt( (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), len );
				else
					ClientCrypt->DecryptFromClient( (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), len );
			}

			pShared->InSend.Length += len;
			ReleaseMutex( CommMutex );

			SendMessage( hPostWnd, WM_UONETEVENT, SEND, 0 );

			WaitForSingleObject( CommMutex, INFINITE );
			FlushSendData();
			Maintenance( pShared->InSend );
			ReleaseMutex( CommMutex );

			ackLen = len;//lie and say we sent it all -- or should we tell the truth? (probably not since then it could try to send it again..)
		}
	}
	else
	{
		ackLen = (*(NetIOFunc)OldSend)(sock,buff,len,flags);
	}

	return ackLen;
}

#define RAZOR_ID_KEY "\x9\x11\x83+\x4\x17\x83\x5\x24\x85\x7\x17\x87\x6\x19\x88"
#define RAZOR_ID_KEY_LEN 16

void FlushSendData()
{
	WaitForSingleObject( CommMutex, INFINITE );
	if ( pShared->OutSend.Length > 0 && CurrentConnection )
	{
		int ackLen = 0;
		int outLen = pShared->OutSend.Length;

		if ( !InGame && !LoginServer )
		{
			int pos = pShared->OutSend.Start;
			unsigned char *buff = &pShared->OutSend.Buff[pos];

			while ( pos < outLen )
			{
				int left = pShared->OutSend.Length - pos;
				int len = GetPacketLength( buff, left );

				if ( *buff == 0x5D && len >= 1+4+30+30 && len <= left )
				{
					// play character
					if ( AllowNegotiate && ServerNegotiated )
					{
						// the first 2 bytes are 0
						// the next 4 bytes are "flags" which say the user's client type (lbr,t2a,aos,etc)
						// the rest are ignored, so we can use them for auth
						memcpy( buff + 1 + 4 + 30 + 2 + 4, pShared->AuthBits, 8 );
						memcpy( buff + 1 + 4 + 30 + 2 + 4 + 8, RAZOR_ID_KEY, RAZOR_ID_KEY_LEN );
					}

					InGame = true;

					if ( pShared->IsHaxed )
						memcpy( buff + 1 + 4 + 30 + 28, "\xDE\xAD", 2 );

					break;
				}
				else if ( *buff == 0x00 && (*((DWORD*)&buff[1])) == 0xEDEDEDED && len >= 1+4+4+1+30+30 && len <= left )
				{
					// char creation
					if ( AllowNegotiate && ServerNegotiated )
					{
						memcpy( buff + 1 + 4 + 4 + 1 + 30 + 15, pShared->AuthBits, 8 );
						memcpy( buff + 1 + 4 + 4 + 1 + 30 + 15 + 8, RAZOR_ID_KEY, min( 7, RAZOR_ID_KEY_LEN ) );
					}

					InGame = true;

					if ( pShared->IsHaxed )
						memcpy( buff + 1 + 4 + 30 + 28, "\xDE\xAD", 2 );
					break;
				}
				
				if ( len <= 0 )
				{
					break;
				}
				else
				{
					pos += len;
					buff += len;
				}
			} // END while
		} // END if ( !InGame && !LoginServer
		
		if ( tempBuff == NULL )
			tempBuff = new char[SHARED_BUFF_SIZE];

		memcpy( tempBuff, &pShared->OutSend.Buff[pShared->OutSend.Start], outLen );

		if ( OSICryptEnabled )
		{
			if ( LoginServer )
				ServerLogin->Encrypt( (BYTE*)tempBuff, (BYTE*)tempBuff, outLen );
			else
				ServerCrypt->EncryptForServer( (BYTE*)tempBuff, (BYTE*)tempBuff, outLen );
		}

		ackLen = (*(NetIOFunc)OldSend)(CurrentConnection,(char*)tempBuff,outLen,0);

		if ( ackLen == SOCKET_ERROR )
		{
			if ( WSAGetLastError() != WSAEWOULDBLOCK )
				pShared->ForceDisconn = true;
		}
		else //if ( ackLen >= 0 )
		{
			pShared->TotalSend += ackLen;

			pShared->OutSend.Start += ackLen;
			pShared->OutSend.Length -= ackLen;
		}
	}

	Maintenance( pShared->OutSend );
	ReleaseMutex( CommMutex );
}

int PASCAL HookConnect( SOCKET sock, const sockaddr *addr, int addrlen )
{
	int retVal = (*(ConnFunc)OldConnect)( sock, addr, addrlen );

	if ( retVal != SOCKET_ERROR )
	{
		Log( "Connecting to %i", sock );

		if ( OSICryptEnabled )
			CreateEncryption();

		Seeded = false;
		LoginServer = false;
		FirstRecv = true;
		FirstSend = true;

		WaitForSingleObject( CommMutex, INFINITE );
		CurrentConnection = sock;
		pShared->OutRecv.Length = pShared->InRecv.Length = pShared->OutSend.Length = pShared->InSend.Length = 0;
		pShared->ForceDisconn = false;
		ReleaseMutex( CommMutex );

		PostMessage( hPostWnd, WM_UONETEVENT, CONNECT, ((sockaddr_in*)addr)->sin_addr.S_un.S_addr );
	}

	return retVal;
}

int PASCAL HookCloseSocket( SOCKET sock )
{
	int retVal = (*(CLSFunc)OldCloseSocket)( sock );

	if ( sock == CurrentConnection && sock != 0 )
	{
		Log( "Closing socket %i", sock );
		CurrentConnection = 0;

		WaitForSingleObject( CommMutex, INFINITE );
		pShared->OutRecv.Length = pShared->InRecv.Length = pShared->OutSend.Length = pShared->InSend.Length = 0;
		memset( pShared->Position, 0, 4*3 );
		pShared->TotalSend = pShared->TotalRecv = 0;
		pShared->ForceDisconn = false;
		ReleaseMutex( CommMutex );
		
		ServerNegotiated = false;
		InGame = false;
		
		memset( pShared->AuthBits, 0, 8 );

		PostMessage( hPostWnd, WM_UONETEVENT, DISCONNECT, 0 );
	}

	return retVal;
}

int PASCAL HookSelect( int ndfs, fd_set *readfd, fd_set *writefd, fd_set *exceptfd, const struct timeval *timeout )
{
	bool checkRecv = false;
	bool checkErr = false;
	bool modified = false;
	int realRet = 0;
	int myRet = 0;

	if ( CurrentConnection )
	{
		if ( readfd != NULL ) 
			checkRecv = FD_ISSET( CurrentConnection, readfd );

		if ( exceptfd != NULL ) 
			checkErr = FD_ISSET( CurrentConnection, exceptfd );
	}

	timeval myTimeout;

	if ( SmartCPU )
	{
		int length = 0;

		if ( Active )
		{
			LARGE_INTEGER end;
			QueryPerformanceCounter( &end );

			length = int( 1000000.0 * ((end.QuadPart-Counter.QuadPart)/double(PerfFreq.QuadPart)) );
		}

		if ( length < 33333 )
		{
			myTimeout.tv_sec = 0;
			myTimeout.tv_usec = 33333 - length;
			timeout = &myTimeout;
		}
	}

	realRet = (*(SelectFunc)OldSelect)( ndfs, readfd, writefd, exceptfd, timeout );

	if ( SmartCPU )
		QueryPerformanceCounter( &Counter );

	if ( checkRecv )
	{
		if ( FD_ISSET( CurrentConnection, readfd ) )
		{
			FD_CLR( CurrentConnection, readfd );
			RecvData();
			realRet--;
		}

		WaitForSingleObject( CommMutex, INFINITE );
		if ( pShared->OutRecv.Length > 0 || ( pShared->ForceDisconn && pShared->AllowDisconn ) )
		{
			FD_SET( CurrentConnection, readfd );
			myRet++;
		}
		ReleaseMutex( CommMutex );
	}

	if ( checkErr && !FD_ISSET( CurrentConnection, exceptfd ) )
	{
		WaitForSingleObject( CommMutex, INFINITE );
		if ( pShared->ForceDisconn && pShared->AllowDisconn && pShared->OutRecv.Length <= 0 )
		{
			FD_SET( CurrentConnection, exceptfd );
			myRet++;
		}
		ReleaseMutex( CommMutex );
	}

	if ( realRet < 0 )
	{
		return myRet;
	}
	else
	{
		return realRet + myRet;
	}
}

bool HookFunction( const char *Dll, const char *FuncName, int Ordinal, unsigned long NewAddr,
				  unsigned long *OldAddr, unsigned long *PatchAddr )
{
	DWORD baseAddr = (DWORD)GetModuleHandle(NULL);
	if ( !baseAddr ) 
		return false;

	IMAGE_DOS_HEADER *idh = (IMAGE_DOS_HEADER *)baseAddr;

	IMAGE_FILE_HEADER *ifh = (IMAGE_FILE_HEADER *)(baseAddr + idh->e_lfanew + sizeof(DWORD));

	IMAGE_OPTIONAL_HEADER *ioh = (IMAGE_OPTIONAL_HEADER *)((DWORD)(ifh) + sizeof(IMAGE_FILE_HEADER));

	IMAGE_IMPORT_DESCRIPTOR *iid = (IMAGE_IMPORT_DESCRIPTOR *)(baseAddr + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	while( iid->Name )
	{
		if( stricmp( Dll, (char *)(baseAddr + iid->Name) ) == 0 )
		{
			IMAGE_THUNK_DATA * pThunk = (IMAGE_THUNK_DATA *)((DWORD)iid->OriginalFirstThunk + baseAddr);
			IMAGE_THUNK_DATA * pThunk2 = (IMAGE_THUNK_DATA *)((DWORD)iid->FirstThunk + baseAddr);

			while( pThunk->u1.AddressOfData )
			{
				char *name = NULL;
				int ord;

				if( pThunk->u1.Ordinal & 0x80000000 )
				{
					// Imported by ordinal only:
					ord = pThunk->u1.Ordinal & 0xFFFF;
				}
				else
				{
					// Imported by name (with ordinal hint)
					IMAGE_IMPORT_BY_NAME * pName = (IMAGE_IMPORT_BY_NAME *)((DWORD)pThunk->u1.AddressOfData + baseAddr);
					ord = pName->Hint;
					name = (char *)pName->Name;
				}

				if ( ord == Ordinal || ( name && FuncName && !strcmp( name, FuncName ) ) )
				{
					*OldAddr = (unsigned long)pThunk2->u1.Function;
					*PatchAddr = (unsigned long)(&pThunk2->u1.Function);
					MemoryPatch( *PatchAddr, NewAddr );

					return true;
				}

				pThunk++;
				pThunk2++;
			}
		}
		iid++;
	}

	return false;
}


bool FindFunction( const char *Dll, const char *FuncName, int Ordinal, unsigned long *ImpAddr, unsigned long *CallAddr )
{
	DWORD baseAddr = (DWORD)GetModuleHandle(NULL);
	if ( !baseAddr ) 
		return false;

	IMAGE_DOS_HEADER *idh = (IMAGE_DOS_HEADER *)baseAddr;

	IMAGE_FILE_HEADER *ifh = (IMAGE_FILE_HEADER *)(baseAddr + idh->e_lfanew + sizeof(DWORD));

	IMAGE_OPTIONAL_HEADER *ioh = (IMAGE_OPTIONAL_HEADER *)((DWORD)(ifh) + sizeof(IMAGE_FILE_HEADER));

	IMAGE_IMPORT_DESCRIPTOR *iid = (IMAGE_IMPORT_DESCRIPTOR *)(baseAddr + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	while( iid->Name )
	{
		if( stricmp( Dll, (char *)(baseAddr + iid->Name) ) == 0 )
		{
			IMAGE_THUNK_DATA * pThunk = (IMAGE_THUNK_DATA *)((DWORD)iid->OriginalFirstThunk + baseAddr);
			IMAGE_THUNK_DATA * pThunk2 = (IMAGE_THUNK_DATA *)((DWORD)iid->FirstThunk + baseAddr);

			while( pThunk->u1.AddressOfData )
			{
				char *name = NULL;
				int ord;

				if( pThunk->u1.Ordinal & 0x80000000 )
				{
					// Imported by ordinal only:
					ord = pThunk->u1.Ordinal & 0xFFFF;
				}
				else
				{
					// Imported by name (with ordinal hint)
					IMAGE_IMPORT_BY_NAME * pName = (IMAGE_IMPORT_BY_NAME *)((DWORD)pThunk->u1.AddressOfData + baseAddr);
					ord = pName->Hint;
					name = (char *)pName->Name;
				}

				if ( ord == Ordinal || ( name && FuncName && !strcmp( name, FuncName ) ) )
				{
					*ImpAddr = (unsigned long)pThunk2->u1.Function;
					*CallAddr = (unsigned long)(&pThunk2->u1.Function);

					return true;
				}

				pThunk++;
				pThunk2++;
			}
		}
		iid++;
	}

	return false;
}

DWORD GumpOutV28( DWORD OutFuncAddr, int esiOff, char TwoFourOff, DWORD CurAddr, int x, int y )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x8D\x44\x24\x20", 4 ); // lea    eax, [esp+70h+var_50]
	count += 4;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x8B\x46", 2 ); // mov    eax, [esi+24h]
	count += 2;
	*((unsigned char*)(CurAddr+count)) = 0x24+TwoFourOff;//+esiOff;
	count ++;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x8B\x56", 2 ); // mov     edx, [esi+44h]
	count += 2;
	*((unsigned char*)(CurAddr+count)) = 0x44+esiOff;
	count ++;
	memcpy( (void*)(CurAddr+count), "\x8B\x42\x14", 3 ); // mov     eax, [edx+14h]
	count += 3;
	memcpy( (void*)(CurAddr+count), "\x83\xC0\x04", 3 ); // add     eax, 4
	count += 3;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x8D\x44\x24\x54", 4 ); // lea    eax, [esp+7Ch+var_28]
	count += 4;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x68\x86\x03\x00\x00", 5 ); // push    386h
	count += 5;

	memcpy( (void*)(CurAddr+count), "\x6A\x01", 2 ); // push    1
	count += 2;

	// push    y
	if ( y < 0x80 )
	{
		*((unsigned char*)(CurAddr+count)) = 0x6A;
		count ++;
		*((unsigned char*)(CurAddr+count)) = (unsigned char)y;
		count ++;
	}
	else
	{
		*((unsigned char*)(CurAddr+count)) = 0x68;
		count ++;
		*((int*)(CurAddr+count)) = y;
		count += 4;
	}

	// push    x
	if ( x < 0x80 )
	{
		*((unsigned char*)(CurAddr+count)) = 0x6A;
		count ++;
		*((unsigned char*)(CurAddr+count)) = (unsigned char)x;
		count ++;
	}
	else
	{
		*((unsigned char*)(CurAddr+count)) = 0x68;
		count ++;
		*((int*)(CurAddr+count)) = x;
		count += 4;
	}

	// call theFunction
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = OutFuncAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x20", 3 ); // add    esp, 20h
	count += 3;

	return count;
}

DWORD Sprintf1UV28( DWORD sprintfAddr, DWORD fmtAddr, DWORD CurAddr, int esiOffset )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x33\xC9", 2 ); // xor     ecx, ecx
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8D\x54\x24\x48", 4 ); // lea     edx, [esp+70h+var_28]
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
	count += 3;
	*((int*)(CurAddr+count)) = esiOffset;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x51", 1 ); // push    ecx
	count ++;

	// push offset fmtAddr
	*((unsigned char*)(CurAddr+count)) = 0x68;
	count ++;
	*((int*)(CurAddr+count)) = fmtAddr;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x52", 1 ); // push    edx
	count ++;

	// call    _sprintf
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = sprintfAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x0C", 3 ); // add    esp, 0Ch
	count += 3;

	return count;
}

DWORD Sprintf2UV28_Byte( DWORD sprintfAddr, DWORD fmtAddr, DWORD CurAddr, int offset1, int offset2 )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x33\xC0", 2 ); // xor     eax, eax
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8A\x86", 2 ); // mov     al, [esi+___]
	count += 2;
	*((int*)(CurAddr+count)) = offset2;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x33\xC9", 2 ); // xor     ecx, ecx
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8D\x54\x24\x4C", 4 ); // lea     edx, [esp+70h+var_28]
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x8A\x8E", 2 ); // mov     cl, [esi+___]
	count += 2;
	*((int*)(CurAddr+count)) = offset1;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x51", 1 ); // push    ecx
	count ++;

	// push offset fmtAddr
	*((unsigned char*)(CurAddr+count)) = 0x68;
	count ++;
	*((int*)(CurAddr+count)) = fmtAddr;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x52", 1 ); // push    edx
	count ++;

	// call    _sprintf
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = sprintfAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x10", 3 ); // add    esp, 10h
	count += 3;

	return count;
}

DWORD Sprintf2UV28( DWORD sprintfAddr, DWORD fmtAddr, DWORD CurAddr, int offset1, int offset2 )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x33\xC0", 2 ); // xor     eax, eax
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x66\x8B\x86", 3 ); // mov     ax, [esi+___]
	count += 3;
	*((int*)(CurAddr+count)) = offset2;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x33\xC9", 2 ); // xor     ecx, ecx
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8D\x54\x24\x4C", 4 ); // lea     edx, [esp+70h+var_28]
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
	count += 3;
	*((int*)(CurAddr+count)) = offset1;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x51", 1 ); // push    ecx
	count ++;

	// push offset fmtAddr
	*((unsigned char*)(CurAddr+count)) = 0x68;
	count ++;
	*((int*)(CurAddr+count)) = fmtAddr;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x52", 1 ); // push    edx
	count ++;

	// call    _sprintf
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = sprintfAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x10", 3 ); // add    esp, 10h
	count += 3;

	return count;
}

/*#define STATCODE_BEG "\x8D\x54\x24\x48\x8B\xC3\x2B\xD3\x8A\x08\x88\x0C\x02\x40\x84\xC9\x75\xF6"
#define STATCODE_B_LEN 18
#define STATCODE_START_OFF (STATCODE_B_LEN+0x15)
#define STACK_OFFSET -3
#define STACK_BASE 0x28
*/
#define STATCODE_BEG "\x68\x86\x03\x00\x00\x6A\x01\x6A\x32\x6A\x28\xE8"
#define STATCODE_B_LEN 12
#define STATCODE_START_OFF (-0x1E)
#define STACK_OFFSET 6
#define STACK_BASE 0x28

#define STATCODE_END "\x51\x52\x68\x86\x03\x00\x00\x6A\x01\x68\x8F\x00\x00\x00\x00"
#define STATCODE_E_LEN 14
#define STATCODE_END_OFFSET 9

#define GUMP_STRING1 "\x53\x55\x8B\x6C\x24\x1C\x56\x57\x8B\xFD\x83\xC9\xFF\x33\xC0\xF2"
#define GS1_LEN 16
#define GS1_OFFSET 0x29

#define GID_REPL "\x6C\x2A\x00\x00"
#define GID_R_LEN 4

#define SPRINTF_STR "\x55\x8B\xEC\x83\xEC\x20\x8B\x45\x08\x56\x89\x45\xE8\x89\x45\xE0\x8D\x45\x10\xC7\x45\xEC\x42\x00\x00\x00\x50\x8D\x45\xE0\xFF\x75\x0C\xC7\x45\xE4\xFF\xFF\xFF\x7F\x50\xE8"
#define SPRINTF_LEN 42

#define LOCK_STR_1 "\x6A\x4E\x6A\x28"
#define LOCK_STR_2 "\x6A\x69\x6A\x28"
#define LOCK_STR_3 "\x68\x85\x00\x00\x00\x6A\x28"
#define LOCK_LEN_1 4
#define LOCK_LEN_2 4
#define LOCK_LEN_3 7

#define MIN_STR "\x68\xA8\x00\x00\x00\x68\x98\x01\x00\x00\x68\x92\x00\x00\x00\x68\x81\x01\x00\x00"
#define MIN_STR_LEN 20

vector<PatchInfo *> *PatchVec = NULL;
bool PatchStatusBar( BOOL preAOS )
{
	if ( !preAOS )
	{
		if ( PatchVec == NULL )
			return true;

		for(unsigned int i=0;i<PatchVec->size();i++)
		{
			PatchInfo *patch = (*PatchVec)[i];
			MemoryPatch( patch->Address, patch->Data, patch->Length );
			delete patch;
		}
		delete PatchVec;
		PatchVec = NULL;
		return true;
	}

	if ( PatchVec != NULL )
		return true;

	const unsigned int NewGumpID = 0x00000802;

	MemFinder mf;

	mf.AddEntry( GUMP_STRING1, GS1_LEN );
	mf.AddEntry( "%u/%u", 6 );
	mf.AddEntry( MIN_STR, MIN_STR_LEN );
	mf.AddEntry( STATCODE_BEG, STATCODE_B_LEN );
	mf.AddEntry( SPRINTF_STR, SPRINTF_LEN );
	mf.AddEntry( LOCK_STR_1, LOCK_LEN_1 );
	mf.AddEntry( GID_REPL, GID_R_LEN );

	mf.Execute();

	DWORD GumpString1 = mf.GetAddress( GUMP_STRING1, GS1_LEN );
	if ( !GumpString1 ) 
		return false;
	GumpString1 -= GS1_OFFSET;

	DWORD uuFmt = mf.GetAddress( "%u/%u", 6 );
	if ( !uuFmt ) 
		return false;
	DWORD uFmt = uuFmt + 3;

	DWORD MinLoc = mf.GetAddress( MIN_STR, MIN_STR_LEN );
	if ( !MinLoc ) 
		return false;
	MinLoc += 1;

	DWORD Start = mf.GetAddress( STATCODE_BEG, STATCODE_B_LEN );
	if ( !Start ) 
		return false;
	Start += STATCODE_START_OFF;

	DWORD End = Start;

	do {
		End = MemFinder::Find( STATCODE_END, STATCODE_E_LEN, End + 1 );
	} while ( End && *((unsigned char*)(End+STATCODE_E_LEN+1)) != 0xE8 );

	if ( !End )
		return false;

	End += STATCODE_E_LEN + STATCODE_END_OFFSET;

	DWORD sprintfAddr = mf.GetAddress( SPRINTF_STR, SPRINTF_LEN );
	DWORD LockLoc = mf.GetAddress( LOCK_STR_1, LOCK_LEN_1 );
	DWORD CurPos = mf.GetAddress( GID_REPL, GID_R_LEN );

	if ( !( sprintfAddr && LockLoc && CurPos ) )
		return false;

	PatchVec = new vector<PatchInfo*>();

	PatchVec->push_back( new PatchInfo( CurPos, 4 ) );
	MemoryPatch( CurPos, NewGumpID );
	CurPos = MemFinder::Find( GID_REPL, GID_R_LEN, CurPos+4 );
	if ( CurPos )
	{
		PatchVec->push_back( new PatchInfo( CurPos, 4 ) );
		MemoryPatch( CurPos, NewGumpID );
	}

	PatchVec->push_back( new PatchInfo( LockLoc+0x01, 1 ) );
	PatchVec->push_back( new PatchInfo( LockLoc+0x03, 1 ) );
	MemoryPatch( LockLoc+0x01, 0x3E, 1 );
	MemoryPatch( LockLoc+0x03, 0x26, 1 );

	LockLoc = MemFinder::Find( LOCK_STR_2, LOCK_LEN_2, LockLoc );
	if ( LockLoc )
	{
		PatchVec->push_back( new PatchInfo( LockLoc+0x01, 1 ) );
		PatchVec->push_back( new PatchInfo( LockLoc+0x03, 1 ) );
		MemoryPatch( LockLoc+0x01, 0x4A, 1 );
		MemoryPatch( LockLoc+0x03, 0x26, 1 );

		LockLoc = MemFinder::Find( LOCK_STR_3, LOCK_LEN_3, LockLoc );
		if ( LockLoc )
		{
			PatchVec->push_back( new PatchInfo( LockLoc+0x01, 4 ) );
			PatchVec->push_back( new PatchInfo( LockLoc+0x06, 1 ) );
			MemoryPatch( LockLoc+0x01, 0x56, 4 );
			MemoryPatch( LockLoc+0x06, 0x26, 1 );
		}
	}

	PatchVec->push_back( new PatchInfo( MinLoc+ 0, 4 ) );
	PatchVec->push_back( new PatchInfo( MinLoc+ 5, 4 ) );
	PatchVec->push_back( new PatchInfo( MinLoc+10, 4 ) );
	PatchVec->push_back( new PatchInfo( MinLoc+15, 4 ) );
	MemoryPatch( MinLoc+ 0, 137 );
	MemoryPatch( MinLoc+ 5, 263 );
	MemoryPatch( MinLoc+10, 111 );
	MemoryPatch( MinLoc+15, 239 );

	int len = End - Start;

	PatchVec->push_back( new PatchInfo( Start, len ) );

	DWORD OldProtect;
	if ( !VirtualProtect( (void *)(Start), len, PAGE_EXECUTE_READWRITE, &OldProtect ) )
		return false;

	CurPos = Start;

	int statOffset = *((unsigned char*)(Start + 0x31 + 0x18 + 3));
	int esiOffset = statOffset - 0xD4;
	int stackChange = STACK_BASE - *((unsigned char*)(Start + STACK_OFFSET));
	int twoFourOff = *((unsigned char*)(Start + 0x31 + 2)) - 0x24;

	bool client5 = false;
	char uuPush[5];
	uuPush[0] = 0x68;
	*((DWORD*)&uuPush[1]) = uuFmt;

	DWORD uuSP = MemFinder::Find( uuPush, 5, Start );
	if ( uuSP )
		client5 = ( *((int*)(uuSP - 10)) != (statOffset + 0x1F) );

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = (char)stackChange;
		CurPos++;
	}

	if ( *((unsigned char*)(Start + 11)) != 0x46 ) // was reg is it in?
	{
		memcpy( (void*)CurPos, "\x8B\xF5", 2 ); // mov     esi, ebp
		CurPos += 2;
	}

	// name
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 53 );

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = -stackChange;
		CurPos++;
	}

	memset( (void*)CurPos, 0x90, (Start+0x31) - CurPos );

	CurPos = Start + 0x31 + 0x12;

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = (char)stackChange;
		CurPos++;
	}

	if ( *((unsigned char*)(Start + 11)) != 0x46 ) // was reg is it in?
	{
		memcpy( (void*)CurPos, "\x8B\xF5", 2 ); // mov     esi, ebp
		CurPos += 2;
	}

	// str
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x00 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 72 );

	// dex
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x02 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 84 );

	// int
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x04 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 96 );

	// sex?
	//memcpy( (void*)(CurPos), "\xC7\x44\x24\x48Yes", 8 ); // mov     [esp+70h+var_28], __
	//CurPos += 8;
	//CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 108 );

	// followers:
	if ( client5 )
	{
		CurPos += Sprintf2UV28_Byte( sprintfAddr, uuFmt, CurPos, statOffset+2+0x1E, statOffset+2+0x1F );
		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 108 );
	}
	else
	{
		CurPos += Sprintf2UV28_Byte( sprintfAddr, uuFmt, CurPos, statOffset+0x1E, statOffset+0x1F );
		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 108 );
	}

	// AR
	if ( client5 )
		CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x1C );
	else
		CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x1A );

	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 120 );

	// hits
	CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x06, statOffset+0x08 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 72 );

	// stam
	CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x0A, statOffset+0x0C );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 96 );

	// mana
	CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x0E, statOffset+0x10 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 84 );

	// gold
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x14 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 108 );

	if ( client5 )
	{
		CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x18, statOffset+0x1A );
		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 120 );
	}
	else
	{
		// Weight:
		memcpy( (void*)CurPos, "\x33\xC9", 2 ); // xor     ecx, ecx
		CurPos += 2;
		memcpy( (void*)CurPos, "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
		CurPos += 3;
		*((int*)CurPos) = statOffset+0x00; // str
		CurPos += 4;
		memcpy( (void*)CurPos, "\x8D\x04\xCD\x00\x00\x00\x00\x2B\xC1\x99\x2B\xC2\xD1\xF8\x83\xC0\x28", 17 ); // eax = ecx*3.5 + 40
		CurPos += 17;
		memcpy( (void*)CurPos, "\x50", 1 ); // push    eax
		CurPos ++;

		memcpy( (void*)CurPos, "\x33\xC9", 2 ); // xor     ecx, ecx
		CurPos += 2;
		memcpy( (void*)CurPos, "\x8D\x54\x24\x4C", 4 ); // lea     edx, [esp+74h+var_28]
		CurPos += 4;
		memcpy( (void*)CurPos, "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
		CurPos += 3;
		*((int*)CurPos) = statOffset+0x18; // weight
		CurPos += 4;
		memcpy( (void*)CurPos, "\x51", 1 ); // push    ecx
		CurPos ++;

		// push offset fmtAddr
		*((unsigned char*)CurPos) = 0x68;
		CurPos ++;
		*((int*)CurPos) = uuFmt;
		CurPos += 4;

		memcpy( (void*)CurPos, "\x52", 1 ); // push    edx
		CurPos ++;

		// call    _sprintf
		*((unsigned char*)CurPos) = 0xE8;
		CurPos ++;
		*((int*)CurPos) = sprintfAddr - (CurPos+4);
		CurPos += 4;

		memcpy( (void*)CurPos, "\x83\xC4\x10", 3 ); // add    esp, 10h
		CurPos += 3;

		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 120 );
	}

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = -stackChange;
		CurPos++;
	}

	memset( (void*)CurPos, 0x90, End-CurPos );

	VirtualProtect( (void *)(Start), len, OldProtect, &OldProtect );

	return true;
}

#define NOTO_HUE_STR "\0\0\0\0\x59\0\0\0\x3F\0\0\0\xb2\x03\0\0"
#define NOTO_HUE_LEN 16

DWORD NotoLoc = 0;
void SetCustomNotoHue( int hue )
{
	if ( !NotoLoc )
	{
		NotoLoc = MemFinder::Find( NOTO_HUE_STR, NOTO_HUE_LEN );
		if ( !NotoLoc )
			NotoLoc = 0xFFFFFFFF;
	}

	if ( NotoLoc != 0xFFFFFFFF )
		*((int*)(NotoLoc + 8*4)) = hue;
}

#define PACKET_TBL_STR "Got Logout OK packet!\0\0\0"
#define PACKET_TS_LEN 24

//search disassembly for
//FC A5 13 2E C1 E2 1F D1 E8 D1 E9 0B C6 0B CA 35 FD A5 13 2E 81 F1 7F 52 1D A2 4D
//static key1 -- -- -- -- -- -- -- -- -- -- -- -- static key2 -- -- dynamic key --   
#define CRYPT_KEY_STR "\xC1\xE2\x1F\xD1\xE8\xD1\xE9\x0B\xC6\x0B\xCA\x35"
#define CRYPT_KEY_LEN 12

void (*RedrawUOScreen)() = NULL;
void (*RedrawGameEdge)() = NULL;

SIZE *SizePtr = NULL;



bool CopyClientMemory()
{
	int count = 0;
	DWORD addr = 0, Seek = 0;
	ClientPacketInfo packet;
	MemFinder mf;

	memcpy( pShared->PacketTable, StaticPacketTable, 256*sizeof(short) );

	mf.AddEntry( "\x80\x02\x00\x00\xE0\x01\x00\x00palette.", 16, 0x00500000 );
	mf.AddEntry( "\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16 ); // resize screen function
	mf.AddEntry( "\x57\x56\x6A\x00\x6A\x00\xE8", 7 );
	mf.AddEntry( CRYPT_KEY_STR, CRYPT_KEY_LEN );
	mf.AddEntry( CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN );
	mf.AddEntry( PACKET_TBL_STR, PACKET_TS_LEN );
	mf.AddEntry( CHEATPROC_STR, CHEATPROC_LEN );

	mf.Execute();

	SizePtr = (SIZE*)mf.GetAddress( "\x80\x02\x00\x00\xE0\x01\x00\x00palette.", 16 );
	if ( SizePtr )
	{
		addr = mf.GetAddress( "\x57\x56\x6A\x00\x6A\x00\xE8", 7 );
		if ( addr && *((unsigned char*)(addr-0x13)) == 0xE8 &&  *((unsigned char*)(addr-0x13+5)) == 0xE8 )
		{
			addr -= 0x12;
			RedrawGameEdge = (void (*)())(addr + 4 + *((int*)addr));
			addr += 5;
			RedrawUOScreen = (void (*)())(addr + 4 + *((int*)addr));
		}
	}

	addr = mf.GetAddress( "\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16 );
	if ( addr )
	{
		DWORD oldProt, origAddr = addr;

		VirtualProtect( (void*)origAddr, 0x50, PAGE_EXECUTE_READWRITE, &oldProt );
		addr += 0x11; // skip to jnz
		addr += *((BYTE*)addr)+1; // skip to target
		memset( (void*)addr, 0x90, 0x14 ); // nop
		*((BYTE*)addr) = 0xBB; *((DWORD*)(addr+1)) = (DWORD)(&DesiredSize.cx); // mov ebx, offset DesiredSize.cx
		addr += 5;
		*((BYTE*)addr) = 0xB8; *((DWORD*)(addr+1)) = (DWORD)(&DesiredSize.cy); // mov eax, offset DesiredSize.cy
		addr += 5;
		*((BYTE*)addr) = 0x8B; *((BYTE*)(addr+1)) = 0x13;  // mov edx, [ebx]
		addr += 2;
		*((BYTE*)addr) = 0x8B; *((BYTE*)(addr+1)) = 0x08;  // mov ecx, [eax]
		addr += 2;
		*((BYTE*)(addr+6)) = 0xEB; // change jnz to jmp
		VirtualProtect( (void*)origAddr, 0x50, oldProt, &oldProt );
	}

	if ( OSICryptEnabled )
	{
		addr = mf.GetAddress( CRYPT_KEY_STR, CRYPT_KEY_LEN );
		if ( !addr )
		{
			addr = mf.GetAddress( CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN );
			if ( !addr )
				return false;
			else
				LoginEncryption::SetKeys( (const BYTE*)(addr - 4), (const BYTE*)(addr + CRYPT_KEY_3D_LEN), (const BYTE*)(addr + CRYPT_KEY_3D_LEN + 19) );
		}
		else
		{
			LoginEncryption::SetKeys( (const BYTE*)(addr - 4), (const BYTE*)(addr + CRYPT_KEY_LEN), (const BYTE*)(addr + CRYPT_KEY_LEN + 6) );
		}
	}

	addr = mf.GetAddress( PACKET_TBL_STR, PACKET_TS_LEN );
	if ( !addr )
		return false;

	addr += PACKET_TS_LEN;

	// these appear at unpredictable offsets from the search string, so we have to seek for them.
	// we use the first packet (0x00 with length 0x68) to find a place in the table to start from.
	while ( Seek != 0x68 && count < 512 )
	{
		addr ++;
		count ++;

		if ( IsBadReadPtr( (void*)addr, sizeof(ClientPacketInfo) ) )
			break;
		memcpy( &Seek, (const void*)addr, 2 );
	}

	if ( Seek != 0x68 )
		return false;

	addr += 4;

	memset( pShared->PacketTable, 0xFF, 512 );
	pShared->PacketTable[0] = 0x68;
	count = 0;
	do {
		if ( IsBadReadPtr( (void*)addr, sizeof(ClientPacketInfo) ) )
			break;

		memcpy( &packet, (const void*)addr, sizeof(ClientPacketInfo) );
		addr += sizeof(ClientPacketInfo);
		if ( pShared->PacketTable[(BYTE)packet.Id] == 0xFFFF )
			pShared->PacketTable[(BYTE)packet.Id] = packet.Length;
		count ++;
	} while ( packet.Id != 0xFFFFFFFF && (BYTE)packet.Id == packet.Id && count < 256 );

	memset( pShared->CheatKey, 0, 16 );

	DWORD cheatKey = mf.GetAddress( CHEATPROC_STR, CHEATPROC_LEN );
	if ( cheatKey )
	{
		cheatKey = MemFinder::Find( "\x8A\x91", 2, cheatKey, cheatKey + 0x80 );
		if ( cheatKey )
		{
			cheatKey += 2;

			/*char msg[256];
			sprintf( msg, "p: %X", (void*)(*((DWORD*)cheatKey)) );
			MessageBox( NULL, msg, "ptr", 0 );*/

			if ( !IsBadReadPtr( (void*)(*((DWORD*)cheatKey)), 16 ) )
				memcpy( pShared->CheatKey, (void*)(*((DWORD*)cheatKey)), 16 );
		}
	}

	return true;
}

bool PatchMemory( void )
{
	Log( "Patching client functions." );

	return
		HookFunction( "wsock32.dll", "closesocket", 3, (unsigned long)HookCloseSocket, &OldCloseSocket, &CloseSocketAddress ) &&
		HookFunction( "wsock32.dll", "connect", 4, (unsigned long)HookConnect, &OldConnect, &ConnectAddress ) &&
		HookFunction( "wsock32.dll", "recv", 16, (unsigned long)HookRecv, &OldRecv, &RecvAddress ) &&
		HookFunction( "wsock32.dll", "select", 18, (unsigned long)HookSelect, &OldSelect, &SelectAddress ) &&
		HookFunction( "wsock32.dll", "send", 19, (unsigned long)HookSend, &OldSend, &SendAddress )
		;
	//HookFunction( "wsock32.dll", "socket", 23, (unsigned long)HookSocket, &OldSocket, &SocketAddress)
	//HookFunction( "wsock32.dll", "WSAAsyncSelect", 101, (unsigned long)HookAsyncSelect, &OldAsyncSelect, &AsyncSelectAddress );
}

void MemoryPatch( unsigned long Address, unsigned long value )
{
	MemoryPatch( Address, &value, 4 ); // sizeof(int)
}

void MemoryPatch( unsigned long Address, int value, int numBytes )
{
	MemoryPatch( Address, &value, numBytes ); 
}

void MemoryPatch( unsigned long Address, const void *value, int length )
{
	DWORD OldProtect;
	if ( !VirtualProtect( (void *)Address, length, PAGE_READWRITE, &OldProtect ) )
		return;

	memcpy( (void *)Address, value, length );

	VirtualProtect( (void *)Address, length, OldProtect, &OldProtect );
}

bool CheckParent( HWND hCheck, HWND hComp )
{
	hCheck = GetParent( hCheck );
	while ( hCheck != hComp && hCheck != NULL )
		hCheck = GetParent( hCheck );

	return ( hCheck == hComp );
}

vector<DWORD> addrList;
void FindList( DWORD val, unsigned short size )
{
	if ( size == 0xFFFF )
	{
		addrList.clear();
		return;
	}

	if ( size > 4 || size < 1 ) size = 4;

	MemFinder mf;

	mf.AddEntry( &val, size, 0x7FFF, 0x00400000 );

	mf.Execute();

	DWORD addr;
	if ( addrList.size() == 0 )
	{
		for(int i=0;i<0x7FFF;i++)
		{
			addr = mf.GetAddress( &val, size, i );
			if ( addr )
				addrList.push_back( addr );
			else
				break;
		}
	}
	else
	{
		vector<DWORD> newList;
		for(unsigned int i=0;i<addrList.size();i++)
		{
			if ( memcmp( (void*)addrList[i], &val, size ) == 0 )
				newList.push_back( addrList[i] );
		}

		addrList = newList;
	}

	PostMessage( hPostWnd, WM_UONETEVENT, MAKELONG(FINDDATA,0), addrList.size() );
	for(unsigned int i=0;i<addrList.size() && i < 10;i++)
		PostMessage( hPostWnd, WM_UONETEVENT, MAKELONG(FINDDATA,i+1), addrList[i] );
}


void MessageProc( HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam, MSG *pMsg )
{
	/*if ( SizePtr && ( SizePtr->cx != DesiredSize.cx || SizePtr->cy != DesiredSize.cy ) )// && ( SizePtr->cx != 640 || SizePtr->cy != 480 ) )
	{
		SizePtr->cx = DesiredSize.cx;
		SizePtr->cy = DesiredSize.cy;

		if ( RedrawGameEdge )
		{
			RedrawGameEdge();
			RedrawUOScreen();
		}
	}*/

	HWND hFore;

	switch ( nMsg )
	{
		// Custom messages
	case WM_PROCREADY:
		hPostWnd = (HWND)lParam;
		UOProcId = GetCurrentProcessId();
		hWatchWnd = hWnd;
		OSICryptEnabled = (wParam & 0x2) != 0;
		AllowNegotiate = (wParam & 0x4) != 0;

		InitThemes();

		if ( !CreateSharedMemory() )
			PostMessage( hPostWnd, WM_UONETEVENT, NOT_READY, NO_SHAREMEM );
		else if ( !PatchMemory() )
			PostMessage( hPostWnd, WM_UONETEVENT, NOT_READY, NO_PATCH );
		else if ( !CopyClientMemory() )
			PostMessage( hPostWnd, WM_UONETEVENT, READY, NO_COPY );
		else
			PostMessage( hPostWnd, WM_UONETEVENT, READY, SUCCESS );
		break;

	case WM_UONETEVENT:
		switch ( LOWORD(wParam) )
		{
		case SEND:
			FlushSendData();
			break;
		case STAT_BAR:
			PatchStatusBar( (BOOL)lParam );
			break;
		case NOTO_HUE:
			SetCustomNotoHue( (int)lParam );
			break;
		case DEATH_MSG:
			PatchDeathMsg();
			break;
		case CALIBRATE_POS:
			WaitForSingleObject( CommMutex, INFINITE );
			if ( pShared->Position[0] >= -255 && pShared->Position[0] <= 255 && pShared->Position[1] >= 0 && pShared->Position[1] <= 8192 && pShared->Position[2] >= 0 && pShared->Position[2] <= 8192 )
			{
				pShared->Position[2] = (int)MemFinder::Find( pShared->Position, sizeof(int)*3, 0x00500000, 0x00C00000 );
				if ( pShared->Position[2] )
				{
					pShared->Position[0] = 0xFFFFFFFF;
					pShared->Position[1] = 0xDEADBEEF;
				}
				else
				{
					memset( pShared->Position, 0, sizeof(int)*3 );
				}
			}
			ReleaseMutex( CommMutex );
			break;

		case OPEN_RPV:
			SendMessage( hPostWnd, WM_UONETEVENT, OPEN_RPV, lParam );
			break;

		case SETWNDSIZE:
			DesiredSize.cx = LOWORD(lParam);
			DesiredSize.cy = HIWORD(lParam);
			break;

		case FINDDATA:
			FindList( (DWORD)lParam, HIWORD(wParam) );
			break;

		case SMART_CPU:
			SmartCPU = lParam;
			break;

		case NEGOTIATE:
			AllowNegotiate = lParam;
			break;
		}
		break;

		/*case WM_SIZE:
		if ( wParam == 2 && pMsg && pMsg->hwnd == hWnd  )
		pMsg->lParam = lParam = MAKELONG( 800, 600 );
		break;
		case WM_GETMINMAXINFO:
		if ( SetMaxSize )
		{
		((MINMAXINFO *)lParam)->ptMaxSize.x = 800;
		((MINMAXINFO *)lParam)->ptMaxSize.y = 600;
		((MINMAXINFO *)lParam)->ptMaxTrackSize.x = 800;
		((MINMAXINFO *)lParam)->ptMaxTrackSize.y = 600;
		}
		break;
		*/

		// Macro stuff
	case WM_SYSKEYDOWN:
	case WM_KEYDOWN:
		if ( pMsg && !SendMessage( hPostWnd, WM_UONETEVENT, KEYDOWN, wParam ) )
		{
			// dont give the key to the client
			pMsg->message = WM_NULL;
			pMsg->lParam = 0;
			pMsg->wParam = 0;
		}
		break;

	case WM_SYSKEYUP:
	case WM_KEYUP:
		if ( pMsg && wParam == VK_SNAPSHOT ) // VK_SNAPSHOT (Print Screen) Doesn't seem to send a KeyDown message
			SendMessage( hPostWnd, WM_UONETEVENT, KEYDOWN, wParam );
		break;

	case WM_MOUSEWHEEL:
		PostMessage( hPostWnd, WM_UONETEVENT, MOUSE, MAKELONG( 0, (((short)HIWORD(wParam)) < 0 ? -1 : 1) ) );
		break;
	case WM_MBUTTONDOWN:
		PostMessage( hPostWnd, WM_UONETEVENT, MOUSE, MAKELONG( 1, 0 ) );
		break;
	case WM_XBUTTONDOWN:
		PostMessage( hPostWnd, WM_UONETEVENT, MOUSE, MAKELONG( HIWORD(wParam) + 1, 0 ) );
		break;

		//Activation tracking : 
	case WM_ACTIVATE:
		Active = wParam;
		PostMessage( hPostWnd, WM_UONETEVENT, ACTIVATE, wParam );
		break;
	case WM_KILLFOCUS:
		hFore = GetForegroundWindow();
		if ( ((HWND)wParam) != hPostWnd && hFore != hPostWnd && !CheckParent( hFore, hPostWnd ) )
			PostMessage( hPostWnd, WM_UONETEVENT, FOCUS, FALSE );
		break;
	case WM_SETFOCUS:
		PostMessage( hPostWnd, WM_UONETEVENT, FOCUS, TRUE );
		break;

		//Custom title bar:
	case WM_NCACTIVATE:
		Active = wParam; 
		//fallthrough
	case WM_GETICON:
	case WM_NCPAINT:
	case WM_SETTEXT:
	case WM_CUSTOMTITLE:
		RedrawTitleBar( hWnd, Active );
		break;
	}
}

LRESULT CALLBACK GetMsgHookFunc( int Code, WPARAM Flag, LPARAM pMsg )
{
	if ( Code >= 0 && Flag != PM_NOREMOVE ) //dont process messages until they are removed from the queue
	{
		MSG *Msg = (MSG*)pMsg;

		if ( Msg->hwnd == hWatchWnd || ( hWatchWnd == NULL && Msg->message == WM_PROCREADY ) )
			MessageProc( Msg->hwnd, Msg->message, Msg->wParam, Msg->lParam, Msg );
	}

	return CallNextHookEx( NULL, Code, Flag, pMsg );
}

LRESULT CALLBACK WndProcRetHookFunc( int Code, WPARAM Flag, LPARAM pMsg )
{
	if ( Code >= 0 )
	{
		CWPRETSTRUCT *Msg = (CWPRETSTRUCT *)(pMsg);

		if ( Msg->hwnd == hWatchWnd || ( hWatchWnd == NULL && Msg->message == WM_PROCREADY ) )
			MessageProc( Msg->hwnd, Msg->message, Msg->wParam, Msg->lParam, NULL );
	}

	return CallNextHookEx( NULL, Code, Flag, pMsg );
}

LRESULT CALLBACK UOAWndProc( HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam )
{
	if ( nMsg >= WM_USER+200 && nMsg < WM_USER+315 )
		return SendMessage( hPostWnd, nMsg, wParam, lParam );
	else
		return DefWindowProc( hWnd, nMsg, wParam, lParam );
}

void Log( const char *format, ... )
{
#ifdef _DEBUG
#ifdef LOGGING
	FILE *log = fopen( "C:\\Crypt.log", "a" );
	if ( log )
	{
		char timeStr[256];
		struct tm *newtime;
		time_t aclock;

		time( &aclock ); 
		newtime = localtime( &aclock );
		strncpy( timeStr, asctime( newtime ), 256 );
		int len = (int)strlen( timeStr );
		if ( timeStr[len-1] == '\n' )
			timeStr[len-1] = 0;

		char OutTxt[512];
		va_list argList;
		va_start( argList, format );
		_vsnprintf( OutTxt, 512, format, argList );
		va_end( argList );

		fprintf( log, "%s: %s\n", timeStr, OutTxt );
		fclose( log );
	}
#endif
#endif
}
