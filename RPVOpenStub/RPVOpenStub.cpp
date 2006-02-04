#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

#include <windows.h>
#include "resource.h"

#define WM_UONETMESSAGE WM_USER+1
#define OPEN_RPV 18

int __stdcall WinMain( HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR CmdLine, int nCmdShow )
{
	ATOM atom = GlobalAddAtom( CmdLine );
	if ( !atom )
		return 1;

	HWND hWnd = FindWindow( "Ultima Online", NULL );
	if ( hWnd == NULL )
		hWnd = FindWindow( "Ultima Online Third Dawn", NULL );

	if ( hWnd )
		SendMessage( hWnd, WM_UONETMESSAGE, OPEN_RPV, atom );
	else
		MessageBox( NULL, "Ultima Online and Razor must be running to play an RPV file.", "UO Not Running", MB_OK|MB_ICONSTOP );

    Sleep( 100 );

	DeleteAtom( atom );

	return 0;
}
