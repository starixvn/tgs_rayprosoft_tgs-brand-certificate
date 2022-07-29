using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificate.Repositories;

internal class TgsRepository : ITgsRepository
{
	private readonly TgsContext _dbContext;

	public TgsRepository(TgsContext dbContext)
	{
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync(new object?[] { brandId }, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var brandCurrency = await _dbContext.BrandCurrencies
			.FirstAsync(bc => bc.BrandId == brandId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return brand == null
			? null
			: new BrandInfo(
				brand.Id,
				brand.Code,
				brandCurrency.CurrencyCode,
				brand.EnableSms,
				brand.EnableSmsRegisterFlow,
				brand.EnablePlayerIdentity,
				brand.EnableEmailRegisterFlow,
				brand.EnableBankInfoRegisterFlow);
	}

	public async ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync(new object?[] { brandId }, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (brand is null)
			return new BrandFrontendInfo(null, null, null, null);

		var logoSettings = string.IsNullOrWhiteSpace(brand.LogoInfo)
			? null
			: JsonSerializer.Deserialize<BrandLogoSettings>(brand.LogoInfo);
		var registerFlowSettings = string.IsNullOrWhiteSpace(brand.PlayerRegisterFlowSettings)
			? null
			: JsonDocument.Parse(brand.PlayerRegisterFlowSettings);

		return logoSettings is null
			? new BrandFrontendInfo(null, null, null, registerFlowSettings)
			: new BrandFrontendInfo(
				logoSettings.DesktopLogoUrl,
				logoSettings.MobileLogoUrl,
				logoSettings.LoadingUrl,
				registerFlowSettings);
	}

	public async ValueTask<BrandStatus?> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync(new object?[] { brandId }, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return brand is null
			? null
			: (BrandStatus)brand.Status;
	}

	public IAsyncEnumerable<VipLevel> QueryVipLevelsAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> _dbContext.VipLevelRows
			.Where(vip => vip.BrandId == brandId)
			.Select(vip => new VipLevel(vip.Id, vip.Level))
			.ToAsyncEnumerable();

	public IAsyncEnumerable<BrandMaintenanceInfo> QueryBrandMaintenanceInfosAsync(string? brandCode, CancellationToken cancellationToken = default)
	{
		var query = from b in _dbContext.Brands.AsQueryable()
					join bm in _dbContext.BrandMaintenances on b.Id equals bm.BrandId into temp
					from bm in temp.DefaultIfEmpty()
					select new
					{
						b.Name,
						b.Code,
						LicenseeName = b.Licensee.Name,
						BrandId = b.Id,
						BrandStatus = b.Status,
						bm.BeginTime,
						bm.EndTime,
						bm.Id
					};
		if (!string.IsNullOrWhiteSpace(brandCode))
			query = query.Where(bmj => bmj.Code == brandCode);

		return query.Select(bmj => new BrandMaintenanceInfo
		{
			BrandName = bmj.Name,
			BrandCode = bmj.Code,
			LicenseeName = bmj.LicenseeName,
			BrandId = bmj.BrandId,
			BrandStatus = (BrandStatus)bmj.BrandStatus,
			BeginTime = bmj.BeginTime,
			EndTime = bmj.EndTime,
			BrandMaintenanceId = bmj.Id
		}).ToAsyncEnumerable();
	}

	public async ValueTask BatchUpdateBrandMaintenanceTimeAsync(Guid[] brandIds, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default)
	{
		var brandMaintenances = (from brandId in brandIds
								 join bm in _dbContext.BrandMaintenances.AsQueryable() on brandId equals bm.BrandId into temp
								 from bm in temp.DefaultIfEmpty()
								 where brandIds.Contains(brandId)
								 select new
								 {
									 BrandMaintenance = bm,
									 BrandId = brandId
								 }).ToArray();

		var updates = new List<BrandMaintenance>();
		var adds = new List<BrandMaintenance>();
		foreach (var item in brandMaintenances)
		{
			if (item.BrandMaintenance != null)
			{
				item.BrandMaintenance.BeginTime = beginTime;
				item.BrandMaintenance.EndTime = endTime;
				updates.Add(item.BrandMaintenance);
			}
			else
			{
				adds.Add(new BrandMaintenance
				{
					BeginTime = beginTime,
					EndTime = endTime,
					BrandId = item.BrandId
				});
			}
		}

		_dbContext.BrandMaintenances.UpdateRange(updates);

		_dbContext.BrandMaintenances.AddRange(adds);

		_ = await _dbContext
		   .SaveChangesAsync(cancellationToken)
		   .ConfigureAwait(false);
	}

	public async ValueTask<BrandMaintenanceModel> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default)
	{
		var brandMaintenance = await _dbContext.BrandMaintenances
			.FirstAsync(bm => bm.Id == brandMaintenanceId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new BrandMaintenanceModel(brandMaintenance.Content);
	}

	public async ValueTask UpdateBrandMaintenanceInfoAsync(Guid brandMaintenanceId, string Content, CancellationToken cancellationToken = default)
	{
		var brandMaintenance = await _dbContext.BrandMaintenances
			.FirstAsync(bm => bm.Id == brandMaintenanceId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		brandMaintenance.Content = Content;

		_ = await _dbContext
			.SaveChangesAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> await _dbContext.Brands
			.Where(brand => brand.Id == brandId)
			.Select(brand => new BrandPaymentSettings(brand.DepositPaymentLockEnabled))
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);
}