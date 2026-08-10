using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;

namespace DPK.Mosavabat.External.Services.Contracts.Lms
{
    public interface ILmsServiceProxy
    {
        Task<BaseResponse<PaymentPermissionIssuanceControlDto>> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlDataModel dataModel);
        Task<BaseResponse<LmsResponse>> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionDataModel dataModel);
        Task<BaseResponse<LmsResponse>> InsertPaymentPermission(InsertPaymentPermissionDataModel dataModel);
        Task<BaseResponse<LmsResponse>> UpdatePaymentPermission(UpdatePaymentPermissionDataModel dataModel);
        Task<BaseResponse<List<GetPaymentPermissionDto>>> FetchPaymentPermissions(GetPaymentPermissionListDataModel dataModel);
        Task<GetIsCanceledPaymentPermissionsDto> FetchIsCanceledPaymentPermissions();
        Task<BaseResponse<BoundaryComplianceInquiryDto>> CheckBoundaryComplianceInquiry(BoundaryComplianceInquiryDataModel dataModel);
        Task<BaseResponse<LmsResponse>> RegisterPaymentPermissionExceptionReason(RegisterPaymentPermissionExceptionReasonsDataModel dataModel);
        Task<BaseResponse<List<GetPaymentPermissionExceptionReasonsDto>>> GetPaymentPermissionExceptionReasons(GetPaymentPermissionExceptionReasonsDataModel dataModel);
    }
}
