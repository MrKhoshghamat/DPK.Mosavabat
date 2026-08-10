using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat;

public interface IRequestFacilityDatabaseService : IMainDatabaseService
{
    Task<UpdateLcpRequestFacilityDto> UpdateLcpRequestFacility(UpdateLcpRequestFacilityVm viewModel, CancellationToken cancellationToken);
    Task<MapLcpRequestFacilityToApprovalRequestDto> MapLcpRequestFacilityToApprovalRequest(MapLcpRequestFacilityToApprovalRequestVm viewModel, CancellationToken cancellationToken);
    Task<GetRequestFacilityCaseIdDto> GetRequestFacilityCaseId(GetRequestFacilityCaseIdVm viewModel, CancellationToken cancellationToken);
    Task<GetContractFileLinkDto> GetContractFileLink(GetContractFileLinkVm viewModel, CancellationToken cancellationToken);
    Task<UpdateContractFileLinkDto> UpdateContractFileLink(UpdateContractFileLinkVm viewModel, CancellationToken cancellationToken);
    Task<GetApprovalContractsDto> GetApprovalContracts(GetApprovalContractsDm dataModel, CancellationToken cancellationToken);
    Task<UpdateLcpRequestRadNumberDto> UpdateLcpRequestRadNumber(UpdateLcpRequestRadNumberDataModel dataModel, CancellationToken cancellationToken);
    Task<UpdateLcpRequestRadNumberDto> InsertReqFacilityStatus(InsertReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken);

    Task<UpdateLcpRequestRadNumberDto> UpdateReqFacilityStatus(UpdateReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken);

    Task<string?> GetApprovalRequestId(UpdateReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken);
    Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default);
    Task LcpRequestFacilityAbortCase(int caseId, CancellationToken cancellationToken);
    Task<CheckCustomerAccountExistsDto> CheckCustomerAccountExistsAsync(CheckCustomerAccountExistsDm dataModel, CancellationToken cancellationToken);
    Task<GetCustomerRankDto> GetCustomerRank(int lcpRequestFacilityId, CancellationToken cancellationToken);
    Task<List<GetCustomerDebtDetailsDto>> GetCustomerDebtDetails(GetCustomerDebtDetailsDataModel dataModel, CancellationToken cancellationToken);
    Task<GetMosavabeNoByCaseIdDto> GetMosavabeNoByCaseId(GetMosavabeNoByCaseIdDataModel dataModel, CancellationToken cancellationToken = default);
    Task<PaymentPermissionIssuanceControlForLcpDto> PaymentPermissionIssuanceControlForLcp(PaymentPermissionIssuanceControlForLcpDM dataModel, CancellationToken cancellationToken = default);

}