using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.Dto;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IAppService
    {
        Profiles LoadData(Profiles profiles);

        IReadOnlyList<ProfileDto> GetProfilesDtos();

        IReadOnlyList<Network> GetNetworks();
        void RegisterProfile(Profile profile);
        Result DeleteProfile(int id);

        Result GoNext();

        Result Run(Uri hostToPing, int retry);
    }
}