namespace BusinessRandomTest.Beta.WrappedAny
{
    using System;
    using BusinessExample;
    using BusinessExample.Beta.WrappedAny;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VacheTacheLibrary;
    using AnyCustomer = BusinessExample.Beta.WrappedAny.AnyCustomer;
    using AnyProject = BusinessExample.Beta.WrappedAny.AnyProject;

    [TestClass]
    public class AnyTests
    {
        [TestMethod]
        public void Full_TheObjectAndParents()
        {
            var rand = new VacheTache(2303);
            var sut = new AnyProject(rand);

            sut.Full();
            BusinessAssert.FullProject(sut.Entity);
        }

        [TestMethod]
        public void One_TheObjectAndMandatoryChildren()
        {
            var rand = new VacheTache(2302);
            var sut = new AnyProject(rand);

            sut.One();

            BusinessAssert.OneProject(sut.Entity);
            Assert.IsNull(sut.Entity.Owner);
        }

        [TestMethod]
        public void WithX_ManipulateEntity()
        {
            var rand = new VacheTache(31);
            var sut = new AnyProject(rand);

            sut.Full()
                .WithX(p => p.Number = "ProjNumberX")
                .With(p => p.Number += "A")
                .WithX(p => p.Number += "Z");
            
            Assert.AreEqual("ProjNumberXAZ", sut.Entity.Number);
        }

        [TestMethod]
        public void ForX_TraverseGraph()
        {
            var rand = new VacheTache(31);
            var sut = new AnyProject(rand);

            sut.Full()
                .ForX<AnyCustomer, Customer>(p => p.Owner)
                .With(c => c.Name = "CustName");
            
            Assert.AreEqual("CustName", sut.Entity.Owner.Name);
        }

        #region Helper methods.

        #endregion
    }
}