namespace DPK.Mosavabat.Contracts;

public interface IRepositoryManager
{
    Task SaveAsync();
    void Save();
}