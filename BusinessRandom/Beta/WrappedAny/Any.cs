namespace BusinessRandom.Beta.WrappedAny
{
    using System;
    using VacheTacheLibrary;

    /// <summary>This is the base class for all AnyXXX classes where XXX typically are the business classes or DTOs.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TAny"></typeparam>
    public abstract class Any<TEntity, TAny>
    {
        public VacheTache Rand { get; set; }

        public TEntity Entity { get; set; }

        /// <summary>A default constructor is needed for <see cref="ForX{TAnyResult,TType}"/> to work.
        /// </summary>
        protected Any()
        {
        }

        /// <summary>This method injects the randomising method we prefer; typically <see cref="PseudoRandom"/>.
        /// </summary>
        /// <param name="rand"></param>
        protected Any(VacheTache rand)
        {
            Rand = rand;
        }

        protected Any(VacheTache rand, TEntity entity)
        :this(rand)
        {
            Entity = entity;
        }

        /// <summary>This method populates the Object and all its mandatory siblings and then its mandatory parents.
        /// </summary>
        /// <returns></returns>
        public abstract TAny Full();

        /// <summary>This method populates the Object and all its mandatory siblings.
        /// </summary>
        /// <returns></returns>
        public abstract TAny One();

        /// <summary>This method is for manipulating the Entity.
        /// The overriding method typically looks like:
        /// <code>
        /// public override AnyCustomer With(Action&lt;MyEntity&gt; action)
        /// {
        ///     action(Entity);
        ///     return this;
        ///}
    /// </code>
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public abstract TAny With(Action<TEntity> action);
        
        /// <summary>This method is used for traversing to another entity.
        /// <para>
        /// Unfortunately we haven't found a way to make it reaturn TAny like the abstract method <see cref="With"/> does.
        /// This forces the inheritor to create its own For method to make the calling code nice.
        /// </para>
        /// </summary>
        /// <typeparam name="TAnyResult"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public TAnyResult ForX<TAnyResult, TType>(Func<TEntity, TType> func)
            where TAnyResult : Any<TType, TAnyResult>, new()
        {
            TType res = func(Entity);
            return new TAnyResult
                {
                    Rand = Rand,
                    Entity = res
                };
        }

        public Any<TEntity, TAny> WithX(Action<TEntity> action)
        {
            action(Entity);
            return this;
        }
    }
}
