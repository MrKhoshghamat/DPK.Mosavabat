namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class UpdatePaymentPermissionDm(
    int id,
    bool isDeleted,
    bool isCanceled)
{
    public int Id { get; } = id;
    public bool IsDeleted { get; } = isDeleted;
    public bool IsCanceled { get; } = isCanceled;
}
