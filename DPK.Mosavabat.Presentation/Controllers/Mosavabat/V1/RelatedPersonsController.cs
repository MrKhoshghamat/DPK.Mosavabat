using AutoMapper;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace DPK.Mosavabat.Presentation.Controllers.Mosavabat.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class RelatedPersonsController : ControllerBase
    {
        private readonly IRelatedPersonsDatabaseService _relatedPersonsService;
        private IValidator<IsRelatedPersonDataModel> _validator;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public RelatedPersonsController(
            IRelatedPersonsDatabaseService relatedPersonsService,
            IValidator<IsRelatedPersonDataModel> validator,
            ILoggerManager logger,
            IMapper mapper)
        {
            _relatedPersonsService = relatedPersonsService;
            _validator = validator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// تشخیص افراد مرتبط
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
        [SwaggerOperation(Tags = new[] { "Related Persons" })]
        public async Task<IActionResult> IsRelatedPerson(IsRelatedPersonDataModel dataModel)
        {
            var validationResult = await _validator.ValidateAsync(dataModel);
            if (!validationResult.IsValid)
            {
                var checkError = new ErrorInfo
                {
                    Message = "کد ملی معتبر نمی‌باشد"
                };
                return Ok(CreateResponse<string>(error: checkError));
            }
            var viewModel = _mapper.Map<IsRelatedPersonVm>(dataModel);
            var result = await _relatedPersonsService.IsRelatedPersonAsync(viewModel);
            return Ok(CreateResponse(result.IsRelated));
        }

        /// <summary>
        /// دریافت بدهی کل افراد مرتبط
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "in-mosavabat-v1")]
        [SwaggerOperation(Tags =new[] { "Related Persons" })]
        public async Task<IActionResult> GetTotalDebtOfRelatedPersons()
        {
            var result = await _relatedPersonsService.GetTotalDebtOfRelatedPersons();
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
