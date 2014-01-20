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
        public List<Rectangle> subdividePoints, intersectionPoints, controlPoints, mirrorPoints;
        public List<List<Rectangle>> controlPointsHoles, mirrorPointsHoles;
        public int currentSelectedHole;
        public Random random;
        private Vertices intersectionVertices;
        public MyGUI _gui;
        private Polygon polygon;
        private FpsCounterComponent fps;
        private cpp_file cpp = new cpp_file();
        private Texture2D texture1px;
        private Texture2D textureTrans;
        private int cols;
        private int rows;
        private int gridWidth;
        private int gridHeight;
        private int topLeftGridX;
        private int topLeftGridY;
        private int gridSize;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyBoardState;
        private bool snappingMode;
        private bool mirrorMode;
          
 
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
            mirrorPoints = new List<Rectangle>();
            controlPointsHoles = new List<List<Rectangle>>();
            mirrorPointsHoles = new List<List<Rectangle>>();

            intersectionVertices = new Vertices();
            polygon = new Polygon(new Vertices(), true);

            //TEST SHAPE
            //loadTestShape();

            // GUI
            _gui.Init();
            fps.Initialize();

            //grid
            gridSize = 28;
            snappingMode = false;

            //mirrormode
            mirrorMode = false;

            // Load
            base.Initialize();
        }

        private void loadTestShape()
        {
            //initial shape to be shown for "playing" rectangle test
            List<Point2D> list = new List<Point2D>();
            list.Add(new Point2D(100, 200));
            list.Add(new Point2D(400, 200));
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

            List<Point2D> hole2 = new List<Point2D>();
            hole2.Add(new Point2D(260, 260));
            hole2.Add(new Point2D(310, 260));
            hole2.Add(new Point2D(310, 310));
            hole2.Add(new Point2D(260, 310));

            currentSelectedHole = 0;

            foreach (Point2D point in list)
            {
                polygon.ControlVertices.Add(point);
                controlPoints.Add(DrawTools.createDrawableRectangle(point));
            }
            polygon.ControlVertices.Holes = new List<Vertices>();
            polygon.ControlVertices.Holes.Add(new Vertices());
            polygon.ControlVertices.Holes.Add(new Vertices());

            controlPointsHoles.Add(new List<Rectangle>());
            controlPointsHoles.Add(new List<Rectangle>());

            foreach (Point2D point in hole)
            {
                polygon.ControlVertices.Holes[0].Add(point);
                controlPointsHoles[0].Add(DrawTools.createDrawableRectangle(point));
            }
            foreach (Point2D point in hole2)
            {
                polygon.ControlVertices.Holes[1].Add(point);
                controlPointsHoles[1].Add(DrawTools.createDrawableRectangle(point));
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

            //Grid Textures
            generateTextures();

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

            //Grid update
            updateGridInfo();

            //Handle the mouse inputs, clicks, drags, etc. 
            handleMouseInput();
            handleKeyBoardInput();

            //Update polygon (vertices / intersections)
            polygon.update(this, mirrorMode);

            //Calculate intersection points
            updateIntersectionPoints();
            
           
            //Calculate points in between lines for subdivision
            updateSubdividePoints();

            //Calculate straight Skeleton 
            updateSS();
            


            //Update the GUI
            _gui.Update();
            
            base.Update(gameTime);
        }

        private void updateSS()
        {
            output = new float[5000];
            output2 = new float[5000];

            Vertices ssVertices = new Vertices();
            ssVertices.Holes = polygon.ControlVertices.Holes;

            foreach (Point2D point in polygon.ControlVertices)
            {
                ssVertices.Add(point);
            }
            if (mirrorMode)
            {
                for (int x = polygon.MirrorVertices.Count - 1; x >= 0; x--)
                {
                    ssVertices.Add(polygon.MirrorVertices[x]);
                }
                foreach (Vertices hole in polygon.MirrorVertices.Holes)
                {
                    ssVertices.Holes.Add(hole); 
                }
            }

            if (ssVertices.IsSimple())
            {
                ssVertices.ForceCounterClockWise();

                float[] points = new float[ssVertices.Count * 2];

                for (int i = 0, j = 0; i < ssVertices.Count * 2; j++, i = i + 2)
                {
                    points[i] = ssVertices[j].X;
                    points[i + 1] = ssVertices[j].Y;
                }

                if (ssVertices.Holes.Count > 0)
                {
                    int i = 0;
                    int totalSpaceNeeded = 0;
                    foreach (Vertices hole in ssVertices.Holes)
                    {
                        hole.ForceClockWiseHole();
                        totalSpaceNeeded += hole.Count * 2;
                    }

                    float[] holes = new float[totalSpaceNeeded];
                    int[] holeEnds = new int[ssVertices.Holes.Count];

                    int x = 0;
                    foreach (Vertices hole in ssVertices.Holes)
                    {
                        for (int j = 0; j < hole.Count; j++, i = i + 2)
                        {
                            holes[i] = hole[j].X;
                            holes[i + 1] = hole[j].Y;
                        }
                        holeEnds[x++] = hole.Count;
                    }


                    cpp.SSAwithHoles(points, points.Length, holes, holeEnds, holeEnds.Length, output, output2, _gui.getSubdivideSize());
                }
                else
                {
                    cpp.SSAwithoutHoles(points, points.Length, output, output2, _gui.getSubdivideSize());
                }
            }
        }

        private void handleKeyBoardInput()
        {
            if (keyboardState.IsKeyDown(Keys.S))
            {
                snappingMode = !snappingMode;
            }
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
            if (mouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released)
            {
                mirrorMode = !mirrorMode;
            }
            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
            {
                //enable/disablef snapping mode
                snappingMode = !snappingMode;
            }

            int xCor = mouseState.X;
            int yCor = mouseState.Y;
            if (snappingMode)
            {
                xCor = ((int)(xCor / gridSize)) * gridSize + topLeftGridX;
                yCor = ((int)(yCor / gridSize)-1) * gridSize + topLeftGridY;
            }

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
                    Point2D point = new Point2D(xCor, yCor);
                    polygon.ControlVertices.Add(point);
                    controlPoints.Add(DrawTools.createDrawableRectangle(point));
                    Point2D mirrorPoint = new Point2D(graphics.PreferredBackBufferWidth - xCor, yCor);
                    polygon.MirrorVertices.Add(mirrorPoint);
                    mirrorPoints.Add(DrawTools.createDrawableRectangle(mirrorPoint));
                }
                else
                {
                    if (_gui.newHoleToggleButton.IsToggled || polygon.ControlVertices.Holes.Count == 0)
                    {
                        controlPointsHoles.Add(new List<Rectangle>());
                        mirrorPointsHoles.Add(new List<Rectangle>());
                        polygon.ControlVertices.Holes.Add(new Vertices());
                        polygon.MirrorVertices.Holes.Add(new Vertices());

                        currentSelectedHole = controlPointsHoles.Count - 1;

                        _gui.newHoleToggleButton.IsToggled = false;
                    }
                    Point2D point = new Point2D(xCor, yCor);
                    polygon.ControlVertices.Holes[currentSelectedHole].Add(point);
                    controlPointsHoles[currentSelectedHole].Add(DrawTools.createDrawableRectangle(point));

                    Point2D mirrorPoint = new Point2D(graphics.PreferredBackBufferWidth - xCor, yCor);
                    polygon.MirrorVertices.Holes[currentSelectedHole].Add(mirrorPoint);
                    mirrorPointsHoles[currentSelectedHole].Add(DrawTools.createDrawableRectangle(mirrorPoint));
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
                    Point2D point = new Point2D(xCor, yCor);
                    polygon.ControlVertices[polygon.ControlVertices.Count - 1] = point;
                    controlPoints[polygon.ControlVertices.Count - 1] = DrawTools.createDrawableRectangle(point);
                    Point2D mirrorPoint = new Point2D(graphics.PreferredBackBufferWidth - xCor, yCor);
                    polygon.MirrorVertices[polygon.MirrorVertices.Count-1] = mirrorPoint;
                    mirrorPoints[polygon.MirrorVertices.Count - 1] = DrawTools.createDrawableRectangle(mirrorPoint);
                }
                else
                {
                    Point2D point = new Point2D(xCor, yCor);
                    polygon.ControlVertices.Holes[currentSelectedHole][polygon.ControlVertices.Holes[currentSelectedHole].Count - 1] = point;
                    controlPointsHoles[currentSelectedHole][polygon.ControlVertices.Holes[currentSelectedHole].Count - 1] = DrawTools.createDrawableRectangle(point);

                    Point2D mirrorPoint = new Point2D(graphics.PreferredBackBufferWidth - xCor, yCor);
                    polygon.MirrorVertices.Holes[currentSelectedHole][polygon.MirrorVertices.Holes[currentSelectedHole].Count - 1] = mirrorPoint;
                    mirrorPointsHoles[currentSelectedHole][polygon.MirrorVertices.Holes[currentSelectedHole].Count - 1] = DrawTools.createDrawableRectangle(mirrorPoint);
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
                for (int x = 0; x < controlPointsHoles.Count; x++)
                {
                    for (int i = 0; i < controlPointsHoles[x].Count; i++)
                    {
                        if (controlPointsHoles[x][i].Contains(new Point(mouseState.X, mouseState.Y)))
                        {
                            currentSelectedHole = x;
                            draggingHolePoints.Add(i);
                        }
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
                    point.X = xCor;
                    point.Y = yCor;
                    polygon.ControlVertices[draggingPoints[i]] = point;

                    Point2D mirrorPoint = polygon.MirrorVertices[draggingPoints[i]];
                    mirrorPoint.X = graphics.PreferredBackBufferWidth - xCor;
                    mirrorPoint.Y = yCor;
                    polygon.MirrorVertices[draggingPoints[i]] = mirrorPoint;

                    
                    Rectangle rectangle = controlPoints[draggingPoints[i]];
                    rectangle.X = xCor - (rectangle.Width / 2);
                    rectangle.Y = yCor - (rectangle.Height / 2);
                    controlPoints[draggingPoints[i]] = rectangle;

                    Rectangle mirrorRectangle = mirrorPoints[draggingPoints[i]];
                    mirrorRectangle.X = graphics.PreferredBackBufferWidth - (xCor + (mirrorRectangle.Width / 2));
                    mirrorRectangle.Y = yCor - (mirrorRectangle.Height / 2);
                    mirrorPoints[draggingPoints[i]] = mirrorRectangle;
                    
                }
                for (int i = 0; i < draggingHolePoints.Count; i++)
                {
                    Point2D point = polygon.ControlVertices.Holes[currentSelectedHole][draggingHolePoints[i]];
                    point.X = xCor;
                    point.Y = yCor;
                    polygon.ControlVertices.Holes[currentSelectedHole][draggingHolePoints[i]] = point;

                    Rectangle rectangle = controlPointsHoles[currentSelectedHole][draggingHolePoints[i]];
                    rectangle.X = xCor - (rectangle.Width / 2);
                    rectangle.Y = yCor - (rectangle.Height / 2);
                    controlPointsHoles[currentSelectedHole][draggingHolePoints[i]] = rectangle;

                    Point2D mirrorPoint = polygon.MirrorVertices.Holes[currentSelectedHole][draggingHolePoints[i]];
                    mirrorPoint.X = xCor;
                    mirrorPoint.Y = yCor;
                    polygon.MirrorVertices.Holes[currentSelectedHole][draggingHolePoints[i]] = mirrorPoint;

                    Rectangle mirrorRectangle = mirrorPointsHoles[currentSelectedHole][draggingHolePoints[i]];
                    mirrorRectangle.X = graphics.PreferredBackBufferWidth - (xCor - (mirrorRectangle.Width / 2));
                    mirrorRectangle.Y = yCor - (mirrorRectangle.Height / 2);
                    mirrorPointsHoles[currentSelectedHole][draggingHolePoints[i]] = mirrorRectangle;
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

            //Draw Grid
            if (snappingMode)
            {
                drawGridLines();
            }

            //Draw Mirror
            if(mirrorMode)
            {
                drawMirrorOverlay();
            }

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

        private void drawMirrorOverlay()
        {
            Rectangle rect = new Rectangle((int)(cols / 2 * gridSize) + topLeftGridX, 0 + topLeftGridY, (int)(cols / 2 * gridSize), (int)(rows * gridSize));
            spriteBatch.Draw(textureTrans, rect, Color.Gold);
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
            if (mirrorMode)
            {
                for (int i = 0; i < mirrorPoints.Count; i++)
                {
                    spriteBatch.Draw(dummyTexture, mirrorPoints[i], Color.Cyan);
                }
                foreach (List<Rectangle> list in mirrorPointsHoles)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        spriteBatch.Draw(dummyTexture, list[i], Color.Cyan);
                    }
                }
            }
            for (int x = 0; x < controlPointsHoles.Count; x++ )
            {
                for (int i = 0; i < controlPointsHoles[x].Count; i++)
                {
                    if (draggingHolePoints.Contains(i))
                    {
                        spriteBatch.Draw(dummyTexture, controlPointsHoles[x][i], Color.Black);
                    }
                    else if (currentSelectedHole == x)
                    {
                        spriteBatch.Draw(dummyTexture, controlPointsHoles[x][i], Color.Gold);
                    }
                    else
                    {
                        spriteBatch.Draw(dummyTexture, controlPointsHoles[x][i], Color.Gray);
                    }
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
            mirrorPoints.Clear();

            polygon.ControlVertices.Holes.Clear();
            polygon.ControlVertices.Holes.Add(new Vertices());
            controlPointsHoles.Clear();
            controlPointsHoles.Add(new List<Rectangle>());
            mirrorPointsHoles.Clear();
            mirrorPointsHoles.Add(new List<Rectangle>());
            currentSelectedHole = 0;
            polygon.Clear();
        }

        internal void removeSelectedHole()
        {
            controlPointsHoles.RemoveAt(currentSelectedHole);
            mirrorPointsHoles.RemoveAt(currentSelectedHole);

            if (controlPointsHoles.Count <= currentSelectedHole)
            {
                currentSelectedHole--;
            }
            if (currentSelectedHole < 0)
            {
                polygon.ControlVertices.Holes.Clear();
                polygon.ControlVertices.Holes.Add(new Vertices());
                polygon.MirrorVertices.Holes.Clear();
                polygon.MirrorVertices.Holes.Add(new Vertices());

                currentSelectedHole = 0;
            }
        }

        private void generateTextures()
        {
            texture1px = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture1px.SetData(new Color[] { Color.White });

            Byte transparency_amount = 100; //0 transparent; 255 opaque
            textureTrans = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.FromNonPremultiplied(255, 255, 255, transparency_amount);
            textureTrans.SetData<Color>(c);
        }

        private void updateGridInfo()
        {
            cols = (int)(this.graphics.PreferredBackBufferWidth / gridSize);
            rows = (int)((this.graphics.PreferredBackBufferHeight - 40) / gridSize);
            gridWidth = this.graphics.PreferredBackBufferWidth;
            gridHeight = this.graphics.PreferredBackBufferHeight;

            topLeftGridX = 0 + (int)((this.graphics.PreferredBackBufferWidth - (cols*gridSize))/2);
            topLeftGridY = 40 + (int)((this.graphics.PreferredBackBufferHeight - 40 - (rows * gridSize)) / 2); ;
        }

        private void drawGridLines()
        {
            for (float x = 0; x < cols + 1; x++)
            {
                Rectangle rectangle = new Rectangle((int)(topLeftGridX + x * gridSize), topLeftGridY, 1, (int)(rows * gridSize));
                spriteBatch.Draw(textureTrans, rectangle, Color.Black);
            }
            for (float y = 0; y < rows + 1; y++)
            {
                Rectangle rectangle = new Rectangle(topLeftGridX, (int)(topLeftGridY + y * gridSize), (int)(cols * gridSize), 1);
                spriteBatch.Draw(textureTrans, rectangle, Color.Black);
            }
        }
    }
}
