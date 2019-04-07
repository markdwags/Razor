using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistant
{
    public class OSIClientCommunication : ClientCommunication
    {

        internal override Version GetUOVersion()
        {
            Version result;
            string[] split = NativeMethods.GetUOVersion().Split( '.' );

            if ( split.Length < 3 )
                result = new Version( 4, 0, 0, 0 );

            int rev = 0;

            if ( split.Length > 3 )
                rev = Utility.ToInt32( split[3], 0 );

            result = new Version(
                Utility.ToInt32( split[0], 0 ),
                Utility.ToInt32( split[1], 0 ),
                Utility.ToInt32( split[2], 0 ),
                rev );

            if ( result == null || result.Major == 0 ) // sanity check if the client returns 0.0.0.0
                result = new Version( 4, 0, 0, 0 );
            return result;
        }
    }
}
