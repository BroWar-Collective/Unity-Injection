using UnityEngine;
using Zenject;

namespace BroWar.Injection.Installers
{
    using BroWar.Common;

    [AddComponentMenu("BroWar/Injection/Debug Installer")]
    public class DebugInstaller : MonoInstaller<DebugInstaller>
    {
        public override void InstallBindings()
        {
            LogHandler.Log($"[Injection][{name}] Debug Installer triggered.", LogType.Log);
        }
    }
}