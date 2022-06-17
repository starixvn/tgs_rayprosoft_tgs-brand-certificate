using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class VipLevelTests
{
    [Test]
    public async Task Get_VipLevels_By_Brand()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetVipLevelsByBrandQueryService(fakeTgsRepository);

        var query = new GetVipLevelsByBrandQuery(Guid.NewGuid());

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryVipLevelsAsync(Arg.Is(query.BrandId));
    }
}
