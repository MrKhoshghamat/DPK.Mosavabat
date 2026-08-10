namespace DPK.Mosavabat.Database.Services.Contracts;

public interface ICollateralTypeDatabase : IMainDatabase
{
    // استخراج شناسه نوع وثیقه
    Task<string?> GetId(int? code = 0);
}