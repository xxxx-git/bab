using System.Collections.Generic;

namespace Shared {
    public interface ISecurityTokenService
    {
        string Generate(string content);

        string Verify(string token);
    }
}