using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using NLog;

namespace IvNetSwitcher.Core.DomainServices
{
    public class WorkerService : IWorkerService
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IServices _service;

        public WorkerService(IServices service)
        {
            _service = service;
        }

        public string GetCurrentStatus()
        {
            var rez = _service.Status();
            return rez.IsSuccess ? rez.Value : rez.Error;
        }

        public IReadOnlyList<Network> ListAvailableNetworks()
        {
            return _service.ListAvailableNetworks();
        }

        public void Run(Profiles profiles, Uri hostToPing, int delay, int retry, string salt, int times = 0)
        {
            // TODO code just to check
            int i = 0;
            while (times == 0 || i < times) 
            {
                _log.Trace($"ping to {profiles.GetCurrentProfile().Name} successful");
                var rez = MakePing(hostToPing);

                if (rez.IsFailure)
                {
                    _log.Error($"Failure connecting to {profiles.GetCurrentProfile().Name}");
                    var prof = profiles.CircularGetNextProfile();

                    for (int j = 0; j < retry; j++)
                    {
                        _log.Debug($"Trying to connect to {prof.Name}");
                        var connected = _service.Connect(prof.Id, prof.User, prof.GetDecPwd(salt), prof.Domain);
                        if (connected.IsSuccess) break;
                    }
                };
                Thread.Sleep(TimeSpan.FromSeconds(delay));
                i++;
            }
        }

        #region Privates region

        private static Result MakePing(Uri hostToPing, int timeout = 120)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("PING");

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions { DontFragment = true };

            PingReply reply = pingSender.Send(hostToPing.Host, timeout, buffer, options);
            return reply.Status == IPStatus.Success ? Result.Ok() : Result.Fail("Can't connect");
        }

        #endregion
    }
}
