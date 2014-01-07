using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using WindowsGame1.Utilities;

namespace WindowsGame1.Style_Generators
{
    public class SimpleStyle : StyleGenerator
    {
        public int maximumSegmentSize { get; set; }
        private int distance;

        public SimpleStyle() 
        {
            this.maximumSegmentSize = 25;
            this.distance = 10;
        }

        public List<OwnVector3> createNormalSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector3 Vab = LineTools.getPerpendicalurDirectionVector(ref centerPoint, ref previousPoint);
            OwnVector3 Vac = LineTools.getPerpendicalurDirectionVector(ref centerPoint, ref nextPoint);

            OwnVector3 leftPerp = centerPoint - (distance * Vab);
            OwnVector3 rightPerp = centerPoint + (distance * Vac);
            OwnVector3 prev = OwnVector3.Lerp(centerPoint, previousPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref previousPoint, 0.5f);
            OwnVector3 next = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref nextPoint, 0.5f);

            ret.Add(prev);
            ret.Add(leftPerp);
            ret.Add(leftPerp);
            ret.Add(rightPerp);
            ret.Add(rightPerp);
            ret.Add(next);

            return ret;
        }

        public List<OwnVector3> createEndSegment(OwnVector3 centerPoint, OwnVector3 prevPoint)
        {
            List<OwnVector3> ret = new List<OwnVector3>();
            
            //Calculate Vab and move centerpoint -50 and +50 distance away from point on perpendicalur line 
            OwnVector3 Vab = LineTools.getPerpendicalurDirectionVector(ref centerPoint, ref prevPoint);

            OwnVector3 leftCorner = centerPoint - (distance * Vab);
            OwnVector3 rightCorner = centerPoint + (distance * Vab);
            OwnVector3 middlePoint = OwnVector3.Lerp(centerPoint, prevPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref nextPoint, 0.5f);

            ret.Add(leftCorner);
            ret.Add(rightCorner);
            ret.Add(rightCorner);
            ret.Add(middlePoint);
            ret.Add(middlePoint);
            ret.Add(leftCorner);

            return ret;
        }

        public List<OwnVector3> createCornerSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint)
        {
            List<OwnVector3> ret = new List<OwnVector3>();


            ret.Add(OwnVector3.Lerp(previousPoint, centerPoint, 0.5f));
            ret.Add(centerPoint);
            ret.Add(centerPoint);
            ret.Add(OwnVector3.Lerp(centerPoint, nextPoint, 0.5f));

            return ret;
        }

        public List<OwnVector3> createTJunctionSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints)
        {
            List<OwnVector3> ret = new List<OwnVector3>();

            if (connectingPoints.Count > 1)
            {
                ret.Add(OwnVector3.Lerp(connectingPoints[0], centerPoint, 0.5f));
                ret.Add(centerPoint);
                ret.Add(centerPoint);
                ret.Add(OwnVector3.Lerp(centerPoint, connectingPoints[1], 0.5f));
            }
            else if(connectingPoints.Count == 1)
            {
                ret.Add(OwnVector3.Lerp(connectingPoints[0], centerPoint, 0.5f));
                ret.Add(centerPoint);
            }
            return ret;
        }

        public List<OwnVector3> createCrossingSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints)
        {
            List<OwnVector3> ret = new List<OwnVector3>();

            ret.Add(OwnVector3.Lerp(connectingPoints[0], centerPoint, 0.5f));
            ret.Add(centerPoint);
            ret.Add(centerPoint);
            ret.Add(OwnVector3.Lerp(centerPoint, connectingPoints[1], 0.5f));

            ret.Add(OwnVector3.Lerp(connectingPoints[2], centerPoint, 0.5f));
            ret.Add(centerPoint);
            ret.Add(centerPoint);
            ret.Add(OwnVector3.Lerp(centerPoint, connectingPoints[3], 0.5f));

            return ret;
        }

        public List<OwnVector3> createTransistionSegment(OwnVector3 centerPoint, OwnVector3 cornerPoint, OwnVector3 nextPoint)
        {
            List<OwnVector3> ret = new List<OwnVector3>();

            OwnVector3 Vab = LineTools.getPerpendicalurDirectionVector(ref centerPoint, ref cornerPoint);
            OwnVector3 Vac = LineTools.getPerpendicalurDirectionVector(ref centerPoint, ref nextPoint);

            OwnVector3 leftPerp = centerPoint - (distance/2 * Vab);
            OwnVector3 rightPerp = centerPoint + (distance/2 * Vab);
            OwnVector3 prev = OwnVector3.Lerp(centerPoint, cornerPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref previousPoint, 0.5f);
            OwnVector3 next = OwnVector3.Lerp(centerPoint, nextPoint, 0.5f); //LineTools.Interpolate(ref centerPoint, ref nextPoint, 0.5f);

            ret.Add(prev);
            ret.Add(leftPerp);
            ret.Add(leftPerp);
            ret.Add(rightPerp);
            ret.Add(rightPerp);
            ret.Add(next);

            return ret;
        }
    }
}
