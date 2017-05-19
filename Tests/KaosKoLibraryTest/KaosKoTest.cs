namespace KaosKoLibraryTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using KaosKoLibrary;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;

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

        #region Currency tests.

        [TestMethod]
        public void Currency_ReturnTypeEveryOneHundredthFraction()
        {
            decimal value = 0;
            var sut = new KaosKo(2204);
            Assert.AreEqual(
                value.GetType(),
                sut.Currency().GetType(), 
                $"Sobriety test that we are checking against the type that {nameof(sut.Currency)} returns.");

            for (int euro = 0; euro < 100; euro++)
            {
                for (int cent = 0; cent < 100; cent++)
                {
                    value = euro + (decimal)cent / 100;
                    var valueString = $"{euro}." + cent.ToString("D2");
                    Assert.AreEqual(
                        valueString, 
                        value.ToString("0.00", CultureInfo.InvariantCulture),
                        $"We should be able to represent every 'cent' with the return type of {nameof(sut.Currency)}");
                }
            }
        }

        [TestMethod]
        public void Currency_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo(2209);

            //  #   Act.
            var res = new[] { sut.Currency(), sut.Currency(), sut.Currency() };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { 1640167078.82m, 115094073.98m, 1424365856.26m },
                res,
                $"Values were [{string.Join(",", res)}]");
        }

        [TestMethod]
        public void Currency_MinMax_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo(2209);
            const int Min = -13;
            const int Max = 33;

            //  #   Act.
            var res = new[] { sut.Currency(Min, Max), sut.Currency(Min, Max), sut.Currency(Min, Max) };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { 22.96m, 25.05m, 11.98m },
                res,
                $"Values were [{string.Join(", ", res)}]");
        }

        [TestMethod]
        public void Currency_MinMax_ReturnBetween()
        {
            //  #   Arrange.
            var sut = new KaosKo(2222);
            const int Min = -12;
            const int Max = 25;
            var euroResults = new Dictionary<int, int>();
            var centResults = new Dictionary<int, int>();
            //var results = new Dictionary<decimal, int>();

            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Currency(Min, Max);
                Assert.IsTrue(Min <= res && res < Max,
                    $"Returned value was {res} which is not greater or equal to {Min} and less than {Max} at index {i}.");

                Increase(euroResults, (int)decimal.Truncate(res));
                Increase(centResults, DecimalPart(res, 2));
            }

            Assert.AreEqual(Max - Min, euroResults.Count);
            Assert.IsTrue(
                euroResults.All(r => r.Value >= 1));
            Assert.AreEqual(100, centResults.Count);
            Assert.IsTrue(
                centResults.All(r => r.Value >= 1));
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

        #region Enum tests.

        private enum MyEnum
        {
            I, 
            prefer, 
            a, 
            painful, 
            truth, 
            over, 
            any, 
            blissful, 
            fantasy
        }

        [TestMethod]
        public void Enum_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo(2315);

            //  #   Act.
            var res = new[] { sut.Enum<MyEnum>(), sut.Enum<MyEnum>(), sut.Enum<MyEnum>() };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { MyEnum.prefer, MyEnum.I, MyEnum.fantasy },
                res,
                $"The result [{string.Join(",", res)}] did not match.");
        }

        [TestMethod]
        public void Enum_ReturnWithinInterval()
        {
            //  #   Arrange.
            var sut = new KaosKo(2327);
            var results = new Dictionary<MyEnum, int>();

            //  #   Act.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.Enum<MyEnum>();
                Increase(results, res);
            }

            Assert.IsTrue(
                results.All(r => r.Value >= 1),
                "With that many iterations every enum item should have been returned at least once.");
        }

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

        #region OneOf tests.

        [TestMethod]
        public void OneOf_ListOfOne_ReturnTheItem()
        {
            //  #   Arrange.
            var sut = new KaosKo(1820);
            var lst = new[] { "E" };

            //  #   Act and Assert.
            for (int i = 0; i < 1_000; i++)
            {
                var res = sut.OneOf(lst);
                Assert.AreEqual("E", res);
            }
        }

        [TestMethod]
        public void OneOf_ReturnAllInList()
        {
            //  #   Arrange.
            var sut = new KaosKo(1820);
            var lst = new[] { "A", "B", "C", "D", "E" };
            var results = new Dictionary<string, int>();

            //  #   Act.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.OneOf(lst);
                Increase(results, res);
            }

            //  #   Assert.
            Assert.IsTrue(
                results.All(r => r.Value >= 1),
                "If we are iterating that many times it would be strange if not all items were returned at least once.");
        }

        [TestMethod]
        public void OneOf_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo(1820);
            var lst = new[] { "A", "B", "C", "D", "E" };

            //  #   Act.
            var res = new[] { sut.OneOf(lst), sut.OneOf(lst), sut.OneOf(lst) };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { "C", "D", "D" },
                res,
                $"The result [{string.Join(",", res)}] did not match.");
        }

        #endregion

        #region Int tests.

        [DataTestMethod]
        [DataRow("7", 0)]
        [DataRow("7.3", 1)]
        [DataRow("7.42", 2)]
        [DataRow("7.427", 3)]
        public void Currency_CustomNumberOfDecimals_ReturnAccordingManyDecimals(string expectedResult, int numberOfDecimals)
        {
            //  #   Arrange.
            var expectedResultDecimal = decimal.Parse(expectedResult, CultureInfo.InvariantCulture);
            var sut = new KaosKo(2156);

            //  #   Act.
            var res = sut.Currency(0, 100, numberOfDecimals);

            //  #   Assert.
            Assert.AreEqual(expectedResultDecimal, res,
                $"When randomising a Currency with {numberOfDecimals} we expect {expectedResult}.");
        }

        [DataTestMethod]
        [DataRow("7", 0)]
        [DataRow("7.3", 1)]
        [DataRow("7.42", 2)]
        [DataRow("7.427", 3)]
        public void Currency_CustomNumberOfDecimalsProperty_ReturnAccordingManyDecimals(string expectedResult, int numberOfDecimals)
        {
            //  #   Arrange.
            var expectedResultDecimal = decimal.Parse(expectedResult, CultureInfo.InvariantCulture);
            var sut = new KaosKo(2156);

            //  #   Act.
            sut.NumberOfCurrencyDecimals = numberOfDecimals;
            var res = sut.Currency(0, 100);

            //  #   Assert.
            Assert.AreEqual(expectedResultDecimal, res,
                $"When randomising a Currency with {numberOfDecimals} we expect {expectedResult}.");
        }

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

            Assert.AreEqual(
                Max - Min,
                results.Count(),
                "We should have gotten one of each integer.");
            Assert.IsTrue(
                results.All(r => r >= 1),
                "If we are iterating that many times it would be strange if not all integers were returned at least once.");
        }

        #endregion

        #region PositiveInt tests.

        [TestMethod]
        public void PositiveInt_ReturnPredictableResult()
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
            const int Max = 43;
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

        #region PositiveLong tests.

        [TestMethod]
        public void PositiveLong_ReturnPredictableResult()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act.
            var res = new[] { sut.PositiveLong(), sut.PositiveLong(), sut.PositiveLong() };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { 4309105566363031553, 4233293422508541441, 5502579974182123521 },
                res,
                $"Values were [{res[0]},{res[1]},{res[2]}].");
        }

        [TestMethod]
        public void PositiveLong_ReturnPositive()
        {
            // A long is so big so a miljon runs won't show anything
            // but at least we have called it to secure the interface.

            //  #   Arrange.
            var sut = new KaosKo(42);

            //  #   Act and assert.
            for (int i = 0; i <= 1_000_000; i++)
            {
                var res = sut.PositiveLong();
                Assert.IsTrue(1 <= res && res <= long.MaxValue, 
                    $"The value {res} was not greater than 0 and <= {long.MaxValue} at index {i}.");
            }
        }

        [TestMethod]
        public void PositiveLong_Max_ReturnBelow()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);
            const long Max = 45;
            var results = new long[Max - 1];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.PositiveLong(Max);
                Assert.IsTrue(1 <= res && res < Max,
                    $"The value {res} was not greater than 0 and <= {Max} at index {i}.");

                //  Keep a tab on how many times each long value was returned.
                results[res - 1] += 1;
            }

            Assert.IsTrue(
                results.All(r => r >= 1),
                "If we are iterating that many times it would be strange if not all longs was returns at least once.");
        }

        [TestMethod]
        public void PositiveLong_MinAndMax_ReturnBelow()
        {
            //  #   Arrange.
            var sut = new KaosKo(42);
            const long Min = 21;
            const long Max = 45;
            var results = new long[Max - Min];

            //  #   Act and Assert.
            for (int i = 0; i < 1_000_000; i++)
            {
                var res = sut.PositiveLong(Min, Max);
                Assert.IsTrue(Min <= res && res < Max,
                    $"The value {res} was not greater than {Min} and < {Max} at index {i}.");

                //  Keep a tab on how many times each long value was returned.
                results[res - Min] += 1;
            }

            Assert.IsTrue(
                results.All(r => r >= 1),
                "If we are iterating that many times it would be strange if not all longs was returns at least once.");
        }

        #endregion

        #region String tests.

        [TestMethod]
        public void String_BePredictable()
        {
            //  #   Arrange.
            var sut = new KaosKo(55);

            //  #   Act.
            var res = new[] { sut.String(), sut.String(), sut.String() };

            //  #   Assert.
            CollectionAssert.AreEqual(
                new[] { "C6mscMHc", "VHXyAfDz", "7fkLhurp" },
                res,
                $"Values were [{string.Join(",",res)}]");
        }

        [TestMethod]
        public void String_CustomStringCharacters_ReturnOnlyThem()
        {
            //  #   Arrange.
            const string Characters = "abcDEF";
            var sut = new KaosKo(2106);
            sut.StringCharacters = Characters;
            var results = new Dictionary<string, int>(); ;

            //  #   Act and Assert.
            for (int i = 0; i < 1_000; i++)
            {
                var res = sut.String();
                Assert.IsTrue(
                    ExpectedContainsActual(Characters, res),
                    $"Result [{res}] contains character(s) that aren't in [{Characters}] at loop index {i}."
                );
            }
        }

        [TestMethod]
        public void String_CustomStringLength_ReturnStringWithSetLength()
        {
            //  #   Arrange.
            var sut = new KaosKo(2132);
            sut.StringLength = 6;

            //  #   Act.
            var res = sut.String();

            //  #   Assert.
            Assert.AreEqual(6, res.Length);
        }

        [TestMethod]
        public void String_Length_ReturnStringWithLength()
        {
            //  #   Arrange.
            var sut = new KaosKo(2132);
            const int Length = 7;
            Assert.AreNotEqual(Length, sut.StringLength, "Sobriety test that we don't set the length to StringLength.");

            //  #   Act.
            var res = sut.String(Length);

            //  #   Assert.
            Assert.AreEqual(Length, res.Length);
        }

        [TestMethod]
        public void String_NullPrefix_ReturnNonPrefixedString()
        {
            //  #   Arrange.
            var sut = new KaosKo(2142);

            //  #   Act.
            var res = sut.String(null);

            //  #   Assert.
            Assert.AreEqual("UtaTrBNd", res);
        }

        [TestMethod]
        public void String_Prefix_ReturnPrefixedString()
        {
            //  #   Arrange.
            var sut = new KaosKo(2142);

            //  #   Act.
            var res = sut.String("321");

            //  #   Assert.
            Assert.AreEqual("321UtaTr", res);
        }

        [TestMethod]
        public void String_NullPrefixAndLength_ReturnNonPrefixedStringOfLength()
        {
            //  #   Arrange.
            var sut = new KaosKo(2142);

            //  #   Act.
            var res = sut.String(null, 3);

            //  #   Assert.
            Assert.AreEqual("Uta", res);
        }

        [TestMethod]
        public void String_PrefixAndLength_ReturnPrefixedString()
        {
            //  #   Arrange.
            var sut = new KaosKo(2142);

            //  #   Act.
            var res = sut.String("321", 5);

            //  #   Assert.
            Assert.AreEqual("321Ut", res);
        }

        [TestMethod]
        public void String_PrefixAndShorterLength_ReturnTruncatedPrefix()
        {
            //  #   Arrange.
            var sut = new KaosKo(2142);

            //  #   Act.
            var res = sut.String("abcdef", 3);

            //  #   Assert.
            Assert.AreEqual("abc", res);
        }

        #endregion

        #region DecimalPart tests.

        [DataTestMethod]
        [DataRow(0, "0.0", 0)]
        [DataRow(0, "0.0", 1)]
        [DataRow(0, "0.0", 2)]
        [DataRow(0, "0.0", 3)]
        [DataRow(0, "1.0", 1)]
        [DataRow(0, "1.0", 2)]
        [DataRow(0, "2.01", 0)]
        [DataRow(0, "2.01", 1)]
        [DataRow(1, "2.01", 2)]
        [DataRow(0, "2.1", 0)]
        [DataRow(0, "2.1", 0)]
        [DataRow(1, "2.1", 1)]
        [DataRow(10, "2.1", 2)]
        [DataRow(1, "2.10", 1)]
        [DataRow(10, "2.10", 2)]
        [DataRow(1, "2.13", 1)]
        [DataRow(13, "2.13", 2)]
        [DataRow(130, "2.13", 3)]
        [DataRow(0, "99.9", 0)]
        [DataRow(9, "99.9", 1)]
        [DataRow(90, "99.9", 2)]
        [DataRow(0, "99.999", 0)]
        [DataRow(9, "99.999", 1)]
        [DataRow(99, "99.999", 2)]
        [DataRow(999, "99.999", 3)]
        [DataRow(0, "99.099", 0)]
        [DataRow(0, "99.099", 1)]
        [DataRow(9, "99.099", 2)]
        public void DecimalPart_Value_ReturnDecimalPart(int expectedDecimalPart, string source, int numberOfDecimals)
        {
            //  #   Arrange.
            var decimalSource = decimal.Parse(source, CultureInfo.InvariantCulture);

            //  #   Act.
            var res = DecimalPart(decimalSource, numberOfDecimals);
            
            //  #   Assert.
            Assert.AreEqual(expectedDecimalPart, res,
                $"The decimal {source} is expected to have decimals {expectedDecimalPart} with {numberOfDecimals} decimals");
        }

        #endregion

        private static bool ExpectedContainsActual(string expected, string actual)
        {
            return actual.All(ec => expected.Contains(ec));
        }

        private static int DecimalPart(decimal source, int numberOfDecimals)
        {
            if( numberOfDecimals == 0)
            {
                return 0;
            }

            var s = source.ToString(CultureInfo.InvariantCulture);
            if(s.Contains(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator) == false)
            {
                s += ".0";
            }
            var decimalPart = s.Substring(s.IndexOf(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)+1);

            decimalPart = decimalPart.PadRight(numberOfDecimals, '0').Substring(0, numberOfDecimals);

            return int.Parse(decimalPart, CultureInfo.InvariantCulture);
        }

        private static void Increase<TKey>(Dictionary<TKey,int> dictionary, TKey key)
        {
            var value = dictionary.ContainsKey(key) ? dictionary[key] : 0;
            dictionary[key] = value + 1;
        }
    }
}
