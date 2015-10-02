using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingUtility.Exceptions
{
   public class MapperNotRegisteredException : Exception
   {
      public MapperNotRegisteredException(Type source, Type destination)
         : base(string.Format("Automapper has not been registered for source: '{0}', destination: '{1}'.", source, destination))
      {

      }
   }
}
