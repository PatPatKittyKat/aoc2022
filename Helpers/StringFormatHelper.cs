using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public class StringFormatHelper
    {
        public static string BuildProblemMessage(int problem)
        {
            string problemName;
            switch (problem)
            {
                case 1:
                    problemName = ProblemStringFormatHelper(problem, "Calorie Counting");
                    break;
                case 2:
                    problemName = ProblemStringFormatHelper(problem, "Rock Paper Scissors");
                    break;
                default:
                    problemName = ProblemStringFormatHelper(0, "Could not find problem name");
                    break;
            };

            return problemName;
        }

        private static string ProblemStringFormatHelper(int day, string name) => string.Concat("Day ", day, ": ", name);
    }
}