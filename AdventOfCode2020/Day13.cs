using System;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 13: Shuttle Search")]
    public sealed class Day13
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(261, BusIdMultipliedByWait(Day13Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(295, BusIdMultipliedByWait(Day13SampleInput));
        }

        private static long BusIdMultipliedByWait(string[] input)
        {
            var arrivalTime = int.Parse(input[0]);
            var busIds = input[1].Split(',').Where(i => i != "x").Select(int.Parse);
            var nextBus = busIds.Select(b => (id: b, time: (long) Math.Ceiling(arrivalTime / (double) b) * b)).OrderBy(b => b.time).First();

            return nextBus.id * (nextBus.time - arrivalTime);
        }

        private static readonly string[] Day13SampleInput =
        {
            "939",
            "7,13,x,x,59,x,31,19"
        };

        private static readonly string[] Day13Input =
        {
            "1001796",
            "37,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,457,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,x,x,x,x,23,x,x,x,x,x,29,x,431,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,19"
        };
    }
}