using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;

namespace DPK.Mosavabat.External.Services.Contracts.Payam;

public interface ISamatServices
{
    Task<string> EstelamCheck(EstelamCheckDataModel dataModel);
}