namespace DPK.Mosavabat.Models;

[DependsOn(
    typeof(DpkDomainLayerModule),
    typeof(DpkAutoMapperModule)
)]
public class ModelLayerModule : DpkModule
{
    public override void PreInitialize()
    {
        //Add Config Only is Sample
        //
        Configuration.Modules.DpkAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ModelLayerModule).GetAssembly());

    }
}