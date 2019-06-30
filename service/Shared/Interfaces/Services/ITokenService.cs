using System.Collections.Generic;

namespace bab.Shared {
    public interface ITokenService
    {
        IUser GenerateToken(IUser user);
    }
}