using System;
using System.Windows.Forms;
using System.Windows.Input;
using GlobalHotKey;

namespace TestApp
{
    public partial class MainForm : Form
    {
        private readonly HotKeyManager _hotKeyManager;

        public MainForm()
        {
            InitializeComponent();
            _hotKeyManager = new HotKeyManager();
            _hotKeyManager.KeyPressed += HotKeyManagerPressed;
        }

        void HotKeyManagerPressed(object sender, KeyPressedEventArgs e)
        {
            MessageBox.Show("Hot key pressed!");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            _hotKeyManager.Register(Key.F5,
                                    System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt);
        }

        private void btnUnregister_Click(object sender, EventArgs e)
        {
            _hotKeyManager.Unregister(Key.F5,
                                    System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Alt);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _hotKeyManager.Dispose();
        }
    }
}
