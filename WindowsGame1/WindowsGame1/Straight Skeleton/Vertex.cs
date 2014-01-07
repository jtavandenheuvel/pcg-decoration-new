using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Utilities;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Straight_Skeleton
{
    class Vertex
    {

        private bool active;
        private Point2D point;

        public Edge prevEdge { get; set; }
        public Edge nextEdge { get; set; }
        private Point2D rayDir;
        private Point2D rayStart;
        private int id;

        public Vertex(Point2D point, int id)
        {
            this.active = true;
            this.point = point;
            this.id = id;
        }

        public int getID()
        {
            return id;
        }

        public Point2D getPoint()
        {
            return point;
        }


        private void calcBisector()
        {
            rayStart = new Point2D(LineTools.LineIntersect(prevEdge.getFirstPoint(), prevEdge.getSecondPoint(), nextEdge.getFirstPoint(), nextEdge.getSecondPoint()));

            float angle1 = MathHelper.ToRadians(prevEdge.getOwnAngle());
            float angle2 = MathHelper.ToRadians(nextEdge.getOwnAngle());
            double angle = (angle1 + ((angle2 - angle1 + 2 * Math.PI) % (2 * Math.PI)) / 2) % (Math.PI * 2);

            rayDir = new Point2D((float)-Math.Sin(angle), (float)Math.Cos(angle));
        }

        public string ToString()
        {
            return "Point: [X: " + point.X + ", Y: " + point.Y + "]";
        }

        public Point2D getRayDirection()
        {
            return rayDir;
        }

        public Point2D getRayStart()
        {
            return rayStart;
        }

        /// <summary>
        /// use only if you want to intersect two complete lines 
        /// </summary>
        /// <returns></returns>
        public Point2D getRayStepPoint()
        {
            if (isConvex())
            {
                return new Point2D(getRayStart().X + getRayDirection().X, getRayStart().Y + getRayDirection().Y);
            }
            return new Point2D(getRayStart().X - getRayDirection().X, getRayStart().Y - getRayDirection().Y);
        }

        public override bool Equals(object obj)
        {
            Vertex ver = obj as Vertex;
            if (ver != null && ver.id == this.id)
            {
                return true;
            }
            return false;
        }

        public bool isActive()
        {
            return active;
        }

        public void setActive(bool p)
        {
            active = p;
        }

        internal void update()
        {
            calcBisector();
        }

        public bool isConvex()
        {
            return calcCornerType() == 1;
        }
        public bool isConcave()
        {
            return calcCornerType() == -1;
        }

        /// <summary>
        /// convex = 1, concave = -1, colinear = 0
        /// </summary>
        /// <returns></returns>
        private  int calcCornerType()
        {
            double z; 

            z = (prevEdge.getSecondPoint().X - prevEdge.getFirstPoint().X) * (nextEdge.getSecondPoint().Y - nextEdge.getFirstPoint().Y);
            z -= (prevEdge.getSecondPoint().Y - prevEdge.getFirstPoint().Y) * (nextEdge.getSecondPoint().X - nextEdge.getFirstPoint().X);

            if (z > 0)
                return 1;
            if (z < 0)
                return -1;
            return 0;
        }
    }
}
