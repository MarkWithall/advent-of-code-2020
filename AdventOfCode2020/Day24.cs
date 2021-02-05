using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 24: Lobby Layout")]
    public sealed class Day24
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(326, CountBlackTiles(Day24Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(10, CountBlackTiles(Day24SampleInput));
        }

        [Test, Category("slow")]
        public void Part2()
        {
            Assert.AreEqual(3979, CountBlackTiles(Day24Input, 100));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(15, CountBlackTiles(Day24SampleInput, 1));
                Assert.AreEqual(12, CountBlackTiles(Day24SampleInput, 2));
                Assert.AreEqual(37, CountBlackTiles(Day24SampleInput, 10));
                Assert.AreEqual(132, CountBlackTiles(Day24SampleInput, 20));
                //Assert.AreEqual(259, CountBlackTiles(Day24SampleInput, 30));
                //Assert.AreEqual(566, CountBlackTiles(Day24SampleInput, 50));
                //Assert.AreEqual(1106, CountBlackTiles(Day24SampleInput, 70));
                //Assert.AreEqual(1373, CountBlackTiles(Day24SampleInput, 80));
                //Assert.AreEqual(1844, CountBlackTiles(Day24SampleInput, 90));
                //Assert.AreEqual(2208, CountBlackTiles(Day24SampleInput, 100));
            });
        }

        private static int CountBlackTiles(string[] input, int days = 0)
        {
            var tilePositions = input.Select(p => ReadPosition(p).ToArray()).ToArray();
            var tileCoordinates = tilePositions.Select(FindCoordinate).ToArray();
            HashSet<(int x, int y, int z)> blackTiles = new();

            foreach (var coordinate in tileCoordinates)
            {
                if (blackTiles.Contains(coordinate))
                {
                    blackTiles.Remove(coordinate);
                }
                else
                {
                    blackTiles.Add(coordinate);
                }
            }

            for (var day = 0; day < days; day++)
            {
                HashSet<(int x, int y, int z)> nextDay = new();

                var minX = blackTiles.Min(c => c.x) - 1;
                var maxX = blackTiles.Max(c => c.x) + 1;

                var minY = blackTiles.Min(c => c.y) - 1;
                var maxY = blackTiles.Max(c => c.y) + 1;

                var minZ = blackTiles.Min(c => c.z) - 1;
                var maxZ = blackTiles.Max(c => c.z) + 1;

                for (var x = minX; x <= maxX; x++)
                for (var y = minY; y <= maxY; y++)
                for (var z = minZ; z <= maxZ; z++)
                {
                    var coordinate = (x, y, z);
                    var blackNeighbours = CountBlackNeighbours(coordinate, blackTiles);

                    // Currently black
                    if (blackTiles.Contains(coordinate))
                    {
                        if (blackNeighbours == 1 || blackNeighbours == 2)
                        {
                            nextDay.Add(coordinate);
                        }
                    }
                    // Currently White
                    else
                    {
                        if (blackNeighbours == 2)
                        {
                            nextDay.Add(coordinate);
                        }
                    }
                }

                blackTiles = nextDay;
            }

            return blackTiles.Count;

            static IEnumerable<string> ReadPosition(string positionString)
            {
                for (var i = 0; i < positionString.Length; i++)
                {
                    if (positionString[i] == 'e' || positionString[i] == 'w')
                    {
                        yield return positionString[i..(i + 1)];
                    }
                    else
                    {
                        yield return positionString[i..(i + 2)];
                        i++;
                    }
                }
            }

            static (int x, int y, int z) FindCoordinate(string[] position)
            {
                var coordinate = (0, 0, 0);

                foreach (var move in position)
                {
                    var offset = move switch
                    {
                        "e" => (1, -1, 0),
                        "se" => (0, -1, 1),
                        "sw" => (-1, 0, 1),
                        "w" => (-1, 1, 0),
                        "nw" => (0, 1, -1),
                        "ne" => (1, 0, -1),
                        _ => throw new InvalidOperationException("Unknown move")
                    };

                    coordinate = Add(coordinate, offset);
                }

                return coordinate;
            }

            static int CountBlackNeighbours((int x, int y, int z) coordinate, ISet<(int x, int y, int z)> blackTiles)
            {
                (int x, int y, int z)[] neighbours =
                {
                    Add(coordinate, (1, -1, 0)),
                    Add(coordinate, (0, -1, 1)),
                    Add(coordinate, (-1, 0, 1)),
                    Add(coordinate, (-1, 1, 0)),
                    Add(coordinate, (0, 1, -1)),
                    Add(coordinate, (1, 0, -1))
                };

                return neighbours.Count(blackTiles.Contains);
            }

            static (int x, int y, int z) Add((int x, int y, int z) a, (int x, int y, int z) b)
            {
                return (a.x + b.x, a.y + b.y, a.z + b.z);
            }
        }

        private static readonly string[] Day24SampleInput =
        {
            "sesenwnenenewseeswwswswwnenewsewsw",
            "neeenesenwnwwswnenewnwwsewnenwseswesw",
            "seswneswswsenwwnwse",
            "nwnwneseeswswnenewneswwnewseswneseene",
            "swweswneswnenwsewnwneneseenw",
            "eesenwseswswnenwswnwnwsewwnwsene",
            "sewnenenenesenwsewnenwwwse",
            "wenwwweseeeweswwwnwwe",
            "wsweesenenewnwwnwsenewsenwwsesesenwne",
            "neeswseenwwswnwswswnw",
            "nenwswwsewswnenenewsenwsenwnesesenew",
            "enewnwewneswsewnwswenweswnenwsenwsw",
            "sweneswneswneneenwnewenewwneswswnese",
            "swwesenesewenwneswnwwneseswwne",
            "enesenwswwswneneswsenwnewswseenwsese",
            "wnwnesenesenenwwnenwsewesewsesesew",
            "nenewswnwewswnenesenwnesewesw",
            "eneswnwswnwsenenwnwnwwseeswneewsenese",
            "neswnwewnwnwseenwseesewsenwsweewe",
            "wseweeenwnesenwwwswnew"
        };

        private static readonly string[] Day24Input =
        {
            "swsenenwwneeseswswsenwnwenewenwnwse",
            "wnewnweswswsenewnenwnwseseweeseswne",
            "swwnewwswswswswswswsweeswwwswnwew",
            "seeneeenwweenwnweswwseeeee",
            "swswseseseswsesesesenwswseswsese",
            "seseseneswwnesenwseseswwnenwseenwsesw",
            "nwseseswswenwnwswswneseswswseswseswswswnwse",
            "enwsenwnewweeeswneenenwesweneswene",
            "nwnwnwneswsenenwnwwsenwswnewnee",
            "sesewsesenwseeseneseseseswswseswswsese",
            "swsenwnwswnwnwneswwnwnenwwwnwnwnenwnw",
            "eseswwwwswwwnewwwwwwswnewsww",
            "neswenwenwswseenwseeneenweenenee",
            "esenwnwwsenwsenenwsenwwnwnwewnwnwwnw",
            "nwswesweseseeeeeeeesenese",
            "neneeneesewsewswenenwswnw",
            "seeneneewneneneneweeneswsenenenee",
            "enwwneeeswneeenweseeeeseneenee",
            "swwswsesesesewneseseseeseseseseseene",
            "nwsewwweswnesewwneseeswwwenwwnw",
            "nwnenenenwnwswswwnwwnewneenwnwsenenenwse",
            "nwsenwnwnenwsewnenwwwnwnwnwwnwwnww",
            "newsenewwwwwwswnwwsesenwwwwwse",
            "senweeneeneneeneneeswnenenenewenewe",
            "wwnwneswsewnwnenwesenenwneneneneneenene",
            "senwnwwwnwnwnwnwnwenenwnwnwswnwnwnwsewnw",
            "eenwnenenwseneneneswwneeeseseeseenwne",
            "nwswewewneenwsweneeweneneneee",
            "swwnwseswswswnweewnwwesw",
            "wnwwwwwwwwewwwnw",
            "swnenwswnenenwnwnwsewnenwnenenwnwsenenw",
            "ewwswswswneswnwwswwwsewwwnwwswsw",
            "wwseswneswwwwneswwesenwwwww",
            "neswswwenwswswswnwswseswswswswswswewswsw",
            "nwswnwswswswswswswseeswswwswswswswswesw",
            "senwwswwnwnenesesewwnenwswwnewwwse",
            "neneneneeneswsweeneneneneswneeenwsenw",
            "swnwneswswwseswswswswswswsweswwsww",
            "wsenewewenwwwwnwsenwnweww",
            "swnwswswsenwswswnwswswswswsesweswsweesw",
            "nenenwnenenenenenwneneswsewnwnwnwsenwnee",
            "eseeswnwseneseenweeesenwweenwnwsw",
            "eeeswnenenewenenenewneneeeneeese",
            "swswswnwseeseswswseseseenwswswse",
            "swseneseesewnwwnwsenwnewnewneseww",
            "swwwnewwwsenwwnwswwewnenwnesww",
            "swseewseeeseeesenwsenesenweeeswe",
            "nwnwnwnwweswnwnwnwnwwswnwnwnwnwe",
            "nwwnenwnwwnwnwswsenwnwnwwsenesenwnwne",
            "seeseseeswseneeeseeseneenweesw",
            "wneswnwswnenenenenwnenenenenesenenwnwnw",
            "eseseseswewseseweeneeeeswneese",
            "nesenenenenenwswnenenwnenwneswnenenenene",
            "nwwnwsenwnwnwwnwwnwnwnwesewesenenwnwnw",
            "nwnewnwseweneswnwwnw",
            "senenesenwneewswewnenenwnenenenwnwne",
            "eswwseswwswnwswswwwswswnewswswswsw",
            "nenenenwneenenesenwsweseneesweeneene",
            "wseneneenewnenenesewnweweneneesw",
            "eeeeesenweweeeeneeswnewewene",
            "nwneneeswwneseeswneneneneenenenenwnwsee",
            "neswseneneenewnewwesweswnee",
            "senwweneswwwsenenewnenwseewwsesww",
            "nwseneeenwnewsweeeneeeeenenee",
            "nwsewnwesweswenwseswneseesese",
            "swseneswnwneneseseswswswswswswswswswswsewse",
            "nwseneseeseswswswseswesesewswsewwne",
            "enwseewsesenwsewnewwnenenenenenwsw",
            "swnwwswsenwewswswwsw",
            "wwewwwwnwwwnwwewwwwsw",
            "nwnewnwnwwnwnwneswnwnwneswsenww",
            "wswnewwwswswswwwnewweswwswwse",
            "swnwweesenewwnesenesenenwwseneene",
            "nwnenwwesenwsewnenwnwwnwnwnwsenw",
            "newnwwnwsenwnwnwsewwnewwsewwsew",
            "swneseswswswsweswswswwswswsenenewswswsw",
            "swsweseswswseseswnweseswwnesesesesesenwsw",
            "neneneneneswneeneeewnenewneene",
            "sewsenwneswewwnewswwswswwwwswwww",
            "eweeeseeesesesesenwese",
            "wnwseswnewseneenwweneeswweeesene",
            "sesewseseseesesesenwsweesese",
            "ewnwnwsesenwnesewewswnwnwneswnwwww",
            "nwwwnenesewnwnwwwnwwwsww",
            "neswwwnenwseewsesweseeene",
            "seseswesenwswseswsese",
            "senwweseseseseseswnwseswswswsweeswse",
            "swwswswwwwsenenewwweswwnwswswesw",
            "eeeenwneenwnewswnenenewswnenenee",
            "swwnewswswswswneswswswwsw",
            "seeswnewswwwseeenwwswswnew",
            "wnwswswswswswneseswswswswwswnesweswswswsw",
            "nenwnenwnwnwnwneswnwnwneseneeneswnwwne",
            "swneneeeeeesweeeswenweeneenee",
            "seseneswnwsweswseswseswsenwse",
            "swewewnweeswwswwewsewnwnwnew",
            "eeeeweeneeeeeneeneseneswesw",
            "wwswwnewwwswwsewnwe",
            "wswswwnwswsweswseswsewwsenwnwwnenene",
            "neeeeswnewseesesweeseseeneesee",
            "nwnwnwnwnwsenenwwnwnwnwwnwnwnenwnwesenw",
            "swseseswneswseswnwseswswswsese",
            "wwswwwwnwnwnenwnesenwswwnwswnwnenw",
            "wwswswseswwswwswswwwne",
            "nwnwnwnwwnwseneesewnwnwswweneewesw",
            "nesesweeeneeeseseneseseswwseeesese",
            "eesweeseenwweeweneeesene",
            "seseneseseseswseswsenwewsesewwewese",
            "wsweseseenwseseesewneeeesesesenwese",
            "wnwnwenweneswnwnenwnwswnwnwnenwwnwsenwnw",
            "nwsenenenenwnwnwwnenwnene",
            "eeenwnesenwseswsesenwesesweseseswe",
            "nwnenwneswnwnwnenwnenenenwnwneswnweswnw",
            "weweeeeeeseesenwneneweeeee",
            "eewsweeenweeeseenweseeswneene",
            "swswwswswnwewnewswseeswswswswwwnesw",
            "wsewwswnewwneewwwwsewwenewww",
            "nwswsesweneenweewweeseeewsewne",
            "nwseswseswswswswswswnweswswsweswswswswnwsw",
            "senwnwwnenwnwwsewwwnenwsenwsenwnwww",
            "swswenwswswsewneswswswwswwnwswenwwsw",
            "nwewwwwsewsenwnwswwwnenesewnwnw",
            "neeesweseeeeneeesewsewwsenenesesw",
            "sewswswswneswneneswneesewswswwnewsenw",
            "wsenwneneneneswnewswseswsweswwwswesw",
            "seswseseswnwenwswseseseseenenwseseesesesw",
            "enwneweeeeeswe",
            "seeswesenewneeseenwsewnwsenesenwsew",
            "wswwnwswwweneswwswswswswnwwenesw",
            "newnwsenenenwenwwwnweneeswnwnwnenenw",
            "nenwnwseswseneswseswseeseeseenwwswese",
            "newenewswwwwwnwwsesw",
            "wsewwwnwwsesewwwwnewnenwsewww",
            "seseseswseseswnenwneseseseseswsewseseswnw",
            "swswsweswnweswwseswswswswenenwwswsww",
            "nwnwnwnwwweswnwwwwnwswenwwnewewnw",
            "nenenwnwsenwnenwnwnwnwwnenwnwnw",
            "seseseswsesenwsenesewseswseswsesesesenw",
            "neeswnwnwswnweeeneneeswneesw",
            "nenwsenenenwswnwnenwnenenwnenwnw",
            "nwwseseseseswsweenesenwseswseeeseese",
            "eneeenewneeeneneneee",
            "neeneneswnewnwnwnwnwnwnwnwnwnwnewnenwse",
            "wewseeseseenweesenwseseeswsesese",
            "wwnwnwwsesewseeewswewnwnwwne",
            "wseswwswnwnwsesenweenwnweeswnewswsw",
            "eewweeswnesesewweneswnesesenwsw",
            "nwnwnwewnwnwwwwnwswnwwnenwse",
            "swnwswneswnwseeswseneswsesesesenw",
            "sweeeseesenweneeeswseseenwwsenenw",
            "nenesenesweswneesewswneeswwwswsww",
            "eenesewnwwswsenewwnwnenwsenenwenw",
            "swnenwswnenenewnenesenenenenenenwnwenenw",
            "seeseseeswneeewseewnwseseseeseee",
            "seewwweweeneeseneeneeeenenew",
            "nwnwnwneswnwenenwswnwwnwnwnwnesweenwnw",
            "swsesweswwswwwneneswswwswswseswnwsw",
            "seswswswswsweneneswwseseseseswswseswwse",
            "nweeseeeseswneeewesesenenw",
            "swwneseseswswswseneswneswseswseseseswswse",
            "seneweswnwweseseseeeneswsesesenwse",
            "wsesenwswsesweweesesenwswnesewsw",
            "eeseeeeeewswenweneesweeee",
            "swnwnwswnwnwenwnwnenenwneenwswnwnwnwnwnwnw",
            "sesesenesesesesesesesesesesenwsenesewwsese",
            "nwnwnwnwnwnwnwnenwnenwnwnesenww",
            "nwnwnenwewwswwseewewwswswswwwsw",
            "senwwseeseswneswswseswsw",
            "nwnweneseseseseseswswswseswseswsesesee",
            "seeswenweeeeeseeseeee",
            "seswseseswseeenwwnenwesesweseswnwsw",
            "seseseseseeseweeeeseesewese",
            "swneneenenenwnenenwnenenenw",
            "wsewnwnwnwwnwwenw",
            "esenweeeeeswenwsee",
            "senenwwenwnwwnwswwwnwnweenwnwnwwsw",
            "eeeeeeenweseeeese",
            "swnwwsewenwsenwewwnwnwnwenwnwwww",
            "neneeewswenenesenenenwnwnenewnwnene",
            "enwwnwwwwsenwneswnwwwnwnwsewnwwwe",
            "sewswswwswseseseseseseseesesenesenwsw",
            "eseseswseesewnesesesesenwesesesese",
            "seeesenwnwnweewswnewseeneeneswne",
            "wwwwwwswseewwwnwsewneswwww",
            "nwswwewnenwswnwenenenwwnweeenenwne",
            "wwneeeseswseswswnwsesenwneswswnesesw",
            "eeeneneneeneeneneeww",
            "nenwwwnwsenwnweswwseewnesenwnwnwne",
            "nenewneswwwewnewseewwsewsesw",
            "neneswneswwsewneesewwnenwneenenese",
            "eswenesewwnenwwsewwswneswnwswwne",
            "eswenwseeenesesewsweenweseeesenee",
            "nwnwnenewnenenwnenwsenenwneneneswenwnw",
            "neenenwswneneswnwnwewnwewnenwneesew",
            "nenenwnweneneswnwnesenwnenwnwnwse",
            "wneseseeswseswswswswesenwnwsesesesenwnw",
            "neneeeeneneneneswneenenenenenw",
            "wwswnwwneeewwesewwwwwsenwnww",
            "nwswswnwseseswswnewswswswswswwswswswe",
            "wnwewnwnwneneeseneeesewswneneewse",
            "wwsewseewwwnewsenwwwwwesenwnw",
            "swwnesenwsenwwnwesenwnenwwnwsewnenwne",
            "eeesewswenwsweeewesewnwneese",
            "newseswswwswwwsewswsewwswswnewnwsw",
            "wnwnwnwwnwenwenweswnwnwswnwnwnwnwnwsw",
            "nwenwnenwsesewwwneewwwwwseswnwse",
            "sesweeneenwnwswenwwseseneeenwswese",
            "weseseesesewsweeenwneneeseeeene",
            "swsewswsweseswswswseeewnwseseswsesw",
            "eneswnesenewnwnenenwnwnweswwswnwswnee",
            "nwswewwseswwsweewseseneesenwnesw",
            "eswseseseenwnwsweeesewewnesenwse",
            "swswwnwswnwweswnewwwswseswwwswww",
            "seenwwneeeeswenwswseeeeenwene",
            "swnewwnewwwswswwwswweww",
            "swseneseswseswneweseswswesenwne",
            "wsewwwwwwnwwwnwnwwwsewnewwe",
            "eeeseneswneenweswnwneeneeenwnee",
            "nwnenwswsenenenwnwnwnwswnwnwnwnwnwnwesw",
            "senwnwwsenenenwnewnwnenwsenwnenenwnwnee",
            "neswswswswwneeswswnewsenenwesewswsee",
            "nwseeeeeseeswwenweeneseseeeee",
            "seseseseswswseswsenwnwewwseseseenesee",
            "esweeeenweese",
            "eneneneweneneeeseeeneenwe",
            "senwnwsesesesewswswswseesesesenenwsesw",
            "senewesweswswnwnwswswswswswswsw",
            "neseseesewswwsenweswnesesesenwenwsww",
            "swnwswswswswswsesesesesenwseswswsw",
            "eseweswneeswwsesesewsenwsesesesenese",
            "neneenweeeeeesweeeeeenwesesw",
            "neeeneeswenwneeswenwe",
            "wnesweneseswwseswswwnesewswnwnewne",
            "eseweseeneneeeeneeseneswswswsese",
            "ewnenwnwneneenenwnwswnwnenwnwnwnesenwnw",
            "enewswneneneneneneenewne",
            "seswnwswwswswsenenewswnwseneswenenenew",
            "enwewwenenweseweeeesweeeswne",
            "eneneneeswnwswneenenwneswnenesweeswee",
            "wnwnesenwwwnwnenwswwnwwnwwseswnwenw",
            "wwnwwwwwwwwswwwsew",
            "enwneneneneneneswneeswseneeenwneene",
            "sesesesesenwseseeseseseswneseswwswswsenee",
            "swneswnwneenesweeneswsweneeneenenwne",
            "nwnwnenwnwswnwnwnwsenwnwnenwsenwwsenenese",
            "neeneseswseeneweweneweeseeneenw",
            "wwswwswnwswneswswwnwseswwwnwswewse",
            "wswwswwswswswwweswsww",
            "swswswnwneseswswsewswswseseneswnwseeswswse",
            "nwwwwnwewwwnwwww",
            "swseenenwewnweswsenwswwseseesesese",
            "sesewneseseseseseenwneweswseenwswsese",
            "nenwnwnenenwswnwnenwnwnenwswneenwseene",
            "neswwswsweneswswwswseswswwnewnesww",
            "swswseseeseesenwsesesenwnwneseeseese",
            "wwnenwnwnwwwwnwsenwwwseweesenw",
            "nwenenwnwsewnwnwnwneewnwwswswww",
            "nenwsenwswnenwnwswnwneneswnwnwenwnenwnw",
            "neneneenenweseewnenene",
            "nwwswsenwseseseeenwnwsesenwenwesese",
            "swswswswswswswswswswneswsewswse",
            "swnwnwnwnesenenewnwnwnwnwnwnee",
            "nwswsenesewwswswswneswswswseseseswwe",
            "swsweswwwneswenwnwewwwwese",
            "ewnewseeseseewseesewseneeenwese",
            "neswenwseeeneswnwwnwwnwseewwswnw",
            "nwsenwsesenwnwnewneswneseneneeenenesw",
            "nenweswnwneswnwwwnwswnwsenenenwnwnwsww",
            "eeeeeweswseseseenwnweeneesee",
            "seseseseswwneswseswsesesese",
            "swwswswswwwnwwwwswwnewwseeswnw",
            "eseseswswseswnwseseswsweswswwseweswse",
            "nwnwnwnwnwwenwwseswnww",
            "neneneneneenwnenenenenwswnene",
            "sesewewnenwwnwsenwswnene",
            "swsenwsweneneenewewneenwwneewne",
            "swnesesesenwseneswnwnenenwnenwswnee",
            "seeswnwnwsenenweneneneneneneneneenew",
            "wneewsesewsesenenwsenewsewwsenenese",
            "nwenwswnwseeswseneseswneseswswseswseswsesw",
            "wsweswneswswswswswswswswswswnweswswsw",
            "seseeeseeeseewse",
            "swnenenwsenwnenwnesewneewneesesenwnwnwne",
            "nwenwsewnwnwenenwnenwnwswnwwwnwnwswe",
            "nwenwswseswwswseseswsweswswswswseswswne",
            "wsweeeneewseweneswnenenwneenenenene",
            "swwswswneseseswseswswswswseswneswswsw",
            "eweswswnenesenwswenwseneneswneswswnene",
            "nwewwnwnwnwenenwswnwsewnwnwswnwnwnw",
            "neeneneneneeswnwneeeeeneweenewne",
            "seseeseeneseseeseseweseswnwsesesese",
            "eeeeewewee",
            "swseneseeenwseewnwseswnenwswnwwnww",
            "seseneswswsesenwswseseswseseswesese",
            "nenenewwswseneeswneswsenenwnesew",
            "nesenenenwnenwnenenwnewenenwnwnewene",
            "eseseseeeweeneseeeseswsese",
            "enwwwwsewwwnenwswsenwnwnwnwnewnwwnw",
            "nwwswwswwnenwwwwwwnwneseewsee",
            "neneswnenenenenwnenenenesenewenenenene",
            "wwwneweswwwswwwwwneswswwsww",
            "neneneneneeenenwneneswnenenenene",
            "seseseseswseseseseseneseseeneswenwswsese",
            "eeeswneswseeseswswseeneeneeenwe",
            "nenenwnwnesesenwnenenwnwnwenwswnwwwnwne",
            "nwseseseeeseseseeeswnwsese",
            "eseseeseenwwseweseseesewseseseee",
            "enwneswwsenenenenenenenwwnenesenenenenene",
            "nweweswnwwnewwnwnwwnwwnwwswwe",
            "wneswswneswswswsewwwswwnwsewswww",
            "nwenwswnwnweswseswsweswswswswsw",
            "seseseewsenwswnweswsenwew",
            "swwwenwnwnwwwwwwnewwnwnwsewwew",
            "swnwwsweswsenwsenewenewneseeenese",
            "nesenwnwnwnwnenwnwnwnwnwnwsese",
            "nenwneenenenenenenenewnenwseswswnenene",
            "enweneesesweeeeswenwenwenweeesw",
            "wswnwnewwwwswwwnwwseesesww",
            "nwwwnenwnwsewewswnweswww",
            "swswswswnenweswswswswswswneswswswneswsw",
            "nwsewswswswswswswnwweswswswswswswswnesw",
            "senwnwnwsenwwwwnwnwnenwnwnw",
            "senenewenwsesenesewswsewneseseswswnewne",
            "wnwwnwenwwwwnwwnwwwnwnw",
            "nweseenwseseseneseesesesenwsesesewsw",
            "seseeeseseswnwsesese",
            "wwwwnwnwewwwwwswsewwnewww",
            "neneeeeeeeeweeene",
            "eneneneneneswsenenenweneseeenenwnewne",
            "wesenewseswswswseewswnesewseneswsw",
            "eseeseesesewseswsesesesewnwsesesee",
            "nweenwseseeseneeenewsesewswwse",
            "nwnesenwnenwwnwneenwnwnenenenwnwswnwne",
            "seseseseseswnenwsesesesesesesesesesenesw",
            "neneneneneenenewneneneneneneneswnwwe",
            "nwwswnwnenwnwnwsew",
            "swwnewwwnwneenwwwwnwsewswwww",
            "swswwnwswswwswwswswswse",
            "nenenwnwnwneeneneenewnenwnwsenwnwswsw",
            "swswswneewsenewswnw",
            "newneeeneseeenenwneeeeewneee",
            "seswseswnwsewseseswsesesenwswseneseesesesw",
            "nenwwnweeswnenwsenenenwwesesenww",
            "nwswseswneswswswswswnwswse",
            "weseeeseseweeseeseseenenwseseee",
            "nwwwwswewswwswsewnwnwnenewewse",
            "wwswnewwwwwwwwwwww",
            "wnwwnwewswswesenwewnwwwnwnwwwne",
            "nenwnwnwnewwnwnwwswnwnwenwnwnwnwsenw",
            "nenwnwneswseswswneeneeswnwnwswnwseswnenw",
            "senwwseneneeseseesenwwseeswseeeseene",
            "senwneenwseenwnwwwneneneenwnwnwnwsew",
            "sesweseseswnwswswnwswseseseseeswseseww",
            "nwwwnwwenwnwnwnwnwnwnwsewsenwwswew",
            "swneswwsewsewseeswswnenwswwwneswnenw",
            "enweswneeseswsesweenwneswnwneenwewe",
            "ewwswnwwwwwwwnwwenwnwnwwnw",
            "nenenwnenwnwnwnwnwnwse",
            "swnwsweneswneeesenwswewnwneseeesw",
            "eewnwnwseswenwswsewnesesesenwnese",
            "nwnenesenesenwwnenwnwnwnwenwnwwnwswnwnw",
            "nwseswnwesenwnesee",
            "newseswsenwnwsewswneneenewnwwnwne",
            "eneneeewenenenwneeeenesw",
            "wswswswwneswswwswwsw",
            "wswseseswwnwnweenw",
            "wwwwwseswwwenesw",
            "neenenwenenenwnesenwneenenenwwswnesw",
            "swswneswenwswswswwswsesweeswswswsenw",
            "sesenenwsenwnenwneswnwnwwswnwnewnewnw",
            "nwewwnwnwwwnwswnwenwenwswewnww",
            "neeneneneeswneneneneswnenwnenenw",
            "nesewewewsesenenenweenenesweswsenw",
            "wnwwseswnwwswnweneswewnwnwnwwnwe",
            "eeswnweeeeseeseeewneeeenweee",
            "wswwswsenewsesewewnewsenwnwwwnw",
            "wswesenwnwneswnwe",
            "wwwwwnewwwwswnewwsenwnwnw",
            "swswsesesesesesenwesesenwse",
            "nwnwnwnwswnenwwsenwnwnwwwnenwnwsewnwnw",
            "neswswswseswneswseseswnwwswswnwneswswswse",
            "wswwwnwnwwnwwsewnenwwwneswwwne",
            "seesweeeenesenweeeweeeenwse",
            "senwnwnwnwsenwneneswenenwwnenenenenwnenw",
            "swneneswwneeswwswwsenwwswnwneswsesw",
            "neeswswsweeeneesweeeneeeneeswnw",
            "nwnwseesenwwnwnwsesenwneenwnwnwwnwne",
            "sewsenwwsesewwseeseeswseeeseesese",
            "swneswswswswswnweswswneswswswswsww",
            "swneswswswseseswswswwswneseseswswswsenesw",
            "eswnwwwswneesewwswwwnwwwwswse",
            "nwnenwnwnwnwnwswsenwnwnwswnenwnwnwnwenw",
            "eenenwswnwneswsenwswnwnenwswwnenenwsw",
            "senwswseseseeeeenw",
            "neneneneneswnwnwwnenwnesenweneswnwnese",
            "swnwwwswswswwwwseww",
            "swsesenwswswnweseseneswswswsesesewswsw",
            "nweeeeeneeeseene",
            "esesewswsesesewnwsewseewnenwnesene",
            "neenwseeweeweswswnwese",
            "wnenwnweseneeesenewseeswww",
            "nwnwnwnwnwnwnwwnenwnwnwwnwse",
            "nwnwseswnwnwwwnwnwnwnenwsenwnwwnenenwnw",
            "neewswnenwseseswwnwseseeeseesenweese",
            "seswswseswseseswewswwswneseswswseeswnw",
            "eeeeeesenwswswenenwneeeewsweswnw"
        };
    }
}