using System.IO;
using System.Windows.Forms;

namespace Kepler.Service.Setup
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InstallButton_Click(object sender, System.EventArgs e)
        {
            if (installationPath.Text.Trim() == "")
            {
                MessageBox.Show("Please, specify installation directory", "Empty installation path",
                    MessageBoxButtons.OK);
                return;
            }

            if (!Directory.Exists(installationPath.Text))
                Directory.CreateDirectory(installationPath.Text);
        }

        private void browseButton_Click(object sender, System.EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            installationPath.Text = folderBrowserDialog.SelectedPath;
        }
    }
}