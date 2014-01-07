using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Utilities;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Straight_Skeleton
{
    class Edge
    {
        private Point2D firstPoint;
        private Point2D secondPoint;

        /// <summary>
        /// Define edge from starting point to end point with Point2D
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        public Edge(Point2D firstPoint, Point2D secondPoint)
        {
            this.firstPoint = firstPoint;
            this.secondPoint = secondPoint;
        }

        /// <summary>
        /// Get's the first point of the edge
        /// </summary>
        /// <returns>POINT2D</returns>
        public Point2D getFirstPoint()
        {
            return firstPoint;
        }

        /// <summary>
        /// Get's the second point of the edge
        /// </summary>
        /// <returns>POINT2D</returns>
        public Point2D getSecondPoint()
        {
            return secondPoint;
        }

        public float getOwnAngle(){
            double xDiff = secondPoint.X - firstPoint.X;
            double yDiff= secondPoint.Y - firstPoint.Y; 
            return MathHelper.ToDegrees((float)Math.Atan2(yDiff,xDiff));
        }
    }
}
