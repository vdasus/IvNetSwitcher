using System.Collections.Generic;
using System.Linq;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Dto;

namespace IvNetSwitcher.Core.Domain
{
    public class Profiles
    {
        private readonly INetService _net;
        private readonly string _salt;
        private int _currentId;
        public IReadOnlyList<Profile> Items { get; }

        public Profiles(INetService net, IReadOnlyList<ProfileDto> items, string salt)
        {
            _net = net;
            _salt = salt;
            Items = FillProfiles(items);
            _currentId = Items.FirstOrDefault(x => x.Active).Id;
        }

        private IReadOnlyList<Profile> FillProfiles(IEnumerable<ProfileDto> items)
        {
            return items.Select(zz =>
                new Profile(_net, zz.Id, zz.Name, zz.User, zz.Password, zz.Domain, zz.Comment, zz.Active, _salt)).ToList();
        }

        public Profile GetCurrentProfile()
        {
            return Items.FirstOrDefault(x => x.Id == _currentId);
        }

        public Profile CircularGetNextProfile()
        {
            var index = _currentId;

            var result = Items.FirstOrDefault(x => x.Id > _currentId) ?? Items.OrderBy(x => x.Id).FirstOrDefault();
            _currentId = result.Id;

            return result;
        }
    }
}
