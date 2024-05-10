using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;
public class ListBrandSocialMediaSettingsQueryServiceTests
{
    [Test]
    public async Task ListBrandSocialMediaSettingsQueryService_查詢Brand的社群連結設定列表()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListBrandSocialMediaSettingsQueryService(fakeTgsRepository);

        var query = new ListBrandSocialMediaSettingsQuery(Guid.NewGuid());

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryBrandSocialMediaSettingsAsync(Arg.Is(query.BrandId));
    }
}
