namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IExchangeCommercialDocDatabase : IMainDatabase
{
    // استخراج کد شعبه
    Task<string?> ExtractExchangeCommercialDocId(string documentCode);
}