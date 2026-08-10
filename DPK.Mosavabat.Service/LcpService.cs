using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services
{
    public class LcpService : ILcpService
    {
        private readonly ILcpServiceProxy _lcpServiceProxy;
        private readonly ILoggerManager _logger;

        public LcpService(ILcpServiceProxy lcpServiceProxy, ILoggerManager logger)
        {
            _lcpServiceProxy = lcpServiceProxy;
            _logger = logger;
        }

        public Task<string> AcceptAgent(AcceptAgentDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject("AcceptAgent request."));
            return _lcpServiceProxy.AcceptAgent(dataModel);
        }
    }
}
