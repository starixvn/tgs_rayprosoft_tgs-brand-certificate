using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using TGS.Paging;

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

        var query = new GetBrandMaintenanceInfosQuery(
            "",
            new Paging<BrandMaintenanceInfo> { Page = 1, RowCount = 30 });

        _ = await sut.QueryAsync(query).ToArrayAsync();

        _ = fakeTgsRepository.Received(1)
            .QueryBrandMaintenanceInfosAsync(Arg.Is(query.BrandCode));
    }

    [Test]
    public async Task Count_Brand_Maintenance_Infos()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandMaintenanceInfosQueryService(fakeTgsRepository);

        var query = new GetBrandMaintenanceInfosQuery(
            "",
            new Paging<BrandMaintenanceInfo> { Page = 1, RowCount = 30 });

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .QueryBrandMaintenanceInfosAsync(Arg.Is(query.BrandCode));
    }

    [Test]
    public async Task Batch_Update_Brand_Maintenance_Time()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new BatchUpdateBrandMaintenanceCommandService(fakeTgsRepository);

        var brandIds = new Guid[]
        {
            Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            Guid.Parse("d71ac5ed-b7d9-49f2-a129-8709fa4cc02a")
        };

        var beginTime = TimeOnly.Parse("09:00:00");
        var endTime = TimeOnly.Parse("10:30:00");

        var command = new BatchUpdateBrandMaintenanceCommand(brandIds, beginTime, endTime);
        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .BatchUpdateBrandMaintenanceTimeAsync(Arg.Is(brandIds), Arg.Is(beginTime), Arg.Is(endTime));
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
        var sut = new UpdateBrandMaintenanceCommandService(fakeTgsRepository);

        var brandMaintenanceId = Guid.NewGuid();
        var content = "This is new Content";

        var command = new UpdateBrandMaintenanceCommand(brandMaintenanceId, content);

        await sut.ExecuteAsync(command);

        _ = fakeTgsRepository.Received(1)
            .UpdateBrandMaintenanceInfoAsync(Arg.Is(brandMaintenanceId), Arg.Is(content));
    }
}