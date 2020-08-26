using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assistant.Core.Gumps;
using Ultima;

namespace Assistant.Gumps
{
    public sealed class TestMessageGump : Gump
    {
        public TestMessageGump(string message ) : base( 500, 250, -1 )
        {
            X = 300;
            Y = 200;

            Movable = true;
            Closable = true;
            Disposable = false;

            AddPage( 0 );
            AddBackground( 0, 0, 500, 400, 9270 );
            AddHtml( 20, 20, 460, 330, message, true, true );
            AddButton( 420, 360, 247, 248, 0, GumpButtonType.Reply, 0 );
        }

        public override void OnResponse( int buttonID, int[] switches, Dictionary<int, string> textEntries = null )
        {
            Debug.WriteLine(buttonID);
            base.OnResponse( buttonID, switches, textEntries );
        }
    }
}