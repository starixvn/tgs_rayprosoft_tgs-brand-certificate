using System.Text.Json;
using ExpectedObjects;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.Clients;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests;

public class BrandTests
{
    [Test]
    public async Task Get_BrandInfo_By_Id()
    {
        var brandId = Guid.NewGuid();

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty
        });

        _ = await dbContext.BrandCurrencies.AddAsync(new BrandCurrency
        {
            BrandId = brandId,
            CurrencyCode = "CNY",
            BrandCurrencyCode = "CNY"
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandInfoAsync(brandId);

        ClassicAssert.IsNotNull(actual);
        ClassicAssert.AreEqual(brandId, actual!.Id);
    }

    [Test]
    public async Task 以BrandCode取得BrandInfo()
    {
        var brandId = Guid.NewGuid();
        var brandCode = "Test";

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            Code = brandCode,
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty
        });

        _ = await dbContext.BrandCurrencies.AddAsync(new BrandCurrency
        {
            BrandId = brandId,
            CurrencyCode = "CNY",
            BrandCurrencyCode = "CNY"
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandInfoByCodeAsync(brandCode);

        ClassicAssert.IsNotNull(actual);
        ClassicAssert.AreEqual(brandId, actual!.Id);
    }

    [Test]
    public async Task 找不到BrandInfo會拿到Null()
    {
        var brandId = Guid.NewGuid();

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandInfoAsync(brandId);

        ClassicAssert.IsNull(actual);
    }

    [Test]
    public async Task Get_BrandFrontendInfo_By_Id()
    {
        var brandId = Guid.NewGuid();

        var brandLogoSettings = new
        {
            DesktopLogoUrl = new Uri("https://localhost/1.png"),
            MobileLogoUrl = new Uri("https://localhost/2.png"),
            LoadingUrl = new Uri("https://localhost/3.png"),
            PlayerRegisterFlowSettings = JsonDocument.Parse(@"[{""type"":""Test1""}]")
        };

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty,
            LogoInfo = JsonSerializer.Serialize(brandLogoSettings),
            PlayerRegisterFlowSettings = JsonSerializer.Serialize(brandLogoSettings.PlayerRegisterFlowSettings)
        });

        _ = await dbContext.BrandCurrencies.AddAsync(new BrandCurrency
        {
            BrandId = brandId,
            CurrencyCode = "CNY",
            BrandCurrencyCode = "CNY"
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandFrontendInfoAsync(brandId);

        brandLogoSettings.ToExpectedObject(ctx =>
        {
            ctx.MapStrategy<JsonDocument>(new JsonDocumentComparisionStrategy());
        }).ShouldMatch(actual);
    }

    [Test]
    public async Task Get_BrandStatus_By_Id()
    {
        var brandId = Guid.NewGuid();

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var reseller = await dbContext.Resellers.AddAsync(new Reseller
        {
            Id = brandId,
            Code = "Test",
            CreatedByUsername = string.Empty,
            Name = "Test",
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active
        });

        var licensee = await dbContext.Licensees.AddAsync(new Licensee
        {
            Id = brandId,
            Code = "Test",
            CreatedByUsername = string.Empty,
            Name = "Test",
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            Reseller = reseller.Entity
        });

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Archived,
            Licensee = licensee.Entity
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandStatusAsync(brandId);

        ClassicAssert.AreEqual(BrandStatus.Archived, actual);
    }

    [Test]
    public async Task Get_Brand_PaymentSettings()
    {
        var brandId = Guid.NewGuid();

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            DepositPaymentLockEnabled = true,
            AutoClearWageringRequirement = true,
            AutoClearWageringRequirementBalance = 5M
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var expected = new BrandCertificate.Clients.BrandPaymentSettings(
            true,
            true,
            5M);

        var actual = await sut.GetBrandPaymentSettingsAsync(brandId);

        ClassicAssert.AreEqual(expected, actual);
    }

    [Test]
    public async Task Get_Brand_Maintenance_Infos_By_Brand_Code()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brandId = Guid.Parse(input: "4e86719f-243f-46f6-b010-5c758b7285af");

        _ = await dbContext.BrandMaintenances.AddAsync(new BrandMaintenance
        {
            Id = Guid.NewGuid(),
            BrandId = brandId,
            Brand = new Brand
            {
                Id = brandId,
                Code = "GPI",
                Status = 0,
                ApiKey = "apiKey",
                ClientId = "GPI",
                Licensee = new Licensee
                {
                    Code = "TGS",
                    Name = "Turnkey Game System",
                    CreatedByUsername = "",
                    UpdatedByUsername = ""
                },
                CreatedByUsername = "",
                UpdatedByUsername = "",
                SecretKey = "secretKey",
                Name = "GameProvider Integration"
            },
            BeginTime = TimeOnly.Parse("09:00:00"),
            EndTime = TimeOnly.Parse("10:30:00")
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandMaintenanceInfosAsync(brandCode: "").ToArrayAsync();

        ClassicAssert.AreEqual(1, actual.Length);
    }

    [Test]
    public async Task Update_All_Brand_Maintenance_Time()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.Parse(input: "4e86719f-243f-46f6-b010-5c758b7285af");
        _ = await dbContext.BrandMaintenances.AddAsync(new BrandMaintenance
        {
            Id = Guid.NewGuid(),
            BrandId = brandId,
            Brand = new Brand
            {
                Id = brandId,
                Code = "GPI",
                ApiKey = "apiKey",
                ClientId = "GPI",
                Licensee = new Licensee
                {
                    Code = "TGS",
                    Name = "Turnkey Game System",
                    CreatedByUsername = "",
                    UpdatedByUsername = ""
                },
                CreatedByUsername = "",
                UpdatedByUsername = "",
                SecretKey = "secretKey",
                Name = "GameProvider Integration"
            },
            BeginTime = TimeOnly.Parse("09:00:00"),
            EndTime = TimeOnly.Parse("10:00:00")
        });

        _ = await dbContext.SaveChangesAsync();

        var beginTime = new TimeOnly(9, 30, 0);
        var endTime = new TimeOnly(10, 30, 0);

        await sut.UpdateAllBrandMaintenanceTimeAsync(
            beginTime,
            endTime);

        var bm = dbContext.BrandMaintenances.Where(bm => bm.BrandId == brandId).First();

        ClassicAssert.AreEqual(TimeOnly.Parse("09:30:00"), bm.BeginTime);
        ClassicAssert.AreEqual(TimeOnly.Parse("10:30:00"), bm.EndTime);
    }

    [Test]
    public async Task Get_Brand_Maintenance_Info_By_Brand_Maintenance_Id()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brandMaintenanceId = Guid.NewGuid();
        var brandId = Guid.Parse(input: "4e86719f-243f-46f6-b010-5c758b7285af");

        _ = await dbContext.BrandMaintenances.AddAsync(new BrandMaintenance
        {
            Id = brandMaintenanceId,
            BrandId = brandId,
            Brand = new Brand
            {
                Id = brandId,
                Code = "GPI",
                ApiKey = "apiKey",
                ClientId = "GPI",
                Licensee = new Licensee
                {
                    Code = "TGS",
                    Name = "Turnkey Game System",
                    CreatedByUsername = "",
                    UpdatedByUsername = ""
                },
                CreatedByUsername = "",
                UpdatedByUsername = "",
                SecretKey = "secretKey",
                Name = "GameProvider Integration"
            },
            BeginTime = TimeOnly.Parse("09:00:00"),
            EndTime = TimeOnly.Parse("10:30:00"),
            Content = "Content"
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandMaintenanceInfoByIdAsync(brandMaintenanceId);

        ClassicAssert.AreEqual("Content", actual.Content);
    }

    [Test]
    public async Task Update_Brand_Maintenance_Content()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandMaintenanceId = Guid.NewGuid();
        var brandId = Guid.Parse(input: "4e86719f-243f-46f6-b010-5c758b7285af");
        _ = await dbContext.BrandMaintenances.AddAsync(new BrandMaintenance
        {
            Id = brandMaintenanceId,
            BrandId = brandId,
            Brand = new Brand
            {
                Id = brandId,
                Code = "GPI",
                ApiKey = "apiKey",
                ClientId = "GPI",
                Licensee = new Licensee
                {
                    Code = "TGS",
                    Name = "Turnkey Game System",
                    CreatedByUsername = "",
                    UpdatedByUsername = ""
                },
                CreatedByUsername = "",
                UpdatedByUsername = "",
                SecretKey = "secretKey",
                Name = "GameProvider Integration"
            },
            BeginTime = TimeOnly.Parse("09:00:00"),
            EndTime = TimeOnly.Parse("10:30:00"),
            Content = "old content"
        });

        _ = await dbContext.SaveChangesAsync();

        await sut.UpdateBrandMaintenanceContentAsync(
            brandMaintenanceId,
            "This is new content");

        var bm = dbContext.BrandMaintenances.Where(bm => bm.Id == brandMaintenanceId).First();

        ClassicAssert.AreEqual("This is new content", bm.Content);
    }

    [Test]
    public async Task Update_Brand_Maintenance_Time()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.Parse(input: "4e86719f-243f-46f6-b010-5c758b7285af");
        _ = await dbContext.BrandMaintenances.AddAsync(new BrandMaintenance
        {
            Id = Guid.NewGuid(),
            BrandId = brandId,
            Brand = new Brand
            {
                Id = brandId,
                Code = "GPI",
                ApiKey = "apiKey",
                ClientId = "GPI",
                Licensee = new Licensee
                {
                    Code = "TGS",
                    Name = "Turnkey Game System",
                    CreatedByUsername = "",
                    UpdatedByUsername = ""
                },
                CreatedByUsername = "",
                UpdatedByUsername = "",
                SecretKey = "secretKey",
                Name = "GameProvider Integration"
            },
            BeginTime = TimeOnly.Parse("09:00:00"),
            EndTime = TimeOnly.Parse("10:00:00")
        });

        _ = await dbContext.SaveChangesAsync();

        await sut.UpdateBrandMaintenanceTimeAsync(
            brandId,
            new TimeOnly(9, 30, 0),
            new TimeOnly(10, 30, 0));

        var bm = dbContext.BrandMaintenances.Where(bm => bm.BrandId == brandId).First();

        ClassicAssert.AreEqual(TimeOnly.Parse("09:30:00"), bm.BeginTime);
        ClassicAssert.AreEqual(TimeOnly.Parse("10:30:00"), bm.EndTime);
    }

    [Test]
    public async Task Get_Brand_Ids_By_Time_Span_Minutes()
    {
        var brandId = Guid.NewGuid();
        var timeSpanMinutes = 480;

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            TimeSpanMinutes = timeSpanMinutes,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            DepositPaymentLockEnabled = true
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandIdsByTimeSpanMinutesAsync(timeSpanMinutes).ToArrayAsync();

        ClassicAssert.AreEqual(1, actual.Length);
    }

    [Test]
    public async Task Get_Time_Span_Minutes_By_Brand_Ids()
    {
        var brandId = Guid.NewGuid();
        var timeSpanMinutes = 480;

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            TimeSpanMinutes = timeSpanMinutes,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            DepositPaymentLockEnabled = true
        });

        _ = await dbContext.BrandCurrencies.AddAsync(new BrandCurrency
        {
            BrandId = brandId,
            CurrencyCode = "CNY",
            BrandCurrencyCode = "CNY"
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var brandInfo = await sut.GetBrandInfoAsync(brandId);

        ClassicAssert.AreEqual(timeSpanMinutes, brandInfo?.TimeSpanMinutes);
    }

    [Test]
    public async Task 以Keyword查詢Brand列表()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.Brands.AddRangeAsync(
            new Brand
            {
                Id = Guid.NewGuid(),
                Code = "Test1",
                Name = "Test1",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Active
            },
            new Brand()
            {
                Id = Guid.NewGuid(),
                Code = "Test2",
                Name = "Test2",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Active
            });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var query = new BrandListQuery
        {
            Keyword = "Test"
        };

        var actual = await sut.QueryBrandListAsync(query).ToArrayAsync();

        ClassicAssert.AreEqual(2, actual.Length);
    }

    [Test]
    public async Task 以Status查詢Brand列表()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        await dbContext.Brands.AddRangeAsync(
            new Brand
            {
                Id = Guid.NewGuid(),
                Code = "Test1",
                Name = "Test1",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Active
            },
            new Brand
            {
                Id = Guid.NewGuid(),
                Code = "Test2",
                Name = "Test2",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Archived
            });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var query = new BrandListQuery
        {
            Status = BrandStatus.Archived
        };

        var actual = await sut.QueryBrandListAsync(query).ToArrayAsync();

        ClassicAssert.AreEqual(1, actual.Length);
    }

    [Test]
    public async Task 以BrandId查詢Brand列表()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brandId = Guid.NewGuid();
        await dbContext.Brands.AddRangeAsync(
            new Brand
            {
                Id = brandId,
                Code = "Test1",
                Name = "Test1",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Active
            },
            new Brand
            {
                Id = Guid.NewGuid(),
                Code = "Test2",
                Name = "Test2",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Archived
            });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var query = new BrandListQuery
        {
            BrandIds = [brandId]
        };

        var actual = await sut.QueryBrandListAsync(query).ToArrayAsync();

        ClassicAssert.AreEqual(1, actual.Length);
    }

    [Test]
    public async Task 取得Brand關聯表()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var reseller = await dbContext.Resellers.AddAsync(new Reseller
        {
            Id = Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            Code = "TGS",
            Name = "TGS",
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active
        });

        var licensee = await dbContext.Licensees.AddAsync(new Licensee
        {
            Id = Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            Code = "TGS",
            Name = "TGS",
            Status = (short)BrandStatus.Active,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Reseller = reseller.Entity
        });

        await dbContext.Brands.AddRangeAsync(
            new Brand
            {
                Id = Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                Code = "Test1",
                Name = "Test1",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Active,
                Licensee = licensee.Entity
            },
            new Brand
            {
                Id = Guid.Parse("9f1ecf19-38cd-474f-b0d2-93638dc07d21"),
                Code = "Test2",
                Name = "Test2",
                ApiKey = string.Empty,
                ClientId = string.Empty,
                SecretKey = string.Empty,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Status = (short)BrandStatus.Active,
                Licensee = licensee.Entity
            });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var expected = new BrandCertificate.Clients.BrandRelationInfo[]
        {
            new(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                "Test1",
                "Test1",
                Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                "TGS",
                "TGS",
                Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                "TGS",
                "TGS"),
            new(Guid.Parse("9f1ecf19-38cd-474f-b0d2-93638dc07d21"),
                "Test2",
                "Test2",
                Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                "TGS",
                "TGS",
                Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                "TGS",
                "TGS")
        };

        var actual = await sut.GetBrandRelationsAsync().ToArrayAsync();

        ClassicAssert.AreEqual(expected, actual);
    }

    [Test]
    public async Task 取得全部Brand的幣別()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        await using var web = new TestBrandCertificateApplication(hostBuilder => hostBuilder
            .ConfigureServices(services => services
                .AddSingleton(fakeTgsRepository)));

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var brandCurrencies = new[]
        {
            new BrandCertificate.BrandCurrencyInfo(
                Guid.NewGuid(),
                "CNY"),
            new BrandCertificate.BrandCurrencyInfo(
                Guid.NewGuid(),
                "TWD")
        };

        _ = fakeTgsRepository.GetAllBrandCurrenciesAsync(Arg.Any<CancellationToken>())
            .Returns(brandCurrencies.ToAsyncEnumerable());

        var expected = brandCurrencies.Select(info => new BrandCertificate.Clients.BrandCurrencyInfo(
            info.BrandId,
            info.Currency)).ToArray();

        var actual = await sut.GetAllBrandCurrenciesAsync().ToArrayAsync();

        ClassicAssert.AreEqual(expected, actual);
    }

    [Test]
    public async Task 更新BrandStatus()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brandId = Guid.NewGuid();
        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "Test1",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            LicenseeId = Guid.NewGuid()
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        await sut.UpdateBrandStatusAsync(brandId, BrandStatus.Inactive, "ModifyUser");

        ClassicAssert.AreEqual((short)BrandStatus.Inactive, brand.Status);
    }

    [Test]
    public async Task 取得Brand的Sms設定值()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var vendorSettings = new TestSmsSettings("123456");
        var vendorCode = "Test";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "Test1",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            LicenseeId = Guid.NewGuid(),
            SmsVendorCode = vendorCode,
            SmsVendorSettings = JsonSerializer.Serialize(vendorSettings)
        };

        var expected = new SmsVendorInfo<TestSmsSettings>(vendorCode, vendorSettings);

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        var actual = await sut.GetBrandSmsSettingsAsync<TestSmsSettings>(brandId);

        ClassicAssert.AreEqual(expected, actual);
    }

    [Test]
    public async Task 取得Brand預設BetLimitId()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.BrandBetLimits
            .AddAsync(new BrandBetLimit
            {
                BrandId = brandId,
                BetLimitId = 100,
                IsDefault = true
            });

        _ = await dbContext.SaveChangesAsync();

        var actual = await sut.GetBrandBetLimitIdAsync(brandId);

        ClassicAssert.AreEqual(100, actual);
    }

    [Test]
    public async Task 整合測試_取得平台資金檢查設定()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "Test1",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            LicenseeId = Guid.NewGuid(),
            EnablePlatformAccountFundCheck = true
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        var actual = await sut.GetBrandPlatformAccountFundAsync(brandId);

        Assert.That(actual, Is.True);
    }

    [Test]
    public async Task 整合測試_設定平台資金檢查設定()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();
        var modifier = "test1";

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "Test1",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            LicenseeId = Guid.NewGuid(),
            EnablePlatformAccountFundCheck = false
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        await sut.SetBrandPlatformAccountFundAsync(brandId, true, modifier);

        Assert.That(brand.EnablePlatformAccountFundCheck, Is.True);
        Assert.That(brand.UpdatedByUsername, Is.EqualTo(modifier));
    }

    [Test]
    public async Task Get_Brand_Agent_Dividends()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "name",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            AgentDividendType = (short)AgentDividendType.OnePercentage,
            SettlementIntervalType = (short)SettlementIntervalType.DoubleWeekly,
            AutoSentAgentBonus = true,
            CheckDividendActiveCount = true,
            PlatformCharge = 10M,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            SettlementDay = null
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        var actual = await sut.GetBrandAgentDividendInfosAsync(default).ToArrayAsync();

        Assert.That(actual.Length, Is.EqualTo(1));
    }

    [Test]
    public async Task Update_Brand_Agent_Dividend_Time_Interval()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var previousSettlementFrom = DateTimeOffset.Parse("2024-01-10 16:00:00+8").UtcDateTime;
        var previousSettlementTo = DateTimeOffset.Parse("2024-01-10 16:00:00+8").UtcDateTime;
        var currentSettlementFrom = DateTimeOffset.Parse("2024-01-11 16:00:00+8").UtcDateTime;
        var currentSettlementTo = DateTimeOffset.Parse("2024-01-11 16:00:00+8").UtcDateTime;
        var nextSettlementFrom = DateTimeOffset.Parse("2024-01-12 16:00:00+8").UtcDateTime;
        var nextSettlementTo = DateTimeOffset.Parse("2024-01-12 16:00:00+8").UtcDateTime;

        var brandId = Guid.NewGuid();
        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "Test1",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            LicenseeId = Guid.NewGuid(),
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        await sut.UpdateBrandAgentDividendTimeIntervalAsync(new UpdateBrandAgentDividendTimeIntervalRequest(brandId,
            previousSettlementFrom,
            previousSettlementTo,
            currentSettlementFrom,
            currentSettlementTo,
            nextSettlementFrom,
            nextSettlementTo));

        Assert.That(brand.PreviousSettlementFrom, Is.EqualTo(previousSettlementFrom));
        Assert.That(brand.PreviousSettlementTo, Is.EqualTo(previousSettlementTo));
        Assert.That(brand.CurrentSettlementFrom, Is.EqualTo(currentSettlementFrom));
        Assert.That(brand.CurrentSettlementTo, Is.EqualTo(currentSettlementTo));
        Assert.That(brand.NextSettlementFrom, Is.EqualTo(nextSettlementFrom));
        Assert.That(brand.NextSettlementTo, Is.EqualTo(nextSettlementTo));
    }

    [Test]
    public async Task Create_Brand()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = await sut.CreateBrandAsync(new CreateBrandFullInfoRequest
        {
            Code = "code",
            Name = "name",
            PlatformCharge = 10,
            Description = "Description",
            OperationalEmail = "OperationalEmail",
            TimeSpanMinutes = 11,
            EnablePlayerAgent = true,
            LicenseeId = Guid.NewGuid(),
            SettlementIntervalType = SettlementIntervalType.DoubleWeekly,
            SettlementDay = 30,
            AgentDividendType = AgentDividendType.OneLayer,
            CheckDividendActiveCount = true,
            AutoClearWageringRequirement = true,
            AutoSendAgentBonus = true,
            EnablePlayerIdentity = true,
            AutoClearWageringRequirementBalance = 100,
            DepositHandlingFeeRateTenPercentile = 11,
            WithdrawHandlingFeeRateTenPercentile = 11,
            EditorUsername = "test"
        });

        var brand = dbContext.Brands.First(x => x.Id == brandId);

        Assert.That(brand.Id, Is.EqualTo(brandId));
        Assert.That(brand.Code, Is.EqualTo("code"));
        Assert.That(brand.Name, Is.EqualTo("name"));
        Assert.That(brand.PlatformCharge, Is.EqualTo(10));
        Assert.That(brand.Description, Is.EqualTo("Description"));
        Assert.That(brand.OperationalEmail, Is.EqualTo("OperationalEmail"));
        Assert.That(brand.EnablePlayerAgent, Is.EqualTo(true));
        Assert.That(brand.SettlementIntervalType, Is.EqualTo((short)SettlementIntervalType.DoubleWeekly));
        Assert.That(brand.SettlementDay, Is.EqualTo(30));
        Assert.That(brand.CheckDividendActiveCount, Is.EqualTo(true));
        Assert.That(brand.AutoClearWageringRequirement, Is.EqualTo(true));
        Assert.That(brand.AutoSentAgentBonus, Is.EqualTo(true));
        Assert.That(brand.EnablePlayerIdentity, Is.EqualTo(true));
        Assert.That(brand.AutoClearWageringRequirementBalance, Is.EqualTo(100));
        Assert.That(brand.EnablePlayerIdentity, Is.EqualTo(true));
        Assert.That(brand.DepositHandlingFeeRateTenPercentile, Is.EqualTo(11));
        Assert.That(brand.DepositHandlingFeeRateTenPercentile, Is.EqualTo(11));
    }

    [Test]
    public async Task Update_Brand()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var brand = new Brand
        {
            Id = brandId,
            Code = "Test1",
            Name = "Test1",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            SecretKey = string.Empty,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Status = (short)BrandStatus.Active,
            LicenseeId = Guid.NewGuid(),
            SettlementIntervalType = (short)SettlementIntervalType.DoubleWeekly,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        var responseBrandId = await sut.UpdateBrandAsync(new UpdateBrandFullInfoRequest
        {
            Id = brandId,
            Name = "name1",
            PlatformCharge = 11,
            Description = "Description1",
            OperationalEmail = "OperationalEmail1",
            EnablePlayerAgent = true,
            SettlementIntervalType = SettlementIntervalType.DoubleWeekly,
            SettlementDay = 11,
            CheckDividendActiveCount = true,
            AutoClearWageringRequirement = true,
            AutoSendAgentBonus = true,
            EnablePlayerIdentity = true,
            AutoClearWageringRequirementBalance = 100,
            DepositHandlingFeeRateTenPercentile = 11,
            WithdrawHandlingFeeRateTenPercentile = 11,
            EditorUsername = "test",
            AreaCodes = ["+86", "+852"]
        });

        var actualBrand = dbContext.Brands.First(x => x.Id == brandId);

        Assert.That(actualBrand.Name, Is.EqualTo("name1"));
        Assert.That(actualBrand.PlatformCharge, Is.EqualTo(11));
        Assert.That(actualBrand.Description, Is.EqualTo("Description1"));
        Assert.That(actualBrand.OperationalEmail, Is.EqualTo("OperationalEmail1"));
        Assert.That(actualBrand.EnablePlayerAgent, Is.EqualTo(true));
        Assert.That(actualBrand.SettlementIntervalType, Is.EqualTo((short)SettlementIntervalType.DoubleWeekly));
        Assert.That(actualBrand.SettlementDay, Is.EqualTo(11));
        Assert.That(actualBrand.CheckDividendActiveCount, Is.EqualTo(true));
        Assert.That(actualBrand.AutoClearWageringRequirement, Is.EqualTo(true));
        Assert.That(actualBrand.AutoSentAgentBonus, Is.EqualTo(true));
        Assert.That(actualBrand.EnablePlayerIdentity, Is.EqualTo(true));
        Assert.That(actualBrand.AutoClearWageringRequirementBalance, Is.EqualTo(100));
        Assert.That(actualBrand.EnablePlayerIdentity, Is.EqualTo(true));
        Assert.That(actualBrand.DepositHandlingFeeRateTenPercentile, Is.EqualTo(11));
        Assert.That(actualBrand.DepositHandlingFeeRateTenPercentile, Is.EqualTo(11));
        Assert.That(actualBrand.AreaCodes, Is.EquivalentTo(JsonSerializer.Serialize(new[] { "+86", "+852" })));
    }

    [Test]
    public async Task Get_Brand_Full_Info_By_Id()
    {
        var brandId = Guid.NewGuid();
        var licenseeId = Guid.NewGuid();
        var resellerId = Guid.NewGuid();

        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        _ = await dbContext.Brands.AddAsync(new Brand
        {
            Id = brandId,
            Code = "Test",
            ApiKey = string.Empty,
            ClientId = string.Empty,
            CreatedByUsername = string.Empty,
            Name = "Test",
            SecretKey = string.Empty,
            AgentDividendType = (short)AgentDividendType.MultipleLayer,
            SettlementIntervalType = (short)SettlementIntervalType.DoubleWeekly,
            PlatformCharge = 10,
            AutoSentAgentBonus = true,
            EnablePlayerAgent = true,
            AutoClearWageringRequirementBalance = 100,
            UpdatedByUsername = string.Empty,
            BrandCurrencies = [
                new BrandCurrency
                {
                    BrandId = brandId,
                    CurrencyCode = "CNY",
                    BrandCurrencyCode = "CNY"
                }
            ],
            Licensee = new Licensee
            {
                Id = licenseeId,
                ResellerId = resellerId,
                Code = "licensee",
                Name = "licensee",
                CreatedByUsername = "user",
                UpdatedByUsername = "user",
                Reseller = new Reseller
                {
                    Id = resellerId,
                    Code = "reseller",
                    Name = "reseller",
                    CreatedByUsername = "user",
                    UpdatedByUsername = "user"
                }
            }
        });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandFullInfoAsync(brandId);

        Assert.That(actual!.Id, Is.EqualTo(brandId));
        Assert.That(actual.LicenseeId, Is.EqualTo(licenseeId));
        Assert.That(actual.LicenseeName, Is.EqualTo("licensee"));
        Assert.That(actual.ResellerId, Is.EqualTo(resellerId));
        Assert.That(actual.ResellerName, Is.EqualTo("reseller"));
        Assert.That(actual.Code, Is.EqualTo("Test"));
        Assert.That(actual.Name, Is.EqualTo("Test"));
        Assert.That(actual.Description, Is.EqualTo(null));
        Assert.That(actual.OperationalEmail, Is.EqualTo(null));
        Assert.That(actual.Status, Is.EqualTo(BrandStatus.Active));
        Assert.That(actual.Currency, Is.EqualTo("CNY"));
        Assert.That(actual.ClientId, Is.EqualTo(string.Empty));
        Assert.That(actual.SecretKey, Is.EqualTo(string.Empty));
        Assert.That(actual.ApiKey, Is.EqualTo(string.Empty));
        Assert.That(actual.SmsEnabled, Is.EqualTo(false));
        Assert.That(actual.SmsRegisterFlowEnabled, Is.EqualTo(false));
        Assert.That(actual.PlayerIdentityEnabled, Is.EqualTo(false));
        Assert.That(actual.EmailRegisterFlowEnabled, Is.EqualTo(false));
        Assert.That(actual.BankInfoRegisterFlowEnabled, Is.EqualTo(false));
        Assert.That(actual.CheckDividendActiveCount, Is.EqualTo(false));
        Assert.That(actual.EnablePlayerIdentity, Is.EqualTo(false));
        Assert.That(actual.AutoClearWageringRequirement, Is.EqualTo(false));
        Assert.That(actual.PlatformCharge, Is.EqualTo(10));
        Assert.That(actual.AutoSendAgentBonus, Is.EqualTo(true));
        Assert.That(actual.EnablePlayerAgent, Is.EqualTo(true));
        Assert.That(actual.AutoClearWageringRequirementBalance, Is.EqualTo(100));
        Assert.That(actual.DepositHandlingFeeRateTenPercentile, Is.EqualTo(0));
        Assert.That(actual.WithdrawHandlingFeeRateTenPercentile, Is.EqualTo(0));
        Assert.That(actual.AgentDividendType, Is.EqualTo(AgentDividendType.MultipleLayer));
        Assert.That(actual.SettlementIntervalType, Is.EqualTo(SettlementIntervalType.DoubleWeekly));
        Assert.That(actual.SettlementDay, Is.EqualTo(null));
        Assert.That(actual.TimeSpanMinutes, Is.EqualTo(null));
    }

    [Test]
    public async Task Get_Brand_Domains()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brandId = Guid.NewGuid();

        await dbContext.BrandDomains.AddRangeAsync(
            new BrandDomain
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Url = "Test1"
            },
            new BrandDomain
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                Url = "Test2",
            });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandDomainsAsync(brandId).ToArrayAsync();

        Assert.That(actual.Length, Is.EqualTo(2));
    }

    [Test]
    public async Task Get_A_Brand_Product_Fee()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var brandId = Guid.NewGuid();

        await dbContext.BrandProductFees.AddRangeAsync(
            new BrandProductFee
            {
                BrandId = brandId,
                CardGamePercentage = 10,
                LiveDealerPercentage = 10,
                LotteryPercentage = 10,
                SportsPercentage = 10,
                SlotPercentage = 10,
                VirtualSportsPercentage = 10,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedByUsername = "test",
                UpdatedByUsername = "test"
            });

        _ = await dbContext.SaveChangesAsync();

        var sut = new BrandServiceClient(http);

        var actual = await sut.GetBrandProductFeeInfoAsync(brandId);

        Assert.That(actual!.CardGamePercentage, Is.EqualTo(10));
        Assert.That(actual.SlotPercentage, Is.EqualTo(10));
        Assert.That(actual.SportsPercentage, Is.EqualTo(10));
        Assert.That(actual.LiveDealerPercentage, Is.EqualTo(10));
        Assert.That(actual.LotteryPercentage, Is.EqualTo(10));
        Assert.That(actual.VirtualSportsPercentage, Is.EqualTo(10));
    }

    [Test]
    public async Task Create_Brand_Product_Fee()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        await sut.CreateUpdateBrandProductFeeAsync(new CreateUpdateBrandProductFeeRequest
        {
            BrandId = brandId,
            CardGamePercentage = 20,
            SlotPercentage = 20,
            SportsPercentage = 20,
            LiveDealerPercentage = 20,
            LotteryPercentage = 20,
            VirtualSportsPercentage = 20,
            EditorUsername = "test"
        });

        var actualBrand = dbContext.BrandProductFees.First(x => x.BrandId == brandId);

        Assert.That(actualBrand.CardGamePercentage, Is.EqualTo(20));
        Assert.That(actualBrand.SlotPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.SportsPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.LiveDealerPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.LotteryPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.VirtualSportsPercentage, Is.EqualTo(20));
    }

    [Test]
    public async Task Update_Brand_Product_Fee()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var brand = new BrandProductFee
        {
            BrandId = brandId,
            CardGamePercentage = 10,
            SlotPercentage = 10,
            SportsPercentage = 10,
            LiveDealerPercentage = 10,
            LotteryPercentage = 10,
            VirtualSportsPercentage = 10,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        };

        _ = await dbContext.BrandProductFees.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        await sut.CreateUpdateBrandProductFeeAsync(new CreateUpdateBrandProductFeeRequest
        {
            BrandId = brandId,
            CardGamePercentage = 20,
            SlotPercentage = 20,
            SportsPercentage = 20,
            LiveDealerPercentage = 20,
            LotteryPercentage = 20,
            VirtualSportsPercentage = 20,
            EditorUsername = "test"
        });

        var actualBrand = dbContext.BrandProductFees.First(x => x.BrandId == brandId);

        Assert.That(actualBrand.CardGamePercentage, Is.EqualTo(20));
        Assert.That(actualBrand.SlotPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.SportsPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.LiveDealerPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.LotteryPercentage, Is.EqualTo(20));
        Assert.That(actualBrand.VirtualSportsPercentage, Is.EqualTo(20));
    }

    [Test]
    public async Task 整合測試_開啟或關閉平台簡訊開關()
    {
        await using var web = new TestBrandCertificateApplication();

        var http = web.CreateClient();

        var dbContext = web.Services.GetRequiredService<TgsContext>();

        var sut = new BrandServiceClient(http);

        var brandId = Guid.NewGuid();

        var brand = new Brand
        {
            Id = brandId,
            EnableSms = false,
            UpdatedByUsername = "system",
            ApiKey = "apiKey",
            ClientId = "clientId",
            Code = "GPI",
            CreatedByUsername = "system",
            Name = "GPI",
            SecretKey = "secretKey",
        };

        _ = await dbContext.Brands.AddAsync(brand);

        _ = await dbContext.SaveChangesAsync();

        await sut.SetBrandEnableSmsAsync(brandId, true, "test");

        var actual = dbContext.Brands.First(x => x.Id == brandId);

        Assert.That(actual.EnableSms, Is.True);
        Assert.That(actual.UpdatedByUsername, Is.EqualTo("test"));
    }
}