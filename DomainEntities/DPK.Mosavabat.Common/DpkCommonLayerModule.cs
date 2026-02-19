using DPK.Mosavabat.Common.Extensions;

namespace DPK.Mosavabat.Common;

[DependsOn()]
public class DpkCommonLayerModule : DpkModule
{
    public override void PreInitialize()
    {
        Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flag-iran", true));
        Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england"));

        AppLocalizationConfigurator.Configure(Configuration.Localization);

    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(DpkCommonLayerModule).GetAssembly());
    }

    public override void PostInitialize()
    {
    }
}