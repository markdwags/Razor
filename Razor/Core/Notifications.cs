using System.Windows.Forms;

namespace Assistant.Core
{
    public class Notifications
    {
        public static void SendError(string caption, string text)
        {
            MessageBox.Show(Engine.ActiveWindow, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void SendWarning(string caption, string text)
        {
            MessageBox.Show(Engine.ActiveWindow, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
