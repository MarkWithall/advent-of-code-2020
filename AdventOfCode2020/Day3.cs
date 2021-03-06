﻿using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 3: Toboggan Trajectory")]
    public sealed class Day3
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(203, CountTrees(Day3Input, 3, 1));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(7, CountTrees(Day3SampleInput, 3, 1));
        }

        [Test]
        public void Part3()
        {
            Assert.AreEqual(3316272960L, CheckAllSlopes(Day3Input, Slopes));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.AreEqual(336, CheckAllSlopes(Day3SampleInput, Slopes));
        }

        private static long CheckAllSlopes(string[] map, (int right, int down)[] slopes) =>
            slopes
                .Select(s => CountTrees(map, s.right, s.down))
                .Aggregate(1L, (product, treeCount) => product * treeCount);

        private static readonly (int right, int down)[] Slopes =
        {
            (1, 1),
            (3, 1),
            (5, 1),
            (7, 1),
            (1, 2)
        };

        private static long CountTrees(string[] map, int right, int down)
        {
            var treeCount = 0;
            var currentX = 0;
            var currentY = 0;
            var mapWidth = map[0].Length;
            var mapHeight = map.Length;
            while (currentY < mapHeight)
            {
                var currentCell = map[currentY][currentX];
                if (currentCell == '#')
                {
                    treeCount++;
                }

                currentX = (currentX + right) % mapWidth;
                currentY += down;
            }

            return treeCount;
        }

        private static readonly string[] Day3SampleInput =
        {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#"
        };

        private static readonly string[] Day3Input =
        {
            ".....##.#.....#........#....##.",
            "....#...#...#.#.......#........",
            ".....##.#......#.......#.......",
            "...##.........#...#............",
            "........#...#.......#.........#",
            "..........#......#..#....#....#",
            "..................#..#..#....##",
            ".....##...#..#..#..#..#.##.....",
            "..##.###....#.#.........#......",
            "#.......#......#......#....##..",
            ".....#..#.#.......#......#.....",
            "............#............#.....",
            "...#.#........#........#.#.##.#",
            ".#..#...#.....#....##..........",
            "##..........#...#...#..........",
            "...........#...###...#.......##",
            ".#..#............#........#....",
            "##.#..#.....#.......#.#.#......",
            ".##.....#....#.#.......#.##....",
            "..##...........#.......#..##.#.",
            "##...#.#........#..#...#...#..#",
            ".#..#........#.#.......#..#...#",
            ".##.##.##...#.#............##..",
            "..#.#..###......#..#......#....",
            ".#..#..#.##.#.##.#.#...........",
            "...#....#..#.#.#.........#..#..",
            "......#.#....##.##......#......",
            "#....#.##.##....#..#...........",
            "...#.#.#.#..#.#..#.#..#.##.....",
            "#.....#######.###.##.#.#.#.....",
            "..#.##.....##......#...#.......",
            "..#....#..#...##.#..#..#..#..#.",
            ".............#.##....#.........",
            ".#....#.##.....#...............",
            ".#............#....#...#.##....",
            ".#.....#.##.###.......#..#.....",
            ".#...#.........#.......#..#....",
            "..#.#..#.##.......##...........",
            ".....##..#..#..#..#.##..#.....#",
            "..##............##...#..#......",
            "...#..#....#..##.....##..#.#...",
            "#.....##....#.#.#...#...#..##.#",
            "#.#..#.........#.##.#...#.#.#..",
            ".....#.#....##....#............",
            "#.......#..#.....##..#...#...#.",
            ".....#.#...#...#..#......#.....",
            "..##....#.#.#.#.#..#...........",
            "##..#...#.........#......#...#.",
            "..#...#.#.#.#..#.#.##..##......",
            "#............###.....###.......",
            "..........#...#........###.....",
            ".......##...#...#...#........#.",
            ".#..#.##.#.....................",
            ".#..##........##.##...#.......#",
            ".......##......#.....#......#..",
            ".##.#.....#......#......#......",
            "#...##.#.#...#.#...............",
            "........#..#...#.##.......#....",
            "...................#...#...##..",
            "...#...#.........#.....#..#.#..",
            ".###..#........#..##.##..#.##..",
            "#...#.....#.....#.....#..#..#..",
            "###..#.....#.#.#.#......#....#.",
            "#........#....##.#...##........",
            ".#.#..##........##....##.#.#...",
            "#...#....#.###.#.#.........#...",
            "...#...##..###.......#.........",
            "......#....#..##..#.....#.#....",
            "........#...##...###......##...",
            "..........##.#.......##........",
            "...#....#......#...##.....#....",
            "###.#.....#.#..#..#....#...#..#",
            ".#.....#.#....#...............#",
            "..#....#....####....###....#.#.",
            "....##........#..#.##.#....#...",
            ".......##...#...#..#....####...",
            "#...##.#......##...#..#........",
            "..##..#.##....#.......##.#.#...",
            "..#.#...............#...#.#....",
            "....#.....#.#.....#.##.......#.",
            "...#.#..##.#.#..............##.",
            "..#.....#...#.............#.##.",
            "##..#.#...#........#..#.....##.",
            "...........##...#.#.###...#....",
            "...#.#.#..#..................#.",
            ".#...##.............#...#......",
            "..#..#...#.#.......#...#.....#.",
            "..##.......#.#.................",
            ".##..#........###.....#....#.##",
            "......#..###.......#....##....#",
            "....#.....#.................#..",
            "........#...#...#..............",
            "...#..#.###.......#..#.#.#.##..",
            "..#...#.....#....#.........#...",
            "...#.............#........###..",
            "......#..............#......#..",
            "#..#...........#...#..........#",
            "...##...#.###..#...#.....#.#...",
            "....#..##......#.......##......",
            "....#....##.#...#.#..#....#...#",
            ".#...........#..#....##...#..##",
            "..#.#.................###.#...#",
            "..#.#.#...##...........#.......",
            "..........#..##...#.#..##....##",
            "........#........#.##..#.#...#.",
            ".....#...##.......##......#...#",
            "....#...#..#..#.....#..........",
            ".#..#......#..#..#..###.......#",
            ".##..........#...#...#.#.....##",
            "..#..........#.#.#...###.......",
            "....#................#...##....",
            ".##..#....#..........#.#.#.....",
            "..##...#.#........#.....#.##...",
            "....####.....#..#.........##..#",
            "......#.........#...#..........",
            "....#...................#..##..",
            ".##....#.#.........#....#...#..",
            "....##...##.....#..####........",
            "..##.#....#.#.......##...#.....",
            "#...#.#.#...#..#..##.....#.....",
            "#..................###.....#...",
            "#.#.....#.......#.#...###.#....",
            ".#..#....#............#........",
            "#.#....#..#.#...............#..",
            "..#..#..#.............#......#.",
            "..#.......##...................",
            ".#....#.........#....#.#.#..#..",
            "....#....#..#...............#..",
            "......#..#..##......#.........#",
            "..#.##........##......#..#..#.#",
            "#.....#.#....#.........##...#..",
            "###..............#....###...##.",
            "....#..##......#.......##......",
            "......#...#.##......##....#..#.",
            "..........#....#..##.......#..#",
            ".#..#...##..#...........#..#..#",
            ".....#....#...#..###...###....#",
            ".#####..#...#.#.#..#.#.###...##",
            "..##............##.#...#.##...#",
            ".##..#...#...#....##.#..#..##..",
            ".#....#...#............##..#...",
            ".#.#......#....#....#..##..##..",
            ".........#...#.......#.##..#...",
            "#.........#.....##.....#..#..#.",
            "...##.#...#...#..#..#....##..##",
            ".#............#...#....##......",
            "..#...#.##.........#.#......#.#",
            "....#.##........#.........#..##",
            "#.........#......#.#......#..#.",
            "........#.#.......#.#........#.",
            "..#..........##.#...#..#.#.....",
            "..#...#....#...#...#..#.#..#.#.",
            ".#.........#....#..#####..#....",
            "#.#....#.#.###...#.............",
            "..##...........##......##......",
            "#.....#..#....#...............#",
            "...#.#..#....##......#...##....",
            "...#........#.....#...#..#.....",
            ".#......##.........#......#....",
            "..#..###.##...#.#.....#........",
            ".............#......#..#.......",
            "..#...............#.#...#..#..#",
            ".......#..#...#.#####......#..#",
            ".........#.....#...............",
            "##........#............#.#.....",
            ".#...#.....#..#..#...#....#...#",
            "..#....#....##......##.....#.#.",
            "#...##..##......#...#....#.....",
            "....#.#.#.....###....##.##....#",
            "..........##...##.......#......",
            "..#.......#...##.#....##.##....",
            "....#........................#.",
            "...#...#.#.##...#.....#...#..#.",
            ".#....##..#..#..........##..##.",
            ".#.....#..#...#.##.....#.......",
            ".#.##...#.#..#.....##....#...#.",
            ".##...#........##....#..#......",
            ".....#........#..........#.#..#",
            "....#..##.......#..#.....#.....",
            "...........#...#........#.##..#",
            ".....#..#....#..#.#.....#....##",
            ".....#....#.##.#..##...........",
            "...##.......##.........#.......",
            "...............##..#....#.#....",
            ".......###..#........#..####.##",
            ".......#.##...#.#....#.####....",
            "....#...............#..........",
            "##.#.......#.....#......#...#..",
            "......##.....#....#.....#..#..#",
            ".....#...##.............#......",
            "#.#.##.#.....#..#........#.....",
            "......##....#..#........#......",
            "............#........#..#.#....",
            "##.......#......#...####..#.##.",
            "..##..#...#.............#.##...",
            ".....#..##......#.##......###..",
            "............#........#........#",
            "#.#.#.#...#.#.....#.........#..",
            ".........#...............#.....",
            ".............###.#.......#....#",
            "###.##..#..#..........#....#...",
            "#......#...#..#..#.....#.##....",
            "............#....#....#..#.....",
            "..#.#....#...#......#.#..#..##.",
            "...#........................#..",
            "#.#...#..........#......#.#....",
            ".........#................#...#",
            "##.....#....#........##.......#",
            "#...##........#...#...........#",
            "...#...#..........##.......#.#.",
            "..#.#.#....#......##...........",
            "...#.#...#.##.#..#.#.##........",
            "#....##.....###..#.......#.....",
            "###.....#.#.#...#..#.........##",
            "..#......#..###...#.#.#.....#.#",
            ".#....#.....#............#..##.",
            "....#....##..........#.....##..",
            "#...........#....#...#..#...##.",
            "..#.......#.....#..........#...",
            ".#..#................#......#..",
            "..#......#.#...#..#.#....#....#",
            "...#..#...###..#..##....#.#....",
            "..#..............#.....#.......",
            "...#.#...#.........#.#.........",
            "##......##...........##.#.##..#",
            "..#..##..#....#.#......#.#...##",
            "...#.###....###...#.....#......",
            "#.#................#......#....",
            "..#.....#.....#....##.......#..",
            ".#.#...............##..#.......",
            "...#....#.......#.#.....##..#..",
            ".........#....#.......#.#...##.",
            "#....#......##.#.........##...#",
            "#.............#..##.#.#..##....",
            "...#....#..#...#....#.#.#.#...#",
            ".#....#....#..##.....#.#...###.",
            "##............#.#...##.#..#.#..",
            "##.#....##.....#..#..###....#..",
            "##....#................##......",
            "...##..#...#..###....#.....##..",
            ".#...##......#..#.#.....#...#..",
            "..##......##...#.##.......#....",
            "......#.....#.....##........#.#",
            "##....#...........#............",
            "#.......#....#..#.##..##.#..#..",
            ".#....##.#.....#..#..#.........",
            ".#....#.#.#...#.....##.....#.#.",
            ".......##.#.#........#......##.",
            "##........#.##.......#...#..#..",
            "...###..##....#.#....#.#.......",
            "......#.......#...##.....#...#.",
            "..#......##.#......#.....#.....",
            ".....#.....###...#.............",
            "#...#.#...#...#..#......#......",
            "#.....#.......###.#....###.#...",
            "...#.......#....####....##..#..",
            "#.#.....#....#........#.......#",
            ".........#.......#......#.#...#",
            "..##....#.....##...............",
            "..........#..#.#..#......#.....",
            "..................##...##.#....",
            "........#.......#...#..#.#.#...",
            ".....#.#..##..#..#.#..#.......#",
            ".....#........#..#..#....#....#",
            "##............#..#..#...#....#.",
            ".....#....................##..#",
            "........##.#....###............",
            "##.......#.##................#.",
            ".....###.#..#..#...#....###.##.",
            ".#......#.#....#.....##.#......",
            "...##......##.........#...#....",
            "....####..............#........",
            "#...#.#..##..##.........##.....",
            "......#......#....#..#.........",
            "#.....#.....#.##...............",
            "..#.##..#...##.#.####..#....###",
            "#..#......#....#.##..##...#.#..",
            "#....#.......#.....#.....#.#...",
            "##.......#.....##...#.....#....",
            "...#...##..........#..##..##..#",
            ".###..#..##...#....#...#..#....",
            "......##..###.......###...#....",
            "....#...#.#.......#.##...##..##",
            "#.#......#..##.#.#..#..#..#....",
            "......#........#.......#.......",
            "..........#.#.....##...........",
            "......#..#........#..#.#..###..",
            "##..#.............##..#........",
            ".........#....#.....#.........#",
            ".....#..##...#..#..##.##......#",
            "###..#...........#.......#....#",
            "...............#....#.#........",
            ".##.#...#.#........##....#.....",
            ".##.###...##..###....#...#...#.",
            ".##..#....#.#.#...#.#.#.#...#..",
            ".###.#...#.......#....#..#.....",
            "..#..#.#.#.#........#.....##...",
            ".#.......#.#...#.#...........##",
            "...#.....##....#.....##...#....",
            "................#.....####...#.",
            ".#.#......#.......##...#.##....",
            ".###.........#.#......#..#.#...",
            "#......#...#....#..##.......#..",
            ".##..#....#..#...........#...#.",
            ".#...#.......##........#.##....",
            "..#...........#...##...........",
            ".....##....##......#....#..#...",
            "#......#.#...#.##.#...##....#..",
            "#....................#...##...#",
            "..#............#........#......",
            ".............#.........##.....#",
            "...#...#......##.#...#...#.#...",
            "..#...#.#.................#....",
            "....##...#....#...###.##......#",
            "...#....#...#..#...#....#.....#",
            "...##.#........#..#.........#..",
            "..##.....#..##...#.....##...#..",
            "#.........#.#.#...#......#...#.",
            "#.#...........#...#..#..#..##..",
            "..#..#..##....#..........#.###.",
            ".....#..#....#.#...#...#..#..#.",
            "###.....#..#.................#.",
            ".#..##.##.#......#....##..#...."
        };
    }
}