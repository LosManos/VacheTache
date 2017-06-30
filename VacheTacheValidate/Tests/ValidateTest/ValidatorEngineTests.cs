namespace VacheTache.ValidateTest
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Validator.Beta;

    [TestClass]
    public class ValidatorEngineTests
    {
        [TestMethod]
        public void EnumerableValidators_ValidateAll()
        {
            var v = new ValidatorsAndFlags();

            var customer = new ABusinessObject();
            Assert.IsFalse(v.ValidatorOneWasCalled, "Sanity check.");
            Assert.IsFalse(v.ValidatorTwoWasCalled, "Sanity check.");

            //  #   Act.
            var res = ABusinessObjectValidator.IsValid(customer, v.Validators);

            Assert.IsTrue(res);
            Assert.IsTrue(v.ValidatorOneWasCalled);
            Assert.IsTrue(v.ValidatorTwoWasCalled);
        }

        private class ValidatorsAndFlags
        {
            public bool ValidatorOneWasCalled = false;
            public bool ValidatorTwoWasCalled = false;
            public readonly IList<Validator<ABusinessObject>> Validators;

            public ValidatorsAndFlags()
            {
                Validators = Create();
            }
            
            private IList<Validator<ABusinessObject>> Create()
            {
                var validatorOneCalled = new Action<ValidatorsAndFlags>(x =>
                    x.ValidatorOneWasCalled = true);
                var validatorTwoCalled = new Action<ValidatorsAndFlags>(x =>
                    x.ValidatorTwoWasCalled = true);
                IList<Validator<ABusinessObject>> validators = new List<Validator<ABusinessObject>>
                {
                    new Validator<ABusinessObject>("ValidatorOne", c =>
                    {
                        validatorOneCalled(this);
                        return true;
                    }),
                    new Validator<ABusinessObject>("ValidatorTwo", c =>
                    {
                        validatorTwoCalled(this);
                        return true;
                    })
                };
                return validators;
            }
        }

        private class ABusinessObjectValidator : ValidatorEngine<ABusinessObject>
        {
        }

        private class ABusinessObject
        {
        }
    }
}