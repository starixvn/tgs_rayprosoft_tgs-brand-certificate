using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class GetBrandInfoQueryService : IGetService<GetBrandInfoQuery, BrandInfo?>
	{
		private readonly ITgsRepository _tgsRepository;

		public GetBrandInfoQueryService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask<BrandInfo?> GetAsync(GetBrandInfoQuery query, CancellationToken cancellationToken = default)
			=> _tgsRepository.GetBrandInfoAsync(query.BrandId);
	}
}
