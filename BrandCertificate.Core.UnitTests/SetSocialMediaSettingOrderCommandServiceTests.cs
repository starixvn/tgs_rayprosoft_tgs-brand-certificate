using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;
public class SetSocialMediaSettingOrderCommandServiceTests
{
    [Test]
    public async Task SetSocialMediaSettingOrderCommandService_設定排序()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new SetSocialMediaSettingOrderCommandService(fakeTgsRepository);

        var command = new SetSocialMediaSettingOrderCommand(Guid.NewGuid(), 100, "test1");

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateSocialMediaSettingOrderAsync(
                Arg.Is(command.SettingId),
                Arg.Is(command.NewOrder),
                Arg.Is(command.Modifier));
    }
}
