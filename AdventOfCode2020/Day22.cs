using System.Collections.Generic;
using System.Diagnostics;
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

        [Test, Category("slow")]
        public void Part2()
        {
            Assert.AreEqual(34031, WinningRecursiveScore(Day22Input));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.AreEqual(291, WinningRecursiveScore(Day22SampleInput));
        }

        [Test]
        public void Part2Sample2()
        {
            Assert.AreEqual(105, WinningRecursiveScore(Day22SampleInput2));
        }

        private static long WinningScore(string[] input)
        {
            var (deck1, deck2) = ReadDecks(input);

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

            return CalculateScore(deck1.Any() ? deck1 : deck2);
        }

        private static long WinningRecursiveScore(string[] input)
        {
            var (deck1, deck2) = ReadDecks(input);
            var winner = PlayGame(deck1, deck2);
            return CalculateScore(winner == 1 ? deck1 : deck2);
        }

        private static long PlayGame(Queue<long> deck1, Queue<long> deck2)
        {
            var previousRounds = new HashSet<string>();

            while (deck1.Any() && deck2.Any())
            {
                // Check for repeated game state
                var gameState = string.Join(",", deck1) + ":" + string.Join(",", deck2);
                Debug.WriteLine(gameState);
                if (previousRounds.Contains(gameState))
                {
                    return 1;
                }

                previousRounds.Add(gameState);

                // Draw cards
                var card1 = deck1.Dequeue();
                var card2 = deck2.Dequeue();

                // Check winner
                var winner = deck1.Count >= card1 && deck2.Count >= card2
                    // Play recursive game
                    ? PlayGame(new Queue<long>(deck1.Take((int) card1)), new Queue<long>(deck2.Take((int) card2)))
                    : card1 > card2
                        ? 1
                        : 2;

                // Update decks
                if (winner == 1)
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

            return deck1.Any() ? 1 : 2;
        }

        private static (Queue<long> deck1, Queue<long> deck2) ReadDecks(string[] input)
        {
            Queue<long> deck1 = new(input.TakeWhile(i => i != "").Skip(1).Select(long.Parse));
            Queue<long> deck2 = new(input.Skip(deck1.Count + 2).Skip(1).Select(long.Parse));
            return (deck1, deck2);
        }

        private static long CalculateScore(Queue<long> deck)
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

        private static readonly string[] Day22SampleInput2 =
        {
            "Player 1:",
            "43",
            "19",
            "",
            "Player 2:",
            "2",
            "29",
            "14"
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