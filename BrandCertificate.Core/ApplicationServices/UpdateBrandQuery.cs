namespace Tgs.BrandCertificate.ApplicationServices;

public record UpdateBrandQuery(
	Guid Id,
	string Name,
	string? Description,
	string? OperationalEmail,
	decimal PlatformCharge,
	bool EnablePlayerIdentity,
	SettlementIntervalType SettlementIntervalType,
	int? SettlementDay,
	bool CheckDividendActiveCount,
	bool AutoClearWageringRequirement,
	bool AutoSendAgentBonus,
	bool EnablePlayerAgent,
	decimal AutoClearWageringRequirementBalance,
	decimal DepositHandlingFeeRateTenPercentile,
	decimal WithdrawHandlingFeeRateTenPercentile,
	string EditorUsername);
