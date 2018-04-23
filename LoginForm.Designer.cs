namespace FIASUpdater
{
    partial class LoginForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbMainDB = new System.Windows.Forms.TextBox();
            this.lblMainDB = new System.Windows.Forms.Label();
            this.lblTempDB = new System.Windows.Forms.Label();
            this.tbTempDB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(79, 31);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(276, 20);
            this.tbServer.TabIndex = 0;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(26, 34);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(47, 13);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "Сервер:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Введите данные для входа.";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(26, 60);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(32, 13);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "Имя:";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(79, 57);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(276, 20);
            this.tbUserName.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(26, 83);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(48, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Пароль:";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(79, 83);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(276, 20);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(153, 167);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "Войти";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbMainDB
            // 
            this.tbMainDB.Location = new System.Drawing.Point(142, 109);
            this.tbMainDB.Name = "tbMainDB";
            this.tbMainDB.Size = new System.Drawing.Size(213, 20);
            this.tbMainDB.TabIndex = 8;
            // 
            // lblMainDB
            // 
            this.lblMainDB.AutoSize = true;
            this.lblMainDB.Location = new System.Drawing.Point(26, 112);
            this.lblMainDB.Name = "lblMainDB";
            this.lblMainDB.Size = new System.Drawing.Size(102, 13);
            this.lblMainDB.TabIndex = 9;
            this.lblMainDB.Text = "Имя основной БД:";
            // 
            // lblTempDB
            // 
            this.lblTempDB.AutoSize = true;
            this.lblTempDB.Location = new System.Drawing.Point(26, 139);
            this.lblTempDB.Name = "lblTempDB";
            this.lblTempDB.Size = new System.Drawing.Size(110, 13);
            this.lblTempDB.TabIndex = 11;
            this.lblTempDB.Text = "Имя временной БД:";
            // 
            // tbTempDB
            // 
            this.tbTempDB.Location = new System.Drawing.Point(142, 135);
            this.tbTempDB.Name = "tbTempDB";
            this.tbTempDB.Size = new System.Drawing.Size(213, 20);
            this.tbTempDB.TabIndex = 10;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 198);
            this.Controls.Add(this.lblTempDB);
            this.Controls.Add(this.tbTempDB);
            this.Controls.Add(this.lblMainDB);
            this.Controls.Add(this.tbMainDB);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.tbServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox tbMainDB;
        private System.Windows.Forms.Label lblMainDB;
        private System.Windows.Forms.Label lblTempDB;
        private System.Windows.Forms.TextBox tbTempDB;
    }
}

