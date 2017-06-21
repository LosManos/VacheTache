namespace VacheTache.Validator.Beta
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string Name { get; }

        public ValidationResult(string name, bool isValid)
        {
            Name = name;
            IsValid = isValid;
        }
    }
}