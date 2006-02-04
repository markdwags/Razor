#include "stdafx.h"
#include "Crypt.h"

#include "PacketInfo.h"

unsigned short *PacketTable = NULL;

DLLFUNCTION int GetPacketLength( unsigned char *packet, int len )
{
	if ( PacketTable != NULL )
	{
		int packetLen = PacketTable[(unsigned char)packet[0]];
		if ( packetLen >= 0x8000 )
		{
			if ( len < 3 )
				return 0;
			else
				return packet[1]<<8 | packet[2];
		}
		else
		{
			return packetLen;
		}
	}
	else if ( len >= 3 )
	{
		//attempt to auto detect an undefined packet
		return packet[1]<<8 | packet[2];
	}
	else
	{
		return -1;
	}
}

DLLFUNCTION bool IsDynLength( BYTE packet )
{
	if ( PacketTable != NULL )
		return PacketTable[packet] >= 0x8000;
	else
		return true;
}
