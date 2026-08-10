namespace DPK.Mosavabat.Database.Services.Contracts;

public interface ISubContractTypeDatabase : IMainDatabase
{
    // استخراج شناسه نوع قرارداد
    Task<string?> ExtractSubContractTypeId(string contractTemplateCode);
}