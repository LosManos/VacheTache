namespace BusinessExample.Beta.WrappedAny
{
    using System;
    using BusinessRandom.Beta.WrappedAny;
    using VacheTacheLibrary;

    public class AnyCustomer : Any<Customer, AnyCustomer>
    {
        /// <summary>A default constructor is needed for <see cref="Any{TEntity,TAny}.ForX{TAnyResult,TType}"/> to work.
        /// </summary>
        public AnyCustomer()
        {

        }

        /// <inheritdoc/>
        public AnyCustomer(VacheTache rand) : base(rand)
        {
        }

        /// <inheritdoc/>
        public AnyCustomer(VacheTache rand, Customer customer)
        : this(rand)
        {
            Entity = customer;
        }

        /// <summary>This method traverses from the Customer entity to the Address.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public AnyAddress For(Func<Customer, Address> func)
        {
            if (Entity.Address == null)
            {
                var ret = new AnyAddress(Rand).One();
                Entity.Address = ret.Entity;
                return ret;
            }
            else
            {
                return new AnyAddress(Rand, Entity.Address);
            }
        }

        /// <summary>This method creates an AnyCustomer with all its mandatory properties populated.
        /// </summary>
        /// <returns></returns>
        public override AnyCustomer One()
        {
            Entity = CreateAnyCustomer(Rand);
            return this;
        }

        /// <summary>This method creates a Customer and its mandatory properties and nothing else really as a Customer is at the top of an aggregate.
        /// </summary>
        /// <returns></returns>
        public override AnyCustomer Full()
        {
            One();
            return this;
        }

        /// <summary>This method is used for manipulating the Customer.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public override AnyCustomer With(Action<Customer> action)
        {
            action(Entity);
            return this;
        }

        /// <summary>This helper method creates a randomised Customer with all mandatory properties populated.
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static Customer CreateAnyCustomer(VacheTache rand)
        {
            return new Customer
            {
                Name = rand.String("CN", 10)
            };
        }
    }
}