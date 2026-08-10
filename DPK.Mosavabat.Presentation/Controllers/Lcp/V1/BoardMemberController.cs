using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Presentation.Controllers.Lcp.V1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "in-lcp-v1")]
    [Route("api/lcp/[controller]/[action]")]
    [ApiController]
    public class BoardMemberController : ControllerBase
    {
        private readonly IBoardMemberService _boardMemberService;
        private readonly ILoggerManager _logger;
        public BoardMemberController(
            IBoardMemberService boardMemberService, 
            ILoggerManager logger)
        {
            _boardMemberService = boardMemberService;
            _logger = logger;
        }

        /// <summary>
        /// دریافت لیست هیات مدیره
        /// </summary>
        /// <param name="getBoardMemberDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetBoardMembersList(GetBoardMemberDataModel getBoardMemberDataModel,CancellationToken cancellationToken)
        {
            var result = await _boardMemberService.GetBoardMembersList(getBoardMemberDataModel,cancellationToken);
            return Ok(CreateResponse(result));
        }

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
