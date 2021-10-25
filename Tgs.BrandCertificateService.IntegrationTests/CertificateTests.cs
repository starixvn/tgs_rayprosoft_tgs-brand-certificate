using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using Tgs.BrandCertificate.Clients;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificateService.IntegrationTests
{
	[TestFixture]
	public class CertificateTests
	{
		[Test]
		public async Task Get_System_Validate_SecurityKey()
		{
			using var testHost = TestSiteHelper.GetBrandCertificateTestHost();
			_ = testHost.StartAsync();

			var testClient = new BrandCertificateClient(
				testHost.GetTestClient());

			var actual = await testClient.GetSystemValidateSecurityKeyAsync();

			Assert.IsNotNull(actual);
		}

		[Test]
		public async Task Get_Brand_JsonWebToken()
		{
			using var testHost = TestSiteHelper.GetBrandCertificateTestHost();
			_ = testHost.StartAsync();

			var dbContext = testHost.Services.GetRequiredService<TgsContext>();

			var brandId = Guid.NewGuid();

			await dbContext.Brands.AddAsync(new Brand
			{
				Id = brandId,
				Code = "Test"
			});

			await dbContext.SaveChangesAsync();

			var testClient = new BrandCertificateClient(
				testHost.GetTestClient());

			var actual = await testClient.MakeJsonWebTokenAsync(brandId);

			Assert.IsNotNull(actual);
		}

		[Test]
		public async Task Client_Validate_Token()
		{
			using var testHost = TestSiteHelper.GetBrandCertificateTestHost();
			_ = testHost.StartAsync();

			var dbContext = testHost.Services.GetRequiredService<TgsContext>();

			var brandId = Guid.NewGuid();

			await dbContext.Brands.AddAsync(new Brand
			{
				Id = brandId,
				Code = "Test"
			});

			await dbContext.SaveChangesAsync();

			var testClient = new BrandCertificateClient(
				testHost.GetTestClient());

			var handler = new JwtSecurityTokenHandler();

			var securityKey = await testClient.GetSystemValidateSecurityKeyAsync();

			var token = await testClient.MakeJsonWebTokenAsync(brandId);

			var actual = handler.ValidateToken(
				token,
				new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = false,
					ValidateLifetime = false,
					ValidIssuers = new[] { "tgs" },
					NameClaimType = ClaimTypes.NameIdentifier,
					IssuerSigningKey = securityKey
				},
				out _);

			Assert.AreEqual(brandId, Guid.Parse(actual.Identity.Name));
		}

		[Test]
		public async Task Client_Invalid_Token()
		{
			using var testHost = TestSiteHelper.GetBrandCertificateTestHost();
			_ = testHost.StartAsync();

			var dbContext = testHost.Services.GetRequiredService<TgsContext>();

			var brandId = Guid.NewGuid();

			await dbContext.Brands.AddAsync(new Brand
			{
				Id = brandId,
				Code = "Test"
			});

			await dbContext.SaveChangesAsync();

			var testClient = new BrandCertificateClient(
				testHost.GetTestClient());

			var handler = new JwtSecurityTokenHandler();

			var securityKey = await testClient.GetSystemValidateSecurityKeyAsync();

			var token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiVGVzdCIsInN1YiI6IjNiY2JkZmY4LTRlZTktNGYwOC1hNWE0LWJiZmEwMWFhN2I5MyIsIm5iZiI6MTYzNTE0MTI1MywiZXhwIjoxNjM1MTQ0ODUzLCJpYXQiOjE2MzUxNDEyNTMsImlzcyI6InRncyJ9.icMql0JpHvjomQSayBvsQ8TQAseGUimhHdjRaDqYZy9_OxTPGj9y_A0c5jdBW6FlgVBbrZZWeoHaiiZFAr7xzq";

			Assert.That(
				() => handler.ValidateToken(
					token,
					new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = false,
						ValidateLifetime = false,
						ValidIssuers = new[] { "tgs" },
						NameClaimType = ClaimTypes.NameIdentifier,
						IssuerSigningKey = securityKey
					},
					out _),
				Throws.Exception);
		}
	}
}
