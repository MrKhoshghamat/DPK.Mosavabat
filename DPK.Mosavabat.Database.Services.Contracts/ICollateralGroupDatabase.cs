namespace DPK.Mosavabat.Database.Services.Contracts;

public interface ICollateralGroupDatabase : IMainDatabase
{
    // استخراج شناسه گروه وثیقه
    Task<string?> GetId(int code);
}