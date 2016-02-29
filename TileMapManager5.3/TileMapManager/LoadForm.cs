using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TileMapManager
{
    public partial class LoadForm : Form
    {
        private string _StatusInfo = "";
        private int _loadProgress = 0;
        public LoadForm()
        {
            // TODO: Complete member initialization
            InitializeComponent();
        }

        public string StatusInfo
        {
            set
            {
                _StatusInfo = value;
            }
            get
            {
                return _StatusInfo;
            }
        }

        public int LoadProgress
        {
            set
            {
                _loadProgress = value;
            }
            get
            {
                return _loadProgress;
            }
        }

        public void ChangeLoadStatus()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.ChangeLoadStatus));
                    return;
                }

                labStatus.Text = _StatusInfo;
                loadProgressBarControl.Position = _loadProgress;
                Refresh();
            }
            catch (Exception /*e*/)
            {
                MessageBox.Show("出现异常啦！");
            }
        }
    }
}
