using DPK.Foundation.Identity.Extentions;
using DPK.Mosavabat.Common.Consts;
using Microsoft.AspNetCore.Identity;

namespace DPK.Mosavabat.BusinessServices;

/// <summary>
/// Management service base
/// </summary>
public class ManagementServiceBase : ApplicationService
{
     
    /// <summary>
    /// Constructor
    /// </summary>
    public ManagementServiceBase()
    {
        this.LocalizationSourceName = MosavabatConsts.LocalizationSourceName;

    }



    //public virtual   Task GetCurrentUserAsync()
    //{
    //    return Task.CompletedTask;

    //}
    //public virtual   Task GetCurrentUser()
    //{
    //    return Task.CompletedTask;

    //}


    /// <summary>
    /// Check errors
    /// </summary>
    /// <param name="identityResult"></param>
    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}