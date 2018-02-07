using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.Dto;
using NLog;

namespace IvNetSwitcher.Core.AppServices
{
    public class AppService: IAppService
    {
        private readonly INetService _ds;
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private Profiles _profiles;

        public AppService(INetService ds)
        {
            _ds = ds;
        }

        #region Implementation of IAppService

        public Profiles LoadData(Profiles profiles)
        {
            _profiles = profiles;
            return _profiles;
        }

        public IReadOnlyList<ProfileDto> GetProfilesDtos()
        {
            return _profiles.GetProfilesDtos();
        }

        public IReadOnlyList<Network> GetNetworks()
        {
            return _ds.ListAvailableNetworks();
        }

        public void RegisterProfile(Profile profile)
        {
            _profiles.AddProfile(profile);
        }

        public Result DeleteProfile(int id)
        {
            return _profiles.DeleteProfile(id);
        }

        public Result GoPlay()
        {
            throw new System.NotImplementedException();
        }

        public Result GoNext()
        {
            throw new System.NotImplementedException();
        }

        public void Run(Uri hostToPing, int delay, int retry, int times = 0)
        {
            // TODO code just to check
            int i = 0;
            while (times == 0 || i < times)
            {
                _log.Trace($"ping to {_profiles.GetCurrentProfile().Name}");
                var rez = MakePing(hostToPing);
                _log.Trace("ping successful");

                if (rez.IsFailure)
                {
                    _log.Error($"Failure ping to {hostToPing.AbsolutePath} with {_profiles.GetCurrentProfile().Name}");
                    var prof = _profiles.CircularGetNextProfile();

                    for (int j = 0; j < retry; j++)
                    {
                        _log.Debug($"Trying to connect to {prof.Name}");
                        prof.Connect();
                        if (prof.IsConnected) break;
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

        #endregion
    }
}
