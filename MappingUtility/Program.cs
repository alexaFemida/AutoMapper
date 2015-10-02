using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MappingUtility.Models;

namespace MappingUtility
{
   class Program
   {
      static void Main(string[] args)
      {
         IosContactName iosContactName = new IosContactName();
         var wcfContactName = new WcfContactName()
         {
            Login = "User1",
            FirstName = "Nick",
            LastName = "Perry",
            Age = 22,
            Address = new Address() { City = "London", Street = "Golden Lane" }
         };
         Mapper mapper = new Mapper();
         mapper.MapTypes<WcfContactName, IosContactName>();
         mapper.ApplyMapping(wcfContactName, iosContactName);
      }
   }
}
