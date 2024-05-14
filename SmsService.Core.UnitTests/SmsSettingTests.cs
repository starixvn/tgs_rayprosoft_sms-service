using System.Text.Json;
using NSubstitute;
using NUnit.Framework;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;

namespace SmsService.Core.UnitTests;

[TestFixture]
public class SmsSettingTests
{
    [Test]
    public async Task AddSmsSettingCommandService_新增簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AddSmsSettingCommandService(fakeTgsRepository);

        var name = "testName";
        var code = "vendorCode";
        var setting = JsonDocument.Parse("{}");
        var modifier = "test";

        var command = new AddSmsSettingCommand(name, code, setting, modifier);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .AddSmsSettingAsync(
                Arg.Is(command.Name),
                Arg.Is(command.Code),
                Arg.Is(command.Setting),
                Arg.Is(command.Modifier));
    }

    [Test]
    public async Task UpdateSmsSettingCommandService_修改簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new UpdateSmsSettingCommandService(fakeTgsRepository);

        var id = Guid.NewGuid();
        var name = "testName";
        var code = "vendorCode";
        var setting = JsonDocument.Parse("{}");
        var modifier = "test";

        var command = new UpdateSmsSettingCommand(id, name, code, setting, modifier);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .UpdateSmsSettingAsync(
                Arg.Is(id),
                Arg.Is(name),
                Arg.Is(code),
                Arg.Is(setting),
                Arg.Is(modifier));
    }

    [Test]
    public async Task RemoveSmsSettingCommandService_移除簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new RemoveSmsSettingCommandService(fakeTgsRepository);

        var id = Guid.NewGuid();
        var command = new RemoveSmsSettingCommand(id);
        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RemoveSmsSettingAsync
                (Arg.Is(id));
    }

    [Test]
    public async Task GetSmsSettingsQueryService_查詢全部簡訊設定()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new GetSmsSettingsQueryService(fakeTgsRepository);

        var command = new GetSmsSettingsQuery();
        // Act
        await sut.QueryAsync(command).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetSmsSettingsAsync();
    }
}