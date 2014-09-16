using Example.Test.AssemblyA;

namespace Example.Test.AssemblyB
{
    public class ServiceB
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
    }
}
