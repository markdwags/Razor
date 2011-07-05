#include "stdafx.h"
#include "Crypt.h"
#include "uo_huffman.h"
#include "PacketInfo.h"
#include "OSIEncryption.h"
#include "LoginEncryption.h"
#include "MemFinder.h"
#include "Checksum.h"
#include "Obfuscation.h"

//#define NO_CHECKSUM_VERSION

//*************************************************************************************
//**************************************Varaibles**************************************
//*************************************************************************************
HHOOK hWndProcRetHook = NULL;
HHOOK hGetMsgHook = NULL;
HWND hWatchWnd = NULL;
HWND hPostWnd = NULL;
HWND hMapWnd = NULL;
DWORD UOProcId = 0;

HANDLE hFileMap = NULL;
HMODULE hInstance = NULL;
SOCKET CurrentConnection = 0;
int ConnectedIP = 0;

HANDLE CommMutex = NULL;

char *tempBuff = NULL;

SharedMemory *pShared = NULL;

LARGE_INTEGER PerfFreq, Counter;

DWORD DeathMsgAddr = 0xFFFFFFFF;
HWND hUOAWnd = NULL;

SIZE DesiredSize = {800,600};

unsigned long OldRecv, OldSend, OldConnect, OldCloseSocket, OldSelect, OldCreateFileA;
unsigned long RecvAddress, SendAddress, ConnectAddress, CloseSocketAddress, SelectAddress, CreateFileAAddress;

bool Seeded = false;
bool FirstRecv = true;
bool FirstSend = true;
bool LoginServer = false;
bool Active = true;
bool Disabled = true;
bool SmartCPU = false;
bool ServerNegotiated = false;
bool InGame = false;
bool CopyFailed = true;
bool Forwarding = false;
bool Forwarded = false;
bool UltimaDLLHaxed = false;
bool ClientEncrypted = false;
bool ServerEncrypted = false;
bool DwmAttrState = true;

enum CLIENT_TYPE { TWOD = 1, THREED = 2 };
CLIENT_TYPE ClientType = TWOD;

BYTE CryptChecksum[16] = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,};

//**************************************OSI Only Stuff*********************************
DWORD CryptSeed = 0x7f000001;
OSIEncryption *ClientCrypt = NULL;
OSIEncryption *ServerCrypt = NULL;
LoginEncryption *ClientLogin = NULL;
LoginEncryption *ServerLogin = NULL;
//*************************************************************************************

//*************************************************************************************
//**************************************Functions**************************************
//*************************************************************************************
LRESULT CALLBACK WndProcRetHookFunc( int, WPARAM, LPARAM );
LRESULT CALLBACK GetMsgHookFunc( int, WPARAM, LPARAM );

bool HookFunction( const char *, const char *, int, unsigned long, unsigned long *, unsigned long * );
void FlushSendData();

bool CreateSharedMemory();
void CloseSharedMemory();

//Hooks:
int PASCAL HookRecv( SOCKET, char *, int, int );
int PASCAL HookSend( SOCKET, char *, int, int );
int PASCAL HookConnect( SOCKET, const sockaddr *, int );
int PASCAL HookCloseSocket( SOCKET );
int PASCAL HookSelect( int, fd_set*, fd_set*, fd_set*, const struct timeval * );
//HANDLE WINAPI CreateFileAHook( LPCTSTR, DWORD, DWORD, LPSECURITY_ATTRIBUTES, DWORD, DWORD, HANDLE );

typedef int (PASCAL *NetIOFunc)(SOCKET, char *, int, int);
typedef int (PASCAL *ConnFunc)(SOCKET, const sockaddr *, int);
typedef int (PASCAL *CLSFunc)(SOCKET);
typedef int (PASCAL *SelectFunc)( int, fd_set*, fd_set*, fd_set*, const struct timeval* );
typedef HANDLE (WINAPI *CreateFileAFunc)(LPCTSTR,DWORD,DWORD,LPSECURITY_ATTRIBUTES,DWORD,DWORD,HANDLE);
typedef char *(__cdecl *GetUOVersionFunc)();

GetUOVersionFunc NativeGetUOVersion = NULL;


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
	int len;
	BYTE *data = NULL;
	FILE *file = NULL;
	char fileName[256], origFilename[256];
	char *namePtr = NULL;

	char *obStr = GetObStr(OB_KERNEL32);

	Disabled = (int)tolower(*obStr) != (int)'k';
	
	if ( !strcmp( exeVer, DLL_VERSION ) )
	{
		GetModuleFileName(NULL, fileName, 256);
		file = fopen( fileName, "rb" );
		if ( file )
		{
			fseek( file, 0, SEEK_END );
			len = ftell( file );
			fseek( file, 0, SEEK_SET );

			data = new BYTE[len];
			fread( data, len, 1, file );
			fclose( file );

			OSIEncryption::MD5( data, len, data );

			for(int i=0;i<16;i++)
				data[i] ^= data[0x1717+i];

			Disabled |= memcmp( data, RAZOR_CHECKSUM, 16 ) != 0;

			delete[] data;
		}

		memcpy(origFilename, fileName, 256);

		//MessageBox( NULL, "Debug me!", "Now", MB_OK );

        namePtr = strrchr( fileName, '\\' );
		if ( namePtr )
			namePtr ++; // AFTER the slash
		else
			namePtr = fileName;
		*namePtr++ = 'U'; *namePtr = 0;
		*namePtr++ = 'l'; *namePtr = 0;
		*namePtr++ = 't'; *namePtr = 0;
		*namePtr++ = 'i'; *namePtr = 0;
		*namePtr++ = 'm'; *namePtr = 0;
		*namePtr++ = 'a'; *namePtr = 0;
		*namePtr++ = '.'; *namePtr = 0;
		*namePtr++ = 'd'; *namePtr = 0;
		*namePtr++ = 'l'; *namePtr = 0;
		*namePtr++ = 'l'; *namePtr = 0;
		*namePtr++ = 000; *namePtr = 0;
		
		file = fopen( fileName, "rb" );
		if ( file )
		{
			fseek( file, 0, SEEK_END );
			len = ftell( file );
			fseek( file, 0, SEEK_SET );

			data = new BYTE[len];
			fread( data, len, 1, file );
			fclose( file );

			OSIEncryption::MD5( data, len, data );

			for(int i=0;i<16;i++)
				data[i] ^= data[0x1717+i];

			UltimaDLLHaxed = memcmp( data, ULTIMA_CHECKSUM, 16 ) != 0;

			delete[] data;
		}
	}
	else
	{
		Disabled |= true;
	}
DLLFUNCTION bool AllowBit( unsigned int bit );
	OSIEncryption::MD5( ((const BYTE*)AllowBit)+9, 0x31-9, CryptChecksum );
	
	HMODULE hKern = LoadLibrary(obStr);
	Disabled |= !hKern;
	
	GetObStr(OB_GETPROCADDR);
	void *(__stdcall *getprocaddr)(HANDLE, const char *);
	getprocaddr = (void *(__stdcall*)(HANDLE, const char *))GetProcAddress(hKern, obStr);
	
	GetObStr(OB_GETMODFN);
	DWORD (__stdcall *getmodfn)(HANDLE, char *, DWORD);
	getmodfn = (DWORD (__stdcall*)(HANDLE, char *, DWORD))getprocaddr(hKern, obStr);
	
	getmodfn(NULL, obStr, 256);
	
	Disabled |= memcmp(obStr, origFilename, strlen(obStr)) != 0;
	Disabled |= memcmp(origFilename, obStr, strlen(origFilename)) != 0;
	
#ifdef NO_CHECKSUM_VERSION
	Disabled = false;
#endif

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
	//memset( pShared, 0, sizeof(SharedMemory) );

	pShared->IsHaxed = UltimaDLLHaxed;

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

	ServerEncrypted = (flags&0x10) != 0;
	ClientEncrypted = (flags&0x08) != 0;

	PostMessage( hWatchWnd, WM_PROCREADY, (WPARAM)flags, (LPARAM)hPostWnd );
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
		Sleep( 10 );
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

	if ( hWatchWnd && IsWindow( hWatchWnd ) )
		PostMessage( hWatchWnd, WM_QUIT, 0, 0 );
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

DLLFUNCTION void DoFeatures( int realFeatures )
{
	int i, c, size = 8;
	char pkt[512];
	char *str = NULL;

	int features = 0;
	if ( (realFeatures & 0x8000) == 0 )
	{
		if ( (realFeatures & 1) != 0 )
			features = 2;
		if ( (realFeatures & 2) != 0 )
			features |= 8;
		features &= 0xFFFF;
	}
	else
	{
		features = realFeatures & 0x7FFF;
	}

	pkt[0] = 0x03;

	pkt[1] = pkt[2] = 0; // size = 0 (filled in later)

	pkt[3] = 0x20; // MessageType.Special

	pkt[4] = 0x02; // hue = 0x2b2
	pkt[5] = 0xB2;

	pkt[6] = 0x00; // font = 3
	pkt[7] = 0x03;

	str = &pkt[8];
	
	// CHEAT UO.exe 1 251--
	sprintf( str, "%c%cE%c%c %s %d %d--", 'C', 'H', 'A', 'T', "UO.exe", ClientType, features );
	c = (int)strlen( str ) + 1;

	memcpy( &str[c], CryptChecksum, 16 );
	c += 16;

	memcpy( &str[c], DLL_VERSION, strlen( DLL_VERSION ) );
	c += (int)strlen( DLL_VERSION );
	str[c++] = 0;

	for (i = 0; i < c; i++)
		str[i] = str[i] ^ pShared->CheatKey[i & 0xF];
	
	size = 8+c;
	
	/*if ( !ServerEncrypted )
	{
		time_t now = time(NULL);

		*((unsigned int*)&str[size]) = ~((unsigned int)now ^ 0x54494D45);
		size += 4;

		*((unsigned int*)&str[size]) = ~(ConnectedIP ^ (int)now);
		size += 4;
	}*/

	// fill in size
	pkt[1] = (BYTE)((size>>8)&0xFF);
	pkt[2] = (BYTE)(size&0xFF);

	WaitForSingleObject( CommMutex, 50 );
	memcpy( pShared->OutSend.Buff + pShared->OutSend.Start + pShared->OutSend.Length, pkt, size );
	pShared->OutSend.Length += (int)size;
	ReleaseMutex( CommMutex );
	PostMessage( FindUOWindow(), WM_UONETEVENT, SEND, 0 );
}

DLLFUNCTION bool AllowBit( unsigned int bit )
{
	bit &= 0x0000003F; // limited to 64 bits
	return !pShared || ( pShared->AuthBits[7-(bit/8)] & (1<<(bit%8)) ) == 0;
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

DLLFUNCTION void TranslateSetup( void (__stdcall *SetupFunc)() )
{
	if ( SetupFunc )
		SetupFunc();
	else
		MessageBox( hPostWnd, "There is no translation DLL installed.", "No DLL", MB_OK|MB_ICONERROR );
}

DLLFUNCTION void TranslateLogin( void (__stdcall *LoginFunc)( char *, char * ), char *name, char *shard )
{
	if ( LoginFunc && name && shard )
		LoginFunc( name, shard );
}

DLLFUNCTION void TranslateDo( void (__stdcall *TransFunc)( char *, char *, DWORD * ), char *in, char *out, DWORD *outLen )
{
	if ( TransFunc && in && out && outLen )
		TransFunc( in, out, outLen );
}

DLLFUNCTION BOOL HandleNegotiate( __int64 features )
{
	if ( pShared && pShared->AuthBits && pShared->AllowNegotiate )
	{	
		memcpy( pShared->AuthBits, &features, 8 );

		ServerNegotiated = true;

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

SIZE *SizePtr = NULL;
void __stdcall OnSetUOWindowSize( int width )
{
	if ( width != 800 && width != 600 ) // in case it actually the height for some reason
	{
		SizePtr->cx = 640;
		SizePtr->cy = 480;
	}
	else
	{
		*SizePtr = DesiredSize;
	}
}

DLLFUNCTION void __stdcall OnAttach( void *params, int paramsLen )
{
	int count = 0;
	DWORD addr = 0, oldProt;
	ClientPacketInfo packet;
	MemFinder mf;

	UOProcId = GetCurrentProcessId();

	if ( !CreateSharedMemory() )
		return;

	pShared->AllowDisconn = true;

	CopyFailed = false;
	
	mf.AddEntry( "UoClientApp", 12, 0x00500000 );
	mf.AddEntry( "report\0", 8, 0x00500000 );
	mf.AddEntry( "Another copy of ", 16, 0x00500000 );
	mf.AddEntry( "\x00\x68\x88\x13\x00\x00\x56\xE8", 8 );
	mf.AddEntry( "\x68\x88\x13\x00\x00", 5, 16, 0x00400000 ); // (end of a push offset), push 5000, push esi
	mf.AddEntry( "Electronic Arts Inc.", 20 );
	mf.AddEntry( "intro.bik", 10 );
	mf.AddEntry( "osilogo.bik", 12 );
	mf.AddEntry( "\x80\x02\x00\x00\xE0\x01\x00\x00", 8, 0x00500000 ); // current screen size
	mf.AddEntry( "\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16 ); // resize screen function
	mf.AddEntry( "\x57\x56\x6A\x00\x6A\x00\xE8", 7 ); // redraw screen/edge function
	mf.AddEntry( PACKET_TBL_STR, PACKET_TS_LEN, 10, 0x00500000 );
	mf.AddEntry( CRYPT_KEY_STR, CRYPT_KEY_LEN );
	mf.AddEntry( CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN );
	mf.AddEntry( CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN );
	mf.AddEntry( CRYPT_KEY_STR_MORE_NEW, CRYPT_KEY_MORE_NEW_LEN );
	mf.AddEntry( CHEATPROC_STR, CHEATPROC_LEN );
	mf.AddEntry( "CHEAT %s", 8, 0x00500000 );
	mf.AddEntry( "UO Version %s", 12 );

	memcpy( pShared->PacketTable, StaticPacketTable, 256*sizeof(short) );

	const BYTE defaultCheatKey[] = { 0x98, 0x5B, 0x51, 0x7E, 0x11, 0x0C, 0x3D, 0x77, 0x2D, 0x28, 0x41, 0x22, 0x74, 0xAD, 0x5B, 0x39 };
	memcpy( pShared->CheatKey, defaultCheatKey, 16 );

	mf.Execute();

	SizePtr = (SIZE*)mf.GetAddress( "\x80\x02\x00\x00\xE0\x01\x00\x00", 8 );
	if ( SizePtr )
	{
		addr = mf.GetAddress( "\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16 );
		if ( addr )
		{
			int i;
			DWORD origAddr = addr;

			VirtualProtect( (void*)origAddr, 128, PAGE_EXECUTE_READWRITE, &oldProt );
			for (i = 16; i < 128; i++)
			{
				if ( *((BYTE*)(addr+i)) == 0xE9 ) // find the first jmp
				{
					memset( (void*)addr, 0x90, i ); // nop
					
					// mov eax, dword [esp+4]
					*((BYTE*)(addr+0)) = 0x8B; // mov
					*((BYTE*)(addr+1)) = 0x44; //  eax
					*((BYTE*)(addr+2)) = 0x24; //  [esp
					*((BYTE*)(addr+3)) = 0x04; //      +4]
					addr += 4;
					
					*((BYTE*)addr) = 0x50; // push eax
					addr++;
					// call OnSetUOWindowSize
					*((BYTE*)addr) = 0xE8;
					*((DWORD*)(addr+1)) = ((DWORD)OnSetUOWindowSize) - (addr + 5);
					addr += 5;
					break;
				}
			}
			VirtualProtect( (void*)origAddr, 128, oldProt, &oldProt );
		}
	}
	
	memset( pShared->PacketTable, 0xFF, 512 );

	int i = 0;
	while (( addr = mf.GetAddress( PACKET_TBL_STR, PACKET_TS_LEN, i++ )) != 0)
	{
		addr += PACKET_TBL_OFFSET;
		if ( IsBadReadPtr( (void*)addr, sizeof(ClientPacketInfo)*256 ) )
			continue;
		
		pShared->PacketTable[0] = ((ClientPacketInfo*)addr)->Length;
		addr += sizeof(ClientPacketInfo);

		int total = 1;
		for (count = 1; count < 256 && total < 256; count++, total++)
		{
			memcpy( &packet, (const void*)addr, sizeof(ClientPacketInfo) );
			addr += sizeof(ClientPacketInfo);
			if ( packet.Id < count || packet.Id >= 256 )
				break;

			if ( pShared->PacketTable[(BYTE)packet.Id] == 0xFFFF )
				pShared->PacketTable[(BYTE)packet.Id] = packet.Length;

			count = packet.Id;
		}

		if ( packet.Id != 0xFFFFFFFF && ( packet.Id >> 16 ) != 0x67 && ( packet.Id >> 16 ) != 0x68 ) /*packet.Id != 0x006761B4 && packet.Id != 0x00678314 && packet.Id != 0x00679314 && packet.Id != 0x0067B8BC && packet.Id != 0x0067CCBC && packet.Id != 0x00680E64 */
			continue;
		else
			break;
	}

	if (!addr)
		CopyFailed = true;
	
	addr = mf.GetAddress( CRYPT_KEY_STR, CRYPT_KEY_LEN );
	if ( !addr )
	{
		addr = mf.GetAddress( CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN );

		if ( !addr )
		{
			addr = mf.GetAddress( CRYPT_KEY_STR_MORE_NEW, CRYPT_KEY_MORE_NEW_LEN );
			if ( !addr )
			{
				addr = mf.GetAddress( CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN );
				if ( addr )
				{
					LoginEncryption::SetKeys( (const DWORD*)(addr + CRYPT_KEY_3D_LEN), (const DWORD*)(addr + CRYPT_KEY_3D_LEN + 19) );
					ClientType = THREED;
				}
				else
				{
					CopyFailed = true;
				}
			}
			else
			{
				addr += CRYPT_KEY_MORE_NEW_LEN;

				const DWORD *pKey1 = *((DWORD**)addr);
				const DWORD *pKey2 = pKey1 + 1;
				if ( IsBadReadPtr( pKey2, 4 ) || IsBadReadPtr( pKey1, 4 ) )
					CopyFailed = true;
				else
					LoginEncryption::SetKeys( pKey1, pKey2 );
			}
		}
		else
		{
			addr += CRYPT_KEY_NEW_LEN;

			const DWORD *pKey1 = *((DWORD**)addr);
			const DWORD *pKey2 = pKey1 - 1;
			if ( IsBadReadPtr( pKey2, 4 ) || IsBadReadPtr( pKey1, 4 ) )
				CopyFailed = true;
			else
				LoginEncryption::SetKeys( pKey1, pKey2 );
		}
	}
	else
	{
		LoginEncryption::SetKeys( (const DWORD*)(addr + CRYPT_KEY_LEN), (const DWORD*)(addr + CRYPT_KEY_LEN + 6) );
	}

	/*addr = mf.GetAddress( CHEATPROC_STR, CHEATPROC_LEN );
	if ( addr )
	{
		addr = MemFinder::Find( "\x8A\x91", 2, addr, addr + 0x80 );
		if ( addr )
		{
			addr += 2;

			if ( !IsBadReadPtr( (void*)(*((DWORD*)addr)), 16 ) )
				memcpy( pShared->CheatKey, (void*)(*((DWORD*)addr)), 16 );
		}
	}
	else
	{
		addr = mf.GetAddress( "CHEAT %s", 8 );
		if ( addr )
		{
			addr -= 16;
			if ( !IsBadReadPtr( (void*)(*((DWORD*)addr)), 16 ) )
				memcpy( pShared->CheatKey, (void*)(*((DWORD*)addr)), 16 );
			ClientType = THREED;
		}
	}*/
	BYTE cheatKey[16] = { 0x98, 0x5B, 0x51, 0x7E, 0x11, 0x0C, 0x3D, 0x77, 0x2D, 0x28, 0x41, 0x22, 0x74, 0xAD, 0x5B, 0x39 };
	memcpy( pShared->CheatKey, cheatKey, 16 );
	
	// Multi UO
	addr = mf.GetAddress( "UoClientApp", 12 );
	if ( addr )
	{
		VirtualProtect( (void*)addr, 12, PAGE_READWRITE, &oldProt );
		_snprintf( (char*)addr, 12, "UoApp%d", UOProcId );
		VirtualProtect( (void*)addr, 12, oldProt, &oldProt );
	}

	addr = mf.GetAddress( "Another copy of ", 16 );
	if ( addr )
	{
		char buff[5];
		
		buff[0] = 0x68; // push
		*((DWORD*)(&buff[1])) = addr;

		addr = 0x00400000;
		do {
			addr = MemFinder::Find( buff, 5, addr, 0x00600000 );
			if ( addr )
			{
				if ( (*((unsigned char*)(addr - 5))) == 0x74 ) // jz?
					MemoryPatch( addr-5, 0xEB, 1 ); // change to jmp
				addr += 5; // skip ahead to find the next instance
			}
		} while ( addr > 0 && addr < 0x00600000 );
	}

	addr = mf.GetAddress( "report\0", 8 );
	if ( addr )
	{
		VirtualProtect( (void*)addr, 12, PAGE_READWRITE, &oldProt );
		_snprintf( (char*)addr, 8, "r%d", UOProcId );
		VirtualProtect( (void*)addr, 12, oldProt, &oldProt );
	}

	// Splash screen crap:
	/*addr = mf.GetAddress( "\x00\x68\x88\x13\x00\x00\x56\xE8", 8 );
	if ( addr )
		MemoryPatch( addr+2, 0x00000005 ); // change 5000ms to 5ms*/
	for (int i = 0; i < 16; i++)
	{
		addr = mf.GetAddress( "\x68\x88\x13\x00\x00", 5, i );
		if ( !addr )
			break;
		for (int e = 5; e < 24; e++)
		{
			if ( *((BYTE*)(addr+e)) == 0x8B && *((BYTE*)(addr+e+1)) == 0x3D )
			{
				MemoryPatch( addr+1, 0x00000001 ); // change 5000ms to 1ms
				i = 99;
				break;
			}
		}
	}
	addr = mf.GetAddress( "intro.bik", 10 );
	if ( addr )
		MemoryPatch( addr, "intro.SUX", 10 );
	addr = mf.GetAddress( "ostlogo.bik", 12 );
	if ( addr )
		MemoryPatch( addr, "osilogo.SUX", 12 );

	addr = mf.GetAddress( "Electronic Arts Inc.", 20 );
	if ( addr )
	{
		addr -= 7;
		VirtualProtect( (void*)addr, 52, PAGE_EXECUTE_READWRITE, &oldProt );
		strncpy( (char*)addr, "[Powered by Razor - The cutting edge UO Assistant]", 51 );
		VirtualProtect( (void*)addr, 52, oldProt, &oldProt );
	}

	NativeGetUOVersion = NULL;
	if ( ClientType == TWOD )
	{
		addr = mf.GetAddress( "UO Version %s", 12 );
		if ( addr )
		{
			char temp[8];
			temp[0] = 0x68;
			*((DWORD*)&temp[1]) = addr;

			addr = MemFinder::Find( temp, 5 );

			if ( addr )
			{
				count = 0;
				while ( *((BYTE*)addr) != 0xE8 && count < 128 )
				{
					addr--;
					count++;
				}

				if ( *((BYTE*)addr) == 0xE8  )
					NativeGetUOVersion = (GetUOVersionFunc)((addr+5) + *((DWORD*)(addr+1)));
			}
		}
	}
	
	//HookFunction( "kernel32.dll", "CreateFileA", 0, (unsigned long)CreateFileAHook, &OldCreateFileA, &CreateFileAAddress );
}

DLLFUNCTION void SetServer( unsigned int addr, unsigned short port )
{
	if (pShared)
	{
		pShared->ServerIP = addr;
		pShared->ServerPort = port;
	}
}

DLLFUNCTION const char *GetUOVersion()
{
	if ( pShared )
		return pShared->UOVersion;
	else
		return "";
}

bool CreateSharedMemory()
{
	char name[256];

	CommMutex = NULL;
	hFileMap = NULL;
	pShared = NULL;

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

	//memset( pShared, 0, sizeof(SharedMemory) );

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
	delete ClientLogin;
	delete ServerCrypt;
	delete ServerLogin;

	ClientCrypt = NULL;
	ClientLogin = NULL;
	ServerCrypt = NULL;
	ServerLogin = NULL;
}

void CreateEncryption()
{
	delete ClientCrypt;
	delete ClientLogin;
	delete ServerCrypt;
	delete ServerLogin;

	if ( ClientEncrypted )
	{
		ClientCrypt = new OSIEncryption();
		ClientLogin = new LoginEncryption();
	}

	if ( ServerEncrypted )
	{
		ServerCrypt = new OSIEncryption();
		ServerLogin = new LoginEncryption();
	}
}

inline void Maintenance( Buffer &buff )
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
			if ( ServerEncrypted )
				ServerCrypt->DecryptFromServer( (BYTE*)buff, (BYTE*)buff, ackLen );

			int blen = Compression::Decompress( (char*)&pShared->InRecv.Buff[pShared->InRecv.Start+pShared->InRecv.Length], buff, ackLen );
			pShared->InRecv.Length += blen;

			if ( !ServerNegotiated && !InGame && pShared && pShared->AllowNegotiate )
			{
				int pos = pShared->InRecv.Start;
				unsigned char *p_buff = &pShared->InRecv.Buff[pos];

				while ( pos < pShared->InRecv.Length )
				{
					int left = pShared->InRecv.Length - pos;
					int p_len = GetPacketLength( p_buff, left );

					if ( *p_buff == 0xA9 && p_len >= 1+2+1+30+30 && p_len <= left )
					{
						// character list

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

						Forwarding = Forwarded = false;

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

				if ( ((BYTE)buff[0]) == 0x8C )
					LoginServer = false;

				if ( Forwarding )
					Seeded = Forwarded = true;

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

				ackLen += Compression::Compress( &buff[ackLen], (char*)&pShared->OutRecv.Buff[pShared->OutRecv.Start], blen );
				
				pShared->OutRecv.Start += blen;
				pShared->OutRecv.Length -= blen;
			}

			if ( ClientEncrypted && ackLen > 0 )
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

int SkipSendData = 0;
int PASCAL HookSend( SOCKET sock, char *buff, int len, int flags )
{
	int ackLen;

	if ( sock == CurrentConnection && CurrentConnection )
	{
		if ( !Seeded )
		{
			if ( len > 0 && ((BYTE)*buff) == ((BYTE)0xEF) )
				SkipSendData = 16;

			if ( len >= 4 )
			{
				Seeded = true;

				CryptSeed = *((DWORD*)buff);

				if ( ServerEncrypted )
				{
					ServerCrypt->Initialize( CryptSeed );
					ServerLogin->Initialize( (BYTE*)&CryptSeed );
				}

				if ( ClientEncrypted )
				{
					ClientCrypt->Initialize( CryptSeed );
					ClientLogin->Initialize( (BYTE*)&CryptSeed );
				}

				Compression::Reset();
			}

			ackLen = (*(NetIOFunc)OldSend)(sock,buff,len,flags);
			pShared->TotalSend += len;
		}
		else if ( SkipSendData < len )
		{
			SkipSendData = 0;

			if ( FirstSend )
			{
				FirstSend = false;

				if ( ClientEncrypted )
					LoginServer = ClientLogin->TestForLogin( (BYTE)buff[0] );
				else
					LoginServer = LoginEncryption::IsLoginByte( (BYTE)buff[0] );

				if ( LoginServer )
					Forwarding = Forwarded = false;
			}

			WaitForSingleObject( CommMutex, INFINITE );

			//memcpy( &pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length], buff, len );

			if ( ClientEncrypted )
			{
				if ( Forwarded )
				{
					CryptSeed = LoginEncryption::GenerateBadSeed( CryptSeed );

					ClientCrypt->Initialize( CryptSeed );

					ClientCrypt->DecryptFromClient( (BYTE*)buff, (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), len );
					ClientLogin->Decrypt( (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), len );

					LoginServer = Forwarding = Forwarded = false;
				}
				else
				{
					if ( LoginServer )
					{
						ClientLogin->Decrypt( (BYTE*)(buff), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), len );

						if ( ((BYTE)pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]) == 0xA0 )
							Forwarding = true;
					}
					else
					{
						ClientCrypt->DecryptFromClient( (BYTE*)(buff), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length]), len );
					}
				}
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
		else
		{
			ackLen = (*(NetIOFunc)OldSend)(sock,buff,len,flags);
			pShared->TotalSend += len;
			SkipSendData -= len;
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
					if ( pShared->AllowNegotiate && ServerNegotiated )
					{
						// the first 2 bytes are 0
						// the next 4 bytes are "flags" which say the user's client type (lbr,t2a,aos,etc)
						// the rest are ignored, so we can use them for auth
						memcpy( buff + 1 + 4 + 30 + 2 + 4, pShared->AuthBits, 8 );
						memcpy( buff + 1 + 4 + 30 + 2 + 4 + 8, RAZOR_ID_KEY, RAZOR_ID_KEY_LEN );
					}

					InGame = true;
#ifdef NO_CHECKSUM_VERSION
					memcpy( buff + 1 + 4 + 30 + 28, "\xDE\xAD", 2 );
#else
					if ( pShared->IsHaxed )
						memcpy( buff + 1 + 4 + 30 + 28, "\xDE\xAD", 2 );
#endif
					break;
				}
				else if ( *buff == 0x00 && (*((DWORD*)&buff[1])) == 0xEDEDEDED && len >= 1+4+4+1+30+30 && len <= left )
				{
					// char creation
					if ( pShared->AllowNegotiate && ServerNegotiated )
					{
						memcpy( buff + 1 + 4 + 4 + 1 + 30 + 15, pShared->AuthBits, 8 );
						memcpy( buff + 1 + 4 + 4 + 1 + 30 + 15 + 8, RAZOR_ID_KEY, min( 7, RAZOR_ID_KEY_LEN ) );
					}

					InGame = true;
#ifdef NO_CHECKSUM_VERSION
					memcpy( buff + 1 + 4 + 30 + 28, "\xDE\xAD", 2 );
#else
					if ( pShared->IsHaxed )
						memcpy( buff + 1 + 4 + 30 + 28, "\xDE\xAD", 2 );
#endif
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
		
		if ( ServerEncrypted )
		{
			if ( tempBuff == NULL )
				tempBuff = new char[SHARED_BUFF_SIZE];

			if ( LoginServer )
				ServerLogin->Encrypt( (BYTE*)&pShared->OutSend.Buff[pShared->OutSend.Start], (BYTE*)tempBuff, outLen );
			else
				ServerCrypt->EncryptForServer( (BYTE*)&pShared->OutSend.Buff[pShared->OutSend.Start], (BYTE*)tempBuff, outLen );
			
			ackLen = (*(NetIOFunc)OldSend)(CurrentConnection,tempBuff,outLen,0);
		}
		else
		{
			ackLen = (*(NetIOFunc)OldSend)(CurrentConnection,(char*)&pShared->OutSend.Buff[pShared->OutSend.Start],outLen,0);
		}

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
	int retVal;

	if ( addr && addrlen >= sizeof(sockaddr_in) )
	{
		const sockaddr_in *old_addr = (const sockaddr_in *)addr;
		sockaddr_in useAddr;

		memcpy( &useAddr, old_addr, sizeof(sockaddr_in) );

		if ( !Forwarded && pShared->ServerIP != 0 )
		{
			useAddr.sin_addr.S_un.S_addr = pShared->ServerIP;
			useAddr.sin_port = htons( pShared->ServerPort );
		}
		
		/*char blah[256];
		sprintf(blah, "%08X - %08X", useAddr.sin_addr.S_un.S_addr, pShared->ServerIP);
		MessageBox(NULL, blah, "Connect To:", MB_OK);*/

		retVal = (*(ConnFunc)OldConnect)( sock, (sockaddr*)&useAddr, sizeof(sockaddr_in) );

		ConnectedIP = useAddr.sin_addr.S_un.S_addr;

		if ( retVal != SOCKET_ERROR )
		{
			Log( "Connecting to %i", sock );

			CreateEncryption();

			Seeded = false;
			LoginServer = false;
			FirstRecv = true;
			FirstSend = true;
			Forwarding = Forwarded = false;

			WaitForSingleObject( CommMutex, INFINITE );
			CurrentConnection = sock;
			pShared->OutRecv.Length = pShared->InRecv.Length = pShared->OutSend.Length = pShared->InSend.Length = 0;
			pShared->ForceDisconn = false;
			ReleaseMutex( CommMutex );

			PostMessage( hPostWnd, WM_UONETEVENT, CONNECT, useAddr.sin_addr.S_un.S_addr );
		}
	}
	else
	{
		retVal = (*(ConnFunc)OldConnect)( sock, addr, addrlen );
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
		if( _stricmp( Dll, (char *)(baseAddr + iid->Name) ) == 0 )
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
		if( _stricmp( Dll, (char *)(baseAddr + iid->Name) ) == 0 )
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

		ClientEncrypted = (wParam & 0x08) != 0;
		ServerEncrypted = (wParam & 0x10) != 0;
		
		InitThemes();

		if ( !pShared ) // If this failed the first time or was not run at all, try it once more before panicing
			OnAttach( NULL, 0 );

		if ( !pShared )
			PostMessage( hPostWnd, WM_UONETEVENT, NOT_READY, NO_SHAREMEM );
		else if ( CopyFailed )
			PostMessage( hPostWnd, WM_UONETEVENT, NOT_READY, NO_COPY );
		else if ( !PatchMemory() )
			PostMessage( hPostWnd, WM_UONETEVENT, NOT_READY, NO_PATCH );
		else
			PostMessage( hPostWnd, WM_UONETEVENT, READY, SUCCESS );

		if ( pShared )
		{
			pShared->AllowNegotiate = (wParam & 0x04) != 0;

			pShared->UOVersion[0] = 0;

			if ( NativeGetUOVersion != NULL )
				strncpy( pShared->UOVersion, NativeGetUOVersion(), 16 );
		}

		break;

// ZIPPY REV 80
/*	case WM_SETFWDWND:
		PostMessage( hPostWnd, WM_UONETEVENT, SET_FWD_HWND, lParam );
		break;
*/
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
			if ( pShared )
				pShared->AllowNegotiate = (lParam != 0);
			break;

		case SET_MAP_HWND:
			hMapWnd = (HWND)lParam;
			break;

// ZIPPY REV 80
/*		case SET_FWD_HWND:
			PostMessage( hPostWnd, WM_UONETEVENT, SET_FWD_HWND, lParam );
			break;
*/		}
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
		if ( ((HWND)wParam) != hPostWnd && hFore != hPostWnd && ((HWND)wParam) != hMapWnd && hFore != hMapWnd 
			&& !CheckParent( hFore, hPostWnd ) )
		{
			PostMessage( hPostWnd, WM_UONETEVENT, FOCUS, FALSE );
		}
		break;
	case WM_SETFOCUS:
		PostMessage( hPostWnd, WM_UONETEVENT, FOCUS, TRUE );
		break;

		//Custom title bar:
	case WM_NCACTIVATE:
		Active = wParam;
		//fallthrough
	case WM_NCPAINT:
	case WM_GETICON:
	case WM_SETTEXT:
	case WM_CUSTOMTITLE:
		CheckTitlebarAttr(hWnd);
		RedrawTitleBar( hWnd, Active );
		break;
	}
}

LRESULT CALLBACK GetMsgHookFunc( int Code, WPARAM Flag, LPARAM pMsg )
{
	if ( Code >= 0 && Flag != PM_NOREMOVE ) //dont process messages until they are removed from the queue
	{
		MSG *Msg = (MSG*)pMsg;
		/*
		Msg->message ^= 0x11;
		Msg->message ^= Disabled * 101;
		Msg->message *= !(Disabled * 020);
		Msg->message ^= 0x11;
		*/
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
		/*
		Msg->message ^= 0x11;
		Msg->message ^= Disabled * 101;
		Msg->message *= !(Disabled * 020);
		Msg->message ^= 0x11;
		*/
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

