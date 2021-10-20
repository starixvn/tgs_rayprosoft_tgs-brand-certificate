using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificateService
{
	internal class BrandCertificateManager
	{
		private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
		private readonly SigningCredentials _signingCredentials;
		private readonly JsonWebKey _validateJsonWebKey;

		public BrandCertificateManager(IOptions<BrandCertificateOptions> optionsAccessor)
		{
			var options = optionsAccessor.Value;

			var rsaFileContent = File.ReadAllText(options.SecurityKeyPath);
			var rsa = RSA.Create();
			rsa.ImportFromPem(rsaFileContent);

			var securityKey = new RsaSecurityKey(rsa);
			_signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

			_validateJsonWebKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(
				new RsaSecurityKey(securityKey.Rsa.ExportParameters(false)));
		}

		public string MakeJwt(Guid brandId, string brandCode)
		{
			var id = new ClaimsIdentity(new[]{
				new Claim(JwtRegisteredClaimNames.Name, brandCode),
				new Claim(JwtRegisteredClaimNames.Sub, brandId.ToString())
			});

			var token = _jwtSecurityTokenHandler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = "tgs",
				Subject = id,
				SigningCredentials = _signingCredentials
			});

			return _jwtSecurityTokenHandler.WriteToken(token);
		}

		public string GenerateJwk()
			=> JsonExtensions.SerializeToJson(_validateJsonWebKey);
	}
}
