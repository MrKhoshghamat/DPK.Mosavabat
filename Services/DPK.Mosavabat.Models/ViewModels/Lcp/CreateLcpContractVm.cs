using System.Collections.Generic;
using System;

namespace DPK.Mosavabat.Models.ViewModels.Lcp;

public class CreateLcpContractVm
{
    public int CaseId { get; set; }
    public int CuApprovalSubjectId { get; set; }
    public int FacilityGroupCode { get; set; }
    public string ContractNo { get; set; }
    public string BrManagerName { get; set; }
    public int EmployeeCode { get; set; }
    public DateTime ContractDate { get; set; }
    public string BranchCode { get; set; }
    public string ContractSubject { get; set; }
    public long ContractAmount1Int { get; set; }
    public long ContractAmount2Int { get; set; }
    public string BenefitRate { get; set; }
    public long BenefitAmountInt { get; set; }
    public long CustomerCashPriceInt { get; set; }
    public long ContractAmount3Int { get; set; }
    public string RefundWay { get; set; }
    public string FacilityLife { get; set; }
    public int CountOfInstallments { get; set; }
    public DateTime ContractDueDate { get; set; }
    public string InterestRate { get; set; }
    public string AllDeposit { get; set; }
    public int ContractTemplateCode { get; set; }
    public string CountOfContractVersion { get; set; }
    public DateTime RequestDate { get; set; }
    public string AccountType { get; set; }
    public string AccountNo { get; set; }
    public long CollateralAmount { get; set; }
    public string IntroductionLetterNo { get; set; }
    public DateTime IntroductionLetterDate { get; set; }
    public string DocumentAmountSum { get; set; }
    public string PaymentAmountSum { get; set; }
    public string AmountSum { get; set; }
    public List<RequestDocumentMosavabatVm> ContractDocuments { get; set; }
}