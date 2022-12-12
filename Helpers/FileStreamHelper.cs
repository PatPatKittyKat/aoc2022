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
    }
}