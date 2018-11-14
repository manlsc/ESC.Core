using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    public class CoreException : Exception
    {
        public CoreException(string message) : base(message) { }

        public CoreException(string message, Exception innerException) : base(message, innerException) { }
    }
}
