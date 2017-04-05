using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Policies
{
    public static class AssemblyScanningPolicies
    {
        public static IAssemblyScanningPolicy[] All =
        {
            new CastleWindsorPolicy(), 
            new FluentWindsorPolicy(), 
            new MicrosoftPolicy(), 
            new MsCorLibPolicy(), 
            new SystemPolicy(), 
        };
    }
}