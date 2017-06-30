namespace VacheTache.Validator.Beta
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ValidatorEngine<T>
    {
        public static bool IsValid(T entity, IEnumerable<Validator<T>> validators)
        {
            return validators.All(v => v.IsValid(entity));
        }
        
        public static bool IsValid(T entity, ValidatorList<T> validators)
        {
            return IsValid(entity, validators.Select(v => v.Value));
        }

        public IEnumerable<ValidationResult> Validate(T entity, IEnumerable<Validator<T>> validators)
        {
            return validators
                .Where(v => v.IsValid(entity) == false)
                .Select(v => new ValidationResult(v.Name, false));
        }

        public IEnumerable<ValidationResult> Validate(T entity, ValidatorList<T> validators)
        {
            return Validate(entity, validators.Select(v => v.Value));
        }
    }
}