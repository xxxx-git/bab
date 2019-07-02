using System.Collections.Generic;

namespace Shared {
    public interface ISecurityTokenService
    {
        string Generate(string user);

        string Verify(string token);
    }
}