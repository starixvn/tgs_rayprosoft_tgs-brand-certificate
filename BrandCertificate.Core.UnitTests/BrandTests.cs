using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

[TestFixture]
public class BrandTests
{
    [Test]
    public async Task Get_BrandInfo_From_Repository()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandInfoQueryService(fakeTgsRepository);

        var query = new GetBrandInfoQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandInfoAsync(Arg.Is(query.BrandId));
    }

    [Test]
    public async Task Get_BrandFrontendInfo_From_Repository()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandFrontendInfoQueryService(fakeTgsRepository);

        var query = new GetBrandFrontendInfoQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandFrontendInfoAsync(Arg.Is(query.BrandId));
    }

    [Test]
    public async Task Get_Brand_Status_From_Repository()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandStatusAsync(Arg.Is(query.BrandId));
    }
}
