using System;
using System.Linq;
using System.Numerics;
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

        [Test]
        public void Part2()
        {
            Assert.AreEqual(new BigInteger(807435693182510), EarliestSequentialDeparture(Day13Input[1]));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(new BigInteger(1068781), EarliestSequentialDeparture(Day13SampleInput[1]));
                Assert.AreEqual(new BigInteger(3417), EarliestSequentialDeparture("17,x,13,19"));
                Assert.AreEqual(new BigInteger(754018), EarliestSequentialDeparture("67,7,59,61"));
                Assert.AreEqual(new BigInteger(779210), EarliestSequentialDeparture("67,x,7,59,61"));
                Assert.AreEqual(new BigInteger(1261476), EarliestSequentialDeparture("67,7,x,59,61"));
                Assert.AreEqual(new BigInteger(1202161486), EarliestSequentialDeparture("1789,37,47,1889"));
            });
        }

        private static long BusIdMultipliedByWait(string[] input)
        {
            var arrivalTime = long.Parse(input[0]);
            var (id, time) = input[1]
                .Split(',')
                .Where(i => i != "x")
                .Select(long.Parse)
                .Select(b => (id: b, time: (long) Math.Ceiling(arrivalTime / (double) b) * b))
                .OrderBy(b => b.time)
                .First();

            return id * (time - arrivalTime);
        }

        private static BigInteger EarliestSequentialDeparture(string input)
        {
            var busIds = input.Split(',').Select(CreateBus).Where(b => b is not UnknownBus).ToArray();

            var offset = new BigInteger(-busIds[0].Index);
            var a = new BigInteger(busIds[0].Id);
            foreach (var b in busIds.Skip(1))
            {
                (offset, a) = OffsetAndStep(a, offset, new BigInteger(b.Id), new BigInteger(-b.Index));
                if (offset % busIds[0].Id != 0)
                    throw new InvalidOperationException("It's gone wrong");
            }

            return offset;

            static IBus CreateBus(string id, int index) => id switch
            {
                "x" => new UnknownBus(),
                _ => new Bus(long.Parse(id), index)
            };
        }

        [Test]
        public void TestOffsetAndStep()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(18, OffsetAndStep(9, 0, 15, 3).offset);
                Assert.AreEqual(120, OffsetAndStep(30, 0, 38, 6).offset);
                Assert.Catch<Exception>(() => OffsetAndStep(9, 0, 12, 5));
            });
        }

        private static (BigInteger offset, BigInteger step) OffsetAndStep(BigInteger a, BigInteger aOffset, BigInteger b, BigInteger bOffset)
        {
            var (gcd, aMultiplier) = ExtendedGcd(a, b);

            var difference = aOffset - bOffset;
            if (difference % gcd != 0)
            {
                throw new InvalidOperationException("Rotation reference points never synchronise");
            }

            var lcm = a * b / gcd;
            var multiplier = aMultiplier * a * (difference / gcd);
            var offset = Mod(aOffset - multiplier, lcm);

            return (offset, lcm);
        }

        private static (BigInteger gcd, BigInteger s) ExtendedGcd(BigInteger a, BigInteger b)
        {
            var (oldR, r) = (a, b);
            var (oldS, s) = (new BigInteger(1), new BigInteger(0));
            var (oldT, t) = (new BigInteger(0), new BigInteger(1));

            while (r != 0)
            {
                var quotient = oldR / r;
                (oldR, r) = (r, oldR - quotient * r);
                (oldS, s) = (s, oldS - quotient * s);
                (oldT, t) = (t, oldT - quotient * t);
            }

            return (oldR, oldS); // oldR = oldS*a + oldT*b
        }

        private static BigInteger Mod(BigInteger a, BigInteger b)
        {
            var mod = a % b;
            return mod < 0 ? mod + b : mod;
        }

        private interface IBus
        {
            public long Id { get; }
            public long Index { get; }
            bool DepartsAtTime(long time);
        }

        private sealed class UnknownBus : IBus
        {
            public long Id { get; } = -1;
            public long Index { get; } = -1;
            public bool DepartsAtTime(long time) => true;
        }

        private sealed class Bus : IBus
        {
            public Bus(long id, long index) => (Id, Index) = (id, index);

            public long Id { get; }
            public long Index { get; }
            public bool DepartsAtTime(long time) => (time + Index) % Id == 0;
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