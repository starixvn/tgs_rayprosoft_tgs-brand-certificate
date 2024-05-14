using NUnit.Framework;
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

    [Test]
    public async Task ListBrandSocialMediaSettingsQueryService_查詢Brand的社群連結設定列表_排序會由小到大排列()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListBrandSocialMediaSettingsQueryService(fakeTgsRepository);

        var query = new ListBrandSocialMediaSettingsQuery(Guid.NewGuid());

        _ = fakeTgsRepository.QueryBrandSocialMediaSettingsAsync(Arg.Is(query.BrandId))
            .Returns(new[]
            {
                new SocialMediaSetting(
                    Guid.NewGuid(),
                    query.BrandId,
                    "Test1",
                    SocialType.Facebook,
                    new Uri("http://localhost"),
                    200),
                new SocialMediaSetting(
                    Guid.NewGuid(),
                    query.BrandId,
                    "Test1",
                    SocialType.Facebook,
                    new Uri("http://localhost"),
                    100)
            }.ToAsyncEnumerable());

        var actual = await sut.QueryAsync(query).ToArrayAsync();

        Assert.That(actual[0].Order, Is.EqualTo(100));
        Assert.That(actual[1].Order, Is.EqualTo(200));
    }
}
