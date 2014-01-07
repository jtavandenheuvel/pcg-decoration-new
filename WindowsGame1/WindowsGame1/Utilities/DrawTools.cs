using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics;

namespace WindowsGame1.Utilities
{
    public static class DrawTools
    {
        public static Rectangle createSmallDrawableRectangle(OwnVector2 vec)
        {
            Rectangle rectangle = new Rectangle((int)vec.X - (Settings.rectangleWidth / 4), (int)vec.Y - (Settings.rectangleHeight / 4), Settings.rectangleWidth / 2, Settings.rectangleHeight / 2);
            return rectangle;
        }

        public static Rectangle createSmallDrawableRectangle(OwnVector3 position)
        {
            Rectangle rectangle = new Rectangle((int)position.X - (Settings.rectangleWidth / 4), (int)position.Y - (Settings.rectangleHeight / 4), Settings.rectangleWidth / 2, Settings.rectangleHeight / 2);
            return rectangle;
        }

        public static Rectangle createDrawableRectangle(OwnVector2 vec)
        {
            Rectangle rectangle = new Rectangle((int)vec.X - (Settings.rectangleWidth / 2), (int)vec.Y - (Settings.rectangleHeight / 2), Settings.rectangleWidth, Settings.rectangleHeight);
            return rectangle;
        }

        public static Rectangle createDrawableRectangle(OwnVector3 position)
        {
            Rectangle rectangle = new Rectangle((int)position.X - (Settings.rectangleWidth / 2), (int)position.Y - (Settings.rectangleHeight / 2), Settings.rectangleWidth, Settings.rectangleHeight);
            return rectangle;
        }

        public static Rectangle createDrawableRectangle(Point point)
        {
            Rectangle rectangle = new Rectangle((int)point.X - (Settings.rectangleWidth / 2), (int)point.Y - (Settings.rectangleHeight / 2), Settings.rectangleWidth, Settings.rectangleHeight);
            return rectangle;
        }

        /// <summary>
        /// Gives new direction vector with angle in degrees turning left
        /// </summary>
        /// <param name="angle">turn rate in degrees</param>
        /// <param name="direction">current direction</param>
        /// <returns></returns>
        public static OwnVector3 TurnLeft(float angle, OwnVector3 direction)
        {
            float newAngle = VectorToAngle(direction) + angle;
            return AngleToVector(newAngle);
        }

        /// <summary>
        /// Gives new direction vector with angle in degrees turning right
        /// </summary>
        /// <param name="angle">turn rate in degrees</param>
        /// <param name="direction">current direction</param>
        /// <returns></returns>
        public static OwnVector3 TurnRight(float angle, OwnVector3 direction)
        {
            float newAngle = VectorToAngle(direction) - angle;
            return AngleToVector(newAngle);
        }

        /// <summary>
        /// Returns new OwnVector3 moved point from origin along direction for distance
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static OwnVector3 MoveForward(OwnVector3 origin, OwnVector3 direction, float distance)
        {
            return origin + (distance * direction);
        }

        public static OwnVector3 MoveBackward(OwnVector3 origin, OwnVector3 direction, float distance)
        {
            return origin - (distance * direction);
        }


        /// <summary>
        /// 2D Case supported only
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static OwnVector3 AngleToVector(float angle)
        {
            return new OwnVector3((float)Math.Cos(MathHelper.ToRadians(angle)), (float)Math.Sin(MathHelper.ToRadians(angle)),0);
        }

        /// <summary>
        /// 2D Case supported only 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private static float VectorToAngle(OwnVector3 vector)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(vector.Y, vector.X));
        }
    }
}
