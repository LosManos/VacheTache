namespace BusinessExample
{
    using System;

    public class Project
    {
        /// <summary>Mandatory.
        /// It is the way the business recognises a Project.
        /// </summary>
        public string Number { get; set; }
        
        /// <summary>Optional.
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>Mandatory.
        /// </summary>
        public Customer Owner { get; set; }
    }
}