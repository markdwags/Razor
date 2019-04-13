#include "Platform.h"

HINSTANCE hInstance = NULL;
HICON hRazorIcon = NULL;
HFONT hRazorFont = NULL;

BOOL APIENTRY DllMain(HANDLE hModule, DWORD dwReason, LPVOID)
{
	hInstance = (HMODULE)hModule;

	switch (dwReason) {
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls(hInstance);
		break;

	case DLL_PROCESS_DETACH:
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}

	return TRUE;
}

DLLFUNCTION HBITMAP CaptureScreen(HWND hUOWindow, BOOL full, const char *msg)
{
	HDC hScreen = NULL;
	int destWidth, destHeight;
	RECT rect;

	if (!hRazorFont)
		hRazorFont = CreateFont(-10, 0, 0, 0, FW_SEMIBOLD, FALSE, FALSE, FALSE, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS, DRAFT_QUALITY, FIXED_PITCH, "Comic Sans MS");

	UpdateWindow(hUOWindow);

	if (full)
	{
		hScreen = GetDC(NULL);
		rect.left = 0;
		rect.top = 0;
		destWidth = GetDeviceCaps(hScreen, HORZRES);
		destHeight = GetDeviceCaps(hScreen, VERTRES);
	}
	else
	{
		hScreen = GetWindowDC(hUOWindow);
		GetWindowRect(hUOWindow, &rect);
		destWidth = rect.right - rect.left;
		destHeight = rect.bottom - rect.top;
	}

	HDC hCap = CreateCompatibleDC(hScreen);
	HBITMAP hBMP = CreateCompatibleBitmap(hScreen, destWidth, destHeight);
	SelectObject(hCap, hBMP);

	BitBlt(hCap, 0, 0, destWidth, destHeight, hScreen, 0, 0, SRCCOPY);

	if (msg != NULL && msg[0] != 0)
	{
		SIZE text;
		int len = (int)strlen(msg);
		SelectObject(hCap, hRazorFont);
		SetTextColor(hCap, RGB(255, 255, 255));
		SetBkMode(hCap, TRANSPARENT);

		GetTextExtentPoint32(hCap, msg, len, &text);

		rect.top = 0;
		rect.bottom = text.cy + 4;
		rect.right = destWidth - 32;
		rect.left = destWidth - 32 - text.cx - 4;
		FillRect(hCap, &rect, (HBRUSH)GetStockObject(BLACK_BRUSH));

		TextOut(hCap, rect.left + 2, 2, msg, len);
	}

	ReleaseDC(full ? NULL : hUOWindow, hScreen);
	DeleteDC(hCap);

	return hBMP;
}

DLLFUNCTION void BringToFront(HWND hWnd)
{
	SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
	ShowWindow(hWnd, SW_SHOW);
	SetForegroundWindow(hWnd);
	SetFocus(hWnd);
}