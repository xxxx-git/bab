using System.Collections.Generic;
using System.Linq;
using bab.Shared;

namespace bab.Services {
    public class UserService : IUserService
    {
        private IList<IUser> _users = new List<IUser>
        { 
            new User { Id = "1", Hierarchy = "Test1", DisplayName = "Test1" },
            new User { Id = "2", Hierarchy = "Test2", DisplayName = "Test2" },
            new User { Id = "3", Hierarchy = "Test3", DisplayName = "Test3" },
        };

        public IEnumerable<IUser> GetAll()
        {
            return _users;
        }
    }
}