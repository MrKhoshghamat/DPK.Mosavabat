using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Abstractions.Commands;
using DPK.Mosavabat.Models.Commands.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Commands.CalculateCustomerDebtBalance;

/// <summary>
/// هندلر سرویس محاسبه مانده بدهی مشتری
/// </summary>
internal class CalculateCustomerDebtBalanceCommandHandler
    : ICommandHandler<
        CalculateCustomerDebtBalanceCommand,
        CalculateCustomerDebtBalanceDto>
{
    public async Task<CalculateCustomerDebtBalanceDto> Handle(
        CalculateCustomerDebtBalanceCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}