namespace TileMapManager
{
    partial class AboutAuthorXtraForm
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
            // AboutAuthorXtraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = global::TileMapManager.Properties.Resources.AboutAuthor;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.ControlBox = false;
            this.Controls.Add(this.labelControl_exit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutAuthorXtraForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutAuthorXtraForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl_exit;

    }
}