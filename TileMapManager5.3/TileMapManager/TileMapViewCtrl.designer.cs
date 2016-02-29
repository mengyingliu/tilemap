namespace TileMapManager
{
    partial class TileMapViewCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mapBrowserContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.放大ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.复位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.LinesMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PolysMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapPicBox = new System.Windows.Forms.PictureBox();
            this.mapBrowserContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mapBrowserContextMenuStrip
            // 
            this.mapBrowserContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.放大ToolStripMenuItem,
            this.缩小ToolStripMenuItem,
            this.移动ToolStripMenuItem,
            this.toolStripSeparator1,
            this.复位ToolStripMenuItem,
            this.toolStripSeparator2,
            this.刷新ToolStripMenuItem,
            this.toolStripSeparator3,
            this.LinesMeasureToolStripMenuItem,
            this.PolysMeasureToolStripMenuItem});
            this.mapBrowserContextMenuStrip.Name = "mapBrowserContextMenuStrip";
            this.mapBrowserContextMenuStrip.Size = new System.Drawing.Size(157, 198);
            // 
            // 放大ToolStripMenuItem
            // 
            this.放大ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuZoomIn;
            this.放大ToolStripMenuItem.Name = "放大ToolStripMenuItem";
            this.放大ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.放大ToolStripMenuItem.Text = "放大窗口";
            this.放大ToolStripMenuItem.Click += new System.EventHandler(this.放大ToolStripMenuItem_Click);
            // 
            // 缩小ToolStripMenuItem
            // 
            this.缩小ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuZoomOut;
            this.缩小ToolStripMenuItem.Name = "缩小ToolStripMenuItem";
            this.缩小ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.缩小ToolStripMenuItem.Text = "缩小窗口";
            this.缩小ToolStripMenuItem.Click += new System.EventHandler(this.缩小ToolStripMenuItem_Click);
            // 
            // 移动ToolStripMenuItem
            // 
            this.移动ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuHander;
            this.移动ToolStripMenuItem.Name = "移动ToolStripMenuItem";
            this.移动ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.移动ToolStripMenuItem.Text = "移动窗口";
            this.移动ToolStripMenuItem.Click += new System.EventHandler(this.移动ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // 复位ToolStripMenuItem
            // 
            this.复位ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuRestore;
            this.复位ToolStripMenuItem.Name = "复位ToolStripMenuItem";
            this.复位ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.复位ToolStripMenuItem.Text = "复位窗口";
            this.复位ToolStripMenuItem.Click += new System.EventHandler(this.复位ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuRefresh;
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.刷新ToolStripMenuItem.Text = "刷新窗口";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(153, 6);
            // 
            // LinesMeasureToolStripMenuItem
            // 
            this.LinesMeasureToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.GetPloyLenghth;
            this.LinesMeasureToolStripMenuItem.Name = "LinesMeasureToolStripMenuItem";
            this.LinesMeasureToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.LinesMeasureToolStripMenuItem.Text = "长度量算(开始)";
            this.LinesMeasureToolStripMenuItem.Click += new System.EventHandler(this.长度量算ToolStripMenuItem_Click);
            // 
            // PolysMeasureToolStripMenuItem
            // 
            this.PolysMeasureToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.GetPolyArea;
            this.PolysMeasureToolStripMenuItem.Name = "PolysMeasureToolStripMenuItem";
            this.PolysMeasureToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.PolysMeasureToolStripMenuItem.Text = "面积量算(开始)";
            this.PolysMeasureToolStripMenuItem.Click += new System.EventHandler(this.面积量算ToolStripMenuItem_Click);
            // 
            // mapPicBox
            // 
            this.mapPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.mapPicBox.ContextMenuStrip = this.mapBrowserContextMenuStrip;
            this.mapPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPicBox.Location = new System.Drawing.Point(0, 0);
            this.mapPicBox.Name = "mapPicBox";
            this.mapPicBox.Size = new System.Drawing.Size(400, 400);
            this.mapPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mapPicBox.TabIndex = 1;
            this.mapPicBox.TabStop = false;
            this.mapPicBox.SizeChanged += new System.EventHandler(this.TileMapPictureBox_SizeChanged);
            this.mapPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TileMapPictureBox_MouseDown);
            this.mapPicBox.MouseEnter += new System.EventHandler(this.mapPicBox_MouseEnter);
            this.mapPicBox.MouseLeave += new System.EventHandler(this.mapPicBox_MouseLeave);
            this.mapPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TileMapPictureBox_MouseMove);
            this.mapPicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TileMapPictureBox_MouseUp);
            // 
            // TileMapViewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.mapPicBox);
            this.Name = "TileMapViewCtrl";
            this.Size = new System.Drawing.Size(400, 400);
            this.mapBrowserContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapPicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mapBrowserContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 放大ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 复位ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem LinesMeasureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PolysMeasureToolStripMenuItem;
        public System.Windows.Forms.PictureBox mapPicBox;
    }
}
