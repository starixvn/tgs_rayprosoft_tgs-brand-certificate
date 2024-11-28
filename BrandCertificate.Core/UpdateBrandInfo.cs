namespace Tgs.BrandCertificate;

public record UpdateBrandInfo(
	Guid BrandId,
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
	DateTimeOffset CurrentSettlementFrom,
	DateTimeOffset CurrentSettlementTo,
	DateTimeOffset NextSettlementFrom,
	DateTimeOffset NextSettlementTo,
	string EditorUsername,
	IEnumerable<string> AreaCodes);