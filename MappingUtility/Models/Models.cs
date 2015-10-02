using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingUtility.Models
{
      public class WcfContactName
      {
         public string Login { get; set; }
         public string FirstName { get; set; }
         public string LastName { get; set; }
         public int Age { get; set; }
         public Address Address { get; set; }
      }
      public class IosContactName
      {
         public string login { get; set; }
         public string FirstName { get; set; }
         public string LastName { get; set; }
         public string Age { get; set; }
         public Address Address { get; set; }
      }

      public class Address
      {
         public string City { get; set; }
         public string Street { get; set; }
      }
      public class AddressDto
      {
         public string City { get; set; }
         public string Street { get; set; }
         public int Number { get; set; }
      }
   }

