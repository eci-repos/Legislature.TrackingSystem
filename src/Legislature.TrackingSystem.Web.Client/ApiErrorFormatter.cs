namespace Legislature.TrackingSystem.Web.Client;

/// <summary>
/// Formats API call failures for display. Network-level failures (e.g., the server being
/// temporarily unreachable during a restart) are shown with a friendly, actionable message
/// instead of the raw exception text.
/// </summary>
public static class ApiErrorFormatter
{
    public static string Format(Exception exception)
    {
        if (exception is HttpRequestException httpException)
        {
            // A null StatusCode means the request never reached the server (network failure).
            // A non-null StatusCode means the server was reached but returned an error.
            return httpException.StatusCode is null
                ? "Couldn't connect to the server. Refresh the page to retry."
                : $"The server returned an error ({(int)httpException.StatusCode}). Refresh the page to retry.";
        }

        return exception.Message;
    }
}
