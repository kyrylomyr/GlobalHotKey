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
    public class HotKeyManager : IDisposable
    {
        private readonly HwndSource _windowHandleSource;

        private readonly Dictionary<HotKey, int> _registered;

        /// <summary>
        /// Occurs when registered system-wide hot key is pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyManager"/> class.
        /// </summary>
        public HotKeyManager()
        {
            _windowHandleSource = new HwndSource(new HwndSourceParameters());
            _windowHandleSource.AddHook(messagesHandler);

            _registered = new Dictionary<HotKey, int>();
        }

        /// <summary>
        /// Registers the system-wide hot key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        /// <returns>The registered <see cref="HotKey"/>.</returns>
        public HotKey RegisterHotKey(Keys key, ModifierKeys modifiers)
        {
            return RegisterHotKey(new HotKey(key, modifiers));
        }

        /// <summary>
        /// Registers the system-wide hot key.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        /// <returns>The registered <see cref="HotKey"/>.</returns>
        public HotKey RegisterHotKey(HotKey hotKey)
        {
            // Check if specified hot key is already registered.
            if (_registered.ContainsKey(hotKey))
                throw new ArgumentException("The specified hot key is already registered.");

            // Register new hot key.
            var id = getFreeKeyId();
            if (!WinApiHelper.RegisterHotKey(_windowHandleSource.Handle, id, (uint)hotKey.Modifiers, (uint)hotKey.Key))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Can't register the hot key.");

            _registered.Add(hotKey, id);

            return hotKey;
        }

        /// <summary>
        /// Unregisters the hot key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        public void UnregisterHotKey(Keys key, ModifierKeys modifiers)
        {
            UnregisterHotKey(new HotKey(key, modifiers));
        }

        /// <summary>
        /// Unregisters the hot key.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public void UnregisterHotKey(HotKey hotKey)
        {
            int id;
            if (_registered.TryGetValue(hotKey, out id))
            {
                WinApiHelper.UnregisterHotKey(_windowHandleSource.Handle, id);
                _registered.Remove(hotKey);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyPressed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyPressedEventArgs"/> instance containing the hot key information.</param>
        public void OnKeyPressed(KeyPressedEventArgs e)
        {
            var handler = KeyPressed;
            if (handler != null)
                handler(this, e);
        }

        private int getFreeKeyId()
        {
            return _registered.Any() ? _registered.Values.Max() + 1 : 0;
        }

        private IntPtr messagesHandler(IntPtr handle, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (message == WinApiHelper.WmHotKey)
            {
                // Extract key and modifiers from the message.
                var key = (Keys)(((int)lParam >> 16) & 0xFFFF);
                var modifiers = (ModifierKeys)((int)lParam & 0xFFFF);
                var hotKey = new HotKey(key, modifiers);
                
                OnKeyPressed(new KeyPressedEventArgs(hotKey));
                
                handled = true;
                return new IntPtr(1);
            }

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
                WinApiHelper.UnregisterHotKey(_windowHandleSource.Handle, hotKey.Value);
            }

            _windowHandleSource.RemoveHook(messagesHandler);
            _windowHandleSource.Dispose();
        }
    }
}
