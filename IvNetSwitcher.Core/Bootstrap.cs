using DryIoc;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.DomainServices;

namespace IvNetSwitcher.Core
{
    public static class Bootstrap
    {
        public static Container Container { get; set; }

        static Bootstrap()
        {
            Container = new Container();
            Container.Register<INetService, WiFiService>(Reuse.Transient);
            Container.Register<IWorkerService, WorkerService>(Reuse.Singleton);
        }
    }
}
