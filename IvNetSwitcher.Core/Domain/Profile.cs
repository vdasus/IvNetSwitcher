using System;
using System.Xml.Serialization;
using IvNetSwitcher.Core.Shared;

namespace IvNetSwitcher.Core.Domain
{
    [Serializable]
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }

        public Profile() {}

        public Profile(int id, string name, string user, string password, string domain, string comment, bool active)
        {
            Id = id;
            Name = name;
            User = user;
            Password = password;
            Domain = domain;
            Comment = comment;
            Active = active;
        }

        public string GetDecPwd(string salt)
        {
            return Utils.GetDecryptedString(Password, salt);
        }
    }
}
