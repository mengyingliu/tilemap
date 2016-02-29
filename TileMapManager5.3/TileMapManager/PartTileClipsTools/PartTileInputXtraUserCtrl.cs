using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml;
using DevExpress.XtraVerticalGrid.Rows;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

namespace TileMapManager.PartTileClipsTools
{
    public partial class PartTileInputXtraUserCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public PartTileInputXtraUserCtrl()
        {
            InitializeComponent();
        }

        private void buttonEdit_orgTileMapPath_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "选择切片目录";
            folderDlg.ShowNewFolderButton = true;
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                buttonEdit_orgTileMapPath.Text = folderDlg.SelectedPath;
                if (!Directory.Exists(buttonEdit_orgTileMapPath.Text))
                {
                    MessageBox.Show(buttonEdit_orgTileMapPath.Text, "该图层目录不存在！");
                    buttonEdit_orgTileMapPath.Text = String.Empty;//已存在 所以路径为空
                    return;
                }
                else
                {
                    ReadTileInfor(folderDlg.SelectedPath);
                    //更新界面
                    editorRow_mapMinX.Properties.Value = m_TotalTileMapRect.xMin;
                    editorRow_mapMinY.Properties.Value = m_TotalTileMapRect.yMin;
                    editorRow_mapMaxX.Properties.Value = m_TotalTileMapRect.xMax;
                    editorRow_mapMaxY.Properties.Value = m_TotalTileMapRect.yMax;
                    editorRow_orgPx.Properties.Value = m_TileOriginX;
                    editorRow_orgPy.Properties.Value = m_TileOriginY;
                    editorRow_imgHeigh.Properties.Value = m_TileRows;
                    editorRow_imgWidth.Properties.Value = m_TileCols;
                    editorRow_DPI.Properties.Value = m_Dpi;
                    editorRow_imgFormat.Properties.Value = m_CacheTileFormat;
                    if (m_StorageFormat == "esriMapCacheStorageModeCompact")
                        editorRow_clipFormat.Properties.Value = "紧凑型";
                    else if (m_StorageFormat == "esriMapCacheStorageModeExploded")
                        editorRow_clipFormat.Properties.Value = "松散型";
                    else editorRow_clipFormat.Properties.Value = "格式未知";

                    categoryRow_orgLODs.ChildRows.Clear();
                    for (int i = 0; i < m_LodInfors.Count; i++)
                    {
                        EditorRow editRow = new EditorRow();
                        editRow.Properties.Caption = "层次" + m_LodInfors[i].LevelID.ToString();
                        editRow.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                        editRow.Properties.Value = "1:" + m_LodInfors[i].Scale.ToString();
                        categoryRow_orgLODs.ChildRows.Add(editRow);
                    }
                }
            }
        }

        public MapRect m_TotalTileMapRect = new MapRect();//地图范围
        public double m_TileOriginX = 0;//切片原点X
        public double m_TileOriginY = 0;//切片原点Y
        public int m_TileCols = 0;//切片宽
        public int m_TileRows = 0;//切片高
        public int m_Dpi = 0;//像素信息
        public String m_StorageFormat = String.Empty;//裁剪格式
        public String m_CacheTileFormat = String.Empty;//图片格式
        public List<LodInfor> m_LodInfors = new List<LodInfor>();//层次信息
        public List<ESRI.ArcGIS.Geometry.IEnvelope> m_clipAreas = new List<ESRI.ArcGIS.Geometry.IEnvelope>();

        /// <summary>
        /// 读取瓦片配置信息
        /// </summary>
        /// <param name="tileMapPath"></param>
        /// <returns></returns>
        public bool ReadTileInfor(string tileMapPath)
        {
            try
            {
                //结构说明
                //路径示例："D:\\360data\\重要数据\\桌面\\testshp\\TitleDataCompact\\图层";
                //1.读取conf.cdi
                //2.读取conf.xml
                XmlDocument doc = new XmlDocument();
                //1.读取conf.cdi
                string configCdi = tileMapPath + "\\conf.cdi";
                doc.Load(configCdi);
                XmlNode envelopeNode = doc.SelectSingleNode("//EnvelopeN");
                XmlNodeList envelopeChildnodeList = envelopeNode.ChildNodes;
                foreach (XmlNode node in envelopeChildnodeList)
                {
                    if (node.Name == "XMin")
                    {
                        m_TotalTileMapRect.xMin = Convert.ToDouble(node.InnerText);
                    }
                    else if (node.Name == "YMin")
                    {
                        m_TotalTileMapRect.yMin = Convert.ToDouble(node.InnerText);
                    }
                    else if (node.Name == "XMax")
                    {
                        m_TotalTileMapRect.xMax = Convert.ToDouble(node.InnerText);
                    }
                    else if (node.Name == "YMax")
                    {
                        m_TotalTileMapRect.yMax = Convert.ToDouble(node.InnerText);
                    }
                }

                //2.读取conf.xml
                string configXml = tileMapPath + "\\conf.xml";
                doc.Load(configXml);
                //读取TileOrigin
                XmlNode tileOriginNode = doc.SelectSingleNode("//TileOrigin");
                XmlNodeList tileOriginChildnodeList = tileOriginNode.ChildNodes;
                foreach (XmlNode node in tileOriginChildnodeList)
                {
                    if (node.Name == "X") m_TileOriginX = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "Y") m_TileOriginY = Convert.ToDouble(node.InnerText);
                }
                //读取TileRows和TileCols
                XmlNode tileColsNode = doc.SelectSingleNode("//TileCols");
                m_TileCols = Convert.ToInt32(tileColsNode.InnerText);
                XmlNode tileRowsNode = doc.SelectSingleNode("//TileRows");
                m_TileRows = Convert.ToInt32(tileRowsNode.InnerText);
                XmlNode dpiNode = doc.SelectSingleNode("//DPI");
                m_Dpi = Convert.ToInt32(dpiNode.InnerText);
                //读取LodInfors
                XmlNode lodInforsNode = doc.SelectSingleNode("//LODInfos");
                XmlNodeList lodInforsChildnodeList = lodInforsNode.ChildNodes;

                m_LodInfors.Clear();
                foreach (XmlNode node in lodInforsChildnodeList)
                {
                    if (node.Name == "LODInfo")
                    {
                        XmlNodeList lodInforNodeList = node.ChildNodes;
                        LodInfor lodInfor = new LodInfor();
                        foreach (XmlNode lodInfornode in lodInforNodeList)
                        {
                            if (lodInfornode.Name == "LevelID") lodInfor.LevelID = Convert.ToInt32(lodInfornode.InnerText);
                            else if (lodInfornode.Name == "Scale") lodInfor.Scale = Convert.ToDouble(lodInfornode.InnerText);
                            else if (lodInfornode.Name == "Resolution") lodInfor.Resolution = Convert.ToDouble(lodInfornode.InnerText);
                        }
                        m_LodInfors.Add(lodInfor);
                    }
                }

                //读取瓦片格式
                XmlNode StorageFormatNode = doc.SelectSingleNode("//StorageFormat");
                m_StorageFormat = StorageFormatNode.InnerText;
                XmlNode CacheTileFormatNode = doc.SelectSingleNode("//CacheTileFormat");
                if (CacheTileFormatNode.InnerText == "BMP")
                    m_CacheTileFormat = "bmp";
                else if (CacheTileFormatNode.InnerText == "JPEG")
                    m_CacheTileFormat = "jpg";
                else if (CacheTileFormatNode.InnerText == "PNG8" || CacheTileFormatNode.InnerText == "PNG24" || CacheTileFormatNode.InnerText == "PNG32")
                    m_CacheTileFormat = "png";
                else
                {
                    MessageBox.Show("暂不支持混合型缓存数据！");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "读取瓦片数据出错");
                return false;
            }

        }

        public void InsertRect(ESRI.ArcGIS.Geometry.IEnvelope pEnv, int dragTimesTag)
        {
            CategoryRow cagRow = new CategoryRow();
            cagRow.Properties.Caption = "矩形" + dragTimesTag;

            EditorRow editRow = new EditorRow();
            editRow.Properties.Caption = "MinX";
            editRow.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            editRow.Properties.Value = pEnv.XMin;
            cagRow.ChildRows.Add(editRow);

            editRow = new EditorRow();
            editRow.Properties.Caption = "MinY";
            editRow.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            editRow.Properties.Value = pEnv.YMin;
            cagRow.ChildRows.Add(editRow);

            editRow = new EditorRow();
            editRow.Properties.Caption = "MaxX";
            editRow.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            editRow.Properties.Value = pEnv.XMax;
            cagRow.ChildRows.Add(editRow);

            editRow = new EditorRow();
            editRow.Properties.Caption = "MaxY";
            editRow.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            editRow.Properties.Value = pEnv.YMax;
            cagRow.ChildRows.Add(editRow);

            vGridControl_dragAreas.Rows.Add(cagRow);
        }

        /// <summary>
        /// 清空界面中的 拉框矩形
        /// </summary>
        public void ClearRect()
        {
            vGridControl_dragAreas.Rows.Clear();
        }

        /// <summary>
        /// 开始局部更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_StartUpdate_Click(object sender, EventArgs e)
        {
            if (buttonEdit_orgTileMapPath.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请先设置原切片路径！");
                return;
            }
            if (m_clipAreas.Count <= 0)
            {
                MessageBox.Show("请先输入切片范围！");
                return;
            }

            //时间计算
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            //1.隐藏拉的框 否则会在更新地图中留下痕迹
            m_axMapControl.Map.ActiveGraphicsLayer.Visible = false;
            //2.更新地图
            bool imgSuccess = false;
            if (m_StorageFormat == "esriMapCacheStorageModeExploded")
            {
                imgSuccess = ExportTileMap.ExportExplodedTileOfPartMap(m_axMapControl, progressBarControl_updateTiles,
                     m_LodInfors, m_Dpi, m_TileOriginX, m_TileOriginY,
                     m_TileCols, m_TileRows, m_CacheTileFormat,
                     buttonEdit_orgTileMapPath.Text, m_clipAreas);
            }
            else if (m_StorageFormat == "esriMapCacheStorageModeCompact")
            {
                imgSuccess = ExportTileMap.ExportCompactTileOfPartMap(m_axMapControl, progressBarControl_updateTiles,
                     m_LodInfors, m_Dpi, m_TileOriginX, m_TileOriginY,
                     m_TileCols, m_TileRows, m_CacheTileFormat,
                     buttonEdit_orgTileMapPath.Text, m_clipAreas);
            }
            else
            {
                MessageBox.Show("暂不支持此格式！");
                return;
            }

            //3.再现拉的框 以便查看更改
            m_axMapControl.Map.ActiveGraphicsLayer.Visible = true;
            timer.Stop();//完成

            if (imgSuccess)
            {
                progressBarControl_updateTiles.Position = 100;
                TimeSpan ts = timer.Elapsed;
                String timeStr = ts.Days + "天" + ts.Hours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
                MessageBox.Show("局部更新完毕,耗时：" + timeStr);
            }
            else
            {
                MessageBox.Show("局部更新失败！");
            }

            ////完成后，清理工具
            simpleButton_StartUpdate.Enabled = false;
            //simpleButton_CompleteUpdate.Enabled = true;
            progressBarControl_updateTiles.Position = 0;
        }

        /// <summary>
        /// 结束局部更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_CompleteUpdate_Click(object sender, EventArgs e)
        {
            IActiveView pActiveView = m_axMapControl.Map as IActiveView;
            IGraphicsContainer pGra = m_axMapControl.Map as IGraphicsContainer;
            pGra.DeleteAllElements();
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            m_dragTimesTag = 0;
            ClearRect();
            m_clipAreas.Clear();

            simpleButton_StartUpdate.Enabled = true;
            //simpleButton_CompleteUpdate.Enabled = false;
            progressBarControl_updateTiles.Position = 0;
        }

        ESRI.ArcGIS.Controls.AxMapControl m_axMapControl = null;
        /// <summary>
        /// 传递控件引用 便于管理 并初始化界面
        /// </summary>
        /// <param name="tileMapDragClipTool"></param>
        /// <param name="axMapControl"></param>
        public void SetToolsObject(ESRI.ArcGIS.Controls.AxMapControl axMapControl)
        {
            m_axMapControl = axMapControl;
            simpleButton_StartUpdate.Enabled = true;
            progressBarControl_updateTiles.Position = 0;
        }
        /// <summary>
        /// 添加切片区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int m_dragTimesTag = 0;
        private void simpleButton_AddArea_Click(object sender, EventArgs e)
        {
            try
            {
                double xmin = Convert.ToDouble(textEdit_XMin.Text.Trim());
                double ymin = Convert.ToDouble(textEdit_YMin.Text.Trim());
                double xmax = Convert.ToDouble(textEdit_XMax.Text.Trim());
                double ymax = Convert.ToDouble(textEdit_YMax.Text.Trim());
                if (xmin >= xmax || ymin >= ymax)
                {
                    MessageBox.Show("存在XMin>=XMax或者YMin>=YMax!");
                    return;
                }
                m_dragTimesTag++;
                IEnvelope envp = new EnvelopeClass();
                envp.PutCoords(xmin, ymin, xmax, ymax);
                m_clipAreas.Add(envp);

                InsertRect(envp, m_dragTimesTag);

                IElement element = ElementFromEnveLop(envp);
                IActiveView pActiveView = m_axMapControl.Map as IActiveView;
                IGraphicsContainer pGra = m_axMapControl.Map as IGraphicsContainer;
                pGra.AddElement(element, 0);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception)
            {
                MessageBox.Show("输入存在非数字，请检查后再添加！");
                return;
            }

        }
        /// <summary>
        /// 清空编辑框中的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_ClearEdit_Click(object sender, EventArgs e)
        {
            textEdit_XMax.Text = "0";
            textEdit_YMax.Text = "0";
            textEdit_XMin.Text = "0";
            textEdit_YMin.Text = "0";
        }

        /// <summary>
        /// 将矩形区域转化为要素
        /// </summary>
        /// <param name="penv"></param>
        /// <returns></returns>
        public IElement ElementFromEnveLop(IEnvelope penv)
        {
            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pEle = pRectangleEle as IElement;
            pEle.Geometry = penv;

            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 50;
            // 产生一个线符号对象
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 1.5;
            pOutline.Color = pColor;
            // 设置颜色属性(如果Transparency为0 表示透明 否则为不透明 没有半透明的效果)
            //pColor = new RgbColorClass();
            //pColor.Red = 255;
            //pColor.Green = 0;
            //pColor.Blue = 0;
            pColor.Transparency = 0;
            // 设置填充符号的属性
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;
            pFillShapeEle.Symbol.Color.Transparency = 0;
            return pFillShapeEle as IElement;
        }
    }
}
