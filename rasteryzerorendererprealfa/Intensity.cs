using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace rasteryzerorenderer
{
    class Intensity
    {
        float r;
        float g;
        float b;

        #region Properties

        public float R
        {
            get { return r; }
            set { if (r > 1) r = 1; else if (r < 0) r = 0; else r = value; }
        }

        public float G
        {
            get { return g; }
            set { if (g > 1)g = 1; else if (g < 0) g = 0; else g = value; }
        }

        public float B
        {
            get { return b; }
            set { if (b > 1) b = 1; else if (b < 0) b = 0; else b = value; }
        }

        #endregion Properties

        #region Constructors

        public Intensity()
        {
            this.R = 0;
            this.G = 0;
            this.B = 0;
        }

        public Intensity(float r, float g, float b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public Intensity(Intensity I)
        {
            this.R = I.R;
            this.G = I.G;
            this.B = I.B;
        }

        public Intensity(Color color)
        {
            this.R = ((float)color.R) / 255.0f;
            this.G = ((float)color.G) / 255.0f;
            this.B = ((float)color.B) / 255.0f;

        }

        #endregion Constructors

        #region Methods

        public void addValues(float r, float g, float b)
        {
            //hm...
            if (R + r <= 1)
            {
                if (R + r >= 0)
                {
                    this.R += r;
                }
                else this.R = 0;
            }
            else
                this.R = 1;
            //hm...
            if (G + g <= 1)
            {
                if (G + g >= 0)
                {
                    this.G += g;
                }
                else this.G = 0;
            }
            else
                this.G = 1;
            //hm...
            if (B + b <= 1)
            {
                if (B + b >= 0)
                {
                    this.B += b;
                }
                else this.B = 0;
            }
            else
                this.B = 1;
            //  this.B += b;
        }


        public void divide(int p)
        {
            this.R /= p;
            this.G /= p;
            this.B /= p;
        }
        public static Intensity operator * (Intensity arg1, Intensity arg2)
        {
            return new Intensity(arg1.R * arg2.R, arg1.G * arg2.G, arg1.B * arg2.B);
        }
        public static Intensity operator +(Intensity i1, Intensity i2)
        {
            return new Intensity(
                Math.Min(1, i1.R + i2.R),
                Math.Min(1, i1.G + i2.G),
                Math.Min(1, i1.B + i2.B));
        }
        public static Intensity operator *(Intensity arg1, float arg2)
        {
            return new Intensity(arg1.R * arg2, arg1.G * arg2, arg1.B * arg2);
        }
        public static Intensity operator *(float arg1, Intensity arg2)
        {
            return new Intensity(arg2.R * arg1, arg2.G * arg1, arg2.B * arg1);
        }

        #endregion Methods
        public override string ToString()
        {
            return "(R="+R+",G="+G+",B="+B+")";
        }
    }
}
