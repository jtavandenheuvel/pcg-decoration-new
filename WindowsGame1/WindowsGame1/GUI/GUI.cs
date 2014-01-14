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
        public ToggleButton drawToggleButton, polyToggleButton, newHoleToggleButton;
        public Button removeHoleButton;
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

            Button clearButton = new Button(0, 5, "Clear Drawing", 5, delegate { game.resetDrawing(); });

            comboBox = new ComboBox(755, 5, 200, "Choose Style", CardinalDirection.South, new List<ComboBox.DropDownItem>{
                new ComboBox.DropDownItem("No Style", null, null),
                new ComboBox.DropDownItem("Color Style", null, null),
                new ComboBox.DropDownItem("Test Style", null, null),
                new ComboBox.DropDownItem("Greek Style", null, null),
                new ComboBox.DropDownItem("ZigZag Style", null,null)
            });

            slider = new Slider(980, 10, 150, delegate{
                sliderLabel.Value = "" + Math.Round(((Slider)slider).Value * 99 + 1, 2);
            });
            sliderLabel = new Label(1122, 12, "1");
            slider.Value = 0.3f;


            slider2 = new Slider(1170, 10, 150, delegate
            {
                sliderLabel2.Value = "" + Math.Round(((Slider)slider2).Value * 39 + 1, 2);
            });
            sliderLabel2 = new Label(1312, 12, "1");
            slider2.Value = 0.3f;

            drawToggleButton = new ToggleButton(200, 5, "Moving Mode", 5);
            polyToggleButton = new ToggleButton(325, 5, "Drawing Poly", 5);
            newHoleToggleButton = new ToggleButton(445, 5, "Current Hole", 10);
            removeHoleButton = new Button(575, 5, "Remove Sel. Hole", 5);
            removeHoleButton.ClickEvent += delegate { game.removeSelectedHole(); };


            //drawToggleButton.OnToggle += delegate { polyToggleButton.IsToggled = false; };
            //polyToggleButton.OnToggle += delegate { drawToggleButton.IsToggled = false; };


            gui = new Gui(game, skin, text, testSkins, testTexts)
            {
                Widgets = new Widget[] { clearButton, drawToggleButton, polyToggleButton, newHoleToggleButton, removeHoleButton, comboBox, slider, sliderLabel, slider2, sliderLabel2 }
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
            newHoleToggleButton.Label = newHoleToggleButton.IsToggled ? "New Hole" : "Selected Hole";
            game.spriteBatch.End();
            gui.Draw();
            game.spriteBatch.Begin();
        }
    }
}
