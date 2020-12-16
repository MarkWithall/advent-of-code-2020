﻿using System;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 16: Ticket Translation")]
    public sealed class Day16
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(27850, ScanningErrorRate(Day16Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(71, ScanningErrorRate(Day16SampleInput));
        }

        private static long ScanningErrorRate(string[] input)
        {
            var ruleStrings = input.TakeWhile(i => i != "").ToArray();
            //var yourTicketString = input[ruleStrings.Length + 2];
            var nearbyTicketsStrings = input[(ruleStrings.Length + 5)..];

            var rules = ruleStrings.Select(s => s.Split(new[] {": "}, 2, StringSplitOptions.None)[1].Split(" or ").Select(r =>
            {
                var parts = r.Split('-');
                return (long.Parse(parts[0]), long.Parse(parts[1]));
            }).ToArray()).ToArray();
            var nearbyTickets = nearbyTicketsStrings.Select(t => t.Split(',').Select(long.Parse).ToArray()).ToArray();

            return nearbyTickets.SelectMany(t => t.Where(n => rules.All(r => DoesNotMatch(n, r)))).Sum();

            static bool DoesNotMatch(long n, (long, long)[] rule) =>
                !(rule[0].Item1 <= n && n <= rule[0].Item2 || rule[1].Item1 <= n && n <= rule[1].Item2);
        }

        private static readonly string[] Day16SampleInput =
        {
            "class: 1-3 or 5-7",
            "row: 6-11 or 33-44",
            "seat: 13-40 or 45-50",
            "",
            "your ticket:",
            "7,1,14",
            "",
            "nearby tickets:",
            "7,3,47",
            "40,4,50",
            "55,2,20",
            "38,6,12"
        };

        private static readonly string[] Day16Input =
        {
            "departure location: 47-691 or 713-954",
            "departure station: 44-776 or 799-969",
            "departure platform: 37-603 or 627-953",
            "departure track: 41-240 or 259-955",
            "departure date: 42-370 or 383-961",
            "departure time: 50-117 or 136-962",
            "arrival location: 33-86 or 104-973",
            "arrival station: 29-339 or 347-962",
            "arrival platform: 46-644 or 659-970",
            "arrival track: 31-584 or 604-960",
            "class: 42-107 or 115-971",
            "duration: 31-753 or 770-972",
            "price: 40-515 or 525-957",
            "route: 31-453 or 465-971",
            "row: 46-845 or 868-965",
            "seat: 45-475 or 489-960",
            "train: 34-317 or 323-968",
            "type: 47-150 or 159-969",
            "wagon: 45-261 or 279-955",
            "zone: 33-879 or 891-952",
            "",
            "your ticket:",
            "191,139,59,79,149,83,67,73,167,181,173,61,53,137,71,163,179,193,107,197",
            "",
            "nearby tickets:",
            "235,447,575,80,384,832,799,806,529,624,144,398,176,583,199,169,914,222,828,314",
            "336,538,772,909,139,848,117,360,684,551,261,813,162,660,660,672,809,939,352,86",
            "63,509,538,681,226,775,383,301,571,112,105,944,199,355,414,809,474,736,541,567",
            "924,825,298,655,874,917,451,297,329,295,191,311,737,370,527,677,468,573,172,199",
            "171,570,161,83,300,724,653,161,85,388,468,776,84,510,946,138,238,891,941,552",
            "53,742,732,469,208,732,576,643,644,508,227,674,293,342,677,872,443,559,805,468",
            "809,824,330,313,820,419,774,869,556,354,713,838,415,867,727,369,323,201,805,224",
            "716,584,723,925,280,427,693,868,331,571,410,146,510,527,835,826,803,872,937,843",
            "462,450,166,826,195,748,164,338,50,324,68,743,810,445,919,185,547,53,746,472",
            "641,835,946,138,528,324,551,432,673,413,991,874,913,411,115,72,60,807,552,191",
            "673,729,473,904,575,290,920,356,144,54,333,719,935,320,189,494,201,933,449,572",
            "434,813,332,50,842,466,728,534,138,552,831,551,878,497,990,806,571,51,875,424",
            "332,634,892,746,741,776,515,74,367,154,207,671,899,281,735,171,64,200,207,147",
            "935,431,853,147,143,630,385,412,230,718,583,900,349,182,941,800,869,424,933,633",
            "162,913,19,583,170,924,512,297,567,362,914,148,917,69,63,419,164,822,919,556",
            "775,808,541,416,885,501,572,192,140,876,396,58,912,814,404,337,339,509,219,547",
            "514,238,926,944,339,292,223,283,501,515,391,289,386,306,552,90,739,230,800,831",
            "287,147,717,82,509,394,50,747,369,939,731,806,355,384,143,680,425,258,384,279",
            "996,539,665,775,175,731,825,426,661,553,285,445,359,70,198,366,352,832,581,149",
            "505,878,339,387,399,362,71,236,307,470,54,261,576,802,628,321,804,316,937,145",
            "279,288,168,439,578,714,503,227,639,627,383,362,440,645,934,827,900,143,837,310",
            "73,52,323,454,294,552,474,357,445,919,182,716,69,733,166,361,420,77,148,467",
            "992,565,328,348,219,879,239,898,640,928,745,403,259,903,507,205,745,893,913,801",
            "58,685,724,321,927,471,561,773,674,143,897,546,812,557,335,830,726,914,64,536",
            "499,685,441,69,56,420,417,847,879,529,564,200,877,168,234,406,174,745,405,565",
            "898,174,841,665,872,403,453,678,200,351,0,189,327,71,809,391,627,728,661,633",
            "753,921,749,730,359,332,451,514,752,368,464,909,150,324,552,353,348,808,467,545",
            "435,367,934,661,295,222,83,784,367,437,307,902,391,425,434,328,191,928,746,333",
            "179,579,871,104,914,663,644,637,729,344,217,198,216,259,564,905,392,577,469,824",
            "525,639,165,917,740,513,179,163,57,904,432,575,577,347,423,346,366,230,575,928",
            "513,394,504,491,105,453,839,75,438,947,408,585,671,186,538,293,499,727,812,317",
            "679,237,547,199,364,468,146,529,470,490,495,303,141,440,54,586,922,294,820,169",
            "680,311,512,528,65,83,140,676,872,400,81,885,80,511,433,348,195,147,894,831",
            "362,903,240,413,440,237,124,323,876,839,584,327,186,290,925,574,833,69,502,747",
            "820,667,197,496,663,95,944,836,576,84,232,912,281,547,918,308,802,235,549,680",
            "73,680,718,579,580,570,415,526,641,536,94,53,948,295,734,718,470,408,279,659",
            "140,563,390,204,529,284,510,382,662,509,397,525,331,776,675,441,370,913,936,328",
            "390,727,723,347,321,347,640,293,745,584,635,503,736,572,302,310,843,143,753,363",
            "294,368,811,776,103,495,360,713,548,79,394,311,472,449,150,929,428,747,843,810",
            "284,310,751,228,499,995,558,667,669,724,312,296,574,946,581,925,670,915,474,745",
            "551,941,425,826,144,423,542,406,540,792,641,76,452,53,402,772,71,333,162,397",
            "396,563,162,261,304,919,291,203,81,392,892,346,773,163,579,729,300,323,824,633",
            "507,471,827,421,810,944,100,308,207,339,716,575,690,721,334,300,569,283,384,536",
            "840,878,672,76,675,825,677,79,944,495,508,198,936,923,214,611,213,218,160,948",
            "260,413,839,449,672,920,81,542,218,212,850,740,141,58,402,820,539,144,830,195",
            "434,282,166,231,584,427,419,362,561,182,886,72,827,451,171,327,826,914,432,900",
            "76,845,742,921,647,191,834,84,567,365,360,55,830,489,815,511,177,564,306,559",
            "512,401,670,325,825,233,385,209,446,846,894,925,64,580,723,260,289,453,515,825",
            "369,284,662,226,179,370,980,910,467,187,553,221,358,223,393,917,738,544,295,386",
            "367,409,214,930,624,302,364,920,354,841,916,663,189,579,627,171,283,218,770,446",
            "415,138,733,631,229,458,419,820,296,350,363,367,424,338,510,387,53,348,667,672",
            "823,200,150,160,800,747,335,538,575,751,313,105,154,353,234,844,573,629,949,313",
            "357,562,160,192,588,668,116,499,443,78,936,475,116,896,469,576,104,69,690,80",
            "689,74,687,307,305,774,849,634,54,187,566,220,175,496,435,329,387,831,433,170",
            "877,725,230,347,415,525,725,229,920,146,664,634,113,86,894,234,504,639,384,260",
            "489,661,51,849,546,283,194,432,469,117,909,159,659,928,64,803,927,841,283,282",
            "177,366,337,565,872,449,945,526,823,679,80,548,628,202,876,225,300,798,438,84",
            "838,72,298,280,701,570,310,398,747,383,165,923,832,389,776,892,431,423,386,384",
            "525,238,554,73,280,441,920,929,59,679,321,928,173,644,804,338,527,534,74,821",
            "360,750,543,184,407,70,150,512,725,767,938,500,668,834,355,751,812,422,291,726",
            "62,670,801,921,141,417,314,806,172,880,430,799,387,808,293,286,444,305,870,178",
            "750,495,279,583,525,472,292,564,541,52,561,995,683,560,423,873,293,868,836,513",
            "566,384,906,59,549,282,188,413,62,292,549,355,473,380,282,180,635,869,50,234",
            "467,825,718,689,494,139,54,833,390,942,349,789,540,59,671,299,554,875,905,225",
            "927,306,315,830,552,391,575,631,682,584,84,170,407,565,15,304,78,660,420,289",
            "809,387,145,713,376,163,677,872,210,530,384,475,337,290,422,738,357,740,902,739",
            "922,184,660,69,270,686,541,236,639,871,419,390,539,502,565,215,630,284,400,930",
            "732,216,352,433,457,405,831,355,943,297,232,513,680,225,815,541,725,312,663,664",
            "875,675,368,68,576,536,230,839,746,291,261,401,508,795,220,311,312,821,365,79",
            "214,715,682,79,324,179,336,510,299,771,12,219,903,845,441,753,949,286,368,339",
            "66,300,731,198,313,81,227,939,387,352,431,829,389,209,501,766,637,742,942,324",
            "497,738,354,942,534,300,289,387,664,421,420,225,452,413,558,323,456,299,506,414",
            "187,575,823,750,182,209,303,213,196,451,742,436,411,200,724,336,75,885,449,751",
            "450,362,825,327,628,495,690,300,905,440,741,975,558,905,803,75,575,145,62,942",
            "440,411,745,713,115,335,406,883,70,259,191,583,632,678,941,428,424,804,359,79",
            "447,71,835,564,838,510,136,715,713,495,662,252,548,50,104,422,350,435,492,895",
            "142,183,546,943,685,730,822,549,190,427,472,865,180,237,357,444,924,150,643,286",
            "302,939,986,930,530,75,543,495,328,215,717,166,445,331,471,514,384,413,533,808",
            "188,660,229,805,417,168,234,828,404,241,64,750,79,941,564,282,826,239,751,578",
            "718,317,591,799,445,716,165,219,931,403,314,418,230,722,218,640,564,874,304,329",
            "360,408,463,907,440,305,426,313,490,871,358,398,750,260,753,751,53,207,173,948",
            "491,291,335,822,237,511,814,361,400,556,411,192,828,683,939,441,816,944,985,492",
            "259,470,431,75,504,225,801,82,237,704,445,365,214,543,498,148,473,513,65,195",
            "76,722,913,813,556,324,402,223,139,266,140,747,533,688,540,444,558,51,804,363",
            "805,457,416,473,527,317,190,474,413,465,892,565,536,316,823,801,284,117,631,417",
            "80,569,538,215,527,184,353,58,173,842,192,839,566,987,536,743,369,902,682,535",
            "576,975,514,739,682,407,909,169,298,748,232,169,558,452,383,494,408,105,639,166",
            "571,746,919,672,365,740,6,106,401,78,559,636,426,165,161,163,279,513,314,535",
            "450,727,284,539,349,873,901,574,201,743,239,841,400,936,206,288,713,133,56,236",
            "942,682,175,839,336,892,292,63,239,662,934,558,661,10,812,368,400,189,176,637",
            "717,368,811,634,355,719,353,142,358,716,830,901,361,841,59,725,720,626,78,937",
            "678,352,171,627,583,736,534,553,287,22,62,67,116,143,60,772,295,417,896,234",
            "642,330,937,210,912,576,194,102,528,430,929,180,403,638,306,735,324,182,199,437",
            "643,543,62,334,831,770,359,62,187,475,576,914,890,291,228,878,714,674,387,445",
            "436,638,433,470,557,535,722,206,339,230,633,845,5,467,54,638,365,141,511,566",
            "468,643,901,420,510,60,617,627,453,922,441,748,876,545,929,727,935,448,630,185",
            "835,78,325,512,469,296,316,370,802,936,58,987,677,672,819,324,941,921,947,195",
            "400,770,413,871,71,490,339,287,260,821,830,331,810,450,89,663,718,716,672,427",
            "299,549,236,170,753,347,293,250,543,564,390,920,328,824,490,403,438,743,770,714",
            "572,74,286,365,566,911,82,331,743,907,807,544,250,140,932,411,166,913,452,902",
            "498,204,195,182,407,654,177,446,740,821,333,947,445,914,818,81,546,163,445,308",
            "410,510,217,414,327,306,325,929,579,171,199,323,469,468,689,450,903,66,862,471",
            "228,829,143,774,341,527,240,304,397,752,174,298,719,839,304,674,429,387,399,926",
            "810,145,800,339,472,407,931,747,294,416,57,644,66,533,877,515,65,655,505,873",
            "172,409,509,508,338,181,240,568,69,819,725,557,202,941,921,251,137,562,627,641",
            "715,415,637,496,921,834,57,323,328,215,991,162,428,396,671,283,138,309,470,176",
            "812,261,196,931,367,806,345,327,898,221,879,163,402,470,541,400,513,138,431,736",
            "310,634,491,420,227,232,533,427,172,630,182,459,746,878,216,816,747,680,811,177",
            "55,288,347,178,922,72,237,280,221,845,385,172,240,979,932,720,401,561,359,391",
            "100,279,162,879,633,334,922,384,842,77,829,774,425,323,935,390,495,417,466,448",
            "800,105,527,720,938,662,3,142,177,295,426,284,239,415,913,82,684,352,474,911",
            "171,285,362,810,364,84,763,874,385,184,230,717,209,570,922,199,915,71,820,355",
            "367,362,163,222,78,188,870,651,920,947,946,179,526,235,363,192,296,745,196,803",
            "58,739,681,161,550,502,226,327,869,947,331,584,537,170,104,735,536,530,949,856",
            "171,281,57,142,554,317,313,562,84,403,870,911,875,20,583,726,191,660,667,571",
            "843,409,237,740,723,563,141,682,311,716,944,107,866,932,466,293,288,730,60,681",
            "333,824,141,179,669,915,363,335,66,540,332,89,240,713,501,542,921,203,569,807",
            "52,442,137,660,842,450,785,562,419,360,691,753,566,810,194,469,59,893,805,877",
            "541,495,674,410,771,470,10,838,917,539,423,293,281,306,491,832,424,307,149,411",
            "314,842,310,583,214,905,721,722,726,685,60,123,542,187,543,827,106,724,433,172",
            "445,78,841,86,79,802,387,345,872,167,909,685,575,283,409,280,514,387,137,293",
            "74,549,822,735,918,415,361,174,822,924,341,642,870,298,896,818,909,676,146,383",
            "526,413,578,886,367,261,449,326,721,359,686,567,841,675,529,208,204,232,829,231",
            "835,74,915,909,67,894,799,731,178,918,408,577,934,681,490,381,404,725,77,677",
            "85,357,202,578,431,828,222,309,106,910,570,349,800,317,667,871,773,318,497,505",
            "312,82,401,198,360,937,662,67,996,302,178,942,813,546,438,61,473,107,417,149",
            "428,845,141,571,68,803,844,385,326,232,169,405,838,568,210,73,203,344,501,312",
            "639,73,219,286,635,982,116,819,813,444,677,328,773,105,424,191,419,418,138,385",
            "644,388,949,174,908,554,539,220,932,361,223,166,296,418,171,361,601,823,211,636",
            "634,906,24,507,191,562,79,435,303,910,142,556,55,385,930,540,930,555,580,336",
            "408,817,351,943,221,547,75,918,389,714,643,513,807,572,62,708,287,836,389,671",
            "750,680,839,674,0,558,941,824,435,309,836,398,167,165,57,550,209,565,879,739",
            "314,217,437,838,301,354,452,4,223,440,547,210,71,312,465,551,206,309,164,433",
            "738,820,469,68,900,162,224,178,738,142,165,515,171,581,439,835,180,927,683,985",
            "728,388,211,988,213,436,440,838,224,678,279,558,532,299,336,722,453,687,237,942",
            "216,339,318,529,328,894,195,926,362,871,679,837,150,891,690,76,820,582,566,187",
            "496,714,203,578,70,309,257,572,279,801,498,390,738,734,282,908,572,347,584,688",
            "833,444,734,223,16,434,561,912,914,260,184,501,300,491,70,744,465,845,386,637",
            "424,677,436,97,576,637,355,503,369,292,146,413,574,578,536,286,198,261,70,288",
            "429,417,465,72,222,906,427,231,830,351,649,282,404,222,898,116,333,83,310,845",
            "326,941,401,437,201,877,827,948,1,748,934,816,196,441,927,389,899,150,578,425",
            "166,910,312,280,427,63,317,541,70,358,331,837,164,503,428,742,578,927,290,981",
            "799,233,689,820,895,944,337,845,870,289,565,383,535,223,575,331,937,702,259,922",
            "296,168,501,713,217,814,280,399,752,549,833,289,669,458,397,394,566,441,227,259",
            "229,96,896,904,918,537,566,171,469,644,946,750,364,72,799,354,452,50,425,239",
            "676,673,295,362,129,630,200,335,231,66,332,221,543,368,104,877,193,395,642,174",
            "542,574,50,161,817,503,549,18,675,896,775,164,817,942,432,900,174,873,136,361",
            "261,74,87,824,911,329,871,910,934,561,407,302,869,398,296,233,572,808,451,832",
            "348,286,452,191,731,357,881,505,418,664,830,325,308,660,299,671,772,338,81,633",
            "664,911,308,815,442,671,182,730,312,435,733,664,775,726,938,329,541,102,399,840",
            "715,352,634,297,367,938,361,434,843,130,557,104,424,311,235,500,583,54,280,843",
            "559,56,938,566,168,900,901,949,442,161,540,314,270,356,560,447,814,818,57,942",
            "432,66,406,945,679,471,77,74,69,548,201,321,116,199,633,228,472,220,833,901",
            "179,444,671,601,413,228,219,166,222,66,538,771,105,228,555,182,190,543,166,813",
            "654,809,453,549,334,506,753,504,193,425,164,228,279,894,927,281,238,628,416,79",
            "196,814,340,470,669,442,725,915,530,573,299,530,675,51,827,86,572,895,638,686",
            "741,568,149,107,912,440,993,428,637,354,561,57,748,237,197,504,944,291,628,57",
            "332,926,571,725,817,201,776,428,577,920,163,148,169,189,183,86,882,413,430,388",
            "775,629,186,676,57,775,907,948,683,354,390,383,206,628,398,990,383,174,435,726",
            "831,848,467,574,408,914,81,224,668,910,211,298,402,393,78,896,61,74,539,899",
            "774,873,391,282,282,449,261,321,227,716,636,471,396,314,629,674,669,292,117,776",
            "392,543,689,809,154,143,878,291,741,220,491,194,233,330,738,386,74,351,177,556",
            "826,844,663,905,242,666,469,431,842,142,289,567,437,666,722,925,721,685,472,922",
            "105,169,288,232,465,895,219,400,202,897,63,685,679,908,180,881,292,818,279,210",
            "934,355,184,949,223,914,462,212,555,640,141,868,683,870,305,217,911,577,443,569",
            "539,944,174,823,549,818,337,139,78,229,145,750,455,291,142,69,927,197,495,507",
            "295,908,552,68,55,934,895,535,664,462,259,190,630,939,819,469,394,491,446,415",
            "936,567,678,419,137,194,337,409,76,17,222,724,82,86,229,511,312,686,261,534",
            "169,883,895,902,831,232,67,107,903,369,751,401,138,828,447,543,203,744,445,369",
            "239,384,420,162,386,359,304,553,473,328,421,640,693,537,802,432,497,509,437,398",
            "729,917,298,335,312,471,311,307,628,306,237,298,663,169,203,391,658,930,424,400",
            "315,491,453,83,369,207,721,290,749,578,501,535,371,635,279,468,387,492,203,814",
            "714,903,82,358,417,76,721,503,357,743,504,381,634,181,929,830,407,240,387,916",
            "307,947,494,207,732,166,928,56,901,416,837,443,530,975,919,355,138,497,333,901",
            "635,354,576,631,747,543,140,300,640,776,195,17,805,178,921,892,421,50,323,410",
            "309,843,165,141,474,627,260,542,261,397,78,742,92,172,642,138,668,359,817,468",
            "176,468,300,513,826,150,541,628,213,168,946,443,743,253,167,807,215,496,335,545",
            "570,301,538,404,141,512,443,167,143,836,165,440,936,326,224,631,237,332,930,462",
            "352,304,874,414,898,497,161,197,329,331,914,909,736,685,139,166,563,603,316,808",
            "303,724,50,340,877,906,393,514,663,749,311,81,540,901,142,473,104,833,352,716",
            "54,180,914,900,290,448,823,539,425,467,414,239,591,219,643,540,895,352,384,941",
            "416,212,310,472,809,558,665,417,85,939,806,799,767,230,928,737,554,201,58,224",
            "635,573,394,442,56,565,896,192,665,377,503,534,421,443,531,498,199,500,412,404",
            "805,508,325,449,720,260,894,601,329,287,728,240,804,471,872,549,197,389,869,837",
            "926,564,947,829,203,353,660,817,364,207,307,238,295,929,932,233,320,313,313,826",
            "429,352,742,472,875,195,663,403,369,899,839,511,806,51,583,124,724,509,716,425",
            "493,572,371,572,350,565,489,900,529,832,144,76,507,530,75,873,837,927,386,470",
            "922,904,874,337,975,296,802,339,948,420,304,819,574,305,185,557,924,892,76,142",
            "738,529,136,354,843,317,637,72,293,63,69,212,340,948,239,324,222,329,220,671",
            "873,211,741,297,412,209,189,150,307,212,585,640,724,402,561,894,560,324,142,209",
            "399,537,326,389,847,825,539,396,874,323,160,738,466,116,337,393,385,290,260,398",
            "328,933,434,206,386,536,444,528,296,743,53,310,578,888,397,557,224,163,402,571",
            "542,144,117,76,734,946,432,506,144,494,544,911,352,417,815,274,772,557,669,415",
            "553,144,691,398,419,900,50,71,736,652,925,333,368,540,360,115,329,875,170,546",
            "411,389,437,946,414,729,210,558,70,237,296,705,668,325,183,432,446,212,939,893",
            "491,221,512,357,219,727,806,904,472,299,740,803,578,134,685,833,718,239,185,205",
            "417,173,66,912,445,184,56,359,161,406,394,651,804,175,279,160,425,333,311,497",
            "533,447,664,354,688,811,361,670,662,917,376,659,209,59,284,748,687,284,395,401",
            "441,238,643,316,814,320,629,360,936,583,435,895,364,161,236,922,473,489,449,505",
            "420,306,469,359,362,163,194,805,674,93,335,629,543,172,395,441,212,51,715,471",
            "117,291,283,331,164,804,168,567,293,301,748,352,390,444,855,638,287,185,909,143",
            "564,233,420,167,175,67,542,829,357,390,370,209,783,868,287,369,430,733,420,634",
            "923,730,65,675,56,751,314,735,804,533,394,728,427,589,57,547,536,725,287,948",
            "413,207,449,723,784,433,365,728,868,73,908,800,929,238,812,630,545,364,451,644",
            "829,928,683,347,59,56,661,434,752,686,736,554,724,807,813,544,281,719,368,103",
            "938,662,410,429,945,807,813,281,335,591,875,217,667,930,287,838,215,937,202,560",
            "718,945,199,452,802,180,911,199,554,227,424,325,654,725,363,538,359,383,473,449",
            "293,85,883,677,308,914,144,501,418,642,679,570,186,918,389,683,162,734,452,310",
            "734,667,649,919,581,301,669,177,536,337,357,633,812,401,541,395,468,876,428,186",
            "922,296,499,510,812,945,405,85,550,401,65,393,366,469,533,651,367,840,396,891",
            "429,501,449,580,754,349,355,501,349,335,52,526,749,313,936,499,388,897,442,548",
            "261,311,584,383,412,284,184,720,363,667,56,203,184,65,572,690,637,978,412,211",
            "440,641,928,545,426,841,411,575,370,320,909,161,69,893,918,168,949,931,875,556",
            "841,803,627,295,301,143,588,445,939,168,411,536,576,309,222,543,910,799,633,439",
            "302,628,230,425,79,260,941,415,574,644,61,841,615,931,306,718,537,635,773,106",
            "573,804,328,567,824,580,871,72,504,0,500,816,539,429,800,637,665,55,543,433",
            "71,105,901,444,728,835,70,879,163,367,387,386,431,927,802,462,143,286,549,392",
            "20,147,934,237,665,190,735,304,203,232,68,388,916,405,329,232,839,223,78,63",
            "366,279,526,431,841,741,500,163,430,871,878,498,121,239,357,819,206,299,307,449",
            "419,441,260,672,324,807,547,217,331,467,510,226,540,87,504,215,351,901,67,799",
            "311,929,207,51,424,260,140,449,909,980,443,332,305,637,411,714,529,810,573,542",
            "948,74,738,149,162,475,287,824,304,218,911,825,664,312,230,498,96,945,713,166",
            "298,729,843,169,803,729,326,455,840,564,260,906,413,501,947,498,167,333,354,337",
            "6,635,500,417,543,299,746,76,580,560,402,530,166,437,417,554,68,808,184,337",
            "746,426,742,672,666,741,53,207,330,207,563,740,879,178,191,405,555,389,980,944",
            "919,875,285,207,290,172,534,509,926,485,940,492,418,74,221,542,827,875,310,496",
            "331,542,721,900,368,738,208,943,80,332,507,900,60,442,559,284,861,349,442,82",
            "422,496,204,723,115,816,322,579,739,903,842,636,490,671,201,107,873,512,70,329",
            "139,670,929,935,81,330,923,427,180,544,254,466,193,876,107,570,427,288,317,542",
            "401,432,506,513,286,186,667,9,668,826,812,743,751,412,914,83,117,644,411,363",
            "280,775,666,737,875,948,181,942,674,188,221,184,321,491,435,392,815,302,751,729",
            "474,489,347,443,578,350,844,829,689,224,799,139,676,218,405,862,943,802,575,505",
            "226,508,302,451,821,563,149,915,279,205,447,415,668,904,239,81,825,464,159,384",
            "884,68,554,530,538,504,801,949,450,910,735,218,370,509,236,357,930,511,163,892",
            "665,583,323,432,63,298,917,717,913,69,740,662,63,771,897,578,870,371,677,197",
            "398,872,644,689,814,52,891,753,73,771,584,323,555,506,233,787,467,413,688,935",
            "867,492,421,259,437,747,566,313,822,168,503,432,724,510,511,535,170,737,62,196",
            "137,679,214,222,409,880,419,406,533,925,138,742,408,397,115,917,411,638,572,145",
            "928,295,468,728,293,338,539,753,83,542,190,227,468,384,737,719,878,860,818,396",
            "139,532,360,184,436,415,503,423,388,316,989,910,644,180,362,669,641,571,810,238",
            "327,727,426,729,331,446,372,679,534,899,727,734,335,117,935,923,551,545,627,503"
        };
    }
}