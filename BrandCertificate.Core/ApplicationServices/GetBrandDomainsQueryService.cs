using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class GetBrandDomainsQueryService(ITgsRepository tgsRepository) : IQueryService<GetBrandDomainsQuery, string>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public IAsyncEnumerable<string> QueryAsync(GetBrandDomainsQuery query, CancellationToken cancellationToken = default)
		=> _tgsRepository.GetBrandDomainsAsync(query.BrandId, cancellationToken);
}
