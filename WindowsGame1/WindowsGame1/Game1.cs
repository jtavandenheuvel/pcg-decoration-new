using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Ruminate.GUI.Content;
using Ruminate.GUI.Framework;
using System.IO;
using WindowsGame1.GUIv2;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using WindowsGame1.Utilities;
using WindowsGame1.Straight_Skeleton;
using XnaContestGame.Components;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public BasicEffect basicEffect;
        private MouseState mouseState, previousMouseState;
        public Texture2D dummyTexture;
        public List<int> draggingPoints, draggingHolePoints;
        public List<Rectangle> subdividePoints, intersectionPoints, controlPoints, controlPointsHoles;
        public Random random;
        private Vertices intersectionVertices;
        public MyGUI _gui;
        private Polygon polygon;
        private FpsCounterComponent fps;
        private cpp_file cpp = new cpp_file();
          
 
        // straight skeleton test
        private float[] output = new float[5000];
        private float[] output2 = new float[5000];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            random = new Random();
            _gui = new MyGUI(this);
            fps = new FpsCounterComponent(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Make mouse visible
            this.IsMouseVisible = true;

            // Create empty points list to save points to 
            draggingPoints = new List<int>();
            draggingHolePoints = new List<int>();
            subdividePoints = new List<Rectangle>();
            intersectionPoints = new List<Rectangle>();
            controlPoints = new List<Rectangle>();
            controlPointsHoles = new List<Rectangle>();

            intersectionVertices = new Vertices();
            polygon = new Polygon(new Vertices(), true);

            //TEST SHAPE
            loadTestShape();

            // GUI
            _gui.Init();
            fps.Initialize();

            // Load
            base.Initialize();
        }

        private void loadTestShape()
        {
            //initial shape to be shown for "playing" rectangle test
            List<Point2D> list = new List<Point2D>();
            list.Add(new Point2D(100, 200));
            list.Add(new Point2D(410, 210));
            list.Add(new Point2D(400, 600));
            list.Add(new Point2D(300, 600));
            list.Add(new Point2D(300, 500));
            list.Add(new Point2D(285, 425));
            list.Add(new Point2D(250, 400));
            list.Add(new Point2D(215, 425));
            list.Add(new Point2D(200, 500));
            list.Add(new Point2D(200, 600));
            list.Add(new Point2D(100, 600));

            List<Point2D> hole = new List<Point2D>();
            hole.Add(new Point2D(150, 250));
            hole.Add(new Point2D(200, 250));
            hole.Add(new Point2D(200, 300));
            hole.Add(new Point2D(150, 300));
            
            foreach (Point2D point in list)
            {
                polygon.ControlVertices.Add(point);
                controlPoints.Add(DrawTools.createDrawableRectangle(point));
            }
            polygon.ControlVertices.Holes = new List<Vertices>();
            polygon.ControlVertices.Holes.Add(new Vertices());

            foreach (Point2D point in hole)
            {
                polygon.ControlVertices.Holes[0].Add(point);
                controlPointsHoles.Add(DrawTools.createDrawableRectangle(point));
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            // Create a BasicEffect for drawing primitives
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Update Controller States
            updateControllerStates();
            //Handle the mouse inputs, clicks, drags, etc. 
            handleMouseInput();
           
            //Update polygon (vertices / intersections)
            polygon.update(this);

            //Calculate intersection points
            updateIntersectionPoints();
            
           
            //Calculate points in between lines for subdivision
            updateSubdividePoints();



            //Calculate straight Skeleton 
            output = new float[5000];
            output2 = new float[5000];
            if (polygon.ControlVertices.IsSimple())
            {
                polygon.ControlVertices.ForceCounterClockWise();
               
                float[] points = new float[polygon.ControlVertices.Count * 2];
            
                for (int i = 0, j = 0; i < polygon.ControlVertices.Count*2; j++, i=i+2)
                {
                    points[i] = polygon.ControlVertices[j].X;
                    points[i+1] = polygon.ControlVertices[j].Y;
                }


                polygon.ControlVertices.Holes[0].ForceClockWiseHole();

                float[] holes = new float[polygon.ControlVertices.Holes[0].Count * 2];
                for (int i = 0, j = 0; i < polygon.ControlVertices.Holes[0].Count * 2; j++, i = i + 2)
                {
                    holes[i] = polygon.ControlVertices.Holes[0][j].X;
                    holes[i + 1] = polygon.ControlVertices.Holes[0][j].Y;
                }
            
                
                cpp.times2(points, holes, output, output2, points.Length, holes.Length, _gui.getSubdivideSize());
            }
            


            //Update the GUI
            _gui.Update();
            
            base.Update(gameTime);
        }

        private void updateIntersectionPoints()
        {
            //update intersections in polygon
            intersectionPoints.Clear();
            foreach (OwnVector2 vec in polygon.getIntersectionPoints()) 
            {
                intersectionPoints.Add(DrawTools.createSmallDrawableRectangle(vec));
            }
        }

        /// <summary>
        /// DEBUG METHOD 
        /// </summary>
        private void updateSubdividePoints()
        {
            //Empty list
            subdividePoints.Clear();

            //Calculate new points
            foreach (OwnVector2 vector in polygon.getSubdivisionPoints())
            {
                subdividePoints.Add(DrawTools.createSmallDrawableRectangle(vector));
            }

        }

        private void handleMouseInput()
        {
            // Adding points when new click
            if (_gui.drawToggleButton.IsToggled
                && this.IsActive
                && !_gui.comboBox.IsPressed
                && mouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Released
                && mouseState.X >= 0 && mouseState.X < graphics.PreferredBackBufferWidth
                && mouseState.Y >= 40 && mouseState.Y < graphics.PreferredBackBufferHeight)
            {
                if (!_gui.polyToggleButton.IsToggled)
                {
                    Point2D point = new Point2D(mouseState.X, mouseState.Y);
                    polygon.ControlVertices.Add(point);
                    controlPoints.Add(DrawTools.createDrawableRectangle(point));
                }
                else
                {
                    Point2D point = new Point2D(mouseState.X, mouseState.Y);
                    polygon.ControlVertices.Holes[0].Add(point);
                    controlPointsHoles.Add(DrawTools.createDrawableRectangle(point));
                }

            }

            // Adding points when holding click
            if (_gui.drawToggleButton.IsToggled
                && this.IsActive
                && !_gui.comboBox.IsPressed
                && mouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Pressed
                && mouseState.X >= 0 && mouseState.X < graphics.PreferredBackBufferWidth
                && mouseState.Y >= 40 && mouseState.Y < graphics.PreferredBackBufferHeight)
            {
                if (!_gui.polyToggleButton.IsToggled)
                {
                    Point2D point = new Point2D(mouseState.X, mouseState.Y);
                    polygon.ControlVertices[polygon.ControlVertices.Count - 1] = point;
                    controlPoints[polygon.ControlVertices.Count - 1] = DrawTools.createDrawableRectangle(point);
                }
                else
                {
                    Point2D point = new Point2D(mouseState.X, mouseState.Y);
                    polygon.ControlVertices.Holes[0][polygon.ControlVertices.Holes[0].Count - 1] = point;
                    controlPointsHoles[polygon.ControlVertices.Holes[0].Count - 1] = DrawTools.createDrawableRectangle(point);
                }
            }

            // Check if start dragging point 
            if (!_gui.drawToggleButton.IsToggled
                && this.IsActive
                && mouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Released
                && mouseState.X >= 0 && mouseState.X < graphics.PreferredBackBufferWidth
                && mouseState.Y >= 40 && mouseState.Y < graphics.PreferredBackBufferHeight)
            {
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    if (controlPoints[i].Contains(new Point(mouseState.X, mouseState.Y)))
                    {
                        draggingPoints.Add(i);
                    }
                }
                for (int i = 0; i < controlPointsHoles.Count; i++)
                {
                    if (controlPointsHoles[i].Contains(new Point(mouseState.X, mouseState.Y)))
                    {
                        draggingHolePoints.Add(i);
                    }
                }
            }

            // Dragging Points 
            if (!_gui.drawToggleButton.IsToggled
                && this.IsActive
                && mouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Pressed
                && mouseState.X >= 0 && mouseState.X < graphics.PreferredBackBufferWidth
                && mouseState.Y >= 40 && mouseState.Y < graphics.PreferredBackBufferHeight)
            {
                for (int i = 0; i < draggingPoints.Count; i++)
                {
                    Point2D point = polygon.ControlVertices[draggingPoints[i]];
                    point.X = mouseState.X;
                    point.Y = mouseState.Y;
                    polygon.ControlVertices[draggingPoints[i]] = point;
                    
                    Rectangle rectangle = controlPoints[draggingPoints[i]];
                    rectangle.X = mouseState.X - (rectangle.Width / 2);
                    rectangle.Y = mouseState.Y - (rectangle.Height / 2);
                    controlPoints[draggingPoints[i]] = rectangle;
                }
                for (int i = 0; i < draggingHolePoints.Count; i++)
                {
                    Point2D point = polygon.ControlVertices.Holes[0][draggingHolePoints[i]];
                    point.X = mouseState.X;
                    point.Y = mouseState.Y;
                    polygon.ControlVertices.Holes[0][draggingHolePoints[i]] = point;

                    Rectangle rectangle = controlPointsHoles[draggingHolePoints[i]];
                    rectangle.X = mouseState.X - (rectangle.Width / 2);
                    rectangle.Y = mouseState.Y - (rectangle.Height / 2);
                    controlPointsHoles[draggingHolePoints[i]] = rectangle;
                }
            }

            // Clear Dragging List if left Button is released
            if (!_gui.drawToggleButton.IsToggled 
                && this.IsActive
                && mouseState.LeftButton == ButtonState.Released
                && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                draggingPoints.Clear();
                draggingHolePoints.Clear();
            }
        }

        private void updateControllerStates()
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // Drawing background
            GraphicsDevice.Clear(Color.White);
            this.spriteBatch.Begin();

            // Drawing debug rectangles 
            drawControlPoints();
            //drawDebugPoints();

            //Decide if we need to draw polygon
            polygon.Draw(this);

            //Draw skeleton V1 
            drawSkeleton(output, Color.Blue);
            drawSkeleton(output2, Color.Red);

            //draw menu
            _gui.Draw();
            base.Draw(gameTime);



            fps.Draw(gameTime);
            drawFPS();
            this.spriteBatch.End();

        }

        private void drawSkeleton(float[] output, Color color)
        {
            int totalVertices = output.Length/2;
            var vertices = new VertexPositionColor[totalVertices];
            for (int j = 0, i=0; j < totalVertices; j = j + 4, i = i+2)
            {
                vertices[i].Position = new Vector3(output[j], output[j + 1], 0);
                vertices[i + 1].Position = new Vector3(output[j + 2], output[j + 3], 0);
                vertices[i].Color = color;
                vertices[i + 1].Color = color;
            }
            if (totalVertices > 0)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Count() / 2);
            }
        }

        private void drawFPS()
        {
            Vector2 pos = new Vector2(graphics.PreferredBackBufferWidth-30, 61);
            this.spriteBatch.DrawString(this._gui.greySpriteFont, fps.InstantaneousFpsCount.ToString(), pos, Color.Black);
        }

        

        private void drawControlPoints()
        {
            for (int i = 0; i < controlPoints.Count; i++)
            {
                if (draggingPoints.Contains(i))
                {
                    spriteBatch.Draw(dummyTexture, controlPoints[i], Color.Black);
                }
                else
                {
                    spriteBatch.Draw(dummyTexture, controlPoints[i], Color.Red);
                }
            }
            for (int i = 0; i < controlPointsHoles.Count; i++)
            {
                if (draggingHolePoints.Contains(i))
                {
                    spriteBatch.Draw(dummyTexture, controlPointsHoles[i], Color.Black);
                }
                else
                {
                    spriteBatch.Draw(dummyTexture, controlPointsHoles[i], Color.Gold);
                }
            }
        }

        private void drawDebugPoints()
        {
            spriteBatch.Begin();
            foreach (Rectangle rectangle in subdividePoints)
            {
                spriteBatch.Draw(dummyTexture, rectangle, Color.Green);
            }
            foreach (Rectangle rectangle in intersectionPoints)
            {
                spriteBatch.Draw(dummyTexture, rectangle, Color.Blue);
            }

            spriteBatch.End();
        }

        
        // emptys the list of points 
        public void resetDrawing()
        {
            controlPoints.Clear();
            controlPointsHoles.Clear();
            polygon.Clear();
        }
    }
}
