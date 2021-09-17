using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;

namespace SmsService.Core.UnitTests
{
    [TestFixture]
    public class SmsTests
    {
        [Test]
        public async Task Send_Sms()
        {
            var fakeSmsSender = Substitute.For<ISmsSender>();
            var logger = NullLogger<SendSmsCommandService>.Instance;

            var sut = new SendSmsCommandService(
                fakeSmsSender,
                logger);

            var command = new SendSmsCommand(
                Guid.NewGuid(),
                "886988123456",
                "test",
                "Test",
                JsonDocument.Parse("{}"));

            await sut.ExecuteAsync(command);

            _ = fakeSmsSender.Received(1)
                .SendSmsAsync(
                    Arg.Is(command.BrandId),
                    Arg.Is(command.VendorCode.ToLower()),
                    Arg.Is(command.Destination),
                    Arg.Is(command.Message));
        }
    }
}
