using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.Payam;
using DPK.Mosavabat.Models.ViewModels.CreditProcesses;
using Microsoft.AspNetCore.Mvc;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.Endpoints.Contract;

/// <summary>
/// سرویس های کنترل صدور مجوز پرداخت
/// </summary>
public interface ICreditProcessApplicationService
    : IApplicationService
{
    /// <summary>
    /// سرویس جمع پرداختی های روز مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<AggregateDailyCustomerPaymentDto> AggregateDailyCustomerPayment(
        [FromBody] AggregateDailyCustomerPaymentVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس محاسبه مانده آزاد مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<CalculateApprovalRequestUnusedBalanceDto> CalculateApprovalRequestUnusedBalance(
        [FromBody] CalculateApprovalRequestUnusedBalanceVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس محاسبه مانده بدهی کل مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<CalculateCustomerTotalDebtDto> CalculateCustomerTotalDebt(
        [FromBody] CalculateCustomerTotalDebtVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس کنترل صدور مجوز پرداخت
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<ControlPaymentPermissionIssuanceDto> ControlPaymentPermissionIssuance(
        [FromBody] ControlPaymentPermissionIssuanceVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس ایجاد شناسه پرداخت
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<InsertPaymentPermissionDto> InsertPaymentPermission(
        [FromBody] InsertPaymentPermissionVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس بروز رسانی شناسه پرداخت | حذف
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<UpdatePaymentPermissionDto> UpdatePaymentPermission(
        [FromBody] UpdatePaymentPermissionVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس ایجاد فرایند اطلاعات تکمیلی قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<CreateCompleteContractInfoProcessDto> CreateCompleteContractInfoProcess(
        [FromBody] CreateCompleteContractInfoProcessVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت اطلاعات درخواست مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(
        [FromBody] GetApprovalRequestInfoVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت مفاد و شروط مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<List<ApprovalRequestTermsAndConditionsDto>> GetApprovalRequestTermsAndConditions(
        [FromBody] GetApprovalRequestTermsAndConditionsVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت قراردادها و مجوزهای پرداخت مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<List<ContractDto>> GetContracts(
        [FromBody] GetContractsVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت بدهی قبلی مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<List<CustomerPastDebtDto>> GetCustomerContractsPastDebts(
        [FromBody] GetCustomerContractsPastDebtsVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت قراردادهای پرداخت شده به مشتری در روز
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<List<CustomerDailyPaidContractDto>> GetDailyPaidContracts(
        [FromBody] GetDailyPaidContractsVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت مجموع مبالغ درخواست مصوبه های در جریان
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(
        [FromBody] GetLiveApprovalRequestsAmountsSumVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت مجوزهای پرداخت مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<List<PaymentPermissionsDto>> GetPaymentPermissions(
        [FromBody] GetPaymentPermissionsVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس دریافت نوع مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<GetSubjectTypeByMosavabeNoDto> GetSubjectTypeByMosavabeNo(
        [FromBody] GetSubjectTypeByMosavabeNoVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس مجاز به محاسبه مجدد معدل موجودی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(
        [FromBody] IsAllowedRecalculateAccountBalanceAverageVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس موجودی مجوز پرداخت فعال
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<IsExistActivePaymentPermissionDto> IsExistActivePaymentPermission(
        [FromBody] IsExistActivePaymentPermissionVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس آیا درخواست از طرف پورتال مشتریان حقوقی است
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<IsLcpRequestDto> IsLcpRequest(
        [FromBody] IsLcpRequestVm vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// سرویس استعلام چک برگشتی مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    Task<CheckInquiryDto> GetCheckInquiry(
        [FromBody] GetCheckInquiryVm vm,
        CancellationToken cancellationToken = default);
}