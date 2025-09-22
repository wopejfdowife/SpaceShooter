using System.Drawing;
using System.Windows.Forms;

namespace SpaceShooter
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            SetupMenu();
        }

        private void SetupMenu()
        {
            this.BackColor = Color.FromArgb(25, 25, 40);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (var gameForm = new MainForm())
            {
                gameForm.ShowDialog();
            }
            this.Show();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {

        }
    }
}
