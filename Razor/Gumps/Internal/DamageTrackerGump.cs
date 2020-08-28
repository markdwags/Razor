using System.Collections.Generic;
using System.Text;
using Assistant.Core;
using Assistant.Core.Gumps;

namespace Assistant.Gumps.Internal
{
    public sealed class DamageTrackerGump : Gump
    {
        public DamageTrackerGump(string message) : base(100, 100)
        {
            Movable = true;
            Closable = false;
            Disposable = false;

            AddPage(0);
            AddBackground(0, 0, 253, 203, 9270);

            AddLabel(20, 20, 2954, "DPS:");
            AddLabel(60, 20, 67, $"{DamageTracker.DamagePerSecond:N2}");

            AddLabel(120, 20, 2954, "Max DPS:");
            AddLabel(190, 20, 67, $"{DamageTracker.MaxDamagePerSecond:N2}");

            AddHtml(20, 50, 213, 100, message, true, true);

            AddButton(20, 160, 4008, 4010, 1, GumpButtonType.Reply, 0);
            
        }

        public override void OnResponse(int buttonId, int[] switches, GumpTextEntry[] textEntries = null)
        {
            if (buttonId == 1)
            {
                StringBuilder sb = new StringBuilder();

                int x = 1;
                foreach (KeyValuePair<string, int> dmg in DamageTracker.GetTotalDamageList())
                {
                    sb.AppendLine($"{x}) {dmg.Key} [{dmg.Value:N2}]");
                    x++;
                }

                DamageTrackerListGump dmgList = new DamageTrackerListGump(sb.ToString());
                dmgList.SendGump();
            }

            base.OnResponse(buttonId, switches, textEntries);
        }
    }
}