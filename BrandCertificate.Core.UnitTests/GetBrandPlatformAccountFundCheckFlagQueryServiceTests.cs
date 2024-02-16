using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class GetBrandPlatformAccountFundCheckFlagQueryServiceTests
{
    [Test]
    public async Task GetBrandPlatformAccountFundCheckFlagQueryService_取得Brand平台資金檢查設定()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandPlatformAccountFundCheckFlagQueryService(fakeTgsRepository);

        var query = new GetBrandPlatformAccountFundCheckFlagQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandPlatformAccountFundCheckFlagAsync(Arg.Is(query.BrandId));
    }
}
