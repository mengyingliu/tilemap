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
    public partial class AboutSystemXtraForm : DevExpress.XtraEditors.XtraForm
    {
        public AboutSystemXtraForm()
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
        /// <summary>
        /// 初始化时 读取文本文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutSystemXtraForm_Load(object sender, EventArgs e)
        {
            label_aboutSys.Text = Properties.Resources.about_sys;
        }

        private void label_aboutSys_Click(object sender, EventArgs e)
        {

        }
    }
}