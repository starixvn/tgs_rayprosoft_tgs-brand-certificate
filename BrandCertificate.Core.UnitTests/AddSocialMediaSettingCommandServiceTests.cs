using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class AddSocialMediaSettingCommandServiceTests
{
    [Test]
    public async Task AddSocialMediaSettingCommandService_新增社群連結()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AddSocialMediaSettingCommandService(fakeTgsRepository);

        var command = new AddSocialMediaSettingCommand(
            Guid.NewGuid(),
            "FX",
            SocialType.Facebook,
            new Uri("https://fx.com/aaaaa"),
            "test1");

        _ = await sut.GetAsync(command);

        _ = fakeTgsRepository.Received(1)
            .InsertBrandSocialMediaSettingAsync(
                Arg.Is<SocialMediaSetting>(s => s.Name == command.Name),
                Arg.Is(command.Modifier));
    }
}
