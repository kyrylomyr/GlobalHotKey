using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace GlobalHotKey
{
    /// <summary>
    /// Arguments for key pressed event which contain information about pressed hot key.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressedEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        public KeyPressedEventArgs(Keys key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Gets the key modifiers.
        /// </summary>
        public ModifierKeys Modifiers { get; private set; }
    }
}