using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Tgs.BrandCertificate.ApplicationServices;
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

	public IAsyncEnumerable<BrandAgentDividendInfo> GetBrandAgentDividendInfosAsync(
		CancellationToken cancellationToken = default)
	{
		return _dbContext.Brands
			.Select(x => new BrandAgentDividendInfo(x.Id,
				x.Code,
				(BrandStatus)x.Status,
				(AgentDividendType)x.AgentDividendType,
				(SettlementIntervalType)x.SettlementIntervalType,
				x.AutoSentAgentBonus,
				x.CheckDividendActiveCount,
				x.PlatformCharge,
				x.PreviousSettlementFrom,
				x.PreviousSettlementTo,
				x.CurrentSettlementFrom,
				x.CurrentSettlementTo,
				x.NextSettlementFrom,
				x.NextSettlementTo,
				x.SettlementDay))
			.ToAsyncEnumerable();
	}

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
		var brand = await _dbContext.Brands.FindAsync(
			[brandId],
			cancellationToken).ConfigureAwait(false);

		if (brand is null)
			return;

		brand.Status = (short)status;
		brand.UpdatedOn = DateTime.UtcNow;
		brand.UpdatedByUsername = modifier;

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<bool?> GetBrandPlatformAccountFundCheckFlagAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands.FindAsync(
			[brandId],
			cancellationToken).ConfigureAwait(false);

		return brand?.EnablePlatformAccountFundCheck;
	}

	public async ValueTask SetBrandPlatformAccountFundCheckFlagAsync(
		Guid brandId,
		bool enabled,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands.FindAsync(
			[brandId],
			cancellationToken).ConfigureAwait(false);

		if (brand is null)
			return;

		brand.EnablePlatformAccountFundCheck = enabled;
		brand.UpdatedOn = DateTime.UtcNow;
		brand.UpdatedByUsername = modifier;

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask UpdateBrandAgentDividendTimeIntervalAsync(Guid brandId,
		DateTime previousSettlementFrom,
		DateTime previousSettlementTo,
		DateTime currentSettlementFrom,
		DateTime currentSettlementTo,
		DateTime nextSettlementFrom,
		DateTime nextSettlementTo,
		CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands.FirstAsync(
			x => x.Id == brandId,
			cancellationToken: cancellationToken).ConfigureAwait(false);

		brand.PreviousSettlementFrom = previousSettlementFrom;
		brand.PreviousSettlementTo = previousSettlementTo;
		brand.CurrentSettlementFrom = currentSettlementFrom;
		brand.CurrentSettlementTo = currentSettlementTo;
		brand.NextSettlementFrom = nextSettlementFrom;
		brand.NextSettlementTo = nextSettlementTo;

		_ = _dbContext.Brands.Update(brand);

		_ = _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask CreateBrandAsync(CreateBrandInfo createBrand, CancellationToken cancellationToken = default)
	{
		var brand = new Brand
		{
			Id = createBrand.BrandId,
			LicenseeId = createBrand.LicenseeId,
			Code = createBrand.Code,
			Name = createBrand.Name,
			PreviousSettlementFrom = createBrand.CurrentSettlementFrom.UtcDateTime,
			PreviousSettlementTo = createBrand.CurrentSettlementTo.UtcDateTime,
			CurrentSettlementFrom = createBrand.CurrentSettlementFrom.UtcDateTime,
			CurrentSettlementTo = createBrand.CurrentSettlementTo.UtcDateTime,
			NextSettlementFrom = createBrand.NextSettlementFrom.UtcDateTime,
			NextSettlementTo = createBrand.NextSettlementTo.UtcDateTime,
			AutoSentAgentBonus = createBrand.AutoSendAgentBonus,
			AutoSentPlayerBonus = true,
			Status = (short)BrandStatus.Active,
			AgentDividendType = (short)createBrand.AgentDividendType,
			Description = createBrand.Description,
			OperationalEmail = createBrand.OperationalEmail,
			SettlementDay = createBrand.SettlementDay,
			SettlementIntervalType = (short)createBrand.SettlementIntervalType,
			ApiKey = createBrand.ApiKey,
			ClientId = createBrand.Code,
			SecretKey = createBrand.SecretKey,
			CreatedByUsername = createBrand.EditorUsername,
			UpdatedByUsername = createBrand.EditorUsername,
			CreatedOn = DateTime.UtcNow,
			UpdatedOn = DateTime.UtcNow,
			PlatformCharge = createBrand.PlatformCharge,
			CheckDividendActiveCount = createBrand.CheckDividendActiveCount,
			EnablePlayerIdentity = createBrand.EnablePlayerIdentity,
			AutoClearWageringRequirement = createBrand.AutoClearWageringRequirement,
			AutoClearWageringRequirementBalance = createBrand.AutoClearWageringRequirementBalance,
			DepositHandlingFeeRateTenPercentile = createBrand.DepositHandlingFeeRateTenPercentile,
			WithdrawHandlingFeeRateTenPercentile = createBrand.WithdrawHandlingFeeRateTenPercentile,
			TimeSpanMinutes = createBrand.TimeSpanMinutes,
			EnablePlayerAgent = createBrand.EnablePlayerAgent,
			AreaCodes = createBrand.AreaCodes
		};

		_ = await _dbContext.Brands.AddAsync(brand, cancellationToken).ConfigureAwait(false);

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask UpdateBrandAsync(UpdateBrandInfo updateBrand, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands.FirstAsync(
			x => x.Id == updateBrand.BrandId,
			cancellationToken).ConfigureAwait(false);

		brand.Name = updateBrand.Name;
		brand.CurrentSettlementFrom = updateBrand.CurrentSettlementFrom.UtcDateTime;
		brand.CurrentSettlementTo = updateBrand.CurrentSettlementTo.UtcDateTime;
		brand.NextSettlementFrom = updateBrand.NextSettlementFrom.UtcDateTime;
		brand.NextSettlementTo = updateBrand.NextSettlementTo.UtcDateTime;
		brand.SettlementDay = updateBrand.SettlementDay;
		brand.SettlementIntervalType = (short)updateBrand.SettlementIntervalType;
		brand.Description = updateBrand.Description;
		brand.OperationalEmail = updateBrand.OperationalEmail;
		brand.UpdatedByUsername = updateBrand.EditorUsername;
		brand.UpdatedOn = DateTime.UtcNow;
		brand.PlatformCharge = updateBrand.PlatformCharge;
		brand.CheckDividendActiveCount = updateBrand.CheckDividendActiveCount;
		brand.EnablePlayerIdentity = updateBrand.EnablePlayerIdentity;
		brand.AutoClearWageringRequirement = updateBrand.AutoClearWageringRequirement;
		brand.AutoClearWageringRequirementBalance = updateBrand.AutoClearWageringRequirementBalance;
		brand.DepositHandlingFeeRateTenPercentile = updateBrand.DepositHandlingFeeRateTenPercentile;
		brand.WithdrawHandlingFeeRateTenPercentile = updateBrand.WithdrawHandlingFeeRateTenPercentile;
		brand.AutoSentAgentBonus = updateBrand.AutoSendAgentBonus;
		brand.EnablePlayerAgent = updateBrand.EnablePlayerAgent;

		_ = _dbContext.Brands.Update(brand);

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<BrandFullInfo?> GetBrandFullInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.Include(x => x.Licensee)
			.ThenInclude(x => x.Reseller)
			.FirstOrDefaultAsync(x => x.Id == brandId, cancellationToken)
			.ConfigureAwait(false);

		if (brand is null)
			return null;

		var brandCurrency = await _dbContext.BrandCurrencies
			.FirstAsync(bc => bc.BrandId == brandId, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new BrandFullInfo(
			brand.Id,
			brand.LicenseeId,
			brand.Licensee.ResellerId,
			brand.Licensee.Name,
			brand.Licensee.Reseller.Name,
			brand.Code,
			brand.Name,
			brand.Description,
			brand.OperationalEmail,
			(BrandStatus)brand.Status,
			(AgentDividendType)brand.AgentDividendType,
			(SettlementIntervalType)brand.SettlementIntervalType,
			brandCurrency.CurrencyCode,
			brand.ClientId,
			brand.SecretKey,
			brand.ApiKey,
			brand.EnableSms,
			brand.EnableSmsRegisterFlow,
			brand.EnablePlayerIdentity,
			brand.EnableEmailRegisterFlow,
			brand.EnableBankInfoRegisterFlow,
			brand.CheckDividendActiveCount,
			brand.EnablePlayerIdentity,
			brand.AutoClearWageringRequirement,
			brand.AutoSentAgentBonus,
			brand.EnablePlayerAgent,
			brand.AutoClearWageringRequirementBalance,
			brand.DepositHandlingFeeRateTenPercentile,
			brand.WithdrawHandlingFeeRateTenPercentile,
			brand.PlatformCharge,
			brand.CreatedOn,
			brand.UpdatedOn,
			brand.SettlementDay,
			brand.TimeSpanMinutes);
	}

	public IAsyncEnumerable<string> GetBrandDomainsAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		return _dbContext.BrandDomains.Where(x => x.BrandId == brandId).Select(x => x.Url).ToAsyncEnumerable();
	}

	public async ValueTask<BrandProductFeeInfo?> GetBrandProductFeeAsync(Guid brandId,
		CancellationToken cancellationToken = default)
	{
		var fee = await _dbContext.BrandProductFees.FirstOrDefaultAsync(
			x => x.BrandId == brandId,
			cancellationToken).ConfigureAwait(false);

		return fee == null
			? null
			: new BrandProductFeeInfo(brandId,
			fee.CardGamePercentage,
			fee.LiveDealerPercentage,
			fee.LotteryPercentage,
			fee.SlotPercentage,
			fee.SportsPercentage,
			fee.VirtualSportsPercentage);
	}

	public async ValueTask CreateBrandProductFeeAsync(Guid brandId,
		decimal cardGamePercentage,
		decimal liveDealerPercentage,
		decimal lotteryPercentage,
		decimal slotPercentage,
		decimal sportsPercentage,
		decimal virtualSportsPercentage,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var fee = new BrandProductFee
		{
			BrandId = brandId,
			CardGamePercentage = cardGamePercentage,
			LiveDealerPercentage = liveDealerPercentage,
			LotteryPercentage = lotteryPercentage,
			SlotPercentage = slotPercentage,
			SportsPercentage = sportsPercentage,
			VirtualSportsPercentage = virtualSportsPercentage,
			CreatedByUsername = modifier,
			UpdatedByUsername = modifier,
			CreatedOn = DateTime.UtcNow,
			UpdatedOn = DateTime.UtcNow
		};

		_ = await _dbContext.BrandProductFees.AddAsync(
			fee,
			cancellationToken).ConfigureAwait(false);

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask UpdateBrandProductFeeAsync(Guid brandId,
		decimal cardGamePercentage,
		decimal liveDealerPercentage,
		decimal lotteryPercentage,
		decimal slotPercentage,
		decimal sportsPercentage,
		decimal virtualSportsPercentage,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var fee = await _dbContext.BrandProductFees.FirstAsync(
			x => x.BrandId == brandId,
			cancellationToken).ConfigureAwait(false);

		fee.CardGamePercentage = cardGamePercentage;
		fee.LiveDealerPercentage = liveDealerPercentage;
		fee.LotteryPercentage = lotteryPercentage;
		fee.SlotPercentage = slotPercentage;
		fee.SportsPercentage = sportsPercentage;
		fee.VirtualSportsPercentage = virtualSportsPercentage;
		fee.UpdatedByUsername = modifier;
		fee.UpdatedOn = DateTime.UtcNow;

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask InsertBrandSocialMediaSettingAsync(
		SocialMediaSetting setting,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var currentTime = DateTime.UtcNow;

		_ = await dbContext.SocialMediaSettingRows.AddAsync(
			new SocialMediaSettingRow
			{
				Id = setting.Id,
				BrandId = setting.BrandId,
				Type = setting.Type.ToString(),
				Name = setting.Name,
				Order = setting.Order,
				Url = setting.Url.ToString(),
				CreatedBy = modifier,
				UpdatedBy = modifier,
				CreatedOn = currentTime,
				UpdatedOn = currentTime
			},
			cancellationToken).ConfigureAwait(false);

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask DeleteBrandSocialMediaSettingAsync(Guid settingId, CancellationToken cancellationToken = default)
	{
		var findRow = await dbContext.SocialMediaSettingRows.FindAsync(
			[settingId],
			cancellationToken).ConfigureAwait(false);

		if (findRow is null)
			return;

		_ = dbContext.SocialMediaSettingRows.Remove(findRow);

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<SocialMediaSetting?> GetSocialMediaSettingAsync(Guid settingId, CancellationToken cancellationToken = default)
	{
		var findRow = await dbContext.SocialMediaSettingRows.FindAsync(
			[settingId],
			cancellationToken).ConfigureAwait(false);

		return findRow is null
			? null
			: new SocialMediaSetting(
				findRow.Id,
				findRow.BrandId,
				findRow.Name,
				Enum.Parse<SocialType>(findRow.Type),
				new Uri(findRow.Url),
				findRow.Order);
	}

	public IAsyncEnumerable<SocialMediaSetting> QueryBrandSocialMediaSettingsAsync(Guid brandId, CancellationToken cancellationToken = default)
		=> dbContext.SocialMediaSettingRows
			.Where(row => row.BrandId == brandId)
			.AsNoTracking()
			.ToAsyncEnumerable()
			.Select(row => new SocialMediaSetting(
				row.Id,
				row.BrandId,
				row.Name,
				Enum.Parse<SocialType>(row.Type),
				new Uri(row.Url),
				row.Order));

	public async ValueTask UpdateSocialMediaSettingOrderAsync(Guid settingId, int newOrder, string modifier, CancellationToken cancellationToken = default)
	{
		var findRow = await dbContext.SocialMediaSettingRows.FindAsync(
			[settingId],
			cancellationToken).ConfigureAwait(false);

		if (findRow is null)
			return;

		findRow.Order = newOrder;
		findRow.UpdatedBy = modifier;
		findRow.UpdatedOn = DateTime.UtcNow;

		_ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask SetBrandEnableSmsAsync(
		Guid brandId,
		bool enableSms,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands.FindAsync(
			[brandId],
			cancellationToken).ConfigureAwait(false);

		if (brand is null)
			return;

		brand.EnableSms = enableSms;
		brand.UpdatedByUsername = modifier;
		brand.UpdatedOn = DateTime.UtcNow;

		_ = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
	}

	public IAsyncEnumerable<BrandPlayerContractDisplayInfo> QueryBrandPlayerContractDisplayAsync(
		Guid brandId,
		CancellationToken cancellationToken = default)
		=> dbContext.BrandPlayerContractDisplays
			.Where(row => row.BrandId == brandId)
			.AsNoTracking()
			.ToAsyncEnumerable()
			.Select(row => new BrandPlayerContractDisplayInfo(
				row.Name,
				row.IsVisualized));

	public async ValueTask UpsertBrandPlayerContractDisplayAsync(
		Guid brandId,
		string name,
		bool isVisualized,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var displayInfo = await _dbContext.BrandPlayerContractDisplays
			.FirstOrDefaultAsync(b => b.BrandId == brandId && b.Name == name, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (displayInfo != null)
		{
			displayInfo.IsVisualized = isVisualized;
			displayInfo.UpdatedBy = modifier;
			displayInfo.UpdatedOn = DateTime.UtcNow;
			_ = _dbContext.BrandPlayerContractDisplays.Update(displayInfo);
		}
		else
		{
			var addDisplayItem = new BrandPlayerContractDisplay
			{
				Id = Guid.NewGuid(),
				BrandId = brandId,
				Name = name,
				IsVisualized = isVisualized,
				CreatedBy = modifier,
				CreatedOn = DateTime.UtcNow,
				UpdatedBy = modifier,
				UpdatedOn = DateTime.UtcNow,
			};
			_ = _dbContext.BrandPlayerContractDisplays.Add(addDisplayItem);
		}

		_ = await _dbContext
			.SaveChangesAsync(cancellationToken)
			.ConfigureAwait(false);
	}
}