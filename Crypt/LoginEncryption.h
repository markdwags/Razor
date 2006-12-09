#pragma once

class LoginEncryption
{
public:
	LoginEncryption();

	void Initialize( const BYTE *pSeed );

	static void SetKeys( const DWORD *k1, const DWORD *k2 );

	void Encrypt( const BYTE *in, BYTE *out, int len );
	void Decrypt( const BYTE *in, BYTE *out, int len );

	BYTE Test( BYTE );

	static DWORD GenerateBadSeed( DWORD oldSeed );

private:
	static const DWORD *Key1, *Key2;

	BYTE Crypt( BYTE );

	DWORD m_Table[2];
};

