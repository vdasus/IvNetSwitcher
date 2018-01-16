using System;

namespace IvNetSwitcher.Core.Dto
{
    [Serializable]
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }

        public ProfileDto() {}

        public ProfileDto(int id, string name, string user, string password, string domain, string comment, bool active)
        {
            Id = id;
            Name = name;
            User = user;
            Password = password;
            Domain = domain;
            Comment = comment;
            Active = active;
        }
    }
}
