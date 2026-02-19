using DPK.Foundation.Dependency;
using DPK.Foundation.MultiTenancy;
using DPK.Mosavabat.GeneralServices.Account.Contract;
using DPK.Foundation.Extensions;

namespace DPK.Mosavabat.GeneralServices.Account.Service;

public abstract class AppUrlServiceBase(IWebUrlService webUrlService, ITenantCache tenantCache)
    : IAppUrlService, ITransientDependency
{
    public abstract string EmailActivationRoute { get; }

    public abstract string PasswordResetRoute { get; }

    protected readonly IWebUrlService WebUrlService = webUrlService;
    protected readonly ITenantCache TenantCache = tenantCache;

    public string CreateEmailActivationUrlFormat(int? tenantId)
    {
        return CreateEmailActivationUrlFormat(GetTenancyName(tenantId));
    }

    public string CreatePasswordResetUrlFormat(int? tenantId)
    {
        return CreatePasswordResetUrlFormat(GetTenancyName(tenantId));
    }

    public string CreateEmailActivationUrlFormat(string tenancyName)
    {
        var activationLink = WebUrlService.GetSiteRootAddress(tenancyName)
            .EnsureEndsWith('/') + EmailActivationRoute + "?userId={userId}&confirmationCode={confirmationCode}";

        activationLink += "&tenantId={tenantId}";

        return activationLink;
    }

    public string CreatePasswordResetUrlFormat(string tenancyName)
    {
        var resetLink = WebUrlService.GetSiteRootAddress(tenancyName).EnsureEndsWith('/') + PasswordResetRoute + "?userId={userId}&resetCode={resetCode}";

        resetLink += "&tenantId={tenantId}";

        return resetLink;
    }


    private string? GetTenancyName(int? tenantId)
    {
        return tenantId.HasValue ? TenantCache.Get(tenantId.Value).TenancyName : null;
    }
}