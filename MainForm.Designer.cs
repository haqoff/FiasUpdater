namespace FIASUpdater
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.btnCheckAllUpdates = new System.Windows.Forms.Button();
            this.pUpdate = new System.Windows.Forms.Panel();
            this.lblReadyToUpdate = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tbVersionDate = new System.Windows.Forms.TextBox();
            this.lblVersionDate = new System.Windows.Forms.Label();
            this.btnLoadNewVersion = new System.Windows.Forms.Button();
            this.tbNewVersionPath = new System.Windows.Forms.TextBox();
            this.lblNewVersion = new System.Windows.Forms.Label();
            this.btnLoadScheme = new System.Windows.Forms.Button();
            this.tbSchemePath = new System.Windows.Forms.TextBox();
            this.lblScheme = new System.Windows.Forms.Label();
            this.pUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCurrentVersion.Location = new System.Drawing.Point(69, 9);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(281, 26);
            this.lblCurrentVersion.TabIndex = 0;
            this.lblCurrentVersion.Text = "Текущая версия FIAS: 00.00.00";
            // 
            // btnCheckAllUpdates
            // 
            this.btnCheckAllUpdates.Location = new System.Drawing.Point(112, 38);
            this.btnCheckAllUpdates.Name = "btnCheckAllUpdates";
            this.btnCheckAllUpdates.Size = new System.Drawing.Size(184, 23);
            this.btnCheckAllUpdates.TabIndex = 1;
            this.btnCheckAllUpdates.Text = "Посмотреть все обновления";
            this.btnCheckAllUpdates.UseVisualStyleBackColor = true;
            this.btnCheckAllUpdates.Click += new System.EventHandler(this.btnCheckAllUpdates_Click);
            // 
            // pUpdate
            // 
            this.pUpdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pUpdate.Controls.Add(this.lblReadyToUpdate);
            this.pUpdate.Controls.Add(this.btnUpdate);
            this.pUpdate.Controls.Add(this.tbVersionDate);
            this.pUpdate.Controls.Add(this.lblVersionDate);
            this.pUpdate.Controls.Add(this.btnLoadNewVersion);
            this.pUpdate.Controls.Add(this.tbNewVersionPath);
            this.pUpdate.Controls.Add(this.lblNewVersion);
            this.pUpdate.Controls.Add(this.btnLoadScheme);
            this.pUpdate.Controls.Add(this.tbSchemePath);
            this.pUpdate.Controls.Add(this.lblScheme);
            this.pUpdate.Location = new System.Drawing.Point(12, 67);
            this.pUpdate.Name = "pUpdate";
            this.pUpdate.Size = new System.Drawing.Size(399, 174);
            this.pUpdate.TabIndex = 2;
            // 
            // lblReadyToUpdate
            // 
            this.lblReadyToUpdate.Location = new System.Drawing.Point(4, 97);
            this.lblReadyToUpdate.Name = "lblReadyToUpdate";
            this.lblReadyToUpdate.Size = new System.Drawing.Size(390, 13);
            this.lblReadyToUpdate.TabIndex = 10;
            this.lblReadyToUpdate.Text = "Готово к обновлению.";
            this.lblReadyToUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblReadyToUpdate.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(152, 146);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 23);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Обновить FIAS";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // tbVersionDate
            // 
            this.tbVersionDate.Location = new System.Drawing.Point(91, 74);
            this.tbVersionDate.Name = "tbVersionDate";
            this.tbVersionDate.ReadOnly = true;
            this.tbVersionDate.Size = new System.Drawing.Size(259, 20);
            this.tbVersionDate.TabIndex = 7;
            // 
            // lblVersionDate
            // 
            this.lblVersionDate.AutoSize = true;
            this.lblVersionDate.Location = new System.Drawing.Point(3, 77);
            this.lblVersionDate.Name = "lblVersionDate";
            this.lblVersionDate.Size = new System.Drawing.Size(75, 13);
            this.lblVersionDate.TabIndex = 6;
            this.lblVersionDate.Text = "Дата версии:";
            // 
            // btnLoadNewVersion
            // 
            this.btnLoadNewVersion.Enabled = false;
            this.btnLoadNewVersion.Location = new System.Drawing.Point(356, 43);
            this.btnLoadNewVersion.Name = "btnLoadNewVersion";
            this.btnLoadNewVersion.Size = new System.Drawing.Size(33, 23);
            this.btnLoadNewVersion.TabIndex = 5;
            this.btnLoadNewVersion.Text = "...";
            this.btnLoadNewVersion.UseVisualStyleBackColor = true;
            this.btnLoadNewVersion.Click += new System.EventHandler(this.btnLoadNewVersion_Click);
            // 
            // tbNewVersionPath
            // 
            this.tbNewVersionPath.Location = new System.Drawing.Point(91, 45);
            this.tbNewVersionPath.Name = "tbNewVersionPath";
            this.tbNewVersionPath.ReadOnly = true;
            this.tbNewVersionPath.Size = new System.Drawing.Size(259, 20);
            this.tbNewVersionPath.TabIndex = 4;
            // 
            // lblNewVersion
            // 
            this.lblNewVersion.AutoSize = true;
            this.lblNewVersion.Location = new System.Drawing.Point(4, 48);
            this.lblNewVersion.Name = "lblNewVersion";
            this.lblNewVersion.Size = new System.Drawing.Size(81, 13);
            this.lblNewVersion.TabIndex = 3;
            this.lblNewVersion.Text = "Новая версия:";
            // 
            // btnLoadScheme
            // 
            this.btnLoadScheme.Location = new System.Drawing.Point(356, 12);
            this.btnLoadScheme.Name = "btnLoadScheme";
            this.btnLoadScheme.Size = new System.Drawing.Size(33, 23);
            this.btnLoadScheme.TabIndex = 2;
            this.btnLoadScheme.Text = "...";
            this.btnLoadScheme.UseVisualStyleBackColor = true;
            this.btnLoadScheme.Click += new System.EventHandler(this.btnLoadScheme_Click);
            // 
            // tbSchemePath
            // 
            this.tbSchemePath.Location = new System.Drawing.Point(91, 14);
            this.tbSchemePath.Name = "tbSchemePath";
            this.tbSchemePath.ReadOnly = true;
            this.tbSchemePath.Size = new System.Drawing.Size(259, 20);
            this.tbSchemePath.TabIndex = 1;
            // 
            // lblScheme
            // 
            this.lblScheme.AutoSize = true;
            this.lblScheme.Location = new System.Drawing.Point(4, 17);
            this.lblScheme.Name = "lblScheme";
            this.lblScheme.Size = new System.Drawing.Size(42, 13);
            this.lblScheme.TabIndex = 0;
            this.lblScheme.Text = "Схема:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 253);
            this.Controls.Add(this.pUpdate);
            this.Controls.Add(this.btnCheckAllUpdates);
            this.Controls.Add(this.lblCurrentVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Обновление FIAS v2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pUpdate.ResumeLayout(false);
            this.pUpdate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Button btnCheckAllUpdates;
        private System.Windows.Forms.Panel pUpdate;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox tbVersionDate;
        private System.Windows.Forms.Label lblVersionDate;
        private System.Windows.Forms.Button btnLoadNewVersion;
        private System.Windows.Forms.TextBox tbNewVersionPath;
        private System.Windows.Forms.Label lblNewVersion;
        private System.Windows.Forms.Button btnLoadScheme;
        private System.Windows.Forms.TextBox tbSchemePath;
        private System.Windows.Forms.Label lblScheme;
        private System.Windows.Forms.Label lblReadyToUpdate;
    }
}