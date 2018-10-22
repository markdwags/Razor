#pragma once
#pragma pack(1)

#include <stdint.h>

#define DLL_VERSION "1.4.1"

#define DLLFUNCTION __declspec(dllexport)
#define DLLVAR DLLFUNCTION

#ifdef _DEBUG
//#define LOGGING
#endif

enum IError
{
	SUCCESS,
	NO_UOWND,
	NO_TID,
	NO_HOOK,
	NO_SHAREMEM,
	LIB_DISABLED,
	NO_PATCH,
	NO_COPY,
	INVALID_PARAMS,

	UNKNOWN,
};

enum UONET_MESSAGE
{
	SEND = 1,
	RECV = 2,
	READY = 3,
	NOT_READY = 4,
	CONNECT = 5,
	DISCONNECT = 6,
	KEYDOWN = 7,
	MOUSE = 8,

	ACTIVATE = 9,
	FOCUS = 10,

	CLOSE = 11,
	STAT_BAR = 12,
	NOTO_HUE = 13,
	DLL_ERROR = 14,

	SETWNDSIZE = 19,

	FINDDATA = 20,

	SMART_CPU = 21,
	NEGOTIATE = 22,
	SET_MAP_HWND = 23
};

enum class UONET_MESSAGE_COPYDATA
{
	POSITION = 1,
};

//#define SHARED_BUFF_SIZE 0x80000 // Client's buffers are 500k
#define SHARED_BUFF_SIZE 524288 // 262144 // 250k
struct Buffer
{
	int Length;
	int Start;
	BYTE Buff[SHARED_BUFF_SIZE];
};

#pragma pack(1)
struct Position {
	uint16_t x;
	uint16_t y;
	uint16_t z;
};
static_assert(sizeof(struct Position) == 6, "Incorrect size\n");

struct SharedMemory
{
	// Do *not* mess with this struct.  Really.  I mean it.
	Buffer InRecv;
	Buffer OutRecv;
	Buffer InSend;
	Buffer OutSend;

	char TitleBar[1024];
	bool ForceDisconn;
	unsigned int TotalSend;
	unsigned int TotalRecv;
	unsigned short PacketTable[256];
	char DataPath[256];
	bool AllowNegotiate;
	unsigned char AuthBits[16];
	bool Reserved0;
	unsigned int ServerIP;
	unsigned short ServerPort;
	char UOVersion[16];
};

class PatchInfo
{
public:
	PatchInfo( DWORD addr, int len )
	{
		Address = addr;
		Length = len;
		Data = new char[Length];
		memcpy( Data, (const void*)Address, Length );
	}

	~PatchInfo()
	{
		delete[] Data;
	}

	DWORD Address;
	int Length;
	char *Data;
};

#define WM_PROCREADY WM_USER
#define WM_UONETEVENT WM_USER+1
#define WM_CUSTOMTITLE WM_USER+2

#ifndef WM_XBUTTONDOWN
#define WM_XBUTTONDOWN                  0x020B
#endif

extern HWND hUOWindow;
extern HINSTANCE hInstance;
extern SharedMemory *pShared;
extern HANDLE CommMutex;

DLLFUNCTION int InstallLibrary( HWND PostWindow, DWORD pId );
DLLFUNCTION void Shutdown( bool closeClient );
DLLFUNCTION HWND FindUOWindow();
DLLFUNCTION void *GetSharedAddress();
DLLFUNCTION int GetPacketLength( unsigned char *data, int len );
DLLFUNCTION bool IsDynLength( unsigned char packet );
DLLFUNCTION int GetUOProcId();
DLLFUNCTION HANDLE GetCommMutex();
DLLFUNCTION unsigned int TotalIn();
DLLFUNCTION unsigned int TotalOut();
DLLFUNCTION HBITMAP CaptureScreen(BOOL full, const char *msg);
DLLFUNCTION void CalibratePosition(uint16_t x, uint16_t y, uint16_t z);
DLLFUNCTION void BringToFront(HWND hWnd);
DLLFUNCTION bool AllowBit(unsigned long bit);
DLLFUNCTION BOOL HandleNegotiate(__int64 features);
DLLFUNCTION void OnAttach(void *params, int paramsLen);
DLLFUNCTION void SetServer(unsigned int addr, unsigned short port);
DLLFUNCTION const char *GetUOVersion();


LRESULT CALLBACK UOAWndProc( HWND, UINT, WPARAM, LPARAM );
void Log( const char *format, ... );
void MemoryPatch( unsigned long, unsigned long );
void MemoryPatch( unsigned long, int, int );
void MemoryPatch( unsigned long, const void *, int );
void RedrawTitleBar( HWND, bool );
void CheckTitlebarAttr(HWND);
void FreeArt();
void InitThemes();
bool PatchStatusBar( BOOL preAOS );

//#define PACKET_TBL_STR "Got Logout OK packet!\0\0\0"
//#define PACKET_TS_LEN 24
#define PACKET_TBL_STR "\x07\0\0\0\x03\0\0\0"
#define PACKET_TS_LEN 8
#define PACKET_TBL_OFFSET (0-(8+12+12))

//search disassembly for
//static key1 C1 E2 1F D1 E8 D1 E9 0B C6 0B CA 35 static key2 81 F1 dynamic key 4D
#define CRYPT_KEY_STR "\xC1\xE2\x1F\xD1\xE8\xD1\xE9\x0B\xC6\x0B\xCA\x35"
#define CRYPT_KEY_LEN 12

//static key1 D1 E8 0B C6 C1 E2 1F 35 static key2 D1 E9 89 83 F0 00 42 00 8B 45 08 0B CA 81 F1 dynamic key 48
#define CRYPT_KEY_STR_3D "\xD1\xE8\x0B\xC6\xC1\xE2\x1F\x35"
#define CRYPT_KEY_3D_LEN 8

/* To calculate login keys:
key1 = ( Major << 23 ) | ( Minor << 14 ) | ( Revision << 4 );
key1 ^= ( Revision * Revision ) << 9;
key1 ^= ( Minor * Minor );
key1 ^= ( Minor * 11 ) << 24;
key1 ^= ( Revision * 7 ) << 19;
key1 ^= 0x2C13A5FD;
key2 = ( Major << 22 ) | ( Revision << 13 ) | ( Minor << 3 );
key2 ^= ( Revision * Revision * 3 ) << 10;
key2 ^= ( Minor * Minor );
key2 ^= ( Minor * 13 ) << 23;
key2 ^= ( Revision * 7 ) << 18;
key2 ^= 0xA31D527F;
*/
/*
.text:0041AA2F C1 E6 1F                          shl     esi, 31
.text:0041AA32 D1 E8                             shr     eax, 1
.text:0041AA34 0B C6                             or      eax, esi
.text:0041AA36 47                                inc     edi
.text:0041AA37 33 05 BC 29 6B 00                 xor     eax, LoginKey_2
.text:0041AA3D C1 E2 1F                          shl     edx, 31
.text:0041AA40 89 83 F8 00 0A 00                 mov     [ebx+0A00F8h], eax
.text:0041AA46 D1 E8                             shr     eax, 1
.text:0041AA48 0B C6                             or      eax, esi
.text:0041AA4A 8B 35 BC 29 6B 00                 mov     esi, LoginKey_2
.text:0041AA50 33 C6                             xor     eax, esi
.text:0041AA52 D1 E9                             shr     ecx, 1
.text:0041AA54 89 83 F8 00 0A 00                 mov     [ebx+0A00F8h], eax
.text:0041AA5A 0B CA                             or      ecx, edx
.text:0041AA5C 8B 15 B8 29 6B 00                 mov     edx, LoginKey_1
*/
// -- -- -- -- -- --
// 1F D1 E8 0B C6 47 33 05 memoryloc_2 C1 E2 1F 89 83 F8 00 0A 00 D1 E8 0B C6 8B 35 memoryloc_2 33 C6 D1 E9 89 83 F8 00 0A 00 0B Ca 8b 15 memoryloc_1 33 CA
#define CRYPT_KEY_STR_NEW "\x1F\xD1\xE8\x0B\xC6\x47\x33\x05"
#define CRYPT_KEY_NEW_LEN 8

/*
.text:0041C599 8B F0                             mov     esi, eax
.text:0041C59B 8B FA                             mov     edi, edx
.text:0041C59D D1 E8                             shr     eax, 1
.text:0041C59F C1 E7 1F                          shl     edi, 31
.text:0041C5A2 0B C7                             or      eax, edi
.text:0041C5A4 33 05 FC AB 6C 00                 xor     eax, dword_6CABFC
.text:0041C5AA D1 EA                             shr     edx, 1
.text:0041C5AC D1 E8                             shr     eax, 1
.text:0041C5AE C1 E6 1F                          shl     esi, 31
.text:0041C5B1 0B C7                             or      eax, edi
.text:0041C5B3 33 05 FC AB 6C 00                 xor     eax, dword_6CABFC
.text:0041C5B9 0B D6                             or      edx, esi
.text:0041C5BB 33 15 00 AC 6C 00                 xor     edx, dword_6CAC00
.text:0041C5C1 83 EB 01                          sub     ebx, 1
.text:0041C5C4 83 C5 01                          add     ebp, 1
.text:0041C5C7 85 DB                             test    ebx, ebx
*/
// E8 C1 E7 1F 0B C7 33 05 memoryloc_2
#define CRYPT_KEY_STR_MORE_NEW "\xE8\xC1\xE7\x1F\x0B\xC7\x33\x05"
#define CRYPT_KEY_MORE_NEW_LEN 8
