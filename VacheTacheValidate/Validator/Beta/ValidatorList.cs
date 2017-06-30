namespace VacheTache.Validator.Beta
{
    using System.Collections.Generic;

    public class ValidatorList<T> : Dictionary<string, Validator<T>>
    {
        public ValidatorList(params Validator<T>[] validators)
        {
            foreach (var validator in validators)
            {
                Add(validator);
            }
        }

        public void Add(Validator<T> validator)
        {
            Add(validator.Name, validator);
        }
    }
}