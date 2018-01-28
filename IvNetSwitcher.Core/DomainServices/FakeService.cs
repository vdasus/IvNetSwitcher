using System;
using System.Collections.Generic;
using System.Threading;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.DomainServices
{
    public class FakeService: INetService
    {
        #region Implementation of INetService

        public IReadOnlyList<Network> ListAvailableNetworks()
        {
            var result = new List<Network>
            {
                new Network(1, "name of network", 10, true, true, true),
                new Network(2, "name2 of network home", 1, true, false, false),
                new Network(3, "name3 of network work just a test of long name", 5, true, true, false)
            };
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return result;
        }

        public Result Connect(int index, string username, string password, string domain)
        {
            return Result.Ok();
        }

        public Result CheckIsConnected()
        {
            return Result.Ok();
        }

        public void Disconnect()
        {
        }

        public string Status()
        {
            return "Connected";
        }

        public Result<string> PrintProfileXml(int index)
        {
            return Result.Ok(@"<test>test</test>");
        }

        public Result<string> ShowAccessPointInfo(int index)
        {
            return Result.Ok("AccessPointInfo");
        }

        public Result<string> DeleteProfile(int index)
        {
            return Result.Ok("deleted");
        }

        #endregion
    }
}
