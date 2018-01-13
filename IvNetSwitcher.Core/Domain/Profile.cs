using CSharpFunctionalExtensions;

namespace IvNetSwitcher.Core.Domain
{
    public class Profile: ValueObject<Profile>
    {
        public int Id { get; }
        public string Name { get; }
        public string User { get; }
        public string Password { get; }
        public string Domain { get; }
        public string Comment { get; }

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
