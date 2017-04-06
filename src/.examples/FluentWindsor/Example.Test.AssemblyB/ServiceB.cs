using System;
using System.Diagnostics;
using Example.Test.AssemblyA;

namespace Example.Test.AssemblyB
{
    public class ServiceB : IDisposable
    {
        private readonly ServiceA serviceA;

        public ServiceB(ServiceA serviceA)
        {
            this.serviceA = serviceA;
        }

        public void Execute()
        {
            serviceA.Execute();
        }

        public void Dispose()
        {
            Debug.WriteLine("ServiceB: Dispose called ... ");
        }
    }
}
