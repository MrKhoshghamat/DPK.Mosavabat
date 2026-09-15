using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;

namespace DPK.Mosavabat.Services.Contracts.CreditProcesses;

public interface IContractTransactionService
{
    /// <summary>
    /// حذف قراردادهای تسویه شده از لیست قراردادها
    /// </summary>
    RemoveSettleContractsDto RemoveSettledContracts(RemoveSettleContractsDataModel dataModel);

    /// <summary>
    /// حذف تمام قراردادهای تسویه شده از لیست قراردادها
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    RemoveSettleAllContractsDto RemoveSettledAllContracts(RemoveSettleAllContractsDataModel dataModel);

    /// <summary>
    /// دریافت قراردادهای تسویه شده
    /// </summary>
    GetSettledContractsDto GetSettledContracts(GetSettledContractsDataModel dataModel);

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش های وصولی قراردادها
    /// </summary>
    GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(GetReceivedContractsDataModel dataModel, List<GetMosavabeContractsDto>? mosavabeContracts);

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش های وصولی قراردادها
    /// </summary>
    GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(GetReceivedContractsDataModel dataModel, List<GetCustomerCurrentFacilityDto>? customerCurrentFacilities);

    /// <summary>
    /// محاسبه جمع مبالغ تراکنش های پرداختی قراردادها
    /// </summary>
    GetPaidContractsDto GetDailyPaidContractsTransactionsAmounts(GetPaidContractsDataModel dataModel);

    /// <summary>
    /// محاسبه مبالغ تراکنش های وصولی هر قرارداد
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    List<GetReceivedAllContractsDto> GetDailyReceivedAllContractsTransactionsAmounts(GetReceivedAllContractsDataModel dataModel);

}