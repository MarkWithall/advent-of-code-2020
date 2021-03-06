﻿using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 11: Seating System")]
    public sealed class Day11
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(2412, SimulateUntilStable(Day11Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(37, SimulateUntilStable(Day11SampleInput));
        }

        [Test, Category("slow")]
        public void Part2()
        {
            Assert.AreEqual(2176, SimulateUntilStable(Day11Input, false));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.AreEqual(26, SimulateUntilStable(Day11SampleInput, false));
        }

        private static int SimulateUntilStable(string[] input, bool part1 = true)
        {
            var seatingArea = SeatingArea.Create(input);
            seatingArea.SimulateUntilStable(part1);
            return seatingArea.OccupiedSeats();
        }

        private sealed class SeatingArea
        {
            private const char Floor = '.';
            private const char EmptySeat = 'L';
            private const char OccupiedSeat = '#';

            private char[,] _area;

            public static SeatingArea Create(string[] input)
            {
                var rows = input.Length;
                var columns = input[0].Length;
                var area = new char[rows + 2, columns + 2];
                for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                {
                    area[row + 1, column + 1] = input[row][column];
                }

                return new SeatingArea(area);
            }

            private SeatingArea(char[,] area)
            {
                _area = area;
            }

            public int OccupiedSeats()
            {
                var occupied = 0;
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                {
                    if (Cell(row, column) == OccupiedSeat)
                    {
                        occupied++;
                    }
                }

                return occupied;
            }

            public void SimulateUntilStable(bool part1 = true)
            {
                while (true)
                {
                    var nextIteration = NextIteration(part1);
                    if (IsSame(nextIteration, _area))
                    {
                        break;
                    }

                    _area = nextIteration;
                }
            }

            private bool IsSame(char[,] a1, char[,] a2)
            {
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                {
                    if (a1[row, column] != a2[row, column])
                    {
                        return false;
                    }
                }

                return true;
            }

            private char[,] NextIteration(bool part1)
            {
                var nextIteration = new char[_area.GetLength(0), _area.GetLength(1)];
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                {
                    nextIteration[row + 1, column + 1] = part1 ? NewStatePart1(row, column) : NewStatePart2(row, column);
                }

                return nextIteration;

                char NewStatePart1(int row, int column) =>
                    Cell(row, column) switch
                    {
                        Floor => Floor,
                        EmptySeat when NeighbouringOccupiedSeatCount(row, column) == 0 => OccupiedSeat,
                        OccupiedSeat when NeighbouringOccupiedSeatCount(row, column) >= 4 => EmptySeat,
                        _ => Cell(row, column)
                    };

                char NewStatePart2(int row, int column) =>
                    Cell(row, column) switch
                    {
                        Floor => Floor,
                        EmptySeat when OccupiedSeatsVisibleFrom(row, column) == 0 => OccupiedSeat,
                        OccupiedSeat when OccupiedSeatsVisibleFrom(row, column) >= 5 => EmptySeat,
                        _ => Cell(row, column)
                    };
            }

            private int NeighbouringOccupiedSeatCount(int row, int column)
            {
                char[] neighbours =
                {
                    Cell(row - 1, column - 1),
                    Cell(row - 1, column),
                    Cell(row - 1, column + 1),
                    Cell(row, column - 1),
                    Cell(row, column + 1),
                    Cell(row + 1, column - 1),
                    Cell(row + 1, column),
                    Cell(row + 1, column + 1)
                };

                return neighbours.Count(n => n == OccupiedSeat);
            }

            private int OccupiedSeatsVisibleFrom(int row, int column)
            {
                return OccupiedSeatsVisibleInDirection(-1, -1) +
                       OccupiedSeatsVisibleInDirection(-1, 0) +
                       OccupiedSeatsVisibleInDirection(-1, +1) +
                       OccupiedSeatsVisibleInDirection(0, -1) +
                       OccupiedSeatsVisibleInDirection(0, +1) +
                       OccupiedSeatsVisibleInDirection(+1, -1) +
                       OccupiedSeatsVisibleInDirection(+1, 0) +
                       OccupiedSeatsVisibleInDirection(+1, +1);

                int OccupiedSeatsVisibleInDirection(int dRow, int dColumn)
                {
                    return CellsInDirection(dRow, dColumn).FirstOrDefault(c => c != Floor) == OccupiedSeat ? 1 : 0;
                }

                IEnumerable<char> CellsInDirection(int dRow, int dColumn)
                {
                    var r = row + dRow;
                    var c = column + dColumn;
                    while (0 <= r && r < Rows && 0 <= c && c < Columns)
                    {
                        yield return Cell(r, c);
                        r += dRow;
                        c += dColumn;
                    }
                }
            }

            private int Rows => _area.GetLength(0) - 2;
            private int Columns => _area.GetLength(1) - 2;
            private char Cell(int row, int column) => _area[row + 1, column + 1];
        }

        private static readonly string[] Day11SampleInput =
        {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL"
        };

        private static readonly string[] Day11Input =
        {
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLL.L",
            "LLLLLLLLLLLLLLLLLLL.L.LL.LLL.LLLLL.LLLLLLLLLLL.LL.LLLLL.LLLLLLLL.LLLLLLLLL.LLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLLLLLLLLLLLL.LL.LL.LLLLLLL.LLLLLL.LLLLLLLLL.LLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL..LLLLLLL.L.LLL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLL.L.LLLL.L.LLL.LLLLLLLLLLLLLL.LLL.LLL.LLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLL.L.LLLLLLLLLLLLLLLLLLL",
            "L...LLL...L....L......L..L....LL...L.LLL...............L..LL.LLL.LL..L..........L......L.L....L.LL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.L.LLLL.LLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLL.L.LLLLL.LLL.LLL.LLLLLL.LLLLL.LLLL.LLL.LLLLL.LLLLLLLLLLLL.L.LLLLLLLLL.LL",
            "LLLLLLLL.LLLL.L.LLL.LLLLLLLLLLLL.L.LLLLLLLLLL.LLLLLLLLL.LLLLLLLL..L.LL.LLLLLLLLLLLLLL..L.LLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLL..LLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LL.LLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLL.",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLL.LLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLL.LLLLLLLL.L.LLLL.LLLLLLL",
            "L.LL.....L...LLL..L..L..L...L..L..L.LLLL...L...LLL...L.L...LL.L.....L.....L.......L..L.L....L..L..",
            "LLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.L.LLL.L.LLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLL.L.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLLL.LLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LL.LLLLLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLL.LLL.LLL.L.LLLL.LLLLLLLLLLLL",
            "L...L.LLL..L..L....LL.....L.LL...L......L...L...LL.LLL....LL........L.L.....LL..L...L..LL.LL.L.LL.",
            "LLLLLLLLLLLLLLLLLLL.LLLLL.LL.LLLLL.LLLLLLLLLLLLLL.L.LLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLL.L.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.L.LLL.LLLL.LLLLLLLL.LL.LL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLLLLLLLLL..LLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            ".L..LL.L.LLLL.LLLLLLLLLLLLLL.LLL.LLLLLLLLL.LLL.LL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "....L..........L.L.L......LL.L....L.LLL...L.L.L.L..L.....L........L...L...L.L.......LL...L.L..LLL.",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LL.LL.LLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLL..LLLL.LLL.LLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLL..L.LLL.LLLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLL.LL.LLLLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.",
            "LLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLLLLL.LLL.LLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLL..LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.LLL.LLLLLLLLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "L.LLLLLL.LLLL.LL.LL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLL",
            ".LL.LL...L.L...L..LL..L........L..L...........L.L..L...L.L...L..LL..L..L......L.L.....L..L......L.",
            "LLLLLLLL.LLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLL.L.LLLLLLLLLL.LLLLLLLLLLLLLL.LLL.LLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLL.LL.LL.LLLLLLLL.L.LLL.LLLLLLLLLLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LL.LL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLL.L.LLLLL.LLLLLL.LLLLLLLLLLL.",
            "LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLL.LLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLL.LLL.LLLL.LLLLL.LLLLLLLL.LLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLL",
            "..L.L.L.L.LL..LL....L....L..L.L.....L...LL.....L.LLL.....L.L.LLLL.L....L.L.....L..L...L.L.LL.L....",
            "LLLLLLLL.LLLLLL.LLLLLLLLLLL..LLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLL.LL.LLLLL.LLLLLLL.LLLLLL.LLL.LLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLL.L.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL..LLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLLLLLL.L.LL.LLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.L.LLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL...LL.L.LLLLLLLLL.LL",
            "LLLLLLLL.LLLL.LLLLLLLLLLLLLL.LLLLL..L.LLLL.LLLLLL.LL.LL.LLLLLLLLLLLLLLLLLL.LLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLL.L.LL.LLLLL.LLLLLLLL.LLLLLLL.LLLLLLLL.L.L.LLLLL..LLLLLLL.LLLLL.LLL.LLL.LLLLLL.LLLLLLLLLLLL",
            ".....L.L.LL.......L..LLL...LL....L.L.L....L..L.L.L.LL...L..L....L....LL.L.L....L....LL..L.L....L..",
            "LLLLLLLL.LLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLL.LL.L.LLLL.LLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            ".LLLLLLL.LLLLLLLLLL.LLLLLL.L.LLLLL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLL.LLL.LLLL.LLLLL.L.LLLLL.LLLLLL.LLLLLLLLLL.L",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLL",
            "LLLLLLLL.LLL..LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLL.LLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "..L....L..L..L..L....L.......L.....L.LL.L.L...LL.L.....LL.L...LL.LL.L...L.L....LL..L.....LL.L..L..",
            "LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLL..LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLL.LL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLL.LL.LLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.L.LLL.L.LLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLL",
            "LLLLLLLL.LL.LLLLLLL.L.LLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLL.",
            "LLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "..L....LL.L.L..L.L.L.LL..L.L.....L.......LLLL...L.....L...LL..L..L..L....L.L.LL...L..L...L......L.",
            "LLLLLLLL.LLLL.LLLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLLL.LLL.LLLLLLLLL.LLL.LLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LL.LL.LLLLLLLL.LLLLL.LL.LLLL.LLL.LL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLL..L.LLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLL.LLLLLLLLL.LLLLL.LLLLLLL.LL.LLL.LLLLLLLLLLLLLL.LLLLL.L.LLLLL.LLLLLL.LL.LLLLL.LLL",
            "LLLLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLL.LLLL.LLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLL.LLLLLLLL.LL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLL.L.LLLLL.LL.LL..LLLLLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLL.LLLL..LLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLL.LLLLLL.L.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "........LLLL.L.LL...L..L.L...LLLL.L.L.L.L....L...L..L.L.LLL....LL.L.....L...LLL.....L.LL..L......L",
            "LLLLLLLL.LLLLLLLLLLLLLLLLLLL.LLLL..LLLLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLL.LLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLL.LLLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL..LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLL.LL.LLLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LL.LL.LLLLLLLLLLLLLL.LLLLL.L.LLLLLLLLLLLLLLLL.LL",
            "LLLLLLLL.LLLL.LLLLL.LLLL.LLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLL.LLLL.LLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLL.L",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLL..LLLLLLL.LLLLLL.LL.LL.L.LLLLLL.LLLL..LLLLLL..LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "L......LLLLL..LL.L.....L......L..L....L..LLL.L....LLLLL....L..L...LL.L..L...L...L......LL.........",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLL..LLLLL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.L.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            ".LLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.L.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLLLLLL..LLLLL.LL.LLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLL.",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLL.LLLL.LLLLLLLLLLLLLLLLLLL.L.LLLLLL.LLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLL..LLLL.LLLL.L.LLLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLL.LLLLL.LLLLLLLL.L.LLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLL.LL.LLLL.LLLLLL.LLLLLLLLLLLL",
            "LLLLLLL...LLLLLLLLL.LLLLLLLL.LL.LLLLLLLLLL.LLLLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLL.L.LLLLLLLLLLLL.LLLLLLLLLLLL",
            "LLLLLLLL.LLLLLL.LLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLL"
        };
    }
}