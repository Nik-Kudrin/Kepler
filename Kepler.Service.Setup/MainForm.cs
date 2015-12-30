using System;
using System.IO;
using System.Windows.Forms;

namespace Kepler.Service.Setup
{
    public partial class MainForm : Form
    {
        private InstallationInfo _installationInfo = new InstallationInfo();

        public MainForm()
        {
            InitializeComponent();
        }

        private void InstallButton_Click(object sender, System.EventArgs e)
        {
            var installationDir = installationPath.Text.Trim();
            if (installationDir == "")
            {
                MessageBox.Show("Please, specify installation directory", "Empty installation path",
                    MessageBoxButtons.OK);
                return;
            }

            var sqlInstanceName = sqlNameTextBox.Text.Trim();

            if (sqlInstanceName == "")
            {
                MessageBox.Show("Please, specify MS SQL instance name", "Empty field",
                    MessageBoxButtons.OK);
                return;
            }

            var dbName = dbNameTextBox.Text.Trim();

            if (dbName == "")
            {
                MessageBox.Show("Please, specify database name", "Empty field", MessageBoxButtons.OK);
                return;
            }

            var userName = userMaskedTextBox.Text.Trim();

            if (userName == "")
            {
                MessageBox.Show("Please, specify user name", "Empty field", MessageBoxButtons.OK);
                return;
            }

            var password = passwordMaskedTextBox.Text;

            if (password == "")
            {
                MessageBox.Show("Please, specify password for SQL user", "Empty field", MessageBoxButtons.OK);
                return;
            }

            long port = 0;

            try
            {
                port = long.Parse(portMaskedTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Please, specify port for Kepler Service", "Empty field",
                    MessageBoxButtons.OK);
                return;
            }

            if (port == 0)
            {
                MessageBox.Show("Please, specify port for Kepler Service", "Empty field",
                    MessageBoxButtons.OK);
                return;
            }

            _installationInfo.InstallationDir = installationDir;
            _installationInfo.SQLInstanceName = sqlInstanceName;
            _installationInfo.DBName = dbName;
            _installationInfo.UserName = userName;
            _installationInfo.Password = password;
            _installationInfo.Port = port;

            InstallKeplerService();
        }

        private void browseButton_Click(object sender, System.EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            installationPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void InstallKeplerService()
        {
            if (!Directory.Exists(installationPath.Text))
                Directory.CreateDirectory(installationPath.Text);
        }
    }
}