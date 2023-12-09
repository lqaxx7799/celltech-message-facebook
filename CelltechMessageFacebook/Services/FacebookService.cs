using System.Net.Http.Headers;
using System.Web;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Objects.FacebookObjects;
using Newtonsoft.Json;

namespace CelltechMessageFacebook.Services;

public interface IFacebookService
{
    Task<FacebookAccountResponse> GetMe(string accessToken);
    Task<FacebookPagedResponse<FacebookPageResponse>> GetPages(string accessToken);
    Task SubscribeApp(string pageId, string accessToken);
    Task<FacebookOAuthResponse> GenerateLongLivedToken(string accessToken);
    Task<T> GetNode<T>(string nodeId, string accessToken);
    Task<T> GetNode<T>(string nodeId, string accessToken, string? fields);
}

public class FacebookService(IHttpClientFactory httpClientFactory, AppSettings appSettings, ILogger<FacebookService> logger) : IFacebookService
{
    public async Task<FacebookAccountResponse> GetMe(string accessToken)
    {
        var httpClient = httpClientFactory.CreateClient();
        var builder = new UriBuilder($"{appSettings.Facebook.BaseApiUrl}/me");
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["fields"] = "id,name";
        builder.Query = query.ToString();
        var url = builder.ToString();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.SendAsync(requestMessage);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = JsonConvert.DeserializeObject<FacebookErrorModel>(content)!.Error.Message;
            throw new BadHttpRequestException(errorMessage ?? string.Empty);
        }

        return JsonConvert.DeserializeObject<FacebookAccountResponse>(content)!;
    }
    
    public async Task<FacebookPagedResponse<FacebookPageResponse>> GetPages(string accessToken)
    {
        var httpClient = httpClientFactory.CreateClient();
        var builder = new UriBuilder($"{appSettings.Facebook.BaseApiUrl}/me/accounts");
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["limit"] = "1000";
        builder.Query = query.ToString();
        var pagesUrl = builder.ToString();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, pagesUrl);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.SendAsync(requestMessage);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = JsonConvert.DeserializeObject<FacebookErrorModel>(content)!.Error.Message;
            throw new BadHttpRequestException(errorMessage ?? string.Empty);
        }

        return JsonConvert.DeserializeObject<FacebookPagedResponse<FacebookPageResponse>>(content)!;
    }

    public async Task SubscribeApp(string pageId, string accessToken)
    {
        var httpClient = httpClientFactory.CreateClient();
        var builder = new UriBuilder($"{appSettings.Facebook.BaseApiUrl}/{pageId}/subscribed_apps");
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["subscribed_fields"] = "messages";
        builder.Query = query.ToString();
        var url = builder.ToString();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<FacebookErrorModel>(content)!.Error.Message;
            throw new BadHttpRequestException(errorMessage ?? string.Empty);
        }
    }
    
    public async Task<FacebookOAuthResponse> GenerateLongLivedToken(string accessToken)
    {
        var httpClient = httpClientFactory.CreateClient();
        var builder = new UriBuilder($"{appSettings.Facebook.BaseApiUrl}/oauth/access_token");
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["grant_type"] = "fb_exchange_token";
        query["client_id"] = appSettings.Facebook.AppId;
        query["client_secret"] = appSettings.Facebook.AppSecret;
        query["fb_exchange_token"] = accessToken;
        builder.Query = query.ToString();
        var url = builder.ToString();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(requestMessage);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = JsonConvert.DeserializeObject<FacebookErrorModel>(content)!.Error.Message;
            throw new BadHttpRequestException(errorMessage ?? string.Empty);
        }

        return JsonConvert.DeserializeObject<FacebookOAuthResponse>(content)!;
    }
    
    public async Task<T> GetNode<T>(string nodeId, string accessToken)
    {
        return await GetNode<T>(nodeId, accessToken, null);
    }

    public async Task<T> GetNode<T>(string nodeId, string accessToken, string? fields)
    {
        var httpClient = httpClientFactory.CreateClient();
        var builder = new UriBuilder( $"{appSettings.Facebook.BaseApiUrl}/{nodeId}");
        var query = HttpUtility.ParseQueryString(builder.Query);
        if (!string.IsNullOrEmpty(fields))
        {
            query["fields"] = fields;
        }
        builder.Query = query.ToString();
        var url = builder.ToString();
        var leadRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        leadRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var leadResponse = await httpClient.SendAsync(leadRequestMessage);
        var leadContent = await leadResponse.Content.ReadAsStringAsync();
        if (!leadResponse.IsSuccessStatusCode)
        {
            var errorMessage = JsonConvert.DeserializeObject<FacebookErrorModel>(leadContent)!.Error.Message;
            throw new BadHttpRequestException(errorMessage ?? string.Empty);
        }

        return JsonConvert.DeserializeObject<T>(leadContent)!;
    }
}