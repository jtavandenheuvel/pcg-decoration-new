using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaContestGame.Components {
    class FpsCounterComponent : DrawableGameComponent, IFpsCounterService {

        const int cAveragingWindow = 25;

        public FpsCounterComponent(Game game)
            : base(game) {
            game.Services.AddService(typeof(IFpsCounterService), this);
        }

        int instantaneousFps, averagedFps;
        int averagingCount = 0, averagingTotal = 0;
        int lowestFps = Int32.MaxValue, highestFps = Int32.MinValue;

        float lastFrameTime = 0.0f, averagedFrameTime = 0.0f, averagingFrameTime = 0.0f;

        public override void Draw(GameTime gameTime) {
            float dTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Skip zero-time frames (infinity problem)
            if (dTime == 0.0f)
                return;

            //Compute instantaneous fps
            instantaneousFps = (int)Math.Round(1.0f / dTime);

            //Highest / Lowest
            if (instantaneousFps > highestFps)
                highestFps = instantaneousFps;
            if (instantaneousFps < lowestFps)
                lowestFps = instantaneousFps;

            //Frame times
            lastFrameTime = 1000.0f / instantaneousFps;

            //Averaging stuff
            ++averagingCount;
            averagingTotal += instantaneousFps;
            averagingFrameTime += lastFrameTime;
            if (averagingCount >= cAveragingWindow) {
                averagedFps = (int)Math.Round((float)averagingTotal / (float)averagingCount);
                averagedFrameTime = averagingFrameTime / averagingCount;

                averagingCount = 0;
                averagingTotal = 0;
                averagingFrameTime = 0.0f;
            }

            base.Draw(gameTime);
        }

        #region IFpsCounterService Members

        public int InstantaneousFpsCount {
            get {
                return instantaneousFps;
            }
        }

        public int AveragedFpsCount {
            get {
                return averagedFps;
            }
        }

        public int LowestFpsCount {
            get {
                return lowestFps;
            }
        }

        public int HighestFpsCount {
            get {
                return highestFps;
            }
        }

        public float LastFrameTime {
            get {
                return lastFrameTime;
            }
        }

        public float AveragedFrameTime {
            get {
                return averagedFrameTime;
            }
        }

        #endregion
    }
}
