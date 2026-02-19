namespace DPK.Mosavabat.SqlBusinessServices;

[DependsOn(
    typeof(DpkDomainLayerModule)
)]
public class SqlBusinessServiceLayerModule : DpkModule
{
    public override void PreInitialize()
    {
    }

    public override void Initialize()
    {
        //IocManager.Register<ISqlRepository, SqlRepository>();
        IocManager.RegisterAssemblyByConvention(typeof(SqlBusinessServiceLayerModule).GetAssembly());

    }
}