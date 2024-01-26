using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NSubstitute;
using Tgs.SmsService;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using TGS.SmsService.Client;

namespace SmsService.IntegrationTests;
public class SendMessageTests
{
    [Fact]
    public async Task 整合測試_送出短信()
    {
        // Arrange
        var fakeSender = Substitute.For<ISmsSender>();
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        _ = fakeTgsRepository.GetBrandInfoByIdAsync(Arg.Any<Guid>())
            .Returns(new BrandInfo(
                Guid.NewGuid(),
                true,
                "Test",
                JsonDocument.Parse("{}")));

        var smsServiceHost = SmsServiceTestHost.Factory
            .WithWebHostBuilder(webBuilder => webBuilder
                .ConfigureServices(services => services
                    .AddSingleton(fakeSender)
                    .AddSingleton(fakeTgsRepository)));

        var httpClient = smsServiceHost.CreateClient();
        var sut = new SmsServiceClient(smsServiceHost.CreateClient());

        var brandId = Guid.NewGuid();
        var areaCode = "012";
        var phone = "3456789";

        // Act
        await sut.SendShortMessageAsync($"{areaCode}{phone}", "test", brandId, areaCode, phone);

        // Assert
        _ = fakeSender.Received(1)
            .SendSmsAsync(
                Arg.Is("test"),
                Arg.Any<ShortMessageData>(),
                Arg.Any<CancellationToken>());
    }
}
