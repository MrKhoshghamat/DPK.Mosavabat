using DPK.Mosavabat.Shared.DataModels.ForLcp;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using UpdateRequestFacilityDto = DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract.UpdateRequestFacilityDto;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat;

public interface IRequestFacilityService
{
    Task<RegisterRequestFacilityDto?> RegisterRequestFacilityAsync(RegisterRequestFacilityVm viewModel, CancellationToken cancellationToken = default);
    Task<GetContractFileLinkDto?> GetContractFileLink(GetContractFileLinkVm viewModel, CancellationToken cancellationToken = default);
    Task<UpdateContractFileLinkDto> UpdateContractFileLink(UpdateContractFileLinkVm viewModel, CancellationToken cancellationToken = default);
    Task<UpdateRequestFacilityDto?> UpdateRequestFacility(UpdateRequestFacilityVm viewModel, CancellationToken cancellationToken = default);
    Task<SaveRequestFacilityDto?> SaveRequestFacility(SaveRequestFacilityVm viewModel, CancellationToken cancellationToken = default);
    Task<AbortCasesDto> AbortCasesAsync(AbortCasesVm viewModel, CancellationToken cancellationToken = default);

    // ثبت درخواست تسهیلات
    Task<MosavabatResponseDataModel?> RegisterRequestFacilityAsyncV2(RequestFacilityUpdateDataModel requestFacility);
    Task<MosavabatResponseDataModel?> NewUpdateRequestFacilityV2(RequestFacilityUpdateDataModel requestFacility, CancellationToken cancellationToken = default);
    Task<string?> GetEntitiesUsingSchemaAsyncV2(FilterRequestFacilityDataModel dataModel);
    Task<UpdateLcpRequestRadNumberDto> UpdateLcpRequestRadNumber(UpdateLcpRequestRadNumberDataModel dataModel, CancellationToken cancellationToken = default);
    Task<CheckCustomerAccountExistsDto> CheckCustomerAccountExistsAsync(CheckCustomerAccountExistsDm dataModel, CancellationToken cancellationToken = default);

    Task<UpdateLcpRequestRadNumberDto> InsertReqFacilityStatus(InsertReqFacilityStatusDataModel dataModel,
        CancellationToken cancellationToken = default);

    Task<RemoveRequestFromCartableDto> RemoveRequestFromCartable(AbortCasesVm viewModel, CancellationToken cancellationToken = default);
    Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default);
    Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default);
    Task LcpRequestFacilityAbortCase(int caseId, CancellationToken cancellationToken);
    Task<List<GetCustomerDebtDetailsDto>> GetCustomerDebtDetails(GetCustomerDebtDetailsDataModel dataModel, CancellationToken cancellationToken = default);
    Task<CalculateRequestFacilityUnusedBalanceDto> CalculateRequestFacilityUnusedBalance(CalculateRequestFacilityUnusedBalanceDataModel dataModel , CancellationToken cancellationToken = default);
    Task<PaymentPermissionIssuanceControlForLcpDto> PaymentPermissionIssuanceControlForLcp(PaymentPermissionIssuanceControlForLcpDataModel dataModel, CancellationToken cancellationToken = default);
    Task<SendApprovalLetterDto> SendApprovalLetter(SendApprovalLetterVm vm, CancellationToken cancellationToken = default);
}