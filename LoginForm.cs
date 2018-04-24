using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FIASUpdater
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            if (string.IsNullOrWhiteSpace(tbMainDB.Text) || string.IsNullOrWhiteSpace(tbPassword.Text)
                || string.IsNullOrWhiteSpace(tbServer.Text) || string.IsNullOrWhiteSpace(tbUserName.Text))
                return;

            var connectionStringPattern = "Data Source={0};Initial Catalog={1};User ID={2};Password={3}";

            var fiasConn = String.Format(connectionStringPattern, tbServer.Text, tbMainDB.Text, tbUserName.Text, tbPassword.Text);
            var fiasDataContext = new FIASClassesDataContext(fiasConn);

            var fias_tempName = "fias_temp_" + Utils.GenerateUnique();
            var fias_temp_Conn = String.Format(connectionStringPattern, tbServer.Text, fias_tempName,
                tbUserName.Text, tbPassword.Text);
            var fias_tempDataContext = new FIASClassesDataContext(fias_temp_Conn);

            if (!fiasDataContext.DatabaseExists())
            {
                MessageBox.Show("Не удалось подключиться. Проверьте данные для входа.");
                btnLogin.Enabled = true;
                return;
            }

            btnLogin.Enabled = true;

            var tempConnOleDBPart = String.Format("Provider=sqloledb;server={0};database={1};user id={2};password={3};persist security info=True;Connect Timeout=30", tbServer.Text, fias_tempName, tbUserName.Text, tbPassword.Text);

            MainForm form = new MainForm(fiasDataContext, fias_tempDataContext, tempConnOleDBPart);
            Hide();
            if (form.ShowDialog() != DialogResult.OK)
                Close();
        }
    }
}
