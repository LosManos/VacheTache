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
            return _rand.Next(maxValue-1)+1;
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
    }
}
