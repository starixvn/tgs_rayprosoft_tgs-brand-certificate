using Tgs.SerialNumberGenerator;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class CreateBrandQueryService : IGetService<CreateBrandQuery, Guid>
{
	private readonly ITgsRepository _tgsRepository;
	private readonly ISerialNumberGenerator _serialNumberGenerator;
	private readonly ICredentialGenerator _credentialGenerator;

	public CreateBrandQueryService(ITgsRepository tgsRepository, ISerialNumberGenerator serialNumberGenerator, ICredentialGenerator credentialGenerator)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		_serialNumberGenerator = serialNumberGenerator;
		_credentialGenerator = credentialGenerator;
	}

	public async ValueTask<Guid> GetAsync(CreateBrandQuery query,
		CancellationToken cancellationToken = default)
	{
		var timeSpan = new TimeSpan(8, 0, 0);
		var now = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month,
			DateTimeOffset.Now.Day, 0,
			0, 0, timeSpan);

		var (currentFromDate, currentToDate, nextFromDate, nextToDate) =
			AgentDividendSettlementIntervalGenerator.GetInterval(query.SettlementIntervalType,
				now,
				query.SettlementDay,
				now);

		var brandId = await _serialNumberGenerator
			.GenerateAsync(cancellationToken)
			.ConfigureAwait(false);

		var apiKey = _credentialGenerator.GetApiKey();

		var secretKey = _credentialGenerator.GetSecretKey();

		var brand = new CreateBrandInfo(
			brandId,
			query.Code,
			query.Name,
			apiKey,
			secretKey,
			query.Description,
			query.OperationalEmail,
			query.PlatformCharge,
			query.TimeSpanMinutes,
			query.EnablePlayerIdentity,
			query.LicenseeId,
			query.SettlementIntervalType,
			query.SettlementDay,
			query.AgentDividendType,
			query.CheckDividendActiveCount,
			query.AutoClearWageringRequirement,
			query.AutoSendAgentBonus,
			query.EnablePlayerAgent,
			query.AutoClearWageringRequirementBalance,
			query.DepositHandlingFeeRateTenPercentile,
			query.WithdrawHandlingFeeRateTenPercentile,
			currentFromDate,
			currentToDate,
			nextFromDate,
			nextToDate,
			query.EditorUsername);

		await _tgsRepository.CreateBrandAsync(brand, cancellationToken);

		return brandId;
	}
}
