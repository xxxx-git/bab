using System.Collections.Generic;

namespace Shared {
    public interface IUserService
    {
        IEnumerable<IUser> GetAll();
    }
}