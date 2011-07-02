#include "stdafx.h"
#include "Crypt.h"
#include "Resource.h"
#include <uxtheme.h>
#include <vssym32.h>
#include <dwmapi.h>

int DrawUOItem( HDC, RECT, int, int );

typedef HTHEME (__stdcall *OPENTHEMEDATA)( HWND, LPCWSTR );
OPENTHEMEDATA zOpenThemeData = NULL;
typedef HRESULT (__stdcall *DRAWTHEMEBACKGROUND)(HTHEME, HDC, int, int, const RECT *, OPTIONAL const RECT *);
DRAWTHEMEBACKGROUND zDrawThemeBackground = NULL;
typedef HRESULT (__stdcall *CLOSETHEMEDATA)( HTHEME );
CLOSETHEMEDATA zCloseThemeData = NULL;
typedef HRESULT (__stdcall *GETTHEMESYSFONT)(HTHEME, int, OUT LOGFONTW*);
GETTHEMESYSFONT zGetThemeSysFont = NULL;
typedef COLORREF (__stdcall *GETTHEMESYSCOLOR)(HTHEME, int);
GETTHEMESYSCOLOR zGetThemeSysColor = NULL;
typedef HRESULT (__stdcall *GETTHEMEMETRIC)(HTHEME,HDC,int,int,int,int*);
GETTHEMEMETRIC zGetThemeMetric = NULL;
typedef HRESULT (__stdcall *DWMSETWINDOWATTRIBUTE)(HWND, DWORD, LPCVOID, DWORD);
DWMSETWINDOWATTRIBUTE zDwmSetWindowAttribute = NULL;
HMODULE hThemes = NULL;
HMODULE hDwmApi = NULL;

HICON hRazorIcon = NULL;
HFONT hRazorFont = NULL;

void InitThemes()
{
	hThemes = LoadLibrary("UXTHEME.DLL");
	if( hThemes )
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

inline int GetHex2( LPCSTR hex )
{
	int num = 0;
	if ( !isxdigit( hex[0] ) || !isxdigit( hex[1] ) )
		return -1;

	num = isdigit(hex[1]) ? ( hex[1]-'0' ) : ( tolower(hex[1])-'a'+10 ); 
	num += (isdigit(hex[0]) ? ( hex[0]-'0' ) : ( tolower(hex[0])-'a'+10 )) * 16;

	return num;
}

inline int GetHex4( LPCSTR hex )
{
	return (GetHex2( hex ) << 8) | GetHex2( &hex[2] );
}

void DoStat( HDC hDC, int v, int t, int l, int h, int w )
{
	if ( w <= 0 )
		return;
	RECT fill;
	fill.top = t;
	fill.left = l;
	fill.bottom = t+h;
	fill.right = l+w;
	HBRUSH hBr = NULL;

	if ( v == 0xFF )//poisoned
		hBr = CreateSolidBrush( RGB(255,128,0) ); // orange
	else if ( v <= 25 )
		hBr = CreateSolidBrush( RGB(255,0,0) ); // red
	else if ( v <= 75 )
		hBr = CreateSolidBrush( RGB(255,255,0) ); // yellow
	else
		hBr = CreateSolidBrush( RGB(0,255,0) ); // green

	FillRect( hDC, &fill, hBr );
	DeleteObject( hBr );
}

int DrawStatBar( HDC hDC, RECT rect, int width, int status, int hp, int mn, int st )
{
	HGDIOBJ hOld = NULL;
	POINT pt[2];
	rect.right = rect.left + width + 2;

	int o = ( rect.bottom - rect.top - 20 ) / 2;
	if ( o > 0 )
		rect.top += o;
	rect.bottom = rect.top + 6 + 6 + 6 + 1;

	hOld = SelectObject( hDC, GetStockObject( BLACK_PEN ) );
	FrameRect( hDC, &rect, (HBRUSH)GetStockObject( BLACK_BRUSH ) );

	pt[0].x = rect.left;
	pt[1].x = rect.right;

	pt[0].y = rect.top + 6;
	pt[1].y = rect.top + 6;
	Polyline( hDC, pt, 2 );
	
	pt[0].y = rect.top + 6 + 6;
	pt[1].y = rect.top + 6 + 6;
	Polyline( hDC, pt, 2 );

	if ( status == 1 ) // poisoned
		DoStat( hDC, 0xFF, rect.top+1, rect.left+1, 5, (int)((double(hp+1)/100.0)*width) );
	else
		DoStat( hDC, hp, rect.top+1, rect.left+1, 5, (int)((double(hp+1)/100.0)*width) );

	DoStat( hDC, mn, rect.top+1+6, rect.left+1, 5, (int)((double(mn+1)/100)*width) );
	DoStat( hDC, st, rect.top+2+6+5, rect.left+1, 5, (int)((double(st+1)/100)*width) );
    
	SelectObject( hDC, hOld );
	return width+2;
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

void DrawColorTitleBar( HTHEME hTheme, HWND hWnd, HDC hOutDC, bool active, bool maximized, LPCSTR str, int len, RECT orig )
{
	const COLORREF def = GetSysColor( active ? COLOR_CAPTIONTEXT : COLOR_INACTIVECAPTIONTEXT );
	COLORREF color = def;

	RECT rect;
	RECT window;
	GetWindowRect( hWnd, &window );
	rect.left = rect.top = 0;
	rect.right = window.right - window.left;
	rect.bottom = (orig.bottom - orig.top);

	HDC hDC = CreateCompatibleDC( hOutDC );
	HBITMAP hBmp = CreateCompatibleBitmap( hOutDC, rect.right, rect.bottom );
	SelectObject( hDC, hBmp );

	bool needRegFill = true;
	HFONT hFont = NULL;

	if ( hThemes )
	{
		LOGFONTW lf;

		if ( maximized )
		{
			needRegFill = zDrawThemeBackground( hTheme, hDC, WP_MAXCAPTION, active ? MXCS_ACTIVE : MXCS_INACTIVE, &rect, NULL ) != S_OK;
		}
		else
		{
			int modTop = GetSystemMetrics(SM_CYFRAME);
			rect.top -= modTop;
			needRegFill = zDrawThemeBackground( hTheme, hDC, WP_CAPTION, active ? CS_ACTIVE : CS_INACTIVE, &rect, NULL ) != S_OK;
			rect.top += modTop;
		}

		if ( zGetThemeSysFont( hTheme, TMT_CAPTIONFONT, &lf ) == S_OK )
		{
			hFont = CreateFontIndirectW( &lf );
			SelectObject( hDC, hFont );
		}
	}

	if ( needRegFill )
		FillRect( hDC, &rect, GetSysColorBrush( active ? COLOR_ACTIVECAPTION : COLOR_INACTIVECAPTION ) );

	if ( hFont == NULL )
		SelectObject( hDC, GetStockObject( ANSI_VAR_FONT ) );

	rect.left = orig.left;
	
	int start = 0;
	int dlen = 0;
	int t;
	SetBkMode( hDC, TRANSPARENT );
	for (int i=0;i<len;i++)
	{
		if ( rect.left >= rect.right )
			break;

		if ( str[i] == '~' )
		{
			switch ( str[i+1] )
			{
				case '#':
					if ( dlen > 0 )
					{
						SetTextColor( hDC, color );
						DrawText( hDC, &str[start], dlen, &rect, DT_LEFT|DT_VCENTER|DT_SINGLELINE );
						SIZE ext;
						GetTextExtentPoint32( hDC, &str[start], dlen, &ext );
						rect.left += ext.cx;
					}
					dlen = 0;

					if ( str[i+2] == '~' )
					{
						color = def;
						SetBkMode( hDC, TRANSPARENT );
						start = i+3;
						i += 2;
					}
					else
					{
						color = RGB( GetHex2(&str[i+2]), GetHex2(&str[i+4]), GetHex2(&str[i+6]) );
						start = i+8;
						i += 7;
					}

					break;
				case 'S':
				case 's':
					// ~SSFRRGGBB
					if ( dlen > 0 )
					{
						SetTextColor( hDC, color );
						DrawText( hDC, &str[start], dlen, &rect, DT_LEFT|DT_VCENTER|DT_SINGLELINE );
						SIZE ext;
						GetTextExtentPoint32( hDC, &str[start], dlen, &ext );
						rect.left += ext.cx;
					}
					dlen = 0;

					t = 30;
					switch ( toupper( str[i+2] ) )
					{
					case 'S': t = 15; break;
					//case 'R': w = 30; break;
					case 'M': t = 45; break;
					case 'L': t = 60; break;
					case 'X': t = 75; break;
					}

					rect.left += 1;
					rect.left += DrawStatBar( hDC, rect, t, str[i+3] - '0', GetHex2(&str[i+4]), GetHex2(&str[i+6]), GetHex2(&str[i+8]) );
					rect.left += 1;

					start = i+10;
					i += 9; 

					break;
				case 'I':
				case 'i':
					if ( dlen > 0 )
					{
						SetTextColor( hDC, color );
						DrawText( hDC, &str[start], dlen, &rect, DT_LEFT|DT_VCENTER|DT_SINGLELINE );
						SIZE ext;
						GetTextExtentPoint32( hDC, &str[start], dlen, &ext );
						rect.left += ext.cx;
					}
					dlen = 0;
					if ( str[i+6] == '~' )
					{
						rect.left += DrawUOItem( hDC, rect, GetHex4( &str[i+2] ), 0 );
						start = i+7;
						i += 6;
					}
					else
					{
						rect.left += DrawUOItem( hDC, rect, GetHex4( &str[i+2] ), GetHex4( &str[i+6] ) );
						start = i+10;
						i += 9;
					}
					break;
				case '^':
					if ( dlen > 0 )
					{
						SetTextColor( hDC, color );
						DrawText( hDC, &str[start], dlen, &rect, DT_LEFT|DT_VCENTER|DT_SINGLELINE );
						SIZE ext;
						GetTextExtentPoint32( hDC, &str[start], dlen, &ext );
						rect.left += ext.cx;
					}
					dlen = 0;

					color = RGB(0,0,0);
					SetBkMode( hDC, OPAQUE );
					SetBkColor( hDC, RGB( GetHex2(&str[i+2]), GetHex2(&str[i+4]), GetHex2(&str[i+6]) ) );

					start = i+8;
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
	
	if ( dlen > 0 )
	{
		SetTextColor( hDC, color );
		DrawText( hDC, &str[start], dlen, &rect, DT_LEFT|DT_VCENTER|DT_SINGLELINE );
	}

	BitBlt( hOutDC, orig.left, orig.top, (orig.right - orig.left), (orig.bottom - orig.top) - 1, hDC, orig.left, 0, SRCCOPY );
	DeleteDC( hDC );
	DeleteObject( hBmp );
	if ( hFont )
		DeleteObject( hFont );
}

void RedrawTitleBar( HWND hWnd, bool active )
{
	if ( !pShared )
		return;

	WaitForSingleObject( CommMutex, INFINITE );
	if ( pShared->TitleBar[0] == 0 )
	{
		ReleaseMutex( CommMutex );
		return;
	}

	int len = (int)strlen( pShared->TitleBar );
	if ( len >= 1024 )
		len = 1023;

	WINDOWPLACEMENT place;
	RECT rect;
	HDC hDC = GetWindowDC( hWnd );//WINDOW dc allows us to draw on the non client area
	
	GetWindowPlacement( hWnd, &place );
	GetWindowRect( hWnd, &rect );

	// Change the coords (believe me, okay?)
	rect.top = GetSystemMetrics(SM_CYFRAME);
	rect.bottom = rect.top + GetSystemMetrics(SM_CYCAPTION);

	rect.right = ( rect.right - rect.left ) - ( 4*GetSystemMetrics(SM_CXSIZE) + GetSystemMetrics(SM_CXFRAME) );
	rect.left = GetSystemMetrics(SM_CXSIZEFRAME) + GetSystemMetrics(SM_CXSMICON) + 5;

	if ( hThemes )
	{
		HTHEME hTheme = zOpenThemeData( hWnd, L"WINDOW" );
		DrawColorTitleBar( hTheme, hWnd, hDC, active, place.showCmd == SW_MAXIMIZE, pShared->TitleBar, len, rect );
		zCloseThemeData( hTheme );
	}
	else
	{
		rect.left += GetSystemMetrics(SM_CXFRAME);
		DrawColorTitleBar( NULL, hWnd, hDC, active, place.showCmd == SW_MAXIMIZE, pShared->TitleBar, len, rect );
	}

	ReleaseDC( hWnd, hDC );
	ReleaseMutex( CommMutex );
}

void GetRazorFont()
{
	if ( !hRazorFont )
		hRazorFont = CreateFont( -10, 0, 0, 0, FW_SEMIBOLD, FALSE, FALSE, FALSE, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS, DRAFT_QUALITY, FIXED_PITCH, "Comic Sans MS" );
}

DLLFUNCTION HBITMAP CaptureScreen( BOOL full, const char *msg )
{
	HDC hScreen = NULL;
	int destWidth, destHeight;
	RECT rect;

	//SetForegroundWindow( hWatchWnd );
	UpdateWindow( hWatchWnd );

	if ( full )
	{
		hScreen = GetDC( NULL );
		rect.left = 0;
		rect.top = 0;
		destWidth = GetDeviceCaps( hScreen, HORZRES );
		destHeight = GetDeviceCaps( hScreen, VERTRES );
	}
	else
	{
		hScreen = GetWindowDC( hWatchWnd );
		GetWindowRect( hWatchWnd, &rect );
		destWidth = rect.right - rect.left;
		destHeight = rect.bottom - rect.top;
	}

	HDC hCap = CreateCompatibleDC( hScreen );
	HBITMAP hBMP = CreateCompatibleBitmap( hScreen, destWidth, destHeight );
	SelectObject( hCap, hBMP );

	BitBlt( hCap, 0, 0, destWidth, destHeight, hScreen, 0, 0, SRCCOPY );
	//StretchBlt( hCap, 0, 0, destWidth, destHeight, hScreen, rect.left, rect.top, srcWidth, srcHeight, SRCCOPY );

	if ( !hRazorIcon )
		hRazorIcon = LoadIcon( hInstance, MAKEINTRESOURCE(IDI_RAZOR) );

	if ( hRazorIcon )
	{
		rect.left = destWidth - 32;
		rect.right = destWidth;
		rect.top = 0;
		rect.bottom = 32;

		//FillRect( hCap, &rect, (HBRUSH)GetStockObject( BLACK_BRUSH ) );
		DrawIcon( hCap, rect.left, rect.top, hRazorIcon );
	}

	if ( msg != NULL && msg[0] != 0 )
	{
		SIZE text;
		int len = (int)strlen(msg);
		SelectObject( hCap, hRazorFont );
		SetTextColor( hCap, RGB(255,255,255) );
		SetBkMode( hCap, TRANSPARENT );

		GetTextExtentPoint32( hCap, msg, len, &text );
		
		rect.top = 0;
		rect.bottom = text.cy + 4;
		rect.right = destWidth - 32;
		rect.left = destWidth - 32 - text.cx - 4;
		FillRect( hCap, &rect, (HBRUSH)GetStockObject( BLACK_BRUSH ) );

		TextOut( hCap, rect.left + 2, 2, msg, len );
	}

	ReleaseDC( full ? NULL : hWatchWnd, hScreen );
	DeleteDC( hCap );

	return hBMP;
}
