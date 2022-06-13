using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients;

public interface IBrandCertificateClient
{
	Task<SecurityKey> GetSystemValidateSecurityKeyAsync(CancellationToken cancellationToken = default);

	Task<string?> MakeJsonWebTokenAsync(Guid brandId, CancellationToken cancellationToken = default);
}
