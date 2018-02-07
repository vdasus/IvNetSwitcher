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
            var rez = Result.Ok();
            var curr = _profiles.GetCurrentProfile();
            if (curr.IsFailure) return Result.Fail(curr.Error);
            
            _log.Trace($"ping to {curr.Value.Name}");
            MakePing(hostToPing)
                .OnSuccess(() => 
                    _log.Trace("ping successful"))
                .OnFailure(() =>
                {
                    _log.Error($"Failure ping to {hostToPing.AbsolutePath} with {curr.Value.Name}");
                    rez = TryReconnect(retry);
                });
            return rez;
        }

        private Result TryReconnect(int retry)
        {
            var i = 0;
            while (i < _profiles.Items.Count)
            {
                var prof = _profiles.CircularGetNextProfile();
                if (prof.IsFailure) break;

                for (int j = 0; j < retry; j++)
                {
                    _log.Debug($"{j + 1} try to connect to {prof.Value.Name}");
                    if (prof.Value.Connect().IsSuccess) break;
                }

                if (prof.Value.IsConnected)
                {
                    _log.Debug($"Connected to {prof.Value.Name}");
                    return Result.Ok();
                }

                _log.Error($"Connect to {prof.Value.Name} failed");
                i++;
            }

            return Result.Fail("Can't reconnect");
        }

        #region Privates region

        private static Result MakePing(Uri hostToPing, int timeout = 120)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("PING");

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions { DontFragment = true };

            PingReply reply = pingSender.Send(hostToPing.Host, timeout, buffer, options);
            return reply.Status == IPStatus.Success ? Result.Ok() : Result.Fail("Can't make ping");
        }

        #endregion

        #endregion
    }
}
