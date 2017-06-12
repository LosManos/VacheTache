namespace BusinessExample.Beta.SimpleAny
{
    using BusinessRandom.Beta.SimpleAny;
    using VacheTacheLibrary;

    public class AnyProject : Any<Project>
    {
        public AnyProject(VacheTache rand)
            :base(rand)
        {
        }

        public override Project One()
        {
            return new Project
            {
                Number = Rand.String(),
            };
        }

        public override Project Full()
        {
            var ret = One();
            ret.Owner = new AnyCustomer(Rand).Full();
            return ret;
        }
    }
}