namespace DPK.Mosavabat.Shared.ViewModels.Lcp
{
    public class UpdateRequestFacilityRadNumberVm
    {
        /// <summary>
        /// شناسه مورد
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// شناسه رهگیری
        /// </summary>
        public int RadNumber { get; set; }

        /// <summary>
        /// شناسه مورد درخواست تسهیلات ثانویه
        /// </summary>
        public int CaseIdApprovalRequest { get; set; }

        public override string ToString()
        {
            return $"CaseId: {CaseId} - RadNumber: {RadNumber} - CaseIdApprovalRequest: {CaseIdApprovalRequest}";
        }
    }
}
