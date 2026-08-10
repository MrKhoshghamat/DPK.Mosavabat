using DPK.Mosavabat.Shared.EntitiesDto.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using Dpk.UseAdanicAbrishamService.Services;

namespace DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan
{
    public interface ILoanExternalService
    {
        Task<LoanInfoDto> GetLoanInfo(GetLoanInfoVm viewModel, CancellationToken cancellationToken = default);
        Task<LoanBillDto> GetLoanBill(GetLoanBillVm viewModel, CancellationToken cancellationToken = default);
        Task<GetSettledLoanBillDto> GetSettledLoanBill(GetSettledLoanBillVm viewModel, CancellationToken cancellationToken = default);
        Task<GetPayedLoanBillDto> GetPayedLoanBill(GetPayedLoanBillVm viewModel, CancellationToken cancellationToken = default);
        Task<GetReceivedLoanBillDto> GetReceivedLoanBill(GetReceivedLoanBillVm viewModel, CancellationToken cancellationToken = default);
        Task<GetLoanCustomerFacilitiesDto> GetLoanCustomerFacilities(GetLoanCustomerFacilitiesVm viewModel, CancellationToken cancellationToken = default);
        Task<LoanContractsAllStatementDto> GetLoanContractsAllStatement(GetLoanContractsAllStatementVm viewModel, CancellationToken cancellationToken = default);
    }
}
