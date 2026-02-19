using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DPK.Mosavabat.BusinessServices.Abstractions.Queries;
using DPK.Mosavabat.ExternalServices.Lms.Contract;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.Queries.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;

namespace DPK.Mosavabat.BusinessServices.Features.CreditProcesses.UseCases.Queries.GetContracts;

/// <summary>
/// هندلر سرویس دریافت قراردادها و مجوزهای پرداخت مصوبه
/// </summary>
internal class GetContractsQueryHandler(
    ICreditProcessSqlService creditProcessSqlService,
    ILmsExternalService lmsExternalService,
    IMapper mapper) :
    IQueryHandler<
        GetContractsQuery,
        List<ContractDto>>
{
    public async Task<List<ContractDto>> Handle(
        GetContractsQuery query,
        CancellationToken cancellationToken)
    {
        var paymentPermissions = await lmsExternalService.FetchPaymentPermissions(
            mapper.Map<FetchPaymentPermissionsDm>(query),
            cancellationToken);

        var cuContracts = await creditProcessSqlService.GetCuContracts(
            mapper.Map<GetCuContractsDm>(query),
            cancellationToken);

        var lmsContracts = paymentPermissions.Select(p => new ContractDto(
            C13: p.C13,
            ContractAmount: p.ContractAmount,
            PrepaidAmount: p.PrepaidAmount,
            MosavabeNo: p.MosavabeNo,
            BranchCode: p.BranchCode,
            CustomerNo: p.CustomerNo,
            ContractType: p.ContractType,
            IsDeleted: p.IsDeleted
        ));

        var sqlContracts = cuContracts.Select(c => new ContractDto(
            C13: c.C13,
            ContractAmount: c.ContractAmount,
            PrepaidAmount: c.PrepaidAmount,
            MosavabeNo: c.MosavabeNo,
            BranchCode: c.BranchCode,
            CustomerNo: c.CustomerNo,
            ContractType: c.ContractType,
            IsDeleted: c.IsDeleted
        ));

        var finalResult = new List<ContractDto>();
        finalResult.AddRange(lmsContracts);
        finalResult.AddRange(sqlContracts);

        return finalResult
            .DistinctBy(x => x.C13)
            .ToList();
    }
}