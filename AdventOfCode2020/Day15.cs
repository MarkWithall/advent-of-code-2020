using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 15: Rambunctious Recitation")]
    public class Day15
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(412, NthNumberSpoken("10,16,6,0,1,17", 2020));
        }

        [TestCase("0,3,6", 436)]
        [TestCase("1,3,2", 1)]
        [TestCase("2,1,3", 10)]
        [TestCase("1,2,3", 27)]
        [TestCase("2,3,1", 78)]
        [TestCase("3,2,1", 438)]
        [TestCase("3,1,2", 1836)]
        public void Part1Sample(string input, long expectedOutput)
        {
            Assert.AreEqual(expectedOutput, NthNumberSpoken(input, 2020));
        }

        private static long NthNumberSpoken(string input, long n)
        {
            Dictionary<long, List<long>> timeLastSpoken = new();

            long time = 0;
            long previous = 0;
            foreach (var i in input.Split(',').Select(long.Parse))
            {
                if (timeLastSpoken.TryGetValue(i, out var list))
                {
                    list.Add(time);
                }
                else
                {
                    timeLastSpoken.Add(i, new List<long> {time});
                }

                previous = i;
                time++;
            }

            while (time < n)
            {
                var l1 = timeLastSpoken[previous];
                var current = l1.Count == 1 ? 0 : l1[^1] - l1[^2];

                if (timeLastSpoken.TryGetValue(current, out var l2))
                {
                    l2.Add(time);
                }
                else
                {
                    timeLastSpoken.Add(current, new List<long> {time});
                }

                previous = current;
                time++;
            }

            return previous;
        }
    }
}