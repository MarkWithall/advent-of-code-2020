using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 23: Crab Cups")]
    public sealed class Day23
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual("45286397", MoveCups("389547612", 100));
        }

        [TestCase("389125467", 10, "92658374")]
        [TestCase("389125467", 100, "67384529")]
        public void Part1Sample(string input, int moves, string expectedOutput)
        {
            Assert.AreEqual(expectedOutput, MoveCups(input, moves));
        }


        [Test, Category("slow")]
        public void Part2()
        {
            Assert.AreEqual(836763710, FindHiddenStars("389547612"));
        }

        [Test, Category("slow")]
        public void Part2Sample()
        {
            Assert.AreEqual(149245887792, FindHiddenStars("389125467"));
        }

        private static string MoveCups(string initialPosition, int moves)
        {
            var cups = new Cups(initialPosition.Select(c => (long) int.Parse(c.ToString())));

            for (var i = 0; i < moves; i++)
            {
                var removed = cups.RemoveThreeClockwiseOfCurrent();
                cups.InsertCups(removed);
            }

            return cups.CurrentState;
        }

        private static long FindHiddenStars(string initialPosition)
        {
            var cups = new Cups(initialPosition.Select(c => int.Parse(c.ToString())).Concat(Enumerable.Range(10, 999_991)).Select(i => (long) i));

            for (var i = 0; i < 10_000_000; i++)
            {
                var removed = cups.RemoveThreeClockwiseOfCurrent();
                cups.InsertCups(removed);
            }

            return cups.ProductOfCupsClockwiseOfCup1();
        }

        private sealed class Cups
        {
            private readonly LinkedList<long> _cups;
            private LinkedListNode<long> _currentCup;
            private readonly Dictionary<long, LinkedListNode<long>> _nodeLookup = new();

            public Cups(IEnumerable<long> cups)
            {
                _cups = new LinkedList<long>(cups);
                _currentCup = _cups.First!;
                for (var n = _cups.First; n is not null; n = n.Next)
                {
                    _nodeLookup.Add(n.Value, n);
                }
            }

            public long[] RemoveThreeClockwiseOfCurrent()
            {
                var a = Next(_currentCup);
                var b = Next(a);
                var c = Next(b);

                var removed = new[] {a.Value, b.Value, c.Value};

                _nodeLookup.Remove(a.Value);
                _nodeLookup.Remove(b.Value);
                _nodeLookup.Remove(c.Value);

                _cups.Remove(a);
                _cups.Remove(b);
                _cups.Remove(c);

                return removed;
            }

            public void InsertCups(long[] cups)
            {
                var previousRemainingCup = PreviousRemainingCup(cups);
                foreach (var i in cups.Reverse())
                {
                    var newNode = _cups.AddAfter(previousRemainingCup, i);
                    _nodeLookup.Add(i, newNode);
                }

                _currentCup = Next(_currentCup);
            }

            public string CurrentState =>
                string.Join("", _cups.SkipWhile(c => c != 1).Skip(1).Concat(_cups.TakeWhile(c => c != 1)));

            public long ProductOfCupsClockwiseOfCup1()
            {
                var cup1 = _nodeLookup[1];
                var a = Next(cup1);
                var b = Next(a);
                return a.Value * b.Value;
            }

            private LinkedListNode<long> PreviousRemainingCup(long[] excluding)
            {
                var destinationCup = _currentCup.Value;
                do
                {
                    destinationCup -= 1;
                    if (destinationCup < 1)
                    {
                        destinationCup = _cups.Max();
                    }
                } while (excluding.Contains(destinationCup));

                return _nodeLookup[destinationCup];
            }

            private LinkedListNode<long> Next(LinkedListNode<long> node) => node.Next ?? _cups.First!;
        }
    }
}