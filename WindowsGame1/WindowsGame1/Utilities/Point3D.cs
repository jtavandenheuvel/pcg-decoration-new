using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Utilities
{
    public class Point3D : OwnVector3
    {
        public List<Point3D> linkedPoints { get; set; }
        public bool drawn { get; set; }
        public int ID { get; set; }

        public Point3D(float value)
            : base(value)
        {
            init();
        }

        public Point3D(float x, float y, float z)
            : base(x, y, z)
        {
            init();
        }

        public Point3D(Vector2 value, float z)
            : base(value, z)
        {
            init();
        }

        public Point3D(OwnVector3 vector)
            : base(vector.X, vector.Y, vector.Z)
        {
             init();
        }


        private void init()
        {
            linkedPoints = new List<Point3D>();
            drawn = false;
        }

        public bool hasLinkedPoints()
        {
            return linkedPoints.Count != 0;
        }
    }
}
