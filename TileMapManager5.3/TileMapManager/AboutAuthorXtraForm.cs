using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace TileMapManager
{
    public partial class AboutAuthorXtraForm : DevExpress.XtraEditors.XtraForm
    {
        public AboutAuthorXtraForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelControl_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}