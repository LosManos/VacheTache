namespace KaosKoLibrary
{
    using System;
    using System.Runtime.CompilerServices;

    public class KaosKo
    {
        protected Random _rand;

        /// <summary>This constructor behaves like he default constructor
        /// and is used for seeding the random values with a known seed.
        /// </summary>
        /// <param name="seed">Optional. 
        /// If left out DateTime.UtcNow.Ticks is used for seeding the randomising.
        /// If provided the parameter is used for seeding the randomising.</param>
        public KaosKo(long? seed = null)
            : this(HashCode, seed ?? DateTime.UtcNow.Ticks)
        {
        }

        /// <summary>This constructor is used for using a custom method for hashing the seed.
        /// </summary>
        /// <param name="hashCodeFunc"></param>
        /// <param name="seed"></param>
        public KaosKo(Func<int, int> hashCodeFunc, long seed)
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