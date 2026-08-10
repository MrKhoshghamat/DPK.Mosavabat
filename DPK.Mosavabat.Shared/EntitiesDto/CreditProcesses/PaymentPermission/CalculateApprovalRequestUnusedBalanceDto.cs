namespace DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission
{
    public class CalculateApprovalRequestUnusedBalanceDto
    {
        public string Amount { get; set; }

        public long? RemainingContractsCount { get; set; }
        //public string Message { get; set; }
    }
}
