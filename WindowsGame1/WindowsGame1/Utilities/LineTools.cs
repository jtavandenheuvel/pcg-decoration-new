using System;
using Microsoft.Xna.Framework;
using WindowsGame1.Utilities;
using System.Collections.Generic;

namespace FarseerPhysics.Common
{
    /// <summary>
    /// Collection of helper methods for misc collisions.
    /// Does float tolerance and line collisions with lines and AABBs.
    /// </summary>
    public static class LineTools
    {
        public static float DistanceBetweenPointAndLineSegment(ref OwnVector2 point, ref OwnVector2 start, ref OwnVector2 end)
        {
            if (start == end)
                return OwnVector2.Distance(point, start);

            OwnVector2 v = OwnVector2.Subtract(end, start);
            OwnVector2 w = OwnVector2.Subtract(point, start);

            float c1 = OwnVector2.Dot(w, v);
            if (c1 <= 0) return OwnVector2.Distance(point, start);

            float c2 = OwnVector2.Dot(v, v);
            if (c2 <= c1) return OwnVector2.Distance(point, end);

            float b = c1 / c2;
            OwnVector2 pointOnLine = OwnVector2.Add(start, OwnVector2.Multiply(v, b));
            return OwnVector2.Distance(point, pointOnLine);
        }

        /// <summary>
        /// Giving linesegment find unit (lenght 1) direction vector of perpendicular line through centerpoint
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="nextPoint"></param>
        /// <returns></returns>
        public static OwnVector3 getPerpendicalurDirectionVector(ref OwnVector3 centerPoint, ref OwnVector3 nextPoint)
        {
            float slope = ((centerPoint.Y - nextPoint.Y) / (centerPoint.X - nextPoint.X));
            float perpSlope = (-1 / slope);
            OwnVector3 pointOnPerpLine = new OwnVector3(centerPoint.X + 10, centerPoint.Y + (perpSlope * 10), 0);
            OwnVector3 Vab = pointOnPerpLine - centerPoint;
            Vab = Vab / Vab.Length();
            if(float.IsNaN(Vab.Y)) 
                Vab.Y = 1;
            return Vab;
        }

        /// <summary>
        /// Return inbetween points (without vec1 and vec2) inside linesegment vec1->vec2 with max segment size
        /// </summary>
        /// <param name="vec1">Begin point line</param>
        /// <param name="vec2">End point line</param>
        /// <param name="segmentSize">The max segment size</param>
        /// <returns></returns>
        public static List<OwnVector3> getSubdividePoints(ref OwnVector3 vec1, ref OwnVector3 vec2, float segmentSize)
        {
            List<OwnVector3> temp = new List<OwnVector3>();
            int totalSegments = (int)Math.Ceiling(OwnVector3.Distance(vec1, vec2) / segmentSize);
            for (int j = 1; j < totalSegments; j++)
            {
                float scaleFactor = (float)j / (float)totalSegments;
                OwnVector3 interpolated = OwnVector3.Lerp(vec1, vec2, scaleFactor); //LineTools.Interpolate(ref vec1, ref vec2, scaleFactor);
                temp.Add(interpolated);
            }
            return temp;
        }

        public static List<Point2D> getSubdividePoints(ref Point2D vec1, ref Point2D vec2, float segmentSize)
        {
            List<Point2D> temp = new List<Point2D>();
            int totalSegments = (int)Math.Ceiling(OwnVector2.Distance(vec1, vec2) / segmentSize);
            for (int j = 1; j < totalSegments; j++)
            {
                float scaleFactor = (float)j / (float)totalSegments;
                Point2D interpolated = new Point2D(OwnVector2.Lerp(vec1, vec2, scaleFactor)); //LineTools.Interpolate(ref vec1, ref vec2, scaleFactor);
                temp.Add(interpolated);
            }
            return temp;
        }

        public static List<OwnVector3> getSubdividedLine(ref OwnVector3 vec1, ref OwnVector3 vec2, float segmentSize)
        {
            List<OwnVector3> temp = new List<OwnVector3>();
            temp.Add(vec1);
            temp.AddRange(getSubdividePoints(ref vec1, ref vec2, segmentSize));
            temp.Add(vec2);
            return temp;
        }

        public static List<Point2D> getSubdividedLine(ref Point2D vec1, ref Point2D vec2, float segmentSize)
        {
            List<Point2D> temp = new List<Point2D>();
            temp.Add(vec1);
            temp.AddRange(getSubdividePoints(ref vec1, ref vec2, segmentSize));
            temp.Add(vec2);
            return temp;
        }


        // From Eric Jordan's convex decomposition library
        /// <summary>
        ///Check if the lines a0->a1 and b0->b1 cross.
        ///If they do, intersectionPoint will be filled
        ///with the point of crossing.
        ///
        ///Grazing lines should not return true.
        /// 
        /// </summary>
        public static bool LineIntersect2(ref OwnVector2 a0, ref OwnVector2 a1, ref OwnVector2 b0, ref  OwnVector2 b1, out OwnVector2 intersectionPoint)
        {
            intersectionPoint = OwnVector2.Zero;

            if (a0 == b0 || a0 == b1 || a1 == b0 || a1 == b1)
                return false;

            float x1 = a0.X;
            float y1 = a0.Y;
            float x2 = a1.X;
            float y2 = a1.Y;
            float x3 = b0.X;
            float y3 = b0.Y;
            float x4 = b1.X;
            float y4 = b1.Y;

            //AABB early exit
            if (Math.Max(x1, x2) < Math.Min(x3, x4) || Math.Max(x3, x4) < Math.Min(x1, x2))
                return false;

            if (Math.Max(y1, y2) < Math.Min(y3, y4) || Math.Max(y3, y4) < Math.Min(y1, y2))
                return false;

            float ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3));
            float ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3));
            float denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
            if (Math.Abs(denom) < Settings.Epsilon)
            {
                //Lines are too close to parallel to call
                return false;
            }
            ua /= denom;
            ub /= denom;

            if ((0 < ua) && (ua < 1) && (0 < ub) && (ub < 1))
            {
                intersectionPoint.X = (x1 + ua * (x2 - x1));
                intersectionPoint.Y = (y1 + ua * (y2 - y1));
                return true;
            }

            return false;
        }

        //From Mark Bayazit's convex decomposition algorithm
        public static OwnVector2 LineIntersect(Point2D p1, Point2D p2, Point2D q1, Point2D q2)
        {
            OwnVector2 i = OwnVector2.Zero;
            float a1 = p2.Y - p1.Y;
            float b1 = p1.X - p2.X;
            float c1 = a1 * p1.X + b1 * p1.Y;
            float a2 = q2.Y - q1.Y;
            float b2 = q1.X - q2.X;
            float c2 = a2 * q1.X + b2 * q1.Y;
            float det = a1 * b2 - a2 * b1;

            if (!MathUtils.FloatEquals(det, 0))
            {
                // lines are not parallel
                i.X = (b2 * c1 - b1 * c2) / det;
                i.Y = (a1 * c2 - a2 * c1) / det;
            }
            return i;
        }

        /// <summary>
        /// This method detects if two line segments (or lines) intersect,
        /// and, if so, the point of intersection. Use the <paramref name="firstIsSegment"/> and
        /// <paramref name="secondIsSegment"/> parameters to set whether the intersection point
        /// must be on the first and second line segments. Setting these
        /// both to true means you are doing a line-segment to line-segment
        /// intersection. Setting one of them to true means you are doing a
        /// line to line-segment intersection test, and so on.
        /// Note: If two line segments are coincident, then 
        /// no intersection is detected (there are actually
        /// infinite intersection points).
        /// Author: Jeremy Bell
        /// </summary>
        /// <param name="point1">The first point of the first line segment.</param>
        /// <param name="point2">The second point of the first line segment.</param>
        /// <param name="point3">The first point of the second line segment.</param>
        /// <param name="point4">The second point of the second line segment.</param>
        /// <param name="point">This is set to the intersection
        /// point if an intersection is detected.</param>
        /// <param name="firstIsSegment">Set this to true to require that the 
        /// intersection point be on the first line segment.</param>
        /// <param name="secondIsSegment">Set this to true to require that the
        /// intersection point be on the second line segment.</param>
        /// <returns>True if an intersection is detected, false otherwise.</returns>
        public static bool LineIntersect(ref Point2D point1, ref Point2D point2, ref Point2D point3, ref Point2D point4, bool firstIsSegment, bool secondIsSegment, out Point2D point)
        {
            point = new Point2D(0, 0);

            // these are reused later.
            // each lettered sub-calculation is used twice, except
            // for b and d, which are used 3 times
            float a = point4.Y - point3.Y;
            float b = point2.X - point1.X;
            float c = point4.X - point3.X;
            float d = point2.Y - point1.Y;

            // denominator to solution of linear system
            float denom = (a * b) - (c * d);

            // if denominator is 0, then lines are parallel
            if (!(denom >= -Settings.Epsilon && denom <= Settings.Epsilon))
            {
                float e = point1.Y - point3.Y;
                float f = point1.X - point3.X;
                float oneOverDenom = 1.0f / denom;

                // numerator of first equation
                float ua = (c * e) - (a * f);
                ua *= oneOverDenom;

                // check if intersection point of the two lines is on line segment 1
                if (!firstIsSegment || ua >= 0.0f && ua <= 1.0f)
                {
                    // numerator of second equation
                    float ub = (b * e) - (d * f);
                    ub *= oneOverDenom;

                    // check if intersection point of the two lines is on line segment 2
                    // means the line segments intersect, since we know it is on
                    // segment 1 as well.
                    if (!secondIsSegment || ub >= 0.0f && ub <= 1.0f)
                    {
                        // check if they are coincident (no collision in this case)
                        if (ua != 0f || ub != 0f)
                        {
                            //There is an intersection
                            point.X = point1.X + ua * b;
                            point.Y = point1.Y + ua * d;

                            //TEST: ROUNDING SHOULD BE GONE? 
                            //point.X = (float)Math.Round((double)(point.X),1);
                            //point.Y = (float)Math.Round((double)(point.Y),1);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This method detects if two line segments (or lines) intersect,
        /// and, if so, the point of intersection. Use the <paramref name="firstIsSegment"/> and
        /// <paramref name="secondIsSegment"/> parameters to set whether the intersection point
        /// must be on the first and second line segments. Setting these
        /// both to true means you are doing a line-segment to line-segment
        /// intersection. Setting one of them to true means you are doing a
        /// line to line-segment intersection test, and so on.
        /// Note: If two line segments are coincident, then 
        /// no intersection is detected (there are actually
        /// infinite intersection points).
        /// Author: Jeremy Bell
        /// </summary>
        /// <param name="point1">The first point of the first line segment.</param>
        /// <param name="point2">The second point of the first line segment.</param>
        /// <param name="point3">The first point of the second line segment.</param>
        /// <param name="point4">The second point of the second line segment.</param>
        /// <param name="intersectionPoint">This is set to the intersection
        /// point if an intersection is detected.</param>
        /// <param name="firstIsSegment">Set this to true to require that the 
        /// intersection point be on the first line segment.</param>
        /// <param name="secondIsSegment">Set this to true to require that the
        /// intersection point be on the second line segment.</param>
        /// <returns>True if an intersection is detected, false otherwise.</returns>
        public static bool LineIntersect(Point2D point1, Point2D point2, Point2D point3, Point2D point4, bool firstIsSegment, bool secondIsSegment, out Point2D intersectionPoint)
        {
            return LineIntersect(ref point1, ref point2, ref point3, ref point4, firstIsSegment, secondIsSegment, out intersectionPoint);
        }

        /// <summary>
        /// This method detects if two line segments intersect,
        /// and, if so, the point of intersection. 
        /// Note: If two line segments are coincident, then 
        /// no intersection is detected (there are actually
        /// infinite intersection points).
        /// </summary>
        /// <param name="point1">The first point of the first line segment.</param>
        /// <param name="point2">The second point of the first line segment.</param>
        /// <param name="point3">The first point of the second line segment.</param>
        /// <param name="point4">The second point of the second line segment.</param>
        /// <param name="intersectionPoint">This is set to the intersection
        /// point if an intersection is detected.</param>
        /// <returns>True if an intersection is detected, false otherwise.</returns>
        public static bool LineIntersect(ref Point2D point1, ref Point2D point2, ref Point2D point3, ref Point2D point4, out Point2D intersectionPoint)
        {
            return LineIntersect(ref point1, ref point2, ref point3, ref point4, true, true, out intersectionPoint);
        }

        /// <summary>
        /// This method detects if two line segments intersect,
        /// and, if so, the point of intersection. 
        /// Note: If two line segments are coincident, then 
        /// no intersection is detected (there are actually
        /// infinite intersection points).
        /// </summary>
        /// <param name="point1">The first point of the first line segment.</param>
        /// <param name="point2">The second point of the first line segment.</param>
        /// <param name="point3">The first point of the second line segment.</param>
        /// <param name="point4">The second point of the second line segment.</param>
        /// <param name="intersectionPoint">This is set to the intersection
        /// point if an intersection is detected.</param>
        /// <returns>True if an intersection is detected, false otherwise.</returns>
        public static bool LineIntersect(Point2D point1, Point2D point2, Point2D point3, Point2D point4, out Point2D intersectionPoint)
        {
            return LineIntersect(ref point1, ref point2, ref point3, ref point4, true, true, out intersectionPoint);
        }

        /// <summary>
        /// Get all intersections between a line segment and a list of vertices
        /// representing a polygon. The vertices reuse adjacent points, so for example
        /// edges one and two are between the first and second vertices and between the
        /// second and third vertices. The last edge is between vertex vertices.Count - 1
        /// and verts0. (ie, vertices from a Geometry or AABB)
        /// </summary>
        /// <param name="point1">The first point of the line segment to test</param>
        /// <param name="point2">The second point of the line segment to test.</param>
        /// <param name="vertices">The vertices, as described above</param>
        public static Vertices LineSegmentVerticesIntersect(ref Point2D point1, ref Point2D point2, Vertices vertices)
        {
            Vertices intersectionPoints = new Vertices();

            for (int i = 0; i < vertices.Count; i++)
            {
                Point2D point;
                if (LineIntersect(vertices[i], vertices[vertices.NextIndex(i)], point1, point2, true, true, out point))
                {
                    intersectionPoints.Add(point);
                }
            }

            return intersectionPoints;
        }

        public static Vertices LineSegmentPolygonIntersect(ref Point2D point1, ref Point2D point2, Polygon polygon)
        {
            Vertices intersectionPoints = new Vertices();
            int count = polygon.Vertices.Count - (polygon.isClosed() ? 0 : 1);

            for (int i = 0; i < count; i++)
            {
                Point2D point;
                if (LineIntersect(polygon.Vertices[i], polygon.Vertices[polygon.Vertices.NextIndex(i)], point1, point2, true, true, out point))
                {
                    intersectionPoints.Add(point);
                }
            }
            return intersectionPoints;
        }
    }
}