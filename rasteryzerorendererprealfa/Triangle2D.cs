using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace rasteryzerorenderer
{
    class Triangle2D
    {
        public Point2D[] v;

        public Triangle2D()
        {
            v = new Point2D[3];
            this.v[0] = new Point2D();
            this.v[1] = new Point2D();
            this.v[2] = new Point2D();


        }
        public Triangle2D(Point2D v1, Point2D v2, Point2D v3)
        {
            v = new Point2D[3];
            this.v[0] = v1;
            this.v[1] = v2;
            this.v[2] = v3;
        }
        public Triangle2D(Point3D v1, Point3D v2, Point3D v3)
        {
            v = new Point2D[3];
            this.v[0] = new Point2D(v1.x, v1.y, v1.z);
            this.v[1] = new Point2D(v2.x, v2.y, v2.z);
            this.v[2] = new Point2D(v3.x, v3.y, v3.z);

        }

        public bool contains_point(Point2D p)
        {
            float shit1 = (v[0].x - v[1].x) * (p.y - v[0].y)
                - (v[0].y - v[1].y) * (p.x - v[0].x);
            float shit2 = (v[1].x - v[2].x) * (p.y - v[1].y)
                - (v[1].y - v[2].y) * (p.x - v[1].x);
            float shit3 = (v[2].x - v[0].x) * (p.y - v[2].y)
                - (v[2].y - v[0].y) * (p.x - v[2].x);

            if (
                (shit1 > 0 && shit2 > 0 && shit3 > 0)
                ||
                (shit1 < 0 && shit2 < 0 && shit3 < 0)
                )
                return true;
            return false;
        }
        /**
          <summary
         * Metoda... Dajmy na to, zwracająca kolor
         * w punkcie z argumentu w układzie płaszczyzny,
         * a jeśli punkt poza trójkątem, to jakiegoś nulla.
         </summary>
         */
        public Color interpolate(Point2D color)
        {
            Color res;// = new Color();
            float[] lambda = new float[3];
            float  y, y1, y2, y3,
                    x, x1, x2, x3;
            float y2_y3, x_x3, x3_x2, y_y3,
                    x1_x3, y1_y3;
          //  float y3_y1;//, x2_x3;
            x = color.x;
            y = color.y;
            y1 = v[0].y; y2 = v[1].y; y3 = v[2].y;
            x1 = v[0].x; x2 = v[1].x; x3 = v[2].x;
            y2_y3 = y2 - y3; 
            x_x3 = x - x3;
            x3_x2 = x3 - x2; 
            y_y3 = y - y3;
            x1_x3 = x1 - x3; 
            y1_y3 = y1 - y3;
           // y3_y1 = -y1_y3; //x2_x3 = -x3_x2;
            float mian = y2_y3 * x1_x3 + x3_x2 * y1_y3;
            //uwaga na indeksy! Jest to \lambda_{1}!
            lambda[0] = y2_y3 * x_x3 + x3_x2 * y_y3;
            lambda[0] /= mian;
            lambda[1] = x1_x3 * y_y3 - y1_y3 * x_x3;
            lambda[1] /= mian;
            lambda[2] = 1 - (lambda[0] + lambda[1]);
            res =Color.FromArgb
                (
                (byte)(255*(lambda[0] * v[0].color.R + lambda[1] * v[1].color.R + lambda[2] * v[2].color.R)),
                (byte)(255*(lambda[0] * v[0].color.G + lambda[1] * v[1].color.G + lambda[2] * v[2].color.G)),
                (byte)(255*(lambda[0] * v[0].color.B + lambda[1] * v[1].color.B + lambda[2] * v[2].color.B))
                );
            return res; 
        }

        public float interpolateDepth(float x, float y)
        {
            float res;
            float[] lambda = new float[3];
            float  y1, y2, y3,
                     x1, x2, x3;
            float y2_y3, x_x3, x3_x2, y_y3,
                    x1_x3, y1_y3;
          //  float y3_y1;//, x2_x3;
            
            y1 = v[0].y; y2 = v[1].y; y3 = v[2].y;
            x1 = v[0].x; x2 = v[1].x; x3 = v[2].x;
            y2_y3 = y2 - y3; x_x3 = x - x3;
            x3_x2 = x3 - x2; y_y3 = y - y3;
            x1_x3 = x1 - x3; y1_y3 = y1 - y3;
        //    y3_y1 = -y1_y3; //x2_x3 = -x3_x2;
            float mian = y2_y3 * x1_x3 + x3_x2 * y1_y3;
            //uwaga na indeksy! Jest to \lambda_{1}!
            lambda[0] = y2_y3 * x_x3 + x3_x2 * y_y3;
            lambda[0] /= mian;
            lambda[1] = x1_x3 * y_y3 - y1_y3 * x_x3;
            lambda[1] /= mian;
            lambda[2] = 1 - (lambda[0] + lambda[1]);
            res = lambda[0] * v[0].depth + lambda[1] * v[1].depth + lambda[2] * v[2].depth;
            return res; //wypadałoby to jednak skasować
        }
    }
}
