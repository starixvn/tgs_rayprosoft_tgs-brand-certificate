using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests;

internal class BrandStatusTests
{
    [Test]
    public async Task 從Repository取得BrandStatus()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = await sut.GetAsync(query);

        _ = fakeTgsRepository.Received(1)
            .GetBrandStatusInfoAsync(Arg.Is(query.BrandId));
    }

    [Test]
    public async Task 若找不到Brand_BrandStatus為Inactive()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns((BrandStatusInfo?)null);

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Inactive, actual);
    }

    [Test]
    public async Task 當Brand全關聯階層皆為Active_BrandStatus為Active()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Active,
                BrandStatus.Active,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Active, actual);
    }

    [Test]
    public async Task 當Reseller為Archived_BrandStatus為Archived()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Archived,
                BrandStatus.Active,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Archived, actual);
    }

    [Test]
    public async Task 當Licensee為Archived_BrandStatus為Archived()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Inactive,
                BrandStatus.Archived,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Archived, actual);
    }

    [Test]
    public async Task 當Brand為Archived_BrandStatus為Archived()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Active,
                BrandStatus.Active,
                BrandStatus.Archived));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Archived, actual);
    }

    [Test]
    public async Task 當Reseller為Inactive_BrandStatus為Inactive()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Inactive,
                BrandStatus.Active,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Inactive, actual);
    }

    [Test]
    public async Task 當Licensee為Inactive_BrandStatus為Inactive()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Active,
                BrandStatus.Inactive,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Inactive, actual);
    }

    [Test]
    public async Task 當Brand為Inactive_BrandStatus為Inactive()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Active,
                BrandStatus.Active,
                BrandStatus.Inactive));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Inactive, actual);
    }

    [Test]
    public async Task 當Reseller為Maintenance_BrandStatus為Maintenance()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Maintenance,
                BrandStatus.Active,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Maintenance, actual);
    }

    [Test]
    public async Task 當Licensee為Maintenance_BrandStatus為Maintenance()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Active,
                BrandStatus.Maintenance,
                BrandStatus.Active));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Maintenance, actual);
    }

    [Test]
    public async Task 當Brand為Maintenance_BrandStatus為Maintenance()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetBrandStatusQueryService(fakeTgsRepository);

        var query = new GetBrandStatusQuery(Guid.NewGuid());

        _ = fakeTgsRepository.GetBrandStatusInfoAsync(Arg.Is(query.BrandId))
            .Returns(new BrandStatusInfo(
                BrandStatus.Active,
                BrandStatus.Active,
                BrandStatus.Maintenance));

        var actual = await sut.GetAsync(query);

        Assert.AreEqual(BrandStatus.Maintenance, actual);
    }
}
