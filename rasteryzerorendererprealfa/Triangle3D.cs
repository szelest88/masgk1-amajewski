using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    /**
     <summary>
     Klasa reprezentuje trójkąt w R3 - składa się on z 3 punktów w R3, zapisanych w
     tablicy Point3D[] v
     </summary>
     */
    class Triangle3D
    {
        public Point3D[] v;
        public Triangle3D(Point3D v1, Point3D v2, Point3D v3)
        {
            v = new Point3D[3];
            v[0] = v1;
            v[1] = v2;
            v[2] = v3;
        }
        public Triangle3D(float x1,float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {
            v = new Point3D[3];
            v[0] = new Point3D(x1, y1, z1);
            v[1] = new Point3D(x2, y2, z2);
            v[2] = new Point3D(x3, y3, z3);
        }
        /**
         <summary>
         mnoży lewostronnie każdy wierzchołek przez macierz
         czyli przekształca trójkąt (np. obraca)
         </summary>
         **/
        public void multiplyLeftByMatrix(Matrix4x4 m){
            v[0] = m * v[0];
            v[1] = m * v[1];
            v[2] = m * v[2];

        }
        public override string ToString()
        {
            return "" + v[0] + ", " + v[1] + ", " + v[2];
        }

        /**
         <summary>
         zwraca wektor normalny do trójkąta
         </summary>
         */
        public Vector fuckinNormal()
        {
            Vector v1 = new Vector(v[0].x - v[1].x, v[0].y - v[1].y, v[0].z - v[1].z);
            Vector v2 = new Vector(v[0].x - v[2].x, v[0].y - v[2].y, v[0].z - v[2].z);
            return (v1.cross(v2)).normalizeProduct();

        }
    }
}
