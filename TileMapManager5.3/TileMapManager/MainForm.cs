using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraBars.Helpers;
using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using DevExpress.XtraBars.Docking;
using TileMapManager.EditTools;
using TileMapManager.PartTileClipsTools;

namespace TileMapManager
{
    public partial class MainForm : Form
    {
        #region 全局变量区
        /// <summary>
        /// 全局区变量
        /// </summary>
        //String m_sDocumentPath = null;//地图文档路径
        TileMapViewCtrl m_tileMapView = null;// 瓦片显示的视图控件
        int m_dpi = 96;//设置为定值了
        String m_currentSelectLayerName = String.Empty;//当前被选中的图层
        private IEngineEditor m_EngineEditor = new EngineEditorClass();//编辑器
        private IEngineEditEvents_Event m_EngineEditEvent_Event;//编辑事件
        #endregion

        #region 初始化及界面事件 
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            Thread.Sleep(1000);
            Splash.setLoadState("状态：控件初始化，正在努力加载中，请稍后。。。", 30);
            InitializeComponent();
            this.m_axTOCControl.SetBuddyControl(this.m_axMapControl.Object);//必须加 通过属性添加 不可靠 会出现不显示的现象
            Splash.setLoadState("状态：初始化数据，马上就到啦。。。", 80);
            InitFormAndData();
            Splash.setLoadState("状态：界面加载中。。。", 90);
            m_tileMapView = new TileMapViewCtrl();
            m_tileMapView.Dock = DockStyle.Fill;
            m_tileMapView.mapPicBox.MouseMove += new MouseEventHandler(mapPicBox_MouseMove);//添加地图显示控件的 事件侦听
            xtraTabPage_tileMapView.Controls.Add(m_tileMapView);
            Splash.setLoadState("状态：恭喜，加载完毕！", 100);
            Splash.Close();
        }

        /// <初始化数据>
        /// 初始化数据 及 控件
        /// </初始化数据>
        private void InitFormAndData()
        {
            //////////////////////////////////////////////////////////////////
            //初始化切片预设比例尺
            m_tileLodScalesInfor.Add(5000);
            m_tileLodScalesInfor.Add(2500);
            m_tileLodScalesInfor.Add(1000);
            //编辑模块的菜单按钮初始化
            barButtonItem_EditStart.Enabled = true;
            barButtonItem_featureSelect.Enabled = true;
            barButtonItem_ClearSelection.Enabled = false;
            barButtonItem_EditRedo.Enabled = false;
            barButtonItem_EditSave.Enabled = false;
            barButtonItem_EditStop.Enabled = false;
            barButtonItem_EditUndo.Enabled = false;
            barButtonItem_featureCopy.Enabled = false;
            barButtonItem_featureCut.Enabled = false;
            barButtonItem_featureDelete.Enabled = false;
            barButtonItem_featureEdit.Enabled = false;
            barButtonItem_featurePaste.Enabled = false;
            barButtonItem_featureSketh.Enabled = false;
            
            //裁剪界面初始化
            partTileDragCtrl.Dock = DockStyle.Fill;
            partScreenTileDragdockPanel.Controls.Add(partTileDragCtrl);
            partTileInputCtrl.Dock = DockStyle.Fill;
            partScreenSetTiledockPanel.Controls.Add(partTileInputCtrl);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //皮肤
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }
        /// <视图切换时>
        /// 视图切换时
        /// </视图切换时>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (ribbonControl1.SelectedPage == ribbonPage_tileMapView)
            {
                xtraTabControl_mapView.SelectedTabPage = xtraTabPage_tileMapView;
            }
            else if (ribbonControl1.SelectedPage == ribbonPage_ArcMapEdit)
            {
                xtraTabControl_mapView.SelectedTabPage = xtraTabPage_arcMapView;
            }
            else if (ribbonControl1.SelectedPage == ribbonPage_tileMapClip)
            {
                xtraTabControl_mapView.SelectedTabPage = xtraTabPage_arcMapView;
            }
            else if (ribbonControl1.SelectedPage == ribbonPage_Project)
            {
                xtraTabControl_mapView.SelectedTabPage = xtraTabPage_arcMapView;
            }
        }

        /// <文档树点击事件>
        /// 选中图层 作为编辑的对象 文档树点击事件
        /// </文档树点击事件>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_axTOCControl_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer layer = null;
            object other = null;
            object index = null;
            //判断所选菜单的类型
            m_axTOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            //选中图层栏
            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                m_axTOCControl.SelectItem(layer, null);
                m_currentSelectLayerName = layer.Name;

                for (int i = 0; i < m_axMapControl.Map.LayerCount; i++)
                {
                    IFeatureLayer feaLayer = m_axMapControl.Map.get_Layer(i) as IFeatureLayer;
                    if (layer.Name == feaLayer.Name)
                    {
                        feaLayer.Selectable = true;
                        ((IEngineEditLayers)m_EngineEditor).SetTargetLayer(feaLayer, 0);

                        barStaticItem_curLayer.Caption = "选中图层:"+layer.Name;//更新当前选择的图层
                    }
                    else
                        feaLayer.Selectable = false;
                }
            }
            //设置CustomProperty为layer (用于自定义的Layer命令)
            m_axTOCControl.CustomProperty = layer;
            ////弹出右键菜单
            //if (item == esriTOCControlItem.esriTOCControlItemMap)
            //    m_menuMap.PopupMenu(e.x, e.y, m_tocControl.hWnd);
            //if (item == esriTOCControlItem.esriTOCControlItemLayer)
            //    //m_menuLayer.PopupMenu(e.x, e.y, m_tocControl.hWnd);
            //    contextMenuStrip1.Show(axTOCControl1, e.x, e.y);
        }


        private void m_axMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            // 显示当前比例尺
            barStaticItem_scale.Caption = "当前比例尺 1:" + ((long)m_axMapControl.MapScale).ToString();
            // 显示当前坐标
            barStaticItem_coordnate.Caption = " 当前坐标X = " + e.mapX.ToString() + " Y = " + e.mapY.ToString() + " " + m_axMapControl.MapUnits.ToString().Substring(4);
        }

        /// <工具条触碰事件>
        /// 工具条触碰事件 更新状态栏属性信息
        /// </工具条触碰事件>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axToolbarControl_mapBrowser_OnMouseMove(object sender, IToolbarControlEvents_OnMouseMoveEvent e)
        {
            // 取得鼠标所在工具的索引号
            int index = axToolbarControl_mapBrowser.HitTest(e.x, e.y, false);
            if (index != -1)
            {
                // 取得鼠标所在工具的ToolbarItem
                IToolbarItem toolbarItem = axToolbarControl_mapBrowser.GetItem(index);
                // 设置状态栏信息
                barStaticItem_Lab.Caption = toolbarItem.Command.Message;
            }
            else
            {
                barStaticItem_Lab.Caption = " 就绪";
            }
        }
        
        /// <瓦片视图中的鼠标移动事件>
        /// 瓦片视图中的鼠标移动事件 更新比例尺和坐标
        /// </瓦片视图中的鼠标移动事件>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mapPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            double tileX = 0;
            double tileY = 0;
            double tileScale = 0;
            m_tileMapView.GetCoordsAndScaleInfors(out tileX, out tileY, out tileScale);
            // 显示当前比例尺
            barStaticItem_scale.Caption = "当前比例尺 1:" + tileScale.ToString();
            // 显示当前坐标
            barStaticItem_coordnate.Caption = " 当前坐标X = " + tileX.ToString() + " Y = " + tileY.ToString();
        }
        #endregion

        #region 地图文档操作操作
        /// <summary>
        /// 打开地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_mapOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //launch a new OpenFile dialog
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Map Documents (*.mxd)|*.mxd";
            dlg.Multiselect = false;
            dlg.Title = "打开地图";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileName != "")
                {
                    m_axMapControl.LoadMxFile(dlg.FileName);
                }
            }
        }

        /// <summary>
        /// 新建地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_mapNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //询问是否保存当前地图
            DialogResult res = MessageBox.Show("是否保存当前地图?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                //如果要保存，调用另存为对话框
                ICommand command = new ControlsSaveAsDocCommandClass();
                command.OnCreate(m_axMapControl.Object);
                command.OnClick();
            }
            //创建新的地图实例
            IMap map = new MapClass();
            map.Name = "Map";
            m_axMapControl.DocumentFilename = string.Empty;
            //更新新建地图实例的共享地图文档
            m_axMapControl.Map = map;
        }
        /// <summary>
        /// 保存地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_mapSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 首先确认当前地图文档是否有效
            if (false != m_axMapControl.CheckMxFile(m_axMapControl.DocumentFilename))
            {
                try
                {
                    //判断pMapDocument是否为空，
                    //获取pMapDocument对象
                    IMxdContents pMxdC = m_axMapControl.Map as IMxdContents;
                    IMapDocument pMapDocument = new MapDocumentClass();
                    pMapDocument.Open(m_axMapControl.DocumentFilename, "");
                    IActiveView pActiveView = m_axMapControl.Map as IActiveView;
                    pMapDocument.ReplaceContents(pMxdC);
                    if (pMapDocument == null) return;
                    //检查地图文档是否是只读
                    if (pMapDocument.get_IsReadOnly(pMapDocument.DocumentFilename) == true)
                    {
                        MessageBox.Show("本地图文档是只读的，不能保存！");
                        return;
                    }
                    //根据相对的路径保存地图文档
                    pMapDocument.Save(pMapDocument.UsesRelativePaths, true);
                    MessageBox.Show("地图文档保存成功!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                //如果要保存，调用另存为对话框
                ICommand command = new ControlsSaveAsDocCommandClass();
                command.OnCreate(m_axMapControl.Object);
                command.OnClick();
            }
        }

        /// <summary>
        /// 另存地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_mapSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //调用另存为命令
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_axMapControl.Object);
            command.OnClick();
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_prjExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region 图层操作
        /// <summary>
        /// 新建shp图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_layerNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新建图层
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "点文件(*.shp)|*.shp|线文件(*.shp)|*.shp|面文件(*.shp)|*.shp";
            saveFileDialog.Title = "输出图层位置";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string strFullPath = saveFileDialog.FileName;
                if (strFullPath.Length == 0) return;
                int index = strFullPath.LastIndexOf("\\");
                string strFolder = strFullPath.Substring(0, index);
                string strFileName = strFullPath.Substring(index + 1);

                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace pFWS = pWorkspaceFactory.OpenFromFile(strFolder, 0) as IFeatureWorkspace;
                esriGeometryType pGeomDefType;
                /////创建的图层类型
                if (saveFileDialog.FilterIndex == 1)
                {
                    pGeomDefType = esriGeometryType.esriGeometryPoint;
                }
                else if (saveFileDialog.FilterIndex == 2)
                {
                    pGeomDefType = esriGeometryType.esriGeometryPolyline;
                }
                else
                {
                    pGeomDefType = esriGeometryType.esriGeometryPolygon;
                }

                ISpatialReferenceFactory ispfac = new ESRI.ArcGIS.Geometry.SpatialReferenceEnvironment();
                IGeographicCoordinateSystem igeocoorsys = ispfac.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_GRS1980);
                IFeatureClass fcls = CreateFeatureClass(pFWS, strFileName, igeocoorsys, esriFeatureType.esriFTSimple, pGeomDefType, null, null, null, "");
                FeatureLayerClass feaLayer = new FeatureLayerClass();
                feaLayer.FeatureClass = fcls;
                feaLayer.Name = fcls.AliasName;
                m_axMapControl.Map.AddLayer(feaLayer);

                m_axTOCControl.SelectItem(feaLayer, null);//选中为新建的图层
                m_currentSelectLayerName = feaLayer.Name;
                m_axMapControl.Refresh();

            }
            saveFileDialog.Dispose();
        }
        /// <summary>
        /// 添加图层 添加shp图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_layerAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "添加shp图层";
            openFileDialog.Filter = "shp文件|*.shp";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] filepaths = openFileDialog.FileNames;

                foreach (string file in filepaths)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string path = file.Substring(0, file.Length - fileInfo.Name.Length);
                    try
                    {
                        m_axMapControl.AddShapeFile(path, fileInfo.Name);
                        m_currentSelectLayerName = fileInfo.Name;//作为当前选中的图层
                    }
                    catch (Exception r)
                    {
                        MessageBox.Show("添加图层失败" + r.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 移除当前所选的图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_layerRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for(int i=0;i<m_axMapControl.Map.LayerCount;i++)
            {
                ILayer layer = m_axMapControl.Map.get_Layer(i);
                if (m_currentSelectLayerName == layer.Name)
                {
                    m_axMapControl.Map.DeleteLayer(layer);
                    m_currentSelectLayerName = String.Empty;
                    break;
                }
            }
        }
        /// <创建要素类>
        /// 创建要素类
        /// </创建要素类>
        /// <param name="pObject">IWorkspace或者IFeatureDataset对象</param>
        /// <param name="pName">要素类名称</param>
        /// <param name="pSpatialReference">空间参考</param>
        /// <param name="pFeatureType">要素类型</param>
        /// <param name="pGeometryType">几何类型</param>
        /// <param name="pFields">字段集</param>
        /// <param name="pUidClsId">CLSID值</param>
        /// <param name="pUidClsExt">EXTCLSID值</param>
        /// <param name="pConfigWord">配置信息关键词</param>
        /// <returns>返回IFeatureClass</returns>
        public static IFeatureClass CreateFeatureClass(object pObject, string pName, ISpatialReference pSpatialReference, esriFeatureType pFeatureType,
                                        esriGeometryType pGeometryType, IFields pFields, UID pUidClsId, UID pUidClsExt, string pConfigWord)
        {
            #region 错误检测
            if (pObject == null)
            {
                throw (new Exception("[pObject] 不能为空!"));
            }
            if (!((pObject is IFeatureWorkspace) || (pObject is IFeatureDataset)))
            {
                throw (new Exception("[pObject] 必须为IFeatureWorkspace 或者 IFeatureDataset"));
            }
            if (pName.Length == 0)
            {
                throw (new Exception("[pName] 不能为空!"));
            }
            if ((pObject is IWorkspace) && (pSpatialReference == null))
            {
                throw (new Exception("[pSpatialReference] 不能为空(对于单独的要素类)"));
            }
            #endregion

            #region pUidClsID字段为空时
            if (pUidClsId == null)
            {
                pUidClsId = new UIDClass();
                switch (pFeatureType)
                {
                    case (esriFeatureType.esriFTSimple):
                        if (pGeometryType == esriGeometryType.esriGeometryLine)
                            pGeometryType = esriGeometryType.esriGeometryPolyline;
                        pUidClsId.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                        break;
                    case (esriFeatureType.esriFTSimpleJunction):
                        pGeometryType = esriGeometryType.esriGeometryPoint;
                        pUidClsId.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTComplexJunction):
                        pUidClsId.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTSimpleEdge):
                        pGeometryType = esriGeometryType.esriGeometryPolyline;
                        pUidClsId.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTComplexEdge):
                        pGeometryType = esriGeometryType.esriGeometryPolyline;
                        pUidClsId.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                        break;
                    case (esriFeatureType.esriFTAnnotation):
                        pGeometryType = esriGeometryType.esriGeometryPolygon;
                        pUidClsId.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                        break;
                    case (esriFeatureType.esriFTDimension):
                        pGeometryType = esriGeometryType.esriGeometryPolygon;
                        pUidClsId.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                        break;
                }
            }
            #endregion

            #region pUidClsExt字段为空时
            if (pUidClsExt == null)
            {
                switch (pFeatureType)
                {
                    case esriFeatureType.esriFTAnnotation:
                        pUidClsExt = new UIDClass();
                        pUidClsExt.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                        break;
                    case esriFeatureType.esriFTDimension:
                        pUidClsExt = new UIDClass();
                        pUidClsExt.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                        break;
                }
            }
            #endregion

            #region 字段集合为空时
            if (pFields == null)
            {
                //实倒化字段集合对象
                pFields = new FieldsClass();
                IFieldsEdit tFieldsEdit = (IFieldsEdit)pFields;

                //创建几何对象字段定义
                IGeometryDef tGeometryDef = new GeometryDefClass();
                IGeometryDefEdit tGeometryDefEdit = tGeometryDef as IGeometryDefEdit;

                //指定几何对象字段属性值
                tGeometryDefEdit.GeometryType_2 = pGeometryType;
                tGeometryDefEdit.GridCount_2 = 1;
                tGeometryDefEdit.set_GridSize(0, 1000);
                if (pObject is IWorkspace)
                {
                    tGeometryDefEdit.SpatialReference_2 = pSpatialReference;
                }

                //创建OID字段
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = fieldOID as IFieldEdit;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                tFieldsEdit.AddField(fieldOID);

                //创建几何字段
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = fieldShape as IFieldEdit;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = tGeometryDef;
                tFieldsEdit.AddField(fieldShape);
            }
            #endregion

            //几何对象字段名称
            string strShapeFieldName = "";
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    strShapeFieldName = pFields.get_Field(i).Name;
                    break;
                }
            }

            if (strShapeFieldName.Length == 0)
            {
                throw (new Exception("字段集中找不到几何对象定义"));
            }

            IFeatureClass tFeatureClass = null;
            if (pObject is IWorkspace)
            {
                //创建独立的FeatureClass
                IWorkspace tWorkspace = pObject as IWorkspace;
                IFeatureWorkspace tFeatureWorkspace = tWorkspace as IFeatureWorkspace;
                tFeatureClass = tFeatureWorkspace.CreateFeatureClass(pName, pFields, pUidClsId, pUidClsExt, pFeatureType, strShapeFieldName, pConfigWord);
            }
            else if (pObject is IFeatureDataset)
            {
                //在要素集中创建FeatureClass
                IFeatureDataset tFeatureDataset = (IFeatureDataset)pObject;
                tFeatureClass = tFeatureDataset.CreateFeatureClass(pName, pFields, pUidClsId, pUidClsExt, pFeatureType, strShapeFieldName, pConfigWord);
            }

            return tFeatureClass;
        }
        #endregion

        #region 图层编辑控制
        ICommand pCmd = null;//命令
        IWorkspaceEdit pWorkspaceEdit = null;//编辑空间
        void m_EngineEditEvent_Event_OnSketchModified()
        {
            IEngineEditProperties ep = new EngineEditorClass();
            ILayer m_pCurrentLayer = ep.TargetLayer;
            if (m_pCurrentLayer == null) return;
            IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pCurrentLayer;
            IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
            if (pDataset == null) return;
            pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;
            bool bHasUndos = false;
            pWorkspaceEdit.HasUndos(ref bHasUndos);
            if (bHasUndos)
                barButtonItem_EditUndo.Enabled = true;
            else
                barButtonItem_EditUndo.Enabled = false;

            bool bHasRedos = false;
            pWorkspaceEdit.HasRedos(ref bHasRedos);
            if (bHasRedos)
                barButtonItem_EditRedo.Enabled = true;
            else
                barButtonItem_EditRedo.Enabled = false;
        }

        void m_EngineEditEvent_Event_OnSelectionChanged()
        {
            if (m_EngineEditor.SelectionCount == 0)
            {
                
                barButtonItem_featureCopy.Enabled = false;
                barButtonItem_featureDelete.Enabled = false;
                barButtonItem_featurePaste.Enabled = false;
                barButtonItem_ClearSelection.Enabled = false;
                barButtonItem_featureCut.Enabled = false;
            }
            else
            {
                barButtonItem_featureCopy.Enabled = true;
                barButtonItem_featureDelete.Enabled = true;
                barButtonItem_featurePaste.Enabled = true;
                barButtonItem_ClearSelection.Enabled = true;
                barButtonItem_featureCut.Enabled = true;
            }

        }

        //void m_EngineEditEvent_Event_OnCurrentTaskChanged()
        //{
        //    if (m_EngineEditor.CurrentTask.Name == "Create New Feature")
        //    {
        //        tspCurrentTask.SelectedIndex = 0;
        //    }
        //    else if (m_EngineEditor.CurrentTask.Name == "Modify Feature")
        //    {
        //        tspCurrentTask.SelectedIndex = 1;
        //    }
        //}

        /// <summary>
        /// 启动编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_EditStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_axMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前文档中没有图层！");
                return;
            }

            if (m_EngineEditor.EditState == esriEngineEditState.esriEngineStateNotEditing)
            {
                //设置编辑的WorkSpace和编辑对象图层     
                ILayer currentLayer = m_axMapControl.Map.get_Layer(0);
                if (currentLayer is IFeatureLayer)
                {
                    IFeatureLayer featureLayer = currentLayer as IFeatureLayer;
                    IDataset dataset = featureLayer.FeatureClass as IDataset;
                    IWorkspace workspace = dataset.Workspace;
                    m_EngineEditor.StartEditing(workspace, m_axMapControl.Map);
                    m_EngineEditEvent_Event = m_EngineEditor as IEngineEditEvents_Event;
                    //m_EngineEditEvent_Event.OnCurrentTaskChanged += new IEngineEditEvents_OnCurrentTaskChangedEventHandler(m_EngineEditEvent_Event_OnCurrentTaskChanged);
                    m_EngineEditEvent_Event.OnSelectionChanged += new IEngineEditEvents_OnSelectionChangedEventHandler(m_EngineEditEvent_Event_OnSelectionChanged);
                    m_EngineEditEvent_Event.OnSketchModified += new IEngineEditEvents_OnSketchModifiedEventHandler(m_EngineEditEvent_Event_OnSketchModified);

                    barButtonItem_EditStart.Enabled = false;

                    barButtonItem_ClearSelection.Enabled = true;
                    barButtonItem_EditRedo.Enabled = true;
                    barButtonItem_EditSave.Enabled = true;
                    barButtonItem_EditStop.Enabled = true;
                    barButtonItem_EditUndo.Enabled = true;
                    barButtonItem_featureCopy.Enabled = true;
                    barButtonItem_featureCut.Enabled = true;
                    barButtonItem_featureDelete.Enabled = true;
                    barButtonItem_featureEdit.Enabled = true;
                    barButtonItem_featurePaste.Enabled = true;
                    barButtonItem_featureSketh.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_EditSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingSaveCommandClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 停止编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_EditStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_EngineEditor.HasEdits() == false)
                m_EngineEditor.StopEditing(false);
            else
            {
                if (MessageBox.Show("保存编辑?", "保存选项", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    m_EngineEditor.StopEditing(true);

                }
                else
                {
                    m_EngineEditor.StopEditing(false);
                }
            }

            barButtonItem_EditStart.Enabled = true;

            barButtonItem_ClearSelection.Enabled = false;
            barButtonItem_EditRedo.Enabled = false;
            barButtonItem_EditSave.Enabled = false;
            barButtonItem_EditStop.Enabled = false;
            barButtonItem_EditUndo.Enabled = false;
            barButtonItem_featureCopy.Enabled = false;
            barButtonItem_featureCut.Enabled = false;
            barButtonItem_featureDelete.Enabled = false;
            barButtonItem_featureEdit.Enabled = false;
            barButtonItem_featurePaste.Enabled = false;
            barButtonItem_featureSketh.Enabled = false;
        }
        #endregion

        #region 要素编辑
        /// <summary>
        /// 撒消编辑
        /// </summary>
        /// <param name="m_pMap">IMap 地图对象</param>
        public void MapUndoEdit(IMap m_pMap)
        {
            bool bHasUndos = false;
            pWorkspaceEdit.HasUndos(ref bHasUndos);
            if (bHasUndos)
                pWorkspaceEdit.UndoEditOperation();
            IActiveView pActiveView = (IActiveView)m_pMap;
            pActiveView.Refresh();
        }
        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="m_pMap">IMap 地图对象</param>
        public void MapRedoEdit(IMap m_pMap)
        {
            bool bHasRedos = false;
            pWorkspaceEdit.HasRedos(ref bHasRedos);
            if (bHasRedos)
                pWorkspaceEdit.RedoEditOperation();
            
            IActiveView pActiveView = (IActiveView)m_pMap;
            pActiveView.Refresh();
        }
        /// <summary>
        /// 添加要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featureSketh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingSketchToolClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 修改要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featureEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingEditToolClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 选择要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featureSelect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsSelectFeaturesToolClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }

        /// <summary>
        /// 复制要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featureCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingCopyCommandClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 剪切要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featureCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingCutCommandClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 黏贴要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featurePaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingPasteCommand();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 删除要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_featureDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsEditingClearCommandClass();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 清除要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_ClearSelection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pCmd = new ControlsClearSelectionCommand();
            pCmd.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = pCmd as ITool;
            pCmd.OnClick();
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_EditUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MapUndoEdit(m_axMapControl.Map);
            bool bHasRedos = false;
            pWorkspaceEdit.HasRedos(ref bHasRedos);
            if (bHasRedos)
                barButtonItem_EditRedo.Enabled = true;
            else
                barButtonItem_EditRedo.Enabled = false;

            bool bHasUndos = false;
            pWorkspaceEdit.HasUndos(ref bHasUndos);
            if (bHasUndos)
                barButtonItem_EditUndo.Enabled = true;
            else
                barButtonItem_EditUndo.Enabled = false;
        }
        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_EditRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MapRedoEdit(m_axMapControl.Map);
            bool bHasUndos = false;
            pWorkspaceEdit.HasUndos(ref bHasUndos);
            if (bHasUndos)
                barButtonItem_EditUndo.Enabled = true;
            else
                barButtonItem_EditUndo.Enabled = false;

            bool bHasRedos = false;
            pWorkspaceEdit.HasRedos(ref bHasRedos);
            if (bHasRedos)
                barButtonItem_EditRedo.Enabled = true;
            else
                barButtonItem_EditRedo.Enabled = false;
        }

        #endregion

        #region 全图切片裁剪
        /// <summary>
        /// 菜单项 全局切片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_fullScreenTile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //判断当前地图是否存在
            if (m_axMapControl.Map != null)
            {
                //判断地图中是否有图层
                if (m_axMapControl.Map.LayerCount > 0)
                {
                    //弹出参数设置界面
                    initFullScreenTiledockPanel();//先进行初始化
                    fullScreenTiledockPanel.Visibility = DockVisibility.Visible;
                    if (partScreenSetTiledockPanel.Visibility == DockVisibility.Visible)
                        partScreenSetTiledockPanel.Visibility = DockVisibility.AutoHide;
                    if (partScreenTileDragdockPanel.Visibility == DockVisibility.Visible)
                        partScreenTileDragdockPanel.Visibility = DockVisibility.AutoHide;
                }
                else
                {
                    MessageBox.Show("地图中无图层，请添加数据！");
                }
            }
            else
            {
                MessageBox.Show("地图不存在，请打开mxd文件！");
            }
        }

        /// <设置切片文件路径>
        /// 设置切片文件路径
        /// </设置切片文件路径>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEdit_fullTileMapOutputPath_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "保存切片目录";
            folderDlg.ShowNewFolderButton = true;
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                buttonEdit_fullTileMapOutputPath.Text = folderDlg.SelectedPath + System.IO.Path.DirectorySeparatorChar + m_axMapControl.Map.Name;
                if (Directory.Exists(buttonEdit_fullTileMapOutputPath.Text))
                {
                    MessageBox.Show(buttonEdit_fullTileMapOutputPath.Text, "目录已存在");
                    buttonEdit_fullTileMapOutputPath.Text = String.Empty;//已存在 所以路径为空
                    return;
                }
            }
        }
        /// <初始化 全局输出的参数设置面板>
        /// 初始化 全局输出的参数设置面板FullScreenTiledockPanel 主要恢复其值
        /// </初始化 全局输出的参数设置面板>
        void initFullScreenTiledockPanel()
        {
            textEdit_tileImgHeigh.Text = "256";
            textEdit_tileImgWidth.Text = "256";
            textEdit_newScaleValue.Text = "10000";

            listBoxControl_scaleLodInfors.Items.Clear();
            listBoxControl_scaleLodInfors.Items.Add("1:5000");
            listBoxControl_scaleLodInfors.Items.Add("1:2500");
            listBoxControl_scaleLodInfors.Items.Add("1:1000");
            m_tileLodScalesInfor.Clear();
            m_tileLodScalesInfor.Add(5000);
            m_tileLodScalesInfor.Add(2500);
            m_tileLodScalesInfor.Add(1000);

            textEdit_tileSchemeOrgX.Text = "0";
            textEdit_tileSchemeOrgY.Text = "0";

            radioGroup_tileStyle.EditValue = "Exploded";

            progressBarControl_tileCreateProgress.Position = 0;

            simpleButton_tileProStart.Enabled = true;
            simpleButton_tileProCancel.Enabled = false;

        }
        /// <添加新的比例尺度>
        /// 添加新的比例尺度
        /// </添加新的比例尺度>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private List<double> m_tileLodScalesInfor = new List<double>();
        private void simpleButton_addLod_Click(object sender, EventArgs e)
        {
            //预先排序 从小到大 1：10000在1：5000之前 
            int tmpLocation = 0;
            double scale = Convert.ToDouble(textEdit_newScaleValue.Text);
            if (m_tileLodScalesInfor.Contains(scale)) return;//已存在 就不在添加了 避免浪费
            for (int i = 0; i < m_tileLodScalesInfor.Count; i++)
            {
                if (scale < m_tileLodScalesInfor[i]) tmpLocation++;
                else break;
            }
            m_tileLodScalesInfor.Insert(tmpLocation, scale);
            listBoxControl_scaleLodInfors.Items.Insert(tmpLocation, "1:" + textEdit_newScaleValue.Text);

        }
        /// <移除选中的比例尺>
        /// 移除选中的比例尺
        /// </移除选中的比例尺>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_delLod_Click(object sender, EventArgs e)
        {
            if (listBoxControl_scaleLodInfors.ItemCount > 1)
            {
                int tmpLocate = listBoxControl_scaleLodInfors.SelectedIndex;
                if (tmpLocate < 0 || tmpLocate >= listBoxControl_scaleLodInfors.ItemCount) return;
                m_tileLodScalesInfor.RemoveAt(tmpLocate);
                listBoxControl_scaleLodInfors.Items.RemoveAt(tmpLocate);
            }
        }
        /// <summary>
        /// 向上移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_moveUp_Click(object sender, EventArgs e)
        {
            if (listBoxControl_scaleLodInfors.ItemCount > 1)
            {
                if (listBoxControl_scaleLodInfors.SelectedIndex > 0)
                    listBoxControl_scaleLodInfors.SelectedIndex--;
            }
        }
        /// <summary>
        /// 向下移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_moveDown_Click(object sender, EventArgs e)
        {
            if (listBoxControl_scaleLodInfors.ItemCount > 1)
            {
                if (listBoxControl_scaleLodInfors.SelectedIndex < listBoxControl_scaleLodInfors.ItemCount - 1)
                    listBoxControl_scaleLodInfors.SelectedIndex++;
            }
        }
        /// <自动计算切片原点>
        /// 根据图片大小和比例尺 自动计算 切片原点的 推荐值
        /// </自动计算切片原点>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_tileSchemeLoadDefValues_Click(object sender, EventArgs e)
        {
            if (m_axMapControl.Map == null || m_axMapControl.Map.LayerCount < 0)
            {
                MessageBox.Show("地图不存在或地图中无图层！");
                return;
            }
            if (m_tileLodScalesInfor.Count <= 0)
            {
                MessageBox.Show("请先设置比例尺级别");
                return;
            }
            double reslution = (25.39999918 / m_dpi) * m_tileLodScalesInfor[m_tileLodScalesInfor.Count - 1] / 1000;// 米/像素
            IEnvelope envlp = m_axMapControl.FullExtent;//地图的全局范围
            int imgW = 0;
            int imgH = 0;
            try
            {
                imgW = Convert.ToInt32(textEdit_tileImgWidth.Text);
                imgH = Convert.ToInt32(textEdit_tileImgHeigh.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("分块宽高必须为整数！");
                return;
            }
            int cols = Convert.ToInt32(Math.Ceiling(envlp.Width / imgW / reslution));
            int rows = Convert.ToInt32(Math.Ceiling(envlp.Height / imgH / reslution));
            double orgX = envlp.XMin - (cols * imgW * reslution - envlp.Width) / 2;
            double orgY = envlp.YMax + (rows * imgH * reslution - envlp.Height) / 2;//注意切片的原点在视图中的左上角，所以Y值有所区别
            textEdit_tileSchemeOrgX.Text = orgX.ToString();
            textEdit_tileSchemeOrgY.Text = orgY.ToString();

        }
        /// <summary>
        /// 开始切片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_tileProStart_Click(object sender, EventArgs e)
        {
            if (m_axMapControl.Map == null || m_axMapControl.Map.LayerCount < 0)
            {
                MessageBox.Show("地图不存在或地图中无图层！");
                return;
            }
            simpleButton_tileProStart.Enabled = false;
            simpleButton_tileProCancel.Enabled = true;

            int imgW = Convert.ToInt32(textEdit_tileImgWidth.Text);
            int imgH = Convert.ToInt32(textEdit_tileImgHeigh.Text);
            double orgX = Convert.ToDouble(textEdit_tileSchemeOrgX.Text);
            double orgY = Convert.ToDouble(textEdit_tileSchemeOrgY.Text);
            /////////////////////////////////////////////////////////////
            //计算范围 并进行切图
            bool imgSuccess = false;
            bool confSuccess = false;

            //时间计算
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            if (radioGroup_tileStyle.SelectedIndex == 0)
            {
                imgSuccess = ExportTileMap.ExportExplodedTileMap(m_axMapControl, progressBarControl_tileCreateProgress,
                                         m_tileLodScalesInfor, m_dpi, orgX, orgY,
                                         imgW, imgH, comboBoxEdit_ImgFormat.Text,
                                         buttonEdit_fullTileMapOutputPath.Text);
                confSuccess = ExportTileMap.SaveTileConfFiles(buttonEdit_fullTileMapOutputPath.Text, m_axMapControl.FullExtent,
                                        orgX, orgY, imgW, imgH, m_dpi, m_tileLodScalesInfor,
                                        "esriMapCacheStorageModeExploded", comboBoxEdit_ImgFormat.Text);
            }
            else if (radioGroup_tileStyle.SelectedIndex == 1)
            {
                imgSuccess = ExportTileMap.ExportCompactTileMap(m_axMapControl, progressBarControl_tileCreateProgress,
                                         m_tileLodScalesInfor, m_dpi, orgX, orgY,
                                         imgW, imgH, comboBoxEdit_ImgFormat.Text,
                                         buttonEdit_fullTileMapOutputPath.Text);
                confSuccess = ExportTileMap.SaveTileConfFiles(buttonEdit_fullTileMapOutputPath.Text, m_axMapControl.FullExtent,
                                        orgX, orgY, imgW, imgH, m_dpi, m_tileLodScalesInfor,
                                        "esriMapCacheStorageModeCompact", comboBoxEdit_ImgFormat.Text);
            }
            timer.Stop();//完成
            if (imgSuccess && confSuccess)
            {
                progressBarControl_tileCreateProgress.Position = 100;
                TimeSpan ts=timer.Elapsed;
                String timeStr = ts.Days + "天" + ts.Hours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
                MessageBox.Show("瓦片切割完成,耗时："+timeStr);
            }
            else
            {
                MessageBox.Show("瓦片切割失败！");
            }

            simpleButton_tileProStart.Enabled = true;
            simpleButton_tileProCancel.Enabled = false;
            progressBarControl_tileCreateProgress.Position = 0;
        }
        /// <summary>
        /// 取消切片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_tileProCancel_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 局部裁剪
        TileMapDragClipTool dragClipTool = new TileMapDragClipTool();
        PartTileDragXtraUserCtrl partTileDragCtrl = new PartTileDragXtraUserCtrl();
        PartTileInputXtraUserCtrl partTileInputCtrl = new PartTileInputXtraUserCtrl();
        /// <summary>
        /// 拉框实现局部裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_partScreenTileForDrag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //界面上操作
            if (fullScreenTiledockPanel.Visibility == DockVisibility.Visible)
                fullScreenTiledockPanel.Visibility = DockVisibility.AutoHide;
            if (partScreenSetTiledockPanel.Visibility == DockVisibility.Visible)
                partScreenSetTiledockPanel.Visibility = DockVisibility.AutoHide;
            partScreenTileDragdockPanel.Visibility = DockVisibility.Visible;

            //工具上的操作
            dragClipTool.OnCreate(m_axMapControl.Object);
            dragClipTool.StartTool(partTileDragCtrl);
            m_axMapControl.CurrentTool = dragClipTool;
        }
        /// <summary>
        /// 键盘输入实现局部裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_partScreenTileForInput_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //界面上操作
            if (fullScreenTiledockPanel.Visibility != DockVisibility.Hidden)
                fullScreenTiledockPanel.Visibility = DockVisibility.AutoHide;
            if (partScreenTileDragdockPanel.Visibility != DockVisibility.Hidden)
                partScreenTileDragdockPanel.Visibility = DockVisibility.AutoHide;
            partScreenSetTiledockPanel.Visibility = DockVisibility.Visible;
            m_axMapControl.CurrentTool = null;//不再拉框

            //工具上的操作
            partTileInputCtrl.SetToolsObject(m_axMapControl);
        }

        #endregion

        #region 瓦片数据操作
        /// <瓦片数据加载>
        /// 打开瓦片数据
        /// </瓦片数据加载>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_tileMapOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "打开瓦片地图所在目录";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                String path = folderBrowserDialog.SelectedPath;
                if (m_tileMapView.SetLayerPath(path) == true)
                    m_tileMapView.RefreshMap();
                else
                    MessageBox.Show("读取失败，请重新读取！");
            }
        }
        #endregion

        #region 其它功能

        /// <量测工具>
        /// 量测工具
        /// </量测工具>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_MeasureTools_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmdtools = new ControlsMapMeasureTool();
            cmdtools.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = cmdtools as ITool;
            cmdtools.OnClick();
        }
        /// <信息识别工具>
        /// 信息识别工具
        /// </信息识别工具>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_identify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmdtools = new ControlsMapIdentifyTool();
            cmdtools.OnCreate(m_axMapControl.Object);
            m_axMapControl.CurrentTool = cmdtools as ITool;
            cmdtools.OnClick();
        }

        #endregion

        #region 瓦片地图浏览操作
        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_tileMapZoomIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tileMapView.SetMapBrowserStyle(TileMapViewCtrl.BrowserStyle.MapZoomIn);
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_tileMapZoomOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tileMapView.SetMapBrowserStyle(TileMapViewCtrl.BrowserStyle.MapZoomOut);
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem__tileMapRestore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tileMapView.RestoreMap();
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_tileMapMove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tileMapView.SetMapBrowserStyle(TileMapViewCtrl.BrowserStyle.MapMove);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tileMapView.RefreshMap();
        }
        /// <summary>
        /// 瓦片地图显示时 是否显示格网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_tileShowGrid_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tileMapView.setShowGrid();
            m_tileMapView.RefreshMap();
        }

        #endregion

        #region 关于系统和作者
        /// <summary>
        /// 关于系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_aboutSys_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutSystemXtraForm aboutform = new AboutSystemXtraForm();
            aboutform.ShowDialog();
        }
        /// <summary>
        /// 关于作者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_aboutAthour_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutAuthorXtraForm aboutform = new AboutAuthorXtraForm();
            aboutform.ShowDialog();
        }
        #endregion

    }
}
