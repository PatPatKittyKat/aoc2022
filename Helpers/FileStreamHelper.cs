using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public static class FileStreamHelper
    {
        public static Dictionary<int, int> ReadFileIntoDictionary(string filePath)
        {
            // Individualized
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

            return elfCollection;
        }

        public static List<string[]> ReadFileIntoStringArrayList(string filePath)
        {
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

            return inputList;
        }

        public static List<int> ReadFileProblem3(string filePath)
        {
            List<int> resultList = new List<int>();

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
                            //inputList.Add(line);
                            Dictionary<char, int> letterDict = new Dictionary<char, int>();
                            string s1 = line.Substring(0,line.Length/2);
                            string s2 = line.Substring( (line.Length/2) , (line.Length/2) );

                            foreach (char c in s1)
                            {
                                // for s1, just add to dict
                                if (!letterDict.TryGetValue(c, out int x))
                                {
                                    letterDict.Add(c, 0);
                                }
                            }
                            foreach (char c in s2)
                            {
                                // for s2, check for any matches from s1
                                if (letterDict.TryGetValue(c, out int x))
                                {
                                    int value = (char.IsUpper(c)) ? (int)c-38 : (int)c-96;
                                    resultList.Add(value);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return resultList;
        }

        public static List<int> ReadFileProblem3Part2(string filePath)
        {
            List<int> resultList = new List<int>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[16384];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        int lineCounter = 0;
                        Dictionary<char, int> letterDict = new Dictionary<char, int>();
                        List<char> charList = new List<char>();

                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            
                            
                            charList = new List<char>();
                            foreach (char c in line)
                            {
                                if (!charList.Contains(c))
                                {
                                    charList.Add(c);
                                    if (letterDict.TryGetValue(c, out int x))
                                    {
                                        letterDict[c] += 1;
                                    }
                                    else
                                    {
                                        letterDict.Add(c, 1);
                                    }

                                }
                                // if (lineCounter == 0 && !letterDict.TryGetValue(c, out int x))
                                // {
                                //     letterDict.Add(c, 1);
                                // }

                                // if (lineCounter > 0 && letterDict.TryGetValue(c, out int y))
                                // {
                                //     letterDict[c] += 1;
                                // }
                            }
                            lineCounter++;

                            // new group
                            if (lineCounter == 3)
                            {
                                char c = letterDict.Where(x => x.Value == 3).FirstOrDefault().Key;
                                int value = (char.IsUpper(c)) ? (int)c-38 : (int)c-96;
                                resultList.Add(value);

                                lineCounter = 0;
                                letterDict = new Dictionary<char, int>();
                            }
                            
                        }
                    }
                }
            }

            return resultList;
        }
    }
}