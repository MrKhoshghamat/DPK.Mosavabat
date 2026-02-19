using DPK.Foundation.MultiTenancy;
using DPK.Mosavabat.GeneralServices.Account.Contract;

namespace DPK.Mosavabat.GeneralServices.Account.Service;

public class AngularAppUrlService(
    IWebUrlService webUrlService,
    ITenantCache tenantCache)
    : AppUrlServiceBase(webUrlService,
        tenantCache)
{
    public override string EmailActivationRoute => "account/confirm-email";

    public override string PasswordResetRoute => "account/reset-password";
}