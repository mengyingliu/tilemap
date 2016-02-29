namespace TileMapManager
{
    partial class LoadForm
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
            this.labStatus = new DevExpress.XtraEditors.LabelControl();
            this.loadProgressBarControl = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.loadProgressBarControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labStatus
            // 
            this.labStatus.Location = new System.Drawing.Point(36, 319);
            this.labStatus.MaximumSize = new System.Drawing.Size(500, 14);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(144, 14);
            this.labStatus.TabIndex = 1;
            this.labStatus.Text = "正在加载中，请稍后。。。";
            this.labStatus.UseWaitCursor = true;
            // 
            // loadProgressBarControl
            // 
            this.loadProgressBarControl.Location = new System.Drawing.Point(12, 352);
            this.loadProgressBarControl.Name = "loadProgressBarControl";
            this.loadProgressBarControl.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.loadProgressBarControl.Size = new System.Drawing.Size(576, 10);
            this.loadProgressBarControl.TabIndex = 2;
            this.loadProgressBarControl.UseWaitCursor = true;
            // 
            // LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::TileMapManager.Properties.Resources.LoadBackImg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.ControlBox = false;
            this.Controls.Add(this.loadProgressBarControl);
            this.Controls.Add(this.labStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseWaitCursor = true;
            ((System.ComponentModel.ISupportInitialize)(this.loadProgressBarControl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labStatus;
        private DevExpress.XtraEditors.ProgressBarControl loadProgressBarControl;
    }
}