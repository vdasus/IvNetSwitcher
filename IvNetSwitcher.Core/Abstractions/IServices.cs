using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SimpleWifi;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IServices
    {
        List<AccessPoint> ListAccessPoints();
        Result Connect(int index, string username, string password, string domain);
        void Disconnect();
        Result Status();
        Result PrintProfileXml(int index);
        Result ShowAccessPointInfo(int index);
        Result DeleteProfile(int index);
    }
}