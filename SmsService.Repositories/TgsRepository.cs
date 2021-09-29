using System;
using System.Text.Json;
using System.Threading.Tasks;
using Tgs.Sms.Repositories.Entities;
using Tgs.SmsService;

namespace SmsService.Repositories
{
    internal class TgsRepository : ITgsRepository
    {
        private readonly TgsContext _tgsContext;

        public TgsRepository(TgsContext tgsContext)
        {
            _tgsContext = tgsContext ?? throw new ArgumentNullException(nameof(tgsContext));
        }

        public async ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId)
        {
            var brand = await _tgsContext.Brands
                .FindAsync(brandId)
                .ConfigureAwait(false);

            if (brand == null)
                return null;

            return new BrandInfo(
                brand.Id,
                brand.EnableSms,
                brand.SmsVendorCode,
                string.IsNullOrWhiteSpace(brand.SmsVendorSettings)
                    ? null
                    : JsonDocument.Parse(brand.SmsVendorSettings));
        }
    }
}
