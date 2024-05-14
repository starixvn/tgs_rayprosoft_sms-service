using System.Text.Json;
using NSubstitute;
using NUnit.Framework;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;

namespace SmsService.Core.UnitTests;

[TestFixture]
public class BrandSmsTests
{
    [Test]
    public async Task AddBrandSmsCommandService_新增平台簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AddBrandSmsCommandService(fakeTgsRepository);

        var brandId = Guid.NewGuid();
        var smsSettingId = Guid.NewGuid();

        var command = new AddBrandSmsCommand(brandId, smsSettingId);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .AddBrandSmsAsync(
                Arg.Is(command.BrandId),
                Arg.Is(command.SmsSettingId));
    }

    [Test]
    public async Task RemoveBrandSmsCommandService_移除平台簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new RemoveBrandSmsCommandService(fakeTgsRepository);

        var brandId = Guid.NewGuid();
        var smsSettingId = Guid.NewGuid();
        var command = new RemoveBrandSmsCommand(brandId, smsSettingId);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RemoveBrandSmsAsync(
                Arg.Is(brandId),
                Arg.Is(smsSettingId));
    }

    [Test]
    public async Task GetBrandSmsQueryService_查詢全部簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new GetBrandSmsQueryService(fakeTgsRepository);

        var brandId = Guid.NewGuid();
        var command = new GetBrandSmsQuery(brandId);
        // Act
        await sut.QueryAsync(command).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .QueryBrandSmsListAsync(Arg.Is(brandId));
    }

    [Test]
    public async Task ApplyBrandSmsCommandService_套用平台簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new ApplyBrandSmsCommandService(fakeTgsRepository);

        var brandId = Guid.NewGuid();
        var smsSettingId = Guid.NewGuid();
        var modifier = "test";

        var smsSetting = new SmsSettingInfo
        {
            SmsSettingId = smsSettingId,
            Code = "code",
            Setting = JsonDocument.Parse("{}"),
            Name = "name",
        };

        _ = fakeTgsRepository
            .GetSmsSettingByIdAsync(Arg.Is(smsSettingId))
            .Returns(smsSetting);

        var command = new ApplyBrandSmsCommand(brandId, smsSettingId, modifier);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .ApplyBrandSmsAsync(
                Arg.Is(brandId),
                Arg.Is(smsSetting.Code),
                Arg.Is(smsSetting.Setting),
                Arg.Is(modifier));
    }
}