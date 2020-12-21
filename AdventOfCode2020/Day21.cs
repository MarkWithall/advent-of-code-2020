﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 21: Allergen Assessment")]
    public sealed class Day21
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(2280, AllergenFreeCount(Day21Input));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(5, AllergenFreeCount(Day21SampleInput));
        }

        [Test]
        public void Part2()
        {
            Assert.AreEqual("vfvvnm,bvgm,rdksxt,xknb,hxntcz,bktzrz,srzqtccv,gbtmdb", DangerousIngredients(Day21Input));
        }

        [Test]
        public void Part2Sample()
        {
            Assert.AreEqual("mxmxvkd,sqjhc,fvjkl", DangerousIngredients(Day21SampleInput));
        }

        private static readonly Regex Format = new(@"^(?<ingredients>.*) \(contains (?<allergens>.*)\)$", RegexOptions.Compiled);

        private static string DangerousIngredients(string[] input)
        {
            var foods = ParseFoods(input).ToArray();
            var possibleAllergens = PossibleAllergens(foods);
            var (_, allergens) = Allergens(foods, possibleAllergens);

            return string.Join(",", allergens.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
        }

        private static long AllergenFreeCount(string[] input)
        {
            var foods = ParseFoods(input).ToArray();
            var possibleAllergens = PossibleAllergens(foods);
            var (allergenFree, _) = Allergens(foods, possibleAllergens);

            var ingredients = foods.SelectMany(f => f.Ingredients);
            return ingredients.Count(allergenFree.Contains);
        }


        private static IEnumerable<Food> ParseFoods(string[] input)
        {
            foreach (var line in input)
            {
                var match = Format.Match(line);
                yield return new(match.Groups["ingredients"].Value.Split(' ').ToHashSet(), match.Groups["allergens"].Value.Split(", ").ToHashSet());
            }
        }

        private static IDictionary<string, ISet<string>> PossibleAllergens(Food[] foods) =>
            foods.SelectMany(f => f.Allergens).Distinct().ToDictionary(a => a, a =>
            {
                var fds = foods.Where(f => f.Allergens.Contains(a)).ToArray();
                return fds.Aggregate(fds.First().Ingredients, (intersection, current) => intersection.Intersect(current.Ingredients).ToHashSet());
            });

        private static (ISet<string> allergenFree, IDictionary<string, string> allergens) Allergens(Food[] foods, IDictionary<string, ISet<string>> possibleAllergens)
        {
            Dictionary<string, string> allergens = new();
            var remaining = possibleAllergens.ToHashSet();
            while (remaining.Any(kvp => kvp.Value.Count > 0))
            {
                var a = remaining.Where(kvp => kvp.Value.Count > 0).OrderBy(kvp => kvp.Value.Count).First();
                if (a.Value.Count != 1) throw new InvalidOperationException("No single count left");
                remaining.Remove(a);
                var al = a.Value.Single();
                foreach (var x in remaining)
                {
                    x.Value.Remove(al);
                }

                allergens.Add(a.Key, al);
            }

            return (foods.SelectMany(f => f.Ingredients).Distinct().Except(allergens.Values).ToHashSet(), allergens);
        }

        private sealed record Food(ISet<string> Ingredients, ISet<string> Allergens);

        private static readonly string[] Day21SampleInput =
        {
            "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
            "trh fvjkl sbzzf mxmxvkd (contains dairy)",
            "sqjhc fvjkl (contains soy)",
            "sqjhc mxmxvkd sbzzf (contains fish)"
        };

        private static readonly string[] Day21Input =
        {
            "crjjvr fhpdhz jdmg nfhj sdvng drmr rdbdng jbtjbj bnjhvjm ldvmdh qrgsbb nxbtmj dvxptd nbrmxl dtlfc kxshrl xsmc vbn ncs qzjgv xltv bktzrz gxm jzdtn nxvx qcxdbh rfpzm bplvg ztkgdb psmzk ftzdbf ljmjtcz bcqbb prg sdhkr qdsqx kfkgkjf hxntcz nxksj fdddkd vrpkhg zxbfhrs fvdd xknb pxls lmghcl fvfpdf xnm qxtzl rlxgn nsjtzn mltghx srzqtccv xkjxpf ztv cmdl sblmknh vxtkg msvx qzzq bstlrlb lvmf fjms xkxvrm npcd trpclz dxtggp ptqbrz mtnx tqkc gbtmdb nxxxc rdksxt mmbbcm jxnf cqdb hfts cql mhvf tsddbt lnrsm bvgm vtbjjlg rhvppq grxqh jxr jsp xpcbp (contains nuts)",
            "gxm zqfh nsjtzn gbtmdb hxntcz vxtkg ntkjt szjzjnbn qdsqx ljmjtcz fvfpdf grxqh lmghcl sghx xkjxpf hhlkfng fjms lbhrh mltghx tnhk vhnl jfb hcg fcrvr fzsqhg hfts bktzrz cmdl njvktm trpclz ntc flvmtng zfcmvxj mnvn rdksxt xnm mmbbcm dxtggp rfpzm srzqtccv sbjvmx nbrmxl qcxdbh xknb gpsgbp ljppc vfvvnm nxxxc xvcfsc ncs rkm (contains sesame, nuts)",
            "ptqbrz rfpzm cbnz gmhgp szjzjnbn sghx fdddkd jklfqg qkdg vhnl gxm nxksj ttmqqkp xkjxpf hxntcz mltghx fslj grxqh mlbhm zgtnsh bstlrlb nbrmxl mdqfj tsddbt zxlvks cql krkmhc ntkjt zxbfhrs vfvvnm sxgkn fvdd tnhk pdc zmgsxd zsb vxz xbk rrzmn crjjvr lbhrh vvgfqqn hlfsfxt dmlpn sblmknh vrpkhg gbtmdb bcqbb svbmd nxxxc sbjvmx kfkgkjf lkjmpqlv dztng xpcbp cvrd ndvzt dvxptd ntc hfts rdksxt sgjkh kdlrt rkm bzknq xvcfsc rlxgn nxbtmj qdsqx mksnnv mtnx qzzq zfcmvxj xltv ztkgdb rhvppq cppld xknb trpclz bvgm jjqlc rdbdng lbbztc bktzrz qzjgv qrgsbb fvfpdf cqdb lvmf qpc (contains peanuts, wheat)",
            "srrhv bstlrlb fdddkd fxxczb ldvmdh tsddbt lkjmpqlv xsmc lqfts sdhkr sfj zhlmsn rdksxt jsp szjzjnbn xkxvrm nxbtmj bktzrz vfvvnm qrgsbb xknb hxntcz lmghcl kxshrl bcqbb nxvx mnvn qpc gbtmdb ntkjt grxqh sdvng ljmjtcz nshgxhq hcg hvvtd mplkr gpsgbp bvgm ttmqqkp hlfsfxt zsb kfkgkjf rrzmn sxgkn jfb sghx zgtnsh zmgsxd xbk ftzdbf bgjjk rfpzm qblxkq (contains fish, wheat)",
            "cppld spbkv tnhk qnvp qpc nxvx rdksxt xvcfsc ggbcmjmc ptqbrz ljmjtcz srzqtccv mhvf gnfnxx ldggpnn qtmnj sgjkh qrgsbb rrzmn vfvvnm pxls nxbtmj mzj cdvxz bvgm tztv ntkjt gpsgbp ckzrn njvktm crjjvr vxz qdsqx zqfh qcxdbh rkm nsjtzn jhpd cczvdhx pqtkn lkjmpqlv bktzrz jfb kdlrt nfhj vhnl zxlvks zhlmsn rfpzm sblmknh trpclz sxgkn nshgxhq bzknq psmzk hxntcz hlj mnvn fjms zsb dxtggp xknb jsp (contains peanuts)",
            "cthrglq hhlkfng rrzmn vhnl vfvvnm jbtjbj tqkc zxlvks qrfvhh zqfh kfkgkjf qpc qzjgv srzqtccv bktzrz lbhrh jsp hl hxntcz zsb srrhv jhpd bvrdq qrgsbb ntc dmlpn xgx gbtmdb xnm rdksxt mltghx nshgxhq mcfs fvfpdf nxbtmj fkmshtk cdvxz sbjvmx prg nzll nbrmxl xsmc ljmjtcz sblmknh pqtkn drmr bstlrlb mdqfj vxz bvgm fslj jzdtn xkjxpf xrlljb kxshrl srzcq crjjvr fvdd zxbfhrs ftzdbf msvx (contains soy)",
            "jfb xkxvrm zsb kdlrt gbtmdb sghx srzqtccv zhlmsn ldggpnn zxlvks ptqbrz prg tvdfjnqc zgtnsh scxgb gsxk cmdl cthrglq cdvxz drmr sblmknh xpcbp cqdb lqfts fvfpdf xrlljb dmlpn krkmhc mzj jxr grxqh ncs fzsqhg lxzk ntkjt gxm nxksj vhnl qnvp flvmtng bktzrz vxtkg bvgm rdksxt qzzq hxntcz pdc ldvmdh jbtjbj xvcfsc bvrdq ftzdbf dvxptd fkmshtk fhpdhz xltv vxz mnvn ztv vbn mcfs bcqbb thjnz spbkv mtnx rdbdng lvmf ntc psmzk vfvvnm crjjvr mdqfj hnk qkdg ggbcmjmc gtfhn nzll zfcmvxj (contains soy, dairy, fish)",
            "bktzrz zqfh dmlpn xpcbp rdksxt jsp qqgv bcqbb cql hlj bplvg mplkr srrhv rkm vfvvnm fvfpdf sghx hxntcz flvmtng nxksj mdqfj trpclz jhpd nbrmxl kfkgkjf bvgm sdhkr ndvzt dbh fcppz jzdtn rlxgn xgx bstlrlb hdlkgft srzqtccv nxvx sbjvmx mzj crjjvr tvdfjnqc mlbhm cppld qzjgv rdbdng hhlkfng fslj nsjtzn bnjhvjm cmdl krkmhc sxngk qblxkq dxtggp thjnz cthrglq xknb psmzk (contains soy, peanuts, fish)",
            "flvmtng kxshrl sfj qxtzl hzvh fhpdhz bvrdq vfvvnm hlfsfxt thjnz cbnz qnvp bvgm ggbcmjmc nxvx rdksxt xbk bktzrz sbjvmx nxxxc hxntcz lbhrh gpsgbp gbtmdb sblmknh ckzrn xknb cmdl ljmjtcz crjjvr hhlkfng ldvmdh mdjzx scxgb nzll gxm hnk (contains eggs)",
            "ttmqqkp xltv lmghcl vfvvnm mhvf ntc gbtmdb thjnz ljppc lxzk ldvmdh qblxkq rrzmn xvcfsc xknb rlxgn mmbbcm pxls fdddkd rhvppq gtfhn ggbcmjmc rdksxt bgjjk lnrsm mtnx jxr bvrdq dxtggp kdlrt hxntcz tqkc qxtzl bvgm sdhkr gjbdb qcxdbh fxxczb ljmjtcz xnm xgx mzj rfpzm kxshrl qpc spbkv jklfqg fcppz tsddbt mdjzx zhlmsn njvktm srzqtccv (contains nuts, dairy, soy)",
            "lbhrh szjzjnbn sbjvmx srrhv ntc xbk gbtmdb fkmshtk njvktm scxgb tsddbt crjjvr vtbjjlg ldggpnn hcg cvrd zgrclc qnvp dtlfc hfts bvgm xkxvrm tztv dbh nfhj kxshrl mplkr bstlrlb sghx hvvtd lnrsm ttmqqkp kdlrt jbtjbj npcd vxz sxngk lkjmpqlv dzlrq ztkgdb rrzmn qblxkq qdsqx jfb rkm lxzk qtmnj xknb fxxczb sxgkn mcfs jzdtn xltv slb dvxptd cbnz xkjxpf rdbdng vfvvnm nxxxc zhlmsn sgjkh bktzrz srzqtccv hxntcz ldvmdh gjbdb (contains nuts, wheat)",
            "lbbztc cvrd hl bplvg ggbcmjmc mdqfj scxgb lkjmpqlv lxzk zhlmsn lqfts vtbjjlg cmdl zgrclc cppld qnvp gpsgbp fvfpdf bvgm crjjvr pdc fvdd vfvvnm srzqtccv gxm lbhrh ztkgdb ldggpnn bktzrz drmr jfb sblmknh dtlfc tnhk sbjvmx tztv ftzdbf mksnnv rdksxt gbtmdb nxbtmj rrzmn jdmg hlj qkrl tqkc xknb (contains wheat)",
            "rdbdng fcppz ljmjtcz nsjtzn lbbztc lvmf zgtnsh bktzrz fcrvr cmdl jfb vxtkg npcd hdlkgft fvdd hxntcz bplvg sfj mdqfj ssnrp srzqtccv hhlkfng qcxdbh lxzk cthrglq bvgm qxtzl svbmd gbtmdb scxgb xbk mcfs mksnnv fjms ncs bgjjk qrgsbb qrfvhh crjjvr vhnl tqkc slb pdc cdvxz qqgv gxm xnm nbrmxl tnhk rdksxt qkrl qblxkq qkdg vrpkhg xknb zfcmvxj zmgsxd rfpzm sgjkh nfhj dxtggp qdsqx gtfhn zqfh (contains nuts, fish, soy)",
            "slb bvgm zxlvks mhvf trpclz gbtmdb hlj mnvn vfvvnm xknb lnrsm fvfpdf jbtjbj zqfh dxtggp vxtkg lqfts mtnx rdksxt cbnz dbh thjnz nzll qtmnj hl nbrmxl jklfqg nxksj nxvx zhlmsn qrgsbb fslj ckzrn ptqbrz dvxptd bplvg bgjjk ftzdbf fdddkd jjqlc spbkv mmbbcm sxngk sdvng fvdd tvdfjnqc zgrclc msvx qzzq nxbtmj hxntcz scxgb fhpdhz fxxczb srzcq qpc gnfnxx lvmf zfcmvxj hhlkfng nxxxc hfts bcqbb kfkgkjf vrpkhg kxshrl dmlpn srzqtccv (contains fish, wheat, dairy)",
            "jjqlc vxz fhpdhz mplkr xsmc mnvn hlfsfxt nbrmxl dtlfc rkm cthrglq prg szjzjnbn xnm fcrvr jhpd rrzmn qkdg lkjmpqlv jfb zqfh bktzrz scxgb zmgsxd cqdb srzqtccv jsp fcppz cdvxz xknb vxtkg zxbfhrs sdhkr hzvh bstlrlb xbk fzsqhg grxqh gpsgbp zgrclc cbnz xvcfsc gbtmdb sfj nxbtmj ldggpnn rdksxt bplvg vfvvnm ptqbrz bvgm sghx ljppc xrlljb qtmnj gnfnxx qblxkq tqkc rlxgn crjjvr qcxdbh zsb zgtnsh srrhv slb qqgv spbkv ztv fvfpdf gmhgp (contains dairy, wheat, sesame)",
            "ptqbrz rrzmn mltghx bktzrz bcqbb nxksj vrpkhg jklfqg qzzq ckzrn xpcbp prg fjms ncs tqkc mhvf hzvh zmgsxd dmlpn nxxxc npcd cthrglq mlbhm zsb kdlrt srrhv scxgb bvrdq svbmd lbhrh srzqtccv lxzk hxntcz sdvng mmbbcm vxz xknb lqfts vfvvnm bzknq hfts dbh dtlfc nzll xltv ntc hlfsfxt jsp rdksxt bvgm szjzjnbn tztv ztkgdb nsjtzn flvmtng trpclz qqgv ljppc hnk zqfh (contains eggs)",
            "npcd fhpdhz srzqtccv bktzrz qtmnj sdhkr tztv bvrdq nxvx bgjjk qrfvhh qxtzl nxbtmj mhvf fslj scxgb mtnx jhpd gjbdb jbtjbj vxz krkmhc lnrsm xknb pqtkn tsddbt nzll fvfpdf xkxvrm nfhj hxntcz qqgv xsmc rdksxt fcrvr zxlvks dztng ntkjt sblmknh hhlkfng drmr ckzrn dbh gb hcg crjjvr szjzjnbn vfvvnm bvgm zxbfhrs vhnl trpclz cvrd kdlrt qzjgv dtlfc mdjzx cql flvmtng fcppz sxngk bstlrlb fjms bzknq prg mksnnv mlbhm kfkgkjf ztkgdb tnhk cppld sghx tqkc grxqh (contains nuts)",
            "sfj kdlrt dzlrq ntkjt rhvppq drmr tnhk mdjzx hfts jklfqg zxlvks gpsgbp vhnl crjjvr gjbdb vxtkg xknb bzknq qzzq bvgm qkrl pxls ndvzt mtnx bstlrlb gsxk bnjhvjm bktzrz fxxczb fkmshtk xgx dxtggp qblxkq sdhkr nbrmxl zxbfhrs fzsqhg vbn hxntcz gnfnxx ptqbrz fdddkd cql ckzrn pdc ftzdbf psmzk sdvng zsb rdksxt vtbjjlg fvdd nshgxhq xkxvrm hzvh spbkv dmlpn hcg ldggpnn gbtmdb qcxdbh mmbbcm jxnf vfvvnm fcppz fhpdhz ntc hdlkgft sxgkn cmdl kxshrl zgrclc srrhv nfhj (contains dairy, sesame)",
            "hnk vrpkhg rdksxt jbtjbj vfvvnm zgtnsh jhpd mplkr hxntcz qblxkq cbnz xsmc nxksj qrfvhh zxbfhrs spbkv ldggpnn ckzrn zhlmsn xknb gbtmdb bvgm hzvh ntkjt nxxxc dztng hl sbjvmx ncs fhpdhz xltv gb xrlljb bktzrz ftzdbf cmdl sxgkn prg qqgv zmgsxd sghx sdhkr lvmf vxtkg npcd pxls psmzk thjnz srrhv dvxptd vbn (contains dairy, wheat, sesame)",
            "sblmknh lbbztc dmlpn hlj cczvdhx ggbcmjmc vxtkg xltv srzqtccv qnvp nxksj qrgsbb gbtmdb nxxxc xgx hxntcz jsp drmr gjbdb vrpkhg cql nsjtzn bvgm cthrglq vfvvnm gtfhn lmghcl nxvx ckzrn mdqfj mnvn cdvxz gpsgbp jfb rhvppq jdmg jhpd qtmnj jxnf crjjvr rfpzm dztng xvcfsc bstlrlb zqfh bvrdq mmbbcm jbtjbj ttmqqkp rdksxt gsxk vtbjjlg trpclz ncs qxtzl bzknq svbmd nbrmxl dzlrq cppld flvmtng srrhv bktzrz ftzdbf hlfsfxt jxr rkm sdhkr gxm fslj hzvh (contains sesame)",
            "bnjhvjm xbk hxntcz ptqbrz qpc zsb tsddbt qdsqx rdksxt hlj ssnrp qqgv zfcmvxj mmbbcm vbn vfvvnm msvx srzqtccv bvgm jklfqg mplkr fvfpdf ftzdbf srzcq pdc sdvng fcrvr drmr hcg mdjzx nbrmxl vtbjjlg cvrd zgrclc xkxvrm kdlrt lmghcl prg hfts hhlkfng tnhk zmgsxd mnvn nsjtzn fdddkd xknb qrgsbb xpcbp bzknq bktzrz lqfts lvmf crjjvr tztv xsmc mzj (contains peanuts, nuts, dairy)",
            "xltv bvgm hnk qkdg vvgfqqn srzqtccv mlbhm rkm ptqbrz rdbdng tztv sgjkh srrhv xsmc nxxxc lbhrh dbh gbtmdb ntkjt crjjvr rlxgn ldggpnn fhpdhz trpclz vxtkg gsxk zhlmsn hxntcz nxksj mplkr srzcq jbtjbj jxnf mhvf zgtnsh nfhj rdksxt jfb lkjmpqlv hhlkfng qkrl jdmg qdsqx fcrvr cppld vhnl xknb fkmshtk mmbbcm ndvzt cthrglq qblxkq vtbjjlg bplvg vfvvnm npcd hlfsfxt prg zmgsxd qnvp qtmnj hlj hvvtd bnjhvjm mtnx rfpzm (contains nuts, soy, wheat)",
            "zgtnsh xkxvrm mlbhm jklfqg nxvx mzj srrhv lqfts sxgkn bvgm thjnz mnvn prg hdlkgft gtfhn rdbdng jxr srzqtccv trpclz cdvxz rlxgn zxlvks ldggpnn slb qcxdbh dztng jhpd qzzq cql ntc zxbfhrs sghx nsjtzn ndvzt mltghx qrgsbb ljmjtcz gb xbk fcppz nxxxc grxqh ptqbrz xkjxpf qnvp bktzrz nshgxhq hfts xvcfsc fvdd ncs kfkgkjf fcrvr gbtmdb crjjvr xsmc hxntcz hlfsfxt cbnz rdksxt qblxkq nxbtmj nbrmxl lbhrh dzlrq mdqfj tnhk vfvvnm ljppc kxshrl qpc rhvppq ssnrp (contains sesame, soy, peanuts)",
            "mnvn bcqbb xnm kdlrt lqfts lvmf ggbcmjmc qcxdbh hlj grxqh lxzk gxm trpclz fhpdhz ttmqqkp sxgkn sxngk sghx bstlrlb zgtnsh pqtkn gbtmdb mltghx vfvvnm bktzrz hvvtd bvgm qzjgv dvxptd bzknq zfcmvxj zgrclc ncs cppld fslj nzll srzqtccv lbhrh cdvxz pxls xknb nfhj ztv bgjjk xgx qdsqx gmhgp fxxczb mksnnv fdddkd tsddbt hdlkgft krkmhc psmzk sbjvmx jzdtn hcg hfts xsmc rdksxt (contains eggs, fish)",
            "sgjkh qzzq vrpkhg qtmnj fzsqhg vbn qpc fjms fslj bgjjk zsb hxntcz vfvvnm tqkc jxnf bktzrz qrgsbb gpsgbp rfpzm nxbtmj gbtmdb vvgfqqn tnhk tsddbt prg rrzmn zfcmvxj grxqh njvktm jbtjbj ttmqqkp xnm mhvf rdbdng jjqlc cbnz mcfs rdksxt mplkr qnvp xknb lmghcl cqdb hvvtd mdqfj dtlfc dbh qcxdbh bvgm jdmg jxr gnfnxx xbk fvfpdf fkmshtk dvxptd qqgv slb hfts vxz (contains wheat, dairy, peanuts)",
            "pqtkn zmgsxd drmr mmbbcm nfhj ckzrn dztng gb pdc fkmshtk gnfnxx vxz gbtmdb psmzk mltghx fcppz mcfs vxtkg cthrglq cvrd rhvppq nxksj cppld dtlfc bplvg bcqbb rrzmn mnvn mdqfj rkm ljppc gjbdb gsxk qqgv fhpdhz fdddkd ljmjtcz sfj tztv zgrclc bgjjk ssnrp gmhgp qcxdbh flvmtng ggbcmjmc bvgm nsjtzn rdksxt xnm rfpzm jxnf hnk cczvdhx zxlvks bktzrz qrgsbb trpclz bzknq cdvxz xltv vfvvnm lvmf hzvh bvrdq rdbdng srzqtccv thjnz ztv vrpkhg hhlkfng hvvtd xknb (contains fish, eggs, wheat)",
            "gnfnxx nxvx vbn jxnf krkmhc hlfsfxt cvrd xknb hcg srzqtccv bnjhvjm pqtkn hnk jfb tsddbt gb mplkr nsjtzn hlj jxr qzjgv cql vtbjjlg tnhk bzknq rlxgn npcd lmghcl gmhgp qblxkq zhlmsn slb bplvg cczvdhx zsb mtnx gbtmdb tvdfjnqc ldggpnn flvmtng qkrl cdvxz hxntcz bktzrz vrpkhg nxksj msvx ljmjtcz fvdd vfvvnm qzzq jjqlc hl pdc hzvh zfcmvxj bcqbb sfj xbk mlbhm lqfts ztv dtlfc rdksxt qrfvhh ftzdbf szjzjnbn mnvn sblmknh srzcq qkdg ttmqqkp xrlljb dztng zmgsxd zgtnsh dxtggp (contains fish, sesame, peanuts)",
            "spbkv jzdtn rhvppq vxz fdddkd cczvdhx svbmd qzjgv fhpdhz xvcfsc lmghcl ptqbrz hl bvrdq hxntcz ckzrn gbtmdb rfpzm jbtjbj crjjvr gnfnxx fxxczb qrfvhh hcg sblmknh cthrglq sgjkh hvvtd mltghx lnrsm mlbhm xgx qnvp vxtkg ldggpnn ggbcmjmc jsp zgrclc fzsqhg fvdd srzqtccv rdksxt ntc ssnrp bvgm vfvvnm srrhv nxbtmj jhpd krkmhc bktzrz ttmqqkp xnm ncs prg cbnz mtnx gb grxqh srzcq pdc gmhgp psmzk ndvzt tnhk nfhj jjqlc npcd fjms fslj rkm qtmnj msvx sxgkn gxm bnjhvjm cvrd qzzq hzvh mksnnv nxvx rrzmn nbrmxl cmdl (contains soy, eggs, peanuts)",
            "ssnrp hxntcz mltghx rrzmn zxlvks sfj fdddkd jsp hhlkfng bnjhvjm mdjzx qnvp gb njvktm sblmknh lnrsm rhvppq dtlfc xvcfsc qpc dmlpn hnk vtbjjlg fvfpdf hzvh qzzq gjbdb rdbdng lmghcl jklfqg bstlrlb gpsgbp cbnz jhpd qtmnj bvgm qcxdbh trpclz hlfsfxt qrfvhh zgrclc vvgfqqn xrlljb sdvng mhvf jdmg vfvvnm scxgb rdksxt lbhrh krkmhc cczvdhx gnfnxx zqfh hvvtd hl vbn rfpzm xknb mnvn lkjmpqlv srzqtccv tvdfjnqc gbtmdb qzjgv (contains soy, fish)",
            "vxz hl sdvng cczvdhx fcrvr mlbhm vvgfqqn bnjhvjm lbhrh rfpzm dtlfc mltghx dbh mhvf ndvzt rlxgn rdksxt lnrsm fhpdhz gbtmdb hhlkfng prg vtbjjlg ptqbrz hxntcz bvgm srzqtccv fslj crjjvr gjbdb lbbztc hdlkgft mzj cppld xrlljb mmbbcm ftzdbf hlfsfxt xpcbp bzknq drmr cdvxz lkjmpqlv nsjtzn bcqbb tsddbt xsmc rhvppq msvx jhpd qblxkq vhnl jbtjbj hcg bplvg gpsgbp jsp vfvvnm lvmf jklfqg bstlrlb nbrmxl dztng qkdg ztkgdb bvrdq mnvn lmghcl bktzrz pqtkn sdhkr flvmtng (contains sesame, wheat, eggs)",
            "fxxczb fvfpdf gmhgp ldggpnn xvcfsc srzqtccv ckzrn mplkr xknb hcg dztng qrgsbb cmdl lkjmpqlv sgjkh thjnz bvgm xpcbp zgtnsh dxtggp bplvg xgx gsxk lvmf zhlmsn krkmhc qxtzl ztkgdb lbbztc jdmg mdjzx vxtkg zgrclc njvktm ldvmdh mltghx cthrglq hlj bstlrlb prg mnvn ssnrp zsb nbrmxl pxls gbtmdb nzll hhlkfng qnvp bktzrz ncs rdksxt ptqbrz nxxxc qqgv tnhk rlxgn rfpzm vfvvnm jsp sdhkr mzj mdqfj hl cppld qtmnj dmlpn gb trpclz lqfts xsmc fcrvr zxbfhrs npcd qcxdbh grxqh ljmjtcz xnm (contains dairy, wheat, nuts)",
            "gmhgp rdksxt fcrvr vvgfqqn lvmf mtnx qrgsbb bktzrz npcd xgx cdvxz gxm qnvp qzjgv fvfpdf lxzk xbk zhlmsn nxksj ldggpnn kdlrt zfcmvxj ljmjtcz tsddbt hxntcz zxbfhrs xknb ssnrp qtmnj zmgsxd vfvvnm cczvdhx lmghcl ckzrn jsp ptqbrz ldvmdh gbtmdb ncs drmr cthrglq rdbdng vxtkg lqfts pqtkn srzcq tvdfjnqc hcg sblmknh vrpkhg dbh srzqtccv bstlrlb srrhv xsmc hdlkgft tqkc nxxxc lnrsm zsb fxxczb gb qkrl (contains peanuts, soy, dairy)",
            "hlj qtmnj bgjjk cql srzqtccv xltv dvxptd qzjgv bktzrz spbkv nfhj krkmhc jxr jxnf gnfnxx qkdg nshgxhq cthrglq rlxgn qrgsbb prg cdvxz nxksj dmlpn vbn ggbcmjmc hfts hl ftzdbf qblxkq cvrd xgx vhnl slb gbtmdb vfvvnm srrhv npcd xrlljb psmzk srzcq qpc qqgv sblmknh xbk ncs xkjxpf qrfvhh gsxk nsjtzn dztng nxxxc ssnrp fzsqhg fjms msvx bvrdq zxbfhrs zqfh mnvn rdksxt sxngk cbnz ztv thjnz drmr tztv jhpd xknb mplkr lbhrh vtbjjlg hxntcz sfj jfb tvdfjnqc (contains peanuts, sesame, soy)",
            "ldggpnn cmdl trpclz sbjvmx vxz dtlfc mplkr cczvdhx vvgfqqn sgjkh vxtkg ndvzt ftzdbf nxbtmj nbrmxl qxtzl dmlpn fvfpdf xknb lqfts fcppz kfkgkjf xgx sxgkn gbtmdb ssnrp fslj rdksxt nfhj svbmd zqfh hhlkfng slb jbtjbj nsjtzn cthrglq ttmqqkp psmzk bvgm lvmf jsp sghx sdhkr hzvh cql tvdfjnqc sfj crjjvr bnjhvjm fxxczb nzll qrfvhh qrgsbb qnvp vfvvnm thjnz mdjzx mksnnv hxntcz gjbdb bktzrz ztkgdb hlj xkjxpf jxnf ckzrn gmhgp mhvf (contains eggs)",
            "gb zgtnsh kdlrt cmdl hvvtd sdvng vtbjjlg tsddbt qzzq mtnx lbbztc mcfs rrzmn kfkgkjf lmghcl dmlpn thjnz rlxgn hlj cbnz hfts dzlrq bnjhvjm ntkjt zhlmsn qpc mksnnv flvmtng gpsgbp hhlkfng tnhk sgjkh mnvn xvcfsc hnk mdjzx qkdg xbk qrfvhh svbmd ldggpnn sbjvmx xknb pxls mhvf lbhrh cdvxz ptqbrz hl rdksxt fxxczb slb sfj gxm nsjtzn xltv cczvdhx nxksj drmr qdsqx sxngk srzqtccv nshgxhq ztkgdb ndvzt bplvg nzll qnvp lkjmpqlv bktzrz ssnrp tqkc mltghx gbtmdb ftzdbf gtfhn vfvvnm pqtkn srzcq bvgm (contains wheat, sesame, fish)",
            "sbjvmx drmr fjms zfcmvxj xknb bgjjk jxr sdhkr nshgxhq ntkjt thjnz rdksxt xnm vhnl mksnnv jbtjbj scxgb ttmqqkp pdc lkjmpqlv gmhgp lbbztc dztng qtmnj sghx ldggpnn fcrvr xgx szjzjnbn cthrglq bvgm cdvxz lnrsm mmbbcm dvxptd bstlrlb hlfsfxt mdqfj cqdb ldvmdh fkmshtk jjqlc lxzk gpsgbp spbkv sfj nsjtzn zqfh dtlfc ztkgdb tvdfjnqc rfpzm psmzk jhpd hxntcz gnfnxx hl zhlmsn srrhv msvx hhlkfng kfkgkjf rhvppq krkmhc bnjhvjm bplvg slb qkdg gjbdb rlxgn gbtmdb zmgsxd ftzdbf cczvdhx ckzrn rrzmn flvmtng sblmknh mlbhm jfb srzqtccv vxtkg srzcq vfvvnm gb (contains fish, eggs, wheat)",
            "lvmf ljmjtcz kxshrl ggbcmjmc zsb bvgm qzjgv hlj rfpzm xltv slb thjnz nxxxc nzll srzqtccv rdksxt zgtnsh sghx fcppz tztv fvfpdf cqdb mhvf bktzrz sxgkn dzlrq jdmg qtmnj mzj gbtmdb qrgsbb vfvvnm ptqbrz tsddbt ldvmdh pxls zmgsxd sblmknh qblxkq ztkgdb hxntcz sfj ztv hnk sgjkh fzsqhg zxbfhrs lkjmpqlv hdlkgft fxxczb lmghcl jklfqg xnm (contains wheat)",
            "zmgsxd hvvtd qqgv gsxk vbn jxr hxntcz sghx thjnz cdvxz rkm fdddkd mdjzx sdhkr pxls pdc ljmjtcz mltghx fvdd zqfh xknb kdlrt bktzrz njvktm gpsgbp mdqfj qblxkq dtlfc psmzk nfhj jklfqg zxbfhrs cqdb cvrd fxxczb qpc mmbbcm fjms mplkr ncs nzll msvx gbtmdb qrfvhh qrgsbb ckzrn hzvh gjbdb lqfts cppld fvfpdf vfvvnm bstlrlb nxbtmj qzzq hcg vhnl ftzdbf mtnx crjjvr jdmg ssnrp zsb lbhrh xnm srzqtccv ptqbrz sbjvmx jfb trpclz gtfhn xrlljb bcqbb bvgm lbbztc mnvn kfkgkjf fkmshtk (contains sesame, nuts)"
        };
    }
}