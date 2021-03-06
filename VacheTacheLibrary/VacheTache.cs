﻿namespace VacheTacheLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VacheTache
    {
        protected Random _rand;

        public string StringCharacters { get; set; } = "abcdefghijklmnopqrstuvwzyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>This is the standard length of the string returned by <see cref="String"/>.
        /// </summary>
        public int StringLength { get; set; } = 8;

        /// <summary>This is the standard amount of decimals returned by <see cref="Currency"/>.
        /// AFAIK there is only Yen that differs with 0 decimals.
        /// </summary>
        public int NumberOfCurrencyDecimals = 2;

        /// <summary>This constructor behaves like he default constructor
        /// and is used for seeding the random values with a known seed.
        /// </summary>
        /// <param name="seed">Optional. 
        /// If left out DateTime.UtcNow.Ticks is used for seeding the randomising.
        /// If provided the parameter is used for seeding the randomising.</param>
        public VacheTache(long? seed = null)
            : this(HashCode, seed ?? DateTime.UtcNow.Ticks)
        {
        }

        /// <summary>This constructor is used for using a custom method for hashing the seed.
        /// </summary>
        /// <param name="hashCodeFunc"></param>
        /// <param name="seed"></param>
        public VacheTache(Func<int, int> hashCodeFunc, long seed)
        {
            var intSeed = unchecked((int)seed);
            _rand = new Random(hashCodeFunc(intSeed));
        }

        /// <summary>This method returns a randomised boolean.
        /// The code is copied with pride from https://msdn.microsoft.com/en-us/library/system.random(v=vs.110).aspx#Boolean
        /// </summary>
        /// <returns></returns>
        public bool Bool()
        {
            return Convert.ToBoolean(_rand.Next(0, 2));
        }

        /// <summary>This method returns a positive randomised currency
        /// Higher or equal to <see cref="Int.Min"/> and less than <see cref="Int.Max"/>.
        /// A currency is a value like Euro and Cents, 12.34 for instance; 2 andn only 2 decimals. Use <see cref="NumberOfCurrencyDecimals" to change this.
        /// </summary>
        /// <returns></returns>
        public decimal Currency()
        {
            return Currency(int.MinValue, int.MaxValue);
        }

        /// <summary>This method returns a randomised currency.
        /// A currency is a value like Euro and Cents, 12.34 for instance; 2 and only 2 decimals. Use <see cref="NumberOfCurrencyDecimals" to change this.
        /// Min is the lowest returned value and can be returned.
        /// Max is above returned value and cannot be returned. See dotnet's Random.Next method for similar behaviour.
        /// The number of decimals (cent) in the result is specified in <see cref="NumberOfCurrencyDecimals"/> which has default value 2.
        /// </summary>
        /// <param name="min">This parameter is of type int, instead of decimal, because we have no way to randomise decimal within an interval.</param>
        /// <param name="max">This parameter is of type int, instead of decimal, because we have no way to randomise decimal within an interval.</param>
        /// <returns></returns>
        public decimal Currency(int min, int max)
        {
            return Currency(min, max, NumberOfCurrencyDecimals);
        }

        /// <summary>This method returns a randomised currency.
        /// A currency is a value like Euro and Cents, 12.34 for instance. 2 and only 2 decimals. Use <see cref="NumberOfCurrencyDecimals" to change this.
        /// Min is the lowest returned value and can be returned.
        /// Max is above returned value and cannot be returned. See dotnet's Random.Next method for similar behaviour.
        /// </summary>
        /// <param name="min">This parameter is of type int, instead of decimal, because we have no way to randomise decimal within an interval.</param>
        /// <param name="max">This parameter is of type int, instead of decimal, because we have no way to randomise decimal within an interval.</param>
        /// <param name="numberOfDecimals">The number of decimals (cent) in use. Normally 2 but for yen it is 0.</param>
        /// <returns></returns>
        public decimal Currency(int min, int max, int numberOfDecimals)
        {
            var euro = Int(min, max);
            decimal centDecimal = 0;

            if(numberOfDecimals >= 1)
            {
                var maxCent = Math.Max(0, (int)Math.Pow(10, numberOfDecimals) - 1);
                decimal cent = Int(0, maxCent);
                centDecimal = cent / (decimal)Math.Pow(10, numberOfDecimals);
            }
            return euro + (decimal)centDecimal;
        }

        /// <summary>This method returns a random Date.
        /// The lowest returned date it DateTime.MinValue
        /// and the highest is DateTime.MaxValue.AddDays(-1).
        /// It returns a Date with time set to 00:00:00
        /// </summary>
        /// <returns></returns>
        public DateTime Date()
        {
            return Date(DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>This method returns a random Date within an interval
        /// where the result is equal or greater than from
        /// and lesser than to.
        /// Note that the to value is not included. This equals the functionality of System.Random.Next().
        /// It returns a Date with time set to 00:00:00
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public DateTime Date(DateTime from, DateTime to)
        {
            int dayRange = (to - from).Days;

            return from.AddDays(_rand.NextDouble() * dayRange).Date;
        }

        /// <summary>This method returns a random Date and Time.
        /// </summary>
        /// <returns></returns>
        public DateTime DateAndTime()
        {
            return Date().AddHours(this.Int(0, 23)).AddMinutes(this.Int(0, 59)).AddSeconds(this.Int(0, 59));
        }

        /// <summary>This method returns a random decimal.
        /// Now decimals can be Very large and Very small and using them with == is seldom a good idea.
        /// The result is not evenly distributed.
        /// Maybe we should use NextDecimal from https://numerics.mathdotnet.com/Random.html instead?
        /// </summary>
        /// <returns></returns>
        public decimal Decimal()
        {
            // This solution is thought through and not only copied from Stack overflow.
            var bits = new Int32[4];
            bits[0] = _rand.Next();
            bits[1] = _rand.Next();
            bits[2] = _rand.Next();
            bits[3] = _rand.Next();

            // According to https://msdn.microsoft.com/en-us/library/t1de0ya1.aspx the decimal has a certain layout
            // where some bits must be 0. Hence we have to mask the 4th int32-part.
            // We have a known issue where "Bits 16 to 23 must contain an exponent between 0 and 28, which indicates the power of 10 to divide the integer number."
            // As for now I was satisfied to limiting it to 16 and not 28. /OF
            // If you want to play with the mask, use this string as it is split into the parts mentioned in the documentation linked above.
            var mask = Convert.ToInt32("10000000000011110000000000000000", 2);
            bits[3] = bits[3] & mask;

            // Due to some reason I haven't figured out the MSb is never randomised. So we do it here.
            // Are there more bits that are not randomised?
            var sign = Bool() ? 1 : -1;

            return new decimal(bits) * sign;
        }

        // This method does not work. Uncomment this code and associated test and see it fails.
        ///// <summary>This method returns a decimal between minValue and maxValue, maxValue not included.
        ///// The result is not evenly distributed.
        ///// <para>
        ///// The code is copied from http://stackoverflow.com/a/28860710/521554
        ///// and not thoroughly tested.
        ///// </para>            
        ///// </summary>
        ///// <param name="minValue"></param>
        ///// <param name="maxValue"></param>
        ///// <returns></returns>
        //public decimal Decimal(decimal minValue, decimal maxValue)
        //{
        //    return Math.Abs(Decimal()) % (maxValue - minValue) + minValue;
        //}

        /// <summary>This method returns a pseudo randomised Enum by choice.
        /// <para>
        /// Note: it does not handle inconsecutive series or items with the same value properly.
        /// That can be remedied by getting all values to a list and then randomising from these values, but it is presently not implemented so.
        /// </para>
        /// <para>
        /// Note it does not handle enums with the same item value.
        /// http://stackoverflow.com/questions/8043027/non-unique-enum-values
        /// </para>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public TEnum Enum<TEnum>() where TEnum : struct
        {
            var minValue = System.Enum.GetValues(typeof(TEnum)).Cast<int>().Min();
            var maxValue = System.Enum.GetValues(typeof(TEnum)).Cast<int>().Max();

            int val = _rand.Next(minValue, maxValue + 1);

            // Whoa! This is what I found necessary for convering an int to a generic enum!
            return (TEnum)System.Enum.Parse(typeof(TEnum), val.ToString());
        }

        /// <summary>This method returns a pseudo randomised Enum by choice except the item provided in the parameter.
        /// <para>
        /// Note: it does not handle inconsecutive series or items with the same value properly.
        /// That can be remedied by getting all values to a list and then randomising from these values, but it is presently not implemented so.
        /// </para>
        /// <para>
        /// Note it does not handle enums with the same item value.
        /// http://stackoverflow.com/questions/8043027/non-unique-enum-values
        /// </para>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="exceptItem"></param>
        /// <returns></returns>
        public TEnum EnumExcept<TEnum>(TEnum exceptItem) where TEnum : struct
        {
            var values = System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
            var exceptName = System.Enum.GetName(typeof(TEnum), exceptItem);
            values.Remove(exceptItem);
            int index = _rand.Next(values.Count());
            return values[index];
        }

        /// <summary>This method creates a new GUIDish value.
        /// It is not a proper GUID as a such adheres to certain rules, for instance there are presently 4 different GUID versions and the generating of randomised values below does not take this into consideration.
        /// The code is copied from http://stackoverflow.com/a/13188409/521554
        /// </summary>
        /// <returns></returns>
        public Guid Guid()
        {
            var guid = new byte[16];
            _rand.NextBytes(guid);
            return new Guid(guid);
        }

        /// <summary>This method returns a randomised integer
        /// same or above int.MinValue and
        /// below int.MaxValue.
        /// It is really just a pass through to Random.Next(int.MinValue, int.MaxValue);
        /// </summary>
        /// <returns></returns>
        public int Int()
        {
            return _rand.Next(int.MinValue, int.MaxValue);
        }
        /// <summary>This method returns a randomised integer below maxValue and above or same as minValue.
        /// Note lower-than-max and not lower-or-equal-to-max.
        /// The reason for this is that the called Random.Next method behaves this way
        /// and right or wrong we keep with it.
        /// It is really just a pass through to Random.Next(int min, int max);
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int Int(int minValue, int maxValue)
        {
            return _rand.Next(minValue, maxValue);
        }

        public T OneOf<T>(IEnumerable<T> lst)
        {
            var index = Int(0, lst.Count() - 1);

            return lst.Skip(index).First();
        }

        /// <summary>This method returns a randomised integer between 0 and int.MaxValue.
        /// It is really just a pass through to Random.Next();
        /// </summary>
        /// <returns></returns>
        public int PositiveInt()
        {
            return _rand.Next();
        }

        /// <summary>This method returns a randomised integer below maxValue.
        /// Note lower-than-max and not lower-or-equal-to-max.
        /// The reason for this is that the called Random.Next method behaves this way
        /// and right or wrong we keep with it.
        /// It is really just a pass through to Random.Next(int max);
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int PositiveInt(int maxValue)
        {
            return _rand.Next(maxValue - 1) + 1;
        }

        /// <summary>This method returns a randomised Long greater than or equal to 0.
        /// Warning: The method is not thoroughly tested for not being zero.
        /// <para>
        /// The code is copied with pride from http://stackoverflow.com/a/2000459/521554
        /// </para>
        /// </summary>
        /// <returns></returns>
        public long PositiveLong()
        {
            return PositiveLong(long.MaxValue);
        }

        /// <summary>This method return a randomised Long greater than 0 and less than max value.
        /// <para>
        /// The code is copied with pride from http://stackoverflow.com/a/2000459/521554
        /// </para>
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public long PositiveLong(long maxValue)
        {
            var max = maxValue - 1;
            return (long)((NextPositiveLong() / (double)long.MaxValue) * max)+1;
        }

        /// <summary>This method returns a long between the values.
        /// <para>
        /// The code is copied with pride from http://stackoverflow.com/a/2000459/521554
        /// </para>
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public long PositiveLong(long minValue, long maxValue)
        {
            if( minValue <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue),
                    $"MinValue must be 1 or greater. It was [{minValue}]");
            }
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), 
                    $"MinValue [{minValue}] must be less than MaxValue [{maxValue}]");
            }
            //var max = maxValue - 1;
            long range = maxValue - minValue + 1;
            return PositiveLong(range) + minValue - 1;
        }

        /// <summary>This method returns a randomised string.
        /// The returned characters are in settable property <see cref="StringCharacters"/>.
        /// The length of the returned string is in settable property <see cref="StringLength"/>
        /// </summary>
        /// <returns></returns>
        public string String()
        {
            return String(StringLength);
        }

        /// <summary>This method returns a randomised stirng
        /// with the length set in the parameter.
        /// The returned characters are in settable property <see cref="StringCharacters"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string String(int length)
        {
            var res = new char[length];
            var chars = StringCharacters.ToCharArray();
            var charsLength = chars.Length;
            for (int i = 0; i < length; i++)
            {
                res[i] = chars[Int(0, charsLength - 1)];
            }
            return new string(res);
        }

        /// <summary>This method returns a prefixed randomised string.
        /// The returned characters are in settable property <see cref="StringCharacters"/>.
        /// The length of the returned string is in settable property <see cref="StringLength"/>
        /// The prefix is included in the string length.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string String( string prefix)
        {
            return String(prefix, StringLength);
        }

        /// <summary>This method returns a prefixed randomised string
        /// with the length set in the parameter.
        /// The returned characters are in settable property <see cref="StringCharacters"/>.
        /// The prefix is included in the string length.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string String(string prefix, int length)
        {
            prefix = prefix ?? string.Empty;
            return (prefix + String(length)).Substring(0, length);
        }
        /// <summary>This is a (simple) hash algorithm.
        /// We don't use the built in GetHashCode since it might
        /// change value between every run.
        /// <seealso cref="http://stackoverflow.com/q/41183507/521554"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=netcore-1.1"/>
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int HashCode(int n)
        {
            return n.GetHashCode();
        }

        /// <summary>This helper method returns a randomised long.
        /// The code is copied with pride from http://stackoverflow.com/a/2000459/521554
        /// </summary>
        /// <returns></returns>
        private long NextPositiveLong()
        {

            byte[] bytes = new byte[sizeof(long)];
            _rand.NextBytes(bytes);
            // strip out the sign bit
            bytes[7] = (byte)(bytes[7] & 0x7f);
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
