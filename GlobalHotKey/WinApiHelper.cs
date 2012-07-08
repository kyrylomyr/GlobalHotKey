using System;
using System.Runtime.InteropServices;

namespace GlobalHotKey
{
    internal class WinApiHelper
    {
        /// <summary>
        /// Registers a system-wide hot key.
        /// </summary>
        /// <param name="handle">The handle of the window that will process hot key messages.</param>
        /// <param name="id">The hot key ID.</param>
        /// <param name="modifiers">The hot key modifiers.</param>
        /// <param name="virtualCode">The hot key virtual code.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr handle, int id, uint modifiers, uint virtualCode);

        /// <summary>
        /// Unregisters previously registered system-wide hot key.
        /// </summary>
        /// <param name="handle">The window handle.</param>
        /// <param name="id">The hot key ID.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr handle, int id);

        /// <summary>
        /// The message code posted when the user presses a hot key.
        /// </summary>
        public static int WmHotKey = 0x0312;
    }
}
