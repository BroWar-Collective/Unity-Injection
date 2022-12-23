using Zenject;

namespace BroWar.Injection
{
    public interface IInjectionManager
    {
        /// <summary>
        /// <see cref="ProjectContext"/> used during injection systems initialization.
        /// </summary>
        ProjectContext ProjectContextPrefab { get; set; }
    }
}