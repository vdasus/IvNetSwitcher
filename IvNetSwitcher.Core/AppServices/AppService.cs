using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using NLog;

namespace IvNetSwitcher.Core.AppServices
{
    public class AppService: IAppService
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private Profiles _profiles;

        #region Implementation of IAppService

        public Profiles LoadData(Profiles profiles)
        {
            _profiles = profiles;
            return _profiles;
        }

        public Result RegisterProfile()
        {
            throw new System.NotImplementedException();
        }

        public Result EditProfile()
        {
            throw new System.NotImplementedException();
        }

        public Result DeleteProfile()
        {
            throw new System.NotImplementedException();
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
                    _log.Error($"Failure connecting to {_profiles.GetCurrentProfile().Name}");
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
