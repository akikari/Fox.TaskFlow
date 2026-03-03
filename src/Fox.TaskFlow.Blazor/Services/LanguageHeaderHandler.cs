//==================================================================================================
// HTTP message handler for adding Accept-Language header based on current language state.
// Injects language preference into every HTTP request.
//==================================================================================================

namespace Fox.TaskFlow.Blazor.Services;

//==================================================================================================
/// <summary>
/// Delegating handler that adds Accept-Language header to all HTTP requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LanguageHeaderHandler"/> class.
/// </remarks>
/// <param name="languageState">The language state service.</param>
//==================================================================================================
public sealed class LanguageHeaderHandler(LanguageState languageState) : DelegatingHandler
{
    //==============================================================================================
    /// <summary>
    /// Sends an HTTP request with Accept-Language header.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP response message.</returns>
    //==============================================================================================
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(languageState);

        request.Headers.AcceptLanguage.Clear();
        request.Headers.AcceptLanguage.ParseAdd(languageState.CurrentLanguage);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
