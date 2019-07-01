using System.Collections.Generic;

namespace Shared {
    public interface ISecurityTokenService
    {
        string GenerateToken(IUser user);
    }
}