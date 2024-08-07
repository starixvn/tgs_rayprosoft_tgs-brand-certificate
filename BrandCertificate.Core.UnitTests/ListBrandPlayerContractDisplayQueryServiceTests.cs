using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class ListBrandPlayerContractDisplayQueryServiceTests
{
    [Test]
    public async Task ListBrandPlayerContractDisplayQueryService_查詢Brand的聯繫信息設定列表()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListBrandPlayerContractDisplayQueryService(fakeTgsRepository);

        var query = new ListBrandPlayerContractDisplayQuery(Guid.NewGuid());

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryBrandPlayerContractDisplayAsync(Arg.Is(query.BrandId));
    }
}