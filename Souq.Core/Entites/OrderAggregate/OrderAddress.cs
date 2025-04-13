using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Entites.OrderAggregate
{
   public class OrderAddress
    {
        public OrderAddress()
        {

        }
        public OrderAddress(string firstName, string lastName, string city, string county, string street)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Country = county;
            Street = street;
        }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
