namespace Tgs.BrandCertificate.ApplicationServices;

public record CreateBrandQuery(
	string Code,
	string Name,
	string? Description,
	string? OperationalEmail,
	decimal PlatformCharge,
	int TimeSpanMinutes,
	bool EnablePlayerIdentity,
	Guid LicenseeId,
	SettlementIntervalType SettlementIntervalType,
	int? SettlementDay,
	AgentDividendType AgentDividendType,
	bool CheckDividendActiveCount,
	bool AutoClearWageringRequirement,
	bool AutoSendAgentBonus,
	bool EnablePlayerAgent,
	decimal AutoClearWageringRequirementBalance,
	decimal DepositHandlingFeeRateTenPercentile,
	decimal WithdrawHandlingFeeRateTenPercentile,
	string EditorUsername,
	List<string> AreaCodes);
