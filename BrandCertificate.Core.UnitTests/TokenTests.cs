using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificateService;

namespace BrandCertificate.Core.UnitTests;

[TestFixture]
public class TokenTests
{
    [Test]
    public async Task Token_Validate_Test()
    {
        var options = new BrandCertificateOptions
        {
            SecurityKeyPath = "Assets/testkey.pem"
        };

        var certMgr = new BrandCertificateManager(
            new OptionsWrapper<BrandCertificateOptions>(options));

        var sut = new GetJsonWebKeyQueryService(certMgr);

        var handler = new JwtSecurityTokenHandler();

        var brandId = Guid.NewGuid();
        var token = certMgr.MakeJwt(brandId, "Test");

        var jwkJson = await sut.GetAsync(new GetJsonWebKeyQuery());

        var jwk = new JsonWebKey(jwkJson);

        var jwks = new JsonWebKeySet();
        jwks.Keys.Add(jwk);

        var securityKey = jwks.GetSigningKeys().First();

        var actual = await handler.ValidateTokenAsync(
            token,
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidIssuers = ["tgs"],
                NameClaimType = ClaimTypes.NameIdentifier,
                IssuerSigningKey = securityKey
            });

        ClassicAssert.AreEqual(
            brandId,
            Guid.Parse(actual.ClaimsIdentity.Name!));
    }
}
