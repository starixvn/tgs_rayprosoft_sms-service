using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace TGS.SmsService.Client;

internal class SmsServiceClient : ISmsServiceClient
{
    private static readonly JsonSerializerOptions _JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly HttpClient _httpClient;

    public SmsServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async ValueTask AddSmsSettingAsync(
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        var requestContent = JsonContent.Create(
            new
            {
                Name = name,
                Code = code,
                Setting = setting,
            },
            MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json),
            _JsonSerializerOptions);

        requestContent.Headers.Add("Modifier", modifier);

        var response = await _httpClient.PostAsync(
            "/api/sms-setting",
            requestContent,
            cancellationToken).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
    }

    public async ValueTask<SmsSettingInfo?> GetSmsSettingAsync(
        Guid SmsSettingId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"/api/sms-setting/{SmsSettingId}", cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        return string.IsNullOrEmpty(json)
            ? null
            : JsonSerializer.Deserialize<SmsSettingInfo>(json, _JsonSerializerOptions);
    }

    public IAsyncEnumerable<SmsSettingInfo> QuerySmsSettingListAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient
            .GetAsync("/api/sms-setting/list", cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<IEnumerable<SmsSettingInfo>>(_JsonSerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .Where(arr => arr is not null)
            .SelectMany(arr => arr!.ToAsyncEnumerable())
            .ToArrayAsync(cancellationToken)
            .AsTask()
            .ToAsyncEnumerable()
            .SelectMany(arr => arr.ToAsyncEnumerable());
    }

    public async ValueTask RemoveSmsSettingAsync(
        Guid SmsSettingId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(
            $"/api/sms-setting/{SmsSettingId}",
            cancellationToken).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
    }

    public async ValueTask SendShortMessageAsync(
        string destination,
        string message,
        Guid brandId,
        string? areaCode = null,
        string? phone = null,
        CancellationToken cancellationToken = default)
    {
        var postData = new
        {
            Destination = destination,
            Message = message,
            BrandId = brandId,
            AreaCode = areaCode,
            Phone = phone
        };

        var body = new StringContent(
            JsonSerializer.Serialize(postData, _JsonSerializerOptions),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var response = await _httpClient
            .PostAsync("/api/sms", body, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    public async ValueTask UpdateSmsSettingAsync(
        Guid SmsSettingId,
        string name,
        string code,
        JsonDocument setting,
        string modifier,
        CancellationToken cancellationToken = default)
    {
        var requestContent = JsonContent.Create(
            new
            {
                Name = name,
                Code = code,
                Setting = setting,
            },
            MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json),
            _JsonSerializerOptions);

        requestContent.Headers.Add("Modifier", modifier);

        var response = await _httpClient.PutAsync(
            $"/api/sms-setting/{SmsSettingId}",
            requestContent,
            cancellationToken).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
    }
}