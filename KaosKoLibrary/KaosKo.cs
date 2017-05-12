namespace KaosKoLIbrary
{
    using System;
    using System.Runtime.CompilerServices;

    public class KaosKo
    {
        private Random _rand;

        /// <summary>This constructor behaves like he default constructor
        /// and is used for seeding the random values with a known seed.
        /// </summary>
        /// <param name="seed">Optional. 
        /// If left out the caller's method name is used for seeding the randomising.
        /// If provided the parameters is hashed and then used for seeding the randomising.</param>
        public KaosKo([CallerMemberName] string seed = null)
            : this(HashCode, seed)
        {
        }

        /// <summary>This constructor is used for using an own method for hashing the seed.
        /// </summary>
        /// <param name="hashCodeFunc"></param>
        /// <param name="seed"></param>
        public KaosKo(Func<string, int> hashCodeFunc, string seed)
        {
            _rand = new Random(hashCodeFunc(seed));
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
            return _rand.Next(maxValue);
        }

        /// <summary>This is a (simple) hash algorithm.
        /// We don't use the built in GetHashCode since it might
        /// change value between every run.
        /// <seealso cref="http://stackoverflow.com/q/41183507/521554"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=netcore-1.1"/>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static int HashCode(string s)
        {
            //return s.Select(a => (int)a).Sum();
            var ret = 0;
            foreach( var c in s ?? string.Empty)
            {
                ret += (int)c;
            }
            return ret;
        }
    }
}
