using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace GlobalHotKey
{
    /// <summary>
    /// Provides an ability to setup system-wide hot keys and react on their events.
    /// </summary>
    public class GlobalHotKey : IDisposable
    {
        // Registers a system-wide hot key.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr handle, int id, uint modifiers, uint virtualCode);

        // Unregisters previously registered system-wide hot key.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr handle, int id);

        private static int WM_HOTKEY = 0x0312;

        private readonly HwndSource _windowHandleSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalHotKey"/> class.
        /// </summary>
        public GlobalHotKey()
        {
            _windowHandleSource = new HwndSource(new HwndSourceParameters());
            _windowHandleSource.AddHook(messagesHandler);
        }

        private IntPtr messagesHandler(IntPtr handle, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _windowHandleSource.RemoveHook(messagesHandler);
        }
    }
}
