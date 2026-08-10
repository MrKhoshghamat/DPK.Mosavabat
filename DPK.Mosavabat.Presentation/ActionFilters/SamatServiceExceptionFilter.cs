using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.Exceptions.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DPK.Mosavabat.Presentation.ActionFilters;

public class SamatServiceExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;

        if (ex is not SamatExternalServiceUnavailableException &&
            ex.InnerException is not SamatExternalServiceUnavailableException) return;
        var error = new ErrorInfo
        {
            Code = 5001,
            Message = "سرویس سمات در حال حاضر در دسترس نیست."
        };

        context.Result = new JsonResult(new
        {
            success = false,
            error
        })
        {
            StatusCode = 200
        };

        context.ExceptionHandled = true;
    }

}