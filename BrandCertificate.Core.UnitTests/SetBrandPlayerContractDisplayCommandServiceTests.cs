using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

public class SetBrandPlayerContractDisplayCommandServiceTests
{
    [Test]
    public async Task SetBrandPlayerContractDisplayCommandService_設置Brand的聯繫信息設定()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new SetBrandPlayerContractDisplayCommandService(fakeTgsRepository);

        var command = new SetBrandPlayerContractDisplayCommand(Guid.NewGuid(), "Qq", true, "system");

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpsertBrandPlayerContractDisplayAsync(
                Arg.Is(command.BrandId),
                Arg.Is(command.Name),
                Arg.Is(command.IsVisualized),
                Arg.Is(command.Modifier));
    }
}