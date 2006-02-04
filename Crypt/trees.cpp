
int Trees[] = {
	0x4CCA, 0x4CCB, 0x4CCC, 0x4CCD, 0x4CD0, 0x4CD3, 0x4CD6, 0x4CD8,
	0x4CDA, 0x4CDD, 0x4CE0, 0x4CE3, 0x4CE6, 0x4CF8, 0x4CFB, 0x4CFE,
	0x4D01, 0x4D41, 0x4D42, 0x4D43, 0x4D44, 0x4D57, 0x4D58, 0x4D59,
	0x4D5A, 0x4D5B, 0x4D6E, 0x4D6F, 0x4D70, 0x4D71, 0x4D72, 0x4D84,
	0x4D85, 0x4D86, 0x52B5, 0x52B6, 0x52B7, 0x52B8, 0x52B9, 0x52BA,
	0x52BB, 0x52BC, 0x52BD,
};

int Leaves[] = {
	0x4CCE, 0x4CCF, 0x4CD1, 0x4CD2, 0x4CD4, 0x4CD5, 0x4CD7, 0x4CD9,
	0x4CDB, 0x4CDC, 0x4CDE, 0x4CDF, 0x4CE1, 0x4CE2, 0x4CE4, 0x4CE5,
	0x4CE7, 0x4CE8, 0x4CF9, 0x4CFA, 0x4CFC, 0x4CFD, 0x4CFF, 0x4D00,
	0x4D02, 0x4D03, 0x4D45, 0x4D46, 0x4D47, 0x4D48, 0x4D49, 0x4D4A,
	0x4D4B, 0x4D4C, 0x4D4D, 0x4D4E, 0x4D4F, 0x4D50, 0x4D51, 0x4D52,
	0x4D53, 0x4D5C, 0x4D5D, 0x4D5E, 0x4D5F, 0x4D60, 0x4D61, 0x4D62,
	0x4D63, 0x4D64, 0x4D65, 0x4D66, 0x4D67, 0x4D68, 0x4D69, 0x4D73,
	0x4D74, 0x4D75, 0x4D76, 0x4D77, 0x4D78, 0x4D79, 0x4D7A, 0x4D7B,
	0x4D7C, 0x4D7D, 0x4D7E, 0x4D7F, 0x4D87, 0x4D88, 0x4D89, 0x4D8A,
	0x4D8B, 0x4D8C, 0x4D8D, 0x4D8E, 0x4D8F, 0x4D90, 0x4D95, 0x4D96,
	0x4D97, 0x4D99, 0x4D9A, 0x4D9B, 0x4D9D, 0x4D9E, 0x4D9F, 0x4DA1,
	0x4DA2, 0x4DA3, 0x4DA5, 0x4DA6, 0x4DA7, 0x4DA9, 0x4DAA, 0x4DAB,
	0x52BE, 0x52BF, 0x52C0, 0x52C1, 0x52C2, 0x52C3, 0x52C4, 0x52C5,
	0x52C6, 0x52C7
};

HANDLE WINAPI NewCreateFile( LPCSTR lpFileName, DWORD dwDesiredAccess, DWORD dwShareMode,
							 LPSECURITY_ATTRIBUTES lpSecurityAttributes, DWORD dwCreationDisposition,
                             DWORD dwFlagsAndAttributes, HANDLE hTemplateFile )
{
	strlwr( (char*)lpFileName );
	if ( strstr( lpFileName, "artidx.mul" ) && (dwDesiredAccess & GENERIC_READ) != 0 )
	{
		char path[MAX_PATH];
		GetTempPath( MAX_PATH, path );
		strcat( path, "/razor_art.tmp" );
		CopyFile( lpFileName, path, FALSE );

		int stump[3], none[3];
		FILE *f = fopen( path, "rb" );
		fseek( f, 12*0xE57, SEEK_SET );
		fread( stump, 12, 1, f );
		fseek( f, 12*0x115E, SEEK_SET );
		fread( none, 12, 1, f );
		fclose( f );

		f = fopen( path, "wb" );
		for(int i=0;Trees[i] != 0;i++)
		{
			fseek( f, 12*Trees[i], SEEK_SET );
			fwrite( stump, 12, 1, f );
		}
		for(int i=0;Leaves[i] != 0;i++)
		{
			fseek( f, 12*Leaves[i], SEEK_SET );
			fwrite( none, 12, 1, f );
		}
		fclose( f );

		return CreateFile( path, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile );
	}
	else
	{
		return CreateFile( lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile );
	}
}

#define CREATE_FILE_LOC "\x68\x00\x00\x00\xC0\x50\xFF\x15\xF8\x71\x56\x00\x83\xF8\xFF"
#define CFL_LEN 15

void PatchCreateFile()
{
	DWORD CurPos = FindInMemory( 0x00400000, CREATE_FILE_LOC, CFL_LEN );
	if ( !CurPos ) 
		return;
	CurPos += 6;
	*((unsigned char*)(CurPos)) = 0xE8; // call
	*((unsigned int*)(CurPos+1)) = int(NewCreateFile) - (CurPos+4);
	*((unsigned char*)(CurPos+5)) = 0x90; // nop
}

void PatchStartup( DWORD ProcID )
{
	HANDLE hProc = OpenProcess( PROCESS_ALL_ACCESS, FALSE, ProcID );
	if ( !hProc )
		return;

	DWORD image_base = ???; 
    IMAGE_DOS_HEADER *idh = (IMAGE_DOS_HEADER *)image_base;
    IMAGE_FILE_HEADER *ifh = (IMAGE_FILE_HEADER *)(image_base + idh->e_lfanew + sizeof(DWORD));
    IMAGE_OPTIONAL_HEADER *ioh = (IMAGE_OPTIONAL_HEADER *)((DWORD)(ifh) + sizeof(IMAGE_FILE_HEADER));

	DWORD acc = 0;
	void *addr = VirtualAllocEx( hProc, NULL, 512, MEM_COMMIT, PAGE_EXECUTE_READWRITE );
	DWORD Pos = (DWORD)addr;

	char file[256];
	acc = GetModuleFileName( GetModuleHandle(NULL), file, 256 );
	WriteProcessMemory( hProc, (void*)Pos, file, acc, &acc );
	DWORD FileLoc = Pos;
	Pos += acc;
	strcpy( file, "AttachedStart" );
	WriteProcessMemory( hProc, (void*)Pos, file, strlen(file)+1, &acc );
	DWORD ProcLoc = Pos;
	Pos += acc;

	unsigned char LoadFunc[] = {
		0x55,					// push ebp
		0x8B, 0xEC,				// mov ebp, esp
		0x53,					// push ebx
		0x56,					// push esi
		0x57,					// push edi
		0x68, 0, 0, 0, 0,		// push _address_
		0xFF,0x15,0xB8,0x71,0x56,0x00, // call ds:LoadLibraryA
		0x68, 0, 0, 0, 0,		// push _address_
		0x50,					// push eax
		0x8B, 0xF0,				// mov esi, eax
		0xFF,0x15,0xBC,0x71,0x56,0x00, // call ds:GetProcAddress
		0xFF, 0xD0,				// call eax
		//0x56,					// push esi             ; hLibModule
		//0xFF,0x15,0xC0,0x71,0x56,0x00, // call ds:FreeLibrary
		0x5F,					// pop edi
		0x5E,					// pop esi
		0x5B,					// pop ebx
		0x5D,					// pop ebp
		0xE9, 0, 0, 0, 0,		// jmp _addressoffset_
		0x90,					// nop
		0x90,					// nop
	};
	const int len = 44;

	*((int*)(LoadFunc+7)) = FileLoc - (Pos+7+4);
	*((int*)(LoadFunc+18)) = ProcLoc - (Pos+18+4);
	*((int*)(LoadFunc+38)) = (ioh->ImageBase + ioh->AddressOfEntryPoint) - (Pos+38+4);

	WriteProcessMemory( hProc, (void*)Pos, LoadFunc, len, &acc );
	ioh->AddressOfEntryPoint = Pos;
	CloseHandle( hProc );
}
