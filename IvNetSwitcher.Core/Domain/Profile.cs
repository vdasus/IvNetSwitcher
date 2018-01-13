using CSharpFunctionalExtensions;

namespace IvNetSwitcher.Core.Domain
{
    public class Profile: ValueObject<Profile>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Comment { get; set; }

        public Profile() { }

        public Profile(int id, string name, string user, string password, string domain, string comment)
        {
            Id = id;
            Name = name;
            User = user;
            Password = password;
            Domain = domain;
            Comment = comment;
        }

        #region Overrides of ValueObject<Profile>

        protected override bool EqualsCore(Profile other)
        {
            return Id == other.Id 
                && Name == other.Name
                && User == other.User
                && Password == other.Password
                && Domain == other.Domain
                && Comment == other.Comment;
        }

        protected override int GetHashCodeCore()
        {
            int hashCode = Id.GetHashCode();
            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            hashCode = (hashCode * 397) ^ User.GetHashCode();
            hashCode = (hashCode * 397) ^ Password.GetHashCode();
            hashCode = (hashCode * 397) ^ Domain.GetHashCode();
            hashCode = (hashCode * 397) ^ Comment.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}
