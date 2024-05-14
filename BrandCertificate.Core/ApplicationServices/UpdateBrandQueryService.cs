using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandQueryService(ITgsRepository tgsRepository) : IGetService<UpdateBrandQuery, Guid>
{
	public async ValueTask<Guid> GetAsync(UpdateBrandQuery query,
		CancellationToken cancellationToken = default)
	{
		var brandAgentDividendInterval = await tgsRepository.GetBrandAgentDividendInfosAsync(cancellationToken)
			.FirstAsync(x => x.Id == query.Id, cancellationToken).ConfigureAwait(false);

		var currentSettlementFrom = brandAgentDividendInterval.CurrentSettlementFrom;
		var currentSettlementTo = brandAgentDividendInterval.CurrentSettlementTo;
		var nextSettlementFrom = brandAgentDividendInterval.NextSettlementFrom;
		var nextSettlementTo = brandAgentDividendInterval.NextSettlementTo;

		if (query.SettlementIntervalType != brandAgentDividendInterval.SettlementIntervalType)
		{
			var timeSpan = new TimeSpan(8, 0, 0);
			var now = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month,
				DateTimeOffset.Now.Day, 0,
				0, 0, timeSpan);

			var (currentFromDate, currentToDate, nextFromDate, nextToDate) =
				AgentDividendSettlementIntervalGenerator.GetInterval(query.SettlementIntervalType, now, query.SettlementDay,
					brandAgentDividendInterval.CurrentSettlementFrom);

			currentSettlementFrom = currentFromDate.UtcDateTime;
			currentSettlementTo = currentToDate.UtcDateTime;
			nextSettlementFrom = nextFromDate.UtcDateTime;
			nextSettlementTo = nextToDate.UtcDateTime;
		}

		var updateBrand = new UpdateBrandInfo(
			query.Id,
			query.Name,
			query.Description,
			query.OperationalEmail,
			query.PlatformCharge,
			query.EnablePlayerIdentity,
			query.SettlementIntervalType,
			query.SettlementDay,
			query.CheckDividendActiveCount,
			query.AutoClearWageringRequirement,
			query.AutoSendAgentBonus,
			query.EnablePlayerAgent,
			query.AutoClearWageringRequirementBalance,
			query.DepositHandlingFeeRateTenPercentile,
			query.WithdrawHandlingFeeRateTenPercentile,
			currentSettlementFrom,
			currentSettlementTo,
			nextSettlementFrom,
			nextSettlementTo,
			query.EditorUsername);

		await tgsRepository.UpdateBrandAsync(
			updateBrand,
			cancellationToken).ConfigureAwait(false);

		return query.Id;
	}
}
