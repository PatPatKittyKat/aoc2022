Hello :)
Haven't heard too much about AoC until recently, and I figured I could use
a refresher on setting up new projects from scratch, new source control, etc and working on some
simpler problems without any pressure to finish before some deadline. Happy reading.

[//]: # "diagram editor: https://www.diagrameditor.com/"

## Day 1: Calorie Counting
The approach here was to simply read the inputs one line at a time, adding each number into a running total. Once a newline is detected, we increment a key and add a new KeyValuePair into a dictionary. Until we reach the next newline, we just add into the current kvp. Then, to get the problem's answer, we can utilize built-in LINQ capabilities to grab what we need from the resulting collection (order by descending, then grab top X results).

## Day 2: Rock Paper Scissors
For this one, the initial approach was just a brute force answer: writing out a bunch of if-statements for every scenario. It got the correct answers, but the result didn't look very readable, so I brainstormed a slightly different approach using a Circular Linked List. 

[//]: # "original img can be found in \images\rpsLL.png -- ![rpsLL](images\rpsLL.png)"
![rpsLL](https://user-images.githubusercontent.com/22353608/207114173-48d151b4-b3a2-4edd-950b-de25ba550ca2.png)

From the above, we can check win/loss based on position in the linked list, where:
- `currentNode` is the selected hand
- `currentNode.Next` is what currentNode beats
- `currentNode.Previous` is what currentNode loses to

For example, let's say your selected Hand is Scissors ("S"), and my Hand is Paper ("P"). Your `currentNode.Value` is "S", and your `currentNode.Next.Value` is "P", so it is determined that you won that round.

Another example, your selected Hand is "R", mine is "P". Your `currentNode.Prev.Value` is "P", so you lost that one.

## Day 3: Rucksack Reorganization
This one was solved by the following: on each line read from the filestream, split the string in half. For the first half, add all unique characters to a dictionary. For the second half, just check against the dictionary for existence. When a duplicate is found, calculate its value and add to a running total, then reset the dictionary.

The part 2 solution is similar, but with a twist. For each new group, we are looking at 3 lines from the filestream, not just 1 line. Like part 1, we only care about unique characters. So, every time we encounter a duplicated character on different lines in the group, we add to the `dict.Value`, which stores the frequency it appears (max 3 = adds +1 if char appears once or more per line). From the problem definition, we are guaranteed to only have one answer per group, so we can just calculate its value and add to a running total. Then reset the dictionary to reuse for the next group.

## Day 4: Camp Cleanup
The problem here is to determine overlapping ranges. It may be normal to keep a log of every covered section, and then try a .Contains on that structure, checking for matches on any. But this approach will solve it with only the given lower and upper bounds.

Essentially, if `group1.lower` < `group2.lower` AND `group1.upper` > `group2.upper`, then group2 is fully contained by group1. 

For part 2, we can still solve by only using the lower/upper ranges. Just need to consider additional more scenarios.

![campcleanup](https://user-images.githubusercontent.com/22353608/208060915-23d7107c-2d24-4d70-8e3c-b6b4c8fb75fc.png)

## Day 5: Supply Stacks
This problem can be solved with a dictionary of stacks. Reading the input file provides its own challenge though. Because the input is less-well-structured than we've seen so far, and when read top-down from the filestream, the stacks will be reversed. The slot which each char will stack into can be represented programmatically, so we can use that to our advantage and start building our data structure without knowing the # of columns in advance. Example of reading one input line:

![supplystackinput2](https://user-images.githubusercontent.com/22353608/208269388-d26fddc2-ed54-411b-a2ba-4472a36d26d0.png)

We'll use `Dictionary<int, Stack<char>>` to keep track of the supply, and use that to execute rearrangement commands. Once that's done, we can just iterate through each part of the dictionary calling `Stack.Peek()` to return what we need.

For Part 2, we only need to modify the actual `.Pop()` and `.Push()` actions from Part 1. If we store popped values into a tempStack, and then push from the tempStack instead of directly moving each one (as in Part 1), then we can move multiple values at once while keeping their initial ordering.

## Day 6: Tuning Trouble
One approach to solve this is to keep a running queue of size 4 which updates for each character read from the given input. After every insert, we can check for uniqueness with `queue.Distinct().Count()` and if that value is 4 (indicating every value in the queue of size 4 is unique), then we've got our answer. The index + 1.

For Part 2, our solution is already there, we just need to change the expected queue size from 4 to 16.

## Day 7: No Space Left On Device
Tough problem. For file systems, a decent representation can be built out with a nonbinary Tree data structure, but AFAIK, these do not inherently exist within C# because specific implementation is largely a case-by-case basis. So we will have to build our own here. From the problem, it looks like we will need the following:
- We need to be able to navigate into and out of each Node (including skip back to root)
- When `ls` is executed, we keep adding to the existing node until we see that another command has been run.
- A Node can be either a Directory or a File:
    - Directory: no inherent filesize, contains children
    - File: has filesize, no children
- All nodes must keep track of their parent directory
- Two command types: Navigational (`cd`) and Informational (`ls`)
    - Navigational: does not create new nodes, only traverses through the tree
    - Informational: will create new nodes

Part2, if we need to keep the file names, just add to the class and insert via string[] splitString, splitString[1].