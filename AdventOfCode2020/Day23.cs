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

        private static string MoveCups(string initialPosition, int moves)
        {
            var cups = new Cups(initialPosition.Select(c => int.Parse(c.ToString())));

            for (var i = 0; i < moves; i++)
            {
                var removed = cups.RemoveThreeClockwiseOfCurrent();
                cups.InsertCups(removed);
            }

            return cups.CurrentState;
        }

        private sealed class Cups
        {
            private readonly List<int> _cups;
            private int _currentCup;

            public Cups(IEnumerable<int> cups)
            {
                _cups = cups.ToList();
                _currentCup = _cups[0];
            }

            public IEnumerable<int> RemoveThreeClockwiseOfCurrent()
            {
                var currentCupIndex = _cups.IndexOf(_currentCup);

                var a = (currentCupIndex + 1) % _cups.Count;
                var b = (currentCupIndex + 2) % _cups.Count;
                var c = (currentCupIndex + 3) % _cups.Count;

                var removed = new[] {_cups[a], _cups[b], _cups[c]};

                foreach (var i in new[] {a, b, c}.OrderByDescending(i => i))
                {
                    _cups.RemoveAt(i);
                }

                return removed;
            }

            public void InsertCups(IEnumerable<int> cups)
            {
                var destinationCup = _currentCup;
                do
                {
                    destinationCup = destinationCup - 1;
                    if (destinationCup < 1)
                    {
                        destinationCup = _cups.Max();
                    }
                } while (!_cups.Contains(destinationCup));

                var destinationIndex = (_cups.IndexOf(destinationCup) + 1) % _cups.Count;
                _cups.InsertRange(destinationIndex, cups);

                var currentCupIndex = _cups.IndexOf(_currentCup);
                _currentCup = _cups[(currentCupIndex + 1) % _cups.Count];
            }

            public string CurrentState =>
                string.Join("", _cups.SkipWhile(c => c != 1).Skip(1).Concat(_cups.TakeWhile(c => c != 1)));
        }
    }
}