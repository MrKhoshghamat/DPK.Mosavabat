using DPK.Mosavabat.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DPK.Mosavabat.Services.Extensions;

public static class UserManagerExtensions
{
    public static async Task<User?> FindByAccessTokenAsync(this UserManager<User> userManager, string? refreshToken)
    {
        return await userManager.Users?.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken)!;
    }
}