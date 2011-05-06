using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    public class Vector
    {
        public float x;
        public float y;
        public float z;

        public Vector(float x, float y, float z)
        {

            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public Vector(Vector v)
        {

            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }


        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ", " +
                z.ToString() + ")";
        }
        /**
         * <summary>
         * normalizuje wektor (nie zwraca)
         * </summary>
         */
        public void normalize()
        {
            float n = this.length();
            if (n != 0)
                this.div(n);
        }
        /**
         * <summary>
         * zwraca znormalizowany wektor
         * </summary>
         */
        public Vector normalizeProduct()
        {
            Vector newV = new Vector(this.x, this.y, this.z);
            float n = this.length();
            if (n != 0)
            {
                newV.div(n);
                return newV;
            }
            else
                return newV;
        }

        public float length()
        {
            return (float)Math.Sqrt(Math.Pow(this.x, 2) + Math.Pow(this.y, 2) + Math.Pow(this.z, 2));
        }
        public float lengthSquared()
        {
            return (float)(Math.Pow(this.x, 2) + Math.Pow(this.y, 2) + Math.Pow(this.z, 2));
        }
        public float dot(Vector v)
        {
            return (this.x * v.x + this.y * v.y + this.z * v.z);
        }
        public Vector cross(Vector v)
        {
            return new Vector(this.y * v.z - this.z * v.y, this.z * v.x - this.x * v.z, this.x * v.y - this.y * v.x);
        }
        public void negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }
        public void add(Vector v)
        {
            this.x += v.x;
            this.y += v.y;
            this.z += v.z;
        }
        public void sub(Vector v)
        {
            this.x -= v.x;
            this.y -= v.y;
            this.z -= v.z;
        }
        public void div(float f)
        {
            if (f != 0)
            {
                this.x /= f;
                this.y /= f;
                this.z /= f;
            }
        }
        public void mag(float f)
        {
            this.x *= f;
            this.y *= f;
            this.z *= f;
        }
        #region Operators
        public static Vector operator *(float scalar, Vector right)
        {
            return new Vector(right.x * scalar, right.y * scalar, right.z * scalar);
        }
        public static Vector operator *(Vector left, float scalar)
        {
            return new Vector(left.x * scalar, left.y * scalar, left.z * scalar);
        }
        public static Vector operator *(Vector left, Vector right)
        {
            return new Vector(left.x * right.x, left.y * right.y, left.z * right.z);
        }
        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(left.x - right.x, left.y - right.y, left.z - right.z);
        }
        public static Vector operator -(Vector left)
        {
            return new Vector(-left.x, -left.y, -left.z);
        }
        public static bool operator ==(Vector left, Vector right)
        {
            return (left.x == right.x && left.y == right.y && left.z == right.z);
        }
        //coś się pitoli
        public static bool operator !=(Vector left, Vector right)
        {
            return (left.x != right.x || left.y != right.y || left.z != right.z);
        }

        public static Vector operator /(Vector left, float scalar)
        {
            Vector v = new Vector();
            float inverse = 1.0f / scalar;

            v.x = left.x * inverse;
            v.y = left.y * inverse;
            v.z = left.z * inverse;

            return v;
        }
        #endregion Operators

        public Vector reflect(Vector normal)
        {
            return this - (2 * this.dot(normal) * normal);
        }
        public Vector lerp(Vector v, float t)
        {
            Vector vector = new Vector();
            vector.x = this.x + t * (v.x - this.x);
            vector.y = this.y + t * (v.y - this.y);
            vector.z = this.z + t * (v.z - this.z);
            return vector;
        }

    }
}
