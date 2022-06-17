using Tgs.BrandCertificateService;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetJsonWebKeyQueryService : IGetService<GetJsonWebKeyQuery, string>
{
	private readonly BrandCertificateManager _certificateManager;

	public GetJsonWebKeyQueryService(BrandCertificateManager certificateManager)
	{
		_certificateManager = certificateManager ?? throw new ArgumentNullException(nameof(certificateManager));
	}

	public ValueTask<string> GetAsync(GetJsonWebKeyQuery query, CancellationToken cancellationToken = default)
		=> ValueTask.FromResult(_certificateManager.GenerateJwk());
}
