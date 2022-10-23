using Zenject;

namespace BroWar.Injection
{
    internal interface IInstaller
    {
        void Install(DiContainer container);
    }
}