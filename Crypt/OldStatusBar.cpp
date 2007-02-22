#include "StdAfx.h"
#include "Crypt.h"
#include "MemFinder.h"

DWORD GumpOutV28( DWORD OutFuncAddr, int esiOff, char TwoFourOff, DWORD CurAddr, int x, int y )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x8D\x44\x24\x20", 4 ); // lea    eax, [esp+70h+var_50]
	count += 4;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x8B\x46", 2 ); // mov    eax, [esi+24h]
	count += 2;
	*((unsigned char*)(CurAddr+count)) = 0x24+TwoFourOff;//+esiOff;
	count ++;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x8B\x56", 2 ); // mov     edx, [esi+44h]
	count += 2;
	*((unsigned char*)(CurAddr+count)) = 0x44+esiOff;
	count ++;
	memcpy( (void*)(CurAddr+count), "\x8B\x42\x14", 3 ); // mov     eax, [edx+14h]
	count += 3;
	memcpy( (void*)(CurAddr+count), "\x83\xC0\x04", 3 ); // add     eax, 4
	count += 3;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x8D\x44\x24\x54", 4 ); // lea    eax, [esp+7Ch+var_28]
	count += 4;
	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x68\x86\x03\x00\x00", 5 ); // push    386h
	count += 5;

	memcpy( (void*)(CurAddr+count), "\x6A\x01", 2 ); // push    1
	count += 2;

	// push    y
	if ( y < 0x80 )
	{
		*((unsigned char*)(CurAddr+count)) = 0x6A;
		count ++;
		*((unsigned char*)(CurAddr+count)) = (unsigned char)y;
		count ++;
	}
	else
	{
		*((unsigned char*)(CurAddr+count)) = 0x68;
		count ++;
		*((int*)(CurAddr+count)) = y;
		count += 4;
	}

	// push    x
	if ( x < 0x80 )
	{
		*((unsigned char*)(CurAddr+count)) = 0x6A;
		count ++;
		*((unsigned char*)(CurAddr+count)) = (unsigned char)x;
		count ++;
	}
	else
	{
		*((unsigned char*)(CurAddr+count)) = 0x68;
		count ++;
		*((int*)(CurAddr+count)) = x;
		count += 4;
	}

	// call theFunction
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = OutFuncAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x20", 3 ); // add    esp, 20h
	count += 3;

	return count;
}

DWORD Sprintf1UV28( DWORD sprintfAddr, DWORD fmtAddr, DWORD CurAddr, int esiOffset )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x33\xC9", 2 ); // xor     ecx, ecx
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8D\x54\x24\x48", 4 ); // lea     edx, [esp+70h+var_28]
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
	count += 3;
	*((int*)(CurAddr+count)) = esiOffset;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x51", 1 ); // push    ecx
	count ++;

	// push offset fmtAddr
	*((unsigned char*)(CurAddr+count)) = 0x68;
	count ++;
	*((int*)(CurAddr+count)) = fmtAddr;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x52", 1 ); // push    edx
	count ++;

	// call    _sprintf
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = sprintfAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x0C", 3 ); // add    esp, 0Ch
	count += 3;

	return count;
}

DWORD Sprintf2UV28_Byte( DWORD sprintfAddr, DWORD fmtAddr, DWORD CurAddr, int offset1, int offset2 )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x33\xC0", 2 ); // xor     eax, eax
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8A\x86", 2 ); // mov     al, [esi+___]
	count += 2;
	*((int*)(CurAddr+count)) = offset2;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x33\xC9", 2 ); // xor     ecx, ecx
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8D\x54\x24\x4C", 4 ); // lea     edx, [esp+70h+var_28]
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x8A\x8E", 2 ); // mov     cl, [esi+___]
	count += 2;
	*((int*)(CurAddr+count)) = offset1;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x51", 1 ); // push    ecx
	count ++;

	// push offset fmtAddr
	*((unsigned char*)(CurAddr+count)) = 0x68;
	count ++;
	*((int*)(CurAddr+count)) = fmtAddr;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x52", 1 ); // push    edx
	count ++;

	// call    _sprintf
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = sprintfAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x10", 3 ); // add    esp, 10h
	count += 3;

	return count;
}

DWORD Sprintf2UV28( DWORD sprintfAddr, DWORD fmtAddr, DWORD CurAddr, int offset1, int offset2 )
{
	int count = 0;

	memcpy( (void*)(CurAddr+count), "\x33\xC0", 2 ); // xor     eax, eax
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x66\x8B\x86", 3 ); // mov     ax, [esi+___]
	count += 3;
	*((int*)(CurAddr+count)) = offset2;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x50", 1 ); // push    eax
	count ++;

	memcpy( (void*)(CurAddr+count), "\x33\xC9", 2 ); // xor     ecx, ecx
	count += 2;

	memcpy( (void*)(CurAddr+count), "\x8D\x54\x24\x4C", 4 ); // lea     edx, [esp+70h+var_28]
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
	count += 3;
	*((int*)(CurAddr+count)) = offset1;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x51", 1 ); // push    ecx
	count ++;

	// push offset fmtAddr
	*((unsigned char*)(CurAddr+count)) = 0x68;
	count ++;
	*((int*)(CurAddr+count)) = fmtAddr;
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x52", 1 ); // push    edx
	count ++;

	// call    _sprintf
	*((unsigned char*)(CurAddr+count)) = 0xE8;
	count ++;
	*((int*)(CurAddr+count)) = sprintfAddr - (CurAddr+count+4);
	count += 4;

	memcpy( (void*)(CurAddr+count), "\x83\xC4\x10", 3 ); // add    esp, 10h
	count += 3;

	return count;
}

/*#define STATCODE_BEG "\x8D\x54\x24\x48\x8B\xC3\x2B\xD3\x8A\x08\x88\x0C\x02\x40\x84\xC9\x75\xF6"
#define STATCODE_B_LEN 18
#define STATCODE_START_OFF (STATCODE_B_LEN+0x15)
#define STACK_OFFSET -3
#define STACK_BASE 0x28
*/
#define STATCODE_BEG "\x68\x86\x03\x00\x00\x6A\x01\x6A\x32\x6A\x28\xE8"
#define STATCODE_B_LEN 12
#define STATCODE_START_OFF (-0x1E)
#define STACK_OFFSET 6
#define STACK_BASE 0x28

#define STATCODE_END "\x51\x52\x68\x86\x03\x00\x00\x6A\x01\x68\x8F\x00\x00\x00\x00"
#define STATCODE_E_LEN 14
#define STATCODE_END_OFFSET 9

#define GUMP_STRING1 "\x53\x55\x8B\x6C\x24\x1C\x56\x57\x8B\xFD\x83\xC9\xFF\x33\xC0\xF2"
#define GS1_LEN 16
#define GS1_OFFSET 0x29

#define GID_REPL "\x6C\x2A\x00\x00"
#define GID_R_LEN 4

#define SPRINTF_STR "\x55\x8B\xEC\x83\xEC\x20\x8B\x45\x08\x56\x89\x45\xE8\x89\x45\xE0\x8D\x45\x10\xC7\x45\xEC\x42\x00\x00\x00\x50\x8D\x45\xE0\xFF\x75\x0C\xC7\x45\xE4\xFF\xFF\xFF\x7F\x50\xE8"
#define SPRINTF_LEN 42

#define LOCK_STR_1 "\x6A\x4E\x6A\x28"
#define LOCK_STR_2 "\x6A\x69\x6A\x28"
#define LOCK_STR_3 "\x68\x85\x00\x00\x00\x6A\x28"
#define LOCK_LEN_1 4
#define LOCK_LEN_2 4
#define LOCK_LEN_3 7

#define MIN_STR "\x68\xA8\x00\x00\x00\x68\x98\x01\x00\x00\x68\x92\x00\x00\x00\x68\x81\x01\x00\x00"
#define MIN_STR_LEN 20

vector<PatchInfo *> *PatchVec = NULL;
bool PatchStatusBar( BOOL preAOS )
{
	if ( !preAOS )
	{
		if ( PatchVec == NULL )
			return true;

		for(unsigned int i=0;i<PatchVec->size();i++)
		{
			PatchInfo *patch = (*PatchVec)[i];
			MemoryPatch( patch->Address, patch->Data, patch->Length );
			delete patch;
		}
		delete PatchVec;
		PatchVec = NULL;
		return true;
	}

	if ( PatchVec != NULL )
		return true;

	const unsigned int NewGumpID = 0x00000802;

	MemFinder mf;

	mf.AddEntry( GUMP_STRING1, GS1_LEN );
	mf.AddEntry( "%u/%u", 6 );
	mf.AddEntry( MIN_STR, MIN_STR_LEN );
	mf.AddEntry( STATCODE_BEG, STATCODE_B_LEN );
	mf.AddEntry( SPRINTF_STR, SPRINTF_LEN );
	mf.AddEntry( LOCK_STR_1, LOCK_LEN_1 );
	mf.AddEntry( GID_REPL, GID_R_LEN );

	mf.Execute();

	DWORD GumpString1 = mf.GetAddress( GUMP_STRING1, GS1_LEN );
	if ( !GumpString1 ) 
		return false;
	GumpString1 -= GS1_OFFSET;

	DWORD uuFmt = mf.GetAddress( "%u/%u", 6 );
	if ( !uuFmt ) 
		return false;
	DWORD uFmt = uuFmt + 3;

	DWORD MinLoc = mf.GetAddress( MIN_STR, MIN_STR_LEN );
	if ( !MinLoc ) 
		return false;
	MinLoc += 1;

	DWORD Start = mf.GetAddress( STATCODE_BEG, STATCODE_B_LEN );
	if ( !Start ) 
		return false;
	Start += STATCODE_START_OFF;

	DWORD End = Start;

	do {
		End = MemFinder::Find( STATCODE_END, STATCODE_E_LEN, End + 1 );
	} while ( End && *((unsigned char*)(End+STATCODE_E_LEN+1)) != 0xE8 );

	if ( !End )
		return false;

	End += STATCODE_E_LEN + STATCODE_END_OFFSET;

	DWORD sprintfAddr = mf.GetAddress( SPRINTF_STR, SPRINTF_LEN );
	DWORD LockLoc = mf.GetAddress( LOCK_STR_1, LOCK_LEN_1 );
	DWORD CurPos = mf.GetAddress( GID_REPL, GID_R_LEN );

	if ( !( sprintfAddr && LockLoc && CurPos ) )
		return false;

	PatchVec = new vector<PatchInfo*>();

	PatchVec->push_back( new PatchInfo( CurPos, 4 ) );
	MemoryPatch( CurPos, NewGumpID );
	CurPos = MemFinder::Find( GID_REPL, GID_R_LEN, CurPos+4 );
	if ( CurPos )
	{
		PatchVec->push_back( new PatchInfo( CurPos, 4 ) );
		MemoryPatch( CurPos, NewGumpID );
	}

	PatchVec->push_back( new PatchInfo( LockLoc+0x01, 1 ) );
	PatchVec->push_back( new PatchInfo( LockLoc+0x03, 1 ) );
	MemoryPatch( LockLoc+0x01, 0x3E, 1 );
	MemoryPatch( LockLoc+0x03, 0x26, 1 );

	LockLoc = MemFinder::Find( LOCK_STR_2, LOCK_LEN_2, LockLoc );
	if ( LockLoc )
	{
		PatchVec->push_back( new PatchInfo( LockLoc+0x01, 1 ) );
		PatchVec->push_back( new PatchInfo( LockLoc+0x03, 1 ) );
		MemoryPatch( LockLoc+0x01, 0x4A, 1 );
		MemoryPatch( LockLoc+0x03, 0x26, 1 );

		LockLoc = MemFinder::Find( LOCK_STR_3, LOCK_LEN_3, LockLoc );
		if ( LockLoc )
		{
			PatchVec->push_back( new PatchInfo( LockLoc+0x01, 4 ) );
			PatchVec->push_back( new PatchInfo( LockLoc+0x06, 1 ) );
			MemoryPatch( LockLoc+0x01, 0x56, 4 );
			MemoryPatch( LockLoc+0x06, 0x26, 1 );
		}
	}

	PatchVec->push_back( new PatchInfo( MinLoc+ 0, 4 ) );
	PatchVec->push_back( new PatchInfo( MinLoc+ 5, 4 ) );
	PatchVec->push_back( new PatchInfo( MinLoc+10, 4 ) );
	PatchVec->push_back( new PatchInfo( MinLoc+15, 4 ) );
	MemoryPatch( MinLoc+ 0, 137 );
	MemoryPatch( MinLoc+ 5, 263 );
	MemoryPatch( MinLoc+10, 111 );
	MemoryPatch( MinLoc+15, 239 );

	int len = End - Start;

	PatchVec->push_back( new PatchInfo( Start, len ) );

	DWORD OldProtect;
	if ( !VirtualProtect( (void *)(Start), len, PAGE_EXECUTE_READWRITE, &OldProtect ) )
		return false;

	CurPos = Start;

	int statOffset = *((unsigned char*)(Start + 0x31 + 0x18 + 3));
	int esiOffset = statOffset - 0xD4;
	int stackChange = STACK_BASE - *((unsigned char*)(Start + STACK_OFFSET));
	int twoFourOff = *((unsigned char*)(Start + 0x31 + 2)) - 0x24;

	bool client5 = false;
	char uuPush[5];
	uuPush[0] = 0x68;
	*((DWORD*)&uuPush[1]) = uuFmt;

	DWORD uuSP = MemFinder::Find( uuPush, 5, Start );
	if ( uuSP )
		client5 = ( *((int*)(uuSP - 10)) != (statOffset + 0x1F) );

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = (char)stackChange;
		CurPos++;
	}

	if ( *((unsigned char*)(Start + 11)) != 0x46 ) // was reg is it in?
	{
		memcpy( (void*)CurPos, "\x8B\xF5", 2 ); // mov     esi, ebp
		CurPos += 2;
	}

	// name
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 53 );

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = -stackChange;
		CurPos++;
	}

	memset( (void*)CurPos, 0x90, (Start+0x31) - CurPos );

	CurPos = Start + 0x31 + 0x12;

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = (char)stackChange;
		CurPos++;
	}

	if ( *((unsigned char*)(Start + 11)) != 0x46 ) // was reg is it in?
	{
		memcpy( (void*)CurPos, "\x8B\xF5", 2 ); // mov     esi, ebp
		CurPos += 2;
	}

	// str
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x00 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 72 );

	// dex
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x02 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 84 );

	// int
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x04 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 96 );

	// sex?
	//memcpy( (void*)(CurPos), "\xC7\x44\x24\x48Yes", 8 ); // mov     [esp+70h+var_28], __
	//CurPos += 8;
	//CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 108 );

	// followers:
	if ( client5 )
	{
		CurPos += Sprintf2UV28_Byte( sprintfAddr, uuFmt, CurPos, statOffset+2+0x1E, statOffset+2+0x1F );
		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 108 );
	}
	else
	{
		CurPos += Sprintf2UV28_Byte( sprintfAddr, uuFmt, CurPos, statOffset+0x1E, statOffset+0x1F );
		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 108 );
	}

	// AR
	if ( client5 )
		CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x1C );
	else
		CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x1A );

	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 86, 120 );

	// hits
	CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x06, statOffset+0x08 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 72 );

	// stam
	CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x0A, statOffset+0x0C );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 96 );

	// mana
	CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x0E, statOffset+0x10 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 84 );

	// gold
	CurPos += Sprintf1UV28( sprintfAddr, uFmt, CurPos, statOffset+0x14 );
	CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 108 );

	if ( client5 )
	{
		CurPos += Sprintf2UV28( sprintfAddr, uuFmt, CurPos, statOffset+0x18, statOffset+0x1A );
		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 120 );
	}
	else
	{
		// Weight:
		memcpy( (void*)CurPos, "\x33\xC9", 2 ); // xor     ecx, ecx
		CurPos += 2;
		memcpy( (void*)CurPos, "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
		CurPos += 3;
		*((int*)CurPos) = statOffset+0x00; // str
		CurPos += 4;
		memcpy( (void*)CurPos, "\x8D\x04\xCD\x00\x00\x00\x00\x2B\xC1\x99\x2B\xC2\xD1\xF8\x83\xC0\x28", 17 ); // eax = ecx*3.5 + 40
		CurPos += 17;
		memcpy( (void*)CurPos, "\x50", 1 ); // push    eax
		CurPos ++;

		memcpy( (void*)CurPos, "\x33\xC9", 2 ); // xor     ecx, ecx
		CurPos += 2;
		memcpy( (void*)CurPos, "\x8D\x54\x24\x4C", 4 ); // lea     edx, [esp+74h+var_28]
		CurPos += 4;
		memcpy( (void*)CurPos, "\x66\x8B\x8E", 3 ); // mov     cx, [esi+___]
		CurPos += 3;
		*((int*)CurPos) = statOffset+0x18; // weight
		CurPos += 4;
		memcpy( (void*)CurPos, "\x51", 1 ); // push    ecx
		CurPos ++;

		// push offset fmtAddr
		*((unsigned char*)CurPos) = 0x68;
		CurPos ++;
		*((int*)CurPos) = uuFmt;
		CurPos += 4;

		memcpy( (void*)CurPos, "\x52", 1 ); // push    edx
		CurPos ++;

		// call    _sprintf
		*((unsigned char*)CurPos) = 0xE8;
		CurPos ++;
		*((int*)CurPos) = sprintfAddr - (CurPos+4);
		CurPos += 4;

		memcpy( (void*)CurPos, "\x83\xC4\x10", 3 ); // add    esp, 10h
		CurPos += 3;

		CurPos += GumpOutV28( GumpString1, esiOffset, twoFourOff, CurPos, 174, 120 );
	}

	if ( stackChange != 0  )
	{
		memcpy( (void*)CurPos, "\x83\xC4", 2 ); // add    esp, __h
		CurPos += 2;
		*((char*)CurPos) = -stackChange;
		CurPos++;
	}

	memset( (void*)CurPos, 0x90, End-CurPos );

	VirtualProtect( (void *)(Start), len, OldProtect, &OldProtect );

	return true;
}

