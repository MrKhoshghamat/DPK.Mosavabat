
using DPK.Mosavabat.Common.Consts;

namespace DPK.Mosavabat.Api.NavigationBar;

/// <summary>
/// Navigation provider 
/// </summary>
public class MosavabatNavigationProvider : NavigationProvider
{


    /// <summary>
    /// Localize
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ILocalizableString L(string name)
    {
        return new LocalizableString(name, MosavabatConsts.LocalizationSourceName);
    }


    /// <summary>
    /// Set navigation
    /// </summary>
    /// <param name="context"></param>
    public override void SetNavigation(INavigationProviderContext context)
    {
        context.Manager.MainMenu
            .AddItem(
                new MenuItemDefinition(
                    "اقدامات",
                    L("Tasks"),
                    url: "/Tasks",
                    icon: "fa fa-tasks"
                )
            ).AddItem(
                new MenuItemDefinition(
                    "گزارشات",
                    L("Reports"),
                    url: "/Reports",
                    icon: "fa fa-bar-chart"
                )
            ).AddItem(
                new MenuItemDefinition(
                    "مدیر سیستم",
                    L("Administration"),
                    icon: "fa fa-cogs"
                ).AddItem(
                    new MenuItemDefinition(
                        "مدیریت کاربران",
                        L("UserManagement"),
                        url: "/Administration/Users",
                        icon: "fa fa-users"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        "مدیریت رولها",
                        L("RoleManagement"),
                        url: "/Administration/Roles",
                        icon: "fa fa-star"
                    )
                )
            );
    }
}