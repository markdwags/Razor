using Assistant.Core;
using Assistant.Core.Gumps;

namespace Assistant.Gumps.Internal
{
    public sealed class DamageTrackerListGump : Gump
    {
        public DamageTrackerListGump(string message) : base(100, 100)
        {
            Movable = true;
            Closable = true;
            Disposable = false;

            AddPage(0);
            AddBackground(0, 0, 273, 318, 9380);
            AddLabel(69, 5, 2954, "Damage Dealt (by name)");
			AddHtml(30, 40, 213, 239, message, true, true);
        }
    }
}