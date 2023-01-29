namespace BroWar.Injection
{
    public interface ISubProcessor<T>
    {
        /// <summary>
        /// Called right before the installation process.
        /// </summary>
        /// <param name="data">Data used to initialize the context installer.</param>
        void OnBeginInstallation(T data);
        /// <summary>
        /// Called right after the installation process.
        /// </summary>
        /// <param name="data">Data used to initialize the context installer.</param>
        void OnCloseInstallation(T data);
    }
}