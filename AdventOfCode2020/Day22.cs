using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 22: Crab Combat")]
    public sealed class Day22
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(30197, WinningScore(Day22Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(306, WinningScore(Day22SampleInput));
        }

        private static long WinningScore(string[] input)
        {
            var deckSize = (input.Length - 1) / 2;
            Queue<long> deck1 = new(input.Take(deckSize).Skip(1).Select(long.Parse));
            Queue<long> deck2 = new(input.Skip(deckSize + 1).Take(deckSize).Skip(1).Select(long.Parse));

            while (deck1.Any() && deck2.Any())
            {
                var card1 = deck1.Dequeue();
                var card2 = deck2.Dequeue();

                if (card1 > card2)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }

            var winningScore = CalculateScore(deck1.Any() ? deck1 : deck2);

            return winningScore;

            static long CalculateScore(Queue<long> deck)
            {
                var score = 0L;
                for (var i = 0; i < deck.Count; i++)
                {
                    var card = deck.ElementAt(i);
                    long multplier = deck.Count - i;
                    score += card * multplier;
                }

                return score;
            }
        }

        private static readonly string[] Day22SampleInput =
        {
            "Player 1:",
            "9",
            "2",
            "6",
            "3",
            "1",
            "",
            "Player 2:",
            "5",
            "8",
            "4",
            "7",
            "10"
        };

        private static readonly string[] Day22Input =
        {
            "Player 1:",
            "48",
            "23",
            "9",
            "34",
            "37",
            "36",
            "40",
            "26",
            "49",
            "7",
            "12",
            "20",
            "6",
            "45",
            "14",
            "42",
            "18",
            "31",
            "39",
            "47",
            "44",
            "15",
            "43",
            "10",
            "35",
            "",
            "Player 2:",
            "13",
            "19",
            "21",
            "32",
            "27",
            "16",
            "11",
            "29",
            "41",
            "46",
            "33",
            "1",
            "30",
            "22",
            "38",
            "5",
            "17",
            "4",
            "50",
            "2",
            "3",
            "28",
            "8",
            "25",
            "24"
        };
    }
}