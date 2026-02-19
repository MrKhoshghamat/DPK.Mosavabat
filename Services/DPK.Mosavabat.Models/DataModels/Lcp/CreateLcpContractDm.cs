using System.Collections.Generic;
using System;

namespace DPK.Mosavabat.Models.DataModels.Lcp;

public class CreateLcpContractDm(
    int caseId,
    int cuApprovalSubjectId,
    int facilityGroupCode,
    string contractNo,
    string brManagerName,
    int employeeCode,
    DateTime contractDate,
    string branchCode,
    string contractSubject,
    long contractAmount1Int,
    long contractAmount2Int,
    string benefitRate,
    long benefitAmountInt,
    long customerCashPriceInt,
    long contractAmount3Int,
    string refundWay,
    string facilityLife,
    int countOfInstallments,
    DateTime contractDueDate,
    string interestRate,
    string allDeposit,
    int contractTemplateCode,
    string countOfContractVersion,
    DateTime requestDate,
    string accountType,
    string accountNo,
    long collateralAmount,
    string introductionLetterNo,
    DateTime introductionLetterDate,
    string documentAmountSum,
    string paymentAmountSum,
    string amountSum,
    List<RequestDocumentMosavabatDm> contractDocuments)
{
    public int CaseId { get; } = caseId;
    public int CuApprovalSubjectId { get; } = cuApprovalSubjectId;
    public int FacilityGroupCode { get; } = facilityGroupCode;
    public string ContractNo { get; } = contractNo;
    public string BrManagerName { get; } = brManagerName;
    public int EmployeeCode { get; } = employeeCode;
    public DateTime ContractDate { get; } = contractDate;
    public string BranchCode { get; } = branchCode;
    public string ContractSubject { get; } = contractSubject;
    public long ContractAmount1Int { get; } = contractAmount1Int;
    public long ContractAmount2Int { get; } = contractAmount2Int;
    public string BenefitRate { get; } = benefitRate;
    public long BenefitAmountInt { get; } = benefitAmountInt;
    public long CustomerCashPriceInt { get; } = customerCashPriceInt;
    public long ContractAmount3Int { get; } = contractAmount3Int;
    public string RefundWay { get; } = refundWay;
    public string FacilityLife { get; } = facilityLife;
    public int CountOfInstallments { get; } = countOfInstallments;
    public DateTime ContractDueDate { get; } = contractDueDate;
    public string InterestRate { get; } = interestRate;
    public string AllDeposit { get; } = allDeposit;
    public int ContractTemplateCode { get; } = contractTemplateCode;
    public string CountOfContractVersion { get; } = countOfContractVersion;
    public DateTime RequestDate { get; } = requestDate;
    public string AccountType { get; } = accountType;
    public string AccountNo { get; } = accountNo;
    public long CollateralAmount { get; } = collateralAmount;
    public string IntroductionLetterNo { get; } = introductionLetterNo;
    public DateTime IntroductionLetterDate { get; } = introductionLetterDate;
    public string DocumentAmountSum { get; } = documentAmountSum;
    public string PaymentAmountSum { get; } = paymentAmountSum;
    public string AmountSum { get; } = amountSum;
    public List<RequestDocumentMosavabatDm> ContractDocuments { get; } = contractDocuments;
}