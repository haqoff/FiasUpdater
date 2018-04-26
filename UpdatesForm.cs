using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIASUpdater
{
    public partial class UpdatesForm : Form
    {
        private FIASClassesDataContext mainDB;

        public UpdatesForm(FIASClassesDataContext mainDB)
        {
            this.mainDB = mainDB;

            InitializeComponent();
        }

        private void UpdatesForm_Load(object sender, EventArgs e)
        {
            dgvUpdates.AllowUserToAddRows = false;
            dgvUpdates.AllowUserToDeleteRows = false;
            dgvUpdates.AllowUserToResizeRows = false;
            dgvUpdates.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvUpdates.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUpdates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUpdates.ReadOnly = true;
            dgvUpdates.DataSource = mainDB.UPDATES;
        }
    }
}
