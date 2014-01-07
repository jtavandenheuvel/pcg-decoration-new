using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Utilities;
using FarseerPhysics.Common;
using Priority_Queue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common.PolygonManipulation;


namespace WindowsGame1.Straight_Skeleton
{
    class StraightSkeletonGenerator
    {
        private Polygon polygon;
        private List<CircularLinkedList<Vertex>> SLAV;
        public List<OwnVector2> testDots;
        private HeapPriorityQueue<Event> Q;
        private int id = 0;
        private Game1 game;

        public StraightSkeletonGenerator(Polygon polygon, Game1 game)
        {
            this.polygon = polygon;
            this.game = game;
            setup();
        }

        /// <summary>
        /// Setup all the necessary lists for the algorithm to work 
        /// </summary>
        private void setup()
        {
            //initialize SLAV
            SLAV = new List<CircularLinkedList<Vertex>>();

            //initialize priority queue Q
            Q = new HeapPriorityQueue<Event>(500);

            //initialize first LAV
            CircularLinkedList<Vertex> LAV = new CircularLinkedList<Vertex>();
            for (int i = 0; i < polygon.ControlVertices.Count; i++)
            {
                //Create vertex for each controlpoint and store edges + calc bisector ray 
                Vertex vertex = new Vertex(polygon.ControlVertices[i], id++);
                vertex.prevEdge = new Edge(polygon.ControlVertices.PreviousVertex(i), vertex.getPoint());
                vertex.nextEdge = new Edge(vertex.getPoint(), polygon.ControlVertices.NextVertex(i));
                vertex.update();

                //Add them to initial LAV
                LAV.AddLast(vertex);
            }

            SLAV.Add(LAV);

            //initial event creation
            testDots = new List<OwnVector2>();
            for(int i = 0; i < LAV.Count; i++)
            {
                Vertex prev = LAV[i].Previous.Value;
                Vertex current = LAV[i].Value;
                Vertex next = LAV[i].Next.Value;

                findClosestIntersectionAndStore(LAV, prev, current, next);
            }
        }

        private void findClosestIntersectionAndStore(CircularLinkedList<Vertex> LAV, Vertex prev, Vertex current, Vertex next)
        {
            Vertices testPoly = new Vertices();
            foreach (Vertex v in LAV)
            {
                testPoly.Add(v.getPoint());
            }
            OwnVector2 prevIntersection = LineTools.LineIntersect(
                prev.getRayStart(), prev.getRayStepPoint(),
                current.getRayStart(), current.getRayStepPoint());
            OwnVector2 nextIntersection = LineTools.LineIntersect(
                current.getRayStart(), current.getRayStepPoint(),
                next.getRayStart(), next.getRayStepPoint());
            OwnVector2 nearestOppositeIntersection = OwnVector2.Zero;

            Point2D testPrev = new Point2D(prevIntersection);
            if (testPoly.PointInPolygon(ref testPrev) == -1)
            {
                prevIntersection = Point2D.Zero;
            }
            Point2D testNext = new Point2D(nextIntersection);
            if (testPoly.PointInPolygon(ref testNext) == -1)
            {
                nextIntersection = Point2D.Zero;
            }

            Vertex firstEdgePoint = null;
            Vertex secondEdgePoint = null;
            if(current.isConcave())
            {                                                                       
                nearestOppositeIntersection = findNearestOppositeEdge(LAV, current, out firstEdgePoint, out secondEdgePoint);
            }

            float distPrev = prevIntersection.Equals(Point2D.Zero) ? float.MaxValue : OwnVector2.Distance(current.getPoint(), prevIntersection);
            float distNext = nextIntersection.Equals(Point2D.Zero) ? float.MaxValue : OwnVector2.Distance(current.getPoint(), nextIntersection);
            float distOpposite = float.MaxValue;

            if (distPrev == 0)
            {
                prevIntersection = Point2D.Zero;
            }
            if (distNext == 0)
            {
                nextIntersection = Point2D.Zero;
            }
            if (current.isConcave())
            {
                distOpposite = OwnVector2.Distance(current.getPoint(), nearestOppositeIntersection);
            }


            if (prevIntersection.Equals(OwnVector2.Zero) && nextIntersection.Equals(OwnVector2.Zero) && nearestOppositeIntersection.Equals(OwnVector2.Zero))
            {
                return;
            }
            if ((nextIntersection.Equals(OwnVector2.Zero) && nearestOppositeIntersection.Equals(OwnVector2.Zero)) || distPrev < distNext && distPrev < distOpposite)
            {
                Event e = new Event(new Point2D(prevIntersection), SLAV.IndexOf(LAV), EventType.Edge);
                e.storeFirstPoint(prev);
                e.storeSecondPoint(current);
                Q.Enqueue(e, distPrev);
            }
            else if ((nearestOppositeIntersection.Equals(OwnVector2.Zero) && prevIntersection.Equals(OwnVector2.Zero)) || distNext <= distPrev && distNext <= distOpposite)
            {
                Event e = new Event(new Point2D(nextIntersection), SLAV.IndexOf(LAV), EventType.Edge);
                e.storeFirstPoint(current);
                e.storeSecondPoint(next);
                Q.Enqueue(e, distNext);
            }
            else
            {
                Event e = new Event(new Point2D(nearestOppositeIntersection), SLAV.IndexOf(LAV), EventType.Split);
                e.storeFirstPoint(current);
                e.storeFirstEdgePoint(firstEdgePoint);
                e.storeSecondEdgePoint(secondEdgePoint);
                Q.Enqueue(e, distOpposite);
            }                                         
        }

        private OwnVector2 findNearestOppositeEdge(CircularLinkedList<Vertex> LAV, Vertex current, out Vertex firstEdgePoint, out Vertex secondEdgePoint)
        {
            firstEdgePoint = null;
            secondEdgePoint = null;
            Node<Vertex> temp = LAV.Find(current).Next;
            Point2D intersectionPoint = new Point2D(0,0);
            Point2D currentClosest = new Point2D(0, 0);
            Vertices testPoly = new Vertices();
            foreach(Vertex v in LAV)
            {
                testPoly.Add(v.getPoint());
            }
            while (!temp.Next.Value.Equals(current))
            {
                //check if the edge is not behind the vertex
                if(LineTools.LineIntersect(
                    current.getRayStart(), 
                    new Point2D(current.getRayStart().X + (-current.getRayDirection().X * int.MaxValue), -current.getRayStart().Y + (-current.getRayDirection().Y * int.MaxValue)),
                    temp.Value.getPoint(),
                    temp.Next.Value.getPoint(),
                    true,false, out intersectionPoint))
                {
                    //Calc Bi (intersection ray current + bisector of triangle)
                    Point2D intersect1 = new Point2D(LineTools.LineIntersect(
                        current.prevEdge.getFirstPoint(),
                        current.prevEdge.getSecondPoint(),
                        temp.Value.getPoint(),
                        temp.Next.Value.getPoint()));
                    Point2D intersect2 = new Point2D(LineTools.LineIntersect(
                        current.nextEdge.getFirstPoint(),
                        current.nextEdge.getSecondPoint(),
                        temp.Value.getPoint(),
                        temp.Next.Value.getPoint()));

                    Vertices tempVer = new Vertices();
                    tempVer.Add(current.getPoint());
                    tempVer.Add(intersect1);
                    tempVer.Add(intersect2);

                    Vertex edgeBisector1 = new Vertex(intersect1, -1);
                    edgeBisector1.prevEdge = new Edge(current.getPoint(), intersect1);
                    edgeBisector1.nextEdge = new Edge(intersect1, intersect2);
                    edgeBisector1.update();

                    Point2D Bi = new Point2D(LineTools.LineIntersect(
                        current.getRayStart(),
                        current.getRayStepPoint(),
                        edgeBisector1.getRayStart(),
                        edgeBisector1.getRayStepPoint()));

                    if (tempVer.PointInPolygon(ref Bi) == -1)
                    {

                        edgeBisector1 = new Vertex(intersect2, -1);
                        edgeBisector1.prevEdge = new Edge(current.getPoint(), intersect2);
                        edgeBisector1.nextEdge = new Edge(intersect2, intersect1);
                        edgeBisector1.update();

                        Bi = new Point2D(LineTools.LineIntersect(
                            current.getRayStart(),
                            current.getRayStepPoint(),
                            edgeBisector1.getRayStart(),
                            edgeBisector1.getRayStepPoint()));
                        if (tempVer.PointInPolygon(ref Bi) == -1)
                        {
                            temp = temp.Next;
                            continue;
                        }
                    }

                    //check if Bi inside polygon to begin with 
                    if (testPoly.PointInPolygon(ref Bi) == -1)
                    {
                        temp = temp.Next;
                        continue;
                    }

                    //check if Bi is in area defined by opposing edge and it's bisectors 
                    //first check if both bisectors of edge are convex so we can see if Bi is inside the defined triangle
                    if(temp.Value.isConvex() && temp.Next.Value.isConvex())
                    {
                        OwnVector2 trianglePoint = LineTools.LineIntersect(
                            temp.Value.getRayStart(),
                            temp.Value.getRayStepPoint(),
                            temp.Next.Value.getRayStart(),
                            temp.Next.Value.getRayStepPoint());

                        if (!MathHelper.PointInTriangle(trianglePoint, temp.Value.getPoint(), temp.Next.Value.getPoint(), Bi))
                        {
                            temp = temp.Next;
                            continue;
                        }
                    }
                    else{
                        Vertices test = new Vertices();
                        int sign1 = temp.Value.isConvex() ? 1 : -1;
                        int sign2 = temp.Next.Value.isConvex() ? 1 : -1;

                        test.Add(temp.Value.getPoint());
                        test.Add(temp.Next.Value.getPoint());
                        test.Add(new Point2D(
                            temp.Next.Value.getPoint().X + (sign2 * temp.Next.Value.getRayDirection().X * int.MaxValue),
                            temp.Next.Value.getPoint().Y + (sign2 * temp.Next.Value.getRayDirection().Y * int.MaxValue)));
                        test.Add(new Point2D(
                            temp.Value.getPoint().X + (sign1 * temp.Value.getRayDirection().X * int.MaxValue),
                            temp.Value.getPoint().Y + (sign1 * temp.Value.getRayDirection().Y * int.MaxValue)));

                        if (test.PointInPolygon(ref Bi) == -1)
                        {
                            temp = temp.Next;
                            continue;
                        }
                    }


                    if (currentClosest.Equals(Point2D.Zero) || Point2D.Distance(current.getPoint(), currentClosest) > Point2D.Distance(current.getPoint(), Bi))
                    {
                        currentClosest = Bi;
                        firstEdgePoint = temp.Value;
                        secondEdgePoint = temp.Next.Value;
                    }
                }
                temp = temp.Next;
            }

            testDots.Add(currentClosest);
            return currentClosest;
        }

        /// <summary>
        /// Performs algorithm and returns skeleton
        /// </summary>
        public List<Point2D> Skeleton()
        {
            List<Point2D> ret = new List<Point2D>();

            int i = 0;
            while (Q.Count != 0)
            {
                Event currentEvent = Q.Dequeue();
                CircularLinkedList<Vertex> currentLAV = SLAV[currentEvent.getLAVID()];
                
                Console.WriteLine("------ Iteration " + i + " LAV ID " + currentEvent.getLAVID() + " ------");
                if (currentLAV.Count > 0)
                {
                    foreach (Vertex vertex in currentLAV)
                    { 
                        if (vertex != null)
                        {
                            Console.WriteLine("Vertex with ID= " + vertex.getID() + (vertex.isActive() ? " is active " : " is not active"));
                        }
                    }
                  Console.WriteLine("---------------------------------");
                }
                
                i++;
                if (currentEvent.getType() == EventType.Edge)
                {
                    Console.WriteLine("Edge event between " + currentEvent.getFirstPoint().getID() + " and " + currentEvent.getSecondPoint().getID());
                    Node<Vertex> prevNode = currentLAV.Find(currentEvent.getFirstPoint());
                    Node<Vertex> nextNode = currentLAV.Find(currentEvent.getSecondPoint());
                    
                    //check if event is outdated
                    if (prevNode == null || nextNode == null || (!prevNode.Value.isActive() && !nextNode.Value.isActive()))
                    {
                        Console.WriteLine("Skipped edge event");
                        continue;
                    }

                    Vertex prevV = prevNode.Value;
                    Vertex nextV = nextNode.Value;

                    //check if we remain to the last 3 points
                    if (prevNode.PrevActive().PrevActive().Value.Equals(nextV))
                    {
                        Point2D intersect = new Point2D(LineTools.LineIntersect(prevV.getPoint(), prevV.getRayStepPoint(), nextV.getPoint(), nextV.getRayStepPoint()));


                        ret.Add(prevV.getPoint());
                        ret.Add(intersect);
                        ret.Add(nextV.getPoint());
                        ret.Add(intersect);
                        ret.Add(prevNode.PrevActive().Value.getPoint());
                        ret.Add(intersect);

                        currentLAV.Find(prevV).Value.setActive(false);
                        currentLAV.Find(nextV).Value.setActive(false);
                        currentLAV.Find(prevV).PrevActive().Value.setActive(false);
                        continue;
                    }

                    //output two arcs
                    ret.Add(currentEvent.getFirstPoint().getPoint());
                    ret.Add(currentEvent.getIntersection());
                    ret.Add(currentEvent.getSecondPoint().getPoint());
                    ret.Add(currentEvent.getIntersection());

                         
                    //modify list
                    currentLAV.Find(currentEvent.getFirstPoint()).Value.setActive(false);
                    currentLAV.Find(currentEvent.getSecondPoint()).Value.setActive(false);

                    Point2D intersection = currentEvent.getIntersection();
                    if (!intersection.Equals(Point2D.Zero))
                    {
                        Vertex newV = new Vertex(intersection, id++);
                        newV.prevEdge = prevV.prevEdge;
                        newV.nextEdge = nextV.nextEdge;
                        newV.update();

                        Node<Vertex> newNode = new Node<Vertex>(newV);

                        currentLAV.AddAfter(prevV, newV);
                        //currentLAV.Remove(prevV);
                        //currentLAV.Remove(nextV);

                        findClosestIntersectionAndStore(currentLAV, currentLAV.Find(newV).PrevActive().Value, currentLAV.Find(newV).Value, currentLAV.Find(newV).NextActive().Value);
                    }
                }
                else
                {
                    Console.WriteLine("Split event " + currentEvent.getFirstPoint().getID());
                    Node<Vertex> prevNode = currentLAV.Find(currentEvent.getFirstPoint());

                    //check if event is outdated
                    if (prevNode == null || !prevNode.Value.isActive())
                    {
                        
                        Console.WriteLine("Skipped split event");
                        continue;
                    }
                    Vertex prevV = prevNode.Value;
                    prevV.setActive(false);

                    //check if we remain to the last 3 points
                    if (prevNode.PrevActive().PrevActive().Value.Equals(prevNode.NextActive().Value))
                    {
                        Point2D intersect = new Point2D(LineTools.LineIntersect(prevV.getPoint(), prevV.getRayStepPoint(), prevNode.Next.Value.getPoint(), prevNode.Next.Value.getRayStepPoint()));



                        ret.Add(prevNode.Value.getPoint());
                        ret.Add(intersect);
                        ret.Add(prevNode.NextActive().Value.getPoint());
                        ret.Add(intersect);
                        ret.Add(prevNode.PrevActive().Value.getPoint());
                        ret.Add(intersect);

                        continue;
                    }
                    //output only VI
                    ret.Add(prevV.getPoint());
                    ret.Add(currentEvent.getIntersection());

                    //split LAV reset que etc
                    Vertex newNodeV1 = new Vertex(currentEvent.getIntersection(), id++);
                    newNodeV1.prevEdge = currentEvent.getFirstPoint().prevEdge;
                    newNodeV1.nextEdge = currentEvent.getFirstEdgePoint().nextEdge;
                    newNodeV1.update();

                    Vertex newNodeV2 = new Vertex(currentEvent.getIntersection(), id++);
                    newNodeV2.prevEdge = currentEvent.getFirstPoint().nextEdge;
                    newNodeV2.nextEdge = currentEvent.getFirstEdgePoint().nextEdge;
                    newNodeV2.update();

                    CircularLinkedList<Vertex> newLAV = new CircularLinkedList<Vertex>();
                    newLAV.AddFirst(newNodeV2);
                    Node<Vertex> current = SLAV[currentEvent.getLAVID()].Find(currentEvent.getFirstPoint());

                    while(!current.Next.Value.Equals(currentEvent.getSecondEdgePoint()))
                    {
                        if (current.Next.Equals(current))
                        {
                            break;
                        }
                        current = current.Next;
                        newLAV.AddLast(current.Value);
                        //current.Value.setActive(false);
                        SLAV[currentEvent.getLAVID()].Remove(current.Value);
                    }
                    SLAV.Add(newLAV);
                    SLAV[currentEvent.getLAVID()].AddAfter(currentEvent.getFirstPoint(),newNodeV1);
                    //SLAV[currentEvent.getLAVID()].Find(currentEvent.getFirstPoint()).Value.setActive(false);
                    //SLAV[currentEvent.getLAVID()].Remove(currentEvent.getFirstPoint());

                    //test
                    for (int x = 0; x < newLAV.Count; x++)
                    {
                        Vertex prev = newLAV[x].PrevActive().Value;
                        Vertex curr = newLAV[x].Value;
                        Vertex next = newLAV[x].NextActive().Value;

                        findClosestIntersectionAndStore(newLAV, prev, curr, next);
                    }

                    //findClosestIntersectionAndStore(newLAV, newLAV.Find(newNodeV2).PrevActive().Value, newLAV.Find(newNodeV2).Value, newLAV.Find(newNodeV2).NextActive().Value);
                    findClosestIntersectionAndStore(SLAV[currentEvent.getLAVID()], SLAV[currentEvent.getLAVID()].Find(newNodeV1).PrevActive().Value, SLAV[currentEvent.getLAVID()].Find(newNodeV1).Value, SLAV[currentEvent.getLAVID()].Find(newNodeV1).NextActive().Value);
                
                }
            }
            return ret;
        }


        public void draw(List<Point2D> toDraw)
        {
            var vertices = convertToVertexPositioncolor(Color.Red, toDraw);

            if (vertices.Count() > 0)
            {
                game.basicEffect.CurrentTechnique.Passes[0].Apply();
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Count() / 2);
            }

            game.spriteBatch.Begin();
            foreach (OwnVector2 vec in this.testDots)
            {
                game.spriteBatch.Draw(game.dummyTexture, DrawTools.createSmallDrawableRectangle(vec), Color.Blue);
            }
            game.spriteBatch.End();
        }

        public void draw(Game1 game)
        {
            List<Point2D> toDraw = this.Skeleton();
            var vertices = convertToVertexPositioncolor(Color.Red, toDraw);

            if (vertices.Count() > 0)
            {
                game.basicEffect.CurrentTechnique.Passes[0].Apply();
                game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Count() / 2);
            }

            game.spriteBatch.Begin();
            foreach (OwnVector2 vec in this.testDots)
            {
                game.spriteBatch.Draw(game.dummyTexture, DrawTools.createSmallDrawableRectangle(vec), Color.Blue);
            }
            game.spriteBatch.End();
        }

        /// <summary>
        /// for debug
        /// </summary>
        /// <param name="color"></param>
        /// <param name="tempList"></param>
        /// <returns></returns>
        private static VertexPositionColor[] convertToVertexPositioncolor(Color color, List<Point2D> tempList)
        {
            int totalVertices = tempList.Count;
            var vertices = new VertexPositionColor[totalVertices];
            for (int i = 0; i < totalVertices; i++ )
            {
                vertices[i].Position = new Vector3(tempList[i].X, tempList[i].Y, 0);
                vertices[i].Color = color;
            }
            return vertices;
        }
    }
}
