using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.AppServices
{
    public class AppService: IAppService
    {
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

        #endregion
    }
}
