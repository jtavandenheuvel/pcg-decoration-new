using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WindowsGame1.Utilities;

namespace WindowsGame1.Style_Generators
{
    interface StyleGenerator
    {
        int maximumSegmentSize {get; set;} 

        //TODO: Still need to decide what inputs need to be supplied
        List<OwnVector3> createNormalSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint);
        List<OwnVector3> createEndSegment(OwnVector3 centerPoint, OwnVector3 prevPoint);
        List<OwnVector3> createCornerSegment(OwnVector3 centerPoint, OwnVector3 previousPoint, OwnVector3 nextPoint);
        List<OwnVector3> createTJunctionSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints);
        List<OwnVector3> createCrossingSegment(OwnVector3 centerPoint, List<OwnVector3> connectingPoints);
        List<OwnVector3> createTransistionSegment(OwnVector3 centerPoint, OwnVector3 prevPoint, OwnVector3 nextPoint);

        
    }
}
