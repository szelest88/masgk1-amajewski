using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace rasteryzerorenderer
{
    class Point2D
    {
        public float x;
        public float y;
        //public Color color;
        public Intensity color;
        public float depth; //odległość od kamery... Zakminimy.

        public Point2D()
        {
            x = 0.0f;
            y = 0.0f;
            this.color = new Intensity(Color.White);
            this.depth = 0.0f;
        }
        public Point2D(float x, float y, float depth)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.color = new Intensity(Color.White);
        }
        public Point2D(float x, float y, float depth, Color color)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.color = new Intensity(color);
        }
        public Point2D(float x, float y, float depth, Intensity color)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.color = color;
        }
        public override string ToString()
        {
            return "x= " + x + ",y= " + y + "depth= " + depth;
        }
    }
}
