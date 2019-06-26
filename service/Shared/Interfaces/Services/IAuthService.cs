using System.Collections.Generic;

namespace bab.Shared {
    public interface IAuthService
    {
        IUser Authenticate(IUser user);
    }
}