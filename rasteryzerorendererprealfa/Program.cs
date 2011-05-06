using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace rasteryzerorenderer
{
    class Program
    {
        static void Main(string[] args)
        {

            #region bazgroły
            Triangle3D tr = new Triangle3D(0, 0, 0, 0, 1, 0, 0, 0, 1);
            Matrix4x4 ident = new Matrix4x4();
            ident.setIdentity();
           // ident.setRotationX(45*(float)Math.PI/180);//to jest ok
            ident.show();
            ident.setTranslation(2, 3, 4);
            System.Console.WriteLine();
            Point3D p = new Point3D(0, 0, 1);
            System.Console.WriteLine("" + ident*p);
            System.Console.WriteLine("" + tr);
            tr.multiplyLeftByMatrix(ident);
            System.Console.WriteLine("" + tr);
            #endregion bazgroły


            //PARAMETRY WIDOKU I ŚWIATŁA!!!
            Vector observer = new Vector(0, 0, 0);
            Vector lookAt = new Vector(1, 0, 1);

            System.Console.WriteLine("TEST PRZEKSZTAŁCEŃ");
            Matrix4x4 view = new Matrix4x4();
            view.setAsViewMatrix(
                observer,
                lookAt,
                new Vector(0, 1, 0));
            Matrix4x4 viewRot = new Matrix4x4();
            viewRot.setIdentity();
            viewRot.setRotationY(0.1f);
            
            view = view.multiply(viewRot);
            System.Console.WriteLine("View:");
            view.show();
            System.Console.WriteLine();
            Matrix4x4 proj = new Matrix4x4();
            proj.setAsProjectionMatrix(45, 0.05f, 100f);
            System.Console.WriteLine("Proj:");
            proj.show();

            Point3D test  = new Point3D(1.2f, 0f,  0.8f);
            System.Console.WriteLine("Proj view test:");
            Matrix4x4 afterTrans =(proj.multiply(view));
            afterTrans.show();
            test = afterTrans * test;

            //dotąd czysty debug
           // Triangle3D
            Triangle3D troj = new Triangle3D(
                new Point3D(1.0f,0.0f,1.1f,new Intensity(Color.Red)),
                new Point3D(1.1f, 0.0f, 1.0f, new Intensity(Color.Red)),
                new Point3D(1f, 0.3f, 1f, new Intensity(Color.Blue))
            );
            Triangle3D troj2 = new Triangle3D(
                new Point3D(1.0f, 0.3f, 1.1f, new Intensity(Color.Fuchsia)),
                new Point3D(1.1f, 0.3f, 1.0f, new Intensity(Color.Fuchsia)),
                new Point3D(1.0f, 0.0f, 1.0f, new Intensity(Color.Fuchsia)));

            //np. tu można dodać pozycję i kolor światła i powinno wyjsć oświetlenie
            //observer jest w 0,0,0
         //   Vector observer = new Vector(0, 0, 0);
            //światło: 0.1,0.0,-0.1
            Vector lightPos = new Vector(0.6f, -0.1f, 0.9f);
            //kolor światła: 1.0,1.0,1.0
            System.Console.WriteLine("Norm:"+troj.fuckinNormal());
            float beta1 = troj2.fuckinNormal().dot((lightPos - new Vector(troj2.v[0].x,troj2.v[0].y,troj2.v[0].z)).normalizeProduct());
            System.Console.WriteLine("hm:" + beta1+", ");
            float beta2 = troj2.fuckinNormal().dot((lightPos - new Vector(troj2.v[1].x, troj2.v[1].y, troj2.v[1].z)).normalizeProduct());
            float beta3 = troj2.fuckinNormal().dot((lightPos - new Vector(troj2.v[2].x, troj2.v[2].y, troj2.v[2].z)).normalizeProduct());
            troj2.v[0].color *= beta1;
            System.Console.WriteLine(troj2.v[0].color.R);
            System.Console.WriteLine(troj2.v[0].color.G);//???!!!
          //  System.Console.ReadKey();
            troj2.v[1].color *= beta2;
            System.Console.WriteLine(troj2.v[1].color.R);
            System.Console.WriteLine(troj2.v[1].color.G);
            troj2.v[2].color *= beta3;
            System.Console.WriteLine(troj2.v[2].color.R);
            System.Console.WriteLine(troj2.v[2].color.G);
           // System.Console.ReadKey();
            //sprawdzić poprawność interpolateDepth()
            Triangle2D res = new Triangle2D();
            for(int i=0;i<3;i++) //przetwarzanie 3D na 2D
            {
                Point3D vert = troj.v[i];
                res.v[i] = new Point2D((afterTrans * vert).x, (afterTrans * vert).y, (afterTrans * vert).z);
                res.v[i].color = troj.v[i].color;
                System.Console.WriteLine(""+res.v[i]);
            }
            Triangle2D res2 = new Triangle2D();
            for (int i = 0; i < 3; i++) //przetwarzanie 3D na 2D
            {
                Point3D vert = troj2.v[i];
                res2.v[i] = new Point2D((afterTrans * vert).x, (afterTrans * vert).y, (afterTrans * vert).z);
                res2.v[i].color = troj2.v[i].color;
                System.Console.WriteLine("" + res2.v[i]);
            }



            ColorBuffer myBuf = new ColorBuffer(400, 400);
            DepthBuffer db = new DepthBuffer(400, 400);//konstruktor inicjuje +niesk


            myBuf.clearBuffer(Color.White); //myBuf 100 x 100
            for (int i = 0; i < 400; i++)
                for (int j = 0; j < 400; j++)
                    if (
                        res.contains_point(new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0))
                        &&
                        res.interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f)<db.depth[i,j]
                        )//trzeci parametr jest ignorowany!
                    {
                        myBuf.setPixel(i, j, res.interpolate(new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 
                            res.interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f))));
                            //zamiast 3 parametru, interpolowana głębokość (funkcja)
                        db.setPixel(i,j,res.interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f));
                    };

            for (int i = 0; i < 400; i++)
                for (int j = 0; j < 400; j++)
                    if (
                        res2.contains_point(new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0))
                        &&
                        res2.interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f) < db.depth[i, j]
                        )//trzeci parametr jest ignorowany!
                    {
                        myBuf.setPixel(i, j, res2.interpolate(new Point2D(-1 + i * 0.005f, -1 + j * 0.005f,
                            res2.interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f))));
                        //zamiast 3 parametru, interpolowana głębokość (funkcja)
                        db.setPixel(i, j, res2.interpolateDepth(-1 + i * 0.005f, -1 + j * 0.005f));
                    };
            /*
            for (int i = 0; i < 400; i++)
                for (int j = 0; j < 400; j++)
                    if (res2.contains_point(new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0)))//trzeci parametr jest ignorowany!
                        myBuf.setPixel(i, j, res2.interpolate(new Point2D(-1 + i * 0.005f, -1 + j * 0.005f, 0)));//zamiast 3 parametru, interpolowana głębokość (funkcja)
           */

           //stąd
          //  System.Console.WriteLine("Zapierdalam pierwszą sferę.");
          //  Renderer.renderujSfere(myBuf, new Point3D(2, 0, 2), 0.1f, afterTrans, lightPos);
          //  System.Console.WriteLine("Zapierdalam drugą sferę.");
          //  Renderer.renderujSfere(myBuf, new Point3D(1.9f, 0, 2.2f), 0.1f, afterTrans, lightPos);
            System.Console.WriteLine("Zapierdalam sześcian.");
            Renderer.renderujSszescian(myBuf, new Point3D(2.7f, -0.2f, 3.0f), 0.05f, afterTrans, lightPos);
            System.Console.WriteLine("Zapierdalam jakieś nie wiadomo co.");
            Renderer.renderujJakisShit(myBuf, new Point3D(0.5f, 0.0f, 1.0f), 0.05f, afterTrans, lightPos);
            
            myBuf.bmp.Save(@"C:\res.bmp", ImageFormat.Bmp);
       


           // System.Console.ReadKey();

            
            
        }
    }
}
