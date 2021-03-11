using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using TechTalk.SpecFlow;
using TGS.Sms;
using TGS.SmsService.Clients;

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

            var smsServiceHost = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<TGS.SmsService.Startup>()
                        .UseTestServer()
                        .ConfigureTestServices(services =>
                        {
                            services
                                .AddTransient(sp => fakeSender);
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

        [Given(@"my currency is ""(.*)""")]
        public void GivenMyCurrency(string currency)
        {
            _context.Add("currency", currency);
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
            var currency = _context.Get<string>("currency");
            var phone = _context.Get<string>("phone");

            await _smsServiceClient.SendShortMessageAsync(phone, "test message", currency, brandId);
        }

        [Then(@"the request will queued")]
        public void ThenTheRequestWillQueued()
        {
            var brandId = _context.Get<Guid>("brandId");
            var currency = _context.Get<string>("currency");
            var phone = _context.Get<string>("phone");
            var fakeSender = _context.Get<ISmsSender>("smsSender");

            fakeSender
                .Received(1)
                .SendSmsAsync(Arg.Is(brandId), Arg.Is(currency), Arg.Is(phone), Arg.Any<string>());
        }
    }
}
