namespace BusinessRandomTest.Beta.SimpleAny
{
    using System;
    using System.Linq;
    using BusinessExample;
    using BusinessExample.Beta.SimpleAny;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VacheTacheLibrary;

    [TestClass]
    public class AnyTests
    {
        [TestMethod]
        public void Full_TheObjectAndParents()
        {
            var rand = new VacheTache(836);
            var sut = new AnyProject(rand);

            var res = sut.Full();
            
            AssertProject(res);
            AssetProjectParentHaveValue(res);
        }

        [TestMethod]
        public void One_TheObjectAndMandatoryChildren()
        {
            var rand = new VacheTache(2231);
            var sut = new AnyCustomer(rand);

            var res = sut.One();

            AssertCustomer(res);
        }

        #region Assert methods.

        private static void AssertAddress(Address address)
        {
            Assert.IsTrue(address.Street.Any());
        }

        private static void AssertCustomer(Customer customer)
        {
            AssertCustomerOptionalHasNotValue(customer);
            Assert.IsTrue(customer.Name.Any());
            AssertAddress(customer.Address);
        }

        private static void AssertCustomerOptionalHasNotValue(Customer customer)
        {
            Assert.IsNull(customer.Email);
            Assert.IsTrue(customer.Projects == null || customer.Projects.Any() == false);
        }

        private static void AssertCustomerParentHasValue(Customer customer)
        {
            //  A Customer has no parent so there is no check.
        }

        private static void AssertProject(Project project)
        {
            AssertProjectOptionalHasNotValue(project);
            Assert.IsTrue(project.Number.Any());
        }

        private static void AssertProjectOptionalHasNotValue(Project project)
        {
            Assert.AreEqual(default(DateTime), project.StartDate);
        }

        private static void AssetProjectParentHaveValue(Project project)
        {
            Assert.IsNotNull(project.Owner);
            AssertCustomerParentHasValue(project.Owner);
        }

        #endregion
    }
}
