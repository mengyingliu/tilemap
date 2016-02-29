namespace TileMapManager.EditTools
{
    partial class PartTileDragXtraUserCtrl
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
            this.buttonEdit_orgTileMapPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.vGridControl_tileMapProperty = new DevExpress.XtraVerticalGrid.VGridControl();
            this.categoryRow_mapRect = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_mapMinX = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_mapMinY = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_mapMaxX = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_mapMaxY = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_clipFormat = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_clipFormat = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_tileInfor = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_imgWidth = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_imgHeigh = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_imgFormat = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_DPI = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_orgPointInfor = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_orgPx = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_orgPy = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_orgLODs = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.vGridControl_dragAreas = new DevExpress.XtraVerticalGrid.VGridControl();
            this.progressBarControl_updateTiles = new DevExpress.XtraEditors.ProgressBarControl();
            this.simpleButton_StartUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_CompleteUpdate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit_orgTileMapPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_tileMapProperty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_dragAreas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl_updateTiles.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonEdit_orgTileMapPath
            // 
            this.buttonEdit_orgTileMapPath.EditValue = "";
            this.buttonEdit_orgTileMapPath.Location = new System.Drawing.Point(69, 9);
            this.buttonEdit_orgTileMapPath.Name = "buttonEdit_orgTileMapPath";
            this.buttonEdit_orgTileMapPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit_orgTileMapPath.Size = new System.Drawing.Size(156, 21);
            this.buttonEdit_orgTileMapPath.TabIndex = 7;
            this.buttonEdit_orgTileMapPath.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit_orgTileMapPath_ButtonPressed);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "切片路径";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.vGridControl_tileMapProperty);
            this.groupControl1.Location = new System.Drawing.Point(0, 36);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(235, 258);
            this.groupControl1.TabIndex = 8;
            this.groupControl1.Text = "切片方案";
            // 
            // vGridControl_tileMapProperty
            // 
            this.vGridControl_tileMapProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGridControl_tileMapProperty.Location = new System.Drawing.Point(2, 23);
            this.vGridControl_tileMapProperty.Name = "vGridControl_tileMapProperty";
            this.vGridControl_tileMapProperty.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.categoryRow_mapRect,
            this.categoryRow_clipFormat,
            this.categoryRow_tileInfor,
            this.categoryRow_orgPointInfor,
            this.categoryRow_orgLODs});
            this.vGridControl_tileMapProperty.Size = new System.Drawing.Size(231, 233);
            this.vGridControl_tileMapProperty.TabIndex = 0;
            // 
            // categoryRow_mapRect
            // 
            this.categoryRow_mapRect.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorRow_mapMinX,
            this.editorRow_mapMinY,
            this.editorRow_mapMaxX,
            this.editorRow_mapMaxY});
            this.categoryRow_mapRect.Name = "categoryRow_mapRect";
            this.categoryRow_mapRect.Properties.Caption = "全图范围";
            // 
            // editorRow_mapMinX
            // 
            this.editorRow_mapMinX.Name = "editorRow_mapMinX";
            this.editorRow_mapMinX.Properties.Caption = "MinX";
            this.editorRow_mapMinX.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_mapMinX.Properties.Value = 0D;
            // 
            // editorRow_mapMinY
            // 
            this.editorRow_mapMinY.Name = "editorRow_mapMinY";
            this.editorRow_mapMinY.Properties.Caption = "MinY";
            this.editorRow_mapMinY.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_mapMinY.Properties.Value = 0D;
            // 
            // editorRow_mapMaxX
            // 
            this.editorRow_mapMaxX.Name = "editorRow_mapMaxX";
            this.editorRow_mapMaxX.Properties.Caption = "MaxX";
            this.editorRow_mapMaxX.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_mapMaxX.Properties.Value = 0D;
            // 
            // editorRow_mapMaxY
            // 
            this.editorRow_mapMaxY.Name = "editorRow_mapMaxY";
            this.editorRow_mapMaxY.Properties.Caption = "MaxY";
            this.editorRow_mapMaxY.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_mapMaxY.Properties.Value = 0D;
            // 
            // categoryRow_clipFormat
            // 
            this.categoryRow_clipFormat.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorRow_clipFormat});
            this.categoryRow_clipFormat.Name = "categoryRow_clipFormat";
            this.categoryRow_clipFormat.Properties.Caption = "切片格式";
            // 
            // editorRow_clipFormat
            // 
            this.editorRow_clipFormat.Appearance.Options.UseTextOptions = true;
            this.editorRow_clipFormat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.editorRow_clipFormat.Name = "editorRow_clipFormat";
            this.editorRow_clipFormat.Properties.Caption = "切片类型";
            this.editorRow_clipFormat.Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.editorRow_clipFormat.Properties.Value = "紧凑型";
            // 
            // categoryRow_tileInfor
            // 
            this.categoryRow_tileInfor.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorRow_imgWidth,
            this.editorRow_imgHeigh,
            this.editorRow_imgFormat,
            this.editorRow_DPI});
            this.categoryRow_tileInfor.Height = 16;
            this.categoryRow_tileInfor.Name = "categoryRow_tileInfor";
            this.categoryRow_tileInfor.Properties.Caption = "分块信息";
            // 
            // editorRow_imgWidth
            // 
            this.editorRow_imgWidth.Name = "editorRow_imgWidth";
            this.editorRow_imgWidth.Properties.Caption = "宽度(像素)";
            this.editorRow_imgWidth.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.editorRow_imgWidth.Properties.Value = 256;
            // 
            // editorRow_imgHeigh
            // 
            this.editorRow_imgHeigh.Name = "editorRow_imgHeigh";
            this.editorRow_imgHeigh.Properties.Caption = "高度(像素)";
            this.editorRow_imgHeigh.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.editorRow_imgHeigh.Properties.Value = 256;
            // 
            // editorRow_imgFormat
            // 
            this.editorRow_imgFormat.Appearance.Options.UseTextOptions = true;
            this.editorRow_imgFormat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.editorRow_imgFormat.Name = "editorRow_imgFormat";
            this.editorRow_imgFormat.Properties.Caption = "图片格式";
            this.editorRow_imgFormat.Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.editorRow_imgFormat.Properties.Value = "JEPG";
            // 
            // editorRow_DPI
            // 
            this.editorRow_DPI.Name = "editorRow_DPI";
            this.editorRow_DPI.Properties.Caption = "DPI";
            this.editorRow_DPI.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.editorRow_DPI.Properties.Value = 96;
            // 
            // categoryRow_orgPointInfor
            // 
            this.categoryRow_orgPointInfor.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorRow_orgPx,
            this.editorRow_orgPy});
            this.categoryRow_orgPointInfor.Name = "categoryRow_orgPointInfor";
            this.categoryRow_orgPointInfor.Properties.Caption = "切片原点";
            // 
            // editorRow_orgPx
            // 
            this.editorRow_orgPx.Name = "editorRow_orgPx";
            this.editorRow_orgPx.Properties.Caption = "X";
            this.editorRow_orgPx.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_orgPx.Properties.Value = 0D;
            // 
            // editorRow_orgPy
            // 
            this.editorRow_orgPy.Name = "editorRow_orgPy";
            this.editorRow_orgPy.Properties.Caption = "Y";
            this.editorRow_orgPy.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_orgPy.Properties.Value = 0D;
            // 
            // categoryRow_orgLODs
            // 
            this.categoryRow_orgLODs.Name = "categoryRow_orgLODs";
            this.categoryRow_orgLODs.Properties.Caption = "切片分级";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.vGridControl_dragAreas);
            this.groupControl2.Location = new System.Drawing.Point(2, 300);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(235, 161);
            this.groupControl2.TabIndex = 9;
            this.groupControl2.Text = "更新区域";
            // 
            // vGridControl_dragAreas
            // 
            this.vGridControl_dragAreas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGridControl_dragAreas.Location = new System.Drawing.Point(2, 23);
            this.vGridControl_dragAreas.Name = "vGridControl_dragAreas";
            this.vGridControl_dragAreas.Size = new System.Drawing.Size(231, 136);
            this.vGridControl_dragAreas.TabIndex = 0;
            // 
            // progressBarControl_updateTiles
            // 
            this.progressBarControl_updateTiles.Location = new System.Drawing.Point(3, 467);
            this.progressBarControl_updateTiles.Name = "progressBarControl_updateTiles";
            this.progressBarControl_updateTiles.Size = new System.Drawing.Size(230, 18);
            this.progressBarControl_updateTiles.TabIndex = 10;
            // 
            // simpleButton_StartUpdate
            // 
            this.simpleButton_StartUpdate.Location = new System.Drawing.Point(15, 491);
            this.simpleButton_StartUpdate.Name = "simpleButton_StartUpdate";
            this.simpleButton_StartUpdate.Size = new System.Drawing.Size(75, 23);
            this.simpleButton_StartUpdate.TabIndex = 11;
            this.simpleButton_StartUpdate.Text = "开始";
            this.simpleButton_StartUpdate.Click += new System.EventHandler(this.simpleButton_StartUpdate_Click);
            // 
            // simpleButton_CompleteUpdate
            // 
            this.simpleButton_CompleteUpdate.Location = new System.Drawing.Point(150, 491);
            this.simpleButton_CompleteUpdate.Name = "simpleButton_CompleteUpdate";
            this.simpleButton_CompleteUpdate.Size = new System.Drawing.Size(75, 23);
            this.simpleButton_CompleteUpdate.TabIndex = 12;
            this.simpleButton_CompleteUpdate.Text = "完成";
            this.simpleButton_CompleteUpdate.Click += new System.EventHandler(this.simpleButton_CompleteUpdate_Click);
            // 
            // PartTileDragXtraUserCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.simpleButton_CompleteUpdate);
            this.Controls.Add(this.simpleButton_StartUpdate);
            this.Controls.Add(this.progressBarControl_updateTiles);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.buttonEdit_orgTileMapPath);
            this.Controls.Add(this.labelControl1);
            this.Name = "PartTileDragXtraUserCtrl";
            this.Size = new System.Drawing.Size(240, 540);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit_orgTileMapPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_tileMapProperty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_dragAreas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl_updateTiles.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit buttonEdit_orgTileMapPath;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl_updateTiles;
        private DevExpress.XtraEditors.SimpleButton simpleButton_StartUpdate;
        private DevExpress.XtraEditors.SimpleButton simpleButton_CompleteUpdate;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl_tileMapProperty;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_clipFormat;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_clipFormat;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_tileInfor;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_imgWidth;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_imgHeigh;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_imgFormat;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_orgPointInfor;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_orgPx;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_orgPy;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_orgLODs;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl_dragAreas;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_mapRect;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMinX;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMinY;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMaxX;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMaxY;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_DPI;
    }
}
