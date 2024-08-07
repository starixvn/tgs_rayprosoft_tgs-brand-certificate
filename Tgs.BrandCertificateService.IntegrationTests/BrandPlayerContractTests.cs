using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tgs.BrandCertificate.Clients;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests;

public class BrandPlayerContractTests
{
    [Test]
    public async Task 整合測試_取得聯繫信息設定()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.BrandPlayerContractDisplays.AddRangeAsync(
            new BrandPlayerContractDisplay
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Name = "Qq",
                IsVisualized = true,
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "system",
                UpdatedOn = DateTime.UtcNow,
            },
            new BrandPlayerContractDisplay
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Name = "Wechat",
                IsVisualized = true,
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "system",
                UpdatedOn = DateTime.UtcNow,
            });

        _ = await dbContext.SaveChangesAsync();

        var actual = await sut.ListBrandPlayerContractDisplayAsync(brandId).ToArrayAsync();

        Assert.That(actual.Length, Is.EqualTo(2));
    }

    [Test]
    public async Task 整合測試_設定聯繫信息_更新現有值()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var name = "Qq";
        var modifier = "modifier";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.BrandPlayerContractDisplays.AddAsync(
            new BrandPlayerContractDisplay
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Name = "Qq",
                IsVisualized = true,
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "system",
                UpdatedOn = DateTime.UtcNow,
            });

        _ = await dbContext.SaveChangesAsync();

        await sut.SetBrandPlayerContractDisplayAsync(brandId, name, false, modifier);

        var display = dbContext.BrandPlayerContractDisplays.Where(b => b.BrandId == brandId && b.Name == name).First();

        ClassicAssert.AreEqual(modifier, display.UpdatedBy);
        ClassicAssert.AreEqual(false, display.IsVisualized);
    }

    [Test]
    public async Task 整合測試_設定聯繫信息_未設定過會新增()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var name = "Qq";
        var modifier = "modifier";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await sut.SetBrandPlayerContractDisplayAsync(brandId, name, false, modifier);

        var display = dbContext.BrandPlayerContractDisplays.Where(b => b.BrandId == brandId && b.Name == name).First();

        ClassicAssert.AreEqual(brandId, display.BrandId);
        ClassicAssert.AreEqual(name, display.Name);
        ClassicAssert.AreEqual(false, display.IsVisualized);
        ClassicAssert.AreEqual(modifier, display.CreatedBy);
        ClassicAssert.AreEqual(modifier, display.UpdatedBy);
    }
}