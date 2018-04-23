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

            if(string.IsNullOrWhiteSpace(tbMainDB.Text)|| string.IsNullOrWhiteSpace(tbPassword.Text)
                || string.IsNullOrWhiteSpace(tbTempDB.Text) || string.IsNullOrWhiteSpace(tbServer.Text) || string.IsNullOrWhiteSpace(tbUserName.Text))
                   return; 

            var connectionString = "Data Source={0};Initial Catalog={1};User ID={2};Password={3}";

            var fiasConn = String.Format(connectionString, tbServer.Text, tbMainDB.Text, tbUserName.Text, tbPassword.Text);
            var fiasDataContext = new FIASClassesDataContext(fiasConn);

            var fias_temp_Conn = String.Format(connectionString, tbServer.Text, tbTempDB.Text, tbUserName.Text, tbPassword.Text);
            var fias_tempDataContext = new FIASClassesDataContext(fias_temp_Conn);

            if (tbTempDB.Text.ToLower() == "fias" &&
                DialogResult.No == MessageBox.Show("Вы точно уверены, что временная БД называется fias?", "Предупреждение.", MessageBoxButtons.YesNo))
            {
                btnLogin.Enabled = true;
                return;
            }

            if (!fiasDataContext.DatabaseExists())
            {
                MessageBox.Show("Не удалось подключиться. Проверьте данные для входа.");
                btnLogin.Enabled = true;
                return;
            }

            if(fias_tempDataContext.DatabaseExists())
            {
                fias_tempDataContext.DeleteDatabase();
            }

            fias_tempDataContext.CreateDatabase();

            btnLogin.Enabled = true;
            
            MainForm form = new MainForm(fiasDataContext, fias_tempDataContext);
            this.Hide();
            if (form.ShowDialog() != DialogResult.OK)
                Close();
        }
    }
}
