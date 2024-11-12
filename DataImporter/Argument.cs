using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter
{
    // TODO: This class needs unit tests
    public class Argument
    {
        private string[] _Args;

        public Argument(string[] args)
        { 
            // TODO: Validate this
            this._Args = args;
        }


        // TODO: ArgumentName should be case-insensitive
        public String ExtractValueFor(String ArgumentName)
        {
            for (int i = 0; i < this._Args.Length - 1; i++)
            {
                if (this._Args[i].Equals($"--{ArgumentName}")) { return this._Args[i + 1]; }
            }
            return "";
        }
    }
}
