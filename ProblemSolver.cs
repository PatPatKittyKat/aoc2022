using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public class ProblemSolver
    {
        const string separator = " | ";

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
                    break;
                case 2:
                    result = RPS();
                    break;
                default:
                    result = "??";
                    break;
            };

            return problemInfo + separator + "Answer: " + result;
        }
        
        //TODO: could use a more intuitive way to indicate part2. Currently need to just call CalorieCounting(true)
        //TODO: put the filestream logic in a separate centralized helper so it's not reused in every solution.
        //Day 1: https://adventofcode.com/2022/day/1
        public static string CalorieCounting(bool part2 = false)
        {
            string filePath = Environment.CurrentDirectory + @"\inputs\problem1.txt";
            Dictionary<int,int> elfCollection = new Dictionary<int, int>(); //<id, sum>
            int key = 0;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[16384];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (!elfCollection.TryGetValue(key, out int value))
                                {
                                    elfCollection.Add(key, Convert.ToInt32(line));
                                }
                                else
                                {
                                    elfCollection[key] += Convert.ToInt32(line);
                                }
                            }
                            else
                            {
                                key++;
                            }
                        }
                    }
                }
            }

            if (!part2)
            {
                // base question: elf carrying most calories
                int answer = elfCollection.OrderByDescending(x => x.Value).FirstOrDefault().Value;
                return Convert.ToString(answer);
            }
            else
            {
                // part 2: get top 3 instead
                //var collection = elfCollection.OrderByDescending(x => x.Value).Take(3).ToDictionary(x => x.Key, y => y.Value);
                int answer2 = elfCollection.OrderByDescending(x => x.Value).Take(3).Sum(x => x.Value);
                return Convert.ToString(answer2);
            }
        }

        //Day 2: https://adventofcode.com/2022/day/2
        public static string RPS(bool part2 = false)
        {
            //col1: A(rock),B(paper),C(scissors)
            //col2: X(rock),Y(paper),Z(scissors)
            // A Y (win, gain 2 for paper + 6 for win = 8)
            // B X (lose, gain 1 for rock + 0 for loss = 1)
            // C Z (draw, gain 3 for scissors + 3 for draw = 6)

            string filePath = Environment.CurrentDirectory + @"\inputs\problem2.txt";
            List<string[]> inputList = new List<string[]>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[16384];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            inputList.Add(line.Split(' '));
                        }
                    }
                }
            }


            if (!part2)
            {
                // Calculating score.
                int total = 0;
                foreach (string[] arr in inputList)
                {
                    //get hands
                    string myHand = arr[1];
                    string elfHand = arr[0];

                    if (myHand == "X") total += 1;
                    if (myHand == "Y") total += 2;
                    if (myHand == "Z") total += 3;

                    //win/loss/draw?
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
            else
            {
                // Calculating score.
                int total = 0;
                foreach (string[] arr in inputList)
                {
                    //get hands
                    string myHand = arr[1];
                    string elfHand = arr[0];

                    //win/loss/draw
                    if (myHand == "X") total += 0;
                    if (myHand == "Y") total += 3;
                    if (myHand == "Z") total += 6;

                    //find myHand
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
            
        }

    }
}