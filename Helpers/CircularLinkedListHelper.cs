﻿using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace aoc2022
{
    /// <summary>
    ///     Extension methods for System.LinkedList:
    ///     - if Node.Next == null, then return first Node
    ///     - if Node.Prev == null, then return last Node
    /// </summary>
    public static class CircularLinkedListHelper
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current) => current.Next ?? current.List.First;
        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current) => current.Previous ?? current.List.Last;
    }
}