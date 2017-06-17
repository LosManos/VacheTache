namespace VacheTacheLibrary
{
    using System;
    using System.Runtime.CompilerServices;

    public class PseudoRandom : VacheTache
    {
        /// <summary>This constructor behaves like he default constructor
        /// and is used for seeding the random values with a known seed.
        /// </summary>
        /// <param name="seed">Optional. 
        /// If left out the caller's method name is used for seeding the randomising.
        /// If provided the parameter is hashed and then used for seeding the randomising.</param>
        public PseudoRandom([CallerMemberName] string seed = null)
            : this(HashCode, seed)
        {
        }

        /// <summary>This constructor is used for using a custom method for hashing the seed.
        /// </summary>
        /// <param name="hashCodeFunc"></param>
        /// <param name="seed"></param>
        public PseudoRandom(Func<string, int> hashCodeFunc, string seed)
        {
            _rand = new Random(hashCodeFunc(seed));
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
            var ret = 0L;
            foreach (var c in s ?? string.Empty)
            {
                ret += (int)c;
            }
            return unchecked((int)ret);
        }
    }
}
