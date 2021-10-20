using System;
using System.Threading.Tasks;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificate.Repositories
{
	internal class TgsRepository : ITgsRepository
	{
		private readonly TgsContext _dbContext;

		public TgsRepository(TgsContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId)
		{
			var brand = await _dbContext.Brands
				.FindAsync(brandId)
				.ConfigureAwait(false);

			return brand == null
				? null
				: new BrandInfo(brand.Id, brand.Code);
		}
	}
}
