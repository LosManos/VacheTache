using System.Collections.Generic;

namespace BusinessExample
{
    public class Customer
    {
        /// <summary>Name is mandatory due to business rules
        /// </summary>
        public string Name { get; set; }

        public EmailAddress Email { get; set; }
        
        /// <summary>Address is mandatory due to business rules.
        /// </summary>
        public Address Address { get; set; }

        public IEnumerable<Project> Projects { get; set; }
    }
}