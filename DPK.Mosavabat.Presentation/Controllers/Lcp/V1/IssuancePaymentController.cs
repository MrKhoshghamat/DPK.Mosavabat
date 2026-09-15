using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Lcp;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Lcp.V1
{
    [ApiVersion("1.0")]
    [Route("api/lcp/[controller]/[action]")]
    [ApiController]

    public class IssuancePaymentController : ControllerBase
    {
        private readonly IssuancePaymentExternalServices _issuancePaymentExternalServices;
        private readonly ILoggerManager _logger;

        public IssuancePaymentController(
            IssuancePaymentExternalServices issuancePaymentExternalServices,
            ILoggerManager logger)
        {
            _issuancePaymentExternalServices = issuancePaymentExternalServices;
            _logger = logger;   
        }
        /// <summary>
        /// پردازش درخواست صدور شناسه پرداخت
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "in-lcp-v1")]
        [SwaggerOperation(Tags = new[] { "Issuance Payment" })]

        public async Task<IActionResult> ProcessIssuancePaymentRequest(ProcessIssuancePaymentDataModel dataModel)
        {
            _logger.Trace(JsonConvert.SerializeObject(dataModel));
            var result =  await _issuancePaymentExternalServices.ProcessIssuancePaymentRequest(dataModel);
            return Ok(CreateResponse(result));
        }

        // ساخت پاسخ
        private AjaxResponseBase<T> CreateResponse<T>(T? result = default, ErrorInfo? error = null)
        {
            var response = new AjaxResponseBase<T>
            {
                Result = result,
                Success = error == null,
                Error = error
            };

            _logger.Trace(JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
