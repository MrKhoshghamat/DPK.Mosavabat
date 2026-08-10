namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IFacilityGroupDatabase : IMainDatabase
{
    // استخراج شناسه نوع عقد
    Task<string?> ExtractFacilityGroupId(string code);
}