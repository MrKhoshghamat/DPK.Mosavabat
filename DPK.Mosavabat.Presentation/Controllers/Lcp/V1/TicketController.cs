using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Lcp.V1;

//[Authorize]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "in-lcp-v1")]
[Route("api/lcp/[controller]/[action]")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly ITicketServices _ticketServices;
    private readonly IRequestFacilityService _requestFacilityService;
    private readonly ILoggerManager _logger;
    public TicketController(ITicketServices ticketServices, ILoggerManager logger, IRequestFacilityService requestFacilityService)
    {
        _ticketServices = ticketServices;
        _requestFacilityService = requestFacilityService;
        _logger = logger;
    }

    /// <summary>
    /// ثبت تیکت ویرایش در سامانه پرتال مشتریان حقوقی
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Roles = "MosavabatAdmin")]
    [SwaggerOperation(Tags = new[] { "Tickets" })]
    public async Task<object?> Register(TicketDataModel dataModel)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var creteTicketResult = await _ticketServices.CreateTicket(dataModel);
        var insertNewReqFacilityStatusDataModel = new InsertReqFacilityStatusDataModel
        {
            IdRequestFacility = dataModel.RequestFacilityId
        };
        await _requestFacilityService.InsertReqFacilityStatus(insertNewReqFacilityStatusDataModel);
        return creteTicketResult;
    }
}