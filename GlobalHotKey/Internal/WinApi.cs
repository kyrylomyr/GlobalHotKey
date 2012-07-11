using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace GlobalHotKey.Internal
{
    /// <summary>
    /// Contains imported Win32 API functions, constants and helper methods.
    /// </summary>
    internal class WinApi
    {
        /// <summary>
        /// Registers a system-wide hot key.
        /// </summary>
        /// <param name="handle">The handle of the window that will process hot key messages.</param>
        /// <param name="id">The hot key ID.</param>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        /// <returns><c>true</c> if hot key was registered; otherwise, <c>false</c>.</returns>
        public static bool RegisterHotKey(IntPtr handle, int id, Key key, ModifierKeys modifiers)
        {
            var virtualCode = KeyInterop.VirtualKeyFromKey(key);
            return RegisterHotKey(handle, id, (uint)modifiers, (uint)virtualCode);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr handle, int id, uint modifiers, uint virtualCode);

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
