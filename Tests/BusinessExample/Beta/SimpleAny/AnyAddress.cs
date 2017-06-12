namespace BusinessExample.Beta.SimpleAny
{
    using BusinessRandom.Beta.SimpleAny;
    using VacheTacheLibrary;

    public class AnyAddress : Any<Address>
    {
        public AnyAddress(VacheTache rand)
            : base(rand)
        {
        }

        public override Address One()
        {
            return new Address
            {
                Street = Rand.String(10)
            };
        }

        public override Address Full()
        {
            return One();
        }
    }
}