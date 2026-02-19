using DPK.Foundation.Extensions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using DPK.Mosavabat.DomainEntities.Base;

namespace DPK.Mosavabat.GeneralServices.Account.Service;

public abstract class WebUrlServiceBase(IAppConfigurationAccessor configurationAccessor)
{
    public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

    public abstract string WebSiteRootAddressFormatKey { get; }

    public abstract string ServerRootAddressFormatKey { get; }

    public string WebSiteRootAddressFormat => _appConfiguration[WebSiteRootAddressFormatKey] ?? "https://localhost:44302/";

    public string ServerRootAddressFormat => _appConfiguration[ServerRootAddressFormatKey] ?? "https://localhost:44302/";

    public bool SupportsTenancyNameInUrl
    {
        get
        {
            var siteRootFormat = WebSiteRootAddressFormat;
            return !siteRootFormat.IsNullOrEmpty() && siteRootFormat.Contains(TenancyNamePlaceHolder);
        }
    }

    readonly IConfigurationRoot _appConfiguration = configurationAccessor.Configuration;

    public string GetSiteRootAddress(string tenancyName = null)
    {
        return ReplaceTenancyNameInUrl(WebSiteRootAddressFormat, tenancyName);
    }

    public string GetServerRootAddress(string tenancyName = null)
    {
        return ReplaceTenancyNameInUrl(ServerRootAddressFormat, tenancyName);
    }

    public List<string> GetRedirectAllowedExternalWebSites()
    {
        var values = _appConfiguration["App:RedirectAllowedExternalWebSites"];
        return values?.Split(',').ToList() ?? [];
    }

    private static string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName)
    {
        if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
        {
            return siteRootFormat;
        }

        if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
        {
            siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);
        }

        if (tenancyName.IsNullOrEmpty())
        {
            return siteRootFormat.Replace(TenancyNamePlaceHolder, "");
        }

        return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
    }
}