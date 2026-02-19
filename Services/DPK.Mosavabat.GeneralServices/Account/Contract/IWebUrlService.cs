using System.Collections.Generic;

namespace DPK.Mosavabat.GeneralServices.Account.Contract;

public interface IWebUrlService
{
    string WebSiteRootAddressFormat { get; }

    string ServerRootAddressFormat { get; }

    bool SupportsTenancyNameInUrl { get; }

    string GetSiteRootAddress(string tenancyName = null);

    string GetServerRootAddress(string tenancyName = null);

    List<string> GetRedirectAllowedExternalWebSites();
}