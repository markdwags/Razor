#pragma once

class LoginEncryption
{
public:
	LoginEncryption();

	void Initialize( const BYTE *pSeed );

	static void SetKeys( const BYTE *sk1, const BYTE *sk2, const BYTE *dk1 );

	void Encrypt( const BYTE *in, BYTE *out, int len );
	void Decrypt( const BYTE *in, BYTE *out, int len );

	BYTE Test( BYTE );

	static DWORD StaticKey1, StaticKey2;
	static DWORD DynamicKey1;

private:
	BYTE Crypt( BYTE );

	DWORD m_Table[2];
};

