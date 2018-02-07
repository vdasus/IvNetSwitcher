using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Dto;

namespace IvNetSwitcher.Core.Domain
{
    public class Profile
    {
        private readonly INetService _netsvc;
        private readonly IUtilsService _svc;
        public int Id { get; }
        public string Name { get; }
        public string User { get; }
        public string Password { get; }
        public string Domain { get; }
        public string Comment { get; }
        public bool Active { get; }

        public bool IsConnected => _netsvc.CheckIsConnected().IsSuccess;

        public Profile(INetService netsvc, IUtilsService svc, int id, string name, string user, string password, string domain, string comment, bool active)
        {
            _netsvc = netsvc;
            _svc = svc;

            Id = id;
            Name = name;
            User = user;
            Password = password;
            Domain = domain;
            Comment = comment;
            Active = active;
        }

        public Result Connect()
        {
            _netsvc.Connect(Id, User, _svc.GetDecryptedString(Password), Domain);
            return _netsvc.CheckIsConnected();
        }

        public ProfileDto GetProfileDto()
        {
            return new ProfileDto(Id, Name, User, _svc.GetEncryptedString(Password), Domain, Comment, Active);
        }
    }
}
