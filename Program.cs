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
            //List of all problems (25) can be found here: https://adventofcode.com/2022
            const string introMsg = "Hello :)"
                            +"\nHaven't heard too much about AoC until recently, and I figured I could use"
                            +"\na refresher on setting up new projects from scratch, new source control, etc and working on some"
                            +"\nsimpler problems without any pressure to finish before some deadline. Happy reading.";
            const string separator = " | ";
            string problemInfo,answer;
            
            //Problem(x) - CHANGE THIS TO SOLVE DIFFERENT PROBELMS.
            int problemNumber = 1;

            problemInfo = ProgramHelper.BuildProblemMessage(problemNumber);
            answer = SolveProblem(problemNumber);
            
            Console.WriteLine(introMsg+'\n');
            Console.WriteLine(problemInfo + separator + "Answer: " + answer);
        }

        //maybe add another ver to show multiple answers at once.
        public static string SolveProblem(int problem)
        {
            string result;

            switch (problem)
            {
                case 1:
                    result = CalorieCounting();
                    break;
                default:
                    result = "??";
                    break;
            };

            return result;
        }


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