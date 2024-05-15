using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Clients;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests;

public class SocialMediaTests
{
    [Test]
    public async Task 整合測試_新增社群連結()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var addSetting = new AddSocialMediaSetting(
            "Test",
            BrandCertificate.SocialType.Facebook,
            new Uri("https://localhost/aa/bb?x=123456789"));
        var modifier = "test1";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var actual = await sut.AddSocialMediaSettingAsync(brandId, addSetting, modifier);
        var actualRow = await dbContext.SocialMediaSettingRows.FindAsync(actual);

        Assert.That(actualRow, Is.Not.Null);
        Assert.That(actualRow!.UpdatedBy, Is.EqualTo(modifier));
        Assert.That(actualRow.Name, Is.EqualTo(addSetting.Name));
    }

    [Test]
    public async Task 整合測試_刪除社群連結()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var settingId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.SocialMediaSettingRows.AddAsync(new SocialMediaSettingRow
        {
            Id = settingId,
            BrandId = brandId,
            Name = "Test",
            Type = nameof(SocialType.Facebook),
            Order = 100,
            Url = "https://localhost/aaa/bb",
            CreatedBy = "system",
            CreatedOn = DateTime.UtcNow,
            UpdatedBy = "system",
            UpdatedOn = DateTime.UtcNow,
        });

        _ = await dbContext.SaveChangesAsync();

        await sut.DeleteSocialMediaSettingAsync(brandId, settingId);
        var actualRow = await dbContext.SocialMediaSettingRows.FindAsync(settingId);

        Assert.That(actualRow, Is.Null);
    }

    [Test]
    public async Task 整合測試_設定社群連結設定排序()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var settingId = Guid.NewGuid();
        var modifier = "test1";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.SocialMediaSettingRows.AddAsync(new SocialMediaSettingRow
        {
            Id = settingId,
            BrandId = brandId,
            Name = "Test",
            Type = nameof(SocialType.Facebook),
            Order = 100,
            Url = "https://localhost/aaa/bb",
            CreatedBy = "system",
            CreatedOn = DateTime.UtcNow,
            UpdatedBy = "system",
            UpdatedOn = DateTime.UtcNow,
        });

        _ = await dbContext.SaveChangesAsync();

        await sut.SetSocialMediaSettingOrderAsync(settingId, 200, modifier);
        var actualRow = await dbContext.SocialMediaSettingRows.FindAsync(settingId);

        Assert.That(actualRow, Is.Not.Null);
        Assert.That(actualRow!.Order, Is.EqualTo(200));
        Assert.That(actualRow.UpdatedBy, Is.EqualTo(modifier));
    }

    [Test]
    public async Task 整合測試_取得社群連結列表()
    {
        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddHttpClientLoggingHandler()));

        var http = web.CreateDefaultClient(web.Services.ResolveLoggingHandler<SocialMediaTests>());
        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.SocialMediaSettingRows.AddRangeAsync(
            new SocialMediaSettingRow
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Name = "Test",
                Type = nameof(SocialType.Facebook),
                Order = 100,
                Url = "https://localhost/aaa/bb",
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "system",
                UpdatedOn = DateTime.UtcNow,
            },
            new SocialMediaSettingRow
            {
                Id = Guid.NewGuid(),
                BrandId = Guid.NewGuid(),
                Name = "Test",
                Type = nameof(SocialType.Facebook),
                Order = 100,
                Url = "https://localhost/aaa/bb",
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "system",
                UpdatedOn = DateTime.UtcNow,
            },
            new SocialMediaSettingRow
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Name = "Test",
                Type = nameof(SocialType.Facebook),
                Order = 100,
                Url = "https://localhost/aaa/bb",
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = "system",
                UpdatedOn = DateTime.UtcNow,
            });

        _ = await dbContext.SaveChangesAsync();

        var actual = await sut.ListSocialMediaSettingsAsync(brandId).ToArrayAsync();

        Assert.That(actual.Length, Is.EqualTo(2));
    }
}
