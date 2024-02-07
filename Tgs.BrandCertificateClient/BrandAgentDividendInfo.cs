namespace Tgs.BrandCertificate.Clients;

public record BrandAgentDividendInfo(
	Guid Id,
	string Code,
	BrandStatus Status,
	AgentDividendType AgentDividendType,
	SettlementIntervalType SettlementIntervalType,
	bool AutoSentAgentBonus,
	bool CheckDividendActiveCount,
	decimal PlatformCharge,
	DateTime PreviousSettlementFrom,
	DateTime PreviousSettlementTo,
	DateTime CurrentSettlementFrom,
	DateTime CurrentSettlementTo,
	DateTime NextSettlementFrom,
	DateTime NextSettlementTo,
	int? SettlementDay);
