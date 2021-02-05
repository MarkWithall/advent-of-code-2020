using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 17: Conway Cubes")]
    public sealed class Day17
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(263, ActiveCubes3D(Day17Input, 6));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(112, ActiveCubes3D(Day17SampleInput, 6));
        }

        [Test, Category("slow")]
        public void Part2()
        {
            Assert.AreEqual(1680, ActiveCubes4D(Day17Input, 6));
        }

        [Test, Category("slow")]
        public void Part2Sample()
        {
            Assert.AreEqual(848, ActiveCubes4D(Day17SampleInput, 6));
        }

        private static long ActiveCubes3D(string[] input, long cycles)
        {
            Dictionary<(long, long, long), bool> grid = new();

            ReadInput();
            for (var i = 0; i < cycles; i++)
            {
                grid = Simulate(grid);
            }

            return grid.Values.Count(v => v);

            static Dictionary<(long, long, long), bool> Simulate(Dictionary<(long x, long y, long z), bool> initialGrid)
            {
                Dictionary<(long, long, long), bool> newGrid = new();

                for (var x = initialGrid.Keys.Min(k => k.x) - 1; x <= initialGrid.Keys.Max(k => k.x) + 1; x++)
                for (var y = initialGrid.Keys.Min(k => k.y) - 1; y <= initialGrid.Keys.Max(k => k.y) + 1; y++)
                for (var z = initialGrid.Keys.Min(k => k.z) - 1; z <= initialGrid.Keys.Max(k => k.z) + 1; z++)
                {
                    var coord = (x, y, z);

                    List<(long, long, long)> neighbours = new();
                    for (var xn = -1; xn <= 1; xn++)
                    for (var yn = -1; yn <= 1; yn++)
                    for (var zn = -1; zn <= 1; zn++)
                    {
                        if (xn != 0 || yn != 0 || zn != 0)
                        {
                            neighbours.Add((coord.x + xn, coord.y + yn, coord.z + zn));
                        }
                    }

                    var activeNeighbourCount = neighbours.Count(IsActive);

                    if (IsActive(coord) && (activeNeighbourCount == 2 || activeNeighbourCount == 3) ||
                        !IsActive(coord) && activeNeighbourCount == 3)
                    {
                        SetState(coord, true);
                    }
                }

                return newGrid;

                bool IsActive((long, long, long) coords) => initialGrid.TryGetValue(coords, out var state) && state;
                void SetState((long, long, long) coords, bool state) => newGrid[coords] = state;
            }

            void ReadInput()
            {
                for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                {
                    grid[(x, y, 0)] = input[x][y] == '#';
                }
            }
        }

        private static long ActiveCubes4D(string[] input, long cycles)
        {
            Dictionary<(long, long, long, long), bool> grid = new();

            ReadInput();
            for (var i = 0; i < cycles; i++)
            {
                grid = Simulate(grid);
            }

            return grid.Values.Count(v => v);

            static Dictionary<(long, long, long, long), bool> Simulate(Dictionary<(long x, long y, long z, long w), bool> initialGrid)
            {
                Dictionary<(long, long, long, long), bool> newGrid = new();

                for (var x = initialGrid.Keys.Min(k => k.x) - 1; x <= initialGrid.Keys.Max(k => k.x) + 1; x++)
                for (var y = initialGrid.Keys.Min(k => k.y) - 1; y <= initialGrid.Keys.Max(k => k.y) + 1; y++)
                for (var z = initialGrid.Keys.Min(k => k.z) - 1; z <= initialGrid.Keys.Max(k => k.z) + 1; z++)
                for (var w = initialGrid.Keys.Min(k => k.w) - 1; w <= initialGrid.Keys.Max(k => k.w) + 1; w++)
                {
                    var coord = (x, y, z, w);

                    List<(long, long, long, long)> neighbours = new();
                    for (var xn = -1; xn <= 1; xn++)
                    for (var yn = -1; yn <= 1; yn++)
                    for (var zn = -1; zn <= 1; zn++)
                    for (var wn = -1; wn <= 1; wn++)
                    {
                        if (xn != 0 || yn != 0 || zn != 0 || wn != 0)
                        {
                            neighbours.Add((coord.x + xn, coord.y + yn, coord.z + zn, coord.w + wn));
                        }
                    }

                    var activeNeighbourCount = neighbours.Count(IsActive);

                    if (IsActive(coord) && (activeNeighbourCount == 2 || activeNeighbourCount == 3) ||
                        !IsActive(coord) && activeNeighbourCount == 3)
                    {
                        SetState(coord, true);
                    }
                }

                return newGrid;

                bool IsActive((long, long, long, long) coords) => initialGrid.TryGetValue(coords, out var state) && state;
                void SetState((long, long, long, long) coords, bool state) => newGrid[coords] = state;
            }

            void ReadInput()
            {
                for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                {
                    grid[(x, y, 0, 0)] = input[x][y] == '#';
                }
            }
        }

        private static readonly string[] Day17SampleInput =
        {
            ".#.",
            "..#",
            "###"
        };

        private static readonly string[] Day17Input =
        {
            "##....#.",
            "#.#..#..",
            "...#....",
            "...#.#..",
            "###....#",
            "#.#....#",
            ".#....##",
            ".#.###.#"
        };
    }
}