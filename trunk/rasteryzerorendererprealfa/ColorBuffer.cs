using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace rasteryzerorenderer
{
    class ColorBuffer
    {

        public Bitmap bmp;
        public int width;
        public int height;
        public ColorBuffer(int width, int height)
        {
            bmp = new Bitmap(width, height);
            this.width = width;
            this.height = height;
        }
        public void clearBuffer(Color clearColor)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    bmp.SetPixel(i, j, clearColor);
        }
        public void setPixel(int x, int y, Color color)
        {
            bmp.SetPixel(x, y, color);
        }
    }
}
