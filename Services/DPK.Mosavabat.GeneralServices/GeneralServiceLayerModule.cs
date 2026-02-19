namespace DPK.Mosavabat.GeneralServices;

[DependsOn(
    typeof(DpkDomainLayerModule)
)]
public class GeneralServiceLayerModule : DpkModule
{
    public override void PreInitialize()
    {
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(GeneralServiceLayerModule).GetAssembly());

    }
}