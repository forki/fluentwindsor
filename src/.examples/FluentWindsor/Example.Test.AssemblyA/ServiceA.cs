using System;
using System.Diagnostics;

namespace Example.Test.AssemblyA
{
    public class ServiceA : BaseObject, IDisposable
    {
        public void Execute()
        {
        }

        public void Dispose()
        {
            Debug.WriteLine("ServiceA: Dispose called ... ");
        }

        ~ServiceA()
        {
            Debug.WriteLine("ServiceA: Finalized ... ");
        }
    }
}
