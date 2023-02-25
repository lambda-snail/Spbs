using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Integrations.Nordigen.Extensions;

public static class WebExtensions
{
    public static Task<HttpResponseMessage> SendPostRequest<TRequestBody>(this HttpClient client,
        TRequestBody? requestBody, string endpoint, string? authToken)
        where TRequestBody : class
    {
        return client.SendRequestWithBody(requestBody, endpoint, HttpMethod.Post, authToken);
    }

    public static Task<HttpResponseMessage> SendGetRequest(this HttpClient client, string endpoint, string? authToken)
    {
        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        ConfigureDefaultHeaders(authToken, request);

        return client.SendAsync(request);
    }

    public static async Task<TResponse?> ParseResponseAsync<TResponse>(this HttpResponseMessage response)
        where TResponse : class
    {
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        return null;
    }

    private static Task<HttpResponseMessage> SendRequestWithBody<TRequest>(this HttpClient client,
        TRequest? requestBody, string endpoint, HttpMethod method, string? authToken)
        where TRequest : class
    {
        HttpRequestMessage request = new(method, endpoint);
        ConfigureDefaultHeaders(authToken, request);

        if (requestBody is not null)
        {
            request.Content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json");
        }

        return client.SendAsync(request);
    }

    private static void ConfigureDefaultHeaders(string? authToken, HttpRequestMessage request)
    {
        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        if (authToken is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
    }
}