using System.Collections.Generic;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IServices
    {
        IReadOnlyList<Network> ListAvailableNetworks();
        Result Connect(int index, string username, string password, string domain);
        void Disconnect();
        Result<string> Status();
        Result<string> PrintProfileXml(int index);
        Result<string> ShowAccessPointInfo(int index);
        Result<string> DeleteProfile(int index);
    }
}