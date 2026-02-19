using Refit;

namespace DPK.Mosavabat.ExternalServices.Extensions;

public static class RefitResponseExtensions
{
    public static T GetContentOrThrow<T>(
        this ApiResponse<T> response,
        string serviceName = "External Service")
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.Content is null)
            {
                throw new InvalidOperationException(
                    $"[{serviceName}] Request succeeded (Status: {response.StatusCode}) but returned NULL content.");
            }

            return response.Content;
        }

        var errorDetails = string.Empty;

        if (response.Error != null)
        {
            errorDetails = response.Error.Content ?? response.Error.Message;
        }

        var errorMessage = $"[{serviceName}] Request Failed. Status: {response.StatusCode} ({((int)response.StatusCode)}). Details: {errorDetails}";

        if (response.Error != null)
        {
            throw response.Error;
        }

        throw new HttpRequestException(errorMessage, null, response.StatusCode);
    }
}