#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistant.UI;

namespace Assistant
{
    public class ScreenCapManager
    {
        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr DeleteObject(IntPtr hGdiObj);

        private static TimerCallback m_DoCaptureCall = new TimerCallback(CaptureNow);

        public static string LastMobileDeathName { get; set; }

        public static void Initialize()
        {
            HotKey.Add(HKCategory.Misc, LocString.TakeSS, new HotKeyCallback(CaptureNow));
        }

        public static void DeathCapture(double delay)
        {
            Timer.DelayedCallback(TimeSpan.FromSeconds(delay), m_DoCaptureCall).Start();
        }

        public static void CaptureNow()
        {
            string filename;
            string playerName = "Unknown";
            string path = Config.GetString("CapPath");
            string type = Config.GetString("ImageFormat").ToLower();

            if (World.Player != null)
                playerName = World.Player.Name;

            if (playerName == null || playerName.Trim() == "" ||
                playerName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                playerName = "Unknown";

            if (LastMobileDeathName == null || LastMobileDeathName.Trim() == "" ||
                LastMobileDeathName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                LastMobileDeathName = "Unknown";

            string imageTimestampTag = Config.GetBool("CapTimeStamp")
                ? $"{playerName} ({World.ShardName}) - {Engine.MistedDateTime:M/dd/yy - HH:mm:ss}"
                : "";

            playerName = !string.IsNullOrEmpty(LastMobileDeathName)
                ? $"{playerName}_{LastMobileDeathName}_{Engine.MistedDateTime:M-d_HH.mm}"
                : $"{playerName}_{Engine.MistedDateTime:M-d_HH.mm}";

            try
            {
                Engine.EnsureDirectory(path);
            }
            catch
            {
                try
                {
                    path = Config.GetUserDirectory("ScreenShots");
                    Config.SetProperty("CapPath", path);
                }
                catch
                {
                    path = "";
                }
            }

            int count = 0;
            do
            {
                filename = Path.Combine(path,
                    $"{playerName}{(count != 0 ? count.ToString() : "")}.{type}");
                count--; // cause a - to be put in front of the number
            } while (File.Exists(filename));

            try
            {
                IntPtr hBmp = Platform.CaptureScreen(Client.Instance.GetWindowHandle(), Config.GetBool("CapFullScreen"),
                    imageTimestampTag);
                using (Image img = Image.FromHbitmap(hBmp))
                    img.Save(filename, GetFormat(type));
                DeleteObject(hBmp);
            }
            catch
            {
                // ignored
            }

            LastMobileDeathName = string.Empty;

            Engine.MainWindow.SafeAction(s => s.ReloadScreenShotsList());
        }

        private static ImageFormat GetFormat(string fmt)
        {
            //string fmt = Config.GetString( "ImageFormat" ).ToLower();
            if (fmt == "jpeg" || fmt == "jpg")
                return ImageFormat.Jpeg;
            else if (fmt == "png")
                return ImageFormat.Png;
            else if (fmt == "bmp")
                return ImageFormat.Bmp;
            else if (fmt == "gif")
                return ImageFormat.Gif;
            else if (fmt == "tiff" || fmt == "tif")
                return ImageFormat.Tiff;
            else if (fmt == "wmf")
                return ImageFormat.Wmf;
            else if (fmt == "exif")
                return ImageFormat.Exif;
            else if (fmt == "emf")
                return ImageFormat.Emf;
            else
                return ImageFormat.Jpeg;
        }

        public static void DisplayTo(ListBox list)
        {
            string path = Config.GetString("CapPath");
            Engine.EnsureDirectory(path);

            //list.BeginUpdate();
            list.Items.Clear();

            AddFiles(list, path, "jpeg");
            AddFiles(list, path, "jpg");
            AddFiles(list, path, "png");
            AddFiles(list, path, "bmp");
            AddFiles(list, path, "gif");
            AddFiles(list, path, "tiff");
            AddFiles(list, path, "tif");
            AddFiles(list, path, "wmf");
            AddFiles(list, path, "exif");
            AddFiles(list, path, "emf");
            //list.EndUpdate();
        }

        public static void AddFiles(ListBox list, string path, string ext)
        {
            if (list.Items.Count >= 500)
                return;

            string[] files = Directory.GetFiles(path, $"*.{ext}");
            for (int i = 0; i < files.Length && list.Items.Count < 500; i++)
                list.Items.Add(Path.GetFileName(files[i]));
        }


        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
    }
}