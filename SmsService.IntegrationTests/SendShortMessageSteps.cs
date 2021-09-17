using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using TechTalk.SpecFlow;
using Tgs.SmsService;
using TGS.SmsService.Client;

namespace TGS.SmsService.IntegrationTests
{
    [Binding]
    public class SendShortMessageSteps
    {
        private readonly ScenarioContext _context;
        private ISmsServiceClient _smsServiceClient;

        public SendShortMessageSteps(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeScenario]
        public void Setup()
        {
            var fakeSender = Substitute.For<ISmsSender>();
            _context.Add("smsSender", fakeSender);

            var fakeTgsRepository = Substitute.For<ITgsRepository>();

            fakeTgsRepository.GetBrandInfoByIdAsync(Arg.Any<Guid>())
                .Returns(new BrandInfo(
                    Guid.NewGuid(),
                    true,
                    "Test",
                    JsonDocument.Parse("{}")));

            var smsServiceHost = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((appCtx, configBuilder) =>
                        {
                            configBuilder.Sources.Clear();
                        })
                        .UseStartup<TGS.SmsService.Startup>()
                        .UseTestServer()
                        .ConfigureTestServices(services =>
                        {
                            services
                                .AddFakeRabbitMessageQueue()
                                .AddTransient(sp => fakeSender)
                                .AddTransient(sp => fakeTgsRepository);
                        });
                })
                .Build();

            _ = smsServiceHost.StartAsync();

            _smsServiceClient = new SmsServiceClient(smsServiceHost.GetTestClient());
        }

        [Given(@"my brandId is ""(.*)""")]
        public void GivenMyBrandId(Guid brandId)
        {
            _context.Add("brandId", brandId);
        }

        [Given(@"my phone number is ""(.*)""")]
        public void GivenMyPhoneNumberIs(string phone)
        {
            _context.Add("phone", phone);
        }

        [When(@"send short message")]
        public async Task WhenSendShortMessageAsync()
        {
            var brandId = _context.Get<Guid>("brandId");
            var phone = _context.Get<string>("phone");

            await _smsServiceClient.SendShortMessageAsync(phone, "test message", brandId);
        }

        [Then(@"the request will queued")]
        public void ThenTheRequestWillQueued()
        {
            var brandId = _context.Get<Guid>("brandId");
            var phone = _context.Get<string>("phone");
            var fakeSender = _context.Get<ISmsSender>("smsSender");

            _ = fakeSender
                .Received(1)
                .SendSmsAsync(
                    Arg.Is("test"),
                    Arg.Is(phone),
                    Arg.Any<string>());
        }
    }
}
