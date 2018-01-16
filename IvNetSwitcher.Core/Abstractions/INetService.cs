using System.Collections.Generic;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface INetService
    {
        IReadOnlyList<Network> ListAvailableNetworks();
        Result Connect(int index, string username, string password, string domain);
        Result CheckIsConnected();
        void Disconnect();
        string Status();
        Result<string> PrintProfileXml(int index);
        Result<string> ShowAccessPointInfo(int index);
        Result<string> DeleteProfile(int index);
    }
}