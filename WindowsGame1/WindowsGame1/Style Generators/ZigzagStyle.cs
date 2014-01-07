using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Utilities;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Style_Generators
{
    public class ZigzagStyle : StyleGenerator
    {
        public int maximumSegmentSize { get; set; }
        private float distance;
        private bool firstEnd;

        public ZigzagStyle() 
        {
            this.maximumSegmentSize = 30;
            this.distance = 10;
            this.firstEnd = true;
        }

        public ZigzagStyle(float drawDistance)
        {
            this.maximumSegmentSize = 30;
            this.distance = drawDistance;
            this.firstEnd = true;
        }

        public List<OwnVector3> createNormalSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            OwnVector3 direction = centerPoint - previousPoint;
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 startPoint = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f);
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);
            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector3 p1, p2, p3;

            p1 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), distance);
            p2 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(90, norDirection), distance);
            p3 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection), distance);

            ret.Add(p1);
            ret.Add(p2);
            ret.Add(p2);
            ret.Add(p3);
            
            return ret;
        }

        public List<OwnVector3> createEndSegment(OwnVector3 centerPoint, OwnVector3 nextPoint)
        {
            OwnVector3 direction = nextPoint - centerPoint;
            OwnVector3 norDirection = direction / direction.Length();   
            OwnVector3 startPoint = centerPoint;
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);

            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector3 p1, p2, p3, p4;

            if (firstEnd)
            {
                p1 = DrawTools.MoveForward(endPoint, DrawTools.TurnLeft(90, norDirection), distance);
                firstEnd = false;
            }
            else
            {
                p1 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection), distance);
            }

            p2 = OwnVector3.Lerp(startPoint, endPoint, 0.5f);
            p3 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance);
            p4 = DrawTools.MoveForward(startPoint, DrawTools.TurnRight(90, norDirection), distance);

            ret.Add(p1);
            ret.Add(p2);
            ret.Add(p2);
            ret.Add(p3);
            ret.Add(p3);
            ret.Add(p4);
            ret.Add(p4);
            ret.Add(p2);

            return ret;
        }

        public List<OwnVector3> createCornerSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            OwnVector3 direction = centerPoint - previousPoint;
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 direction2 = centerPoint - nextPoint;
            OwnVector3 norDirection2 = direction2 / direction2.Length();
            OwnVector3 startPoint = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f);
            OwnVector3 endPoint = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f);

            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector2 vector1 = new OwnVector2(previousPoint.X, previousPoint.Y) - new OwnVector2(centerPoint.X, centerPoint.Y);
            OwnVector2 vector2 = new OwnVector2(nextPoint.X, nextPoint.Y) - new OwnVector2(centerPoint.X, centerPoint.Y);
            float angle = MathHelper.ToDegrees((float)MathUtils.VectorAngle(ref vector1, ref vector2));

           
            if (105 < angle || angle < -150)
            {
                OwnVector3 p1 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance);
                OwnVector3 p2 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection2), distance);

                OwnVector3 temp = OwnVector3.Lerp(p1, p2, 0.5f);
                OwnVector3 tempDirection = temp - p1;
                OwnVector3 tempNorDirection = tempDirection / tempDirection.Length();

                OwnVector3 p3 = DrawTools.MoveForward(temp, DrawTools.TurnRight(90, tempNorDirection), distance * 2);

                ret.Add(p1);
                ret.Add(p3);
                ret.Add(p3);
                ret.Add(p2);
            }
            else if (angle < 0)
            {
                OwnVector3 p1, p2, p3, p4;
                p1 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance);
                p2 = DrawTools.MoveForward(centerPoint, DrawTools.TurnRight(90, norDirection), distance);

                p3 = DrawTools.MoveForward(centerPoint, DrawTools.TurnLeft(90, norDirection2), distance);
                p4 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection2), distance);

                ret.Add(p1);
                ret.Add(p2);
                ret.Add(p2);
                ret.Add(p3);
                ret.Add(p3);
                ret.Add(p4);
            }
            else
            {
                OwnVector3 p1, p2;
                p1 = DrawTools.MoveForward(startPoint, DrawTools.TurnLeft(90, norDirection), distance);
                p2 = DrawTools.MoveForward(endPoint, DrawTools.TurnRight(90, norDirection2), distance);

                ret.Add(p1);
                ret.Add(p2);
            }
            return ret;

        }

        public List<OwnVector3> createTJunctionSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints)
        {
            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector3 direction = centerPoint - connectingPoints[0];
            OwnVector3 norDirection = direction / direction.Length();
            OwnVector3 temp = OwnVector3.Lerp(centerPoint, connectingPoints[0], 0.5f);
            OwnVector3 p1 = DrawTools.MoveForward(temp, DrawTools.TurnLeft(90, norDirection), distance);

            direction = centerPoint - connectingPoints[1];
            norDirection = direction / direction.Length();
            temp = OwnVector3.Lerp(centerPoint, connectingPoints[1], 0.5f);
            OwnVector3 p2 = DrawTools.MoveForward(temp, DrawTools.TurnRight(90, norDirection), distance);

            direction = centerPoint - connectingPoints[2];
            norDirection = direction / direction.Length();
            temp = OwnVector3.Lerp(centerPoint, connectingPoints[2], 0.5f);
            OwnVector3 p3 = DrawTools.MoveForward(temp, DrawTools.TurnLeft(90, norDirection), distance);

            OwnVector3 middle = (p1 + p2 + p3) / 3;

            ret.Add(p1);
            ret.Add(middle);
            ret.Add(p2);
            ret.Add(middle);
            ret.Add(p3);
            ret.Add(middle);

            return ret;
        }

        public List<OwnVector3> createCrossingSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints)
        {

            List<OwnVector3> ret = new List<OwnVector3>();

            List<OwnVector3> temp = new List<OwnVector3>();

            for(int i = 0; i < connectingPoints.Count; i++)
            {
                OwnVector3 direction = centerPoint - connectingPoints[i];
                OwnVector3 norDirection = direction / direction.Length();
                OwnVector3 p1 = OwnVector3.Lerp(centerPoint,  connectingPoints[i], 0.5f);
                OwnVector3 p2 = DrawTools.MoveForward(p1, DrawTools.TurnLeft(90, norDirection), distance);
                OwnVector3 p3 = DrawTools.MoveForward(p1, DrawTools.TurnRight(90, norDirection), distance);

                if (i % 2 == 0)
                {
                    temp.Add(p2);
                }
                else
                {
                    temp.Add(p3);
                }
            }
            ret.Add(temp[0]);
            ret.Add(temp[3]);
            ret.Add(temp[1]);
            ret.Add(temp[2]);
            ret.Add(OwnVector3.Lerp(temp[0], temp[3], 0.5f));
            ret.Add(OwnVector3.Lerp(temp[1], temp[2], 0.5f));
            return ret;
        }
        public List<OwnVector3> createTransistionSegment(OwnVector3 centerPoint, OwnVector3 cornerPoint, OwnVector3 nextPoint)
        {
            return createNormalSegment(centerPoint, cornerPoint, nextPoint);
        }

    }
}
