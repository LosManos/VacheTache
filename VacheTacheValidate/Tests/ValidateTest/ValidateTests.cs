namespace VacheTache.ValidateTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Validator.Beta;

    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public void TestRenameMe()
        {
            var customer = new Customer
            {
                Name = "MyName"
            };
            
            var sut = new CustomerValidator();

            var res = sut.Validate(customer, sut.EntityValidators);

            Assert.AreEqual(1, res.Count());
            Assert.IsFalse(res.Single().IsValid);
        }
    }

    public class Customer
    {
        public string Name { get; set; }

        public IList<Project> Projects { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }
    }

    public class CustomerValidator : ValidatorEngine<Customer>
    {
        public static Func<Customer, bool> HasName = c => string.IsNullOrWhiteSpace(c.Name);
        public static Validator<Customer>.ValidatorFunction NameLength = c => (c?.Name ?? string.Empty).Length <= 10;

        public readonly ValidatorList<Customer> EntityValidators = new ValidatorList<Customer>(
            new Validator<Customer>("HasName", c => HasName(c)),
            new Validator<Customer>("NameLength", c => NameLength(c))
        );

        public bool Validate(Customer customer)
        {
            return IsValid(customer, EntityValidators);
        }
    }

    public class ProjectValidator : ValidatorEngine<Project>
    {
        
    }
}
