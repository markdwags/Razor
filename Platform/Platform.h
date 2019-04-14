#pragma once

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <winsock2.h>
#include <time.h>
#include <zmouse.h>

#define DLLFUNCTION __declspec(dllexport)

DLLFUNCTION HBITMAP CaptureScreen(HWND hUOWindow, BOOL full, const char *msg);
DLLFUNCTION void BringToFront(HWND hWnd);