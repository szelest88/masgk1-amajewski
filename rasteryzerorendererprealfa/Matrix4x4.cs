//ta klasa jest Open Source i4 pozwalam na korzystanie z niej
//w ogóle zawsze i wszędzie - autor.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    class Matrix4x4
    {
        public float[,] tab;
        public Matrix4x4()
        {
            tab = new float[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    tab[i, j] = 0.0f;
        }
        public void setIdentity()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == j)
                        tab[i, j] = 1.0f;
                    else
                        tab[i, j] = 0.0f;
                }
        }
        public void set(int i, int j, float val)
        {
            this.tab[i, j] = val;
        }

        public void show()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    System.Console.Write(tab[j, i] + "\t");
                }
                System.Console.Write("\n");
            }
        }

        public float this[int index1, int index2]
        {
            get
            {
                return tab[index1, index2];
            }
        }

        public Matrix4x4 multiply(Matrix4x4 arg2)
        {
            Matrix4x4 res = new Matrix4x4();
            for (int ires = 0; ires < 4; ires++)
                for (int jres = 0; jres < 4; jres++)
                {
                    // res.tab[ires,jres]=0.0;
                    for (int i = 0; i < 4; i++)
                        res.tab[ires, jres] += (this[i, jres] * arg2[ires, i]);

                }

            //this topierwszy argument
            return res;
        }

        public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
        {
            Matrix4x4 res = new Matrix4x4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res.tab[i, j] = left
                        [i, j] + right[i, j];
            return res;
        }

        public void setTranslation(float x, float y, float z)
        {
            this.setIdentity();
            this.tab[0, 3] = x;
            this.tab[1, 3] = y;
            this.tab[2, 3] = z;
        }
        /**
          <summary>
          radiany
          </summary>
         */
        public void setRotationX(float angle) //V
        {
            this.setIdentity();
            this.tab[1, 1] = (float)Math.Cos(angle);
            this.tab[2, 1] = -(float)Math.Sin(angle);
            this.tab[1, 2] = -tab[2, 1];
            this.tab[2, 2] = tab[1, 1];
        }
        /**
          <summary>
          radiany
          </summary>
         */
        public void setRotationY(float angle) //V
        {
            this.setIdentity();
            this.tab[0, 0] = (float)Math.Cos(angle);
            this.tab[2, 0] = (float)Math.Sin(angle);
            this.tab[0, 2] = -tab[2, 0];
            this.tab[2, 2] = tab[0, 0];
        }
        /**
          <summary>
          radiany
          </summary>
         */
        public void setRotationZ(float angle)
        {
            this.setIdentity();
            this.tab[0, 0] = (float)Math.Cos(angle);
            this.tab[1, 0] = -(float)Math.Sin(angle);
            this.tab[0, 1] = -tab[1, 0];
            this.tab[1, 1] = tab[0, 0];
        }
        public static Point3D operator *(Matrix4x4 left, Point3D right)
        {
            float wiersz1;
            float wiersz2;
            float wiersz3;
            float wiersz4;
            wiersz1 = left.tab[0, 0] * right.x;
            wiersz1 += left.tab[1, 0] * right.y;
            wiersz1 += left.tab[2, 0] * right.z;

            //wiersz1 += left.tab[3, 0] * 0;

            wiersz2 = left.tab[0, 1] * right.x;
            wiersz2 += left.tab[1, 1] * right.y;
            wiersz2 += left.tab[2, 1] * right.z;

           // wiersz2 += left.tab[3, 1] * 0;

            wiersz3 = left.tab[0, 2] * right.x;
            wiersz3 += left.tab[1, 2] * right.y;
            wiersz3 += left.tab[2, 2] * right.z;
            
            //wiersz3 += left.tab[3, 2] * 0;

            wiersz4 = left.tab[0, 3] * right.x;
            wiersz4 += left.tab[1, 3] * right.y;
            wiersz4 += left.tab[2, 3] * right.z;
            //wiersz4 += left.tab[3, 3] * 0;

            //wiersz1 += wiersz4;
            //wiersz2 += wiersz4;
            //wiersz3 += wiersz4;



            return new Point3D(wiersz1, wiersz2, wiersz3, wiersz4);
        }
        /**
         * <summary>
         * Madness? This... is... SPARTA!!!
         * </summary>
         * Source:
         * http://kb.komires.net/article.php?id=6
         */
        public void setAsViewMatrix(Vector observer, Vector lookAt, Vector up)
        {

            Vector right = (lookAt - observer).cross(up);
            right *= -1;
            //observer -  
            observer.normalize();
            lookAt.normalize();
            up.normalize();
            right.normalize();
            this.tab = new float[4, 4]
            {
                {right.x,               up.x,       lookAt.x,       0},
                {right.y,               up.y,       lookAt.y,       0},
                {right.z,               up.z,       lookAt.z,       0},
                {-observer.dot(right),  -observer.dot(up),          -observer.dot(lookAt),          1}
            };
        }
        /**
         * przepisać z:
         * http://kb.komires.net/article.php?id=6
         * 
         * Viewmatrix będzie przyjmować parametry
         * bryły obcinania - angles i odległości, kąt jest w stopniach!
         * przedniej i tylnej płaszczyzny
         */
        public void setAsProjectionMatrix(float alpha, float Zn, float Zf)
        {
            alpha=alpha*(float)Math.PI/180.0f;
            float w = 1 / (float)Math.Tan(alpha/2);
            float h = w;
            float Q = Zf / (Zf - Zn);
            this.tab = new float[4, 4]
            {
                {w,     0,      0,  0},
                {0,     h,      0,  0},
                {0,     0,      Q,  1},
                {0,     0,      -Q*Zn,  0}
            };
        }


    }

}
