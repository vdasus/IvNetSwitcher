using System;
using System.Collections.Generic;
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

        public Result Run(Uri hostToPing, int retry)
        {
            _log.Trace($"ping to {_profiles.GetCurrentProfile().Name}");
            var rez = MakePing(hostToPing)
                .OnFailure(() =>
                {
                    _log.Error(
                        $"Failure ping to {hostToPing.AbsolutePath} with {_profiles.GetCurrentProfile().Name}");
                    TryReconnect(hostToPing, retry);
                })
                .OnSuccess(() =>
                    _log.Trace("ping successful"));
            return _profiles.GetCurrentProfile().IsConnected
                ? Result.Ok()
                : Result.Fail($"Can't connect to any");
        }

        private void TryReconnect(Uri hostToPing, int retry)
        {
            var i = 0;
            while (i < _profiles.Items.Count)
            {
                var prof = _profiles.CircularGetNextProfile();

                for (int j = 0; j < retry; j++)
                {
                    _log.Debug($"{j + 1} try to connect to {prof.Name}");
                    if (prof.Connect().IsSuccess) break;
                }

                if (prof.IsConnected)
                {
                    _log.Debug($"Connected to {prof.Name}");
                    return;
                }

                _log.Error($"Connect to {prof.Name} failed");
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
