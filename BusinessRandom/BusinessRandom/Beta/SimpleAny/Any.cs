namespace BusinessRandom.Beta.SimpleAny
{
    using VacheTacheLibrary;

    public abstract class Any<T>
    {
        protected VacheTache Rand { get;  private set; }

        protected Any(VacheTache rand)
        {
            Rand = rand;
        }

        /// <summary>This method populates the Object and all its mandatory siblings and then its mandatory parents.
        /// </summary>
        /// <returns></returns>
        public abstract T Full();

        /// <summary>This method populates the Object and all its mandatory siblings.
        /// </summary>
        /// <returns></returns>
        public abstract T One();
    }
}
