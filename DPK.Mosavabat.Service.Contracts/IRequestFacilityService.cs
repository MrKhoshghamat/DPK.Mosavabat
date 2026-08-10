using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine.AbortCases;

namespace DPK.Mosavabat.Services.Contracts;

public interface IRequestFacilityService
{
    // ثبت درخواست تسهیلات
    Task<MosavabatResponseDataModel?> RegisterRequestFacilityAsync(MosavabatRequestFacilityDataModel requestFacility);
    // دریافت لینک فایل قرارداد
    Task<string?> GetContractFileLink(GetMorabeheContractFileLinkDataModel getMorabeheContractFileLinkDataModel);
    // به روز رسانی فایل قرارداد
    Task<bool> UpdateContractFileLink(UpdateMorabeheContractFileLink updateMorabeheContractFileLink);
    // به روز رسانی درخواست تسهیلات
    Task<MosavabatResponseDataModel?> UpdateRequestFacility(MosavabatRequestFacilityUpdateDataModel updateRequestFacility);
    // به روز رسانی درخواست تسهیلات
    Task<MosavabatResponseDataModel?> SaveRequestFacility(MosavabatRequestFacilityUpdateDataModel updateRequestFacility);
    Task<MosavabatResponseDataModel?> AbortCasesAsync(AbortCasesDataModel dataModel);
}