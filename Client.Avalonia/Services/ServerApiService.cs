using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Avalonia.Services;

/// <summary>服务端管理 API 实现 — 通过 HTTP 调用 JT808.Web 管理接口</summary>
public class ServerApiService : IServerApiService
{
    private readonly HttpClient _http;

    public String BaseUrl { get; set; } = "http://127.0.0.1:5000";
    public String? Token { get; set; }

    public ServerApiService()
    {
        _http = new HttpClient();
    }

    private void ApplyAuth(HttpRequestMessage request)
    {
        if (!String.IsNullOrEmpty(Token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        }
    }

    private async Task<T?> GetAsync<T>(String path) where T : class
    {
        var url = $"{BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        ApplyAuth(request);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    private async Task<Boolean> PostAsync(String path, Object? body = null)
    {
        var url = $"{BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        ApplyAuth(request);

        if (body != null)
            request.Content = JsonContent.Create(body);

        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    private async Task<Boolean> DeleteAsync(String path)
    {
        var url = $"{BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        ApplyAuth(request);

        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<ServerStats?> GetStatsAsync() => await GetAsync<ServerStats>("api/jt808/stats");
    public async Task<SessionList?> GetSessionsAsync() => await GetAsync<SessionList>("api/jt808/sessions");
    public async Task<SessionInfo?> GetSessionAsync(String mobile) => await GetAsync<SessionInfo>($"api/jt808/session/{mobile}");
    public async Task<Boolean> RemoveSessionAsync(String mobile) => await DeleteAsync($"api/jt808/session/{mobile}");
    public async Task<BlacklistInfo?> GetBlacklistAsync() => await GetAsync<BlacklistInfo>("api/jt808/blacklist");
    public async Task<Boolean> AddBlacklistAsync(String mobile) => await PostAsync("api/jt808/blacklist", new { mobile });
    public async Task<Boolean> RemoveBlacklistAsync(String mobile) => await DeleteAsync($"api/jt808/blacklist/{mobile}");

    public async Task<Boolean> SendCommandAsync(String mobile, String bodyType, Dictionary<String, Object> body)
    {
        return await PostAsync("api/jt808/command", new
        {
            mobile,
            bodyType,
            body
        });
    }
}
