using System.Text.Json;
using ExpectedObjects;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
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

		Assert.IsNotNull(actual);
		Assert.AreEqual(brandId, actual!.Id);
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

		Assert.IsNotNull(actual);
		Assert.AreEqual(brandId, actual!.Id);
	}

	[Test]
	public async Task 找不到BrandInfo會拿到Null()
	{
		var brandId = Guid.NewGuid();

		await using var web = new TestBrandCertificateApplication();

		var http = web.CreateClient();

		var sut = new BrandServiceClient(http);

		var actual = await sut.GetBrandInfoAsync(brandId);

		Assert.IsNull(actual);
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

		Assert.AreEqual(BrandStatus.Archived, actual);
	}

	[Test]
	public async Task Get_VipLevels_By_Brand()
	{
		var brandId = Guid.NewGuid();

		await using var web = new TestBrandCertificateApplication();

		var http = web.CreateClient();

		var dbContext = web.Services.GetRequiredService<TgsContext>();

		_ = await dbContext.VipLevelRows.AddAsync(new VipLevelRow
		{
			Id = Guid.NewGuid(),
			Level = 1,
			BrandId = brandId,
			CreatedByUsername = string.Empty,
			UpdatedByUsername = string.Empty
		});

		_ = await dbContext.SaveChangesAsync();

		var sut = new BrandServiceClient(http);

		var actual = await sut.GetVipLevelInfosAsync(brandId).ToArrayAsync();

		Assert.AreEqual(1, actual.Length);
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

		Assert.AreEqual(expected, actual);
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

		Assert.AreEqual(1, actual.Length);
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

		Assert.AreEqual(TimeOnly.Parse("09:30:00"), bm.BeginTime);
		Assert.AreEqual(TimeOnly.Parse("10:30:00"), bm.EndTime);
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

		Assert.AreEqual("Content", actual.Content);
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

		Assert.AreEqual("This is new content", bm.Content);
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

		Assert.AreEqual(TimeOnly.Parse("09:30:00"), bm.BeginTime);
		Assert.AreEqual(TimeOnly.Parse("10:30:00"), bm.EndTime);
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

		Assert.AreEqual(1, actual.Length);
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

		Assert.AreEqual(timeSpanMinutes, brandInfo?.TimeSpanMinutes);
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

		Assert.AreEqual(2, actual.Length);
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

		Assert.AreEqual(1, actual.Length);
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
			BrandIds = new[] { brandId }
		};

		var actual = await sut.QueryBrandListAsync(query).ToArrayAsync();

		Assert.AreEqual(1, actual.Length);
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

		Assert.AreEqual(expected, actual);
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

		Assert.AreEqual(expected, actual);
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

		Assert.AreEqual((short)BrandStatus.Inactive, brand.Status);
	}
}
