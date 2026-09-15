using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.ViewModels.Lcp;
using Newtonsoft.Json;

namespace DPK.Mosavabat.External.Services.Lcp
{
    public class IssuancePaymentExternalServices : IIssuancePaymentExternalServices
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper; 
        private readonly IExternalServiceProvider _externalServiceProvider;

        public IssuancePaymentExternalServices(
            ILoggerManager logger, 
            IExternalServiceProvider externalServiceProvider,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _externalServiceProvider = externalServiceProvider;
        }


        public async Task<ProcessIssuancePaymentDto> ProcessIssuancePaymentRequest(ProcessIssuancePaymentDataModel dataModel, CancellationToken cancellationToken = default)
        {
            var viewModel = _mapper.Map<DiscussionPaymentPermissionVm>(dataModel);
            var result = await _externalServiceProvider.DiscussionPaymentPermission(viewModel,cancellationToken);
            _logger.Trace(JsonConvert.SerializeObject(result));
            return result;
        }
    }
}
