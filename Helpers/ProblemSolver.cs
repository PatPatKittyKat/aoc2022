﻿using System.Collections;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    public static class ProblemSolver
    {
        const int FILE_BUFFER_SIZE = 16384; // 2^14

        // Day 1
        public static Dictionary<int, int> CalorieCounting(string filePath)
        {
            // Individualized
            Dictionary<int,int> elfCollection = new Dictionary<int, int>(); //<id, sum>
            int key = 0;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
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

        // Day 2
        public static List<string[]> RPS(string filePath)
        {
            List<string[]> inputList = new List<string[]>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
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

        // Day 3
        public static List<int> RucksackReorg(string filePath)
        {
            List<int> resultList = new List<int>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Dictionary<char, int> letterDict = new Dictionary<char, int>();
                            string s1 = line.Substring(0, line.Length/2);
                            string s2 = line.Substring((line.Length/2), (line.Length/2));

                            foreach (char c in s1)
                            {
                                // for s1, just add unique chars to dict
                                if (!letterDict.TryGetValue(c, out int x))
                                {
                                    letterDict.Add(c, 0);
                                }
                            }
                            foreach (char c in s2)
                            {
                                // for s2, just check for any matches from s1 dict
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

        public static List<int> RucksackReorgPart2(string filePath)
        {
            List<int> resultList = new List<int>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
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
                            }
                            lineCounter++;

                            // Indicates new group (find the single char that exists in all 3 lines in the group, document it, then reset all objects)
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

        // Day 4
        public static string CampCleanup(string filePath, int part = 1)
        {
            int pairCount = 0;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            int separatorIndex = line.IndexOf(',');
                            string[] s1 = line.Substring(0, separatorIndex).Split('-');
                            string[] s2 = line.Substring(separatorIndex+1, line.Length-separatorIndex-1).Split('-');
                            
                            // Get lower and upper bounds of given ranges for each group
                            Tuple<int, int> assignment1 = new Tuple<int, int>(Convert.ToInt32(s1[0]),Convert.ToInt32(s1[1])); // [lower1, upper1]
                            Tuple<int, int> assignment2 = new Tuple<int, int>(Convert.ToInt32(s2[0]),Convert.ToInt32(s2[1])); // [lower2, upper2]

                            if (part == 1)
                            {
                                // Check if one range is contained in the other
                                if ((assignment1.Item1 <= assignment2.Item1 && assignment1.Item2 >= assignment2.Item2) ||
                                    (assignment1.Item1 >= assignment2.Item1 && assignment1.Item2 <= assignment2.Item2))
                                {
                                    pairCount++;
                                }
                            }
                            
                            else if (part == 2)
                            {
                                // Check for *any* overlapping
                                if (assignment1.Item2 == assignment2.Item1 
                                    || assignment1.Item1 == assignment2.Item2
                                    || (assignment1.Item2 >= assignment2.Item1 && assignment1.Item1 <= assignment2.Item1)
                                    || (assignment2.Item2 >= assignment1.Item1 && assignment2.Item1 <= assignment1.Item1))
                                {
                                    pairCount++;
                                }
                            }
                        }
                    }
                }
            }

            return pairCount.ToString();
        }


        // Day 5
        // Create initial setup structure first (starting arrangement of crates)
        public static Dictionary<int, Stack<char>> SupplyStacksSetup(string filePath)
        {
            Dictionary<int, Stack<char>> supply = new Dictionary<int, Stack<char>>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;

                        // Only read file until the line-break (reading only the initial setup, not any directions)
                        while ((line = reader.ReadLine()) != null && line != "")
                        {
                            for (int i = 0; i < line.Length; i++)
                            {
                                char c = line[i];
                                int slot = 0;

                                // Excluding the first slot, ((index-1)/4 plus 1) indicates the slot number each character is assigned into.
                                // Utilizing char.IsLetter() prevents the bottom numeric labels or space characters from interfering with our input.
                                if (i == 1)
                                {
                                    if (char.IsLetter(c)) 
                                    {
                                        if (supply.TryGetValue(1, out Stack<char> stack))
                                        {
                                            supply[1].Push(c);
                                        }
                                        else
                                        {
                                            Stack<char> newStack = new Stack<char>();
                                            newStack.Push(c);
                                            supply.Add(1, newStack);
                                        }
                                    }
                                }
                                else if (i > 1 && (i-1) % 4 == 0)
                                {
                                    slot = ((i-1)/4) + 1;

                                    if (char.IsLetter(c)) 
                                    {
                                        if (supply.TryGetValue(slot, out Stack<char> stack))
                                        {
                                            supply[slot].Push(c);
                                        }
                                        else
                                        {
                                            Stack<char> newStack = new Stack<char>();
                                            newStack.Push(c);
                                            supply.Add(slot, newStack);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Because when read from the file top-down, we're reading the most-recently inserted elements of each stack first. Need to reverse it.
            ReverseStackOrders(supply);
            return supply;
        }

        // When calling Stack<T>(IEnumerable<T>) constructor, we are calling .Push() into the stack with IEnumerable order intact (so the new stack is reversed).
        public static void ReverseStackOrders(Dictionary<int, Stack<char>> supply)
        {
            foreach (KeyValuePair<int, Stack<char>> kvp in supply)
            {
                Stack<char> reversedStack = new Stack<char>(kvp.Value);
                supply[kvp.Key] = reversedStack;
            }
        }

        // Execute rearrangements and return top of final supply stacks
        public static string SupplyStacks(string filePath, Dictionary<int, Stack<char>> supply, int part = 1)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;

                        // Only read movements commands from file: we've already read the initial setup part at this point.
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.StartsWith("move"))
                            {
                                string[] splitString = line.Split(' ');

                                // Tuple<amountToMove, fromSlot, toSlot)
                                Tuple<int, int, int> movements = Tuple.Create(Convert.ToInt32(splitString[1]),Convert.ToInt32(splitString[3]),Convert.ToInt32(splitString[5]));

                                if (part == 1)
                                {
                                    for (int i = 1; i <= movements.Item1; i++)
                                    {
                                        char c = supply[movements.Item2].Pop();
                                        supply[movements.Item3].Push(c);
                                    }
                                }
                                
                                else if (part == 2)
                                {
                                    Stack<char> tempStack = new Stack<char>();
                                    for (int i = 1; i <= movements.Item1; i++)
                                    {
                                        char c = supply[movements.Item2].Pop();
                                        tempStack.Push(c);
                                    }
                                    foreach (char c in tempStack)
                                    {
                                        supply[movements.Item3].Push(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Peek all stacks in dict. Using explicit index because the dictionary KeyValuePairs are not guaranteed order when initially inserted.
            string result = string.Empty;
            for (int i = 1; i <= supply.Keys.Count; i++)
            {
                result += supply[i].Peek();
            }

            return result;
        }

        // Day 6
        public static string TuningTrouble(string filePath, int part = 1)
        {
            int packetSize = 0;
            if (part == 1)
            {
                packetSize = 4;
            }
            else if (part == 2)
            {
                packetSize = 14;
            }

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line = reader.ReadLine();
                        
                        Queue<char> queue = new Queue<char>(4);
                        for (int i = 0; i < line.Length; i++)
                        {
                            char c = line[i];

                            if (queue.Count < packetSize)
                            {
                                queue.Enqueue(c);
                            }
                            else
                            {
                                queue.Dequeue();
                                queue.Enqueue(c);
                            }

                            // Check uniqueness
                            if (queue.Distinct().Count() == packetSize)
                            {
                                return Convert.ToString(i+1);
                            }
                        }
                    }
                }
            }
            return "";
        }

        // Day 7
        public static string NoSpaceLeftOnDevice(string filePath, int part = 1)
        {
            // // Set-up file system with root.
            // Dictionary<string, FileSystemObject> fileSystem = new Dictionary<string, FileSystemObject>();
            // FileSystemObject root = new FileSystemObject()
            // {
            //     Parent = string.Empty,
            //     Name = "/",
            //     Children = new List<FileSystemObject>(),
            //     Filesize = 0
            // };
            // fileSystem.Add("/", root);

            // string currDirName = root.Name;

            // // Reading filestream
            // using (FileStream fs = File.OpenRead(filePath))
            // {
            //     byte[] b = new byte[FILE_BUFFER_SIZE];
            //     int readLen;

            //     while ((readLen = fs.Read(b, 0, b.Length)) > 0)
            //     {
            //         string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
            //         using (StringReader reader = new StringReader(bufferString))
            //         {
            //             string? line;
            //             while ((line = reader.ReadLine()) != null)
            //             {
            //                 string[] splitString = line.Split(' ');

            //                 // If line does not begin with '$', we are listing out directory contents
            //                 if (line.StartsWith('$'))
            //                 {

            //                     if (splitString[1] == "cd")
            //                     {
            //                         // "$ cd /" => go back to root
            //                         if (splitString[2] == "/")
            //                         {
            //                             currDirName = root.Name;
            //                         }

            //                         // "$ cd .." => go back to Parent node
            //                         else if (splitString[2] == "..")
            //                         {
            //                             currDirName = fileSystem[currDirName].Parent;
            //                         }

            //                         // "$ cd dirName" => navigate into child Directory
            //                         else
            //                         {
            //                             currDirName = splitString[2];
            //                         }
            //                     }

            //                     // "$ ls" => ignore, since we know lines not beginning with "$" will be the CurrentDirectory contents.
            //                     else 
            //                     {
            //                         // do nothing
            //                     }
            //                 }
            //                 else
            //                 {
            //                     // Create Directory (if doesn't already exist)
            //                     if (line.StartsWith("dir"))
            //                     {
            //                         if (!fileSystem.TryGetValue(splitString[1], out FileSystemObject fso))
            //                         {
            //                             FileSystemObject newDir = new FileSystemObject()
            //                             {
            //                                 Parent = currDirName,
            //                                 Name = splitString[1],
            //                                 Children = new List<FileSystemObject>(),
            //                                 Filesize = 0
            //                             };

            //                             fileSystem[currDirName].Children.Add(newDir);
            //                         }
            //                     }

            //                     // Add File to Current Directory (if it doesn't already exist)
            //                     else
            //                     {
            //                         if (!fileSystem.TryGetValue(splitString[1], out FileSystemObject fso))
            //                         {
            //                             FileSystemObject newFile = new FileSystemObject()
            //                             {
            //                                 Parent = currDirName,
            //                                 Name = splitString[1],
            //                                 Children = new List<FileSystemObject>(),
            //                                 Filesize = Convert.ToInt32(splitString[0])
            //                             };

            //                             fileSystem[currDirName].Children.Add(newFile);

            //                             // Increase Filesize for all Parents up to the root
            //                             string iterator = currDirName;
            //                             while (!string.IsNullOrEmpty(fileSystem[iterator].Parent))
            //                             {
            //                                 int fileSize = fileSystem[iterator].Filesize;
            //                                 iterator = fileSystem[iterator].Parent;
            //                                 fileSystem[iterator].Filesize = fileSize;
            //                             }
            //                         }
                                    
            //                     }

            //                 }
            //             }
            //         }
            //     }
            // }

            return "";
        }

        // Day 8
        public static string TreetopTreeHouse(string filePath, int part = 1)
        {
            // Get # rows and columns
            int numRows = 0;
            int numCols = 0;
            int result = 0;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        bool isFirstLine = true;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (isFirstLine)
                            {
                                numCols = line.Length;
                                isFirstLine = false;
                            }

                            numRows += 1;
                        }
                    }
                }
            }
            
            // 2D Array
            int[,] array = new int[numRows, numCols];
            int rowCounter = 0;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            for (int i = 0; i < line.Length; i++)
                            {
                                int height = (int)Char.GetNumericValue(line[i]);
                                array[rowCounter,i] = height;
                            }
                            rowCounter++;
                        }
                    }
                }
            }

            int numVisibleAtEdge = (numRows-1)*2 + (numCols-1)*2;
            int bestScenicScore = 0;
            int currX, currY;

            // Check whether a coordinate can reach an edge without encountering any others along the X or Y axis that is >= its value.
            for (int i = 1; i < numRows-1; i++)
            {
                for (int j = 1; j < numCols-1; j++)
                {
                    currX = 0;
                    currY = 0;

                    // Part 1 and Part 2 solutions
                    if (part == 1)
                    {
                        bool isVisible = true;

                        // Check +Y
                        currX = i-1;
                        while (currX != -1)
                        {
                            if (array[currX,j] >= array[i,j])
                            {
                                isVisible = false;
                            }
                            currX--;
                        }
                        if (isVisible)
                        {
                            result++;
                            continue;
                        }

                        // Check -Y
                        isVisible = true;
                        currX = i+1;
                        while (currX != numCols)
                        {
                            if (array[currX,j] >= array[i,j])
                            {
                                isVisible = false;
                            }
                            currX++;
                        }
                        if (isVisible)
                        {
                            result++;
                            continue;
                        }

                        // Check -X
                        isVisible = true;
                        currY = j-1;
                        while (currY != -1)
                        {
                            if (array[i,currY] >= array[i,j])
                            {
                                isVisible = false;
                            }
                            currY--;
                        }
                        if (isVisible)
                        {
                            result++;
                            continue;
                        }

                        // Check +X
                        isVisible = true;
                        currY = j+1;
                        while (currY != numRows)
                        {
                            if (array[i,currY] >= array[i,j])
                            {
                                isVisible = false;
                            }
                            currY++;
                        }
                        if (isVisible)
                        {
                            result++;
                            continue;
                        }
                    }
                    else if (part == 2)
                    {
                        int currScenicScore = 1;
                        int currViewingDistance = 0;
                        currX = 0;
                        currY = 0;

                        // Check +Y
                        currX = i-1;
                        while (currX != -1)
                        {
                            if (array[currX,j] < array[i,j])
                            {
                                currViewingDistance++;
                            }
                            else
                            {
                                currViewingDistance++;
                                break;
                            }
                            currX--;
                        }
                        currScenicScore*=currViewingDistance;

                        // Check -Y
                        currViewingDistance = 0;
                        currX = i+1;
                        while (currX != numCols)
                        {
                            if (array[currX,j] < array[i,j])
                            {
                                currViewingDistance++;
                            }
                            else
                            {
                                currViewingDistance++;
                                break;
                            }
                            currX++;
                        }
                        currScenicScore*=currViewingDistance;

                        // Check -X
                        currViewingDistance = 0;
                        currY = j-1;
                        while (currY != -1)
                        {
                            if (array[i,currY] < array[i,j])
                            {
                                currViewingDistance++;
                            }
                            else
                            {
                                currViewingDistance++;
                                break;
                            }
                            currY--;
                        }
                        currScenicScore*=currViewingDistance;

                        // Check +X
                        currViewingDistance = 0;
                        currY = j+1;
                        while (currY != numRows)
                        {
                            if (array[i,currY] < array[i,j])
                            {
                                currViewingDistance++;
                            }
                            else
                            {
                                currViewingDistance++;
                                break;
                            }
                            currY++;
                        }
                        currScenicScore*=currViewingDistance;
                        
                        // Check against current highest scenic score
                        if (currScenicScore > bestScenicScore)
                        {
                            bestScenicScore = currScenicScore;
                        }
                    }
                }
            }
            
            // Return answers
            if (part == 1)
            {
                return Convert.ToString(result + numVisibleAtEdge);
            }
            else if (part == 2)
            {
                return Convert.ToString(bestScenicScore);
            }
            
            return Convert.ToString("");
        }

        // Day 9
        public static string RopeBridge(string filePath, int part = 1)
        {
            // Get # rows and columns
            int minX,maxX,minY,maxY,currX,currY,result;
            minX = maxX = minY = maxY = currX = currY = result = 0;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Starting at (0,0), we can find our required array size by tracking the extremes for X and Y values.
                            string[] inputLine = line.Split(' ');
                            char direction = Convert.ToChar(inputLine[0]);
                            int numSteps = Convert.ToInt32(inputLine[1]);

                            switch (direction)
                            {
                                case 'R':
                                    currX += numSteps;
                                    if (currX > maxX) maxX = currX;
                                    break;
                                case 'L':
                                    currX -= numSteps;
                                    if (currX < minX) minX = currX;
                                    break;
                                case 'U':
                                    currY += numSteps;
                                    if (currY > maxY) maxY = currY;
                                    break;
                                case 'D':
                                    currY -= numSteps;
                                    if (currY < minY) minY = currY;
                                    break;
                            };
                        }
                    }
                }
            }
            
            // 2D Array: Y represents which column we're on (left-right), X represents which row we're on (up-down)
            int[,] array = new int[maxY-minY+1, maxX-minX+1];
            Dictionary<Tuple<int,int>, bool> model = new Dictionary<Tuple<int,int>, bool>(); // Key: Coordinate, Value: isVisited
            currX = currY = 0;
            RopeBridgeHelper.BuildModel(array, ref model);

            //List<RopeBridgeModel> model2 = RopeBridgeHelper.BuildModel2(array, minX, maxX, minY, maxY);

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] inputLine = line.Split(' ');
                            char direction = Convert.ToChar(inputLine[0]);
                            int numSteps = Convert.ToInt32(inputLine[1]);

                            switch (direction)
                            {
                                case 'R':
                                    currX += numSteps;
                                    break;
                                case 'L':
                                    currX -= numSteps;
                                    break;
                                case 'U':
                                    currY += numSteps;
                                    break;
                                case 'D':
                                    currY -= numSteps;
                                    break;
                                
                            };

                            Tuple<int,int> currentCoordinate = new Tuple<int,int>(currY,currX);
                            RopeBridgeHelper.FlagVisitedCoordinate(currentCoordinate, ref model);
                        }
                    }
                }
            }

            result = model.Count(x => x.Value);
            return Convert.ToString(result);
        }

        // Day 10
        public static string CathodeRayTube(string filePath, int part = 1)
        {
            int x = 1;
            int sum = 0;
            string part2Result = string.Empty;

            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] b = new byte[FILE_BUFFER_SIZE];
                int readLen;

                while ((readLen = fs.Read(b, 0, b.Length)) > 0)
                {
                    string bufferString = System.Text.Encoding.Default.GetString(b, 0, readLen);
                    using (StringReader reader = new StringReader(bufferString))
                    {
                        string? line;
                        int cycleIteration = 1;
                        Queue<int> queue = new Queue<int>();
                        List<bool> pixelList = new List<bool>();
                        
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] splitInstruction = line.Split(' ');
                            string op = line.Split(' ')[0];

                            if (part == 1)
                            {
                                //Console.WriteLine("\nCycle iteration begin: " + cycleIteration + " | X value: " + x);

                                if ((cycleIteration + 20) % 40 == 0)
                                {
                                    sum += x*cycleIteration;
                                }

                                if (op == "addx")
                                {
                                    int instructionValue = Convert.ToInt32(splitInstruction[1]);
                                    queue.Enqueue(instructionValue);
                                    //Console.WriteLine("\nCycle iteration during: " + cycleIteration + " | X value: " + x);
                                    cycleIteration++;

                                    if ((cycleIteration + 20) % 40 == 0)
                                    {
                                        sum += x*cycleIteration;
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine("\nCycle iteration during: " + cycleIteration + " | X value: " + x);
                                }
                                
                                if (queue.TryDequeue(out int dequeueValue))
                                {
                                    x += dequeueValue;
                                }

                                //Console.WriteLine("\nCycle iteration end: " + cycleIteration + " | X value: " + x);                            
                                cycleIteration++;
                            }
                            else
                            {
                                // For part 2, row 3, 5, and 6 misaligned. Otherwise solid result.

                                if (x >= cycleIteration-2 && x <= cycleIteration )
                                {
                                    pixelList.Add(true);
                                }
                                else
                                {
                                    pixelList.Add(false);
                                }

                                if (cycleIteration % 40 == 0)
                                {
                                    string row = string.Empty;
                                    foreach (bool p in pixelList)
                                    {
                                        char c = p ? '#' : '.';
                                        row += c;
                                    }
                                    part2Result += row + '\n';
                                    pixelList = new List<bool>();
                                    cycleIteration = 0;
                                }

                                if (op == "addx")
                                {
                                    int instructionValue = Convert.ToInt32(splitInstruction[1]);
                                    queue.Enqueue(instructionValue);
                                    cycleIteration++;

                                    if (cycleIteration % 40 == 0)
                                    {
                                        if (x >= cycleIteration-2 && x <= cycleIteration )
                                        {
                                            pixelList.Add(true);
                                        }
                                        else
                                        {
                                            pixelList.Add(false);
                                        }

                                        string row = string.Empty;
                                        foreach (bool p in pixelList)
                                        {
                                            char c = p ? '#' : '.';
                                            row += c;
                                        }
                                        part2Result += row + '\n';
                                        pixelList = new List<bool>();
                                        cycleIteration = 0;
                                    }

                                    if (x >= cycleIteration-2 && x <= cycleIteration )
                                    {
                                        pixelList.Add(true);
                                    }
                                    else
                                    {
                                        pixelList.Add(false);
                                    }
                                }
                                if (queue.TryDequeue(out int dequeueValue))
                                {
                                    x += dequeueValue;
                                }

                                cycleIteration++;
                            }
                        }
                    }
                }
            }
            return (part == 1) ? sum.ToString() : part2Result;
        }

    }
}