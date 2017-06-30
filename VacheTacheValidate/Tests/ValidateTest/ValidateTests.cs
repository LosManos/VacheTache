namespace VacheTache.ValidateTest
{
    using System;
    using System.Linq;
    using BusinessObjects;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Validator.Beta;

    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public void ExampleValidationSuccess()
        {
            var customer = new Customer
            {
                Name = "MyName"
            };
            
            var sut = new CustomerValidator();

            var res = sut.Validate(customer, CustomerValidator.EntityValidators).ToList();

            Assert.AreEqual(0, res.Count());
        }

        [TestMethod]
        public void ExampleValidationFail()
        {
            var customer = new Customer
            {
                Name = "12345678901"
            };

            var sut = new CustomerValidator();

            var res = sut.Validate(customer, CustomerValidator.EntityValidators).ToList();

            Assert.AreEqual(2, res.Count);
            Assert.IsFalse(res[0].IsValid);
            Assert.IsFalse(res[1].IsValid);
        }
    }

    public class CustomerValidator : ValidatorEngine<Customer>
    {
        public static Func<Customer, bool> HasName = c => string.IsNullOrWhiteSpace(c.Name) == false;
        public static Validator<Customer>.ValidatorFunction NameLength = c => (c?.Name ?? string.Empty).Length <= 10;
        public static Validator<Customer>.ValidatorFunction StartsWithCapitalLetter =
            c => (c.Name ?? string.Empty).Any() && char.IsUpper(c.Name[0]);
        
        public static readonly ValidatorList<Customer> EntityValidators = new ValidatorList<Customer>(
            new Validator<Customer>("HasName", c => HasName(c)),
            new Validator<Customer>("NameLength", c => NameLength(c)), 
            new Validator<Customer>("CapitalStart", c => StartsWithCapitalLetter(c))
        );

        public static bool Validate(Customer customer)
        {
            return IsValid(customer, EntityValidators);
        }
    }

    public class ProjectValidator : ValidatorEngine<Project>
    {
        
    }
}
