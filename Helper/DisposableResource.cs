using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
    public abstract class DisposableResource : IDisposable
    {
        ~DisposableResource()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
