namespace DPK.Mosavabat.Models.DataModels.Proxies.Lms;

public class UpdatePaymentPermissionProxyDm(
    int id,
    bool isDeleted,
    bool isCanceled)
{
    public int Id { get; } = id;
    public bool IsDeleted { get; } = isDeleted;
    public bool IsCanceled { get; } = isCanceled;
}