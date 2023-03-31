using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

internal class BrandStatusTests
{

    [Test]
    public async Task 從Repository取得BrandStatus()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandStatusInfoAsync(Arg.Is(query.BrandId));
    }

    [Test]
    public async Task 若找不到Brand_BrandStatus為Inactive()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns((BrandStatusInfo?)null);

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Inactive, actual);
    }

}