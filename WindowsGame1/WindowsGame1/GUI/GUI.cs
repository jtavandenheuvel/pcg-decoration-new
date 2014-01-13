using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using FarseerPhysics;

namespace WindowsGame1.GUIv2
{
    public class MyGUI
    {
        //constants
        private Game1 game;
        private Gui gui;
        public ToggleButton drawToggleButton, polyToggleButton;
        public ComboBox comboBox;
        public SpriteFont greySpriteFont;
        public string greyMap;
        public SpriteFont testSpriteFont;
        public Texture2D testImageMap;
        public string testMap;
        public Texture2D greyImageMap;
        Slider slider, slider2;
        Label sliderLabel, sliderLabel2;

        public MyGUI(Game1 game)
        {
            this.game = game;
        }

        public void Init()
        {
            greyImageMap = game.Content.Load<Texture2D>(@"GreySkin\ImageMap");
            greyMap = File.OpenText(@"Content\GreySkin\Map.txt").ReadToEnd();
            greySpriteFont = game.Content.Load<SpriteFont>(@"GreySkin\Texture");
            testImageMap = game.Content.Load<Texture2D>(@"TestSkin\ImageMap");
            testMap = File.OpenText(@"Content\TestSkin\Map.txt").ReadToEnd();
            testSpriteFont = game.Content.Load<SpriteFont>(@"TestSkin\Font");

            var skin = new Skin(this.greyImageMap, this.greyMap);
            var text = new Text(this.greySpriteFont, Color.White);

            var testSkin = new Skin(this.testImageMap, this.testMap);
            var testText = new Text(this.testSpriteFont, Color.White);

            var testSkins = new[] { new Tuple<string, Skin>("testSkin", testSkin) };
            var testTexts = new[] { new Tuple<string, Text>("testText", testText) };

            Button exitButton = new Button(0, 5, "Exit App", 5, delegate { game.Exit(); });
            Button clearButton = new Button(90, 5, "Clear Drawing", 5, delegate { game.resetDrawing(); });

            comboBox = new ComboBox(575, 5, 200, "Choose Style", CardinalDirection.South, new List<ComboBox.DropDownItem>{
                new ComboBox.DropDownItem("No Style", null, null),
                new ComboBox.DropDownItem("Color Style", null, null),
                new ComboBox.DropDownItem("Test Style", null, null),
                new ComboBox.DropDownItem("Greek Style", null, null),
                new ComboBox.DropDownItem("ZigZag Style", null,null)
            });

            slider = new Slider(800, 10, 150, delegate{
                sliderLabel.Value = "" + Math.Round(((Slider)slider).Value * 99 + 1, 2);
            });
            sliderLabel = new Label(950, 10, "1");
            slider.Value = 0.3f;


            slider2 = new Slider(990, 10, 150, delegate
            {
                sliderLabel2.Value = "" + Math.Round(((Slider)slider2).Value * 39 + 1, 2);
            });
            sliderLabel2 = new Label(1150, 10, "1");
            slider2.Value = 0.3f;

            drawToggleButton = new ToggleButton(250, 5, "Moving Mode", 5);
            polyToggleButton = new ToggleButton(375, 5, "Drawing Poly", 5);
            //drawToggleButton.OnToggle += delegate { polyToggleButton.IsToggled = false; };
            //polyToggleButton.OnToggle += delegate { drawToggleButton.IsToggled = false; };


            gui = new Gui(game, skin, text, testSkins, testTexts)
            {
                Widgets = new Widget[] { exitButton, clearButton, drawToggleButton, polyToggleButton, comboBox, slider, sliderLabel, slider2, sliderLabel2 }
            };
        }

        public Settings.DrawStyle getDrawStyle()
        {
            if (comboBox.SelectedItem != null)
            {
                string selected = comboBox.SelectedItem.Item.Item2;
                if (selected.Equals("Color Style"))
                    return Settings.DrawStyle.Color;
                else if (selected.Equals("Test Style"))
                    return Settings.DrawStyle.Test;
                else if (selected.Equals("Greek Style"))
                    return Settings.DrawStyle.Greek;
                else if (selected.Equals("ZigZag Style"))
                    return Settings.DrawStyle.ZigZag;
                else
                    return Settings.DrawStyle.None;
            }
            return Settings.DrawStyle.None;
        }

        public float getSubdivideSize()
        {
            return (float)Math.Round(((Slider)slider).Value * 99 + 1, 2);
        }

        public float getSegmentSize()
        {
            return (float)Math.Round(((Slider)slider2).Value * 39 + 1, 2);
        }

        public void OnResize()
        { 
            gui.Resize(); 
        }

        public void Update() 
        { 
            gui.Update(); 
        }

        public void Draw() {
            //draw menu backbar
            Rectangle menuBar = new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, 40);
            game.spriteBatch.Draw(game.dummyTexture, menuBar, Color.LightSlateGray);

            //change text on toggle buttons if needed
            drawToggleButton.Label = drawToggleButton.IsToggled ? "Drawing Mode" : "Moving Mode";
            polyToggleButton.Label = polyToggleButton.IsToggled ? "Drawing Holes" : "Drawing Poly";
            game.spriteBatch.End();
            gui.Draw();
            game.spriteBatch.Begin();
        }
    }
}
