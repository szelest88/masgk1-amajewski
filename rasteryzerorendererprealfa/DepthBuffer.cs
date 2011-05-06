using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    class DepthBuffer
    {
        public float[,] depth;
        public int width;
        public int height;
        public DepthBuffer(int width, int height)
        {
            depth = new float[width, height];
            this.width = width;
            this.height = height;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    depth[i, j] = float.PositiveInfinity;
        }
        /**
         <<summary>
         czyści bufor (zapełnia +inf)
         </summary>
         */
        public void clearBuffer()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    depth[i, j] = float.PositiveInfinity;
        }
        public void setPixel(int x, int y,float val)
        {
            depth[x, y] = val;
        }
    }
}
