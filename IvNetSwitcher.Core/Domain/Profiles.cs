using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Dto;

namespace IvNetSwitcher.Core.Domain
{
    public class Profiles
    {
        private readonly INetService _net;
        private readonly IUtilsService _svc;
        
        private Profile _currentProfile;
        public IList<Profile> Items { get; }

        public Profiles(INetService net, IUtilsService svc, IEnumerable<ProfileDto> preofilesDto)
        {
            _net = net;
            _svc = svc;
            
            Items = FillProfiles(preofilesDto);
            _currentProfile = Items.FirstOrDefault(x => x.Active);
        }

        public Profile GetCurrentProfile()
        {
            return Items.FirstOrDefault(x => x.Id == _currentProfile.Id);
        }

        public int GetCurrentProfileId()
        {
            return _currentProfile.Id;
        }

        public void AddProfile(Profile profile)
        {
            Items.Add(profile);
        }

        public Result DeleteProfile(int id)
        {
            var tmp = Items.First(x => x.Id == id);
            if (tmp == null) return Result.Fail($"Not found profile with id={id}");

            Items.Remove(tmp);
            return Result.Ok();
        }

        public IReadOnlyList<ProfileDto> GetProfilesDtos()
        {
            return Items.Select(zz => zz.GetProfileDto()).ToList();
        }

        public Profile CircularGetNextProfile()
        {
            _currentProfile = Items.FirstOrDefault(x => x.Id > _currentProfile.Id) ?? Items.OrderBy(x => x.Id).FirstOrDefault();
            return _currentProfile;
        }

        private IList<Profile> FillProfiles(IEnumerable<ProfileDto> items)
        {
            return items.Select(zz =>
                new Profile(_net, _svc, zz.Id, zz.Name, zz.User, zz.Password, zz.Domain, zz.Comment, zz.Active)).ToList();
        }
    }
}
