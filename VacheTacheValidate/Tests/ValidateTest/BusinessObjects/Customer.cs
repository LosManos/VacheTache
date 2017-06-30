namespace VacheTache.ValidateTest.BusinessObjects
{
    using System.Collections.Generic;

    public class Customer
    {
        public string Name { get; set; }

        public IList<Project> Projects { get; set; }
    }
}