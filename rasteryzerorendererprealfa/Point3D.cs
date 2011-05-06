using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace rasteryzerorenderer
{
    class Point3D
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public Intensity color;
        public Point3D()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            w = 0.0f;

            this.color = new Intensity(Color.Green);
        }
        public Point3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0f;
            this.color = new Intensity(Color.Green);
        }
        public Point3D(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;

            this.color = new Intensity(Color.Blue);
        }
        public Point3D(float x, float y, float z, Intensity color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0f;
            this.color = color;
        }
        public Point3D(float x, float y, float z, float w, Intensity color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            this.color = color;
        }
        public static Point3D operator +(Point3D left, Point3D right)
        {
            return new Point3D(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public override string ToString()
        {
            return "X= " + this.x + ", Y= " + this.y + ", Z= " + this.z+", W= "+this.w;
        }
    }
}
