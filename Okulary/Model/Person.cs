using System;
using System.Collections.Generic;

namespace Okulary.Model
{
    public class Person
    {
        public Person()
        {
        }

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime BirthDate { get; set; }

        public ICollection<Binocle> Binocles { get; set; }
    }
}
