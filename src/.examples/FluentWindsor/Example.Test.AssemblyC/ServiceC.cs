using System;
using System.Diagnostics;
using Example.Test.AssemblyA;
using Example.Test.AssemblyB;

namespace Example.Test.AssemblyC
{
    public class ServiceC : IDisposable
    {
        private readonly ServiceA serviceA;
        private readonly ServiceB serviceB;

        public ServiceC(ServiceA serviceA, ServiceB serviceB)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
        }

        public void Execute()
        {
            serviceA.Execute();
            serviceB.Execute();
        }

        public void Dispose()
        {
            Debug.WriteLine("ServiceC: Dispose called ... ");
        }

        ~ServiceC()
        {
            Debug.WriteLine("ServiceC: Finalized ... ");
        }
    }
}
