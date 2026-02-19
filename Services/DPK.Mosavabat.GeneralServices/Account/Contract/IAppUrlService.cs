namespace DPK.Mosavabat.GeneralServices.Account.Contract;

public interface IAppUrlService
{
    string CreateEmailActivationUrlFormat(int? tenantId);

    string CreatePasswordResetUrlFormat(int? tenantId);

    string CreateEmailActivationUrlFormat(string tenancyName);

    string CreatePasswordResetUrlFormat(string tenancyName);
}