using NSubstitute;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class GetBrandBetLimitIdQueryServiceTests
{
    [Test]
    public async Task 取得Brand的預設BetLimitId()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandBetLimitIdQueryService(fakeTgsRepository);

        var query = new GetBrandBetLimitIdQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandBetLimitIdAsync(Arg.Is(query.BrandId));
    }
}
