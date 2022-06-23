using Tgs.BrandCertificateService;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class MakeBrandTokenCommandService : IGetService<MakeBrandTokenCommand, string>
{
	private readonly BrandCertificateManager _certificateManager;

	public MakeBrandTokenCommandService(BrandCertificateManager certificateManager)
	{
		_certificateManager = certificateManager ?? throw new System.ArgumentNullException(nameof(certificateManager));
	}

	public ValueTask<string> GetAsync(MakeBrandTokenCommand query, CancellationToken cancellationToken = default)
		=> ValueTask.FromResult(_certificateManager.MakeJwt(query.BrandId, query.Code));
}
