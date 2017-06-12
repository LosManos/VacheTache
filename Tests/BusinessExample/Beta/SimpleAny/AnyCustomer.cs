namespace BusinessExample.Beta.SimpleAny
{
    using BusinessRandom.Beta.SimpleAny;
    using VacheTacheLibrary;

    public class AnyCustomer : Any<Customer>
    {
        public AnyCustomer(VacheTache rand)
            :base(rand)
        {
        }

        /// <summary>This method populates all the Customer's mandatory properties.
        /// </summary>
        /// <returns></returns>
        public override Customer One()
        {
            return new Customer
            {
                Name = Rand.String(),
                Address = new AnyAddress(Rand).One()
            };
        }

        /// <summary>A Customer is a top in the domain graph and has no mandatory parents but it fully populates itself.
        /// </summary>
        /// <returns></returns>
        public override Customer Full()
        {
            return One();
        }
    }
}