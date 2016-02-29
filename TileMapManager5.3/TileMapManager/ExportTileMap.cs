using System.Drawing;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
namespace TileMapManager
{
    class ExportTileMap
    {
        /// <输出指定范围的地图到指定大小的图片中>
        /// 输出指定范围的地图到指定大小的图片中
        /// </输出指定范围的地图到指定大小的图片中>
        /// <param name="pMap">需转出的MAP</param>
        /// <param name="outRect">输出的图片大小</param>
        /// <param name="pEnvelope">指定的输出范围（为Envelope类型）</param>
        /// <returns>输出的Image 具体需要保存为什么格式，可通过Image对象来实现</returns>
        public static Image SaveCurrentToImage(IMapDocument doc, Size outRect, IEnvelope pEnvelope)
        {
            //赋值
            tagRECT rect = new tagRECT();
            rect.bottom = outRect.Height;
            rect.top = 0;
            rect.left = 0;
            rect.right = outRect.Width;
            try
            {
                //转换成activeView，若为ILayout，则将Layout转换为IActiveView
                IActiveView pActiveView = doc.ActiveView;
                // 创建图像,为24位色
                Image image = new Bitmap(outRect.Width, outRect.Height); //, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                // 填充背景色(白色)
                //g.FillRectangle(Brushes.White, 0, 0, outRect.Width, outRect.Height);
                //g.DrawRectangle(Pens.Green, 0, 0, outRect.Width, outRect.Height);
                int dpi = (int)(outRect.Width / pEnvelope.Width);
                ITrackCancel pTrackCancel = new TrackCancelClass();
                pActiveView.Output(g.GetHdc().ToInt32(), dpi, ref rect, pEnvelope, pTrackCancel);
                g.ReleaseHdc();//要在这里调用完后才能使用g 并且出图
                return image;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message + "将当前地图转出出错，原因未知", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <导出松散型瓦片>
        /// 导出松散型瓦片
        /// </导出松散型瓦片>
        /// <param name="mapCtrl">视图控件</param>
        /// /// <param name="prbCtl">进度条控件</param>
        /// <param name="lodScales">层次信息</param>
        /// <param name="dpi">分辨率</param>
        /// <param name="orgX">切片原点X</param>
        /// <param name="orgY">切片原点Y</param>
        /// <param name="imgW">图片宽</param>
        /// <param name="imgH">图片高</param>
        /// <param name="imgFormat">图片格式</param>
        /// <param name="mapOutputPath">地图输出路径</param>
        /// <returns>是否导出成功</returns>
        public static bool ExportExplodedTileMap(ESRI.ArcGIS.Controls.AxMapControl mapCtrl, 
                                            DevExpress.XtraEditors.ProgressBarControl prbCtl,
                                            List<double> lodScales,int dpi, 
                                            double orgX, double orgY,
                                            int imgW, int imgH, String imgFormat, String mapOutputPath)
        {
            //输出矩形
            tagRECT rect = new tagRECT();
            rect.bottom = imgH;
            rect.top = 0;
            rect.left = 0;
            rect.right = imgW;
            //转换成activeView，若为ILayout，则将Layout转换为IActiveView
            IActiveView pActiveView = mapCtrl.ActiveView;
            IEnvelope mapEnvp = mapCtrl.FullExtent;//全图范围
            double imgSum = 0;//瓦片总量
            int currentNum = 0;//当前生产瓦片量

            try
            {
                ITrackCancel pTrackCancel = new TrackCancelClass();
                for (int level = 0; level < lodScales.Count; level++)
                {
                    double scale = lodScales[level];
                    double reslution = (25.39999918 / dpi) * scale / 1000;// 米/像素

                    double tileWidth = imgW * reslution;
                    double tileHeigh = imgH * reslution;
                    /////////////////////////////////////////////////////////////////////////
                    //这里不能直接用mapEnvp的宽和高来计算 否则容易出现多一行或列的情况
                    int cols = Convert.ToInt32(Math.Ceiling((mapEnvp.XMax - orgX) / tileWidth));
                    int rows = Convert.ToInt32(Math.Ceiling((orgY - mapEnvp.YMin) / tileHeigh));

                    if (imgSum == 0)
                    {
                        for (int i = 0; i < lodScales.Count; i++)
                        {
                            imgSum += (lodScales[0] / lodScales[i]) * (lodScales[0] / lodScales[i]) * rows * cols;
                        }
                    }
                    //生成对应的范围 并将对应范围的切片保存到 指定文件目录下
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            //设置小切片范围
                            IEnvelope tileEnv = new EnvelopeClass();
                            tileEnv.PutCoords(orgX+j*tileWidth,orgY-(i+1)*tileHeigh,orgX+(j+1)*tileWidth,orgY-i*tileHeigh);
                            // 创建图像,为24位色
                            Image image = new Bitmap(imgW, imgH);
                            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                            // 填充背景色(青色)
                            g.FillRectangle(Brushes.White, 0, 0, imgW, imgH);
                            //g.DrawRectangle(Pens.Green, 0, 0, imgW, imgH);
                            pActiveView.Output(g.GetHdc().ToInt32(), dpi, ref rect, tileEnv, pTrackCancel);
                            g.ReleaseHdc();//要在这里调用完后才能使用g 并且出图
                            //根据行列计算图片所在路径 并保存该路径
                            String tileImageName = GetExplodedTileImageName(i, j, level, mapOutputPath,imgFormat);
                            //根据各种格式保存
                            if(imgFormat=="bmp")
                            {
                                image.Save(tileImageName,System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            else if (imgFormat == "jpg")
                            {
                                image.Save(tileImageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else if (imgFormat == "png")
                            {
                                image.Save(tileImageName, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            else
                            {
                                image.Save(tileImageName);
                            }
                            g.Dispose();
                            currentNum++;
                            prbCtl.Position = Convert.ToInt32(currentNum*100 / imgSum);
                        }
                    }
                }
                return true;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message + "将当前地图转出出错，原因未知", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <获取松散型切片图片的名字>
        /// 获取松散型切片图片的名字 如果该路径不存在 则创建一个路径
        /// </获取松散型切片图片的名字>
        /// <param name="row">所在行</param>
        /// <param name="col">所在列</param>
        /// <param name="level">所在级别</param>
        /// <param name="orignalPath">原始路径</param>
        /// <param name="imgType">图片格式</param>
        /// <returns>图片路径</returns>
        public static String GetExplodedTileImageName(int row,int col,int level,String orignalPath,String imgType)
        {
            String path = orignalPath + "\\_alllayers" 
                          + "\\L" + level.ToString().PadLeft(2, '0')
                          + "\\R" + row.ToString("x").PadLeft(8, '0');
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            String tileName = path + "\\C" + col.ToString("x").PadLeft(8, '0')
                          + "."+ imgType;
            return tileName;
        }


        /// <导出紧凑型瓦片>
        /// 导出紧凑型瓦片
        /// </导出紧凑型瓦片>
        /// <param name="mapCtrl">视图控件</param>
        /// /// <param name="prbCtl">进度条控件</param>
        /// <param name="lodScales">层次信息</param>
        /// <param name="dpi">分辨率</param>
        /// <param name="orgX">切片原点X</param>
        /// <param name="orgY">切片原点Y</param>
        /// <param name="imgW">图片宽</param>
        /// <param name="imgH">图片高</param>
        /// <param name="imgFormat">图片格式</param>
        /// <param name="mapOutputPath">地图输出路径</param>
        /// <returns>是否导出成功</returns>
        public static bool ExportCompactTileMap(ESRI.ArcGIS.Controls.AxMapControl mapCtrl,
                                            DevExpress.XtraEditors.ProgressBarControl prbCtl,
                                            List<double> lodScales, int dpi,
                                            double orgX, double orgY,
                                            int imgW, int imgH, String imgFormat, String mapOutputPath)
        {
            //输出矩形
            tagRECT rect = new tagRECT();
            rect.bottom = imgH;
            rect.top = 0;
            rect.left = 0;
            rect.right = imgW;
            //转换成activeView，若为ILayout，则将Layout转换为IActiveView
            IActiveView pActiveView = mapCtrl.ActiveView;
            IEnvelope mapEnvp = mapCtrl.FullExtent;//全图范围
            double imgSum = 0;//瓦片总量
            int currentNum = 0;//当前生产瓦片量

            try
            {
                ITrackCancel pTrackCancel = new TrackCancelClass();
                for (int level = 0; level < lodScales.Count; level++)
                {
                    double scale = lodScales[level];
                    double reslution = (25.39999918 / dpi) * scale / 1000;// 米/像素

                    double tileWidth = imgW * reslution;
                    double tileHeigh = imgH * reslution;

                    /////////////////////////////////////////////////////////////////////////
                    //计算总行数 列数
                    //这里不能直接用mapEnvp的宽和高来计算 否则容易出现多一行或列的情况 
                    int cols = Convert.ToInt32(Math.Ceiling((mapEnvp.XMax - orgX) / tileWidth));
                    int rows = Convert.ToInt32(Math.Ceiling((orgY - mapEnvp.YMin) / tileHeigh));
                    if (imgSum == 0)
                    {
                        for (int i = 0; i < lodScales.Count; i++)
                        {
                            imgSum += (lodScales[0] / lodScales[i]) * (lodScales[0] / lodScales[i]) * cols * rows ;
                        }
                    }
                    //要对剩下的进行分块存储 128 x 128为一个bundle中
                    int bundleCols = cols / 128 + 1;
                    int bundleRows = rows / 128 + 1;

                    for (int br = 0; br < bundleRows; br++)
                    {
                        for (int bl = 0; bl < bundleCols; bl++)
                        {
                            //每一个都是一个bundle文件

                            //1.自动生成对应的文件夹路径和文件名
                            String baseName = GetCompactBundlePathName(br, bl, level, mapOutputPath);
                            String bundleName = baseName + ".bundle";
                            String bundlxName = baseName + ".bundlx";
                                //声明对应的流 并打开对应文件
                            System.IO.FileStream bundleFileStream=new System.IO.FileStream(bundleName,FileMode.OpenOrCreate);
                            System.IO.FileStream bundlxFileStream=new System.IO.FileStream(bundlxName,FileMode.OpenOrCreate);
                            //初始化 bundlxFileStream 文件流 起始的16个字节无效 （暂时不可读取）
                            byte[] wipeHeaderData = new byte[16] { 3, 0, 0, 0, 16, 0, 0, 0, 0, 64, 0, 0, 5, 0, 0, 0 };
                            bundlxFileStream.Write(wipeHeaderData, 0, wipeHeaderData.Length);
                            //2.声明索引文件记录值 并将其初始化为0 这里记录的是 各个切片的长度 不是真正的索引值
                            //计算时 按照行优先 存储时按照列优先
                            int[,] indexData = new int[128, 128];//初始化的索引文件 
                            indexData[0, 0] = 0;
                            int offsetLabel = 0;//跟踪偏移量
                            //3.计算并写bundle文件 格式 长度+图片
                            for (int i = 0; i < 128; i++)
                            {
                                for (int j = 0; j < 128; j++)
                                {
                                    indexData[i, j] = offsetLabel;//记录下来偏移量 开始的位置
                                    //获取该切片的图片信息
                                    //a.计算切片所在的行列
                                    int tileRow = 128 * br + i;
                                    int tileCol = 128 * bl + j;
                                    if (tileRow >= rows || tileCol >= cols)//超出范围 直接把长度赋为0 偏移量加4 
                                    {
                                        byte[] lengthBytes = BitConverter.GetBytes(0);//不知道为什么 不用转换都可以
                                        bundleFileStream.Write(lengthBytes, 0, 4);
                                        offsetLabel += 4;
                                    }
                                    else 
                                    {
                                        //b.设置小切片范围
                                        IEnvelope tileEnv = new EnvelopeClass();
                                        tileEnv.PutCoords(orgX + tileCol * tileWidth, orgY - (tileRow + 1) * tileHeigh, orgX + (tileCol + 1) * tileWidth, orgY - tileRow * tileHeigh);
                                        //c.获取图片数据
                                        Image image = new Bitmap(imgW, imgH);
                                        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                                        g.FillRectangle(Brushes.White, 0, 0, imgW, imgH);
                                        pActiveView.Output(g.GetHdc().ToInt32(), dpi, ref rect, tileEnv, pTrackCancel);
                                        g.ReleaseHdc();
                                        //d.写bundle文件  数据长度+图片数据 （低位到高位写长度 长度为4）
                                        byte[] imagedata = null;
                                        //图片数据根据各种格式保存
                                        if (imgFormat == "bmp")
                                        {
                                            //长度数据
                                            MemoryStream ms = new MemoryStream();
                                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                                            imagedata = ms.GetBuffer();//图片数据
                                            ms.Close();

                                        }
                                        else if (imgFormat == "jpg")
                                        {
                                            MemoryStream ms = new MemoryStream();
                                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            imagedata = ms.GetBuffer();//图片数据
                                            ms.Close();
                                        }
                                        else if (imgFormat == "png")
                                        {
                                            MemoryStream ms = new MemoryStream();
                                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                            imagedata = ms.GetBuffer();//图片数据
                                            ms.Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show("暂时不支持混合格式！");
                                        }
                                        //写数据 
                                        byte[] lengthBytes = BitConverter.GetBytes(imagedata.Length);//不知道为什么 不用转换都可以
                                        bundleFileStream.Write(lengthBytes, 0, 4);
                                        bundleFileStream.Write(imagedata, 0, imagedata.Length);
                                        offsetLabel += 4 + imagedata.Length;//移动偏移量
                                        g.Dispose();
                                        currentNum++;
                                        prbCtl.Position = Convert.ToInt32(currentNum * 100 / imgSum);
                                    }
                                }
                            }
                            //关闭bundle文件
                            bundleFileStream.Close();
                            //写入索引文件
                            for (int i = 0; i < 128; i++)
                            {
                                for (int j = 0; j < 128; j++)
                                {
                                    byte[] lenBytes = BitConverter.GetBytes(indexData[j,i]);
                                    if (lenBytes.Length < 5)
                                    {
                                        bundlxFileStream.Write(lenBytes, 0, lenBytes.Length);
                                        bundlxFileStream.WriteByte(0);
                                    }
                                    else
                                        bundlxFileStream.Write(lenBytes, 0, 5);
                                }
                            }
                            byte[] wipeEnderData = new byte[16] { 0, 0, 0, 0, 16, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0 };
                            bundlxFileStream.Write(wipeEnderData, 0, wipeEnderData.Length);
                            bundlxFileStream.Close();
                        }
                    }
                }
                return true;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message + "将当前地图转出出错，原因未知", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <获取紧凑型切片文件的名字>
        /// 获取紧凑型切片文件的名字 如果该路径不存在 则创建一个路径
        /// </获取紧凑型切片文件的名字>
        /// <param name="row">bundle所在行</param>
        /// <param name="col">bundle所在列</param>
        /// <param name="level">bundle所在级别</param>
        /// <param name="orignalPath">原始路径</param>
        /// <returns>切片文件夹路径</returns>
        public static String GetCompactBundlePathName(int bundleRow, int bundleCol, int level, String orignalPath)
        {
            String bundlesDir = orignalPath + "\\_alllayers"
                          + "\\L" + level.ToString().PadLeft(2, '0');
            if (!Directory.Exists(bundlesDir))
                Directory.CreateDirectory(bundlesDir);

            int rGroup = 128 * bundleRow;
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

            int cGroup = 128 * bundleCol;
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
            String bundleBase = string.Format("{0}\\{1}{2}", bundlesDir, r,c);
            return bundleBase;
        }


        /// <生成瓦片配置文件>
        /// 生成瓦片配置文件 conf.xml和conf.cdi
        /// 这里不知道为什么 存在一定的问题 虽然可以读取 但是和arcserver存在一定的区别
        /// </生成瓦片配置文件>
        /// <param name="layerPath">图层路径</param>
        /// <param name="totalMapEnvelop">图层最大范围</param>
        /// <param name="tileOriginX">切片原点x</param>
        /// <param name="tileOriginY">切片原点y</param>
        /// <param name="tileCols">切片列数</param>
        /// <param name="tileRows">切片行数</param>
        /// <param name="dpi">分辨率</param>
        /// <param name="lodScales">LOD层次信息</param>
        /// <param name="storageFormat">存储格式 紧凑型？松散型</param>
        /// <param name="cacheTileFormat">瓦片格式</param>
        /// <returns></returns>
        public static bool SaveTileConfFiles(String layerPath, IEnvelope totalMapEnvelop, 
                                        double tileOriginX, double tileOriginY,
                                        int tileCols, int tileRows, int dpi,
                                        List<double> lodScales, 
                                        String storageFormat, String cacheTileFormat)
        {
            //1.读取并修改内置cdi模版
            string configCdi = layerPath + "\\conf.cdi";
            XmlDocument cdiDoc = new XmlDocument();
            cdiDoc.LoadXml(Properties.Resources.confCdi);
            XmlNode envelopeNode = cdiDoc.SelectSingleNode("//EnvelopeN");
            XmlNodeList envelopeChildnodeList = envelopeNode.ChildNodes;
            foreach (XmlNode node in envelopeChildnodeList)
            {
                if (node.Name == "XMin") node.InnerText = totalMapEnvelop.XMin.ToString();
                else if (node.Name == "YMin") node.InnerText = totalMapEnvelop.YMin.ToString();
                else if (node.Name == "XMax") node.InnerText = totalMapEnvelop.XMax.ToString();
                else if (node.Name == "YMax") node.InnerText = totalMapEnvelop.YMax.ToString();
            }
            cdiDoc.Save(configCdi);

            //2.读取并修改内置xml模版
            string configXml = layerPath + "\\conf.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Properties.Resources.confXml);
            //修改TileOrigin
            XmlNode tileOriginNode = xmlDoc.SelectSingleNode("//TileOrigin");
            XmlNodeList tileOriginChildnodeList = tileOriginNode.ChildNodes;
            foreach (XmlNode node in tileOriginChildnodeList)
            {
                if (node.Name == "X") node.InnerText = tileOriginX.ToString();
                else if (node.Name == "Y") node.InnerText = tileOriginY.ToString();
            }

            //读取TileRows和TileCols
            XmlNode tileColsNode = xmlDoc.SelectSingleNode("//TileCols");
            tileColsNode.InnerText = tileCols.ToString();
            XmlNode tileRowsNode = xmlDoc.SelectSingleNode("//TileRows");
            tileRowsNode.InnerText = tileRows.ToString();
            XmlNode dpiNode = xmlDoc.SelectSingleNode("//DPI");
            dpiNode.InnerText = dpi.ToString();
            //读取LodInfors
            XmlNode lodInforsNode = xmlDoc.SelectSingleNode("//LODInfos");
            lodInforsNode.RemoveAll();
            for (int i = 0; i < lodScales.Count; i++)
            {
                XmlNode lodNode = xmlDoc.CreateElement("LODInfo");

                XmlNode lodInforNode = xmlDoc.CreateElement("LevelID");
                lodInforNode.InnerText = i.ToString();
                lodNode.AppendChild(lodInforNode);

                lodInforNode = xmlDoc.CreateElement("Scale");
                lodInforNode.InnerText = lodScales[i].ToString();
                lodNode.AppendChild(lodInforNode);

                lodInforNode = xmlDoc.CreateElement("Resolution");
                double reslution = (25.39999918 / dpi) * lodScales[i] / 1000;// 米/像素
                lodInforNode.InnerText = reslution.ToString();
                lodNode.AppendChild(lodInforNode);

                lodInforsNode.AppendChild(lodNode);
            }

            //读取瓦片格式
            XmlNode StorageFormatNode = xmlDoc.SelectSingleNode("//StorageFormat");
            StorageFormatNode.InnerText = storageFormat;
            XmlNode CacheTileFormatNode = xmlDoc.SelectSingleNode("//CacheTileFormat");
            if (cacheTileFormat == "bmp") CacheTileFormatNode.InnerText = "BMP";
            else if (cacheTileFormat == "jpg") CacheTileFormatNode.InnerText = "JPEG";
            else if (cacheTileFormat == "png") CacheTileFormatNode.InnerText = "PNG24";
            else
            {
                MessageBox.Show("暂不支持此类型缓存数据！");
                return false;
            }
            xmlDoc.Save(configXml);
            return true;
        }

        /// <局部导出松散型瓦片>
        /// 局部导出松散型瓦片
        /// </局部导出松散型瓦片>
        /// <param name="mapCtrl">视图控件</param>
        /// /// <param name="prbCtl">进度条控件</param>
        /// <param name="lodInfors">层次信息</param>
        /// <param name="dpi">分辨率</param>
        /// <param name="orgX">切片原点X</param>
        /// <param name="orgY">切片原点Y</param>
        /// <param name="imgW">图片宽</param>
        /// <param name="imgH">图片高</param>
        /// <param name="imgFormat">图片格式</param>
        /// <param name="mapOutputPath">地图输出路径</param>
        /// <param name="areaList">需要裁剪的区域</param>
        /// <returns>是否导出成功</returns>
        public static bool ExportExplodedTileOfPartMap(ESRI.ArcGIS.Controls.AxMapControl mapCtrl,
                                            DevExpress.XtraEditors.ProgressBarControl prbCtl,
                                            List<LodInfor> lodInfors, int dpi,
                                            double orgX, double orgY,
                                            int imgW, int imgH, String imgFormat, String mapOutputPath,List<IEnvelope> areaList)
        {
            //输出矩形
            tagRECT rect = new tagRECT();
            rect.bottom = imgH;
            rect.top = 0;
            rect.left = 0;
            rect.right = imgW;
            //转换成activeView，若为ILayout，则将Layout转换为IActiveView
            IActiveView pActiveView = mapCtrl.ActiveView;
            IEnvelope mapEnvp = mapCtrl.FullExtent;//全图范围
            double sum = lodInfors.Count * areaList.Count;//总计的区域个数
            double num = 0;//当前导出的区域个数
            try
            {
                ITrackCancel pTrackCancel = new TrackCancelClass();
                for (int level = 0; level < lodInfors.Count; level++)
                {
                    double reslution = lodInfors[level].Resolution;// 米/像素

                    double tileWidth = imgW * reslution;
                    double tileHeigh = imgH * reslution;


                    for (int areaIndex = 0; areaIndex < areaList.Count; areaIndex++)
                    {
                        IEnvelope areaEnv = areaList[areaIndex];
                        //范围超出检测
                        if(areaEnv.XMin<mapEnvp.XMin)areaEnv.XMin=mapEnvp.XMin;
                        if(areaEnv.YMin<mapEnvp.YMin)areaEnv.YMin=mapEnvp.YMin;
                        if(areaEnv.XMax>mapEnvp.XMax)areaEnv.XMax=mapEnvp.XMax;
                        if(areaEnv.YMax>mapEnvp.YMax)areaEnv.YMax=mapEnvp.YMax;
                        /////////////////////////////////////////////////////////////////////////
                        //这里不能直接用mapEnvp的宽和高来计算 否则容易出现多一行或列的情况
                        //计算出需要更新的行列起始值
                        int colsS = Convert.ToInt32(Math.Floor((areaEnv.XMin - orgX) / tileWidth));
                        int colsE = Convert.ToInt32(Math.Ceiling((areaEnv.XMax - orgX) / tileWidth));
                        int rowsS = Convert.ToInt32(Math.Floor((orgY - areaEnv.YMax) / tileHeigh));
                        int rowsE = Convert.ToInt32(Math.Ceiling((orgY - areaEnv.YMin) / tileHeigh));

                        //生成对应的范围 并将对应范围的切片保存到 指定文件目录下
                        for (int i = rowsS; i < rowsE; i++)
                        {
                            for (int j = colsS; j < colsE; j++)
                            {
                                //设置小切片范围
                                IEnvelope tileEnv = new EnvelopeClass();
                                tileEnv.PutCoords(orgX + j * tileWidth, orgY - (i + 1) * tileHeigh, orgX + (j + 1) * tileWidth, orgY - i * tileHeigh);
                                // 创建图像,为24位色
                                Image image = new Bitmap(imgW, imgH);
                                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                                // 填充背景色(青色)
                                g.FillRectangle(Brushes.White, 0, 0, imgW, imgH);
                                //g.DrawRectangle(Pens.Green, 0, 0, imgW, imgH);
                                pActiveView.Output(g.GetHdc().ToInt32(), dpi, ref rect, tileEnv, pTrackCancel);
                                g.ReleaseHdc();//要在这里调用完后才能使用g 并且出图
                                //根据行列计算图片所在路径 并保存该路径
                                String tileImageName = GetExplodedTileImageName(i, j, level, mapOutputPath, imgFormat);
                                //根据各种格式保存
                                if (imgFormat == "bmp")
                                {
                                    image.Save(tileImageName, System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                                else if (imgFormat == "jpg")
                                {
                                    image.Save(tileImageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                else if (imgFormat == "png")
                                {
                                    image.Save(tileImageName, System.Drawing.Imaging.ImageFormat.Png);
                                }
                                else
                                {
                                    image.Save(tileImageName);
                                }
                                g.Dispose();
                            }
                        }
                        //更新进度条
                        num++;
                        prbCtl.Position = Convert.ToInt32((num / sum) * 100);
                    }
                }
                return true;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message + "将当前地图转出出错，原因未知", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        /// <导出紧凑型瓦片>
        /// 局部导出紧凑型瓦片
        /// </导出紧凑型瓦片>
        /// <param name="mapCtrl">视图控件</param>
        /// /// <param name="prbCtl">进度条控件</param>
        /// <param name="lodScales">层次信息</param>
        /// <param name="dpi">分辨率</param>
        /// <param name="orgX">切片原点X</param>
        /// <param name="orgY">切片原点Y</param>
        /// <param name="imgW">图片宽</param>
        /// <param name="imgH">图片高</param>
        /// <param name="imgFormat">图片格式</param>
        /// <param name="mapOutputPath">地图输出路径</param>
        /// <returns>是否导出成功</returns>
        public static bool ExportCompactTileOfPartMap(ESRI.ArcGIS.Controls.AxMapControl mapCtrl,
                                            DevExpress.XtraEditors.ProgressBarControl prbCtl,
                                            List<LodInfor> lodInfors, int dpi,
                                            double orgX, double orgY,
                                            int imgW, int imgH, String imgFormat, String mapOutputPath, List<IEnvelope> areaList)
        {
            //输出矩形
            tagRECT rect = new tagRECT();
            rect.bottom = imgH;
            rect.top = 0;
            rect.left = 0;
            rect.right = imgW;
            //转换成activeView，若为ILayout，则将Layout转换为IActiveView
            IActiveView pActiveView = mapCtrl.ActiveView;
            IEnvelope mapEnvp = mapCtrl.FullExtent;//全图范围
            double imgSum = lodInfors.Count * areaList.Count;//瓦片总量
            double currentNum = 0;//当前生产瓦片量
            try
            {
                ITrackCancel pTrackCancel = new TrackCancelClass();
                for (int level = 0; level < lodInfors.Count; level++)
                {
                    //double scale = lodInfors[level].Scale;
                    double reslution = lodInfors[level].Resolution;// 米/像素

                    double tileWidth = imgW * reslution;
                    double tileHeigh = imgH * reslution;

                    for (int areaIndex = 0; areaIndex < areaList.Count; areaIndex++)
                    {
                        IEnvelope areaEnv = areaList[areaIndex];
                        //范围超出检测
                        if (areaEnv.XMin < mapEnvp.XMin) areaEnv.XMin = mapEnvp.XMin;
                        if (areaEnv.YMin < mapEnvp.YMin) areaEnv.YMin = mapEnvp.YMin;
                        if (areaEnv.XMax > mapEnvp.XMax) areaEnv.XMax = mapEnvp.XMax;
                        if (areaEnv.YMax > mapEnvp.YMax) areaEnv.YMax = mapEnvp.YMax;
                        /////////////////////////////////////////////////////////////////////////
                        //这里不能直接用mapEnvp的宽和高来计算 否则容易出现多一行或列的情况
                        //计算出需要更新的行列起始值
                        int colsS = Convert.ToInt32(Math.Floor((areaEnv.XMin - orgX) / tileWidth));
                        int colsE = Convert.ToInt32(Math.Floor((areaEnv.XMax - orgX) / tileWidth));
                        int rowsS = Convert.ToInt32(Math.Floor((orgY - areaEnv.YMax) / tileHeigh));
                        int rowsE = Convert.ToInt32(Math.Floor((orgY - areaEnv.YMin) / tileHeigh));

                        //计算需要修改的切片行列起始 128 x 128为一个bundle中
                        int bundleColsS = colsS / 128;
                        int bundleColsE = colsE / 128 + 1;
                        int bundleRowsS = rowsS / 128;
                        int bundleRowsE = rowsE / 128 + 1;
                        //对受影响的切片进行遍历
                        for (int br = bundleRowsS; br < bundleRowsE; br++)
                        {
                            for (int bl = bundleColsS; bl < bundleColsE; bl++)
                            {
                                //每一个都是一个bundle文件
                                //1.自动生成对应的文件夹路径和文件名
                                String baseName = GetCompactBundlePathName(br, bl, level, mapOutputPath);
                                String bundleName = baseName + ".bundle";
                                String bundlxName = baseName + ".bundlx";
                                //声明对应的流 并打开对应文件 两个源文件 两个临时文件
                                System.IO.FileStream bundleFileStream = new System.IO.FileStream(bundleName, FileMode.Open);
                                System.IO.FileStream bundleTmpFileStream = new System.IO.FileStream(bundleName+"~", FileMode.OpenOrCreate);

                                //2.声明索引文件记录值 并将其初始化为0 这里记录的是 各个切片的长度 不是真正的索引值
                                //计算时 按照行优先 存储时按照列优先
                                long[,] OrgIndexData = GetIndexFromBunldxFile(bundlxName);//初始化的索引文件
                                long[,] indexTmpData = new long[128, 128];
                                indexTmpData[0, 0] = 0;
                                int offsetLabel = 0;//跟踪偏移量
                                //3.计算并写bundle文件 格式 长度+图片
                                for (int i = 0; i < 128; i++)
                                {
                                    for (int j = 0; j < 128; j++)
                                    {
                                        indexTmpData[i, j] = offsetLabel;//记录下来偏移量 开始的位置
                                        //获取该切片的图片信息
                                        //a.计算切片所在的行列
                                        int tileRow = 128 * br + i;
                                        int tileCol = 128 * bl + j;
                                        //(tileRow,tileCol)在areaEnv多代表的（colsS,rowsS）到（colsE,rowsE）需要更新
                                        //则需要从地图中获取数据 否则从原数据中读取
                                        if (tileCol >= colsS && tileCol <= colsE && tileRow >= rowsS && tileRow <= rowsE)
                                        {
                                            //b.设置小切片范围
                                            IEnvelope tileEnv = new EnvelopeClass();
                                            tileEnv.PutCoords(orgX + tileCol * tileWidth, orgY - (tileRow + 1) * tileHeigh, orgX + (tileCol + 1) * tileWidth, orgY - tileRow * tileHeigh);
                                            //c.获取图片数据
                                            Image image = new Bitmap(imgW, imgH);
                                            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                                            g.FillRectangle(Brushes.White, 0, 0, imgW, imgH);
                                            pActiveView.Output(g.GetHdc().ToInt32(), dpi, ref rect, tileEnv, pTrackCancel);
                                            g.ReleaseHdc();
                                            //d.写bundle文件  数据长度+图片数据 （低位到高位写长度 长度为4）
                                            byte[] imagedata = null;
                                            //图片数据根据各种格式保存
                                            if (imgFormat == "bmp")
                                            {
                                                //长度数据
                                                MemoryStream ms = new MemoryStream();
                                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                                                imagedata = ms.GetBuffer();//图片数据
                                                ms.Close();

                                            }
                                            else if (imgFormat == "jpg")
                                            {
                                                MemoryStream ms = new MemoryStream();
                                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                imagedata = ms.GetBuffer();//图片数据
                                                ms.Close();
                                            }
                                            else if (imgFormat == "png")
                                            {
                                                MemoryStream ms = new MemoryStream();
                                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                                imagedata = ms.GetBuffer();//图片数据
                                                ms.Close();
                                            }
                                            else
                                            {
                                                MessageBox.Show("暂时不支持混合格式！");
                                            }
                                            //写数据 
                                            byte[] lengthBytes = new byte[4]; 
                                            lengthBytes=BitConverter.GetBytes(imagedata.Length);//不知道为什么 不用转换都可以
                                            bundleTmpFileStream.Write(lengthBytes, 0, 4);
                                            bundleTmpFileStream.Write(imagedata, 0, imagedata.Length);
                                            offsetLabel += 4 + imagedata.Length;//移动偏移量
                                            g.Dispose();
                                        }
                                        else //从原文件读取 写到bundleTmpFileStream中
                                        {
                                            //从原数据中读取
                                            byte[] lengthBytes = new byte[4];
                                            bundleFileStream.Seek(OrgIndexData[i, j], SeekOrigin.Begin);
                                            bundleFileStream.Read(lengthBytes, 0, lengthBytes.Length);
                                            int imgDateLenth = BitConverter.ToInt32(lengthBytes, 0);
                                            byte[] imgData = new byte[imgDateLenth];
                                            bundleFileStream.Read(imgData, 0, imgDateLenth);
                                            //写到临时文件中
                                            bundleTmpFileStream.Write(lengthBytes, 0, 4);
                                            bundleTmpFileStream.Write(imgData, 0, imgData.Length);
                                            offsetLabel += imgDateLenth + 4;
                                        }
                                    }
                                }
                                //关闭bundle文件
                                bundleFileStream.Close();
                                bundleTmpFileStream.Close();
                                File.Replace(bundleName + "~", bundleName, null);//替换更新原文件
                                //写入索引文件
                                //初始化 bundlxFileStream 文件流 起始的16个字节无效 （暂时不可读取）
                                byte[] wipeHeaderData = new byte[16];
                                System.IO.FileStream bundlxFileStream = new System.IO.FileStream(bundlxName, FileMode.Open);
                                System.IO.FileStream bundlxTmpFileStream = new System.IO.FileStream(bundlxName + "~", FileMode.OpenOrCreate);
                                bundlxFileStream.Read(wipeHeaderData, 0, wipeHeaderData.Length);
                                bundlxTmpFileStream.Write(wipeHeaderData, 0, wipeHeaderData.Length);

                                for (int i = 0; i < 128; i++)
                                {
                                    for (int j = 0; j < 128; j++)
                                    {
                                        byte[] lenBytes = BitConverter.GetBytes(indexTmpData[j, i]);
                                        if (lenBytes.Length < 5)
                                        {
                                            bundlxTmpFileStream.Write(lenBytes, 0, lenBytes.Length);
                                            bundlxTmpFileStream.WriteByte(0);
                                        }
                                        else
                                            bundlxTmpFileStream.Write(lenBytes, 0, 5);
                                    }
                                }
                                bundlxFileStream.Seek(16, SeekOrigin.End);
                                byte[] wipeEnderData = new byte[16];
                                bundlxFileStream.Read(wipeEnderData, 0, 16);
                                bundlxTmpFileStream.Write(wipeEnderData, 0, wipeEnderData.Length);
                                bundlxFileStream.Close();
                                bundlxTmpFileStream.Close();
                                File.Replace(bundlxName + "~", bundlxName, null);
                            }
                        }
                        currentNum++;
                        prbCtl.Position = Convert.ToInt32(currentNum / imgSum * 100);
                    }    
                }
                return true;
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message + "将当前地图转出出错，原因未知", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <获取Bundlx中的索引信息>
        /// 获取Bundlx中的索引信息
        /// </获取Bundlx中的索引信息>
        /// <param name="bundlxFile"></param>
        /// <returns></returns>
        public static long[,] GetIndexFromBunldxFile(String bundlxFile)
        {
            long[,] indexData = new long[128, 128];
            System.IO.FileStream isBundlx = new System.IO.FileStream(bundlxFile,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read);
            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    int skip = 16 + 5 * (i*128+j);
                    byte[] buffer = new byte[5];
                    isBundlx.Seek((long)skip, System.IO.SeekOrigin.Begin);
                    isBundlx.Read(buffer, 0, buffer.Length);
                    //低位到高位转为高位到低位（5个字节反方向记录长度） 
                    // 如ABCDE->EDCBA 所以是A*236^0+B*256^1+C*256^2+D*256^3+E*256^4 这里取&0xff不知道有什么意义
                    long offset = (long)(buffer[0] & 0xff) + (long)(buffer[1] & 0xff)
                            * 256 + (long)(buffer[2] & 0xff) * 65536
                            + (long)(buffer[3] & 0xff) * 16777216
                            + (long)(buffer[4] & 0xff) * 4294967296L;
                    indexData[j,i] = offset;
                }
            }
            isBundlx.Close();//关闭文件
            return indexData;
        }
    }
}
