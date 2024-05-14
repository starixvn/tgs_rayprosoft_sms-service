using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Tgs.SmsService;
using TGS.SmsService.Client;

namespace SmsService.IntegrationTests;

public class SmsSettingTests
{
    [Fact]
    public async Task 新增簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var name = "testName";
        var code = "code";
        var setting = JsonDocument.Parse("{}");
        var modifier = "test";

        // Act
        await sut.AddSmsSettingAsync(name, code, setting, modifier);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .AddSmsSettingAsync(
                Arg.Is(name),
                Arg.Is(code),
                Arg.Any<JsonDocument>(),
                Arg.Is(modifier),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task 修改簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var id = Guid.NewGuid();
        var name = "testName";
        var code = "code";
        var setting = JsonDocument.Parse("{}");
        var modifier = "test";

        // Act
        await sut.UpdateSmsSettingAsync(id, name, code, setting, modifier);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .UpdateSmsSettingAsync(
                Arg.Is(id),
                Arg.Is(name),
                Arg.Is(code),
                Arg.Any<JsonDocument>(),
                Arg.Is(modifier),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task 移除簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var id = Guid.NewGuid();

        // Act
        await sut.RemoveSmsSettingAsync(id);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RemoveSmsSettingAsync(
                Arg.Is(id),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task 查詢簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var id = Guid.NewGuid();

        // Act
        _ = await sut.GetSmsSettingAsync(id);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetSmsSettingByIdAsync(Arg.Is(id), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task 查詢全部簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        // Act
        _ = await sut.QuerySmsSettingListAsync().ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetSmsSettingsAsync(Arg.Any<CancellationToken>());
    }
}