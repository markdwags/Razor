#include "stdafx.h"
#include "UOArt.h"
#include "Crypt.h"
#include "Resource.h"
#include <uxtheme.h>
#include <vssym32.h>
#include <dwmapi.h>

UOItem *ArtCache = NULL;
unsigned short **Hues = NULL;
int NumHues = 0;

inline int Round(float n)
{
	int i = (int)n;
	return i + (n - i >= 0.5 ? 1 : 0);
}

unsigned short *GetHue(int index)
{
	if (Hues == NULL)
	{
		if (!pShared)
			return NULL;

		char str[512];
		int length, blockCount, index;
		FILE *huesMul = NULL;

		WaitForSingleObject(CommMutex, INFINITE);
		sprintf(str, "%s/hues.mul", pShared->DataPath);
		ReleaseMutex(CommMutex);

		huesMul = fopen(str, "rb");
		if (!huesMul)
		{
			Hues = new unsigned short *[1];
			Hues[0] = new unsigned short[34];
			memset(Hues[0], 0, 34 * 2);
			NumHues = 1;
			return NULL;
		}

		fseek(huesMul, 0, SEEK_END);
		length = (int)ftell(huesMul);
		fseek(huesMul, 0, SEEK_SET);
		blockCount = length / 708;
		if (blockCount > 375)
			blockCount = 375;
		NumHues = blockCount * 8;

		Hues = new unsigned short *[NumHues];

		index = 0;
		for (int b = 0; b < blockCount; b++)
		{
			fseek(huesMul, 4, SEEK_CUR);

			for (int i = 0; i < 8; i++, index++)
			{
				Hues[index] = new unsigned short[34];
				for (int c = 0; c < 34; c++)
				{
					unsigned short color;
					fread(&color, 2, 1, huesMul);
					Hues[index][c] = color | 0x8000;
				}

				fseek(huesMul, 20, SEEK_CUR); // ignore name
			}
		}
		fclose(huesMul);
	}

	if (index > 0 && index <= NumHues)
		return Hues[index - 1];
	else
		return NULL;
}

unsigned short ApplyHueToPixel(unsigned short *hue, unsigned short pix)
{
	if (hue)
		return hue[(pix >> 10) & 31];
	else
		return pix;
}

UOItem *ReadUOItem(int item, int bh)
{
	if (item == 0 || item >= 0xFFFF || !pShared)
		return NULL;

	char str[512];
	short *Lookup;
	unsigned short *Run;

	FILE *idxMul, *artMul;
	ArtIdx idx;
	ArtHeader header;
	memset(&header, 0, sizeof(ArtHeader));

	WaitForSingleObject(CommMutex, INFINITE);
	sprintf(str, "%s/artidx.mul", pShared->DataPath);
	ReleaseMutex(CommMutex);

	idxMul = fopen(str, "rb");
	if (!idxMul)
		return NULL;
	fseek(idxMul, item * sizeof(ArtIdx), SEEK_SET);
	fread(&idx, sizeof(ArtIdx), 1, idxMul);
	fclose(idxMul);
	if (idx.FilePos == -1 || idx.Length == -1)
		return NULL;

	WaitForSingleObject(CommMutex, INFINITE);
	sprintf(str, "%s/art.mul", pShared->DataPath);
	ReleaseMutex(CommMutex);

	artMul = fopen(str, "rb");
	if (!artMul)
	{
		fclose(idxMul);
		return NULL;
	}
	fseek(artMul, idx.FilePos, SEEK_SET);
	fread(&header, sizeof(ArtHeader), 1, artMul);
	if (header.Height <= 0 || header.Width <= 0 || header.Height >= 1024 || header.Width >= 1024 || header.Unknown > 0xFFFF || header.Unknown == 0)
	{
		fclose(artMul);
		return NULL;
	}

	Run = new unsigned short[header.Width]; // it should never be wider than the whole image!
	Lookup = new short[header.Height];
	fread(Lookup, header.Height * 2, 1, artMul);
	long dataStart = ftell(artMul);

	UOItem *pNew = new UOItem;
	pNew->ItemID = item;
	pNew->pNext = ArtCache;
	ArtCache = pNew;

	unsigned short **Image = new unsigned short *[header.Width];
	for (int i = 0; i < header.Width; i++)
	{
		Image[i] = new unsigned short[header.Height];
		memset(Image[i], 0, header.Height * 2);
	}

	pNew->Left = pNew->Top = 0x7FFFFFFF;
	pNew->Right = pNew->Bottom = 0;
	for (int y = 0; y < header.Height; y++)
	{
		int x = 0;

		fseek(artMul, dataStart + Lookup[y] * 2, SEEK_SET);
		do
		{
			short RunOffset = 0, RunLength = 0;

			fread(&RunOffset, 2, 1, artMul);
			fread(&RunLength, 2, 1, artMul);

			if (RunLength <= 0 || RunOffset < 0 || RunOffset + RunLength >= 2048 || RunLength > header.Width)
				break;

			if (y > pNew->Bottom)
				pNew->Bottom = y;
			if (y < pNew->Top)
				pNew->Top = y;

			x += RunOffset;
			if (x < pNew->Left)
				pNew->Left = x;

			fread(Run, RunLength * 2, 1, artMul);
			for (int o = 0; o < RunLength; o++, x++)
				Image[x][y] = Run[o];

			if (x > pNew->Right)
				pNew->Right = x;
		} while (true);
	}
	fclose(artMul);

	delete[] Run;
	delete[] Lookup;

	float scale = float(bh) / float(pNew->GetHeight());
	if (scale > 1 || scale <= 0)
		scale = 1;

	pNew->RealHeight = (int)(header.Height * scale + 1);
	pNew->RealWidth = (int)(header.Width * scale + 1);
	pNew->Data = new unsigned short *[pNew->RealWidth];
	for (int x = 0; x < pNew->RealWidth; x++)
	{
		pNew->Data[x] = new unsigned short[pNew->RealHeight];
		memset(pNew->Data[x], 0, 2 * pNew->RealHeight);
	}

	for (int x = 0; x < header.Width; x++)
	{
		for (int y = 0; y < header.Height; y++)
			pNew->Data[(int)(x * scale)][(int)(y * scale)] |= Image[x][y];
	}

	pNew->Top = (int)(pNew->Top * scale);
	pNew->Left = (int)(pNew->Left * scale);
	pNew->Bottom = (int)(pNew->Bottom * scale);
	pNew->Right = (int)(pNew->Right * scale);

	for (int x = 0; x < header.Width; x++)
		delete[] Image[x];
	delete[] Image;

	return pNew;
}

UOItem *FindItem(int item)
{
	UOItem *node = ArtCache;
	while (node != NULL)
	{
		if (node->ItemID == item)
			return node;
		else
			node = node->pNext;
	}

	return NULL;
}

inline COLORREF Brightness(int shift, COLORREF c)
{
	return RGB(min(255, GetRValue(c) + shift), min(255, GetGValue(c) + shift), min(255, GetBValue(c) + shift));
}

int DrawUOItem(HDC hDC, RECT rect, int item, int hueIdx)
{
	item |= 0x4000;

	rect.top++;
	rect.bottom--;
	int maxHeight = rect.bottom - rect.top;

	UOItem *i = FindItem(item);
	if (i == NULL)
		i = ReadUOItem(item, maxHeight);

	if (i == NULL)
		return 0;

	if (i->GetHeight() < maxHeight)
		rect.top += (maxHeight - i->GetHeight()) / 2;

	unsigned short *hue = GetHue(hueIdx);
	for (int x = i->Left; x <= i->Right; x++)
	{
		for (int y = i->Top; y <= i->Bottom; y++)
		{
			if (i->Data[x][y] != 0)
				SetPixel(hDC, rect.left + x - i->Left, rect.top + y - i->Top, Brightness(0x30, Color16to32(ApplyHueToPixel(hue, i->Data[x][y]))));
		}
	}

	return i->GetWidth() + 3;
}

void FreeItem(UOItem *node)
{
	if (node != NULL)
	{
		FreeItem(node->pNext);
		for (int i = 0; i < node->RealWidth; i++)
			delete[] node->Data[i];
		delete[] node->Data;
		delete node;
	}
}

void FreeArt()
{
	FreeItem(ArtCache);
	if (Hues && NumHues > 0)
	{
		for (int i = 0; i < NumHues; i++)
			delete[] Hues[i];
		delete[] Hues;
	}
}

int DrawUOItem(HDC, RECT, int, int);

typedef HTHEME(__stdcall *OPENTHEMEDATA)(HWND, LPCWSTR);
OPENTHEMEDATA zOpenThemeData = NULL;
typedef HRESULT(__stdcall *DRAWTHEMEBACKGROUND)(HTHEME, HDC, int, int, const RECT *, OPTIONAL const RECT *);
DRAWTHEMEBACKGROUND zDrawThemeBackground = NULL;
typedef HRESULT(__stdcall *CLOSETHEMEDATA)(HTHEME);
CLOSETHEMEDATA zCloseThemeData = NULL;
typedef HRESULT(__stdcall *GETTHEMESYSFONT)(HTHEME, int, OUT LOGFONTW *);
GETTHEMESYSFONT zGetThemeSysFont = NULL;
typedef COLORREF(__stdcall *GETTHEMESYSCOLOR)(HTHEME, int);
GETTHEMESYSCOLOR zGetThemeSysColor = NULL;
typedef HRESULT(__stdcall *GETTHEMEMETRIC)(HTHEME, HDC, int, int, int, int *);
GETTHEMEMETRIC zGetThemeMetric = NULL;
typedef HRESULT(__stdcall *DWMSETWINDOWATTRIBUTE)(HWND, DWORD, LPCVOID, DWORD);
DWMSETWINDOWATTRIBUTE zDwmSetWindowAttribute = NULL;
HMODULE hThemes = NULL;
HMODULE hDwmApi = NULL;

void InitThemes()
{
	hThemes = LoadLibrary("UXTHEME.DLL");
	if (hThemes)
	{
		zOpenThemeData = (OPENTHEMEDATA)GetProcAddress(hThemes, "OpenThemeData");
		zDrawThemeBackground = (DRAWTHEMEBACKGROUND)GetProcAddress(hThemes, "DrawThemeBackground");
		zCloseThemeData = (CLOSETHEMEDATA)GetProcAddress(hThemes, "CloseThemeData");
		zGetThemeSysColor = (GETTHEMESYSCOLOR)GetProcAddress(hThemes, "GetThemeSysColor");
		zGetThemeSysFont = (GETTHEMESYSFONT)GetProcAddress(hThemes, "GetThemeSysFont");
		zGetThemeMetric = (GETTHEMEMETRIC)GetProcAddress(hThemes, "GetThemeMetric");
	}

	hDwmApi = LoadLibrary("DWMAPI.DLL");
	if (hDwmApi)
	{
		zDwmSetWindowAttribute = (DWMSETWINDOWATTRIBUTE)GetProcAddress(hDwmApi, "DwmSetWindowAttribute");
	}
}

inline int GetHex2(LPCSTR hex)
{
	int num = 0;
	if (!isxdigit(hex[0]) || !isxdigit(hex[1]))
		return -1;

	num = isdigit(hex[1]) ? (hex[1] - '0') : (tolower(hex[1]) - 'a' + 10);
	num += (isdigit(hex[0]) ? (hex[0] - '0') : (tolower(hex[0]) - 'a' + 10)) * 16;

	return num;
}

inline int GetHex4(LPCSTR hex)
{
	return (GetHex2(hex) << 8) | GetHex2(&hex[2]);
}

void DoStat(HDC hDC, int v, int t, int l, int h, int w)
{
	if (w <= 0)
		return;
	RECT fill;
	fill.top = t;
	fill.left = l;
	fill.bottom = t + h;
	fill.right = l + w;
	HBRUSH hBr = NULL;

	if (v == 0xFF)                                //poisoned
		hBr = CreateSolidBrush(RGB(255, 128, 0)); // orange
	else if (v <= 25)
		hBr = CreateSolidBrush(RGB(255, 0, 0)); // red
	else if (v <= 75)
		hBr = CreateSolidBrush(RGB(255, 255, 0)); // yellow
	else
		hBr = CreateSolidBrush(RGB(0, 255, 0)); // green

	FillRect(hDC, &fill, hBr);
	DeleteObject(hBr);
}

int DrawStatBar(HDC hDC, RECT rect, int width, int status, int hp, int mn, int st)
{
	HGDIOBJ hOld = NULL;
	POINT pt[2];
	rect.right = rect.left + width + 2;

	int o = (rect.bottom - rect.top - 20) / 2;
	if (o > 0)
		rect.top += o;
	rect.bottom = rect.top + 6 + 6 + 6 + 1;

	hOld = SelectObject(hDC, GetStockObject(BLACK_PEN));
	FrameRect(hDC, &rect, (HBRUSH)GetStockObject(BLACK_BRUSH));

	pt[0].x = rect.left;
	pt[1].x = rect.right;

	pt[0].y = rect.top + 6;
	pt[1].y = rect.top + 6;
	Polyline(hDC, pt, 2);

	pt[0].y = rect.top + 6 + 6;
	pt[1].y = rect.top + 6 + 6;
	Polyline(hDC, pt, 2);

	if (status == 1) // poisoned
		DoStat(hDC, 0xFF, rect.top + 1, rect.left + 1, 5, (int)((double(hp + 1) / 100.0) * width));
	else
		DoStat(hDC, hp, rect.top + 1, rect.left + 1, 5, (int)((double(hp + 1) / 100.0) * width));

	DoStat(hDC, mn, rect.top + 1 + 6, rect.left + 1, 5, (int)((double(mn + 1) / 100) * width));
	DoStat(hDC, st, rect.top + 2 + 6 + 5, rect.left + 1, 5, (int)((double(st + 1) / 100) * width));

	SelectObject(hDC, hOld);
	return width + 2;
}

void CheckTitlebarAttr(HWND hWnd)
{
	static bool curNCRP = true;
	bool newNCRP = !pShared || pShared->TitleBar[0] == '\0';

	if (curNCRP != newNCRP && zDwmSetWindowAttribute)
	{
		DWMNCRENDERINGPOLICY policy = newNCRP ? DWMNCRP_ENABLED : DWMNCRP_DISABLED;
		zDwmSetWindowAttribute(hWnd, DWMWA_NCRENDERING_POLICY, &policy, sizeof(policy));
		curNCRP = newNCRP;
	}
}

void DrawColorTitleBar(HTHEME hTheme, HWND hWnd, HDC hOutDC, bool active, bool maximized, LPCSTR str, int len, RECT orig)
{
	const COLORREF def = GetSysColor(active ? COLOR_CAPTIONTEXT : COLOR_INACTIVECAPTIONTEXT);
	COLORREF color = def;

	RECT rect;
	RECT window;
	GetWindowRect(hWnd, &window);
	rect.left = rect.top = 0;
	rect.right = window.right - window.left;
	rect.bottom = (orig.bottom - orig.top);

	HDC hDC = CreateCompatibleDC(hOutDC);
	HBITMAP hBmp = CreateCompatibleBitmap(hOutDC, rect.right, rect.bottom);
	SelectObject(hDC, hBmp);

	bool needRegFill = true;
	HFONT hFont = NULL;

	if (hThemes)
	{
		LOGFONTW lf;

		if (maximized)
		{
			needRegFill = zDrawThemeBackground(hTheme, hDC, WP_MAXCAPTION, active ? MXCS_ACTIVE : MXCS_INACTIVE, &rect, NULL) != S_OK;
		}
		else
		{
			int modTop = GetSystemMetrics(SM_CYFRAME);
			rect.top -= modTop;
			needRegFill = zDrawThemeBackground(hTheme, hDC, WP_CAPTION, active ? CS_ACTIVE : CS_INACTIVE, &rect, NULL) != S_OK;
			rect.top += modTop;
		}

		if (zGetThemeSysFont(hTheme, TMT_CAPTIONFONT, &lf) == S_OK)
		{
			hFont = CreateFontIndirectW(&lf);
			SelectObject(hDC, hFont);
		}
	}

	if (needRegFill)
		FillRect(hDC, &rect, GetSysColorBrush(active ? COLOR_ACTIVECAPTION : COLOR_INACTIVECAPTION));

	if (hFont == NULL)
		SelectObject(hDC, GetStockObject(ANSI_VAR_FONT));

	rect.left = orig.left;

	int start = 0;
	int dlen = 0;
	int t;
	SetBkMode(hDC, TRANSPARENT);
	for (int i = 0; i < len; i++)
	{
		if (rect.left >= rect.right)
			break;

		if (str[i] == '~')
		{
			switch (str[i + 1])
			{
			case '#':
				if (dlen > 0)
				{
					SetTextColor(hDC, color);
					DrawText(hDC, &str[start], dlen, &rect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
					SIZE ext;
					GetTextExtentPoint32(hDC, &str[start], dlen, &ext);
					rect.left += ext.cx;
				}
				dlen = 0;

				if (str[i + 2] == '~')
				{
					color = def;
					SetBkMode(hDC, TRANSPARENT);
					start = i + 3;
					i += 2;
				}
				else
				{
					color = RGB(GetHex2(&str[i + 2]), GetHex2(&str[i + 4]), GetHex2(&str[i + 6]));
					start = i + 8;
					i += 7;
				}

				break;
			case 'S':
			case 's':
				// ~SSFRRGGBB
				if (dlen > 0)
				{
					SetTextColor(hDC, color);
					DrawText(hDC, &str[start], dlen, &rect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
					SIZE ext;
					GetTextExtentPoint32(hDC, &str[start], dlen, &ext);
					rect.left += ext.cx;
				}
				dlen = 0;

				t = 30;
				switch (toupper(str[i + 2]))
				{
				case 'S':
					t = 15;
					break;
					//case 'R': w = 30; break;
				case 'M':
					t = 45;
					break;
				case 'L':
					t = 60;
					break;
				case 'X':
					t = 75;
					break;
				}

				rect.left += 1;
				rect.left += DrawStatBar(hDC, rect, t, str[i + 3] - '0', GetHex2(&str[i + 4]), GetHex2(&str[i + 6]), GetHex2(&str[i + 8]));
				rect.left += 1;

				start = i + 10;
				i += 9;

				break;
			case 'I':
			case 'i':
				if (dlen > 0)
				{
					SetTextColor(hDC, color);
					DrawText(hDC, &str[start], dlen, &rect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
					SIZE ext;
					GetTextExtentPoint32(hDC, &str[start], dlen, &ext);
					rect.left += ext.cx;
				}
				dlen = 0;
				if (str[i + 6] == '~')
				{
					rect.left += DrawUOItem(hDC, rect, GetHex4(&str[i + 2]), 0);
					start = i + 7;
					i += 6;
				}
				else
				{
					rect.left += DrawUOItem(hDC, rect, GetHex4(&str[i + 2]), GetHex4(&str[i + 6]));
					start = i + 10;
					i += 9;
				}
				break;
			case '^':
				if (dlen > 0)
				{
					SetTextColor(hDC, color);
					DrawText(hDC, &str[start], dlen, &rect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
					SIZE ext;
					GetTextExtentPoint32(hDC, &str[start], dlen, &ext);
					rect.left += ext.cx;
				}
				dlen = 0;

				color = RGB(0, 0, 0);
				SetBkMode(hDC, OPAQUE);
				SetBkColor(hDC, RGB(GetHex2(&str[i + 2]), GetHex2(&str[i + 4]), GetHex2(&str[i + 6])));

				start = i + 8;
				i += 7;
				break;
			case 'B':
			case 'b': // skip bold identifier
				i++;
				break;
			}
		}
		else
		{
			dlen++;
		}
	}

	if (dlen > 0)
	{
		SetTextColor(hDC, color);
		DrawText(hDC, &str[start], dlen, &rect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
	}

	BitBlt(hOutDC, orig.left, orig.top, (orig.right - orig.left), (orig.bottom - orig.top) - 1, hDC, orig.left, 0, SRCCOPY);
	DeleteDC(hDC);
	DeleteObject(hBmp);
	if (hFont)
		DeleteObject(hFont);
}

void RedrawTitleBar(HWND hWnd, bool active)
{
	if (!pShared)
		return;

	WaitForSingleObject(CommMutex, INFINITE);
	if (pShared->TitleBar[0] == 0)
	{
		ReleaseMutex(CommMutex);
		return;
	}

	int len = (int)strlen(pShared->TitleBar);
	if (len >= 1024)
		len = 1023;

	WINDOWPLACEMENT place;
	RECT rect;
	HDC hDC = GetWindowDC(hWnd); //WINDOW dc allows us to draw on the non client area

	GetWindowPlacement(hWnd, &place);
	GetWindowRect(hWnd, &rect);

	// Change the coords (believe me, okay?)
	rect.top = GetSystemMetrics(SM_CYFRAME);
	rect.bottom = rect.top + GetSystemMetrics(SM_CYCAPTION);

	rect.right = (rect.right - rect.left) - (4 * GetSystemMetrics(SM_CXSIZE) + GetSystemMetrics(SM_CXFRAME));
	rect.left = GetSystemMetrics(SM_CXSIZEFRAME) + GetSystemMetrics(SM_CXSMICON) + 5;

	if (hThemes)
	{
		HTHEME hTheme = zOpenThemeData(hWnd, L"WINDOW");
		DrawColorTitleBar(hTheme, hWnd, hDC, active, place.showCmd == SW_MAXIMIZE, pShared->TitleBar, len, rect);
		zCloseThemeData(hTheme);
	}
	else
	{
		rect.left += GetSystemMetrics(SM_CXFRAME);
		DrawColorTitleBar(NULL, hWnd, hDC, active, place.showCmd == SW_MAXIMIZE, pShared->TitleBar, len, rect);
	}

	ReleaseDC(hWnd, hDC);
	ReleaseMutex(CommMutex);
}
