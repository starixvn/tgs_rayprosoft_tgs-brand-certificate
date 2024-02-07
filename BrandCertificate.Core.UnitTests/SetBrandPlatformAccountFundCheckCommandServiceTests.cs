using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class SetBrandPlatformAccountFundCheckCommandServiceTests
{
    [Test]
    public async Task SetBrandPlatformAccountFundCheckCommandService_設定平台資金告警檢查是否啟用()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new SetBrandPlatformAccountFundCheckCommandService(fakeTgsRepository);

        var command = new SetBrandPlatformAccountFundCheckCommand(Guid.NewGuid(), true, "system");

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .SetBrandPlatformAccountFundCheckFlagAsync(
                Arg.Is(command.BrandId),
                Arg.Is(command.Enableld),
                Arg.Is(command.Modifier));
    }
}
