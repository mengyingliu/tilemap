using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TileMapManager
{
    public partial class TileMapViewCtrl : UserControl
    {
        TileMapLayer layer = new TileMapLayer();
        double mapScale = 1;//比例尺
        double mapCenX = 100;
        double mapCenY = 100;
        private bool showGrid=false;//是否显示格网
        MapRect mapRect = new MapRect();//当前显示范围
        //耗时测试
        //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        public TileMapViewCtrl()
        {
            InitializeComponent();
            mapScale = 2000;
            mapCenX = 1000;
            mapCenY = 985;
            //watch.Reset();
        }
        /// <设置图层路径>
        /// 设置图层路径
        /// </设置图层路径>
        /// <param name="layerPath">地图路径</param>
        /// <returns>图层设置是否成功</returns>
        public bool SetLayerPath(String layerPath)
        {
            layer = new TileMapLayer();
            return layer.ReadTileInfor(layerPath);
        }
        /// <获取视图中 当前坐标和比例尺信息>
        /// 获取视图中 当前坐标和比例尺信息
        /// </获取视图中 当前坐标和比例尺信息>
        /// <param name="valx"></param>
        /// <param name="valy"></param>
        /// <param name="valscale"></param>
        public void GetCoordsAndScaleInfors(out double valx, out double valy, out double valscale)
        {
            valx = currentMapPoint.x;
            valy = currentMapPoint.y;
            valscale = mapScale;
        }

        #region 坐标转换及显示范围 及主要的算法

        /// <summary>
        /// 重新计算当前地图控件表示的地理范围
        /// </summary>
        private void ReCalculateMapRect()
        {
            double resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            mapRect.xMin = mapCenX - resolution * mapPicBox.Width / 2;
            mapRect.yMin = mapCenY - resolution * mapPicBox.Height / 2;
            mapRect.xMax = mapCenX + resolution * mapPicBox.Width / 2;
            mapRect.yMax = mapCenY + resolution * mapPicBox.Height / 2;
        }

        /// <summary>
        /// 窗口中坐标转为地图中坐标
        /// </summary>
        /// <param name="vx">窗口中点x</param>
        /// <param name="vy">窗口中点y</param>
        /// <param name="wx">对应地图点x</param>
        /// <param name="wy">对应地图点y</param>
        /// <returns></returns>
        public void CoordViewToMap(int vx, int vy, out double wx, out double wy)
        {
            double resolution = 0;
            if (layer == null)
                resolution = (25.39999918 / 96) * mapScale / 1000;
            else 
                resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            wx = mapCenX - (mapPicBox.Width / 2 - vx) * resolution;
            wy = mapCenY + (mapPicBox.Height / 2 - vy) * resolution;
        }
        private List<Point> CoordMapToViewPointList(List<MapPoint> mapListP)
        {
            double resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            List<Point> viewListP = new List<Point>();
            Point tmpViewP = new Point();
            foreach (MapPoint mapP in mapListP)
            {
                tmpViewP.X = Convert.ToInt32((mapP.x - mapCenX) / resolution + mapPicBox.Width / 2);
                tmpViewP.Y = Convert.ToInt32(mapPicBox.Height / 2-(mapP.y - mapCenY) / resolution);
                viewListP.Add(tmpViewP);
            }
            return viewListP;
        }
        /// <summary>
        /// 位移转换 由窗口上位移转为地图位移
        /// </summary>
        /// <param name="orgP">起始点</param>
        /// <param name="desP">终止点</param>
        /// <param name="dx">x偏移</param>
        /// <param name="dy">y偏移</param>
        public void TransWindToMap(Point orgP, Point desP, out double dx, out double dy)
        {
            double resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            dx = (orgP.X - desP.X) * resolution;
            dy = (desP.Y - orgP.Y) * resolution;
        }

        /// <summary>
        /// 图片移动 鼠标拖动瓦片时 将底图拖动显示 避免不断去读取对应的瓦片 同时延迟
        /// 未添加PictureBox时用法
        /// </summary>
        /// <param name="orgP">起始点</param>
        /// <param name="desP">终止点</param>
        /// <param name="imgRectToShow">从底图上截取的矩形范围</param>
        /// <param name="viewRectToShow">显示在窗口中的矩形范围</param>
        private void ImgTransImgToView(Point orgP, Point desP, out Rectangle imgRectToShow, out Rectangle viewRectToShow)
        {
            //视图上的位移
            int vdx = desP.X - orgP.X;
            int vdy = desP.Y - orgP.Y;
            //底图中的位移 现在的模型是 地图和位图一样大（减少存储空间 以提高速度）
            int idx = vdx;
            int idy = vdy;
            //图像裁剪区
            int istartx = 0, istarty = 0;//裁剪区 起始点
            int imgW = 0, imgH = 0;//裁剪区 高宽
            if (idx > 0)
            {
                istartx = 0;
                imgW = mapImg.Width - idx;
            }
            else
            {
                istartx = -idx;
                imgW = mapImg.Width + idx;
            }
            if (idy > 0)
            {
                istarty = 0;
                imgH = mapImg.Height - idy;
            }
            else
            {
                istarty = -idy;
                imgH = mapImg.Height + idy;
            }
            imgRectToShow = new Rectangle(istartx, istarty, imgW, imgH);

            //图片在 视图中大的 显示区域
            int vstartx = 0, vstarty = 0;//显示区 起始点
            int viewW = 0, viewH = 0;//显示区 高宽
            if (vdx > 0)
            {
                vstartx = vdx;
                viewW = mapPicBox.Width - vdx;
            }
            else
            {
                vstartx = 0;
                viewW = mapPicBox.Width + vdx;
            }
            if (vdy > 0)
            {
                vstarty = vdy;
                viewH = mapPicBox.Height - vdy;
            }
            else
            {
                vstarty = 0;
                viewH = mapPicBox.Height + vdy;
            }
            viewRectToShow = new Rectangle(vstartx, vstarty, viewW, viewH);

        }
        /// <summary>
        /// 获取当前屏幕起始点之间对应的图片
        /// </summary>
        /// <param name="startP">起点</param>
        /// <param name="endP">终点</param>
        /// <returns>对应区域的图片</returns>
        private Image ImgAreaImgToView(Rectangle viewRect)
        {
            if (viewRect.Width == 0 || viewRect.Height == 0) return null;
            Image tmpImg = new Bitmap(viewRect.Width, viewRect.Height);
            Graphics imgGraphic = Graphics.FromImage(tmpImg);
            imgGraphic.DrawImage(mapImg, new Rectangle(0, 0, tmpImg.Width, tmpImg.Height), viewRect.X, viewRect.Y, viewRect.Width, viewRect.Height, GraphicsUnit.Pixel);
            imgGraphic.Dispose();
            return tmpImg;
        }
        /// <summary>
        /// 获取屏幕上 任意两点构成的 矩形
        /// </summary>
        /// <param name="startxP">起点</param>
        /// <param name="endP">终点</param>
        /// <returns></returns>
        private Rectangle GetRectFromTwoPoint(Point startxP, Point endP)
        {
            int rectX = 0;
            int rectY = 0;
            int rectW = 0;
            int rectH = 0;
            if (startxP.X > endP.X)
            {
                rectX = endP.X;
                rectW = startxP.X - endP.X;
            }
            else
            {
                rectX = startxP.X;
                rectW = endP.X - startxP.X;
            }

            if (startxP.Y > endP.Y)
            {
                rectY = endP.Y;
                rectH = startxP.Y - endP.Y;
            }
            else
            {
                rectY = startxP.Y;
                rectH = endP.Y - startxP.Y;
            }
            return new Rectangle(rectX, rectY, rectW, rectH);
        }


          // 点的叉乘: AB * AC
        double cross(MapPoint A, MapPoint B, MapPoint C)
        {
            return (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x);
        }
        /// <summary>
        /// 计算多边形面积
        /// </summary>
        /// <param name="ployPointsList">多边形点的链表</param>
        /// <returns>面积</returns>
        private double GetPloygonArea(List<MapPoint> ployPointsList)
        {
            if (ployPointsList.Count < 3) return 0;

            double area = 0.0;

            int i;
            MapPoint temp;
            temp.x = temp.y = 0;//原点
            for (i = 0; i < ployPointsList.Count - 1; ++i)
            {
                area += cross(temp, ployPointsList[i], ployPointsList[i + 1]);
            }
            area += cross(temp, ployPointsList[ployPointsList.Count - 1], ployPointsList[0]);//首尾相连
            area = area / 2.0;        //注意要除以2
            return area > 0 ? area : -area;    //返回非负数
        }
        #endregion

        #region 瓦片图层的显示操作
        private Bitmap mapImg = null;//当前显示的地图
        /// <summary>
        /// 刷新地图
        /// </summary>
        public void RefreshMap()
        {
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// 复位地图
        /// </summary>
        public void RestoreMap()
        {
            MapRect totalRect = layer.TotalTileMapRect;
            mapCenX = (totalRect.xMax + totalRect.xMin) / 2;
            mapCenY = (totalRect.yMax + totalRect.yMin) / 2;
            double wk = totalRect.GetWidth() / mapPicBox.Width;
            double hk = totalRect.GetHeigh() / mapPicBox.Height;
            if (wk > hk)
                mapScale = wk * 1000 * layer.Dpi / 25.39999918;
            else
                mapScale = hk * 1000 * layer.Dpi / 25.39999918;
            RefreshMap();
        }

        /// <summary>
        /// 按固定比例缩小
        /// </summary>
        public void ZoomOut()
        {
            mapScale = mapScale * 1.25;
            double maxScale = layer.GetMaxScale();
            if (mapScale > maxScale * 4)//缩小太小 获取得到minscale/4作为当前比例尺
            {
                mapScale = maxScale * 4;
            }
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// 按固定比例放大
        /// </summary>
        public void ZoomIn()
        {
            mapScale = mapScale * 0.8;
            double minScale = layer.GetMinScale();
            if (mapScale < minScale / 4)//缩小太小 获取得到minscale/4作为当前比例尺
            {
                mapScale = minScale / 4;
            }
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// 按照指定比例尺显示
        /// </summary>
        /// <param name="scale">指定的比例尺</param>
        public void SetScale(double scale)
        {
            mapScale = scale;

            double minScale = layer.GetMinScale();
            if (mapScale < minScale / 4)//缩小太小 获取得到minscale/4作为当前比例尺
            {
                mapScale = minScale / 4;
            }
            double maxScale = layer.GetMaxScale();
            if (mapScale > maxScale * 4)//缩小太小 获取得到minscale/4作为当前比例尺
            {
                mapScale = maxScale * 4;
            }
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// 按照指定比例尺显示
        /// </summary>
        /// <param name="scale">指定的比例尺</param>
        public void SetScaleForMouseWeel(double scale, double curCenX, double curCenY)
        {
            double tmpMapScale = scale;
            double minScale = layer.GetMinScale();
            if (tmpMapScale < minScale / 4)//缩小太小 获取得到minscale/4作为当前比例尺
            {
                tmpMapScale = minScale / 4;
            }
            double maxScale = layer.GetMaxScale();
            if (tmpMapScale > maxScale * 4)//缩小太小 获取得到minscale/4作为当前比例尺
            {
                tmpMapScale = maxScale * 4;
            }
            mapCenX = curCenX - (curCenX - mapCenX) * scale / mapScale;
            mapCenY = curCenY - (curCenY - mapCenY) * scale / mapScale;
            mapScale = tmpMapScale;
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// 将地图中心移到指定目标位置
        /// </summary>
        /// <param name="desX">目标点x</param>
        /// <param name="desY">目标点y</param>
        public void MoveTo(double desX, double desY)
        {
            mapCenX = desX;
            mapCenY = desY;
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }

        /// <summary>
        /// 显示或取消格网显示
        /// </summary>
        public void setShowGrid()
        {
            showGrid = !showGrid;
        }
        #endregion

        #region 瓦片地图浏览状态操作 成为指针 移动 放大 缩小状态
        public enum BrowserStyle
        {
            MapDefaultPiont,//默认的指针状态
            MapMove,//移动
            MapZoomIn,//放大
            MapZoomOut//缩小
        }
        private BrowserStyle mapBrowserStyle = BrowserStyle.MapDefaultPiont;
        private Cursor CursorZoomIn = new Cursor(Properties.Resources.ZoomIn.ToBitmap().GetHicon());
        private Cursor CursorZoomOut = new Cursor(Properties.Resources.ZoomOut.ToBitmap().GetHicon());
        public void SetMapBrowserStyle(BrowserStyle style)
        {
            mapBrowserStyle = style;
            switch (mapBrowserStyle)
            {
                case BrowserStyle.MapDefaultPiont:
                    {
                        mapPicBox.Cursor = Cursors.Arrow;
                    }
                    break;
                case BrowserStyle.MapMove:
                    {
                        mapPicBox.Cursor = Cursors.Hand;
                    }
                    break;
                case BrowserStyle.MapZoomIn:
                    {
                        mapPicBox.Cursor = CursorZoomIn;
                    }
                    break;
                case BrowserStyle.MapZoomOut:
                    {
                        mapPicBox.Cursor = CursorZoomOut;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 事件响应 鼠标 及 视图

        /// <summary>
        /// 窗体大小发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileMapPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (mapPicBox.Width > 0 && mapPicBox.Height > 0)
            {
                if (layer != null)
                    RefreshMap();
            }
        }

        private Point dragOldPoint;//拖动前的鼠标位置
        bool dragTag = false;
        private void TileMapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mapBrowserStyle == BrowserStyle.MapMove)//移动地图
                {
                    dragTag = true;
                    dragOldPoint = e.Location;
                    moveLastPoint = e.Location;
                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomOut)//缩小
                {
                    dragOldPoint = e.Location;
                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomIn)//放大
                {
                    dragTag = true;
                    dragOldPoint = e.Location;
                    //rectLastPoint = e.Location;
                }

                if (m_MeasureState == MeasureStyle.LinesMeasure && mapBrowserStyle == BrowserStyle.MapDefaultPiont)
                {
                    MapPoint tmpMapP;
                    CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                    m_MeasureListPoint.Add(tmpMapP);
                    DrawFrontPage();
                }
                if (m_MeasureState == MeasureStyle.PloysMeasure && mapBrowserStyle == BrowserStyle.MapDefaultPiont)
                {
                    MapPoint tmpMapP;
                    CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                    m_MeasureListPoint.Add(tmpMapP);
                    DrawFrontPage();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                SetMapBrowserStyle(BrowserStyle.MapDefaultPiont);
            }
        }

        private void TileMapPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mapBrowserStyle == BrowserStyle.MapMove)//移动
                {
                    if (dragTag == true)
                    {
                        double dx = 0; double dy = 0;
                        TransWindToMap(dragOldPoint, e.Location, out dx, out dy);
                        MoveTo(mapCenX + dx, mapCenY + dy);
                        dragTag = false;

                    }
                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomOut)//缩小
                {
                    //缩小时 将视图中心移动到当前点 并按照一定比例缩小
                    CoordViewToMap(e.X, e.Y, out mapCenX, out mapCenY);
                    ZoomOut();
                    dragTag = false;

                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomIn)//放大
                {
                    //分为点击放大 和拉框放大
                    if (e.X == dragOldPoint.X && e.Y == dragOldPoint.Y)//还是这个点 表示点击放大
                    {
                        CoordViewToMap(e.X, e.Y, out mapCenX, out mapCenY);
                        ZoomIn();
                    }
                    else
                    {
                        //lastImg = null;//左键弹起后 将拉框用的赋为空
                        Rectangle zoomRect = GetRectFromTwoPoint(dragOldPoint, e.Location);
                        if (zoomRect.Width != 0 && zoomRect.Height != 0)
                        {
                            int cenx = (dragOldPoint.X + e.Location.X) / 2;
                            int ceny = (dragOldPoint.Y + e.Location.Y) / 2;
                            CoordViewToMap(cenx, ceny, out mapCenX, out mapCenY);
                            double tmpScale = 1;
                            if (mapPicBox.Width / zoomRect.Width < mapPicBox.Height / zoomRect.Height)
                            {
                                tmpScale = mapScale * zoomRect.Width / mapPicBox.Width;
                            }
                            else tmpScale = mapScale * zoomRect.Height / mapPicBox.Height;
                            SetScale(tmpScale);
                            mapPicBox.Image = null;//前图层不可见
                        }
                    }
                    dragTag = false;
                }

                DrawFrontPage();
            }

        }

        Point moveLastPoint;//用于移动
        //Point rectLastPoint;//用于拉框
        //Image lastImg = null;
        MapPoint currentMapPoint=new MapPoint();
        private void TileMapPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            CoordViewToMap(e.X, e.Y, out currentMapPoint.x, out currentMapPoint.y);

            if (mapBrowserStyle == BrowserStyle.MapMove)
            {
                if (mapImg != null && dragTag == true)
                {
                    Rectangle imgRect = new Rectangle();
                    Rectangle viewRect = new Rectangle();
                    ImgTransImgToView(moveLastPoint, e.Location, out imgRect, out viewRect);
                    Bitmap moveImg = new Bitmap(mapPicBox.Width, mapPicBox.Height);
                    Graphics imgGraphic = Graphics.FromImage(moveImg);
                    //imgGraphic.Clear(Color.White);//清除画笔颜色
                    imgGraphic.DrawImage(mapImg, viewRect, imgRect, GraphicsUnit.Pixel);
                    imgGraphic.Dispose();
                    mapPicBox.BackgroundImage = moveImg;
                }
            }
            else if (mapBrowserStyle == BrowserStyle.MapZoomIn)
            {
                if (dragTag == true)
                {
                    //画出对应的矩形框

                    //方法一：通过每次获取矩形范围的 原图 进行裁剪 贴到picturebox上 以实现消除上次的痕迹 效果不好 存在移位
                    //Graphics zoomRectGraphic = mapPicBox.CreateGraphics();
                    //Rectangle viewRect = GetRectFromTwoPoint(dragOldPoint, rectLastPoint);
                    //Image lastImg = ImgAreaImgToView(viewRect);
                    //if (lastImg != null)
                    //    zoomRectGraphic.DrawImage(lastImg, viewRect.X, viewRect.Y, viewRect.Width, viewRect.Height);
                    //zoomRectGraphic.DrawRectangle(Pens.Green, GetRectFromTwoPoint(dragOldPoint, e.Location));
                    //rectLastPoint = e.Location;

                    ////方法二  每次都在原底画的基础上画 实现消除原有痕迹
                    ////Image lastImg = (Image)mapImg.Clone();
                    ////watch.Reset();
                    ////watch.Start();
                    //if (lastImg == null)
                    //{
                    //    lastImg = new Bitmap(mapPicBox.Width, mapPicBox.Height);
                    //}
                    //Graphics zoomRectGraphic = Graphics.FromImage(lastImg);
                    //if (mapImg != null)
                    //    zoomRectGraphic.DrawImage(mapImg, 0, 0);//把底图画在临时图片上
                    //Rectangle dragRect = GetRectFromTwoPoint(dragOldPoint, e.Location);
                    ////注意：将该范围转换为图片上的范围
                    //RectangleF dragImgRectF = new RectangleF(dragRect.X * lastImg.Width / mapPicBox.Width, dragRect.Y * lastImg.Width / mapPicBox.Width, dragRect.Width * lastImg.Width / mapPicBox.Width, dragRect.Height * lastImg.Width / mapPicBox.Width);
                    //zoomRectGraphic.DrawRectangle(Pens.Green, dragImgRectF.X, dragImgRectF.Y, dragImgRectF.Width, dragImgRectF.Height);
                    //zoomRectGraphic.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), dragImgRectF);
                    ////watch.Stop();
                    ////zoomRectGraphic.DrawString(watch.ElapsedMilliseconds.ToString(), Font, Brushes.Red, 30, 30, StringFormat.GenericDefault);
                    ////mapPicBox.BackgroundImage.Save("D:\\360data\\重要数据\\桌面\\testshp\\TitleDataCompact\\图层\\123.bmp");
                    //zoomRectGraphic.Dispose();
                    //mapPicBox.BackgroundImage = lastImg;

                    //方法三 通过在新的位图上操作 并将位图设置为透明 然后将其值赋给 picturebox的image
                    Bitmap zoomImg = new Bitmap(mapPicBox.Width, mapPicBox.Height);
                    Graphics zoomRectGraphic = Graphics.FromImage(zoomImg);
                    Rectangle dragRect = GetRectFromTwoPoint(dragOldPoint, e.Location);
                    zoomRectGraphic.DrawRectangle(Pens.Green, dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
                    zoomRectGraphic.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), dragRect);
                    zoomRectGraphic.Dispose();
                    zoomImg.MakeTransparent();
                    mapPicBox.Image = zoomImg;
                }
            }
            else if (mapBrowserStyle == BrowserStyle.MapDefaultPiont)
            {
                if (m_MeasureState == MeasureStyle.LinesMeasure)
                {
                    if (m_MeasureListPoint.Count > 0)
                    {
                        if (m_MeasureListPoint.Count > 1)
                            m_MeasureListPoint.RemoveAt(m_MeasureListPoint.Count - 1);
                        MapPoint tmpMapP;
                        CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                        m_MeasureListPoint.Add(tmpMapP);
                        DrawFrontPage();
                    }
                }
                else if (m_MeasureState == MeasureStyle.PloysMeasure)
                {
                    if (m_MeasureListPoint.Count > 0)
                    {
                        if (m_MeasureListPoint.Count > 1)
                            m_MeasureListPoint.RemoveAt(m_MeasureListPoint.Count - 1);
                        MapPoint tmpMapP;
                        CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                        m_MeasureListPoint.Add(tmpMapP);
                        DrawFrontPage();
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (mapBrowserStyle == BrowserStyle.MapMove || mapBrowserStyle == BrowserStyle.MapZoomIn || mapBrowserStyle == BrowserStyle.MapZoomOut)
            {
                //原地放大 放大地点不变 这种方法存在一点误差 主要是计算获取时 存在一定的偏移
                double tmpScale = 1;
                if (e.Delta > 0)
                    tmpScale = mapScale * 1.2;
                else if (e.Delta < 0)
                    tmpScale = mapScale / 1.2;
                double curCenX = 0;
                double curCenY = 0;
                CoordViewToMap(e.X, e.Y, out curCenX, out curCenY);
                SetScaleForMouseWeel(tmpScale, curCenX, curCenY);
                DrawFrontPage();
            }
            base.OnMouseWheel(e);
        }
        /// <summary>
        /// 鼠标进入时 获取焦点 否则无法用滚轮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapPicBox_MouseEnter(object sender, EventArgs e)
        {
            if (!this.Focused)
                this.Focus();//为滚轮操作服务 获取焦点
        }
        /// <summary>
        /// 鼠标离开时 失去焦点 否则出现在外边也会响应滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapPicBox_MouseLeave(object sender, EventArgs e)
        {
            if (this.Focused)
                this.Parent.Focus();//离开时 就失去焦点
        }
        #endregion

        #region 右键菜单操作 地图浏览方式 放大 缩小 移动 复位 刷新 测量长度 测量面积

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapBrowserStyle(BrowserStyle.MapZoomIn);
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapBrowserStyle(BrowserStyle.MapZoomOut);
        }

        private void 移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapBrowserStyle(BrowserStyle.MapMove);
        }

        private void 复位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreMap();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshMap();
        }
        
        public enum MeasureStyle
        {
            NoMeasure,//没有任何测量任务
            LinesMeasure,//长度测量
            PloysMeasure//面积测量
        }
        private MeasureStyle m_MeasureState = MeasureStyle.NoMeasure;
        private List<MapPoint> m_MeasureListPoint = new List<MapPoint>();
        private Bitmap FrontImgMap = null;//用于测量画图的 前图（最终将 前图设为透明 前后图合并 以实现交互绘画的效果）
        private void 长度量算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_MeasureState == MeasureStyle.NoMeasure)//当前没有进行测量任务
            {
                LinesMeasureToolStripMenuItem.Text = "长度量算(结束)";
                m_MeasureState = MeasureStyle.LinesMeasure;
            }
            else if (m_MeasureState == MeasureStyle.LinesMeasure)//正在进行长度测量任务
            {
                m_MeasureListPoint.Clear();
                DrawFrontPage();

                LinesMeasureToolStripMenuItem.Text = "长度量算(开始)";
                m_MeasureState = MeasureStyle.NoMeasure;
            }
            else if (m_MeasureState == MeasureStyle.PloysMeasure)//正在进行面积测量任务
            {
                LinesMeasureToolStripMenuItem.Text = "长度量算(结束)";
                PolysMeasureToolStripMenuItem.Text = "面积量算(开始)";
                m_MeasureState = MeasureStyle.LinesMeasure;
                m_MeasureListPoint.Clear();
                DrawFrontPage();
            }       
        }
        private void 面积量算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_MeasureState == MeasureStyle.NoMeasure)//当前没有进行测量任务
            {
                PolysMeasureToolStripMenuItem.Text = "面积量算(结束)";
                m_MeasureState = MeasureStyle.PloysMeasure;
            }
            else if (m_MeasureState == MeasureStyle.LinesMeasure)//正在进行长度测量任务
            {
                LinesMeasureToolStripMenuItem.Text = "长度量算(开始)";
                PolysMeasureToolStripMenuItem.Text = "面积量算(结束)";
                m_MeasureState = MeasureStyle.PloysMeasure;
                m_MeasureListPoint.Clear();
                DrawFrontPage();
            }
            else if (m_MeasureState == MeasureStyle.PloysMeasure)//正在进行面积测量任务
            {
                m_MeasureListPoint.Clear();
                DrawFrontPage();
                PolysMeasureToolStripMenuItem.Text = "面积量算(开始)";
                m_MeasureState = MeasureStyle.NoMeasure;
            }
        }
        private void DrawFrontPage()
        {
            FrontImgMap = new Bitmap(mapPicBox.Width, mapPicBox.Height);
            Graphics frontImgGraphic = Graphics.FromImage(FrontImgMap);
            if (m_MeasureState == MeasureStyle.LinesMeasure)//将所有的线 画出来
            {
                if (m_MeasureListPoint.Count > 0)
                {
                    List<Point> tmpViewPoints = CoordMapToViewPointList(m_MeasureListPoint);
                    double lenth = 0;
                    Point []LinesPoints=new Point [m_MeasureListPoint.Count];
                    for(int i=0;i<tmpViewPoints.Count;i++)
                    {
                        LinesPoints[i]=tmpViewPoints[i];
                        if(i>0)
                        {
                            lenth += Math.Sqrt((m_MeasureListPoint[i].x - m_MeasureListPoint[i - 1].x) * (m_MeasureListPoint[i].x - m_MeasureListPoint[i - 1].x) + (m_MeasureListPoint[i].y - m_MeasureListPoint[i - 1].y) * (m_MeasureListPoint[i].y - m_MeasureListPoint[i - 1].y));
                        }
                        frontImgGraphic.DrawString(lenth.ToString("0.00"),this.Font, new SolidBrush(Color.Red), LinesPoints[i].X+5, LinesPoints[i].Y-10);
                        frontImgGraphic.DrawEllipse(new Pen(Color.Red, 2), LinesPoints[i].X-4, LinesPoints[i].Y-4,8,8);
                    }
                    if (tmpViewPoints.Count>1)
                        frontImgGraphic.DrawLines(new Pen(Color.RoyalBlue, 1),LinesPoints);
                    
                }
                
            }
            else if (m_MeasureState == MeasureStyle.PloysMeasure)//进行面积测量
            {
                if (m_MeasureListPoint.Count > 0)
                {
                    List<Point> tmpViewPoints = CoordMapToViewPointList(m_MeasureListPoint);
                    Point[] PloysPoints = new Point[m_MeasureListPoint.Count];
                    for (int i = 0; i < tmpViewPoints.Count; i++)
                    {
                        PloysPoints[i] = tmpViewPoints[i];
                        frontImgGraphic.DrawEllipse(new Pen(Color.Red, 2), PloysPoints[i].X - 4, PloysPoints[i].Y - 4, 8, 8);
                    }
                    if (tmpViewPoints.Count > 1)
                    {
                        frontImgGraphic.DrawPolygon(new Pen(Color.Green, 1), PloysPoints);
                        if (tmpViewPoints.Count > 2)
                        {
                            double area = GetPloygonArea(m_MeasureListPoint);
                            frontImgGraphic.FillPolygon(new SolidBrush(Color.FromArgb(100, Color.Blue)), PloysPoints);
                            frontImgGraphic.DrawString(area.ToString("0.00")+"平方米", this.Font, new SolidBrush(Color.Red), tmpViewPoints[tmpViewPoints.Count - 1].X + 5, tmpViewPoints[tmpViewPoints.Count - 1].Y - 10);
                        }
                    }
                }
            }
            frontImgGraphic.Dispose();
            FrontImgMap.MakeTransparent();
            mapPicBox.Image = FrontImgMap;
            
        }
        #endregion
    }
}
