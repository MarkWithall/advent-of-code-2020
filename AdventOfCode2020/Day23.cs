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
            var cups = new Cups(initialPosition);

            for (var i = 0; i < moves; i++)
            {
                var removed = cups.RemoveThreeClockwiseOfCurrent();
                cups.InsertCups(removed);
            }

            return cups.CurrentState;
        }

        private sealed class Cups
        {
            private string _cups;
            private char _currentCup;

            public Cups(string cups)
            {
                _cups = cups;
                _currentCup = _cups[0];
            }

            public string RemoveThreeClockwiseOfCurrent()
            {
                var currentCupIndex = _cups.IndexOf(_currentCup);

                var a = (currentCupIndex + 1) % _cups.Length;
                var b = (currentCupIndex + 2) % _cups.Length;
                var c = (currentCupIndex + 3) % _cups.Length;

                var removed = new string(new[] {_cups[a], _cups[b], _cups[c]});

                foreach (var i in new[] {a, b, c}.OrderByDescending(i => i))
                {
                    _cups = _cups.Remove(i, 1);
                }

                return removed;
            }

            public void InsertCups(string cups)
            {
                var destinationCup = _currentCup;
                do
                {
                    destinationCup = (char) (destinationCup - 1);
                    if (destinationCup < '0')
                    {
                        destinationCup = (char) (destinationCup + 10);
                    }
                } while (!_cups.Contains(destinationCup));

                var destinationIndex = (_cups.IndexOf(destinationCup) + 1) % _cups.Length;
                _cups = _cups.Insert(destinationIndex, cups);

                var currentCupIndex = _cups.IndexOf(_currentCup);
                _currentCup = _cups[(currentCupIndex + 1) % _cups.Length];
            }

            public string CurrentState =>
                string.Join("", _cups.SkipWhile(c => c != '1').Skip(1).Concat(_cups.TakeWhile(c => c != '1')));
        }
    }
}