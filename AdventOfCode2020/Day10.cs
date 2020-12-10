using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 10: Adapter Array")]
    public sealed class Day10
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(1625, ProductOf1JoltAnd3JoltDifferenceCounts(Day10Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(35, ProductOf1JoltAnd3JoltDifferenceCounts(Day10SampleInput1));
                Assert.AreEqual(220, ProductOf1JoltAnd3JoltDifferenceCounts(Day10SampleInput2));
            });
        }

        [Test]
        public void Part2()
        {
            Assert.AreEqual(3100448333024, ArrangementsToConnect(Day10Input));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(8, ArrangementsToConnect(Day10SampleInput1));
                Assert.AreEqual(19208, ArrangementsToConnect(Day10SampleInput2));
            });
        }

        private static long ArrangementsToConnect(int[] input)
        {
            var deviceJoltage = input.Max() + 3;
            var joltages = input.OrderBy(j => j).Prepend(0).Append(deviceJoltage).ToArray();
            return FindArrangements(0, joltages.Skip(1).ToArray(), new Dictionary<int, long>());
        }

        private static long FindArrangements(int currentJoltage, int[] remainingJoltages, IDictionary<int, long> cache)
        {
            if (!remainingJoltages.Any())
            {
                return 1;
            }

            var neighbours = remainingJoltages.TakeWhile(j => j - currentJoltage <= 3).ToArray();
            var sum = 0L;
            for (var i = 0; i < neighbours.Length; i++)
            {
                var joltage = neighbours[i];
                if (!cache.TryGetValue(joltage, out var count))
                {
                    count = FindArrangements(joltage, remainingJoltages.Skip(i + 1).ToArray(), cache);
                    cache.Add(joltage, count);
                }

                sum += count;
            }

            return sum;
        }

        private static int ProductOf1JoltAnd3JoltDifferenceCounts(int[] input)
        {
            var deviceJoltage = input.Max() + 3;
            var joltages = input.OrderBy(j => j).Prepend(0).Append(deviceJoltage).ToArray();
            var differences = joltages.Zip(joltages.Skip(1), (i, j) => j - i);
            var lookup = differences.ToLookup(d => d);
            return lookup[1].Count() * lookup[3].Count();
        }

        private static readonly int[] Day10SampleInput1 =
        {
            16,
            10,
            15,
            5,
            1,
            11,
            7,
            19,
            6,
            12,
            4
        };

        private static readonly int[] Day10SampleInput2 =
        {
            28,
            33,
            18,
            42,
            31,
            14,
            46,
            20,
            48,
            47,
            24,
            23,
            49,
            45,
            19,
            38,
            39,
            11,
            1,
            32,
            25,
            35,
            8,
            17,
            7,
            9,
            4,
            2,
            34,
            10,
            3
        };

        private static readonly int[] Day10Input =
        {
            67,
            118,
            90,
            41,
            105,
            24,
            137,
            129,
            124,
            15,
            59,
            91,
            94,
            60,
            108,
            63,
            112,
            48,
            62,
            125,
            68,
            126,
            131,
            4,
            1,
            44,
            77,
            115,
            75,
            89,
            7,
            3,
            82,
            28,
            97,
            130,
            104,
            54,
            40,
            80,
            76,
            19,
            136,
            31,
            98,
            110,
            133,
            84,
            2,
            51,
            18,
            70,
            12,
            120,
            47,
            66,
            27,
            39,
            109,
            61,
            34,
            121,
            38,
            96,
            30,
            83,
            69,
            13,
            81,
            37,
            119,
            55,
            20,
            87,
            95,
            29,
            88,
            111,
            45,
            46,
            14,
            11,
            8,
            74,
            101,
            73,
            56,
            132,
            23
        };
    }
}