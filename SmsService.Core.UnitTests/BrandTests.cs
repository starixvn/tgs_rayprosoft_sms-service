using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tgs.SmsService;
using Tgs.SmsService.ApplicationServices;

namespace SmsService.Core.UnitTests
{
    [TestFixture]
    public class BrandTests
    {
        [Test]
        public async Task Get_BrandInfo()
        {
            var fakeTgsRepository = Substitute.For<ITgsRepository>();

            var sut = new GetBrandInfoQueryService(fakeTgsRepository);

            var query = new GetBrandInfoQuery(Guid.NewGuid());

            await sut.GetAsync(query);

            _ = fakeTgsRepository.Received(1)
                .GetBrandInfoByIdAsync(Arg.Is(query.BrandId));
        }
    }
}
