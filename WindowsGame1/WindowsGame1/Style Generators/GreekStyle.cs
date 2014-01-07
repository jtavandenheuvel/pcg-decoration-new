using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Utilities;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Style_Generators
{
    public class GreekStyle : StyleGenerator
    {
        public int maximumSegmentSize { get; set; }
        private float distance;

        public GreekStyle() 
        {
            this.maximumSegmentSize = 30;
            this.distance = 10;
        }

        public GreekStyle(float drawDistance)
        {
            this.maximumSegmentSize = 30;
            this.distance = drawDistance;
        }

        public List<OwnVector3> createNormalSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            OwnVector3 direction = centerPoint - previousPoint;
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 startPoint = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f);
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);
            List<OwnVector3> ret = new List<OwnVector3>();

            //Add upper line
            OwnVector3 upper1 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), 2 * distance);
            OwnVector3 upper2 = DrawTools.MoveForward(upper1, norDirection, OwnVector3.Distance(startPoint, endPoint));
            ret.Add(upper1);
            ret.Add(upper2);

            //Add lower line
            OwnVector3 lower1 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), 2 * distance);
            OwnVector3 lower2 = DrawTools.MoveForward(lower1, norDirection, OwnVector3.Distance(startPoint, endPoint));
            ret.Add(lower1);
            ret.Add(lower2);

            //Add middle section 
            OwnVector3 p1 = startPoint;
            OwnVector3 p2 = DrawTools.MoveForward(p1, DrawTools.TurnLeft(90, norDirection), distance);
            OwnVector3 p3 = DrawTools.MoveForward(p2, norDirection, OwnVector3.Distance(startPoint, endPoint)*0.7f);
            OwnVector3 p4 = DrawTools.MoveForward(p3, DrawTools.TurnRight(90, norDirection), distance);
            OwnVector3 p5 = DrawTools.MoveBackward(p4, norDirection, OwnVector3.Distance(startPoint, endPoint) * 0.4f);
            OwnVector3 p6 = DrawTools.MoveForward(p5, DrawTools.TurnRight(90, norDirection), distance);
            OwnVector3 p7 = DrawTools.MoveForward(p6, norDirection, OwnVector3.Distance(startPoint, endPoint) * 0.7f);
            OwnVector3 p8 = DrawTools.MoveForward(p7, DrawTools.TurnLeft(90, norDirection), distance);

            ret.Add(p1);
            ret.Add(p2);
            ret.Add(p2);
            ret.Add(p3);
            ret.Add(p3);
            ret.Add(p4);
            ret.Add(p4);
            ret.Add(p5);
            ret.Add(p5);
            ret.Add(p6);
            ret.Add(p6);
            ret.Add(p7);
            ret.Add(p7);
            ret.Add(p8);

            /*
            OwnVector3 p1 = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref previousPoint, 0.5f);
            OwnVector3 vectorP = LineTools.getPerpendicalurDirectionVector(ref p1, ref centerPoint);

            OwnVector3 p2 = p1 - (distance * vectorP);
            OwnVector3 p4 = OwnVector3.Lerp(centerPoint, nextPoint, 0.2f);
            OwnVector3 p3 = p4 - (distance * vectorP);

            OwnVector3 n1 = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref previousPoint, 0.5f);
            OwnVector3 vectorN = LineTools.getPerpendicalurDirectionVector(ref n1, ref centerPoint);
            OwnVector3 n2 = n1 + (distance * vectorP);
            OwnVector3 n4 = OwnVector3.Lerp(previousPoint, centerPoint, 0.8f);
            OwnVector3 n3 = n4 + (distance * vectorP);
             * */

            return ret;
        }

        public List<OwnVector3> createEndSegment(OwnVector3 centerPoint, OwnVector3 nextPoint)
        {
            OwnVector3 direction = nextPoint - centerPoint;
            OwnVector3 norDirection = direction / direction.Length();   
            OwnVector3 startPoint = centerPoint;
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);

            List<OwnVector3> ret = new List<OwnVector3>();

            //Add upper line
            OwnVector3 upper1 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance * 2);
            OwnVector3 upper2 = DrawTools.MoveForward(upper1, norDirection, OwnVector3.Distance(startPoint, endPoint));
            ret.Add(upper1);
            ret.Add(upper2);

            //Add lower line
            OwnVector3 lower1 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), distance * 2);
            OwnVector3 lower2 = DrawTools.MoveForward(lower1, norDirection, OwnVector3.Distance(startPoint, endPoint));
            ret.Add(lower1);
            ret.Add(lower2);

            //Add border line 
            ret.Add(lower1);
            ret.Add(upper1);

            //Add middle line
            OwnVector3 p1 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), distance);
            OwnVector3 p2 = DrawTools.MoveForward(p1, norDirection, OwnVector3.Distance(startPoint, endPoint));
            OwnVector3 p3 = endPoint;
            ret.Add(p1);
            ret.Add(p2);
            ret.Add(p2);
            ret.Add(p3);

            return ret;
        }

        public List<OwnVector3> createCornerSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {

            //return createNormalSegment(centerPoint, previousPoint, nextPoint);

            // Get points
            //return createCorner1(centerPoint, previousPoint, nextPoint);
            return createCorner2(centerPoint, previousPoint, nextPoint);
        }

        private List<OwnVector3> createCorner2(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            OwnVector2 vector1 = new OwnVector2(previousPoint.X, previousPoint.Y) - new OwnVector2(centerPoint.X, centerPoint.Y);
            OwnVector2 vector2 = new OwnVector2(nextPoint.X, nextPoint.Y) - new OwnVector2(centerPoint.X, centerPoint.Y);

            float angle = (float)MathUtils.VectorAngle(ref vector1, ref vector2);

            List<OwnVector3> ret = new List<OwnVector3>();
            OwnVector3 direction = centerPoint - previousPoint;
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 direction2 = centerPoint - nextPoint;
            OwnVector3 norDirection2 = direction2 / direction2.Length();
            OwnVector3 startPoint = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f);
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);
            OwnVector3 upper1, upper2, upper3, lower1, lower2, intersectionPoint, intersectionPoint2;

            if (angle > 0)
            {

                upper2 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance * 2);
                upper3 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection2), distance * 2);
                intersectionPoint = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(upper2.X, upper2.Y),
                    new Point2D(upper2.X + norDirection.X, upper2.Y + norDirection.Y),
                    new Point2D(upper3.X, upper3.Y),
                    new Point2D(upper3.X + norDirection2.X, upper3.Y + norDirection2.Y)), 0);

                if (OwnVector3.Distance(centerPoint, intersectionPoint) > distance * 3)
                {
                    upper1 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance * 3);
                }
                else
                {
                    upper1 = intersectionPoint;
                }

                lower1 = DrawTools.MoveBackward(startPoint, DrawTools.TurnLeft(90, norDirection), distance * 2);
                lower2 = DrawTools.MoveBackward(endPoint, DrawTools.TurnRight(90, norDirection2), distance * 2);
                intersectionPoint2 = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(lower1.X, lower1.Y),
                    new Point2D(lower1.X + norDirection.X, lower1.Y + norDirection.Y),
                    new Point2D(lower2.X, lower2.Y),
                    new Point2D(lower2.X + norDirection2.X, lower2.Y + norDirection2.Y)), 0);

            }
            else
            {
                upper1 =
                upper2 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), distance * 2);
                upper3 = DrawTools.MoveForward(endPoint, DrawTools.TurnLeft(90, norDirection2), distance * 2);
                intersectionPoint = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(upper2.X, upper2.Y),
                    new Point2D(upper2.X + norDirection.X, upper2.Y + norDirection.Y),
                    new Point2D(upper3.X, upper3.Y),
                    new Point2D(upper3.X + norDirection2.X, upper3.Y + norDirection2.Y)), 0);
                if (OwnVector3.Distance(centerPoint, intersectionPoint) > distance * 3)
                {
                    upper1 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance * 3);
                }
                else
                {
                    upper1 = intersectionPoint;
                }
                lower1 = DrawTools.MoveBackward(startPoint, DrawTools.TurnRight(90, norDirection), distance * 2);
                lower2 = DrawTools.MoveBackward(endPoint, DrawTools.TurnLeft(90, norDirection2), distance * 2);
                intersectionPoint2 = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(lower1.X, lower1.Y),
                    new Point2D(lower1.X + norDirection.X, lower1.Y + norDirection.Y),
                    new Point2D(lower2.X, lower2.Y),
                    new Point2D(lower2.X + norDirection2.X, lower2.Y + norDirection2.Y)), 0);
            }

            ret.Add(upper2);
            ret.Add(upper1);
            ret.Add(upper1);
            ret.Add(upper3);
            bool testValue = (OwnVector3.Distance(intersectionPoint2, centerPoint) <= Math.Sqrt(Math.Pow(distance * 2, 2) + Math.Pow(OwnVector3.Distance(startPoint, centerPoint), 2)));
            if (testValue)
            {

                ret.Add(lower1);
                ret.Add(intersectionPoint2);
                ret.Add(intersectionPoint2);
                ret.Add(lower2);
            }

            OwnVector3 start2 = OwnVector3.Lerp(startPoint, centerPoint, 0.5f);
            OwnVector3 end2 = OwnVector3.Lerp(endPoint, centerPoint, 0.5f);

            


            OwnVector3 p1, p2, p3, p4, p5, p6;
            if (angle > 0)
            {
                p3 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), OwnVector3.Distance(centerPoint, upper1) / 2);
                p6 = DrawTools.MoveBackward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), OwnVector3.Distance(centerPoint, intersectionPoint2) / 2);
            }
            else
            {
                p3 = DrawTools.MoveBackward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), OwnVector3.Distance(centerPoint, intersectionPoint2) / 2);
                p6 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), OwnVector3.Distance(centerPoint, upper1) / 2);
            }

            if (testValue)
            {
                p1 = DrawTools.MoveForward(start2, DrawTools.TurnLeft(90, norDirection), distance);
                p2 = DrawTools.MoveForward(start2, DrawTools.TurnRight(90, norDirection), distance);
                ret.Add(p1);
                ret.Add(p2);
                ret.Add(previousPoint - (startPoint - previousPoint));
                ret.Add(OwnVector3.Lerp(p1,p2, 0.5f));

                p4 = DrawTools.MoveForward(end2, DrawTools.TurnLeft(90, norDirection2), distance);
                p5 = DrawTools.MoveForward(end2, DrawTools.TurnRight(90, norDirection2), distance);
                ret.Add(p4);
                ret.Add(p5);
                ret.Add(nextPoint + (nextPoint - endPoint));
                ret.Add(OwnVector3.Lerp(p4, p5, 0.5f));

                ret.Add(p1);
                ret.Add(p3);
                ret.Add(p3);
                ret.Add(p5);

                ret.Add(p2);
                ret.Add(p6);
                ret.Add(p6);
                ret.Add(p4);

                ret.Add(DrawTools.MoveBackward(upper2, norDirection, OwnVector3.Distance(previousPoint, startPoint) * 2));
                ret.Add(upper2);

                ret.Add(DrawTools.MoveBackward(lower1, norDirection, OwnVector3.Distance(previousPoint, startPoint) * 2));
                ret.Add(lower1);

                ret.Add(DrawTools.MoveBackward(upper3, norDirection2, OwnVector3.Distance(endPoint, nextPoint) * 2));
                ret.Add(upper3);

                ret.Add(DrawTools.MoveBackward(lower2, norDirection2, OwnVector3.Distance(endPoint, nextPoint) * 2));
                ret.Add(lower2);
            }
            else
            {
                

                ret.Add(DrawTools.MoveBackward(upper2, norDirection, OwnVector3.Distance(previousPoint, startPoint) * 2));
                ret.Add(upper2);

                ret.Add(DrawTools.MoveBackward(lower1, norDirection, OwnVector3.Distance(previousPoint, startPoint) * 2));
                ret.Add(intersectionPoint2);

                ret.Add(DrawTools.MoveBackward(upper3, norDirection2, OwnVector3.Distance(endPoint, nextPoint) * 2));
                ret.Add(upper3);

                ret.Add(DrawTools.MoveBackward(lower2, norDirection2, OwnVector3.Distance(endPoint, nextPoint) * 2));
                ret.Add(intersectionPoint2);

                if (angle > 0)
                {
                    p1 = DrawTools.MoveForward(start2, DrawTools.TurnLeft(90, norDirection), distance);
                    p2 = DrawTools.MoveForward(end2, DrawTools.TurnRight(90, norDirection2), distance);
                    OwnVector2 intersect = LineTools.LineIntersect(new Point2D(p1.X, p1.Y), new Point2D(p6.X, p6.Y), new Point2D(startPoint.X, startPoint.Y), new Point2D(centerPoint.X, centerPoint.Y));
                    ret.Add(previousPoint - (startPoint - previousPoint));
                    ret.Add(new OwnVector3(intersect.X, intersect.Y, 0));


                    intersect = LineTools.LineIntersect(new Point2D(p2.X, p2.Y), new Point2D(p6.X, p6.Y), new Point2D(endPoint.X, endPoint.Y), new Point2D(centerPoint.X, centerPoint.Y));
                    ret.Add(nextPoint + (nextPoint - endPoint));
                    ret.Add(new OwnVector3(intersect.X, intersect.Y, 0));


                    ret.Add(p1);
                    ret.Add(p6);

                    ret.Add(p6);
                    ret.Add(p2);



                    ret.Add(p1);
                    ret.Add(p3); 
                    ret.Add(p3);
                    ret.Add(p2); 

                }
                else
                {
                    p1 = DrawTools.MoveForward(start2, DrawTools.TurnRight(90, norDirection), distance);
                    p2 = DrawTools.MoveForward(end2, DrawTools.TurnLeft(90, norDirection2), distance);
                    OwnVector2 intersect = LineTools.LineIntersect(new Point2D(p1.X, p1.Y), new Point2D(p3.X, p3.Y), new Point2D(startPoint.X, startPoint.Y), new Point2D(centerPoint.X, centerPoint.Y));
                    ret.Add(previousPoint - (startPoint - previousPoint));
                    ret.Add(new OwnVector3(intersect.X, intersect.Y, 0));


                    intersect = LineTools.LineIntersect(new Point2D(p2.X, p2.Y), new Point2D(p3.X, p3.Y), new Point2D(endPoint.X, endPoint.Y), new Point2D(centerPoint.X, centerPoint.Y));
                    ret.Add(nextPoint + (nextPoint - endPoint));
                    ret.Add(new OwnVector3(intersect.X, intersect.Y, 0));

                    ret.Add(DrawTools.MoveForward(start2, DrawTools.TurnRight(90, norDirection), distance));
                    ret.Add(p3);

                    ret.Add(p3);
                    ret.Add(DrawTools.MoveForward(end2, DrawTools.TurnLeft(90, norDirection2), distance));
                    
                    ret.Add(DrawTools.MoveForward(start2, DrawTools.TurnRight(90, norDirection), distance));
                    ret.Add(p6); 
                    ret.Add(p6);
                    ret.Add(DrawTools.MoveForward(end2, DrawTools.TurnLeft(90, norDirection2), distance));
                }
            }
            

            return ret;
        }

        private List<OwnVector3> createCorner1(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            OwnVector2 vector1 = new OwnVector2(previousPoint.X, previousPoint.Y) - new OwnVector2(centerPoint.X, centerPoint.Y);
            OwnVector2 vector2 = new OwnVector2(nextPoint.X, nextPoint.Y) - new OwnVector2(centerPoint.X, centerPoint.Y);

            float angle = (float)MathUtils.VectorAngle(ref vector1, ref vector2);

            List<OwnVector3> ret = new List<OwnVector3>();
            OwnVector3 direction = centerPoint - previousPoint;
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 direction2 = centerPoint - nextPoint;
            OwnVector3 norDirection2 = direction2 / direction.Length();
            OwnVector3 startPoint = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f);
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);
            OwnVector3 upper1, upper2, upper3, lower1, lower2, intersectionPoint, intersectionPoint2;

            if (angle > 0)
            {

                upper2 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance * 2);
                upper3 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection2), distance * 2);
                intersectionPoint = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(upper2.X, upper2.Y),
                    new Point2D(upper2.X + norDirection.X, upper2.Y + norDirection.Y),
                    new Point2D(upper3.X, upper3.Y),
                    new Point2D(upper3.X + norDirection2.X, upper3.Y + norDirection2.Y)), 0);

                if (OwnVector3.Distance(centerPoint, intersectionPoint) > distance * 3)
                {
                    upper1 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance * 3);
                }
                else
                {
                    upper1 = intersectionPoint;
                }

                lower1 = DrawTools.MoveBackward(startPoint, DrawTools.TurnLeft(90, norDirection), distance * 2);
                lower2 = DrawTools.MoveBackward(endPoint, DrawTools.TurnRight(90, norDirection2), distance * 2);
                intersectionPoint2 = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(lower1.X, lower1.Y),
                    new Point2D(lower1.X + norDirection.X, lower1.Y + norDirection.Y),
                    new Point2D(lower2.X, lower2.Y),
                    new Point2D(lower2.X + norDirection2.X, lower2.Y + norDirection2.Y)), 0);

            }
            else
            {
                upper1 =
                upper2 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), distance * 2);
                upper3 = DrawTools.MoveForward(endPoint, DrawTools.TurnLeft(90, norDirection2), distance * 2);
                intersectionPoint = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(upper2.X, upper2.Y),
                    new Point2D(upper2.X + norDirection.X, upper2.Y + norDirection.Y),
                    new Point2D(upper3.X, upper3.Y),
                    new Point2D(upper3.X + norDirection2.X, upper3.Y + norDirection2.Y)), 0);
                if (OwnVector3.Distance(centerPoint, intersectionPoint) > distance * 3)
                {
                    upper1 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance * 3);
                }
                else
                {
                    upper1 = intersectionPoint;
                }
                lower1 = DrawTools.MoveBackward(startPoint, DrawTools.TurnRight(90, norDirection), distance * 2);
                lower2 = DrawTools.MoveBackward(endPoint, DrawTools.TurnLeft(90, norDirection2), distance * 2);
                intersectionPoint2 = new OwnVector3(LineTools.LineIntersect(
                    new Point2D(lower1.X, lower1.Y),
                    new Point2D(lower1.X + norDirection.X, lower1.Y + norDirection.Y),
                    new Point2D(lower2.X, lower2.Y),
                    new Point2D(lower2.X + norDirection2.X, lower2.Y + norDirection2.Y)), 0);
            }

            ret.Add(upper2);
            ret.Add(upper1);
            ret.Add(upper1);
            ret.Add(upper3);
            if (OwnVector3.Distance(intersectionPoint2, centerPoint) <= Math.Sqrt(Math.Pow(distance * 2, 2) + Math.Pow(OwnVector3.Distance(startPoint, centerPoint), 2)))
            {

                ret.Add(lower1);
                ret.Add(intersectionPoint2);
                ret.Add(intersectionPoint2);
                ret.Add(lower2);
            }

            OwnVector3 p3, p6;
            if (angle > 0)
            {
                p3 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance);
                p6 = DrawTools.MoveBackward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance);
            }
            else
            {
                p3 = DrawTools.MoveBackward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance);
                p6 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(MathHelper.ToDegrees(angle / 2), norDirection), distance);
            }
            OwnVector3 p2 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance);
            OwnVector3 p5 = DrawTools.MoveForward(endPoint, DrawTools.TurnLeft(90, norDirection2), distance);
            OwnVector3 p7 = DrawTools.MoveForward(p6, DrawTools.TurnRight(90, norDirection2), distance);


            ret.Add(startPoint);
            ret.Add(p2);
            ret.Add(p2);
            ret.Add(p3);

            ret.Add(endPoint);
            ret.Add(p5);
            ret.Add(p5);
            ret.Add(p6);

            ret.Add(p3);
            ret.Add(p6);

            return ret;
        }
        public List<OwnVector3> createTJunctionSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints)
        {
            List<OwnVector3> ret = new List<OwnVector3>();
            return ret;
        }
        public List<OwnVector3> createCrossingSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints)
        {
            OwnVector3 startPoint = OwnVector3.Lerp(centerPoint, connectingPoints[0], 0.5f);
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, connectingPoints[1], 0.5f);
            OwnVector3 startPoint2 = OwnVector3.Lerp(centerPoint, connectingPoints[2], 0.5f);
            OwnVector3 endPoint2 = OwnVector3.Lerp(centerPoint, connectingPoints[3], 0.5f);


            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector3 p1 = OwnVector3.Lerp(centerPoint, connectingPoints[0], 0.7f);
            OwnVector3 p2 = OwnVector3.Lerp(centerPoint, connectingPoints[1], 0.7f);
            OwnVector3 p3 = OwnVector3.Lerp(centerPoint, connectingPoints[2], 0.7f);
            OwnVector3 p4 = OwnVector3.Lerp(centerPoint, connectingPoints[3], 0.7f);
            OwnVector3 p5 = connectingPoints[0] - (startPoint - connectingPoints[0]);
            OwnVector3 p6 = connectingPoints[1] - (endPoint - connectingPoints[1]);
            OwnVector3 p7 = connectingPoints[2] - (startPoint2 - connectingPoints[2]);
            OwnVector3 p8 = connectingPoints[3] - (endPoint2 - connectingPoints[3]);

            ret.Add(p1);
            ret.Add(p4);
            ret.Add(p2);
            ret.Add(p3);

            ret.Add(p1);
            ret.Add(p3);
            ret.Add(p2);
            ret.Add(p4);

            ret.Add(p5);
            ret.Add(p1);

            ret.Add(p6);
            ret.Add(p2);

            ret.Add(p7);
            ret.Add(p3);

            ret.Add(p8);
            ret.Add(p4);

            OwnVector3 direction = centerPoint - p5;
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 direction2 = centerPoint - p6;
            OwnVector3 norDirection2 = direction2 / direction2.Length();
            OwnVector3 direction3 = centerPoint - p7;
            OwnVector3 norDirection3 = direction3 / direction3.Length();
            OwnVector3 direction4 = centerPoint - p8;
            OwnVector3 norDirection4 = direction4 / direction4.Length();

            OwnVector3 p9 = DrawTools.MoveForward(p5, DrawTools.TurnRight(90, norDirection), 2 * distance);
            OwnVector3 p10 = DrawTools.MoveForward(p8, DrawTools.TurnLeft(90, norDirection4), 2 * distance);
            OwnVector3 p11 = DrawTools.MoveForward(p6, DrawTools.TurnRight(90, norDirection2), 2 * distance);
            OwnVector3 p12 = DrawTools.MoveForward(p7, DrawTools.TurnLeft(90, norDirection3), 2 * distance);
            OwnVector3 p13 = DrawTools.MoveForward(p5, DrawTools.TurnLeft(90, norDirection), 2 * distance);
            OwnVector3 p14 = DrawTools.MoveForward(p7, DrawTools.TurnRight(90, norDirection3), 2 * distance);
            OwnVector3 p15 = DrawTools.MoveForward(p6, DrawTools.TurnLeft(90, norDirection2), 2 * distance);
            OwnVector3 p16 = DrawTools.MoveForward(p8, DrawTools.TurnRight(90, norDirection4), 2 * distance);

            Point2D intersectionPoint = new Point2D(0,0);
            
            LineTools.LineIntersect(new Point2D(p9.X, p9.Y), new Point2D(p10.X, p10.Y), new Point2D(p13.X, p13.Y), new Point2D(p14.X, p14.Y), true, true, out intersectionPoint);

            if (intersectionPoint == new Point2D(0, 0))
            {
                ret.Add(p9);
                ret.Add(p10);

                ret.Add(p11);
                ret.Add(p12);

                ret.Add(p13);
                ret.Add(p14);

                ret.Add(p15);
                ret.Add(p16);
            }
            else
            {
                ret.Add(p9);
                ret.Add(p12);

                ret.Add(p14);
                ret.Add(p15);

                ret.Add(p10);
                ret.Add(p11);

                ret.Add(p13);
                ret.Add(p16);
            }

            return ret;
        }
        public List<OwnVector3> createTransistionSegment(OwnVector3 centerPoint, OwnVector3 prevPoint, OwnVector3 nextPoint)
        {
            List<OwnVector3> ret = new List<OwnVector3>();
            return ret;
            //return createNormalSegment(centerPoint, prevPoint, nextPoint);
        }

    }
}
