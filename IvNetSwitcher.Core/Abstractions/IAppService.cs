using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IAppService
    {
        Profiles LoadData(Profiles profiles);

        IReadOnlyList<Network> GetNetworks();
        Result RegisterProfile();
        Result EditProfile();
        Result DeleteProfile();

        Result GoPlay();
        Result GoNext();

        void Run(Uri hostToPing, int delay, int retry, int times = 0);
    }
}