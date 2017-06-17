namespace PseudoRandomLibraryTest
{
    using VacheTacheLibrary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class PseudoRandomTest
    {
        private PseudoRandom pr;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            pr = new PseudoRandom(TestContext.TestName);
        }

        #region Constructor tests.

        [TestMethod]
        public void PseudoRandom_DifferentSeed_ReturnDifferentResult()
        {
            //  #   Arrange.
            var sut1 = new PseudoRandom("Seed1");
            var sut2 = new PseudoRandom("Seed2");

            //  #   Act.
            var res1 = sut1.PositiveInt();
            var res2 = sut2.PositiveInt();

            //  #   Assert.
            Assert.AreNotEqual(res1, res2);
        }

        [TestMethod]
        public void PseudoRandom_NoSeed_UseCallerName()
        {
            //  #   Arrange.
            var sutKnownSeed = new PseudoRandom(nameof(PseudoRandom_NoSeed_UseCallerName));
            var sutMethodSeed = new PseudoRandom();

            //  #   Act.
            var knownSeedResult = sutKnownSeed.PositiveInt();
            var methodSeedResult = sutMethodSeed.PositiveInt();
            var initialisedSeedResult = pr.PositiveInt();

            //  #   Assert.
            Assert.AreEqual(knownSeedResult, methodSeedResult, 
                "The SUTs should all return the same result as all use the same seed.");
            Assert.AreEqual(knownSeedResult, initialisedSeedResult,
                "The SUTs should all return the same result as all use the same seed.");
        }

        [TestMethod]
        public void PseudoRandom_HashCodeFunc_CustomMethod_UseIt()
        {
            //  #   Arrange.
            // Create a hashing function that always returns the same hash code.
            var hashingFunction = new Func<string, int>(s => 0);

            //  #   Act.
            var res1 = new PseudoRandom(hashingFunction, "a seed").PositiveInt();
            var res2 = new PseudoRandom(hashingFunction, "another seed").PositiveInt();

            //  #   Assert.
            Assert.AreEqual(res1, res2);
            Assert.AreEqual(1559595546, res1);
        }

        [TestMethod]
        public void PseudoRandom_SameSeed_ReturnSameResult()
        {
            //  #   Arrange.
            const string Seed = "MySeed";
            var sut1 = new PseudoRandom(Seed);
            var sut2 = new PseudoRandom(Seed);

            //  #   Act.
            var res1 = new[] { sut1.PositiveInt(), sut1.PositiveInt(), sut1.PositiveInt() };
            var res2 = new[] { sut2.PositiveInt(), sut2.PositiveInt(), sut2.PositiveInt() };

            //  #   Assert.
            CollectionAssert.AreEqual(res1, res2);
        }

        #endregion
    }
}
