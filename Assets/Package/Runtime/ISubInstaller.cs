using Zenject;

namespace BroWar.Injection
{
    public interface ISubInstaller
    {
        void Install(DiContainer container);
    }
}