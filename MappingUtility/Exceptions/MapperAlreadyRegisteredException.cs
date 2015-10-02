using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingUtility.Exceptions
{
    public class MapperAlreadyRegisteredException : Exception
    {
        public MapperAlreadyRegisteredException(Type source, Type destination)
            : base(string.Format("Automapper has already been registered for source: '{0}', destination: '{1}'.",source, destination))
        {

        }
    }
}
