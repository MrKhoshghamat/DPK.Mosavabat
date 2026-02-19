using DPK.Mosavabat.Models.Settings.Abrisham;
using Dpk.UseAdanicAbrishamService.Services;
using Microsoft.Extensions.Options;

namespace DPK.Mosavabat.ExternalServices.Abrisham.Loan.Proxy;

public class LoanProxyService
    : ILoanProxyService
{
    private readonly AdanicAbrishamService _adanicAbrishamService;

    public LoanProxyService(
        IOptions<AbrishamServiceSettings> options)
    {
        AdanicAbrishamService.TenantId = options.Value.TenantId;
        _adanicAbrishamService = new AdanicAbrishamService();
    }

    public async Task<LoanBillDto> GetLoanBillAsync(
        LoanBillVM vm,
        CancellationToken cancellationToken)
    {
        vm.BillType = "03";
        vm.Phase = "00";
        vm.Terminal = "1111";

        return await _adanicAbrishamService.LoanBillAsync(
            vm,
            cancellationToken);
    }

    public async Task<LoanBillV2Dto> GetLoanBillV2Async(
        LoanBillV2VM vm,
        CancellationToken cancellationToken)
    {
        vm.BillType = "03";
        vm.Phase = "00";
        vm.Terminal = "1111";

        return await _adanicAbrishamService.LoanBill2Async(
            vm,
            cancellationToken);
    }
}