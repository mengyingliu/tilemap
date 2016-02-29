using System;
using System.Collections.Generic;
using System.Text;

namespace TileMapManager
{
    public class MapRect
    {
        public double xMax = 0;
        public double yMax = 0;
        public double xMin = 0;
        public double yMin = 0;
        public MapRect()
        {
            xMax=100;yMax=100;xMin=0;yMin=0;
        }
        public MapRect(double minX,double minY,double maxX,double maxY)
        {
            xMax=maxX;yMax=maxY;xMin=minX;yMin=maxY;
        }
        public double GetWidth()
        {
            return xMax - xMin;
        }
        public double GetHeigh()
        {
            return yMax - yMin;
        }
    }

    public struct MapPoint
    {
        public double x;
        public double y;
    }
}
