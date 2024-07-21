using Shared.DTOs.Identity;

namespace Contract.Identity;

public interface ITokenService
{
    TokenResponse GetToken(TokenRequest tokenRequest);
}
