using DPK.Mosavabat.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace DPK.Mosavabat.Services.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(RepairAccessTokenDto tokenDto);
}