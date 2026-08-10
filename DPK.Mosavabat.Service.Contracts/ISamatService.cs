using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Services.Contracts;

public interface ISamatService
{
    Task<string> EstelamCheck(EstelamCheckDataModel dataModel);
}