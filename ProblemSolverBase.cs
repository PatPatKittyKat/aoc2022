using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public class ProblemSolverBase
    {
        const string separator = " | ";
        const string part2 = "Part2: ";

        public static List<string> SolveMultiple(List<int> problems)
        {
            List<string> answers = new List<string>();
            foreach (int x in problems)
            {
                answers.Add(SolveProblem(x));
            }
            return answers;
        }

        public static string SolveProblem(int problem)
        {
            string result;
            string problemInfo = StringFormatHelper.BuildProblemMessage(problem);

            switch (problem)
            {
                case 1:
                    result = CalorieCounting();
                    result += separator + part2 + CalorieCounting(2);
                    break;
                case 2:
                    //result = RPS(); // my brute force solution
                    result = RPS2();  // my LinkedList solution
                    result += separator + part2 + RPS2(2);
                    break;
                case 3:
                    result = RucksackReorg();
                    result += separator + part2 + RucksackReorg(2);
                    break;
                case 4:
                    result = CampCleanup();
                    result += separator + part2 + CampCleanup(2);
                    break;
                case 5:
                    result = SupplyStacks();
                    result += separator + part2 + SupplyStacks(2);
                    break;
                case 6:
                    result = TuningTrouble();
                    result += separator + part2 + TuningTrouble(2);
                    break;
                case 7:
                    result = NoSpaceLeftOnDevice();
                    result += separator + part2 + NoSpaceLeftOnDevice(2);
                    break;
                case 8:
                    result = TreetopTreeHouse();
                    result += separator + part2 + TreetopTreeHouse(2);
                    break;
                default:
                    result = "??";
                    break;
            };

            return problemInfo + separator + "Answer: " + result;
        }
        
        // Day 1: https://adventofcode.com/2022/day/1
        public static string CalorieCounting(int part = 1)
        {
            // FileStream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem1.txt";
            Dictionary<int,int> elfCollection = ProblemSolver.CalorieCounting(filePath);

            // part 1
            if (part == 1)
            {
                int answer = elfCollection.OrderByDescending(x => x.Value).FirstOrDefault().Value;
                return Convert.ToString(answer);
            }

            // part 2
            int answer2 = elfCollection.OrderByDescending(x => x.Value).Take(3).Sum(x => x.Value);
            return Convert.ToString(answer2);
        }

        // Day 2: https://adventofcode.com/2022/day/2
        // THIS METHOD IS OBSOLETE. It's a brute force attempt that I refactored the next day. See RPS2()
        public static string RPS(int part = 1)
        {
            // FileStream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem2.txt";
            List<string[]> inputList = ProblemSolver.RPS(filePath);
            int total = 0;

            if (part == 1)
            {
                foreach (string[] arr in inputList)
                {
                    // get hands
                    string myHand = arr[1];
                    string elfHand = arr[0];

                    if (myHand == "X") total += 1;
                    if (myHand == "Y") total += 2;
                    if (myHand == "Z") total += 3;

                    // win/loss/draw?
                    if (elfHand == "A")
                    {
                        if (myHand == "X") total += 3;
                        if (myHand == "Y") total += 6;
                        if (myHand == "Z") total += 0;
                    }
                    if (elfHand == "B")
                    {
                        if (myHand == "X") total += 0;
                        if (myHand == "Y") total += 3;
                        if (myHand == "Z") total += 6;
                    }
                    if (elfHand == "C")
                    {
                        if (myHand == "X") total += 6;
                        if (myHand == "Y") total += 0;
                        if (myHand == "Z") total += 3;
                    }

                }

                return total.ToString();
            }

            // part 2
            foreach (string[] arr in inputList)
            {
                // get hands
                string myHand = arr[1];
                string elfHand = arr[0];

                // win/loss/draw
                if (myHand == "X") total += 0;
                if (myHand == "Y") total += 3;
                if (myHand == "Z") total += 6;

                // determine what myHand needs to be
                if (elfHand == "A")
                {
                    if (myHand == "X") total += 3; //need to lose against rock: pick scissors (+3)
                    if (myHand == "Y") total += 1; //need to draw against rock: pick rock (+1)
                    if (myHand == "Z") total += 2; //need to win against rock: pick paper (+2)
                }
                if (elfHand == "B")
                {
                    if (myHand == "X") total += 1; //need to lose against paper: pick rock (+1)
                    if (myHand == "Y") total += 2; //need to draw against paper: pick paper (+2)
                    if (myHand == "Z") total += 3; //need to win against paper: pick scissors (+3)
                }
                if (elfHand == "C")
                {
                    if (myHand == "X") total += 2; //need to lose against scissors: pick paper (+2)
                    if (myHand == "Y") total += 3; //need to draw against scissors: pick scissors (+3)
                    if (myHand == "Z") total += 1; //need to win against scissors: pick rock (+1)
                }

            }

            return total.ToString();
        }

        // Day 2: https://adventofcode.com/2022/day/2
        // This is RPS() rewritten to utilize a Circular LinkedList
        public static string RPS2(int part = 1)
        {
            // FileStream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem2.txt";
            List<string[]> inputList = ProblemSolver.RPS(filePath);
            int total = 0;

            // LinkedList setup
            LinkedList<string> ll = new LinkedList<string>(new [] {"R", "S", "P"});

            if (part == 1)
            {
                foreach (string[] arr in inputList)
                {
                    string myHand = arr[1];
                    string elfHand = arr[0];

                    // Convert both hands to RPS and add totals for myHand value.
                    RPS2Helper(ref myHand, ref total);
                    RPS2Helper(ref elfHand, ref total);

                    LinkedListNode<string> myHandNode = ll.Find(myHand);
                    
                    // win
                    if (myHandNode.NextOrFirst().Value == elfHand)
                    {
                        total += 6;
                    }
                    // draw
                    if (myHandNode.Value == elfHand)
                    {
                        total += 3;
                    }
                    // loss
                    if (myHandNode.PreviousOrLast().Value == elfHand)
                    {
                        total += 0;
                    }
                }

                return total.ToString();
            }

            // part 2
            foreach (string[] arr in inputList)
            {
                string myHand = arr[1]; // now indicates win/loss/draw instead of XYZ->RPS
                string elfHand = arr[0];

                // Convert given hands to RPS.
                RPS2Helper(ref elfHand, ref total);

                LinkedListNode<string> elfHandNode = ll.Find(elfHand);
                
                // lose
                if (myHand == "X")
                {
                    myHand = elfHandNode.NextOrFirst().Value;
                    total += 0;
                }
                // draw
                else if (myHand == "Y")
                {
                    myHand = elfHandNode.Value;
                    total += 3;
                }
                // win
                else if (myHand == "Z")
                {
                    myHand = elfHandNode.PreviousOrLast().Value;
                    total += 6;
                }

                // add point values for selection
                if (myHand == "R")
                {
                    total += 1;
                }
                if (myHand == "P")
                {
                    total += 2;
                }
                if (myHand == "S")
                {
                    total += 3;
                }
            }

            return total.ToString();
        }

        // Convert hands from (ABC or XYZ) to RPS
        private static void RPS2Helper(ref string hand, ref int total)
        {
            // get myHand and convert XYZ to RPS for win-check
            if (hand == "X") 
            {
                hand = "R";
                total += 1;
            }
            else if (hand == "Y") 
            {
                hand = "P";
                total += 2;
            }
            else if (hand == "Z") 
            {
                hand = "S";
                total += 3;
            }

            // get elfHand and convert ABC to RPS for win-check
            if (hand == "A") 
            {
                hand = "R";
            }
            else if (hand == "B") 
            {
                hand = "P";
            }
            else if (hand == "C") 
            {
                hand = "S";
            }
        }

        // Day 3: https://adventofcode.com/2022/day/3
        public static string RucksackReorg(int part = 1)
        {
            // FileStream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem3.txt";
            List<int> inputList = new List<int>();
            int sum = 0;

            if (part == 1)
            {
                inputList = ProblemSolver.RucksackReorg(filePath);
                sum = inputList.Sum();
                return sum.ToString();
            }

            // part 2
            inputList = ProblemSolver.RucksackReorgPart2(filePath);
            sum = inputList.Sum();
            return sum.ToString();
        }

        // Day 4: https://adventofcode.com/2022/day/4
        public static string CampCleanup(int part = 1)
        {
            // FileStream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem4.txt";

            return ProblemSolver.CampCleanup(filePath, part);
        }

        // Day 5: https://adventofcode.com/2022/day/5
        public static string SupplyStacks(int part = 1)
        {
            // Filestream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem5.txt";
            Dictionary<int, Stack<char>> supply = ProblemSolver.SupplyStacksSetup(filePath);

            if (part == 1)
            {
                return ProblemSolver.SupplyStacks(filePath, supply);
            }

            // part 2
            return ProblemSolver.SupplyStacks(filePath, supply, 2);
        }

        // Day 6: https://adventofcode.com/2022/day/6
        public static string TuningTrouble(int part = 1)
        {
            // Filestream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem6.txt";

            if (part == 1)
            {
                return ProblemSolver.TuningTrouble(filePath);
            }

            // part 2
            return ProblemSolver.TuningTrouble(filePath, 2);
        }

        // Day 7: httpss://adventofcode.com/2022/day/7
        public static string NoSpaceLeftOnDevice(int part = 1)
        {
            // Filestream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem7test.txt";

            if (part == 1)
            {
                return ProblemSolver.NoSpaceLeftOnDevice(filePath);
            }

            // part 2
            return ProblemSolver.NoSpaceLeftOnDevice(filePath, 2);
        }

        // Day 8: httpss://adventofcode.com/2022/day/8
        public static string TreetopTreeHouse(int part = 1)
        {
            // Filestream setup
            string filePath = Environment.CurrentDirectory + @"\inputs\problem8.txt";

            if (part == 1)
            {
                return ProblemSolver.TreetopTreeHouse(filePath);
            }

            // part 2
            return ProblemSolver.TreetopTreeHouse(filePath, 2);
        }
    }
}