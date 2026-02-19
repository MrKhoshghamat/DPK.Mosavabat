
using DPK.Mosavabat.Common.Extensions;

namespace DPK.Mosavabat.DomainEntities;

[DependsOn(
    typeof(DpkCommonLayerModule)

)]
public class DpkDomainLayerModule : DpkModule
{
    public override void PreInitialize()
    {

        base.IocManager.Register<AspNetPrimitiveConfiguration>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(DpkDomainLayerModule).GetAssembly());
    }

    public override void PostInitialize()
    {
    }
}