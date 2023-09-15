using NSubstitute;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class GetBrandSmsVendorSettingsQueryServiceTests
{
    [Test]
    public async Task 取得Brand的Sms設定內容()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandSmsVendorSettingsQueryService(fakeTgsRepository);

        var query = new GetBrandSmsVendorSettingsQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandSmsSettingsAsync(Arg.Is(query.BrandId));
    }
}
