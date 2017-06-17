namespace BusinessExample.Beta.WrappedAny
{
    using System;
    using BusinessRandom.Beta.WrappedAny;
    using VacheTacheLibrary;

    public class AnyAddress : Any<Address, AnyAddress>
    {
        /// <summary>A default constructor is needed for <see cref="Any{TEntity,TAny}.ForX{TAnyResult,TType}"/> to work.
        /// </summary>
        public AnyAddress()
        {
            
        }
        
        public AnyAddress(VacheTache rand) : base(rand)
        {
        }

        public AnyAddress(VacheTache rand, Address address)
            : this(rand)
        {
            Entity = address;
        }
        
        public override AnyAddress Full()
        {
            One();

            return this;
        }

        /// <inheritdoc/>
        public override AnyAddress One()
        {
            Entity = CreateAnyAddress(Rand);
            return this;
        }

        /// <inheritdoc/>
        public override AnyAddress With(Action<Address> action)
        {
            action(Entity);
            return this;
        }

        /// <summary>This helper method creates a randomised Address with all mandatory properties populated.
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static Address CreateAnyAddress(VacheTache rand)
        {
            return new Address
            {
                Street = rand.String("Street-", 10)
            };
        }
    }
}