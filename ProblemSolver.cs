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
                default:
                    result = "??";
                    break;
            };

            return problemInfo + separator + "Answer: " + result;
        }
        
        //TODO: could use a more intuitive way to indicate part2. Currently need to just call CalorieCounting(true)
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
                        string line;
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

    }
}