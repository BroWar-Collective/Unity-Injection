using System.Collections.Generic;

namespace BroWar.Injection
{
    public interface IInjectablesProvider
    {
        void ProvideInjectables(List<object> injectables);
    }
}