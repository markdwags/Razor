using System.Windows.Forms;

namespace Assistant.Core
{
    public class Clipboard
    {
        public static void SetText(string text)
        {
            System.Windows.Forms.Clipboard.SetText(text);
        }

        public static string GetText()
        {
            return System.Windows.Forms.Clipboard.GetText();
        }

        public static void SetDataObject(object data, bool copy)
        {
            System.Windows.Forms.Clipboard.SetDataObject(data, copy);
        }

        public static void Clear()
        {
            System.Windows.Forms.Clipboard.Clear();
        }
    }
}
