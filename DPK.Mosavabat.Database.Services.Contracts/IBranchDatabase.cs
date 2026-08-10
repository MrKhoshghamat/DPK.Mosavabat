namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IBranchDatabase : IMainDatabase
{
    // استخراج کد شعبه
    Task<string?> ExtractBranchCode(int idLcpRequestFacility);
}