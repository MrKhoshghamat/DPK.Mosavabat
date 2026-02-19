namespace DPK.Mosavabat.Common.Extensions;

public static class AspNetPrimitiveConfigurationExtensions
{
    public static AspNetPrimitiveConfiguration AspNetPrimitive(this IModuleConfigurations configurations) =>
        configurations.DpkConfiguration.Get<AspNetPrimitiveConfiguration>();
}


public class AspNetPrimitiveConfiguration
{
    public string LicenseCode { get; set; }
}