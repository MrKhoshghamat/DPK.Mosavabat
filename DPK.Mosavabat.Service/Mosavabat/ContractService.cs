using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Mosavabat;

public class ContractService : IContractService
{
    private readonly IRequestFacilityDatabaseService _requestFacilityDatabaseService;
    private readonly IContractDatabaseService _contractDatabaseService;
    private readonly ILoggerManager _logger;

    public ContractService(
        IRequestFacilityDatabaseService requestFacilityDatabaseService,
        IContractDatabaseService contractDatabaseService,
        ILoggerManager logger)
    {
        _requestFacilityDatabaseService = requestFacilityDatabaseService;
        _contractDatabaseService = contractDatabaseService;
        _logger = logger;
    }

    public async Task<GetContractFileLinkDto?> GetContractFileLink(GetContractFileLinkVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _requestFacilityDatabaseService.GetContractFileLink(viewModel, cancellationToken);
    }

    public async Task<GetApprovalContractsDto> GetApprovalContracts(GetApprovalContractsDm dataModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _requestFacilityDatabaseService.GetApprovalContracts(dataModel, cancellationToken);
    }

    public async Task<UpdateIsDeletedCuContractDto> UpdateIsDeletedCuContract(UpdateIsDeletedCuContractDm dataModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _contractDatabaseService.UpdateIsDeletedCuContract(dataModel, cancellationToken);
    }

    public async Task<GetContractTypeTitleByContractTypeIdDto> GetContractTypeTitleByContractTypeId(GetContractTypeTitleByContractTypeIdDm dataModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _contractDatabaseService.GetContractTypeTitleByContractTypeId(dataModel, cancellationToken);
    }

    public async Task<GetSubContractTypeTitleBySubContractTypeIdDto> GetSubContractTypeTitleBySubContractTypeId(GetSubContractTypeTitleBySubContractTypeIdDm dataModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        return await _contractDatabaseService.GetSubContractTypeTitleBySubContractTypeId(dataModel, cancellationToken);
    }
}