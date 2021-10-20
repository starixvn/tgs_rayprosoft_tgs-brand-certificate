using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients
{
	public interface IBrandCertificateClient
	{
		Task<SecurityKey> GetSystemValidateSecurityKeyAsync();

		Task<string?> MakeJsonWebTokenAsync(Guid brandId);
	}
}
