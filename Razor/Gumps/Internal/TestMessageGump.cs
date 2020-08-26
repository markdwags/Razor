using Assistant.Core.Gumps;

namespace Assistant.Gumps.Internal
{
    public sealed class TestMessageGump : Gump
    {
        public TestMessageGump(string message) : base(500, 250, -1)
        {
            X = 300;
            Y = 200;

            Movable = true;
            Closable = true;
            Disposable = false;

            AddPage(0);
            AddBackground(0, 0, 500, 400, 9270);
            AddHtml(20, 20, 460, 330, message, true, true);
            AddButton(200, 360, 247, 248, 0, GumpButtonType.Reply, 0);
            AddButton(420, 360, 247, 248, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            World.Player.OverheadMessage($"Button {buttonId}");
            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}