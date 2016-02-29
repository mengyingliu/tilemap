using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Collections.Generic;
using TileMapManager.EditTools;

namespace TileMapManager
{
    /// <summary>
    /// Summary description for TileMapDragClipTool.
    /// 用于拉框选取范围 同时改变外界界面 支持多拉框操作
    /// </summary>
    [Guid("a3f059df-f8db-409e-873f-36eece70127a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TileMapManager.TileMapPartCutTool")]
    public sealed class TileMapDragClipTool : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        public TileMapDragClipTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "CustomTools"; //localizable text 
            base.m_caption = "拉框裁剪工具";  //localizable text 
            base.m_message = "通过拉框，确定裁剪范围";  //localizable text
            base.m_toolTip = "选取裁剪范围";  //localizable text
            base.m_name = "CustomTools_GetClipEnvelop";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }


        #region Overridden Class Methods

        public override bool Enabled
        {
            get
            {
                if (m_hookHelper.ActiveView != null) 
                    return true;
                else return false;
            }
        } 

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            // TODO:  Add TileMapDragClipTool.OnCreate implementation
            m_hookHelper.Hook = hook;
            IntPtr pHandle = new IntPtr(m_hookHelper.ActiveView.ScreenDisplay.hWnd);
            m_axMapControl = System.Windows.Forms.Form.FromHandle(pHandle) as AxMapControl;//对这个地图控件对象操作，会直接反应到主窗体的地图控件上 
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add TileMapDragClipTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add TileMapDragClipTool.OnMouseDown implementation
            if (memRedy)
            {
                IEnvelope pEnv = m_axMapControl.TrackRectangle();//拉框操作
                if (pEnv.XMin == pEnv.XMax || pEnv.YMin == pEnv.YMax) return;//拉出的不是矩形 就不作存储
                m_clipAreas.Add(pEnv);//记录下拉框所得的矩形
                dragTimesTag++;

                IElement element = ElementFromEnveLop(pEnv);
                pGra.AddElement(element, 0);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                ///在属性信息界面中添加一个拉选的Area值
                m_partDragCtrl.InsertRect(pEnv, dragTimesTag);
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add TileMapDragClipTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add TileMapDragClipTool.OnMouseUp implementation
        }
        #endregion

        bool memRedy = false;
        IGraphicsContainer pGra = null;
        IActiveView pActiveView = null;
        IScreenDisplay screenDisp = null;
        private IHookHelper m_hookHelper = null;
        private AxMapControl m_axMapControl = null;
        public List<IEnvelope> m_clipAreas = new List<IEnvelope>();
        private PartTileDragXtraUserCtrl m_partDragCtrl = null;
        int dragTimesTag = 0;//用于标记当前拉的矩形的序号
        /// <summary>
        /// 启动拉框裁剪工具
        /// 主要用于传递界面参数
        /// </summary>
        /// <param name="dragInforView"></param>
        public void StartTool(PartTileDragXtraUserCtrl dragInforView)
        {
            //传界面和数据进来
            screenDisp = m_axMapControl.ActiveView.ScreenDisplay;
            IMap pMap = m_axMapControl.Map;
            pActiveView = pMap as IActiveView;
            pGra = pMap as IGraphicsContainer;
            memRedy = true;
            m_partDragCtrl = dragInforView;
            //将当前对象传到 拉框信息控件中 并初始化界面
            m_partDragCtrl.SetToolsObject(this,m_axMapControl);
        }
        /// <summary>
        /// 终止拉框裁剪操作
        /// </summary>
        public void StopTool()
        {
            //终止工具还原操作 进行更新
            pGra.DeleteAllElements();
            m_clipAreas.Clear();
            pActiveView.Refresh();
            memRedy = false;

            m_partDragCtrl.ClearRect();
            dragTimesTag = 0;
            m_axMapControl.CurrentTool = null;
        }

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
        /// <summary>
        /// 隐藏graphics层
        /// </summary>
        public void setGraphicsUnVisiable()
        {
            m_axMapControl.Map.ActiveGraphicsLayer.Visible = false;
        }
        /// <summary>
        /// 显示graphics层
        /// </summary>
        public void setGraphicsVisiable()
        {
            m_axMapControl.Map.ActiveGraphicsLayer.Visible = true;
        }
    }
}
