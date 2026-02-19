namespace DPK.Mosavabat.DomainEntities.Base;

public interface IAppConfigurationAccessor
{
    IConfigurationRoot Configuration { get; }
}