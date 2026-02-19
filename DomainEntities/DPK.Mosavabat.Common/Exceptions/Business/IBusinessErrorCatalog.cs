namespace DPK.Mosavabat.Common.Exceptions.Business;

public interface IBusinessErrorCatalog
{
    bool TryGet(int code, out BusinessError error);
}