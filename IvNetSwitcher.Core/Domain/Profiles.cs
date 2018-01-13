using System.Collections.Generic;
using System.Linq;

namespace IvNetSwitcher.Core.Domain
{
    public class Profiles
    {
        private int _currentId;
        public IReadOnlyList<Profile> Items { get; }

        public Profiles(IReadOnlyList<Profile> items)
        {
            Items = items;
            _currentId = Items.FirstOrDefault(x => x.Active).Id;
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
