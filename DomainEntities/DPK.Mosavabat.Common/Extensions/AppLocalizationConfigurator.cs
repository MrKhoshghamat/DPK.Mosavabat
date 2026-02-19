using DPK.Mosavabat.Common.Consts;

namespace DPK.Mosavabat.Common.Extensions;

public static class AppLocalizationConfigurator
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(
                MosavabatConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(AppLocalizationConfigurator).GetAssembly(),
                    "DPK.Mosavabat.Common.Localization.Source"
                )
            )
        );
    }
}