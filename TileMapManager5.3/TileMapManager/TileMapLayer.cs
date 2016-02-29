using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

namespace TileMapManager
{
    /// <summary>
    /// 层次模型的层信息
    /// </summary>
    public struct LodInfor 
    {
        public int LevelID;//层次级别
        public double Scale;//缩放比例
        public double Resolution;//实际距离与图片像素的比值 单位： 实际距离/像素
    };
    class TileMapLayer
    {
        MapRect m_TotalTileMapRect=new MapRect();//瓦片图层全图范围
        public MapRect TotalTileMapRect
        {
            get { return m_TotalTileMapRect; }
            set { m_TotalTileMapRect = value; }
        }
        double m_TileOriginX = 0;//瓦片切割时参照原点X
        double m_TileOriginY = 0;//瓦片切割时参照原点Y
        int m_TileRows = 0;//单个瓦片图片的行数
        int m_TileCols = 0;//单个瓦片图片的列数
        List<LodInfor> m_LodInfors = new List<LodInfor>();//层次模型信息
        int m_Dpi = 96;//DPI用于地图外界中计算Resolution 
        public int Dpi
        {
            get { return m_Dpi; }
        }
        
        string m_layerPath = "";//地图所在路径
        Bitmap m_TileImage = null;//缓存的瓦片图

        private Bitmap nonImg = null;

        //缓存格式 初始化为紧凑型 两种：紧凑型esriMapCacheStorageModeCompact和 松散型esriMapCacheStorageModeExploded
        String m_StorageFormat = "esriMapCacheStorageModeCompact";
        String m_CacheTileFormat = "jpg";//松散型瓦片格式 目前只支持 BMP PNG JEPG

        public TileMapLayer()
        {
            nonImg = new Bitmap(Properties.Resources.nonImg);
        }
        //获取最大比例尺
        public double GetMaxScale()
        {
            return m_LodInfors[0].Scale;
        }
        //获取最小比例尺
        public double GetMinScale()
        {
            return m_LodInfors[m_LodInfors.Count - 1].Scale;
        }
        /// <summary>
        /// 读取瓦片数据的信息
        /// </summary>
        /// <param name="tileMapPath">瓦片数据路径</param>
        /// <returns>是否成功读取</returns>
        public bool ReadTileInfor(string tileMapPath)
        {
            m_layerPath = tileMapPath;
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
                XmlNode envelopeNode=doc.SelectSingleNode("//EnvelopeN");
                XmlNodeList envelopeChildnodeList = envelopeNode.ChildNodes;
                foreach (XmlNode node in envelopeChildnodeList)
                {
                    if (node.Name == "XMin") m_TotalTileMapRect.xMin = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "YMin") m_TotalTileMapRect.yMin = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "XMax") m_TotalTileMapRect.xMax = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "YMax") m_TotalTileMapRect.yMax = Convert.ToDouble(node.InnerText);
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
                foreach (XmlNode node in lodInforsChildnodeList)
                {
                    if (node.Name == "LODInfo")
                    {
                        XmlNodeList lodInforNodeList = node.ChildNodes;
                        LodInfor lodInfor=new LodInfor();
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
        /// <summary>
        /// 根据比例尺和显示范围 获取显示的图片
        /// </summary>
        /// <param name="mapScale">当前地图的比例尺</param>
        /// <param name="mapRect">当前地图的显示范围</param>
        /// /// <param name="imgSize">需要获取图片的大小</param>
        /// <returns>对应范围的图片</returns>
        /// <param name="showGrid">是否显示格网</param>
        /// <returns></returns>
        public Bitmap GetLayerImage(double mapScale, MapRect mapRect,Size imgSize,bool showGrid)
        {
            if(m_LodInfors.Count<=0) return null;
            //确定对应范围所在的层次
            int curLevel = m_LodInfors[m_LodInfors.Count - 1].LevelID;
            for (int i = 0; i < m_LodInfors.Count; i++)
            {
                if (mapScale >= m_LodInfors[i].Scale)
                {
                    curLevel = m_LodInfors[i].LevelID;
                    break;
                }
            }
            //计算该区域左下角 右上角 对应的行列值
            int mapColsStart = Convert.ToInt32(Math.Floor((mapRect.xMin - m_TileOriginX) / (m_LodInfors[curLevel].Resolution * m_TileCols)));//起始列
            int mapRowsEnd = Convert.ToInt32(Math.Floor((m_TileOriginY - mapRect.yMin) / (m_LodInfors[curLevel].Resolution * m_TileRows)));//起始行
            int mapColsEnd = Convert.ToInt32(Math.Floor((mapRect.xMax - m_TileOriginX) / (m_LodInfors[curLevel].Resolution * m_TileCols)));//终止列
            int mapRowsStart = Convert.ToInt32(Math.Floor((m_TileOriginY - mapRect.yMax) / (m_LodInfors[curLevel].Resolution * m_TileRows)));//终止行
            //获取对应行列的瓦片并且拼接起来 成为一副大图
            m_TileImage = new Bitmap(m_TileCols * (mapColsEnd - mapColsStart + 1), m_TileRows * (mapRowsEnd - mapRowsStart + 1));
            Graphics backImgGraphic = Graphics.FromImage(m_TileImage);
            //backImgGraphic.Clear(Color.Pink);//清除颜色
            //画底图 用水印图片填底图    
            TextureBrush texture = new TextureBrush(nonImg,System.Drawing.Drawing2D.WrapMode.Tile);//平铺
            backImgGraphic.FillRectangle(texture,0,0,m_TileImage.Width,m_TileImage.Height);
            
            //m_TileImage.Save("D:\\360data\\重要数据\\桌面\\testshp\\TitleDataCompact\\图层\\test\\total.bmp");
            
            //获取最大的行列数 超出范围则可以不去读取对应位置的瓦片数据（一次过滤）
            int maxRow = Convert.ToInt32(Math.Floor((m_TileOriginY - m_TotalTileMapRect.yMin) / (m_LodInfors[curLevel].Resolution * m_TileRows)));//起始行
            int maxCol = Convert.ToInt32(Math.Floor((m_TotalTileMapRect.xMax - m_TileOriginX) / (m_LodInfors[curLevel].Resolution * m_TileCols)));//终止列

            if (m_StorageFormat == "esriMapCacheStorageModeCompact")//紧凑型数据读法
            {
                for (int j = mapRowsStart; j <= mapRowsEnd; j++)
                {
                    for (int i = mapColsStart; i <= mapColsEnd; i++)
                    {
                        //画瓦片地图 (超出瓦片范围 就不去读取数据 过滤)
                        if (!(i < 0 || j < 0 || i > maxCol || j > maxRow))
                        {
                            Image RowColImage = getCompactMapTile(curLevel, j, i);
                            if (RowColImage != null)
                                backImgGraphic.DrawImage(RowColImage, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);//将图片画到缓存图上

                        }
                        //画方格条纹
                        if(showGrid)
                            backImgGraphic.DrawRectangle(Pens.Blue, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);
                        //RowColImage.Save("D:\\360data\\重要数据\\桌面\\testshp\\TitleDataCompact\\图层\\test\\r" + j + "c" + i + ".bmp");
                    }
                }
            }
            else if (m_StorageFormat == "esriMapCacheStorageModeExploded")//松散型数据读法
            {
                for (int j = mapRowsStart; j <= mapRowsEnd; j++)
                {
                    for (int i = mapColsStart; i <= mapColsEnd; i++)
                    {
                        //画瓦片地图 (超出瓦片范围 就不去读取数据 过滤)
                        if (!(i < 0 || j < 0 || i > maxCol || j > maxRow))
                        {
                            Image RowColImage = getExplodedMapTile(curLevel, j, i);
                            if (RowColImage != null)
                                backImgGraphic.DrawImage(RowColImage, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);//将图片画到缓存图上
                        }
                        //画方格条纹
                        if (showGrid)
                            backImgGraphic.DrawRectangle(Pens.Blue, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);
                        //RowColImage.Save("D:\\360data\\重要数据\\桌面\\testshp\\TitleDataCompact\\图层\\test\\r" + j + "c" + i + ".bmp");
                    }
                }
            }

            //backImgGraphic.Dispose();
            //m_TileImage.Save("D:\\360data\\重要数据\\桌面\\testshp\\TitleDataCompact\\图层\\test\\total.bmp");
            //计算实际范围中 通过裁剪缓存图 获取实际需要的图片
            double startx = mapColsStart * m_LodInfors[curLevel].Resolution * m_TileCols + m_TileOriginX;
            double endy = m_TileOriginY - mapRowsStart * m_LodInfors[curLevel].Resolution * m_TileRows;
            double endx = (mapColsEnd+1) * m_LodInfors[curLevel].Resolution * m_TileCols + m_TileOriginX;
            double starty = m_TileOriginY - (mapRowsEnd + 1) * m_LodInfors[curLevel].Resolution * m_TileRows;

            int imgRectX = Convert.ToInt32((mapRect.xMin - startx) / (endx - startx) * m_TileImage.Width);
            int imgRectY = Convert.ToInt32((endy - mapRect.yMax) / (endy - starty) * m_TileImage.Height);
            int imgRectWid = Convert.ToInt32(mapRect.GetWidth() / (endx - startx) * m_TileImage.Width);
            int imgRectHeight = Convert.ToInt32(mapRect.GetHeigh() / (endy - starty) * m_TileImage.Height);
            //Bitmap mapImg = m_TileImage.Clone(new Rectangle(imgRectX, imgRectY, imgRectWid, imgRectHeight), m_TileImage.PixelFormat);
            Bitmap mapImg=new Bitmap(imgSize.Width,imgSize.Height);
            backImgGraphic = Graphics.FromImage(mapImg);
            backImgGraphic.DrawImage(m_TileImage, new Rectangle(0, 0, imgSize.Width, imgSize.Height), new Rectangle(imgRectX, imgRectY, imgRectWid, imgRectHeight),GraphicsUnit.Pixel);
            backImgGraphic.Dispose();
            return mapImg;
        }

        /// <summary>
        /// 读取松散型瓦片
        /// </summary>
        /// <param name="level">LOD</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns>对应行列的瓦片图片</returns>
        private Image getExplodedMapTile(int level, int row, int col)
        {
            String imgPath = GetExplodedTileImageName(row, col, level, m_layerPath, m_CacheTileFormat);
            if (System.IO.File.Exists(imgPath))
            {
                Image img = new Bitmap(imgPath);
                return img;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取松散型切片图片的名字 如果该路径不存在 则创建一个路径
        /// </summary>
        /// <param name="row">所在行</param>
        /// <param name="col">所在列</param>
        /// <param name="level">所在级别</param>
        /// <param name="orignalPath">原始路径</param>
        /// <param name="imgType">图片格式</param>
        /// <returns>图片路径</returns>
        public String GetExplodedTileImageName(int row, int col, int level, String orignalPath, String imgType)
        {
            String path = orignalPath + "\\_alllayers"
                          + "\\L" + level.ToString().PadLeft(2, '0')
                          + "\\R" + row.ToString("x").PadLeft(8, '0');
            if (!System.IO.Directory.Exists(path)) return null;//路径不存在
            String tileName = path + "\\C" + col.ToString("x").PadLeft(8, '0')
                          + "." + imgType;
            return tileName;
        }

        /// <summary>
        /// 读取瓦片
        /// </summary>
        /// <param name="level">LOD</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns>对应行列的瓦片图片</returns>
        private Image getCompactMapTile(int level, int row, int col)
        {
            byte[] result = null;
            System.IO.FileStream isBundlx = null, isBundle = null;
            try
            {
                //计算补充文件路径
                String bundlesDir = m_layerPath + "\\_alllayers";

                String l = "0" + level;
                int lLength = l.Length;
                if (lLength > 2)
                {
                    l = l.Substring(1);
                }
                l = "L" + l;

                int rGroup = 128 * (row / 128);
                String r = "000" + rGroup.ToString("X");//转为16进制
                int rLength = r.Length;
                if (rLength > 7)
                {
                    r = r.Substring(3);
                }
                else if (rLength > 4)
                {
                    r = r.Substring(rLength - 4);
                }
                r = "R" + r;

                int cGroup = 128 * (col / 128);
                String c = "000" + cGroup.ToString("X");//转为16进制
                int cLength = c.Length;
                if (cLength > 7)
                {
                    c = c.Substring(3);
                }
                else if (cLength > 4)
                {
                    c = c.Substring(cLength - 4);
                }
                c = "C" + c;

                String bundleBase = string.Format("{0}\\{1}\\{2}{3}", bundlesDir, l, r, c);
                String bundlxFileName = bundleBase + ".bundlx";
                String bundleFileName = bundleBase + ".bundle";

                //读取该瓦片在切片中的位置
                int index = 128 * (col - cGroup) + (row - rGroup);//计算对应切片中 所在的索引位置（相当于行列信息）
                isBundlx = new System.IO.FileStream(bundlxFileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                int skip = 16 + 5 * index;
                byte[] buffer = new byte[5];
                isBundlx.Seek((long)skip, System.IO.SeekOrigin.Begin);
                isBundlx.Read(buffer, 0, buffer.Length);
                //低位到高位转为高位到低位（5个字节反方向记录长度） 
                // 如ABCDE->EDCBA 所以是A*236^0+B*256^1+C*256^2+D*256^3+E*256^4 这里取&0xff不知道有什么意义
                long offset = (long)(buffer[0] & 0xff) + (long)(buffer[1] & 0xff)
                        * 256 + (long)(buffer[2] & 0xff) * 65536
                        + (long)(buffer[3] & 0xff) * 16777216
                        + (long)(buffer[4] & 0xff) * 4294967296L;

                isBundle = new System.IO.FileStream(bundleFileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                //读取该瓦片的图像数据
                byte[] lengthBytes = new byte[4];
                isBundle.Seek(offset, System.IO.SeekOrigin.Begin);
                isBundle.Read(lengthBytes, 0, lengthBytes.Length);
                //低位到高位转为高位到低位（4个字节反方向记录长度）
                int length = (int)(lengthBytes[0] & 0xff)
                        + (int)(lengthBytes[1] & 0xff) * 256
                        + (int)(lengthBytes[2] & 0xff) * 65536
                        + (int)(lengthBytes[3] & 0xff) * 16777216;
                result = new byte[length];
                isBundle.Seek(offset + lengthBytes.Length, System.IO.SeekOrigin.Begin);
                isBundle.Read(result, 0, length);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(result);

                return System.Drawing.Image.FromStream(ms);
            }
            catch (Exception /*ex*/)
            {
                return null;//如果没有读到图片 就返回无数据图片
                //throw (ex);
            }
            finally
            {
                if (isBundle != null)
                {
                    isBundle.Close();
                    isBundle.Dispose();
                }
                if (isBundlx != null)
                {
                    isBundlx.Close();
                    isBundlx.Dispose();
                }
            }
        }
    }
}
