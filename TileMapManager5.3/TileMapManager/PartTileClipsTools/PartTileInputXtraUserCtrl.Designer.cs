namespace TileMapManager.PartTileClipsTools
{
    partial class PartTileInputXtraUserCtrl
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
            this.editorRow_orgPx = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_orgPointInfor = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_orgPy = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_DPI = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_imgFormat = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_orgLODs = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.simpleButton_StartUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControl_updateTiles = new DevExpress.XtraEditors.ProgressBarControl();
            this.vGridControl_dragAreas = new DevExpress.XtraVerticalGrid.VGridControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.editorRow_imgHeigh = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_imgWidth = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.categoryRow_tileInfor = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.categoryRow_mapRect = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_mapMinX = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_mapMinY = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_mapMaxX = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.editorRow_mapMaxY = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.vGridControl_tileMapProperty = new DevExpress.XtraVerticalGrid.VGridControl();
            this.categoryRow_clipFormat = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.editorRow_clipFormat = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_CompleteUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.textEdit_XMin = new DevExpress.XtraEditors.TextEdit();
            this.textEdit_YMin = new DevExpress.XtraEditors.TextEdit();
            this.textEdit_XMax = new DevExpress.XtraEditors.TextEdit();
            this.textEdit_YMax = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_AddArea = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_ClearEdit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit_orgTileMapPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl_updateTiles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_dragAreas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_tileMapProperty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_XMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_YMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_XMax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_YMax.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonEdit_orgTileMapPath
            // 
            this.buttonEdit_orgTileMapPath.EditValue = "";
            this.buttonEdit_orgTileMapPath.Location = new System.Drawing.Point(71, 3);
            this.buttonEdit_orgTileMapPath.Name = "buttonEdit_orgTileMapPath";
            this.buttonEdit_orgTileMapPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit_orgTileMapPath.Size = new System.Drawing.Size(156, 21);
            this.buttonEdit_orgTileMapPath.TabIndex = 14;
            this.buttonEdit_orgTileMapPath.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit_orgTileMapPath_ButtonPressed);
            // 
            // editorRow_orgPx
            // 
            this.editorRow_orgPx.Name = "editorRow_orgPx";
            this.editorRow_orgPx.Properties.Caption = "X";
            this.editorRow_orgPx.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_orgPx.Properties.Value = 0D;
            // 
            // categoryRow_orgPointInfor
            // 
            this.categoryRow_orgPointInfor.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.editorRow_orgPx,
            this.editorRow_orgPy});
            this.categoryRow_orgPointInfor.Height = 20;
            this.categoryRow_orgPointInfor.Name = "categoryRow_orgPointInfor";
            this.categoryRow_orgPointInfor.Properties.Caption = "切片原点";
            // 
            // editorRow_orgPy
            // 
            this.editorRow_orgPy.Name = "editorRow_orgPy";
            this.editorRow_orgPy.Properties.Caption = "Y";
            this.editorRow_orgPy.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.editorRow_orgPy.Properties.Value = 0D;
            // 
            // editorRow_DPI
            // 
            this.editorRow_DPI.Name = "editorRow_DPI";
            this.editorRow_DPI.Properties.Caption = "DPI";
            this.editorRow_DPI.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.editorRow_DPI.Properties.Value = 96;
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
            // categoryRow_orgLODs
            // 
            this.categoryRow_orgLODs.Height = 20;
            this.categoryRow_orgLODs.Name = "categoryRow_orgLODs";
            this.categoryRow_orgLODs.Properties.Caption = "切片分级";
            // 
            // simpleButton_StartUpdate
            // 
            this.simpleButton_StartUpdate.Location = new System.Drawing.Point(17, 494);
            this.simpleButton_StartUpdate.Name = "simpleButton_StartUpdate";
            this.simpleButton_StartUpdate.Size = new System.Drawing.Size(75, 23);
            this.simpleButton_StartUpdate.TabIndex = 18;
            this.simpleButton_StartUpdate.Text = "开始";
            this.simpleButton_StartUpdate.Click += new System.EventHandler(this.simpleButton_StartUpdate_Click);
            // 
            // progressBarControl_updateTiles
            // 
            this.progressBarControl_updateTiles.Location = new System.Drawing.Point(6, 470);
            this.progressBarControl_updateTiles.Name = "progressBarControl_updateTiles";
            this.progressBarControl_updateTiles.Size = new System.Drawing.Size(227, 18);
            this.progressBarControl_updateTiles.TabIndex = 17;
            // 
            // vGridControl_dragAreas
            // 
            this.vGridControl_dragAreas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGridControl_dragAreas.Location = new System.Drawing.Point(2, 23);
            this.vGridControl_dragAreas.Name = "vGridControl_dragAreas";
            this.vGridControl_dragAreas.Size = new System.Drawing.Size(227, 98);
            this.vGridControl_dragAreas.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.vGridControl_dragAreas);
            this.groupControl2.Location = new System.Drawing.Point(4, 196);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(231, 123);
            this.groupControl2.TabIndex = 16;
            this.groupControl2.Text = "更新区域";
            // 
            // editorRow_imgHeigh
            // 
            this.editorRow_imgHeigh.Name = "editorRow_imgHeigh";
            this.editorRow_imgHeigh.Properties.Caption = "高度(像素)";
            this.editorRow_imgHeigh.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.editorRow_imgHeigh.Properties.Value = 256;
            // 
            // editorRow_imgWidth
            // 
            this.editorRow_imgWidth.Name = "editorRow_imgWidth";
            this.editorRow_imgWidth.Properties.Caption = "宽度(像素)";
            this.editorRow_imgWidth.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.editorRow_imgWidth.Properties.Value = 256;
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
            this.vGridControl_tileMapProperty.Size = new System.Drawing.Size(231, 135);
            this.vGridControl_tileMapProperty.TabIndex = 0;
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
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.vGridControl_tileMapProperty);
            this.groupControl1.Location = new System.Drawing.Point(2, 30);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(235, 160);
            this.groupControl1.TabIndex = 15;
            this.groupControl1.Text = "切片方案";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "切片路径";
            // 
            // simpleButton_CompleteUpdate
            // 
            this.simpleButton_CompleteUpdate.Location = new System.Drawing.Point(152, 494);
            this.simpleButton_CompleteUpdate.Name = "simpleButton_CompleteUpdate";
            this.simpleButton_CompleteUpdate.Size = new System.Drawing.Size(75, 23);
            this.simpleButton_CompleteUpdate.TabIndex = 19;
            this.simpleButton_CompleteUpdate.Text = "完成";
            this.simpleButton_CompleteUpdate.Click += new System.EventHandler(this.simpleButton_CompleteUpdate_Click);
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.simpleButton_ClearEdit);
            this.groupControl3.Controls.Add(this.simpleButton_AddArea);
            this.groupControl3.Controls.Add(this.labelControl5);
            this.groupControl3.Controls.Add(this.labelControl4);
            this.groupControl3.Controls.Add(this.labelControl3);
            this.groupControl3.Controls.Add(this.labelControl2);
            this.groupControl3.Controls.Add(this.textEdit_YMax);
            this.groupControl3.Controls.Add(this.textEdit_XMax);
            this.groupControl3.Controls.Add(this.textEdit_YMin);
            this.groupControl3.Controls.Add(this.textEdit_XMin);
            this.groupControl3.Location = new System.Drawing.Point(6, 325);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(227, 139);
            this.groupControl3.TabIndex = 20;
            this.groupControl3.Text = "输入更新区域";
            // 
            // textEdit_XMin
            // 
            this.textEdit_XMin.EditValue = "0";
            this.textEdit_XMin.Location = new System.Drawing.Point(55, 26);
            this.textEdit_XMin.Name = "textEdit_XMin";
            this.textEdit_XMin.Size = new System.Drawing.Size(100, 21);
            this.textEdit_XMin.TabIndex = 0;
            // 
            // textEdit_YMin
            // 
            this.textEdit_YMin.EditValue = "0";
            this.textEdit_YMin.Location = new System.Drawing.Point(55, 53);
            this.textEdit_YMin.Name = "textEdit_YMin";
            this.textEdit_YMin.Size = new System.Drawing.Size(100, 21);
            this.textEdit_YMin.TabIndex = 1;
            // 
            // textEdit_XMax
            // 
            this.textEdit_XMax.EditValue = "0";
            this.textEdit_XMax.Location = new System.Drawing.Point(55, 80);
            this.textEdit_XMax.Name = "textEdit_XMax";
            this.textEdit_XMax.Size = new System.Drawing.Size(100, 21);
            this.textEdit_XMax.TabIndex = 2;
            // 
            // textEdit_YMax
            // 
            this.textEdit_YMax.EditValue = "0";
            this.textEdit_YMax.Location = new System.Drawing.Point(55, 107);
            this.textEdit_YMax.Name = "textEdit_YMax";
            this.textEdit_YMax.Size = new System.Drawing.Size(100, 21);
            this.textEdit_YMax.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 29);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "XMin=";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 56);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(35, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "YMin=";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(15, 83);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(37, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "XMax=";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(15, 110);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(38, 14);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "YMax=";
            // 
            // simpleButton_AddArea
            // 
            this.simpleButton_AddArea.Location = new System.Drawing.Point(161, 26);
            this.simpleButton_AddArea.Name = "simpleButton_AddArea";
            this.simpleButton_AddArea.Size = new System.Drawing.Size(60, 48);
            this.simpleButton_AddArea.TabIndex = 8;
            this.simpleButton_AddArea.Text = "添加";
            this.simpleButton_AddArea.Click += new System.EventHandler(this.simpleButton_AddArea_Click);
            // 
            // simpleButton_ClearEdit
            // 
            this.simpleButton_ClearEdit.Location = new System.Drawing.Point(161, 80);
            this.simpleButton_ClearEdit.Name = "simpleButton_ClearEdit";
            this.simpleButton_ClearEdit.Size = new System.Drawing.Size(60, 48);
            this.simpleButton_ClearEdit.TabIndex = 9;
            this.simpleButton_ClearEdit.Text = "清零";
            this.simpleButton_ClearEdit.Click += new System.EventHandler(this.simpleButton_ClearEdit_Click);
            // 
            // PartTileInputXtraUserCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.buttonEdit_orgTileMapPath);
            this.Controls.Add(this.simpleButton_StartUpdate);
            this.Controls.Add(this.progressBarControl_updateTiles);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton_CompleteUpdate);
            this.Name = "PartTileInputXtraUserCtrl";
            this.Size = new System.Drawing.Size(240, 525);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit_orgTileMapPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl_updateTiles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_dragAreas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl_tileMapProperty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_XMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_YMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_XMax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_YMax.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit buttonEdit_orgTileMapPath;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_orgPx;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_orgPointInfor;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_orgPy;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_DPI;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_imgFormat;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_orgLODs;
        private DevExpress.XtraEditors.SimpleButton simpleButton_StartUpdate;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl_updateTiles;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl_dragAreas;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_imgHeigh;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_imgWidth;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_tileInfor;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_mapRect;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMinX;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMinY;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMaxX;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_mapMaxY;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl_tileMapProperty;
        private DevExpress.XtraVerticalGrid.Rows.CategoryRow categoryRow_clipFormat;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow editorRow_clipFormat;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton_CompleteUpdate;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton_ClearEdit;
        private DevExpress.XtraEditors.SimpleButton simpleButton_AddArea;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit_YMax;
        private DevExpress.XtraEditors.TextEdit textEdit_XMax;
        private DevExpress.XtraEditors.TextEdit textEdit_YMin;
        private DevExpress.XtraEditors.TextEdit textEdit_XMin;
    }
}
