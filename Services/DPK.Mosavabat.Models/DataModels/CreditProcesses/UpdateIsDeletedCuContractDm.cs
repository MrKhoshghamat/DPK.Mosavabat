namespace DPK.Mosavabat.Models.DataModels.CreditProcesses;

public class UpdateIsDeletedCuContractDm(
    int paymentPermissionId,
    bool isDeleted)
{
    public int PaymentPermissionId { get; } = paymentPermissionId;
    public bool IsDeleted { get; } = isDeleted;
}