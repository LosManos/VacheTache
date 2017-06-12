namespace BusinessRandomTest.Beta.WrappedAny
{
    using BusinessExample;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal static class BusinessAssert
    {
        internal static void FullProject(Project project)
        {
            OneProject(project);
            Assert.IsNotNull(project.Owner);
            OneCustomer(project.Owner);
        }

        internal static void OneCustomer(Customer customer)
        {
            Assert.IsNotNull(customer.Name);
        }

        internal static void OneProject(Project project)
        {
            Assert.IsNotNull(project.Number);
        }
    }
}
