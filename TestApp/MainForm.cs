using System;
using System.Windows.Forms;
using GlobalHotKey;

namespace TestApp
{
    public partial class MainForm : Form
    {
        private readonly HotKeyManager _manager;

        public MainForm()
        {
            InitializeComponent();
            _manager = new HotKeyManager();
            _manager.KeyPressed += _manager_KeyPressed;
        }

        void _manager_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            MessageBox.Show("Hot key pressed!");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            _manager.RegisterHotKey(Keys.F5,
                                    System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt);
        }

        private void btnUnregister_Click(object sender, EventArgs e)
        {
            _manager.UnregisterHotKey(Keys.F5,
                                    System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt);
        }
    }
}
