using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame1.Utilities;
using Priority_Queue;

namespace WindowsGame1.Straight_Skeleton
{
    public enum EventType {
        Edge,
        Split
    }

    class Event : PriorityQueueNode
    {

        private Point2D intersectionPoint;
        private EventType type;
        private Vertex VA;
        private Vertex VB;
        private Vertex EA;
        private Vertex EB;
        private int idLAV;

        public Event(Point2D intersectionPoint, int idLAV, EventType type)
        {
            this.intersectionPoint = intersectionPoint;
            this.type = type;
            this.idLAV = idLAV;
        }

        public EventType getType(){
            return type;
        }

        public Point2D getIntersection()
        {
            return intersectionPoint;
        }

        public void storeFirstPoint(Vertex point)
        {
            this.VA = point;
        }

        public void storeSecondPoint(Vertex point)
        {
            this.VB = point;
        }

        public void storeFirstEdgePoint(Vertex point)
        {
            this.EA = point;
        }

        public void storeSecondEdgePoint(Vertex point)
        {
            this.EB = point;
        }

        public Vertex getFirstPoint()
        {
            return VA;
        }

        public Vertex getSecondPoint()
        {
            return VB;
        }

        public Vertex getFirstEdgePoint()
        {
            return EA;
        }

        public Vertex getSecondEdgePoint()
        {
            return EB;
        }

        public int getLAVID()
        {
            return idLAV;
        }
    }
}
