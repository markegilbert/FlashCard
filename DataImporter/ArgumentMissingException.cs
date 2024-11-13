using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter
{
    public class ArgumentMissingException: Exception
    {
        public ArgumentMissingException(String ArgumentName) : base($"The command line argument '{ArgumentName}' is missing and needs to be supplied")
        { }
    }
}
