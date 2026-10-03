using DPK.Mosavabat.Shared.DataModels.Bsi;

namespace DPK.Mosavabat.External.Services.Contracts.Bsi;

public interface ICustomerBsiExternalService
{
    Task<List<CustomerBsiModel>?> GetCustomer(BsiCustomerDataModel dm, CancellationToken cancellationToken);
    Task<IEnumerable<BalanceBsiDataModel>?> GetAllBalances(BsiBalanceRequestDataModel dm, CancellationToken cancellationToken);
    Task<IEnumerable<BalanceBsiDataModel>?> GetBalance(BsiBalanceRequestDataModel dm, CancellationToken cancellationToken);
    Task<List<BranchBsiDataModel>?> GetBranches(PagingDataModel dm, CancellationToken cancellationToken);
}