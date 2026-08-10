using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services;

public class SamatService : ISamatService
{
    private readonly ISamatServices _samatServices;
    private readonly ILoggerManager _logger;

    public SamatService(ILoggerManager logger, ISamatServices samatServices)
    {
        _logger = logger;
        _samatServices = samatServices;
    }

    /// <summary>
    /// استعلام چک برگشتی
    /// </summary>
    /// <param name="dataModel"></param>
    /// <returns></returns>
    public async Task<string> EstelamCheck(EstelamCheckDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel.NationalId));
        var checkCount = await _samatServices.EstelamCheck(dataModel);
        return checkCount;
    }
}