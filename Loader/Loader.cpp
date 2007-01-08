
#include "stdafx.h"

BOOL APIENTRY DllMain( HANDLE hModule, DWORD dwReason, LPVOID lpReserved )
{
	switch ( dwReason )
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls( (HMODULE)hModule );
		break;
	}

    return TRUE;
}

enum ERROR_TYPE
{
	SUCCESS = 0,
	NO_OPEN_EXE,
	NO_MAP_EXE,
	NO_READ_EXE_DATA,

	NO_RUN_EXE,
	NO_ALLOC_MEM,

	NO_WRITE,
	NO_VPROTECT,
	NO_READ,
};

__declspec(dllexport) DWORD __stdcall Load( const char *exe, const char *dll, const char *func, const void *dllData, int dataLen, DWORD *pid )
{
	IMAGE_OPTIONAL_HEADER32 *ioh;
	IMAGE_DOS_HEADER *idh;
	STARTUPINFO StartInfo;
	PROCESS_INFORMATION ProcInfo;
	HANDLE hFile, hMap;
	LPVOID Entry;
	SIZE_T Num;
	char buff[MAX_PATH];

	// first lets read the desired info from the exe file...
	hFile = CreateFile( exe, FILE_GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );
	if ( !hFile )
	{
		buff[0] = 0;
		if ( GetShortPathName( exe, buff, MAX_PATH ) >= MAX_PATH )
			buff[0] = 0;
		hFile = CreateFile( buff, FILE_GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );
		if ( !hFile )
			return NO_OPEN_EXE;
	}

	hMap = CreateFileMapping( hFile, NULL, PAGE_READONLY|SEC_COMMIT, 0, 0, NULL );
	if ( !hMap )
	{
		CloseHandle( hFile );
		return NO_MAP_EXE;
	}

	idh = (IMAGE_DOS_HEADER *)MapViewOfFile( hMap, FILE_MAP_READ, 0, 0, 0 );
	if ( !idh )
	{
		CloseHandle( hMap );
		CloseHandle( hFile );
		return NO_READ_EXE_DATA;
	}

    ioh = (IMAGE_OPTIONAL_HEADER32 *)(__int64(idh) + idh->e_lfanew + 4 + sizeof(IMAGE_FILE_HEADER));
    
	Entry = (LPVOID)(ioh->ImageBase + ioh->AddressOfEntryPoint); // ok we got the exe's entry point (first thing called, before main or WinMain)

	// close the exe file
	UnmapViewOfFile( idh );
	CloseHandle( hMap );
	CloseHandle( hFile );

	// find the exe's working directory
	strcpy( buff, exe );

	int i=(int)strlen(buff);
	while ( buff[i] != '\\' && buff[i] != '/' && i > 0 )
		i--;
	buff[i] = 0;

	//GetStartupInfo( &StartInfo );
	memset( &StartInfo, 0, sizeof(STARTUPINFO) );
	StartInfo.cb = sizeof(STARTUPINFO);

	// launch the exe
	if ( !CreateProcess( exe, NULL, NULL, NULL, FALSE, CREATE_SUSPENDED, NULL, buff, &StartInfo, &ProcInfo ) )
		return NO_RUN_EXE;

	*pid = ProcInfo.dwProcessId;

	int dllNameLen = (int)strlen(dll)+1;
	int funcNameLen = (int)strlen(func)+1;
	const int LoadAsmSize = 60;
	SIZE_T allocSize = dllNameLen + funcNameLen + dataLen + LoadAsmSize;

	// allocate some space in the exe for our memory
	DWORD ProcMem = (DWORD)VirtualAllocEx( ProcInfo.hProcess, NULL, allocSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE );
	if ( !ProcMem )
	{
		TerminateProcess( ProcInfo.hProcess, 0 );
		return NO_ALLOC_MEM;
	}

	char *toWrite = new char[allocSize];
	if ( !toWrite )
	{
		TerminateProcess( ProcInfo.hProcess, 0 );
		return NO_ALLOC_MEM;
	}

	// change protection to allow us to read/write the entry point (note: the old protection is not restored (so that we can change this space again later...))
	if ( !VirtualProtectEx( ProcInfo.hProcess, Entry, 8, PAGE_EXECUTE_READWRITE, (PDWORD)&Num ) )
	{
		char temp[256];
		sprintf( temp, "Entry = %llx, GetLastError() = %d", (__int64)Entry, GetLastError() );
		MessageBox( NULL, temp, "ERROR!", 0 );
		TerminateProcess( ProcInfo.hProcess, 0 );
		return NO_VPROTECT;
	}
	
	// read the old code at the entry point
	if ( !ReadProcessMemory( ProcInfo.hProcess, Entry, buff, 8, &Num ) || Num != 8 )
	{
		TerminateProcess( ProcInfo.hProcess, 0 );
		return NO_READ;
	}

	// note if you change this size you must rewrite the LoadAsm to copy back the original code properly
	unsigned char EntryAsm[] = {
		0xE9, 0,0,0,0,			// jmp (offset)
		0x90,0x90,0x90,			// nop (for 4 byte alignment)
	};

	*((DWORD*)&EntryAsm[1]) = ProcMem - ((DWORD)Entry + 5);

	// write the new code at the entry point
	if ( !WriteProcessMemory( ProcInfo.hProcess, Entry, EntryAsm, 8, &Num ) || Num != 8 )
	{
		TerminateProcess( ProcInfo.hProcess, 0 );
		return NO_WRITE;
	}

	// this asm section does everything
	unsigned char LoadAsm[LoadAsmSize] = {
		// restore the original entry point bytes
		0xB8, 0,0,0,0,			// mov     eax, (number)
		0xC7, 0x00, 0,0,0,0,	// mov     dword ptr [eax], (num)
		0xC7, 0x40, 4, 0,0,0,0, // mov     dword ptr [eax+4], (num)

		// LoadLibrary( name )
		0x68, 0,0,0,0,			// push (num)
		0xE8, 0,0,0,0,			// call (offset)

		// GetProcAddress( lib, name )
		0x68, 0,0,0,0,			// push (num)
		0x50,					// push eax
		0xE8, 0,0,0,0,			// call (offset)

		// name( data, dataLen )
		0x68, 0,0,0,0,			// push (num)
		0x68, 0,0,0,0,			// push (num)
		0xFF, 0xD0,				// call eax

		// go back to the original entry point
		0xE9, 0,0,0,0,			// jmp  (offset)
		
		0x90, 0x90, 0x90, 0x90,	// nop (padding)
	};

	*((DWORD*)(LoadAsm+ 1)) = (DWORD)Entry; // data dest
	*((DWORD*)(LoadAsm+ 7)) = *((DWORD*)&buff[0]); // 1st data 
	*((DWORD*)(LoadAsm+14)) = *((DWORD*)&buff[4]); // 2nd data

	HMODULE hKernel = LoadLibrary( "Kernel32" );

	*((DWORD*)(LoadAsm+19)) = (ProcMem + LoadAsmSize); // begining of dll name
	*((DWORD*)(LoadAsm+24)) = ((DWORD)GetProcAddress( hKernel, "LoadLibraryA" )) - (ProcMem + 28); // offset to LoadLibraryA
	
	*((DWORD*)(LoadAsm+29)) = (ProcMem + LoadAsmSize + dllNameLen); // begining of function name
	*((DWORD*)(LoadAsm+35)) = ((DWORD)GetProcAddress( hKernel, "GetProcAddress" )) - (ProcMem + 39); // offset to GetProcAddress

	FreeLibrary( hKernel );

	*((DWORD*)(LoadAsm+40)) = dataLen; // user data Length
	*((DWORD*)(LoadAsm+45)) = ProcMem + LoadAsmSize + dllNameLen + funcNameLen; // begining of user data
	*((DWORD*)(LoadAsm+52)) = (DWORD)Entry - (ProcMem + 56); // entry points

	memcpy( toWrite, LoadAsm, LoadAsmSize );
	memcpy( &toWrite[LoadAsmSize], dll, dllNameLen );
	memcpy( &toWrite[LoadAsmSize+dllNameLen], func, funcNameLen );

	if ( dataLen > 0 )
		memcpy( &toWrite[LoadAsmSize+dllNameLen+funcNameLen], dllData, dataLen );

	if ( !WriteProcessMemory( ProcInfo.hProcess, (void*)ProcMem, toWrite, allocSize, &Num ) || Num != allocSize )
	{
		TerminateProcess( ProcInfo.hProcess, 0 );
		return NO_WRITE;
	}

	delete[] toWrite;

	*pid = ProcInfo.dwProcessId;

	ResumeThread( ProcInfo.hThread );

	CloseHandle( ProcInfo.hProcess );
	CloseHandle( ProcInfo.hThread );

	return SUCCESS;
}
