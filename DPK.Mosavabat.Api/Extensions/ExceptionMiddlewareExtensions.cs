using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Entities.Exceptions;
using DPK.Mosavabat.Shared.DataModels.General;
using DPK.Mosavabat.Shared.Exceptions.ExternalServices;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace DPK.Mosavabat.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature == null)
                    return;

                var exception = contextFeature.Error;

                context.Response.StatusCode = exception switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    SamatExternalServiceUnavailableException => StatusCodes.Status200OK, // اگر طبق منطق فعلی پروژه می‌خواهی 200 باشد
                    _ => StatusCodes.Status500InternalServerError
                };

                AjaxResponseBase<string?> response;

                if (exception is SamatExternalServiceUnavailableException)
                {
                    response = new AjaxResponseBase<string?>
                    {
                        Result = null,
                        Success = false,
                        Error = new ErrorInfo
                        {
                            Code = 5001,
                            Message = "سرویس سمات در حال حاضر در دسترس نیست"
                        }
                    };
                }
                else if (exception.Message == "Server was unable to process request. ---> Object reference not set to an instance of an object.")
                {
                    response = new AjaxResponseBase<string?>
                    {
                        Result = null,
                        Success = false,
                        Error = new ErrorInfo
                        {
                            Code = context.Response.StatusCode,
                            Message = "ارتباط با سرور در حال حاضر امکان پذیر نمی باشد. لطفا دوباره تلاش کنید"
                        }
                    };
                }
                else
                {
                    response = new AjaxResponseBase<string?>
                    {
                        Result = null,
                        Success = false,
                        Error = new ErrorInfo
                        {
                            Code = context.Response.StatusCode,
                            Message = exception.Message
                        }
                    };
                }

                logger.Error($"ExceptionMiddleware | StatusCode: {context.Response.StatusCode} | Message: {exception.Message}");

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });
    }
}
