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
				brand.EnableSmsRegisterFlow,
				brand.EnableEmailRegisterFlow,
				brand.EnableBankInfoRegisterFlow,
				brand.EnablePlayerIdentity);
	}

	public async ValueTask<BrandFrontendInfo?> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var brand = await _dbContext.Brands
			.FindAsync(new object?[] { brandId }, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (brand is null || string.IsNullOrWhiteSpace(brand.LogoInfo))
			return new BrandFrontendInfo(null, null, null, null);

		var logoSettings = JsonSerializer.Deserialize<BrandLogoSettings>(brand.LogoInfo);

		return logoSettings is null
			? null
			: new BrandFrontendInfo(
				logoSettings.DesktopLogoUrl,
				logoSettings.MobileLogoUrl,
				logoSettings.LoadingUrl,
				string.IsNullOrWhiteSpace(brand.PlayerRegisterFlowSettings)
					? null
					: JsonDocument.Parse(brand.PlayerRegisterFlowSettings));
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
}
