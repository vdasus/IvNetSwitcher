using System.Collections.Generic;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IServices
    {
        List<Network> ListAvailableNetworks();
        Result Connect(int index, string username, string password, string domain);
        void Disconnect();
        Result Status();
        Result PrintProfileXml(int index);
        Result ShowAccessPointInfo(int index);
        Result DeleteProfile(int index);
    }
}