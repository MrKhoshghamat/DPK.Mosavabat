using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IRequestFacilityDatabase : IMainDatabase
{
    // به روز رسانی درخواست تسهیلات اولیه
    Task<string?> UpdateLcpRequestFacility(int idCase, int initialRadNumber, int? idCaseApprovalRequest, int? radNumberApprovalRequest, int? lcpRequestFacilityId, string? guidLcpRequestFacility);
    // نگاشت اطلاعات درخواست اولیه با درخواست اصلی
    Task<string?> MapLcpRequestFacilityToApprovalRequest(int idCase, int idCaseApprovalRequest, int idLcpRequestFacility);
    Task<string?> GetRequestFacilityCaseId(int idCaseApprovalRequest);
    // دریافت لینک فایل قرارداد
    Task<string?> GetContractFileLink(GetMorabeheContractFileLinkDataModel getMorabeheContractFileLinkDataModel);
    // به روز رسانی فایل قرارداد
    Task<string?> UpdateContractFileLink(UpdateMorabeheContractFileLink updateMorabeheContractFileLink);
    // یررسی وجود شماره حساب مشتری در DevProvider
    Task<CustomerAccountNoDataModel> FindCustomerAccountNoDataModel(CustomerNoDataModel lcpRequestFacilityDataModel);
}