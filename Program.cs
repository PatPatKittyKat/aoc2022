using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public class Program
    {
        const string introMsg = "Hello :)"
                            +"\nHaven't heard too much about AoC until recently, and I figured I could use"
                            +"\na refresher on setting up new projects from scratch, new source control, etc and working on some"
                            +"\nsimpler problems without any pressure to finish before some deadline. Happy reading.";

        public static void Main(string[] args)
        {
            //List of all problems (25) can be found here: https://adventofcode.com/2022
            List<int> problems = new List<int>{1};

            // Problem Solver.
            List<string> answers = ProblemSolver.SolveMultiple(problems);

            // Introductory messaging.
            Console.WriteLine(introMsg+'\n');
            
            // Can print answers as they're solved, or just add to our list to print at the end.
            foreach(string s in answers)
            {
                Console.WriteLine(s);
            }
        }

        //moved to ProblemSolver.
    }
}