using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;
public class DeleteSocialMediaSettingCommandServiceTests
{
    [Test]
    public async Task DeleteSocialMediaSettingCommandService_刪除社群連結設定()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new DeleteSocialMediaSettingCommandService(fakeTgsRepository);

        var command = new DeleteSocialMediaSettingCommand(Guid.NewGuid(), Guid.NewGuid());

        _ = fakeTgsRepository.GetSocialMediaSettingAsync(Arg.Is(command.SettingId))
            .Returns(new SocialMediaSetting(
                command.SettingId,
                command.BrandId,
                "aaa",
                SocialType.Facebook,
                new Uri("https://localhost")));

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .DeleteBrandSocialMediaSettingAsync(Arg.Is(command.SettingId));
    }

    [Test]
    public async Task DeleteSocialMediaSettingCommandService_刪除社群連結設定_找不到資料不會動作()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new DeleteSocialMediaSettingCommandService(fakeTgsRepository);

        var command = new DeleteSocialMediaSettingCommand(Guid.NewGuid(), Guid.NewGuid());

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.DidNotReceive()
            .DeleteBrandSocialMediaSettingAsync(Arg.Any<Guid>());
    }
}
