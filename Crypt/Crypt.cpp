#include "stdafx.h"
#include "Crypt.h"
#include "uo_huffman.h"
#include "PacketInfo.h"
#include "OSIEncryption.h"
#include "LoginEncryption.h"
#include "MemFinder.h"

//*************************************************************************************
//**************************************Variables**************************************
//*************************************************************************************
HHOOK hWndProcRetHook = NULL;
HHOOK hGetMsgHook = NULL;
HWND hUOWindow = NULL;
HWND hRazorWnd = NULL;
HWND hMapWnd = NULL;
DWORD UOProcId = 0;

HANDLE hFileMap = NULL;
HMODULE hInstance = NULL;
SOCKET CurrentConnection = 0;

HANDLE CommMutex = NULL;

char *tempBuff = NULL;

SharedMemory *pShared = NULL;

LARGE_INTEGER PerfFreq, Counter;

HWND hUOAWnd = NULL;

SIZE DesiredSize = {800,600};

unsigned long OldRecv, OldSend, OldConnect, OldCloseSocket, OldSelect, OldCreateFileA;
unsigned long RecvAddress, SendAddress, ConnectAddress, CloseSocketAddress, SelectAddress, CreateFileAAddress;

bool Seeded = false;
bool FirstRecv = true;
bool FirstSend = true;
bool LoginServer = false;
bool Active = true;
bool SmartCPU = false;
bool ServerNegotiated = false;
bool InGame = false;
bool CopyFailed = true;
bool Forwarding = false;
bool Forwarded = false;
bool ClientEncrypted = false;
bool ServerEncrypted = false;

enum CLIENT_TYPE { TWOD = 1, THREED = 2 };
CLIENT_TYPE ClientType = TWOD;

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
int HookRecv( SOCKET, char *, int, int );
int HookSend( SOCKET, char *, int, int );
int HookConnect( SOCKET, const sockaddr *, int );
int HookCloseSocket( SOCKET );
int HookSelect( int, fd_set*, fd_set*, fd_set*, const struct timeval * );

typedef int (*NetIOFunc)(SOCKET, char *, int, int);
typedef int (*ConnFunc)(SOCKET, const sockaddr *, int);
typedef int (*CLSFunc)(SOCKET);
typedef int (*SelectFunc)( int, fd_set*, fd_set*, fd_set*, const struct timeval* );
typedef HANDLE (*CreateFileAFunc)(LPCTSTR,DWORD,DWORD,LPSECURITY_ATTRIBUTES,DWORD,DWORD,HANDLE);
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
		if ( IsWindow( hRazorWnd ) )
			GetWindowThreadProcessId( hRazorWnd, &postID );

		if ( thisID == postID || thisID == UOProcId )
		{
			if ( IsWindow( hRazorWnd ) )
				PostMessage( hRazorWnd, WM_UONETEVENT, CLOSE, 0 );

			if ( IsWindow( hUOWindow ) )
			{
				PostMessage( hUOWindow, WM_QUIT, 0, 0 );
				SetForegroundWindow( hUOWindow );
				SetFocus( hUOWindow );
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

DLLFUNCTION void *GetSharedAddress()
{
	Log( "Get shared address [0x%x]", pShared );
	return pShared;
}

DLLFUNCTION HWND FindUOWindow( void )
{
	if ( hUOWindow == NULL || !IsWindow(hUOWindow) )
	{
		HWND hWnd = FindWindow( "Ultima Online", NULL );
		if (hWnd == NULL)
			hWnd = FindWindow( "Ultima Online Third Dawn", NULL );
		return hWnd;
	}
	else
	{
		return hUOWindow;
	}
}

DLLFUNCTION void SetDataPath( const char *path )
{
	WaitForSingleObject( CommMutex, INFINITE );
	strncpy( pShared->DataPath, path, MAX_PATH);
	ReleaseMutex( CommMutex );
}

DLLFUNCTION int InstallLibrary( HWND PostWindow, DWORD pid, int flags )
{
	DWORD UOTId = 0;

	Log( "Initialize library..." );

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
				if (UOProcId == pid)
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

	hUOWindow = hWnd;
	hRazorWnd = PostWindow;

	if ( hUOWindow == NULL )
		return NO_UOWND;

	if ( !UOTId || !UOProcId )
		return NO_TID;

	if ( !CreateSharedMemory() )
		return NO_SHAREMEM;

	pShared->Reserved0 = false;

	hWndProcRetHook = SetWindowsHookEx( WH_CALLWNDPROCRET, WndProcRetHookFunc, hInstance, UOTId );
	if ( !hWndProcRetHook )
		return NO_HOOK;

	hGetMsgHook = SetWindowsHookEx( WH_GETMESSAGE, GetMsgHookFunc, hInstance, UOTId );
	if ( !hGetMsgHook )
		return NO_HOOK;

	WNDCLASS wc;
	wc.style = 0;
	wc.lpfnWndProc = (WNDPROC)UOAWndProc;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon(NULL, IDI_WINLOGO);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground = NULL;
	wc.lpszMenuName = NULL;
	wc.lpszClassName = "UOASSIST-TP-MSG-WND";
	RegisterClass(&wc);
	DWORD error = GetLastError();

	hUOAWnd = CreateWindow("UOASSIST-TP-MSG-WND", "UOASSIST-TP-MSG-WND", WS_OVERLAPPEDWINDOW, 0, 0, 50, 50, NULL, NULL, hInstance, 0);
	if (hUOAWnd)
		ShowWindow(hUOAWnd, FALSE);

	ServerEncrypted = (flags&0x10) != 0;
	ClientEncrypted = (flags&0x08) != 0;

	PostMessage( hUOWindow, WM_PROCREADY, (WPARAM)flags, (LPARAM)hRazorWnd );
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
				if (UOProcId == pid)
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

	if (hUOAWnd && IsWindow(hUOAWnd))
	{
		UnregisterClass("UOASSIST-TP-MSG-WND", hInstance);
		SendMessage(hUOAWnd, WM_CLOSE, 0, 0);
		hUOAWnd = NULL;
	}

	if ( hUOWindow && IsWindow( hUOWindow ) )
		PostMessage( hUOWindow, WM_QUIT, 0, 0 );
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

DLLFUNCTION void CalibratePosition( uint16_t x, uint16_t y, uint16_t z )
{
	Position pos;
	COPYDATASTRUCT copydata;

	pos.x = x;
	pos.y = y;
	pos.z = z;

	copydata.dwData = (ULONG_PTR)UONET_MESSAGE_COPYDATA::POSITION;
	copydata.cbData = sizeof(pos);
	copydata.lpData = &pos;

	SendMessage(hUOWindow, WM_COPYDATA, (WPARAM)hRazorWnd, (LPARAM)&copydata);
}

/* These variables are used in the UO client process address space */

/* 7.0.15.1 Client, Modified for Outlands
   Located at 0x00A72BD8 */
#pragma pack(1)
struct PositionOutlands {
	uint16_t x;
	uint16_t y;
	uint16_t z;
};
static_assert(sizeof(struct PositionOutlands) == 6, "Incorrect size\n");

/* 5.0.8.3 Client, UO Renaissance
   Located at address 0x007D7C58 */
#pragma pack(1)
struct PositionUOR {
	uint32_t z;
	uint32_t y;
	uint32_t x;
};
static_assert(sizeof(struct PositionUOR) == 12, "Incorrect size\n");
// UOR client this is at 0x007D7C60 (x is)

// Fall back search. Not always reliable.
#pragma pack(1)
struct PositionGeneric {
	uint32_t z;
	uint32_t y;
	uint32_t x;
};
static_assert(sizeof(struct PositionGeneric) == 12, "Incorrect size\n");

Position g_TestPosition = {};
Position g_LastPosition = {};
void *g_ClientMem = nullptr;

static void CheckPosition()
{
	if (g_ClientMem == nullptr) {
		if (strncmp(pShared->UOVersion, "5.0.8.3", sizeof(pShared->UOVersion)) == 0) {
			/* On UOR, we know exactly where the position is. */
			g_ClientMem = (void *)(0x007D7C60);
		}
		else if (strncmp(pShared->UOVersion, "7.0.15.1", sizeof(pShared->UOVersion)) == 0) {
			/* Similarly on Outlands, we know where the position is. */
			g_ClientMem = (void *)(0x00A72BD8);
		} else {
			/* Scan the region of memory in the client known to hold the player's position */
			for (uintptr_t addr = 0x00500000; addr < 0x00C00000; addr += 2)
			{
				if (IsBadReadPtr((void*)addr, sizeof(PositionGeneric))) {
					break;
				}

				PositionGeneric* mem = (PositionGeneric*)addr;

				if (mem->x == g_TestPosition.x && mem->y == g_TestPosition.y && mem->z == g_TestPosition.z) {
					g_ClientMem = mem;
					break;
				}
			}
		}
	}

	if (g_ClientMem != nullptr) {
		Position pos;
		if (strncmp(pShared->UOVersion, "5.0.8.3", sizeof(pShared->UOVersion)) == 0) {
			PositionUOR *mem = (PositionUOR*)g_ClientMem;
			pos.x = mem->x;
			pos.y = mem->y;
			pos.z = mem->z;
		} else if (strncmp(pShared->UOVersion, "7.0.15.1", sizeof(pShared->UOVersion)) == 0) {
			/* Similarly on Outlands, we know where the position is. */
			PositionOutlands *mem = (PositionOutlands*)g_ClientMem;
			pos.x = mem->x;
			pos.y = mem->y;
			pos.z = mem->z;
		} else {
			PositionGeneric *mem = (PositionGeneric*)g_ClientMem;
			pos.x = mem->x;
			pos.y = mem->y;
			pos.z = mem->z;
		}

		if (pos.x != g_LastPosition.x || pos.y != g_LastPosition.y || pos.z != g_LastPosition.z) {
			/* Inform Razor of a position change */

			COPYDATASTRUCT copydata;

			copydata.dwData = (ULONG_PTR)UONET_MESSAGE_COPYDATA::POSITION;
			copydata.cbData = sizeof(pos);
			copydata.lpData = &pos;

			SendMessage(hRazorWnd, WM_COPYDATA, (WPARAM)hUOWindow, (LPARAM)&copydata);
		}

		g_LastPosition = pos;
	}
}

VOID CALLBACK OnTick(HWND hwnd, UINT Message, UINT TimerId, DWORD dwTime) {
	/* Scan client memory for position updates */
	CheckPosition();

	/* Post a message to Razor to indicate a game loop tick.*/
	PostMessage(hRazorWnd, WM_UONETEVENT, ON_TICK, 0);
}

SIZE *SizePtr = NULL;
void OnSetUOWindowSize( int width )
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

DLLFUNCTION void OnAttach( void *params, int paramsLen )
{
	int count = 0;
	DWORD addr = 0, oldProt;
	MemFinder mf;

	UOProcId = GetCurrentProcessId();

	if ( !CreateSharedMemory() )
		return;

		CopyFailed = false;
		mf.AddEntry("UoClientApp", 12, 0x00500000);
		mf.AddEntry("report\0", 8, 0x00500000);
		mf.AddEntry("Another copy of ", 16, 0x00500000);
		mf.AddEntry("\x00\x68\x88\x13\x00\x00\x56\xE8", 8);
		mf.AddEntry("\x68\x88\x13\x00\x00", 5, 16, 0x00400000); // (end of a push offset), push 5000, push esi
		mf.AddEntry("Electronic Arts Inc.", 20);
		mf.AddEntry("intro.bik", 10);
		mf.AddEntry("osilogo.bik", 12);
		mf.AddEntry("\x80\x02\x00\x00\xE0\x01\x00\x00", 8, 0x00500000); // current screen size
		mf.AddEntry("\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16); // resize screen function
		mf.AddEntry("\x57\x56\x6A\x00\x6A\x00\xE8", 7); // redraw screen/edge function
		mf.AddEntry(PACKET_TBL_STR, PACKET_TS_LEN, 10, 0x00500000);
		mf.AddEntry(CRYPT_KEY_STR, CRYPT_KEY_LEN);
		mf.AddEntry(CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN);
		mf.AddEntry(CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN);
		mf.AddEntry(CRYPT_KEY_STR_MORE_NEW, CRYPT_KEY_MORE_NEW_LEN);
		mf.AddEntry("UO Version %s", 12);
		mf.AddEntry("Multiple Instances Running", 26, 0x00500000);

		memcpy( pShared->PacketTable, StaticPacketTable, 256*sizeof(short) );

		mf.Execute();

		SizePtr = (SIZE*)mf.GetAddress("\x80\x02\x00\x00\xE0\x01\x00\x00", 8);
		if (SizePtr)
		{
			addr = mf.GetAddress("\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16);
			if (addr)
			{
				int i;
				DWORD origAddr = addr;

				VirtualProtect((void*)origAddr, 128, PAGE_EXECUTE_READWRITE, &oldProt);
				for (i = 16; i < 128; i++)
				{
					if (*((BYTE*)(addr + i)) == 0xE9) // find the first jmp
					{
						memset((void*)addr, 0x90, i); // nop

						// mov eax, dword [esp+4]
						*((BYTE*)(addr + 0)) = 0x8B; // mov
						*((BYTE*)(addr + 1)) = 0x44; //  eax
						*((BYTE*)(addr + 2)) = 0x24; //  [esp
						*((BYTE*)(addr + 3)) = 0x04; //      +4]
						addr += 4;

						*((BYTE*)addr) = 0x50; // push eax
						addr++;
						// call OnSetUOWindowSize
						*((BYTE*)addr) = 0xE8;
						*((DWORD*)(addr + 1)) = ((DWORD)OnSetUOWindowSize) - (addr + 5);
						addr += 5;
						break;
					}
				}
				VirtualProtect((void*)origAddr, 128, oldProt, &oldProt);
			}
		}

		int i = 0;
		while ((addr = mf.GetAddress(PACKET_TBL_STR, PACKET_TS_LEN, i++)) != 0)
		{
			memset(pShared->PacketTable, 0xFF, 512);

			addr += PACKET_TBL_OFFSET;
			if (IsBadReadPtr((void*)addr, sizeof(ClientPacketInfo) * 128))
				continue;
			ClientPacketInfo *tbl = (ClientPacketInfo*)addr;

			if (tbl[0].Id == 1 || tbl[0].Id == 2 || tbl[0].Id >= 256)
				continue;

			// this one isnt in order because OSI are idiots (0xF8)
			pShared->PacketTable[tbl[0].Id] = tbl[0].Length;

			int idx = 1;
			bool got1 = false, got2 = false;
			for (int prev = 0; prev < 255 && idx < 256; idx++)
			{
				if (IsBadReadPtr((void*)(tbl + idx), sizeof(ClientPacketInfo)) ||
					tbl[idx].Id <= prev || tbl[idx].Id >= 256)
				{
					break;
				}

				got1 |= tbl[idx].Id == 1 && tbl[idx].Length == StaticPacketTable[1];
				got2 |= tbl[idx].Id == 2 && tbl[idx].Length == StaticPacketTable[2];

				prev = tbl[idx].Id;
				if (pShared->PacketTable[prev] == 0xFFFF)
					pShared->PacketTable[prev] = tbl[idx].Length;
			}

			if (idx < 128 || !got1 || !got2)
				continue;
			else
				break;
		}

		if (!addr)
			CopyFailed = true;

		addr = mf.GetAddress(CRYPT_KEY_STR, CRYPT_KEY_LEN);
		if (!addr)
		{
			addr = mf.GetAddress(CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN);

			if (!addr)
			{
				addr = mf.GetAddress(CRYPT_KEY_STR_MORE_NEW, CRYPT_KEY_MORE_NEW_LEN);
				if (!addr)
				{
					addr = mf.GetAddress(CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN);
					if (addr)
					{
						LoginEncryption::SetKeys((const DWORD*)(addr + CRYPT_KEY_3D_LEN), (const DWORD*)(addr + CRYPT_KEY_3D_LEN + 19));
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
					if (IsBadReadPtr(pKey2, 4) || IsBadReadPtr(pKey1, 4))
						CopyFailed = true;
					else
						LoginEncryption::SetKeys(pKey1, pKey2);
				}
			}
			else
			{
				addr += CRYPT_KEY_NEW_LEN;

				const DWORD *pKey1 = *((DWORD**)addr);
				const DWORD *pKey2 = pKey1 - 1;
				if (IsBadReadPtr(pKey2, 4) || IsBadReadPtr(pKey1, 4))
					CopyFailed = true;
				else
					LoginEncryption::SetKeys(pKey1, pKey2);
			}
		}
		else
		{
			LoginEncryption::SetKeys((const DWORD*)(addr + CRYPT_KEY_LEN), (const DWORD*)(addr + CRYPT_KEY_LEN + 6));
		}

		// Multi UO
		addr = mf.GetAddress("UoClientApp", 12);
		if (addr)
		{
			VirtualProtect((void*)addr, 12, PAGE_READWRITE, &oldProt);
			_snprintf((char*)addr, 12, "UoApp%d", UOProcId);
			VirtualProtect((void*)addr, 12, oldProt, &oldProt);
		}

		addr = mf.GetAddress("Another copy of ", 16);
		if (addr)
		{
			char buff[5];

			buff[0] = 0x68; // push
			*((DWORD*)(&buff[1])) = addr;

			addr = 0x00400000;
			do {
				addr = MemFinder::Find(buff, 5, addr, 0x00600000);
				if (addr)
				{
					if ((*((unsigned char*)(addr - 5))) == 0x74) // jz?
						MemoryPatch(addr - 5, 0xEB, 1); // change to jmp
					addr += 5; // skip ahead to find the next instance
				}
			} while (addr > 0 && addr < 0x00600000);
		}

		addr = mf.GetAddress("Multiple Instances Running", 26);
		if (addr)
		{
			char buff[5];

			buff[0] = 0x68; // push
			*((DWORD*)(&buff[1])) = addr;

			addr = 0x00400000;
			do {
				addr = MemFinder::Find(buff, 5, addr, 0x00600000);
				if (addr)
				{
					char in = (*((unsigned char*)(addr - 4)));
					if (in == 0x74 || in == 0x75) { // jz or jnz
						MemoryPatch(addr - 4, 0xEB, 1); // change to jmp
					}
					addr += 5; // skip ahead to find the next instance
				}
			} while (addr > 0 && addr < 0x00600000);
		}

		addr = mf.GetAddress("report\0", 8);
		if (addr)
		{
			VirtualProtect((void*)addr, 12, PAGE_READWRITE, &oldProt);
			_snprintf((char*)addr, 8, "r%d", UOProcId);
			VirtualProtect((void*)addr, 12, oldProt, &oldProt);
		}

		// Splash screen crap:
		for (int i = 0; i < 16; i++)
		{
			addr = mf.GetAddress("\x68\x88\x13\x00\x00", 5, i);
			if (!addr)
				break;
			for (int e = 5; e < 24; e++)
			{
				if (*((BYTE*)(addr + e)) == 0x8B && *((BYTE*)(addr + e + 1)) == 0x3D)
				{
					MemoryPatch(addr + 1, 0x00000001); // change 5000ms to 1ms
					i = 99;
					break;
				}
			}
		}
		addr = mf.GetAddress("intro.bik", 10);
		if (addr)
			MemoryPatch(addr, "intro.SUX", 10);
		addr = mf.GetAddress("ostlogo.bik", 12);
		if (addr)
			MemoryPatch(addr, "osilogo.SUX", 12);

		addr = mf.GetAddress("Electronic Arts Inc.", 20);
		if (addr)
		{
			addr -= 7;
			VirtualProtect((void*)addr, 52, PAGE_EXECUTE_READWRITE, &oldProt);
			strncpy((char*)addr, "[Powered by Razor - The cutting edge UO Assistant]\0", 52);
			VirtualProtect((void*)addr, 52, oldProt, &oldProt);
		}

		NativeGetUOVersion = NULL;
		if (ClientType == TWOD)
		{
			addr = mf.GetAddress("UO Version %s", 12);
			if (addr)
			{
				char temp[8];
				temp[0] = 0x68;
				*((DWORD*)&temp[1]) = addr;

				addr = MemFinder::Find(temp, 5);

				if (addr)
				{
					count = 0;
					while (*((BYTE*)addr) != 0xE8 && count < 128)
					{
						addr--;
						count++;
					}

					if (*((BYTE*)addr) == 0xE8)
						NativeGetUOVersion = (GetUOVersionFunc)((addr + 5) + *((DWORD*)(addr + 1)));
				}
			}
		}
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
	if (pShared)
		return true;

	char name[256];

	CommMutex = NULL;
	hFileMap = NULL;
	pShared = NULL;

	Log( "Creating shared mem, proc: %x", UOProcId );

	sprintf( name, "UONetSharedCOMM_%x", UOProcId );

	CommMutex = CreateMutex( NULL, FALSE, name );
	if ( !CommMutex )
		return false;

	for (int i = 0; i < 30; i++) {
		sprintf(name, "UONetSharedFM_%x%d", UOProcId, i);
		hFileMap = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SharedMemory), name);
		if (!hFileMap || hFileMap == INVALID_HANDLE_VALUE) {
			auto err = GetLastError();
			continue;
		}

		pShared = (SharedMemory*)MapViewOfFile(hFileMap, FILE_MAP_ALL_ACCESS, 0, 0, sizeof(SharedMemory));
		if (!pShared) {
			auto err = GetLastError();
			continue;
		}

		break;
	}

	if (!pShared) {
		return false;
	}

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

		SendMessage( hRazorWnd, WM_UONETEVENT, RECV, 0 );
	}

	return ackLen;
}

int HookRecv( SOCKET sock, char *buff, int len, int flags )
{
	int ackLen;

	if ( sock == CurrentConnection && CurrentConnection )
	{
		WaitForSingleObject( CommMutex, INFINITE );
		if ( pShared->ForceDisconn && pShared->OutRecv.Length <= 0 )
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
int HookSend( SOCKET sock, char *buff, int len, int flags )
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

			if (ClientEncrypted)
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
						ClientCrypt->DecryptFromClient((BYTE*)(buff), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]), len);
					}
				}
			}
			else
				memcpy( &pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length], buff, len );

			pShared->InSend.Length += len;
			ReleaseMutex( CommMutex );

			SendMessage( hRazorWnd, WM_UONETEVENT, SEND, 0 );

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

int HookConnect( SOCKET sock, const sockaddr *addr, int addrlen )
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

		retVal = (*(ConnFunc)OldConnect)( sock, (sockaddr*)&useAddr, sizeof(sockaddr_in) );

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

			PostMessage( hRazorWnd, WM_UONETEVENT, CONNECT, useAddr.sin_addr.S_un.S_addr );
		}
	}
	else
	{
		retVal = (*(ConnFunc)OldConnect)( sock, addr, addrlen );
	}

	return retVal;
}

int HookCloseSocket( SOCKET sock )
{
	int retVal = (*(CLSFunc)OldCloseSocket)( sock );

	if ( sock == CurrentConnection && sock != 0 )
	{
		Log( "Closing socket %i", sock );
		CurrentConnection = 0;

		WaitForSingleObject( CommMutex, INFINITE );
		pShared->OutRecv.Length = pShared->InRecv.Length = pShared->OutSend.Length = pShared->InSend.Length = 0;
		pShared->TotalSend = pShared->TotalRecv = 0;
		pShared->ForceDisconn = false;
		ReleaseMutex( CommMutex );

		ServerNegotiated = false;
		InGame = false;

		memset( pShared->AuthBits, 0, 8 );

		PostMessage( hRazorWnd, WM_UONETEVENT, DISCONNECT, 0 );
	}

	return retVal;
}

int HookSelect( int ndfs, fd_set *readfd, fd_set *writefd, fd_set *exceptfd, const struct timeval *timeout )
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
		if ( pShared->OutRecv.Length > 0 || ( pShared->ForceDisconn ) )
		{
			FD_SET( CurrentConnection, readfd );
			myRet++;
		}
		ReleaseMutex( CommMutex );
	}

	if ( checkErr && !FD_ISSET( CurrentConnection, exceptfd ) )
	{
		WaitForSingleObject( CommMutex, INFINITE );
		if ( pShared->ForceDisconn && pShared->OutRecv.Length <= 0 )
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

void MessageProc( HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam, MSG *pMsg )
{
	HWND hFore;

	switch ( nMsg )
	{
		// Custom messages
	case WM_PROCREADY:
		hRazorWnd = (HWND)lParam;
		UOProcId = GetCurrentProcessId();
		hUOWindow = hWnd;

		if (!pShared) // If this failed the first time or was not run at all, try it once more before panicing
			OnAttach(NULL, 0);

		ClientEncrypted = (wParam & 0x08) != 0;
		ServerEncrypted = (wParam & 0x10) != 0;

		InitThemes();

		if ( !pShared )
			PostMessage( hRazorWnd, WM_UONETEVENT, NOT_READY, NO_SHAREMEM );
		else if ( CopyFailed )
			PostMessage( hRazorWnd, WM_UONETEVENT, NOT_READY, NO_COPY );
		else if ( !PatchMemory() )
			PostMessage( hRazorWnd, WM_UONETEVENT, NOT_READY, NO_PATCH );
		else
			PostMessage( hRazorWnd, WM_UONETEVENT, READY, SUCCESS );

		if ( pShared )
		{
			pShared->AllowNegotiate = (wParam & 0x04) != 0;

			pShared->UOVersion[0] = 0;

			if ( NativeGetUOVersion != NULL )
				strncpy( pShared->UOVersion, NativeGetUOVersion(), 16 );
		}

		/* Start a timer that will call a callback each tick. We use this to implement
		 * timers as well as scan client memory for position updates. */
		SetTimer(hUOWindow, (UINT_PTR)0xAA, 25, OnTick);

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

		case SETWNDSIZE:
			DesiredSize.cx = LOWORD(lParam);
			DesiredSize.cy = HIWORD(lParam);
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
		}
		break;

	case WM_COPYDATA: {
		COPYDATASTRUCT *copydata = (COPYDATASTRUCT *)lParam;

		switch ((UONET_MESSAGE_COPYDATA)copydata->dwData) {
		case UONET_MESSAGE_COPYDATA::POSITION:
			g_TestPosition = *(Position *)copydata->lpData;
			break;

		}
		break;
	}

		// Macro stuff
	case WM_SYSKEYDOWN:
	case WM_KEYDOWN:
		if ( pMsg && !SendMessage( hRazorWnd, WM_UONETEVENT, KEYDOWN, wParam ) )
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
			SendMessage( hRazorWnd, WM_UONETEVENT, KEYDOWN, wParam );
		break;

	case WM_MOUSEWHEEL:
		PostMessage( hRazorWnd, WM_UONETEVENT, MOUSE, MAKELONG( 0, (((short)HIWORD(wParam)) < 0 ? -1 : 1) ) );
		break;
	case WM_MBUTTONDOWN:
		PostMessage( hRazorWnd, WM_UONETEVENT, MOUSE, MAKELONG( 1, 0 ) );
		break;
	case WM_XBUTTONDOWN:
		PostMessage( hRazorWnd, WM_UONETEVENT, MOUSE, MAKELONG( HIWORD(wParam) + 1, 0 ) );
		break;

		//Activation tracking :
	case WM_ACTIVATE:
		Active = wParam;
		PostMessage( hRazorWnd, WM_UONETEVENT, ACTIVATE, wParam );
		break;
	case WM_KILLFOCUS:
		hFore = GetForegroundWindow();
		if ( ((HWND)wParam) != hRazorWnd && hFore != hRazorWnd && ((HWND)wParam) != hMapWnd && hFore != hMapWnd
			&& !CheckParent( hFore, hRazorWnd ) )
		{
			PostMessage( hRazorWnd, WM_UONETEVENT, FOCUS, FALSE );
		}
		break;
	case WM_SETFOCUS:
		PostMessage( hRazorWnd, WM_UONETEVENT, FOCUS, TRUE );
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
		if ( Msg->hwnd == hUOWindow || ( hUOWindow == NULL && Msg->message == WM_PROCREADY ) )
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
		if ( Msg->hwnd == hUOWindow || ( hUOWindow == NULL && Msg->message == WM_PROCREADY ) )
			MessageProc( Msg->hwnd, Msg->message, Msg->wParam, Msg->lParam, NULL );
	}

	return CallNextHookEx( NULL, Code, Flag, pMsg );
}

LRESULT CALLBACK UOAWndProc(HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam)
{
	if (nMsg >= WM_USER + 200 && nMsg < WM_USER + 315)
	   return SendMessage(hRazorWnd, nMsg, wParam, lParam);
	else
	   return DefWindowProc(hWnd, nMsg, wParam, lParam);
}

void Log( const char *format, ... )
{
#ifdef _DEBUG
#ifdef LOGGING
	FILE *log = fopen( "Crypt.log", "a" );
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

