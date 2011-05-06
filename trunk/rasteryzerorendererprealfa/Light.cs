using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rasteryzerorenderer
{
    class Light
    {
        public Light()
        {
            this.position = new Point3D(0, 0, 0);
        }
        public Light(Point3D position)
        {
            this.position = position;
        }
        public Point3D position;
    }
}
