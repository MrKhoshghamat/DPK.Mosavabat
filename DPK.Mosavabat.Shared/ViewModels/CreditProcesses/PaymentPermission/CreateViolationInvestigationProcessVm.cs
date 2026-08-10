namespace DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission
{
    public class CreateViolationInvestigationProcessVm
    {
        public DateTime CreateDate { get; set; }
        public int? CuApprovalSubject { get; set; }
        public string? ContractNo { get; set; }
        public int? Branches { get; set; }
        public int? InfractionTypeDataModel { get; set; }
        public int? Inspector { get; set; }
    }
}
