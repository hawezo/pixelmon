using System;
using System.Runtime.InteropServices;

namespace Pixelmon
{
    public static class WindowsApi
    {
        /// <summary>
        /// The FindWindow API
        /// </summary>
        /// <param name="lpClassName">the class name for the window to search for</param>
        /// <param name="lpWindowName">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        /// <summary>
        /// Sets the window to the foreground.
        /// </summary>
        /// <param name="hWnd">a handle to the parent window</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

    }
}
