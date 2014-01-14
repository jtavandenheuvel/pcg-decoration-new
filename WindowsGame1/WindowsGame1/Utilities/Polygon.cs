using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.Style_Generators;
using FarseerPhysics;
using FarseerPhysics.Common.PolygonManipulation;
using RoundLineCode;

namespace WindowsGame1.Utilities
{
    public class Polygon
    {
        public Vertices Vertices {get; set;}
        public Vertices ControlVertices { get; set; }
        private Vertices intersectionPoints, subdivisionPoints;
        public bool Closed;
        private StyleGenerator style;


        public Polygon(Vertices v, bool closed)
        {
            Vertices = v;
            Closed = closed;
            intersectionPoints = new Vertices();
            subdivisionPoints = new Vertices();
            ControlVertices = new Vertices();
        }

        internal void update(Game1 game)
        {

            ControlVertices.ForceCounterClockWise(game);
            Vertices = new Vertices(ControlVertices);
            updateHoles(game);
            updateIntersectionPoints();
            updatePointLinks();
            updateSegmentTypes();
            updateSubdivisionPoints(game._gui.getSubdivideSize());
        }

        private void updateHoles(Game1 game)
        {
            ControlVertices.Holes = new List<Vertices>();
            for(int i = 0; i < game.controlPointsHoles.Count; i++)
            {
                ControlVertices.Holes.Add(new Vertices());
                foreach (Rectangle point in game.controlPointsHoles[i])
                {
                    ControlVertices.Holes[i].Add(new Point2D(point.X + point.Width / 2, point.Y + point.Height / 2));
                }
            }
        }


        public void Draw(Game1 game)
        {
            resetDrawn();
            //drawRoundLines(game);
            drawPolygonLines(game);
            drawHoles(game);
            //drawVerticesNumbers(game);
        }

        private void drawRoundLines(Game1 game)
        {
            RoundLine line = new RoundLine(50, 100, 150, 200);
            RoundLineManager manager = new RoundLineManager();
            manager.Init(game.GraphicsDevice, game.Content);
            float projScaleX = game.GraphicsDevice.Viewport.Height / game.GraphicsDevice.Viewport.Width;
            float projScaleY = 1.0f;
            Matrix projMatrix = Matrix.CreateScale(projScaleX, projScaleY, 0.0f);
            projMatrix.M43 = 0.5f;
            manager.Draw(line, 1, Color.Black, projMatrix, 1, manager.TechniqueNames[6]);

            manager.Draw(line, 1, Color.Black, projMatrix, 1, manager.TechniqueNames[6]);
        }

        private void resetDrawn()
        {
            foreach (Point2D point in Vertices)
            {
                point.drawn = false;
            }
        }


        private void updateSubdivisionPoints(float segmentSize)
        {
            Vertices result = new Vertices();
            subdivisionPoints.Clear();

            //Calculate new intersections
            int lineCount = this.Vertices.Count - (isClosed() ? 0 : 1);
            if (lineCount > 0)
            {
                for (int i = 0; i < lineCount; i++)
                {
                    Vertices vertices = this.Vertices;
                    Point2D vec1 = vertices[i]; 
                    Point2D vec2 = vertices.NextVertex(i); 

                    //Get intersections and sort them distance wise from vec1 (longest first) 
                    List<Point2D> subdivisions = LineTools.getSubdividedLine(ref vec1, ref vec2, segmentSize);

                    //add them to the existing vertices
                    for (int j = 0; j < subdivisions.Count - 1; j++)
                    {
                        subdivisionPoints.Add(subdivisions[j]);
                    }
                    result.AddRange(subdivisions);
                }
            }
            this.Vertices = result;// SimplifyTools.MergeIdenticalNeighborPoints(result);
        }

        private void updateSegmentTypes()
        {
            //Reset every segment to normal 
            for(int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].segmentType = Settings.SegmentType.Corner;
            }

            //If not closed classify first and last point as end segment
            if (!Closed && Vertices.Count-1 > 0)
            {
                Vertices[0].segmentType = Settings.SegmentType.End;
                Vertices[Vertices.Count - 1].segmentType = Settings.SegmentType.End;
            }

            //Handle all the linked points (may reset and endpoint to a TJunction)
            foreach (Point2D point in Vertices)
            {
                if (point.hasLinkedPoints())
                {
                    if (point.segmentType == Settings.SegmentType.End || point.linkedPoints[0].segmentType == Settings.SegmentType.End)
                    {
                        point.segmentType = Settings.SegmentType.TJunction;
                    }
                    else
                    {
                        point.segmentType = Settings.SegmentType.Crossing;
                    }
                }
            }
        }

        private void updatePointLinks()
        {
            //clear links
            foreach (Point2D point in Vertices)
            {
                point.linkedPoints.Clear();
            }

            //add new links
            for (int i = 0; i < Vertices.Count; i++)
            {
                for (int j = i+1; j < Vertices.Count; j++)
                {
                    float length = (Vertices[j]-Vertices[i]).Length();
                    //TODO hardcoded 0.5 parameter for rounding
                    if (length < 0.5)
                    {

                        Vertices[i].linkedPoints.Add(Vertices[j]);
                        Vertices[j].linkedPoints.Add(Vertices[i]);
                    }
                }
            }
        }

        private bool areThereLinkedPoints()
        {
            foreach (Point2D point in Vertices)
            {
                if (point.hasLinkedPoints())
                {
                    return true;
                }
            }
            return false;
        }

        public bool isClosed()
        {
            return this.Closed;
        }

        public Vertices getIntersectionPoints()
        {
            return intersectionPoints;
        }

        public Vertices getSubdivisionPoints()
        {
            return subdivisionPoints;
        }


        public void updateIntersectionPoints()
        {
            Vertices result = new Vertices();
            Polygon tempPoly = this;

            //Calculate new intersections
            int lineCount = this.Vertices.Count - (isClosed() ? 0 : 1);
            if (lineCount > 0)
            {

                for (int i = 0; i < lineCount; i++)
                {
                    Vertices vertices = this.Vertices;
                    Point2D vec1 = new Point2D(vertices[i].X, vertices[i].Y);
                    Point2D vec2 = new Point2D(vertices[vertices.NextIndex(i)].X, vertices[vertices.NextIndex(i)].Y);

                    //Get intersections and sort them distance wise from vec1 (longest first) 
                    Vertices intersections = LineTools.LineSegmentPolygonIntersect(ref vec1, ref vec2, tempPoly);
                    intersections.Sort(
                        delegate(Point2D p1, Point2D p2)
                        {
                            return OwnVector2.Distance(vec1, p2).CompareTo(OwnVector2.Distance(vec1, p1));
                        }
                    );

                    //add them to the existing vertices
                    for (int j = 0; j < intersections.Count; j++)
                    {
                        Point2D intersection = intersections[j];
                        if (!(intersection.Equals(vec1) || intersection.Equals(vec2)))
                        {
                            this.Vertices.Insert(i + 1, intersection);
                            result.Add(intersection);
                        }
                    }
                    //Simplify the polygon (reducing points that are closer then 2 pixels to one point)
                    this.Vertices = SimplifyTools.ReduceByDistance(this.Vertices, 2);
                    this.intersectionPoints = SimplifyTools.ReduceByDistance(result, 2);
                    this.Vertices = SimplifyTools.MergeIdenticalPoints(this.Vertices);

                    lineCount = this.Vertices.Count - (isClosed() ? 0 : 1);
                }
            }
            this.Vertices = SimplifyTools.MergeIdenticalNeighborPoints(this.Vertices);
        }

        private void drawVerticesNumbers(Game1 game)
        {
            for (int i = 0; i < ControlVertices.Count; i++)
            {
                Vector2 pos = ControlVertices[i].getVector();
                pos.X = pos.X + 5;
                pos.Y = pos.Y - 1;
                game.spriteBatch.DrawString(game._gui.greySpriteFont, i.ToString(),pos,  Color.Black);
            }
        }

        private void drawPolygonLines(Game1 game)
        {
            int totalLinesToDraw = Vertices.Count - (this.isClosed() ? 0 : 1);
            if (totalLinesToDraw > 0)
            {
                
                game.basicEffect.CurrentTechnique.Passes[0].Apply();

                Settings.DrawStyle drawStyle = game._gui.getDrawStyle();
                //TEST: Changing dropdown changes color
                Color color;
                if (drawStyle.Equals(Settings.DrawStyle.Color))
                    color = new Color(
                                            (byte)game.random.Next(0, 255),
                                            (byte)game.random.Next(0, 255),
                                            (byte)game.random.Next(0, 255));
                else
                    color = Color.Black;
                //Get vertices from polygon 
                var vertices = this.getVertexPositionColor(Vertices, color, drawStyle, game, this.isClosed());
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Count() / 2);
            }
        }



        private void drawHoles(Game1 game)
        {
            for (int i = 0; i < ControlVertices.Holes.Count; i++)
            {
                int totalLinesToDraw = ControlVertices.Holes[i].Count;
                if (totalLinesToDraw > 0)
                {

                    game.basicEffect.CurrentTechnique.Passes[0].Apply();

                    Color color = Color.Green;

                    //Get vertices from polygon 
                    var vertices = this.getVertexPositionColor(ControlVertices.Holes[i], color, Settings.DrawStyle.None, game, true);
                    game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Count() / 2);
                }
            }
        }

        public VertexPositionColor[] getVertexPositionColor(Vertices verticeIn, Color color, Settings.DrawStyle drawStyle, Game1 game, bool closed)
        {
            int count = verticeIn.Count - (closed ? 0 : 1);
            if (drawStyle != Settings.DrawStyle.Color)
            {
                count++;
            }
            //Create all the positions that need drawing
            List<OwnVector3> tempList = new List<OwnVector3>();

            if (drawStyle == Settings.DrawStyle.Greek)
            {
                style = new GreekStyle(game._gui.getSegmentSize());
               
            }
            else if (drawStyle == Settings.DrawStyle.ZigZag)
            {
                style = new ZigzagStyle(game._gui.getSegmentSize());
            }
            else if (drawStyle == Settings.DrawStyle.Test)
            {
                style = new SimpleStyle();
            }
            else if (drawStyle == Settings.DrawStyle.None || drawStyle == Settings.DrawStyle.Color)
            {
                style = null;
            }

            for (int i = 0; i < count-1; i++)
            {
                Point2D current = verticeIn[i];
                Point2D previous = verticeIn.PreviousVertex(i);
                Point2D next = verticeIn.NextVertex(i);

                if (style != null && !current.drawn)
                {
                    applyStyle(count, tempList, i, current, previous, next);
                }
                else if(style == null)
                {
                    tempList.Add(new OwnVector3(current,0));
                    tempList.Add(new OwnVector3(next, 0));
                }
                current.drawn = true;
                foreach (Point2D point in current.linkedPoints)
                {
                    point.drawn = true;
                }
            }
            //Convert to drawing vertex
            var vertices = convertToVertexPositioncolor(ref color, tempList);
            return vertices;
        }


        //TODO: Fix this ugly code
        private void applyStyle(int count, List<OwnVector3> tempList, int i, Point2D current, Point2D prev, Point2D next)
        {
            OwnVector3 vCurrent = new OwnVector3(current, 0);
            OwnVector3 vPrev = new OwnVector3(prev,0);
            OwnVector3 vNext = new OwnVector3(next,0);

            Settings.SegmentType prevType = prev.segmentType;
            Settings.SegmentType curType = current.segmentType;
            Settings.SegmentType nextType = next.segmentType;

            if (curType == Settings.SegmentType.End && i==0)
            {
                tempList.AddRange(style.createEndSegment(vCurrent, vNext));
            }
            else if (curType == Settings.SegmentType.End)
            {
                tempList.AddRange(style.createEndSegment(vCurrent, vPrev));
            }
            else if (curType == Settings.SegmentType.TJunction)
            {
                List<OwnVector3> neighbors = new List<OwnVector3>();
                if (i != 0 && i != count - 1)
                {
                    neighbors.Add(vPrev);
                    neighbors.Add(vNext);
                }
                else if (i == 0)
                {
                    neighbors.Add(vPrev);
                }
                else
                {
                    neighbors.Add(vNext);
                }
                //Search for other neighbors and add if they exist
                for (int j = 0; j < current.linkedPoints.Count; j++ )
                {
                    int index = Vertices.IndexOf(current.linkedPoints[j]);
                    if (i == 0 || i == count - 1)
                    {
                        neighbors.Add(new OwnVector3(Vertices.PreviousVertex(index), 0));
                        neighbors.Add(new OwnVector3(Vertices.NextVertex(index), 0));
                    }
                    else if(current.linkedPoints[j].ID == 0)
                    {
                        neighbors.Add(new OwnVector3(Vertices.NextVertex(index), 0));
                    }
                    else
                    {
                        neighbors.Add(new OwnVector3(Vertices.PreviousVertex(index), 0));
                    }
                }

                tempList.AddRange(style.createTJunctionSegment(vCurrent, neighbors));
            }
            else if (curType == Settings.SegmentType.Crossing)
            {
                //Add previous and next node 
                List<OwnVector3> neighbors = new List<OwnVector3>();
                neighbors.Add(vPrev);
                neighbors.Add(vNext);

                //Search for other neighbors and add if they exist
                foreach (Point2D point in current.linkedPoints)
                {
                    int index = Vertices.IndexOf(point);
                    neighbors.Add(new OwnVector3(Vertices.PreviousVertex(index), 0));
                    neighbors.Add(new OwnVector3(Vertices.NextVertex(index), 0));
                }
                tempList.AddRange(style.createCrossingSegment(vCurrent, neighbors));
            }
            else if (curType == Settings.SegmentType.Corner)
            {
                tempList.AddRange(style.createCornerSegment(vCurrent, vPrev, vNext));
            }
            else if (
                prevType == Settings.SegmentType.Corner || 
                nextType == Settings.SegmentType.Corner ||
                prevType == Settings.SegmentType.Crossing ||
                nextType == Settings.SegmentType.Crossing)
            {
                tempList.AddRange(style.createTransistionSegment(vCurrent, vNext, vPrev));
            }
            else
            {
                tempList.AddRange(style.createNormalSegment(vCurrent, vNext, vPrev));
            }
        }

        private static VertexPositionColor[] convertToVertexPositioncolor(ref Color color, List<OwnVector3> tempList)
        {
            int totalVertices = tempList.Count;
            var vertices = new VertexPositionColor[totalVertices];
            for (int j = 0; j < totalVertices; j = j + 2)
            {
                vertices[j].Position = tempList[j].getVector();
                vertices[j + 1].Position = tempList[j + 1].getVector();
                vertices[j].Color = color;
                vertices[j + 1].Color = color;
            }
            return vertices;
        }

        internal void Clear()
        {
            Vertices.Clear();
            ControlVertices.Clear();
        }
    }
}