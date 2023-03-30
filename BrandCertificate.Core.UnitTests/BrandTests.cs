using System.Text.Json;
using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.MessageQueue;

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

    [Test]
    public async Task Get_Brand_PaymentSettings()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandPaymentSettingsQueryService(fakeTgsRepository);

        var query = new GetBrandPaymentSettingsQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandPaymentSettingsAsync(Arg.Is(query.BrandId));
    }

    [Test]
    public async Task Get_Brand_Maintenance_Infos()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandMaintenanceInfosQueryService(fakeTgsRepository);

        var query = new GetBrandMaintenanceInfosQuery("");

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryBrandMaintenanceInfosAsync(Arg.Is(query.BrandCode));
    }

    [Test]
    public async Task Update_All_Brand_Maintenance_Time()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new UpdateAllBrandMaintenanceTimeCommandService(fakeTgsRepository);

        var beginTime = TimeOnly.Parse("09:00:00");
        var endTime = TimeOnly.Parse("10:30:00");

        var command = new UpdateAllBrandMaintenanceTimeCommand(beginTime, endTime);
        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateAllBrandMaintenanceTimeAsync(Arg.Is(beginTime), Arg.Is(endTime));
    }

    [Test]
    public async Task Get_Brand_Maintenance_Info()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandMaintenanceInfoQueryService(fakeTgsRepository);

        var brandMaintenanceId = Guid.NewGuid();
        var query = new GetBrandMaintenanceInfoQuery(brandMaintenanceId);
        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandMaintenanceInfoByIdAsync(Arg.Is(brandMaintenanceId));
    }

    [Test]
    public async Task Update_Brand_Maintenance_Content()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new UpdateBrandMaintenanceContentCommandService(fakeTgsRepository);

        var brandMaintenanceId = Guid.NewGuid();
        var content = "This is new Content";

        var command = new UpdateBrandMaintenanceContentCommand(brandMaintenanceId, content);

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateBrandMaintenanceContentAsync(Arg.Is(brandMaintenanceId), Arg.Is(content));
    }

    [Test]
    public async Task Update_Brand_Maintenance_Time()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();
        var sut = new UpdateBrandMaintenanceTimeCommandService(fakeTgsRepository);

        var brandId = Guid.NewGuid();
        var beginTime = new TimeOnly(9, 0, 0);
        var endTime = new TimeOnly(10, 0, 0);

        var command = new UpdateBrandMaintenanceTimeCommand(brandId, beginTime, endTime);

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateBrandMaintenanceTimeAsync(Arg.Is(brandId), Arg.Is(beginTime), Arg.Is(endTime));
    }

    [Test]
    public async Task Update_Brand_Player_Register_Flow_Setting()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var fakeMessageSender = Substitute.For<IMessageSender>();
        var sut = new UpdateBrandPlayerRegisterFlowSettingCommandService(fakeTgsRepository, fakeMessageSender);

        var brandId = Guid.NewGuid();
        var settingJson = JsonDocument.Parse("[{\"type\": \"InitPhone\"}]");
        _ = fakeTgsRepository.UpdateBrandPlayerRegisterFlowSettingAsync(brandId, settingJson).Returns(true);

        var command = new UpdateBrandPlayerRegisterFlowSettingCommand(brandId, settingJson);

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateBrandPlayerRegisterFlowSettingAsync(Arg.Is(brandId), Arg.Is(settingJson));

        _ = fakeMessageSender.Received(1)
            .PublishAsync(Arg.Is("tgs.brand.frontendinfo.changed"), Arg.Any<ReadOnlyMemory<byte>>());
    }

    [Test]
    public async Task 查詢BrandList()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new BrandListQueryService(fakeTgsRepository);

        var query = new BrandListQuery(
            Array.Empty<Guid>(),
            null,
            BrandStatus.Archived);

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryAllBrandsAsync(
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task 取得Brand關係表()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new BrandRelationQueryService(fakeTgsRepository);

        _ = await sut.QueryAsync(new BrandRelationQuery()).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .GetBrandRelationsAsync(
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task 更新Brand狀態()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new UpdateBrandStatusCommandService(fakeTgsRepository);

        var command = new UpdateBrandStatusCommand(
            Guid.NewGuid(),
            BrandStatus.Active,
            "user1");

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateBrandStatusAsync(
                Arg.Is(command.BrandId),
                Arg.Is(command.Status),
                Arg.Is(command.Modifier));
    }
}
