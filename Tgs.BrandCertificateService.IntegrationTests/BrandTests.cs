using System.Text.Json;
using ExpectedObjects;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tgs.BrandCertificate.Clients;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests;

public class BrandTests
{
	[Test]
	public async Task Get_BrandInfo_By_Id()
	{
		var brandId = Guid.NewGuid();

		using var web = new TestBrandCertificateApplication();

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
	public async Task Get_BrandFrontendInfo_By_Id()
	{
		var brandId = Guid.NewGuid();

		var brandLogoSettings = new
		{
			DesktopLogoUrl = new Uri("https://localhost/1.png"),
			MobileLogoUrl = new Uri("https://localhost/2.png"),
			LoadingUrl = new Uri("https://localhost/3.png")
		};

		using var web = new TestBrandCertificateApplication();

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
			LogoInfo = JsonSerializer.Serialize(brandLogoSettings)
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

		brandLogoSettings.ToExpectedObject().ShouldMatch(actual);
	}
}
