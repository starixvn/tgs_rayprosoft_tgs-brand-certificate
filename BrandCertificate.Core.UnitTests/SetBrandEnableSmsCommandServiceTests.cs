using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class SetBrandEnableSmsCommandServiceTests
{
    [Test]
    public async Task SetBrandEnableSmsCommandService_設定平台啟用簡訊商()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new SetBrandEnableSmsCommandService(fakeTgsRepository);

        var command = new SetBrandEnableSmsCommand(Guid.NewGuid(), true, "system");

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .SetBrandEnableSmsAsync(
                Arg.Is(command.BrandId),
                Arg.Is(command.EnableSms),
                Arg.Is(command.Modifier));
    }
}