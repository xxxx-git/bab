using System.Collections.Generic;

namespace bab.Shared {
    public interface IUserService
    {
        IEnumerable<IUser> GetAll();
    }
}