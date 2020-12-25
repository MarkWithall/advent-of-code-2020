using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture(TestName = "Day 25: Combo Breaker")]
    public sealed class Day25
    {
        private const long SamplePublicKey1 = 5764801;
        private const long SamplePublicKey2 = 17807724;

        private const long PublicKey1 = 11562782;
        private const long PublicKey2 = 18108497;

        [Test, Ignore("Slow")]
        public void Part1()
        {
            Assert.AreEqual(2947148, EncryptionKey(PublicKey1, PublicKey2));
        }

        [Test]
        public void Part1Sample()
        {
            Assert.AreEqual(14897079, EncryptionKey(SamplePublicKey1, SamplePublicKey2));
        }

        private static long EncryptionKey(long publicKey1, long publicKey2)
        {
            var ls1 = LoopSize(publicKey1);
            //var ls2 = LoopSize(publicKey2);

            long encryptionKey1 = 1;
            for (var i = 0; i < ls1; i++)
            {
                encryptionKey1 *= publicKey2;
                encryptionKey1 %= 20201227;
            }

            //long encryptionKey2 = 1;
            //for (var i = 0; i < ls2; i++)
            //{
            //    encryptionKey2 *= publicKey1;
            //    encryptionKey2 %= 20201227;
            //}

            return encryptionKey1;

            static long LoopSize(long key)
            {
                long loopSize = 0;
                long current = 1;
                do
                {
                    current *= 7;
                    current %= 20201227;
                    loopSize++;
                } while (current != key);

                return loopSize;
            }
        }
    }
}