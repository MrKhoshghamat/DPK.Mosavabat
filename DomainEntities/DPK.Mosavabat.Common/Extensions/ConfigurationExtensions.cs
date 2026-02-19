namespace DPK.Mosavabat.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetConfigModel<T>(this IModuleConfigurations moduleConfigurations) where T : class
    {

        return moduleConfigurations.DpkConfiguration.Get<T>();
    }

    public static void SetConfigModel<T>(this IModuleConfigurations moduleConfigurations, T obj) where T : class
    {
        moduleConfigurations.DpkConfiguration.Set(typeof(T).FullName, obj);
    }
}