using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Shared;

namespace IvNetSwitcher.Core.Domain
{
    public class Profile
    {
        private readonly INetService _svc;
        public int Id { get; }
        public string Name { get; }
        public string User { get; }
        public string Password { get; }
        public string Domain { get; }
        public string Comment { get; }
        public bool Active { get; }

        public bool IsConnected => _svc.CheckIsConnected().IsSuccess;

        public Profile(INetService svc, int id, string name, string user, string password, string domain, string comment, bool active, string salt)
        {
            _svc = svc;
            Id = id;
            Name = name;
            User = user;
            Password = Utils.GetDecryptedString(password, salt); ;
            Domain = domain;
            Comment = comment;
            Active = active;
        }

        public Result Connect()
        {
            _svc.Connect(Id, User, Password, Domain);
            return _svc.CheckIsConnected();
        }
    }
}
