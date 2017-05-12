namespace KaosKoLibraryTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using KaosKoLibrary;
    using System.Linq;

    [TestClass]
    public class KaosKoTest
    {
        #region Constructor tests.

        [TestMethod]
        public void KaosKo_DifferentSeed_ReturnDifferentResult()
        {
            //  #   Arrange.
            var sut1 = new KaosKo("Seed1");
            var sut2 = new KaosKo("Seed2");

            //  #   Act.
            var res1 = sut1.PositiveInt();
            var res2 = sut2.PositiveInt();

            //  #   Assert.
            Assert.AreNotEqual(res1, res2);
        }

        [TestMethod]
        public void KaosKo_NoSeed_UseCallerName()
        {
            //  #   Arrange.
            var sutKnownSeed = new KaosKo(nameof(KaosKo_NoSeed_UseCallerName));
            var sutMethodSeed = new KaosKo();

            //  #   Act.
            var knownSeedResult = sutKnownSeed.PositiveInt();
            var methodSeedResult = sutMethodSeed.PositiveInt();

            //  #   Assert.
            Assert.AreEqual(knownSeedResult, methodSeedResult);
        }

        [TestMethod]
        public void KaosKo_HashCodeFunc_NewMethod_UseIt()
        {
            //  #   Arrange.
            // Create a hashing function that always returns the same hash code.
           var hashingFunction = new Func<string, int>(s => 0);

            //  #   Act.
            var res1 = new KaosKo(hashingFunction, "a seed").PositiveInt();
            var res2 = new KaosKo(hashingFunction, "another seed").PositiveInt();

            //  #   Assert.
            Assert.AreEqual(res1, res2);
            Assert.AreEqual(1559595546, res1);
        }

        [TestMethod]
        public void KaosKo_SameSeed_ReturnSameResult()
        {
            //  #   Arrange.
            const string Seed = "MySeed";
            var sut1 = new KaosKo(Seed);
            var sut2 = new KaosKo(Seed);

            //  #   Act.
            var res1 = new[] { sut1.PositiveInt(), sut1.PositiveInt(), sut1.PositiveInt() };
            var res2 = new[] { sut2.PositiveInt(), sut2.PositiveInt(), sut2.PositiveInt() };

            //  #   Assert.
            CollectionAssert.AreEqual(res1, res2);
        }

        #endregion

        #region Bool tests.

        [TestMethod]
        public void Bool_ReturnBool()
        {
            //  #   Arrange
        }
        #endregion

        #region Guid tests.

        [TestMethod]
        public void Guid_ReturnValidGuid()
        {
            //  #   Arrange.
            var sut = new KaosKo();

            //  #   Act.
            var res = sut.Guid();

            //  #   Assert.
            Assert.IsTrue(Guid.TryParse(res.ToString(), out _));
        }

        #endregion

        #region Int tests.

        [TestMethod]
        public void Int_NoParameter_ReturnSameOrAboveIntMinAndBelowIntMax()
        {
            //  Without doing lots of calls there is not much we can test here.
            //  So lets fall back to calling it to make sure we have tested the interface.

            //  #   Arrange.
            var sut = new KaosKo();

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Int();
                Assert.IsTrue(int.MinValue <= res && res < int.MaxValue);
            }
        }

        [TestMethod]
        public void Int_MinAndMax_ReturnBetweenInclusiveMinAndExclusiveMax()
        {
            //  #   Arrange.
            var sut = new KaosKo();
            const int Min = -12;
            const int Max = 420;
            var results = new int[Max];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.PositiveInt(Max);
                Assert.IsTrue(Min <= res && res < Max);

                //  Keep a tab on how many times each integer value was returned.
                results[res] += 1;
            }

            Assert.IsTrue(
                results.All(r => r >= 1),
                "If we are iterating that many times it would be strange if not all integers was returns at least once.");
        }

        #endregion

        #region PositiveInt tests.

        [TestMethod]
        public void PoistiveInt_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo("MySeed");

            //  #   Act.
            var res = new[] { sut.PositiveInt(), sut.PositiveInt(), sut.PositiveInt() };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { 644677688, 1890600396, 597045847 },
                res,
                $"Values were [{res[0]},{res[1]},{res[2]}].");
        }

        [TestMethod]
        public void PositiveInt_ReturnPositive()
        {
            //  #   Arrange.
            var sut = new KaosKo();

            //  #   Act and assert.
            for (int i = 0; i <= 1_000_000; i++)
            {
                var res = sut.PositiveInt();
                Assert.IsTrue(0 <= res && res <= int.MaxValue);
            }
        }

        [TestMethod]
        public void PositiveInt_Max_ReturnBelow()
        {
            //  #   Arrange.
            var sut = new KaosKo();
            const int Max = 420;
            var results = new int[Max];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.PositiveInt(Max);
                Assert.IsTrue(0 <= res && res < Max);

                //  Keep a tab on how many times each integer value was returned.
                results[res] += 1; 
            }

            Assert.IsTrue(
                results.All(r => r >= 1), 
                "If we are iterating that many times it would be strange if not all integers was returns at least once.");
        }

        #endregion
    }
}
