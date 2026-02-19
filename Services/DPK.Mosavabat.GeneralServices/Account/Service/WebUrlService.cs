using DPK.Foundation.Dependency;
using DPK.Mosavabat.DomainEntities.Base;
using DPK.Mosavabat.GeneralServices.Account.Contract;

namespace DPK.Mosavabat.GeneralServices.Account.Service;

public class WebUrlService(IAppConfigurationAccessor configurationAccessor)
    : WebUrlServiceBase(configurationAccessor), IWebUrlService, ITransientDependency
{
    public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

    public override string ServerRootAddressFormatKey => "App:WebSiteRootAddress";
}