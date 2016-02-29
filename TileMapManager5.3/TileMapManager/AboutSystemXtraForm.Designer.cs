namespace TileMapManager
{
    partial class AboutSystemXtraForm
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
            this.labelControl_exit = new DevExpress.XtraEditors.LabelControl();
            this.label_aboutSys = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl_exit
            // 
            this.labelControl_exit.Location = new System.Drawing.Point(500, 374);
            this.labelControl_exit.Name = "labelControl_exit";
            this.labelControl_exit.Size = new System.Drawing.Size(48, 14);
            this.labelControl_exit.TabIndex = 0;
            this.labelControl_exit.Text = "点击退出";
            this.labelControl_exit.Click += new System.EventHandler(this.labelControl_exit_Click);
            // 
            // label_aboutSys
            // 
            this.label_aboutSys.AutoSize = true;
            this.label_aboutSys.Location = new System.Drawing.Point(3, 0);
            this.label_aboutSys.Name = "label_aboutSys";
            this.label_aboutSys.Size = new System.Drawing.Size(30, 14);
            this.label_aboutSys.TabIndex = 0;
            this.label_aboutSys.Text = "text";
            this.label_aboutSys.Click += new System.EventHandler(this.label_aboutSys_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label_aboutSys);
            this.panel1.Location = new System.Drawing.Point(12, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(576, 303);
            this.panel1.TabIndex = 1;
            // 
            // AboutSystemXtraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = global::TileMapManager.Properties.Resources.AboutSys;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelControl_exit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutSystemXtraForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutAuthorXtraForm";
            this.Load += new System.EventHandler(this.AboutSystemXtraForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl_exit;
        private System.Windows.Forms.Label label_aboutSys;
        private System.Windows.Forms.Panel panel1;

    }
}