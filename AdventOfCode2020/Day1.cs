using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 1: Report Repair")]
    public sealed class Day1
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(73371, ExpenseChecksum2(Day1Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(514579, ExpenseChecksum2(Day1SampleInput));
        }

        [Test]
        public void Part2()
        {
            Assert.AreEqual(127642310, ExpenseChecksum3(Day1Input));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.AreEqual(241861950, ExpenseChecksum3(Day1SampleInput));
        }

        private static int ExpenseChecksum2(IReadOnlyList<int> expenses)
        {
            for (var i = 0; i < expenses.Count; i++)
            for (var j = i + 1; j < expenses.Count; j++)
            {
                var e1 = expenses[i];
                var e2 = expenses[j];
                if (e1 + e2 == 2020)
                {
                    return e1 * e2;
                }
            }

            throw new InvalidOperationException("Can't find checksum");
        }

        private static int ExpenseChecksum3(IReadOnlyList<int> expenses)
        {
            for (var i = 0; i < expenses.Count; i++)
            for (var j = i + 1; j < expenses.Count; j++)
            for (var k = j + 1; k < expenses.Count; k++)
            {
                var e1 = expenses[i];
                var e2 = expenses[j];
                var e3 = expenses[k];
                if (e1 + e2 + e3 == 2020)
                {
                    return e1 * e2 * e3;
                }
            }

            throw new InvalidOperationException("Can't find checksum");
        }

        private static readonly int[] Day1SampleInput =
        {
            1721,
            979,
            366,
            299,
            675,
            1456
        };

        private static readonly int[] Day1Input =
        {
            1322,
            1211,
            1427,
            1428,
            1953,
            1220,
            1629,
            1186,
            1354,
            1776,
            1906,
            1849,
            1327,
            1423,
            401,
            1806,
            1239,
            1934,
            1256,
            1223,
            1504,
            1365,
            1653,
            1706,
            1465,
            1810,
            1089,
            1447,
            1983,
            1505,
            1763,
            1590,
            1843,
            1534,
            1886,
            1842,
            1878,
            1785,
            1121,
            1857,
            1496,
            1696,
            1863,
            1944,
            1692,
            1255,
            1572,
            1767,
            1509,
            1845,
            1479,
            1935,
            1507,
            1852,
            1193,
            1797,
            1573,
            1317,
            1266,
            1707,
            1819,
            925,
            1976,
            1908,
            1571,
            1646,
            1625,
            1719,
            1980,
            1970,
            1566,
            1679,
            1484,
            1818,
            1985,
            1794,
            1699,
            1530,
            1645,
            370,
            1658,
            1345,
            1730,
            1340,
            1281,
            1722,
            1623,
            1148,
            1545,
            1728,
            1325,
            1164,
            1462,
            1893,
            1736,
            160,
            1543,
            1371,
            1930,
            1162,
            2010,
            1302,
            1967,
            1889,
            1547,
            1335,
            1416,
            1359,
            1622,
            1682,
            1701,
            1939,
            1697,
            1436,
            1367,
            1119,
            1741,
            1466,
            1997,
            1856,
            1824,
            1323,
            1478,
            1963,
            1832,
            1748,
            1260,
            1244,
            1834,
            1990,
            1567,
            1147,
            1588,
            1694,
            1487,
            1151,
            1347,
            1315,
            1502,
            546,
            730,
            1742,
            1869,
            1277,
            1224,
            1169,
            1708,
            1661,
            174,
            1207,
            1801,
            1880,
            1390,
            1747,
            1215,
            1684,
            1498,
            1965,
            1933,
            1693,
            1129,
            1578,
            1189,
            1251,
            1727,
            1440,
            1178,
            746,
            1564,
            944,
            1822,
            1225,
            1523,
            1575,
            1185,
            37,
            1866,
            1766,
            1737,
            1800,
            1633,
            1796,
            1161,
            1932,
            1583,
            1395,
            1288,
            1991,
            229,
            1875,
            1540,
            1876,
            1191,
            1858,
            1713,
            1725,
            1955,
            1250,
            1987,
            1724
        };
    }
}