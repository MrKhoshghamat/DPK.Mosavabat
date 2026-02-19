using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Execution;
using DPK.Mosavabat.BusinessServices.Features.CreditProcesses.Endpoints.Contract;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.Payam;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.Models.ViewModels.CreditProcesses;
using Microsoft.AspNetCore.Mvc;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.Endpoints.Service;

/// <summary>
/// سرویس های کنترل صدور مجوز پرداخت
/// </summary>
public class CreditProcessApplicationService(
    IExecutionModule executionModule)
    : ManagementServiceBase,
        ICreditProcessApplicationService
{
    /// <summary>
    /// سرویس جمع پرداختی های روز مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<AggregateDailyCustomerPaymentDto> AggregateDailyCustomerPayment(
        [FromBody] AggregateDailyCustomerPaymentVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<AggregateDailyCustomerPaymentCommand>(vm),
                cancellationToken);
    }

    /// <summary>
    /// سرویس محاسبه مانده آزاد مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CalculateApprovalRequestUnusedBalanceDto> CalculateApprovalRequestUnusedBalance(
        [FromBody] CalculateApprovalRequestUnusedBalanceVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<CalculateApprovalRequestUnusedBalanceCommand>(vm),
                cancellationToken);
    }

    /// <summary>
    /// سرویس محاسبه مانده بدهی کل مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CalculateCustomerTotalDebtDto> CalculateCustomerTotalDebt(
        [FromBody] CalculateCustomerTotalDebtVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<CalculateCustomerTotalDebtCommand>(vm),
                cancellationToken);
    }

    /// <summary>
    /// سرویس کنترل صدور مجوز پرداخت
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ControlPaymentPermissionIssuanceDto> ControlPaymentPermissionIssuance(
        [FromBody] ControlPaymentPermissionIssuanceVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<ControlPaymentPermissionIssuanceCommand>(vm),
                cancellationToken);
    }

    /// <summary>
    /// سرویس ایجاد شناسه پرداخت
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<InsertPaymentPermissionDto> InsertPaymentPermission(
        [FromBody] InsertPaymentPermissionVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule
            .ExecuteCommandAsync(
                ObjectMapper.Map<InsertPaymentPermissionCommand>(vm),
                cancellationToken);
    }

    /// <summary>
    /// سرویس بروز رسانی شناسه پرداخت | حذف
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<UpdatePaymentPermissionDto> UpdatePaymentPermission(
        [FromBody] UpdatePaymentPermissionVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<UpdatePaymentPermissionCommand>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس ایجاد فرایند اطلاعات تکمیلی قرارداد
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CreateCompleteContractInfoProcessDto> CreateCompleteContractInfoProcess(
        [FromBody] CreateCompleteContractInfoProcessVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteCommandAsync(
            ObjectMapper.Map<CreateCompleteContractInfoProcessCommand>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت اطلاعات درخواست مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(
        [FromBody] GetApprovalRequestInfoVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetApprovalRequestInfoQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت مفاد و شروط مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ApprovalRequestTermsAndConditionsDto>> GetApprovalRequestTermsAndConditions(
        [FromBody] GetApprovalRequestTermsAndConditionsVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetApprovalRequestTermsAndConditionsQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت قراردادها و مجوزهای پرداخت مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ContractDto>> GetContracts(
        [FromBody] GetContractsVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetContractsQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت بدهی قبلی مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<CustomerPastDebtDto>> GetCustomerContractsPastDebts(
        [FromBody] GetCustomerContractsPastDebtsVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetCustomerContractsPastDebtsQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت قراردادهای پرداخت شده به مشتری در روز
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<CustomerDailyPaidContractDto>> GetDailyPaidContracts(
        [FromBody] GetDailyPaidContractsVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetDailyPaidContractsQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت مجموع مبالغ درخواست مصوبه های در جریان
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(
        [FromBody] GetLiveApprovalRequestsAmountsSumVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetLiveApprovalRequestsAmountsSumQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت مجوزهای پرداخت مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<PaymentPermissionsDto>> GetPaymentPermissions(
        [FromBody] GetPaymentPermissionsVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetPaymentPermissionsQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس دریافت نوع مصوبه
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GetSubjectTypeByMosavabeNoDto> GetSubjectTypeByMosavabeNo(
        [FromBody] GetSubjectTypeByMosavabeNoVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetSubjectTypeByMosavabeNoQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس مجاز به محاسبه مجدد معدل موجودی
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(
        [FromBody] IsAllowedRecalculateAccountBalanceAverageVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<IsAllowedRecalculateAccountBalanceAverageQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس موجودی مجوز پرداخت فعال
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IsExistActivePaymentPermissionDto> IsExistActivePaymentPermission(
        [FromBody] IsExistActivePaymentPermissionVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<IsExistActivePaymentPermissionQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس آیا درخواست از طرف پورتال مشتریان حقوقی است
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IsLcpRequestDto> IsLcpRequest(
        [FromBody] IsLcpRequestVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<IsLcpRequestQuery>(vm),
            cancellationToken);
    }

    /// <summary>
    /// سرویس استعلام چک برگشتی مشتری
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CheckInquiryDto> GetCheckInquiry(
        [FromBody] GetCheckInquiryVm vm,
        CancellationToken cancellationToken = default)
    {
        return await executionModule.ExecuteQueryAsync(
            ObjectMapper.Map<GetCheckInquiryQuery>(vm),
            cancellationToken);
    }
}