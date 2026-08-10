using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Services.Contracts;

public interface ILmsService
{
    Task<BaseResponse<LmsResponse>> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel);
    Task<BaseResponse<LmsResponse>> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel);
    Task<BaseResponse<LmsResponse>> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel);
    Task<BaseResponse<List<GetPaymentPermissionDto>>> FetchPaymentPermissions(GetPaymentPermissionListDataModel dataModel);
    Task<GetIsCanceledPaymentPermissionsDto> FetchIsCanceledPaymentPermissions();
    Task<BaseResponse<LmsResponse>> RegisterPaymentPermissionExceptionReason(RegisterPaymentPermissionExceptionReasonsDataModel dataModel);
    Task<BaseResponse<List<GetPaymentPermissionExceptionReasonsDto>>> GetPaymentPermissionExceptionReasons(GetPaymentPermissionExceptionReasonsDataModel dataModel);
    Task<BaseResponse<BoundaryComplianceInquiryDto>> CheckBoundaryComplianceInquiry(BoundaryComplianceInquiryDataModel dataModel);
    Task<MosavabatResponseDataModel?> CreateIssuancePayment(CreateIssuancePaymentPermissionDataModel dataModel);

}