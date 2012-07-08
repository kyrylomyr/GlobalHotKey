using System;

namespace GlobalHotKey
{
    /// <summary>
    /// Arguments for key pressed event which contain information about pressed hot key.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the pressed hot key.
        /// </summary>
        public HotKey HotKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressedEventArgs"/> class.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public KeyPressedEventArgs(HotKey hotKey)
        {
            HotKey = hotKey;
        }
    }
}