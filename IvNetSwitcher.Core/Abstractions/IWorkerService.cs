using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IWorkerService
    {
        string GetCurrentStatus();
        IReadOnlyList<Network> ListAvailableNetworks();
        void Run(Profiles profiles, Uri hostToPing, int delay, int retry, string salt, int times = 0);
    }
}