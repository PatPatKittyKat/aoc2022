using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // List of all problems (25) can be found here: https://adventofcode.com/2022
            List<int> problems = new List<int>
                {1,2,3};

            // Problem Solver
            List<string> answers = ProblemSolver.SolveMultiple(problems);

            // Can print answers as they're solved, or just add to our list to print at the end.
            foreach(string s in answers)
            {
                Console.WriteLine(s);
            }

        }
    }
}