using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public static class RopeBridgeHelper
    {
        public static void BuildModel(int[,] array, ref Dictionary<Tuple<int,int>, bool> model)
        {

        }

        // public static List<RopeBridgeModel> BuildModel2(int[,] array, int minX, int maxX, int minY, int maxY)
        // {
        //     List<RopeBridgeModel> models = new List<RopeBridgeModel>();
        //     for (int i = minX; i <= maxX; i++)
        //     {
        //         for (int j = minY; j <= maxY; j++)
        //         {
        //             RopeBridgeModel model = new RopeBridgeModel{
        //                 X = i,
        //                 Y = j,
        //                 IsVisited = false
        //             };
        //             models.Add(model);
        //         }
        //     }

        //     return models;
        // }

        public static void FlagVisitedCoordinate(Tuple<int,int> currentCoordinate, ref Dictionary<Tuple<int,int>, bool> model)
        {
            if (model.TryGetValue(currentCoordinate, out bool hasBeenVisited))
            {
                hasBeenVisited = true;
            }
        }

        
    }

    // public class RopeBridgeModel
    // {
    //     public int X = 0;
    //     public int Y = 0;
    //     public bool IsVisited = false;
    // }
}