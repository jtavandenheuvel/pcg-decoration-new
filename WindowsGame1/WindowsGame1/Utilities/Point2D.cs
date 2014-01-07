using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics;

namespace WindowsGame1.Utilities
{

    public class Point2D : OwnVector2
    {
        public List<Point2D> linkedPoints {get;set;}
        public Settings.SegmentType segmentType { get; set; }
        public bool drawn { get; set; }
        public int ID { get; set; }

        public Point2D(float value) 
            : base(value)
        {
            init();
        }

        public Point2D(float x, float y)
            : base(x, y)
        {
            init();
        }

        public Point2D(OwnVector2 vector)
            : base(vector.X, vector.Y)
        {
            init();
        }

        private void init()
        {
            linkedPoints = new List<Point2D>();
            drawn = false;
        }

        public bool hasLinkedPoints()
        {
            return linkedPoints.Count != 0;
        }
    }
}
