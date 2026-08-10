using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IUserDatabase : IMainDatabase
{
    // پیدا کردن مسوول تسهیلات
    Task<string?> FindFacilityManagerOrSupervisor(int idLcpRequestFacility);
    Task<string?> GetUserName(int branchCode, MosavabatClassName mosavabatClassName);
    Task<int> GetUserId(string userName);
}