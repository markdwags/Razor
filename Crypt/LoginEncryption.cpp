#include "stdafx.h"

#include "Crypt.h"
#include "LoginEncryption.h"

#define N2L(C, LL) \
    LL  = ((unsigned int)(*((C)++))) << 24, \
    LL |= ((unsigned int)(*((C)++))) << 16, \
    LL |= ((unsigned int)(*((C)++))) << 8, \
    LL |= ((unsigned int)(*((C)++)))

DWORD LoginEncryption::StaticKey1 = 0;
DWORD LoginEncryption::StaticKey2 = 0;
DWORD LoginEncryption::DynamicKey1 = 0;

LoginEncryption::LoginEncryption()
{
	m_Table[0] = m_Table[1] = 0;
}

void LoginEncryption::SetKeys( const BYTE *sk1, const BYTE *sk2, const BYTE *dk1 )
{
	LoginEncryption::StaticKey1  = *((DWORD*)sk1);
	LoginEncryption::StaticKey2  = *((DWORD*)sk2);
	LoginEncryption::DynamicKey1 = *((DWORD*)dk1);
}

void LoginEncryption::Initialize( const BYTE *pSeed )
{
	DWORD seed;
    N2L(pSeed, seed);

    m_Table[0] = (((~seed) ^ 0x00001357) << 16) | ((seed ^ 0xffffaaaa) & 0x0000ffff);
    m_Table[1] = ((seed ^ 0x43210000) >> 16) | (((~seed) ^ 0xabcdffff) & 0xffff0000);
}

void LoginEncryption::Encrypt( const BYTE *in, BYTE *out, int len )
{
	for (int i=0;i<len;i++)
		out[i] = Crypt( in[i] );
}

void LoginEncryption::Decrypt( const BYTE *in, BYTE *out, int len )
{
	for (int i=0;i<len;i++)
		out[i] = Crypt( in[i] );
}

BYTE LoginEncryption::Crypt( BYTE in )
{
	//m_Table[1] = (((((pt1 >> 1) | (pt0 << 31)) ^ LOGIN_KEY_1) >> 1) | (pt0<<31)) ^ LOGIN_KEY_1;
	//m_Table[0] = ((pt0 >> 1) | (pt1 << 31)) ^ LOGIN_KEY_2;
	
	BYTE out = in ^ ((unsigned char)m_Table[0]);
	
	DWORD OldT1 = m_Table[1];
	m_Table[1] = ((((m_Table[1] >> 1) | (m_Table[0] << 31)) ^ StaticKey1) >> 1 | (m_Table[0] << 31)) ^ StaticKey2;
	m_Table[0] = ((m_Table[0]>>1) | (OldT1<<31)) ^ DynamicKey1;
	
	return out;
}

BYTE LoginEncryption::Test( BYTE in )
{
	return in ^ ((unsigned char)m_Table[0]);
}


