namespace VacheTache.Validator.Beta
{
    public class Validator<T>
    {
        public string Name { get; }
        
        private readonly ValidatorFunction _validatorFunc;

        public bool IsValid(T entity)
        {
            return _validatorFunc(entity);
        }

        public Validator(string name, ValidatorFunction validatorFunc)
        {
            Name = name;
            _validatorFunc = validatorFunc;
        }

        public delegate bool ValidatorFunction(T entity);
    }
}