using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    static class Renderer
    {
        /**
         * <summary>
         * elemnts 9 or 18
         * </summary>
         */
        public static void renderujSfere(ColorBuffer myBuf, Point3D sr, float radius, Matrix4x4 afterTrans, Vector lightPos, int elements, DepthBuffer db, Intensity color) //ost arg - proj*view
        {
            float rad = radius;//0.1
            //rysujemy sferę...
            //Point3D sr = new Point3D(3, 0, 3);
            //prom = 0.2
            Point3D[,] pol1 = new Point3D[elements, elements];//1 9>18
            Point3D[,] pol2 = new Point3D[elements, elements];//1 9>18
            Matrix4x4 rot_dol = new Matrix4x4();
            float a20stopni = (float)((360/elements) / 180.0 * Math.PI);
//            float a40stopni = (float)(20.0 / 180.0 * Math.PI);

            // rot_dol.setRotationX(a20stopni);
            Matrix4x4 rot_left = new Matrix4x4();
            rot_left.setRotationZ(a20stopni);//0.5

            Matrix4x4 rot_left2 = new Matrix4x4();
            // rot_left2.setRotationZ(a20stopni);//0.5
            for (int i = 0; i < elements; i++)//9>18
            {

                rot_dol.setRotationX(i * a20stopni);
                for (int j = 0; j < elements; j++)
                {
                    rot_left2.setRotationZ(j * a20stopni);
                    pol1[i, j] = rot_left2 * (rot_dol * new Point3D(0.0f, rad, 0.0f));
                    pol2[i, j] = rot_left * pol1[i, j];
                }
            }

            for (int i = 0; i < elements; i++)//9>18
                for (int j = 0; j < elements; j++)
                    pol1[i, j] += sr;
            for (int i = 0; i < elements; i++)//9>18
                for (int j = 0; j < elements; j++)
                    pol2[i, j] += sr;

            Triangle3D[, ,] kula = new Triangle3D[elements, elements-2, 2];//8->16
            Vector[, ,] kulaNormalne = new Vector[elements, elements-2, 2];
            for (int i = 0; i < elements-2; i++)//8->16
                for (int j = 0; j < elements; j++)
                {
                    kula[j, i, 0] = new Triangle3D(pol2[i, j], pol1[i, j], pol2[i + 1, j]);
                    kula[j, i, 1] = new Triangle3D(pol1[i, j], pol1[i + 1, j], pol2[i + 1, j]);

                }

            for (int i = 0; i < elements-2; i++)//8->16
                for (int j = 0; j < elements; j++)
                {
                    kulaNormalne[j, i, 0] = kulaNormalne[j, i, 1] = kula[j, i, 0].fuckinNormal();
                }
            Triangle2D[, ,] kula2D = new Triangle2D[elements, elements-2, 2];//8->16
            for (int i = 0; i < elements-2; i++)//8->16
                for (int j = 0; j < elements; j++)
                {
                    kula2D[j, i, 0] = new Triangle2D();
                    kula2D[j, i, 1] = new Triangle2D();

                }
            for (int i2 = 0; i2 < elements-2; i2++)//8->16
            {
                for (int j2 = 0; j2 < elements; j2++)
                {
                    for (int k2 = 0; k2 < 2; k2++)
                    {
                        for (int z2 = 0; z2 < 3; z2++)
                        {

                            Point3D vert = kula[j2, i2, k2].v[z2]; //obiekt
                            vert.color = color;
                            Point3D temp = afterTrans * vert; //po transformacji
                            kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);  //chyba zbędne
                            Vector additional = //wektor od światła do obiektu
                                lightPos - 
                                new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z);
                            
                            Vector kierunek = additional.normalizeProduct();
                            //znormalizowany swiatło=obiekt
                            
                            Vector normalna = new Vector(vert.x - sr.x, vert.y - sr.y, vert.z - sr.z).normalizeProduct();// kula[j2, i2, k2].fuckinNormal();
                            //normalna (kula!)
                            
                            float beta1a = normalna.dot(kierunek);
                            //wartość diffuse'a

                            //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                            //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());

                            //SPEC?
                            Vector do_obs = additional;// lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z);
                            //światło-obiekt
                            
                            Vector I = kierunek;
                            //znormalizowany swiatło=obiekt

                            Vector N = normalna;
                            //normalna

                            Vector R = I - N * (beta1a * 2.0f);//R = I - N * (N.dot(I) * 2.0f);
                            //  odbity do speculara

                            float ss = do_obs.normalizeProduct().dot(R);
                            //składowa specular

                            float beta2a;
                            if (ss < 0)
                                beta2a = (float)Math.Pow(ss, 8);
                                //alfa jest na sztywno!
                            else
                                beta2a = 0;


                            // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                           // szescian2D[i2].v[z2].color = (vert.color * beta1a) + (new Intensity(1, 1, 1) * beta2a);
                            //SPEC??
                            kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a + (new Intensity(1,1,1) * beta2a);
                            //System.Console.WriteLine("" + res2.v[z2]);
                        }
                        for (int i = 0; i < 400; i++)
                            for (int j = 0; j < 400; j++)
                            {
                                Point2D testowany = new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0);
                                if (
                                    kula2D[j2, i2, k2].contains_point(testowany)
                                      &&
                        kula2D[j2, i2, k2].interpolateDepth(testowany.x,testowany.y) < db.depth[i, j] //i jego głębokość < od zapisanej w buforze gł
                       
                                      )//trzeci parametr jest ignorowany (chyba już nie)
                                {
                                    myBuf.setPixel(i, j, kula2D[j2, i2, k2].interpolate(testowany));
                                    //zamiast 3 parametru, interpolowana głębokość (funkcja)
                                    db.setPixel(i, j, kula2D[j2, i2, k2].interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f)); //i jego głebokość do bufora głębokości
                                };
                            }
                    }
                }
            }
        }

        public static void renderujSszescian(ColorBuffer myBuf, Point3D sr, float radius, Matrix4x4 afterTrans, Vector lightPos, Intensity color) //ost arg - proj*view
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
            pol1[0] = new Point3D(-szer, szer, -szer,color);
            pol1[1] = new Point3D(-szer, -szer, szer,color);
            pol1[2] = new Point3D(-szer, -szer, -szer,color);

            pol1[3] = new Point3D(-szer, szer, szer,color);
            pol1[4] = new Point3D(-szer, -szer, szer,color);
            pol1[5] = new Point3D(-szer, szer, -szer, color);


            pol1[6] = new Point3D(szer, -szer, -szer, color);
            pol1[7] = new Point3D(szer, -szer, szer, color);
            pol1[8] = new Point3D(szer, szer, -szer, color);

            pol1[9] = new Point3D(szer, szer, -szer, color);
            pol1[10] = new Point3D(szer, -szer, szer, color);
            pol1[11] = new Point3D(szer, szer, szer, color);
            //1 z 3

            pol1[12] = new Point3D(-szer, szer, -szer, color);
            pol1[13] = new Point3D(szer, -szer, -szer, color);
            pol1[14] = new Point3D(-szer, -szer, -szer, color);

            pol1[15] = new Point3D(-szer, szer, -szer, color);//ok
            pol1[16] = new Point3D(szer, -szer, -szer, color);
            pol1[17] = new Point3D(szer, szer, -szer, color);


            pol1[18] = new Point3D(szer, -szer, -szer, color);//ok
            pol1[19] = new Point3D(-szer, szer, -szer, color);
            pol1[20] = new Point3D(-szer, -szer, -szer, color);

            pol1[21] = new Point3D(szer, szer, -szer, color);
            pol1[22] = new Point3D(szer, szer, -szer, color);
            pol1[23] = new Point3D(-szer, -szer, -szer, color);

            for (int i = 24; i < 48; i++)
            {
                Matrix4x4 obr = new Matrix4x4();
                obr.setRotationX((float)Math.PI);
                Matrix4x4 trans = new Matrix4x4();

                trans.setTranslation(0, 0, 0);
                pol1[i] = obr * (trans * pol1[i - 24]);
            }

            Matrix4x4 rotate = new Matrix4x4();
            rotate.setIdentity();
            rotate.setRotationZ(0.5f);
            for (int i = 0; i < 48; i++)
            {
                pol1[i] = rotate * pol1[i];
               // pol1[i].color = new Intensity(1, 0, 0);
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
            {
                szescian2D[i] = new Triangle2D();
               
            }
            for (int i2 = 0; i2 < 16; i2++)
            {

                for (int z2 = 0; z2 < 3; z2++)
                {

                    Point3D vert = szescian[i2].v[z2];
                    Point3D temp = afterTrans * vert;
                    temp.color = szescian2D[i2].v[z2].color;
                    //     kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    szescian2D[i2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    //    float beta1a = kula[j2, i2, k2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z)).normalizeProduct());
                    //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                    //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());
                    float beta1a = szescian[i2].fuckinNormal().dot((lightPos - new Vector(szescian[i2].v[z2].x, szescian[i2].v[z2].y, szescian[i2].v[z2].z)).normalizeProduct());
                    // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                   //NOWE
                    Vector do_obs = lightPos - new Vector(szescian[i2].v[z2].x, szescian[i2].v[z2].y, szescian[i2].v[z2].z);
                    Vector I = do_obs.normalizeProduct();
                    Vector N = -szescian[i2].fuckinNormal();
                    Vector R = I - N * (N.dot(I) * 2.0f);
                    //  float cos = I.dot(N);

                    float ss = do_obs.normalizeProduct().dot(R);
                    //?

                    float beta2a;
                    if (ss > 0)
                        beta2a = (float)Math.Pow(ss, 4);
                    else
                        beta2a = 0;


                    // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                    System.Console.WriteLine("BETA2A: " + beta2a);
                    System.Console.WriteLine("Inten spec:" + new Intensity(0.5f, 0.5f, 0.5f) * beta2a);
                    szescian2D[i2].v[z2].color = (vert.color * beta1a) + (new Intensity(0.5f, 0.5f, 0.5f) * beta2a);
                    //EO
                    
                    //szescian2D[i2].v[z2].color = vert.color * beta1a;//EO!!!
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

        public static void renderujJakisShit(ColorBuffer myBuf, Point3D sr, Matrix4x4 afterTrans, Vector lightPos, DepthBuffer db, Intensity kolor_czubka) //ost arg - proj*view
        {
            //rysujemy piramidkę
            //Point3D sr = new Point3D(3, 0, 3);
            //prom = 0.2
            Intensity blue = new Intensity(0, 0, 1);
            Point3D[] pol1 = new Point3D[48];
            Matrix4x4 rot_dol = new Matrix4x4();
            float a20stopni = (float)(20.0 / 180.0 * Math.PI);
            float a40stopni = (float)(40.0 / 180.0 * Math.PI);

            // rot_dol.setRotationX(a20stopni);
            Matrix4x4 rot_left = new Matrix4x4();
            rot_left.setRotationZ(a20stopni);//0.5

            Matrix4x4 rot_left2 = new Matrix4x4();
             rot_left2.setRotationZ(3*a20stopni);//0.5
            float szer = 0.1f;

            /*
             pol1[18] = new Point3D(-szer, szer, szer);//chyba ok!
            pol1[19] = new Point3D(-szer, szer, -szer);
            pol1[20] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0);
            */
            pol1[0] = new Point3D(szer, -szer, szer, blue);//tu jest błąd!
            pol1[1] = new Point3D(szer, -szer, -szer, blue);
            pol1[2] = new Point3D(-szer*0.5f, -szer*0.5f, 0,kolor_czubka);

           //pol1[0]=pol1[1]=pol1[2]= 
               pol1[3] = new Point3D(0, 0, 0);
            pol1[4] = new Point3D(0, 0, 0);
            pol1[5] = new Point3D(0, 0, 0);


            pol1[6] = new Point3D(szer, -szer, -szer, blue);//ten  jest ok
            pol1[7] = new Point3D(-szer, szer, -szer, blue);
            pol1[8] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0,kolor_czubka);

            pol1[9] = new Point3D(0, 0, 0);
            pol1[10] = new Point3D(0, 0, 0);
            pol1[11] = new Point3D(0, 0, 0);
            //1 z 3

            pol1[12] = new Point3D(-szer, szer, szer, blue);//ten jest ok
            pol1[13] = new Point3D(szer, -szer, szer, blue);
            pol1[14] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0,kolor_czubka);

            pol1[15] = new Point3D(0, 0, 0);
            pol1[16] = new Point3D(0, 0, 0);
            pol1[17] = new Point3D(0, 0, 0);


            pol1[18] = new Point3D(-szer, szer, szer, blue);//chyba ok!
            pol1[19] = new Point3D(-szer, szer, -szer, blue);
            pol1[20] = new Point3D(-szer * 0.5f, -szer * 0.5f, 0,kolor_czubka);

       //  pol1[18]=
             pol1[21] = new Point3D(0, 0, 0);
      //  pol1[19]=    
            pol1[22] = new Point3D(0, 0, 0);
     //  pol1[20]=     
           pol1[23] = new Point3D(0, 0, 0);

            

            Matrix4x4 rotate = new Matrix4x4();
            rotate.setIdentity();
            rotate.setRotationX(-0.4f);
            for (int i = 0; i < 24; i++)
            {
                pol1[i] = rotate * pol1[i];
                //pol1[i].color = new Intensity(1, 0, 0);
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
                   // temp.color = vert.color;
                    //     kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);
                    szescian2D[i2].v[z2] = new Point2D(temp.x, temp.y, temp.z,temp.color);
                    vert.color = temp.color;
                    //    float beta1a = kula[j2, i2, k2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z)).normalizeProduct());
                    //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                    //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());
                    float beta1a = szescian[i2].fuckinNormal().dot((lightPos - new Vector(szescian[i2].v[z2].x, szescian[i2].v[z2].y, szescian[i2].v[z2].z)).normalizeProduct());
                    //?
                    Vector do_obs = lightPos - new Vector(szescian[i2].v[z2].x, szescian[i2].v[z2].y, szescian[i2].v[z2].z);
                    Vector I = do_obs.normalizeProduct();
                    Vector N = szescian[i2].fuckinNormal();
                    Vector R = I-N*(N.dot(I)*2.0f);
                  //  float cos = I.dot(N);

                    float ss = do_obs.normalizeProduct().dot(R);
                    //?

                    float beta2a;
                    if (ss > 0)
                        beta2a = (float)Math.Pow(ss, 4);
                    else
                        beta2a = 0;
                    

                    // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                    szescian2D[i2].v[z2].color = (vert.color * beta1a)+(new Intensity(0.5f, 0.5f, 0.5f) * beta2a);//?
                    //System.Console.WriteLine("" + res2.v[z2]);
                }
                for (int i = 0; i < 400; i++)
                    for (int j = 0; j < 400; j++)
                    {
                        Point2D testowany = new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0);
                        if (
                            //kula2D[j2, i2, k2].contains_point(testowany)
                            szescian2D[i2].contains_point(testowany)
                      &&
                        szescian2D[i2].interpolateDepth(testowany.x, testowany.y) < db.depth[i, j]
              
                              )//trzeci parametr jest ignorowany!
                        {
                            myBuf.setPixel(i, j, szescian2D[i2].interpolate(testowany));
                            //zamiast 3 parametru, interpolowana głębokość (funkcja)
                            db.setPixel(i, j, szescian2D[i2].interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f)); //i jego głebokość do bufora głębokości
                        };
                    }
            }
        }

        public static void renderujStozek(ColorBuffer myBuf, Point3D sr, float radius, Matrix4x4 afterTrans, Vector lightPos, int elements, DepthBuffer db, Intensity color) //ost arg - proj*view
        {
            float rad = radius;//0.1
            //rysujemy sferę...
            //Point3D sr = new Point3D(3, 0, 3);
            //prom = 0.2
            Point3D[,] pol1 = new Point3D[4, elements];//1 9>18
            Point3D[,] pol2 = new Point3D[4, elements];//1 9>18
            Matrix4x4 rot_dol = new Matrix4x4();
            
            float a20stopni = (float)((360 / (elements-1)) / 180.0 * Math.PI);
            //            float a40stopni = (float)(20.0 / 180.0 * Math.PI);
            // rot_dol.setRotationX(a20stopni);
            Matrix4x4 rot_left = new Matrix4x4();
            rot_left.setRotationZ(a20stopni);//0.5

            Matrix4x4 rot_left2 = new Matrix4x4();
            // rot_left2.setRotationZ(a20stopni);//0.5
            for (int i = 0; i < 4; i++)//9>18
            {

                rot_dol.setRotationX((i+8) * a20stopni);
                for (int j = 0; j < elements; j++)
                {
                    rot_left2.setRotationZ(j * a20stopni);
                    pol1[i, j] = rot_left2 * (rot_dol * new Point3D(rad, 0, rad));
                    pol2[i, j] = rot_left * pol1[i, j];
                }
            }

            for (int i = 0; i < 4; i++)//9>18
                for (int j = 0; j < elements; j++)
                    pol1[i, j] += sr;
            for (int i = 0; i <4; i++)//9>18
                for (int j = 0; j < elements; j++)
                    pol2[i, j] += sr;

            Triangle3D[, ,] kula = new Triangle3D[elements, 3, 2];//8->16
            Vector[, ,] kulaNormalne = new Vector[elements, 3, 2];
            for (int i = 0; i < 3; i++)//8->16
                for (int j = 0; j < elements; j++)
                {
                    kula[j, i, 0] = new Triangle3D(pol2[i, j], pol1[i, j], pol2[i + 1, j]);
                    kula[j, i, 1] = new Triangle3D(pol1[i, j], pol1[i + 1, j], pol2[i + 1, j]);

                }

            for (int i = 0; i < 3; i++)//8->16
                for (int j = 0; j < elements; j++)
                {
                    kulaNormalne[j, i, 0] = kulaNormalne[j, i, 1] = kula[j, i, 0].fuckinNormal();
                }
            Triangle2D[, ,] kula2D = new Triangle2D[elements, 3, 2];//8->16
            for (int i = 0; i < 3; i++)//8->16
                for (int j = 0; j < elements; j++)
                {
                    kula2D[j, i, 0] = new Triangle2D();
                    kula2D[j, i, 1] = new Triangle2D();

                }
            for (int i2 = 0; i2 < 3; i2++)//8->16
            {
                for (int j2 = 0; j2 < elements; j2++)
                {
                    for (int k2 = 0; k2 < 2; k2++)
                    {
                        for (int z2 = 0; z2 < 3; z2++)
                        {

                            Point3D vert = kula[j2, i2, k2].v[z2]; //obiekt
                            vert.color = color;
                            Point3D temp = afterTrans * vert; //po transformacji
                            kula2D[j2, i2, k2].v[z2] = new Point2D(temp.x, temp.y, temp.z);  //chyba zbędne
                            Vector additional = //wektor od światła do obiektu
                                lightPos -
                                new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z);

                            Vector kierunek = additional.normalizeProduct();
                            //znormalizowany swiatło=obiekt

                            Vector normalna = new Vector(vert.x - sr.x, vert.y - sr.y, vert.z - sr.z).normalizeProduct();// kula[j2, i2, k2].fuckinNormal();
                            //normalna (kula!)

                            float beta1a = normalna.dot(kierunek);
                            //wartość diffuse'a

                            //    float beta2a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[1].x, kula[j2, i2].v[1].y, kula[j2, i2].v[1].z)).normalizeProduct());
                            //    float beta3a = kula[j2, i2].fuckinNormal().dot((lightPos - new Vector(kula[j2, i2].v[2].x, kula[j2, i2].v[2].y, kula[j2, i2].v[2].z)).normalizeProduct());

                            //SPEC?
                            Vector do_obs = additional;// lightPos - new Vector(kula[j2, i2, k2].v[z2].x, kula[j2, i2, k2].v[z2].y, kula[j2, i2, k2].v[z2].z);
                            //światło-obiekt

                            Vector I = kierunek;
                            //znormalizowany swiatło=obiekt

                            Vector N = normalna;
                            //normalna

                            Vector R = I - N * (beta1a * 2.0f);//R = I - N * (N.dot(I) * 2.0f);
                            //  odbity do speculara

                            float ss = do_obs.normalizeProduct().dot(R);
                            //składowa specular

                            float beta2a;
                            if (ss < 0)
                                beta2a = (float)Math.Pow(ss, 8);
                            //alfa jest na sztywno!
                            else
                                beta2a = 0;


                            // kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a;
                            // szescian2D[i2].v[z2].color = (vert.color * beta1a) + (new Intensity(1, 1, 1) * beta2a);
                            //SPEC??
                            kula2D[j2, i2, k2].v[z2].color = vert.color * beta1a + (new Intensity(1, 1, 1) * beta2a);
                            //System.Console.WriteLine("" + res2.v[z2]);
                        }
                        for (int i = 0; i < 400; i++)
                            for (int j = 0; j < 400; j++)
                            {
                                Point2D testowany = new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0);
                                if (
                                    kula2D[j2, i2, k2].contains_point(testowany)
                                      &&
                        kula2D[j2, i2, k2].interpolateDepth(testowany.x, testowany.y) < db.depth[i, j] //i jego głębokość < od zapisanej w buforze gł

                                      )//trzeci parametr jest ignorowany (chyba już nie)
                                {
                                    myBuf.setPixel(i, j, kula2D[j2, i2, k2].interpolate(testowany));
                                    //zamiast 3 parametru, interpolowana głębokość (funkcja)
                                    db.setPixel(i, j, kula2D[j2, i2, k2].interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f)); //i jego głebokość do bufora głębokości
                                };
                            }
                    }
                }
            }
        }
    }
}





