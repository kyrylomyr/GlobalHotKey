using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace GlobalHotKey
{
    /// <summary>
    /// Provides an ability to setup system-wide hot keys and react on their events.
    /// </summary>
    public class GlobalHotKey : IDisposable
    {
        private readonly HwndSource _windowHandleSource;

        private readonly HashSet<HotKey> _registered;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalHotKey"/> class.
        /// </summary>
        public GlobalHotKey()
        {
            _windowHandleSource = new HwndSource(new HwndSourceParameters());
            _windowHandleSource.AddHook(messagesHandler);

            _registered = new HashSet<HotKey>();
        }

        /// <summary>
        /// Registers the system-wide hot key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        /// <returns>The registered <see cref="HotKey"/> with system ID.</returns>
        public HotKey RegisterHotKey(Keys key, ModifierKeys modifiers)
        {
            // Check if specified hot key is already registered.
            var hotKey = new HotKey(key, modifiers);
            if (_registered.Contains(hotKey))
                throw new ArgumentException("The specified hot key is already registered.");

            // Register new hot key.
            var id = getFreeKeyId();
            if (!WinApiHelper.RegisterHotKey(_windowHandleSource.Handle, id, (uint)modifiers, (uint)key))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Can't register the hot key.");

            hotKey.Id = id;
            _registered.Add(hotKey);
            
            return hotKey;
        }

        private int getFreeKeyId()
        {
            return _registered.Any() ? _registered.Max(it => it.Id) + 1 : 0;
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
            // Unregister hot keys.
            foreach (var hotKey in _registered)
            {
                WinApiHelper.UnregisterHotKey(_windowHandleSource.Handle, hotKey.Id);
            }

            _windowHandleSource.RemoveHook(messagesHandler);
            _windowHandleSource.Dispose();
        }
    }
}
