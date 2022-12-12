Hello :)
Haven't heard too much about AoC until recently, and I figured I could use
a refresher on setting up new projects from scratch, new source control, etc and working on some
simpler problems without any pressure to finish before some deadline. Happy reading.

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

## Day 3: x
d
