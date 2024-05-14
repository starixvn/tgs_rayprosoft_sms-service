using Microsoft.Extensions.DependencyInjection;
using Tgs.SmsService;
using TGS.SmsService.Client;

namespace SmsService.IntegrationTests;

public class BrandSmsTests
{
    [Fact]
    public async Task 新增平台簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var brandId = Guid.NewGuid();
        var smsSettingId = Guid.NewGuid();

        // Act
        await sut.AddBrandSmsAsync(brandId, smsSettingId);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .AddBrandSmsAsync(
                Arg.Is(brandId),
                Arg.Is(smsSettingId),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task 移除平台簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var brandId = Guid.NewGuid();
        var smsSettingId = Guid.NewGuid();

        // Act
        await sut.RemoveBrandSmsAsyncAsync(brandId, smsSettingId);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RemoveBrandSmsAsync(
                Arg.Is(brandId),
                Arg.Is(smsSettingId),
                Arg.Any<CancellationToken>());
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
        var brandId = Guid.NewGuid();

        // Act
        _ = await sut.QueryBrandSmsListAsync(brandId).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .QueryBrandSmsListAsync(
                Arg.Is(brandId),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task 套用平台簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var brandId = Guid.NewGuid();
        var smsSettingId = Guid.NewGuid();
        var modifier = "test";

        // Act
        await sut.ApplyBrandSmsAsync(brandId, smsSettingId, modifier);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .ApplyBrandSmsAsync(
                Arg.Is(brandId),
                Arg.Is(smsSettingId),
                Arg.Is(modifier),
                Arg.Any<CancellationToken>());
    }
}