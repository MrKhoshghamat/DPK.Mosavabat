using DPK.Mosavabat.Common.Consts;

namespace DPK.Mosavabat.GeneralServices;

public abstract class MosavabatDomainService : DomainService
{
    protected MosavabatDomainService()
    {


        this.LocalizationSourceName = MosavabatConsts.LocalizationSourceName;
    }
}