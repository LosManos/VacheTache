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
            var sut1 = new KaosKo(1);
            var sut2 = new KaosKo(2);

            //  #   Act.
            var res1 = sut1.PositiveInt();
            var res2 = sut2.PositiveInt();

            //  #   Assert.
            Assert.AreNotEqual(res1, res2);
        }

        [TestMethod]
        public void KaosKo_NoSeed_ReturnDifferentResult()
        {
            //  #   Arrange.
            var sut1 = new KaosKo();
            System.Threading.Thread.Sleep(1);
            var sut2 = new KaosKo();

            //  #   Act.
            var res1 = sut1.PositiveInt();
            var res2 = sut2.PositiveInt();

            //  #   Assert.
            Assert.AreNotEqual(res1, res2);
        }

        [TestMethod]
        public void KaosKo_HashCodeFunc_CustomMethod_UseIt()
        {
            //  #   Arrange.
            // Create a hashing function that always returns the same hash code.
           var hashingFunction = new Func<int, int>(s => 0);

            //  #   Act.
            var res1 = new KaosKo(hashingFunction, 42).PositiveInt();
            var res2 = new KaosKo(hashingFunction, 43).PositiveInt();

            //  #   Assert.
            Assert.AreEqual(res1, res2);
            Assert.AreEqual(1559595546, res1);
        }

        [TestMethod]
        public void KaosKo_SameSeed_ReturnSameResult()
        {
            //  #   Arrange.
            const long Seed = 42;
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
            //  #   Arrange.
            var sut = new KaosKo(42);
            var results = new int[2];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Bool();
                results[res ? 1 : 0] += 1;
            }

            Assert.IsTrue(results.All(r => r >= 1), 
                "If we are iterating that many times it would be strange if not both true and false was returned at least once.");
        }

        #endregion

        #region Date tests.

        [TestMethod]
        public void Date_ReturnDate()
        {
            //  Without doing lots of calls there is not much we can test here.
            //  So lets fall back to calling it to make sure we have tested the interface.

            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Date();
                Assert.IsTrue(DateTime.MinValue.Date <= res && res < DateTime.MaxValue.Date);
            }
        }

        [TestMethod]
        public void Date_FromAndTo_ReturnDateBetween()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);
            var Min = new DateTime(2017, 05, 12);
            var Max = new DateTime(2018, 06, 10);
            var results = new int[(Max - Min).Days];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Date(Min, Max);
                Assert.IsTrue(Min <= res && res < Max);
                results[(res - Min).Days] += 1;
            }

            Assert.IsTrue(results.All(r => r >= 1),
                "If we are iterating that many times it would be strange if not both true and false was returned at least once.");
        }

        #endregion

        #region DateAndTime tests.

        [TestMethod]
        public void DateAndTime_ReturnDateAndTime()
        {
            //  Without doing lots of calls there is not much we can test here.
            //  So lets fall back to calling it to make sure we have tested the interface.

            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.DateAndTime();
                Assert.IsTrue(DateTime.MinValue <= res && res < DateTime.MaxValue);
            }
        }

        #endregion

        #region Decimal tests.

        [TestMethod]
        public void Decimal_ReturnDecimal()
        {
            //  Without doing lots of calls there is not much we can test here.
            //  So lets fall back to calling it to make sure we have tested the interface.

            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Decimal();
                Assert.IsTrue(Decimal.MinValue <= res && res < Decimal.MaxValue);
            }
        }

        // This method does not work. Uncomment this test code and associated code and see it fails.
        //[TestMethod]
        //public void Decimal_Interval_ReturnDecimalBetween()
        //{
        //    //  #   Arrange.
        //    var sut = new KaosKo(42);
        //    const decimal Min = -1.234m;
        //    const decimal Max = 2.345m;

        //    //  #   Act and Assert.
        //    for (int i = 0; i < 1_000_000; i++)
        //    {
        //        var res = sut.Decimal(Min, Max);
        //        Assert.IsTrue(
        //            Min <= res && res < Max, 
        //            $"The value {res} is not greater than or equal to {Min} and less than {Max} in iteration {i}.");
        //    }
        //}

        #endregion

        #region Guid tests.

        [TestMethod]
        public void Guid_ReturnValidGuid()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Guid();
                Assert.IsTrue(Guid.TryParse(res.ToString(), out _));
            }
        }

        #endregion

        #region Int tests.

        [TestMethod]
        public void Int_NoParameter_ReturnSameOrAboveIntMinAndBelowIntMax()
        {
            //  Without doing lots of calls there is not much we can test here.
            //  So lets fall back to calling it to make sure we have tested the interface.

            //  #   Arrange.
            var sut = new KaosKo(42);

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
            var sut = new KaosKo(42);
            const int Min = -12;
            const int Max = 420;
            var results = new int[Max-Min];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Int(Min, Max);
                Assert.IsTrue(Min <= res && res < Max);

                //  Keep a tab on how many times each integer value was returned.
                results[res-Min] += 1;
            }

            Assert.IsTrue(
                results.All(r => r >= 1),
                "If we are iterating that many times it would be strange if not all integers was returned at least once.");
        }

        #endregion

        #region PositiveInt tests.

        [TestMethod]
        public void PoistiveInt_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act.
            var res = new[] { sut.PositiveInt(), sut.PositiveInt(), sut.PositiveInt() };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { 1434747710, 302596119, 269548474 },
                res,
                $"Values were [{res[0]},{res[1]},{res[2]}].");
        }

        [TestMethod]
        public void PositiveInt_ReturnPositive()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act and assert.
            for (int i = 0; i <= 1_000_000; i++)
            {
                var res = sut.PositiveInt();
                Assert.IsTrue(1<= res && res <= int.MaxValue);
            }
        }

        [TestMethod]
        public void PositiveInt_Max_ReturnBelow()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);
            const int Max = 420;
            var results = new int[Max-1];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.PositiveInt(Max);
                Assert.IsTrue(1 <= res && res < Max);

                //  Keep a tab on how many times each integer value was returned.
                results[res-1] += 1; 
            }

            Assert.IsTrue(
                results.All(r => r >= 1), 
                "If we are iterating that many times it would be strange if not all integers was returns at least once.");
        }

        #endregion
    }
}
