using System;
using System.Collections.Generic;
using Assistant.Core.Gumps;
using Ultima;

namespace Assistant.Gumps
{
    public sealed class TestMessageGump : Gump
    {
        //private readonly Version _version;

        public TestMessageGump(string message ) : base( 500, 250, -1 )
        {
            X = 300;
            Y = 200;

            //_version = version;
            Closable = true;
            Disposable = false;

            AddPage( 0 );
            AddBackground( 0, 0, 500, 400, 9270 );
            AddHtml( 20, 20, 460, 330, message, true, true );
            AddButton( 420, 360, 247, 248, 0, GumpButtonType.Reply, 0 );
        }

        public override void OnResponse( int buttonID, int[] switches, Dictionary<int, string> textEntries = null )
        {
            //AssistantOptions.UpdateGumpVersion = _version;
            base.OnResponse( buttonID, switches, textEntries );
        }
    }
}