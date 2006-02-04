#pragma once

#include "TwoFish.h"

class OSIEncryption
{
public:
	OSIEncryption();

	void Initialize( DWORD dwSeed );

	void DecryptFromServer( const BYTE *in, BYTE *out, int len );
	void DecryptFromClient( const BYTE *in, BYTE *out, int len );

	void EncryptForServer( const BYTE *in, BYTE *out, int len );
	void EncryptForClient( const BYTE *in, BYTE *out, int len );

	static void MD5( const BYTE *input, int inLen, BYTE *output );

private:
	void TwoFishCrypt( const BYTE *in, BYTE *out, int len );
	void ReinitTFTable();

	void XORCrypt( const BYTE *in, BYTE *out, int len );
	void InitializeXORTable( const BYTE* data, int dataLen );
	static void Hash( BYTE *field, const BYTE *param );
	static void CallHash( BYTE *key, const BYTE *challenge, unsigned int len );
	static void CalcResponse( BYTE *result, BYTE *field );

	keyInstance m_Key; 
	cipherInstance m_Cipher; 
	unsigned int m_TFPos;
	unsigned char m_XORPos;
	BYTE m_TFTable[256];
	BYTE m_XORTable[16];
};

