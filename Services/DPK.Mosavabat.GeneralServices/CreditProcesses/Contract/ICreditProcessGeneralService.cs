using System.Threading;
using DPK.Foundation.Dependency;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.GeneralServices.CreditProcesses.Contract;

public interface ICreditProcessGeneralService
    : ITransientDependency
{
    GetReceivedContractsDto GetDailyReceivedContractsTransactionsAmounts(
        GetReceivedContractsDm dm,
        CancellationToken cancellationToken);

    GetDailyPaidContractsTransactionsAmountsDto GetDailyPaidContractsTransactionsAmounts(
        GetDailyPaidContractsTransactionsAmountsDm dm,
        CancellationToken cancellationToken);

    GetDailySettledContractsDto GetDailySettledContracts(
        GetDailySettledContractsDm dm,
        CancellationToken cancellationToken);

    RemoveSettledContractsDto RemoveSettledContracts(
        RemoveSettledContractsDm dm,
        CancellationToken cancellationToken);
}