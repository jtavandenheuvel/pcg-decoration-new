using System;
using System.Collections.Generic;
using System.Text;

namespace XnaContestGame.Components {
    interface IFpsCounterService {

        int InstantaneousFpsCount { get; }

        int AveragedFpsCount { get; }

        int LowestFpsCount { get; }

        int HighestFpsCount { get; }

        float LastFrameTime { get; }

        float AveragedFrameTime { get; }

    }
}
