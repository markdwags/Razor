#ifndef __RAZORRECEIVER_H_
#define __RAZORRECEIVER_H_ 

#include <windows.h>

#define WM_SET_FWD_HWND WM_USER+4 // Send this message to UO to give it your HWND
#define WM_PACKETREADY WM_USER+5 // Razor sends these messages when we should call ReceivePacket()
#define SHARED_BUFF_SIZE 524288 // 262144 // 250k
#define FWD_BUFF_OFFSET 0

class RazorReceiver
{
public:
	RazorReceiver();
	virtual ~RazorReceiver();

	bool Initialize( DWORD uoProcId ); // returns false if initialization failed

	int ReceivePacket( void *data, int maxLen ); // returns the amount of data copied
	bool PacketAvailable();

	HWND GetUoHwnd(); // returns UO's HWND

private:
	struct Buffer
	{
		int Length;
		int Start;
		unsigned char Buff[SHARED_BUFF_SIZE];
	};

	DWORD _UoPID;
	HANDLE _hMutex;
	HANDLE _hMap;
	void *_pShared;
	RazorReceiver::Buffer *_pBuff;
};

#endif
