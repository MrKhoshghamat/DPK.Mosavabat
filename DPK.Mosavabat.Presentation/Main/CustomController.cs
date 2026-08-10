using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Shared.DataModels.General;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DPK.Mosavabat.Presentation.Main;

[ApiVersion("1.0")]
[Route("api/mosavabat/[controller]/[action]")]
[ApiController]
// Todo.H.M : This Controller Is Under Development (Must Be Tested)
public class CustomController : ControllerBase
{
    private readonly ILoggerManager _logger;

    public CustomController(ILoggerManager logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Method to create a response object
    /// </summary>
    protected AjaxResponseBase<T> CreateResponse<T>(T? result = default, ErrorInfo? error = null)
    {
        // Creating a new response object
        var response = new AjaxResponseBase<T>
        {
            Result = result, // Assigning result to response
            Success = error == null, // Setting success based on error presence
            Error = error // Assigning error to response
        };
        // Logging the response
        _logger.Trace(JsonConvert.SerializeObject(response));
        // Returning the response
        return response;
    }

    /// <summary>
    /// Method to process requests
    /// </summary>
    protected async Task<IActionResult> ProcessRequest<T>(T viewModel, Func<Task<object?>> serviceCall, string methodName)
    {
        // Logging the service type being run
        _logger.Trace(JsonConvert.SerializeObject($"Run {methodName} Service From MA"));
        // Logging the view model
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        // Starting the stopwatch for performance measurement
        var stopwatch = Stopwatch.StartNew();
        // Checking if the model state is valid
        if (!ModelState.IsValid)
        {
            // Stopping the stopwatch
            stopwatch.Stop();
            // Logging the service runtime
            _logger.Trace(JsonConvert.SerializeObject($"Service Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
            // Returning unprocessable entity if model state is invalid
            return UnprocessableEntity(ModelState);
        }
        // Calling the service and awaiting the response
        var response = await serviceCall();
        // Stopping the stopwatch
        stopwatch.Stop();
        // Logging the service runtime
        _logger.Trace(JsonConvert.SerializeObject($"Service {methodName} Runtime: {stopwatch.ElapsedMilliseconds / 1000.0} Seconds"));
        // Returning the response wrapped in an OK result
        return Ok(CreateResponse(response));
    }
}
