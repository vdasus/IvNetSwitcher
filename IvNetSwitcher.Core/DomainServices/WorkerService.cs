using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.DomainServices
{
    public class WorkerService : IWorkerService
    {
        private readonly IServices _service;

        public WorkerService(IServices service)
        {
            _service = service;
        }

        #region Implementation of IWorkerService

        public string GetCurrentStatus()
        {
            var rez = _service.Status();
            return rez.IsSuccess ? rez.Value : rez.Error;
        }

        public IReadOnlyList<Network> ListAvailableNetworks()
        {
            return _service.ListAvailableNetworks();
        }

        public void Run(Profiles profiles, Uri hostToPing, int delay, int retry, int times = 0)
        {
            var rez = MakePing(hostToPing);
            if(rez.IsFailure) throw new ApplicationException();
        }

        private static Result MakePing(Uri hostToPing, int timeout = 120)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions {DontFragment = true};
            
            PingReply reply = pingSender.Send(hostToPing.Host, timeout, buffer, options);
            return reply.Status == IPStatus.Success ? Result.Ok() : Result.Fail("Can't connect");
        }

        #endregion
    }
}
