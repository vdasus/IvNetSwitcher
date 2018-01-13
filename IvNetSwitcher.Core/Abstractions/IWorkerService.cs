using System.Collections.Generic;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IWorkerService
    {
        string GetCurrentStatus();
        IReadOnlyList<Network> ListAvailableNetworks();
        void Run(int delay, int retry, int times = 0);
    }
}