using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    static class Renderer
    {
        public static void renderujSfere(ColorBuffer myBuf, Point3D sr, float radius, Matrix4x4 afterTrans, Vector lightPos) //ost arg - proj*view
        {
            float rad = radius;//0.1
            //rysujemy sferę...
            //Point3D sr = new Point3D(3, 0, 3);
            //prom = 0.2
            Point3D[,] pol1 = new Point3D[9, 18];
            Point3D[,] pol2 = new Point3D[9, 18];
            Matrix4x4 rot_dol = new Matrix4x4();
            float a20stopni = (float)(20.0 / 180.0 * Math.PI);
            float a40stopni = (float)(40.0 / 180.0 * Math.PI);

            // rot_dol.setRotationX(a20stopni);
            Matrix4x4 rot_left = new Matrix4x4();
            rot_left.setRotationZ(a20stopni);//0.5

            Matrix4x4 rot_left2 = new Matrix4x4();
            // rot_left2.setRotationZ(a20stopni);//0.5
            for (int i = 0; i < 9; i++)
            {

                rot_dol.setRotationX(i * a40stopni);
                for (int j = 0; j < 18; j++)
                {
                    rot_left2.setRotationZ(j * a20stopni);
                    pol1[i, j] = rot_left2 * (rot_dol * new Point3D(0.0f, rad, 0.0f));
                    pol2[i, j] = rot_left * pol1[i, j];
                }
            }

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 18; j++)
                    pol1[i, j] += sr;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 18; j++)
                    pol2[i, j] += sr;

            Triangle3D[, ,] kula = new Triangle3D[18, 8, 2];
            Vector[, ,] kulaNormalne = new Vector[18, 8, 2];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 18; j++)
                {
                    kula[j, i, 0] = new Triangle3D(pol2[i, j], pol1[i, j], pol2[i + 1, j]);
                    kula[j, i, 1] = new Triangle3D(pol1[i, j], pol1[i + 1, j], pol2[i + 1, j]);

                }

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 18; j++)
                {
                    kulaNormalne[j, i, 0] = kulaNormalne[j, i, 1] = kula[j, i, 0].fuckinNormal();
                }
            Triangle2D[, ,] kula2D = new Triangle2D[18, 8, 2];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 18; j++)
                {
                    kula2D[j, i, 0] = new Triangle2D();
                    kula2D[j, i, 1] = new Triangle2D();

                }
            for (int i2 = 0; i2 < 8; i2++)
            {
                for (int j2 = 0; j2 < 18; j2++)
                {
                    for (int k2 = 0; k2 < 2; k2++)
                    {
                        for (int z2 = 0; z2 < 3; z2++)
                        {

                            Point3D vert = kula[j2, i2, k2].v[z2];
                            Point3D temp = afterTrans * vert;
                            kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);

                            float beta1a = kula[j2, i2, k2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z)).normalizeProduct());
                            //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                            //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());

                            kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                            //System.Console.WriteLine("" + res2.v[z2]);
                        }
                        for (int i = 0; i < 400; i++)
                            for (int j = 0; j < 400; j++)
                            {
                                Point2D testowany = new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0);
                                if (
                                    kula2D[j2, i2, k2].contains_point(testowany)
                                      )//trzeci parametr jest ignorowany!
                                {
                                    myBuf.setPixel(i, j, kula2D[j2, i2, k2].interpolate(testowany));
                                    //zamiast 3 parametru, interpolowana głębokość (funkcja)
                                };
                            }
                    }
                }
            }
        }

        public static void renderujSszescian(ColorBuffer myBuf, Point3D sr, float radius, Matrix4x4 afterTrans, Vector lightPos) //ost arg - proj*view
        {
            float rad = radius;//0.1
            //rysujemy sferę...
            //Point3D sr = new Point3D(3, 0, 3);
            //prom = 0.2
            Point3D[] pol1 = new Point3D[48];
            Matrix4x4 rot_dol = new Matrix4x4();
            float a20stopni = (float)(20.0 / 180.0 * Math.PI);
            float a40stopni = (float)(40.0 / 180.0 * Math.PI);

            // rot_dol.setRotationX(a20stopni);
            Matrix4x4 rot_left = new Matrix4x4();
            rot_left.setRotationZ(a20stopni);//0.5

            Matrix4x4 rot_left2 = new Matrix4x4();
            // rot_left2.setRotationZ(a20stopni);//0.5
            float szer = 0.1f;
            pol1[0] = new Point3D(-szer, szer, -szer);
            pol1[1] = new Point3D(-szer, -szer, szer);
            pol1[2] = new Point3D(-szer, -szer, -szer);

            pol1[3] = new Point3D(-szer, szer, szer);
            pol1[4] = new Point3D(-szer, -szer, szer);
            pol1[5] = new Point3D(-szer, szer, -szer);


            pol1[6] = new Point3D(szer, -szer, -szer);
            pol1[7] = new Point3D(szer, -szer, szer);
            pol1[8] = new Point3D(szer, szer, -szer);

            pol1[9] = new Point3D(szer, szer, -szer);
            pol1[10] = new Point3D(szer, -szer, szer);
            pol1[11] = new Point3D(szer, szer, szer);
            //1 z 3

            pol1[12] = new Point3D(-szer, szer, -szer);
            pol1[13] = new Point3D(szer, -szer, -szer);
            pol1[14] = new Point3D(-szer, -szer, -szer);

            pol1[15] = new Point3D(-szer, szer, -szer);//ok
            pol1[16] = new Point3D(szer, -szer, -szer);
            pol1[17] = new Point3D(szer, szer, -szer);


            pol1[18] = new Point3D(szer, -szer, -szer);//ok
            pol1[19] = new Point3D(-szer, szer, -szer);
            pol1[20] = new Point3D(-szer, -szer, -szer);

            pol1[21] = new Point3D(szer, szer, -szer);
            pol1[22] = new Point3D(szer, szer, -szer);
            pol1[23] = new Point3D(-szer, -szer, -szer);

            for (int i = 24; i < 48; i++)
            {
                Matrix4x4 obr = new Matrix4x4();
                obr.setRotationX((float)Math.PI);
                Matrix4x4 trans = new Matrix4x4();

                trans.setTranslation(0, 0, 1);
                pol1[i] = obr * (trans * pol1[i - 24]);
            }

            Matrix4x4 rotate = new Matrix4x4();
            rotate.setIdentity();
            rotate.setRotationZ(0.5f);
            for (int i = 0; i < 48; i++)
            {
                pol1[i] = rotate * pol1[i];
                pol1[i].color = new Intensity(1, 0, 0);
            }
            for (int i = 0; i < 48; i++)
                pol1[i] += sr;

            Triangle3D[] szescian = new Triangle3D[16];
            Vector[] szescianNormalne = new Vector[16];
            for (int i = 0; i < 16; i++)
                szescian[i] = new Triangle3D(pol1[i * 3], pol1[i * 3 + 1], pol1[i * 3 + 2]);


            for (int i = 0; i < 16; i++)

                szescianNormalne[i] = (szescian[i].fuckinNormal());

            Triangle2D[] szescian2D = new Triangle2D[16];
            for (int i = 0; i < 16; i++)
                szescian2D[i] = new Triangle2D();
            for (int i2 = 0; i2 < 16; i2++)
            {

                for (int z2 = 0; z2 < 3; z2++)
                {

                    Point3D vert = szescian[i2].v[z2];
                    Point3D temp = afterTrans * vert;
                    //     kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    szescian2D[i2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    //    float beta1a = kula[j2, i2, k2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z)).normalizeProduct());
                    //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                    //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());
                    float beta1a = szescian[i2].fuckinNormal().dot((lightPos - new Vector(szescian[i2].v[z2].x, szescian[i2].v[z2].y, szescian[i2].v[z2].z)).normalizeProduct());
                    // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                    szescian2D[i2].v[z2].color = vert.color * beta1a;
                    //System.Console.WriteLine("" + res2.v[z2]);
                }
                for (int i = 0; i < 400; i++)
                    for (int j = 0; j < 400; j++)
                    {
                        Point2D testowany = new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0);
                        if (
                            //kula2D[j2, i2, k2].contains_point(testowany)
                            szescian2D[i2].contains_point(testowany)
                              )//trzeci parametr jest ignorowany!
                        {
                            myBuf.setPixel(i, j, szescian2D[i2].interpolate(testowany));
                            //zamiast 3 parametru, interpolowana głębokość (funkcja)
                        };
                    }
            }
        }

        public static void renderujJakisShit(ColorBuffer myBuf, Point3D sr, float radius, Matrix4x4 afterTrans, Vector lightPos) //ost arg - proj*view
        {
            float rad = radius;//0.1
            //rysujemy sferę...
            //Point3D sr = new Point3D(3, 0, 3);
            //prom = 0.2
            Point3D[] pol1 = new Point3D[48];
            Matrix4x4 rot_dol = new Matrix4x4();
            float a20stopni = (float)(20.0 / 180.0 * Math.PI);
            float a40stopni = (float)(40.0 / 180.0 * Math.PI);

            // rot_dol.setRotationX(a20stopni);
            Matrix4x4 rot_left = new Matrix4x4();
            rot_left.setRotationZ(a20stopni);//0.5

            Matrix4x4 rot_left2 = new Matrix4x4();
            // rot_left2.setRotationZ(a20stopni);//0.5
            float szer = 0.1f;
            pol1[0] = new Point3D(-szer, szer, -szer);
            pol1[1] = new Point3D(-szer, -szer, szer);
            pol1[2] = new Point3D(-szer*0.5f, -szer*0.5f, 0);

            pol1[3] = new Point3D(0, 0, 0);
            pol1[4] = new Point3D(0, 0, 0);
            pol1[5] = new Point3D(0, 0, 0);


            pol1[6] = new Point3D(szer, -szer, -szer);
            pol1[7] = new Point3D(szer, -szer, szer);//niesp
            pol1[8] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0);

            pol1[9] = new Point3D(0, 0, 0);
            pol1[10] = new Point3D(0, 0, 0);
            pol1[11] = new Point3D(0, 0, 0);
            //1 z 3

            pol1[12] = new Point3D(-szer, szer, -szer);
            pol1[13] = new Point3D(szer, -szer, -szer);
            pol1[14] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0);

            pol1[15] = new Point3D(0, 0, 0);
            pol1[16] = new Point3D(0, 0, 0);
            pol1[17] = new Point3D(0, 0, 0);


            pol1[18] = new Point3D(-szer, -szer, szer);//ok
            pol1[19] = new Point3D(szer, szer, szer);//niesparowane
            pol1[20] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0);

            pol1[21] = new Point3D(0, 0, 0);
            pol1[22] = new Point3D(0, 0, 0);
            pol1[23] = new Point3D(0, 0, 0);

            

            Matrix4x4 rotate = new Matrix4x4();
            rotate.setIdentity();
            rotate.setRotationX(1f);
            for (int i = 0; i < 24; i++)
            {
                pol1[i] = rotate * pol1[i];
                pol1[i].color = new Intensity(1, 0, 0);
            }
            for (int i = 0; i < 24; i++)
                pol1[i] += sr;

            Triangle3D[] szescian = new Triangle3D[8];
            Vector[] szescianNormalne = new Vector[8];
            for (int i = 0; i < 8; i++)
                szescian[i] = new Triangle3D(pol1[i * 3], pol1[i * 3 + 1], pol1[i * 3 + 2]);


            for (int i = 0; i < 8; i++)

                szescianNormalne[i] = (szescian[i].fuckinNormal());

            Triangle2D[] szescian2D = new Triangle2D[16];
            for (int i = 0; i < 8; i++)
                szescian2D[i] = new Triangle2D();
            for (int i2 = 0; i2 < 8; i2++)
            {

                for (int z2 = 0; z2 < 3; z2++)
                {

                    Point3D vert = szescian[i2].v[z2];
                    Point3D temp = afterTrans * vert;
                    //     kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    szescian2D[i2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    //    float beta1a = kula[j2, i2, k2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z)).normalizeProduct());
                    //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                    //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());
                    float beta1a = szescian[i2].fuckinNormal().dot((lightPos - new Vector(szescian[i2].v[z2].x, szescian[i2].v[z2].y, szescian[i2].v[z2].z)).normalizeProduct());
                    // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                    szescian2D[i2].v[z2].color = vert.color * beta1a;
                    //System.Console.WriteLine("" + res2.v[z2]);
                }
                for (int i = 0; i < 400; i++)
                    for (int j = 0; j < 400; j++)
                    {
                        Point2D testowany = new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0);
                        if (
                            //kula2D[j2, i2, k2].contains_point(testowany)
                            szescian2D[i2].contains_point(testowany)
                              )//trzeci parametr jest ignorowany!
                        {
                            myBuf.setPixel(i, j, szescian2D[i2].interpolate(testowany));
                            //zamiast 3 parametru, interpolowana głębokość (funkcja)
                        };
                    }
            }
        }


    }
}





