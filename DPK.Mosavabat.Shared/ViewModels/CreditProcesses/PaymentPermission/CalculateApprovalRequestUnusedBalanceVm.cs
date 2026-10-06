namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission
{
    public class CalculateApprovalRequestUnusedBalanceVm
    {
        public string MosavabeNumber { get; set; }
        public IEnumerable<string>? SettledC13List { get; set; }
    }
}
