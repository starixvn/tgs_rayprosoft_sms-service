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
}
