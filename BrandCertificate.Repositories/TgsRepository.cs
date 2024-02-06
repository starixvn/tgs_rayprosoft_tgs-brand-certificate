using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Tgs.BrandCertificate.Repositories.Entities;

namespace Tgs.BrandCertificate.Repositories;

internal class TgsRepository(TgsContext dbContext) : ITgsRepository
{

	private readonly TgsContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

	public IAsyncEnumerable<BrandCurrencyInfo> GetAllBrandCurrenciesAsync(CancellationToken cancellationToken = default)
		=> _dbContext.BrandCurrencyInfos
			.FromSqlRaw("""
				SELECT DISTINCT
					brand_id,
					first_value(currency_code) OVER w AS currency
				FROM
					dbo.brand_currencies
				WINDOW w AS (PARTITION BY brand_id ORDER BY currency_code);
				""")
			.AsNoTracking()
			.ToAsyncEnumerable();

	public async ValueTask<int?> GetBrandBetLimitIdAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> await _dbContext.BrandBetLimits.Where(row => row.BrandId == brandId && row.IsDefault)
			.Select(row => row.BetLimitId)
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);

	public async ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync([brandId], cancellationToken: cancellationToken)
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

	public IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(int timeSpanMinutes,
		CancellationToken cancellationToken = default)
		=> _dbContext.Brands
			.Where(x => x.TimeSpanMinutes == timeSpanMinutes)
			.Select(x => x.Id).ToAsyncEnumerable();

	public async ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync([brandId], cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (brand is null)
			return null;

		var brandCurrency = await _dbContext.BrandCurrencies
			.FirstAsync(bc => bc.BrandId == brandId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new BrandInfo(
			brand.Id,
			brand.Code,
			(BrandStatus)brand.Status,
			brandCurrency.CurrencyCode,
			brand.EnableSms,
			brand.EnableSmsRegisterFlow,
			brand.EnablePlayerIdentity,
			brand.EnableEmailRegisterFlow,
			brand.EnableBankInfoRegisterFlow,
			brand.TimeSpanMinutes);
	}

	public async ValueTask<BrandInfo?> GetBrandInfoByCodeAsync(string brandCode, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.Where(row => row.Code == brandCode)
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (brand is null)
			return null;

		var brandCurrency = await _dbContext.BrandCurrencies
			.FirstAsync(bc => bc.BrandId == brand.Id, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new BrandInfo(
			brand.Id,
			brand.Code,
			(BrandStatus)brand.Status,
			brandCurrency.CurrencyCode,
			brand.EnableSms,
			brand.EnableSmsRegisterFlow,
			brand.EnablePlayerIdentity,
			brand.EnableEmailRegisterFlow,
			brand.EnableBankInfoRegisterFlow,
			brand.TimeSpanMinutes);
	}

	public async ValueTask<BrandMaintenanceModel> GetBrandMaintenanceInfoByIdAsync(Guid brandMaintenanceId, CancellationToken cancellationToken = default)
	{
		var brandMaintenance = await _dbContext.BrandMaintenances
			.FirstAsync(bm => bm.Id == brandMaintenanceId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new BrandMaintenanceModel(brandMaintenance.Content ?? string.Empty);
	}

	public async ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> await _dbContext.Brands
			.Where(brand => brand.Id == brandId)
			.Select(brand => new BrandPaymentSettings(
				brand.DepositPaymentLockEnabled,
				brand.AutoClearWageringRequirement,
				brand.AutoClearWageringRequirementBalance))
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);

	public IAsyncEnumerable<BrandRelationInfo> GetBrandRelationsAsync(CancellationToken cancellationToken = default)
		=> _dbContext.Brands
			.Include(b => b.Licensee)
			.ThenInclude(l => l.Reseller)
			.Select(b => new
			{
				BrandId = b.Id,
				BrandName = b.Name,
				BrandCode = b.Code,
				LicenseeId = b.Licensee.Id,
				LicenseeName = b.Licensee.Name,
				LicenseeCode = b.Licensee.Code,
				ResellerId = b.Licensee.Reseller.Id,
				ResellerName = b.Licensee.Reseller.Name,
				ResellerCode = b.Licensee.Reseller.Code,
			})
			.ToArrayAsync(cancellationToken: cancellationToken)
			.ToAsyncEnumerable()
			.SelectMany(arr => arr.ToAsyncEnumerable())
			.Select(info => new BrandRelationInfo(
				info.BrandId,
				info.BrandCode,
				info.BrandName,
				info.LicenseeId,
				info.LicenseeCode,
				info.LicenseeName,
				info.ResellerId,
				info.ResellerCode,
				info.ResellerName));

	public async ValueTask<SmsSettingsInfo?> GetBrandSmsSettingsAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands.FindAsync(
			[brandId],
			cancellationToken: cancellationToken).ConfigureAwait(false);

		return brand?.SmsVendorSettings is null || string.IsNullOrEmpty(brand?.SmsVendorCode)
			? null
			: new SmsSettingsInfo(brand.SmsVendorCode, JsonDocument.Parse(brand?.SmsVendorSettings!));
	}

	public async ValueTask<BrandStatusInfo?> GetBrandStatusInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> await _dbContext.Brands
			.Where(b => b.Id == brandId)
			.Select(b => new BrandStatusInfo(
				(BrandStatus)b.Licensee.Reseller.Status,
				(BrandStatus)b.Licensee.Status,
				(BrandStatus)b.Status))
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);

	public IAsyncEnumerable<BrandListItem> QueryAllBrandsAsync(CancellationToken cancellationToken = default)
		=> _dbContext.Brands
			.Select(b => new
			{
				BrandId = b.Id,
				BrandCode = b.Code,
				BrandName = b.Name,
				b.Status,
				b.Description,
				b.OperationalEmail,
				b.CreatedOn,
				b.UpdatedOn
			})
			.ToArrayAsync(cancellationToken: cancellationToken)
			.ToAsyncEnumerable()
			.SelectMany(arr => arr.ToAsyncEnumerable())
			.Select(b => new BrandListItem(
				b.BrandId,
				b.BrandCode,
				b.BrandName,
				(BrandStatus)b.Status,
				b.Description,
				b.OperationalEmail,
				b.CreatedOn,
				b.UpdatedOn));

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

	public IAsyncEnumerable<VipLevel> QueryVipLevelsAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> _dbContext.VipLevelRows
			.Where(vip => vip.BrandId == brandId)
			.Select(vip => new VipLevel(vip.Id, vip.Level))
			.ToAsyncEnumerable();

	public async ValueTask UpdateAllBrandMaintenanceTimeAsync(TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default)
	{
		var brandMaintenances = (from brand in _dbContext.Brands.AsQueryable()
								 join bm in _dbContext.BrandMaintenances.AsQueryable() on brand.Id equals bm.BrandId into temp
								 from bm in temp.DefaultIfEmpty()
								 select new
								 {
									 BrandMaintenance = bm,
									 BrandId = brand.Id
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
					Id = Guid.NewGuid(),
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

	public async ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? content, CancellationToken cancellationToken = default)
	{
		var brandMaintenance = await _dbContext.BrandMaintenances
			.FirstAsync(bm => bm.Id == brandMaintenanceId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		brandMaintenance.Content = content;

		_ = _dbContext.BrandMaintenances.Update(brandMaintenance);

		_ = await _dbContext
			.SaveChangesAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default)
	{
		var brandMaintenance = await _dbContext.BrandMaintenances
			.FirstOrDefaultAsync(bm => bm.BrandId == brandId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (brandMaintenance != null)
		{
			brandMaintenance.BeginTime = beginTime;
			brandMaintenance.EndTime = endTime;
			_ = _dbContext.BrandMaintenances.Update(brandMaintenance);
		}
		else
		{
			var addBrandMaintenance = new BrandMaintenance
			{
				Id = Guid.NewGuid(),
				BrandId = brandId,
				BeginTime = beginTime,
				EndTime = endTime
			};
			_ = _dbContext.BrandMaintenances.Add(addBrandMaintenance);
		}

		_ = await _dbContext
			.SaveChangesAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<bool> UpdateBrandPlayerRegisterFlowSettingAsync(Guid brandId, JsonDocument playerRegisterFlowSettings, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync([brandId], cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (brand == null)
			return false;

		brand.PlayerRegisterFlowSettings = JsonSerializer.Serialize(playerRegisterFlowSettings);

		_ = _dbContext.Brands.Update(brand);

		_ = await _dbContext
			.SaveChangesAsync(cancellationToken)
			.ConfigureAwait(false);

		return true;
	}

	public async ValueTask UpdateBrandStatusAsync(
		Guid brandId,
		BrandStatus status,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync([brandId], cancellationToken)
			.ConfigureAwait(false);

		if (brand is null)
			return;

		brand.Status = (short)status;
		brand.UpdatedOn = DateTime.UtcNow;
		brand.UpdatedByUsername = modifier;

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

}
