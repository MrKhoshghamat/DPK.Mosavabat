namespace DPK.Mosavabat.Api.Extensions;

/// <summary>
/// Session Base
/// </summary>
public class DpkMosavabatSessionBase : DpkSessionBase

{
    public override long? UserId => 2;
    public override int? TenantId => 1;
    public override long? ImpersonatorUserId => new long?();

    public override int? ImpersonatorTenantId => new int?();


    /// <summary>
    /// Overload
    /// </summary>
    /// <param name="multiTenancy"></param>
    /// <param name="sessionOverrideScopeProvider"></param>
    public DpkMosavabatSessionBase(
        IMultiTenancyConfig multiTenancy,
        IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) :
        base(multiTenancy, sessionOverrideScopeProvider)
    {
    }
}