using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace rasteryzerorenderer
{
    /**
     punkt w R3. W sumie tą klasę można wywalić i zastąpnić klasą Vector
     */
    class Point3D
    {
        public float x; //3 współrzędne punktu w R3
        public float y;
        public float z;
        public float w; //coś kurde miałem na myśli, ale zrezygnowałem. To jest zbędne ;)
        public Intensity color; //kolor (jeśli np. punkt jest wierzchołkiem trójkąta)
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
