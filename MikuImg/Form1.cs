using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MikuImg {
    public partial class Form : System.Windows.Forms.Form {
        public Form() {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;

            var args = Environment.GetCommandLineArgs();
            if(args.Length > 1) {
                pictureBox.ImageLocation = args[1];
            }
        }

        private void pictureBox_LoadCompleted(object sender, AsyncCompletedEventArgs e) {
            var screenSize = Screen.FromControl(this.pictureBox).WorkingArea.Size;
            float height = pictureBox.Image.Height;
            float width = pictureBox.Image.Width;

            if(width > screenSize.Width) {
                height = (height / width) * screenSize.Width;
                width = screenSize.Width;
            }

            if(height > screenSize.Height) {
                width = (width / height) * screenSize.Height;
                height = screenSize.Height;
            }

            this.Width = pictureBox.Width = (int)width;
            this.Height = pictureBox.Height = (int)height;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape) Application.Exit();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void pictureBox_MouseDown(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

            if(e.Button == MouseButtons.Right) {
                contextMenuStrip1.Show((Control)sender, e.Location);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void pictureBox_Click(object sender, EventArgs e) {

        }
    }
}
