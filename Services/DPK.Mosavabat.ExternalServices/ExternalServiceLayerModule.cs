namespace DPK.Mosavabat.ExternalServices
{
    [DependsOn(
     typeof(DpkDomainLayerModule)
     )]
    public class ExternalServiceLayerModule : DpkModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ExternalServiceLayerModule).GetAssembly());

        }
    }
}