namespace BusinessExample.Beta.WrappedAny
{
    using System;
    using BusinessRandom.Beta.WrappedAny;
    using VacheTacheLibrary;

    public class AnyProject : Any<Project, AnyProject>
    {
        /// <summary>A default constructor is needed for <see cref="Any{TEntity,TAny}.ForX{TAnyResult,TType}"/> to work.
        /// </summary>
        public AnyProject()
        {
            
        }
        
        public AnyProject(VacheTache rand) : base(rand)
        {
        }
        
        /// <inheritdoc/>
        public override AnyProject One()
        {
            Entity = CreateAnyProject(Rand);
            return this;
        }

        /// <inheritdoc/>
        public override AnyProject Full()
        {
            One();
            Entity.Owner = new AnyCustomer(Rand).Full().Entity;
            return this;
        }

        /// <inheritdoc/>
        public override AnyProject With(Action<Project> action)
        {
            //_actions.Add(func);
            action(Entity);
            return this;
        }

        /// <summary>This helper method creates a randomised Project with all mandatory properties populated.
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns> 
        private static Project CreateAnyProject(VacheTache rand)
        {
            return new Project
            {
                Number = rand.String("PN", 5)
            };
        }
    }
}
