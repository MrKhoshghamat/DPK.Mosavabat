namespace DPK.Mosavabat.ValidationServices;

[DependsOn(

    typeof(DpkDomainLayerModule),
    typeof(DpkFluentValidationModule)
)]
public class ValidationServiceLayerModule : DpkModule
{
    public override void PreInitialize()
    {
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ValidationServiceLayerModule).GetAssembly());

    }
}