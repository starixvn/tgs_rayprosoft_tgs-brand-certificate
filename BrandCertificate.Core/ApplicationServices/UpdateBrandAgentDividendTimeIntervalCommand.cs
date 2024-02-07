namespace Tgs.BrandCertificate.ApplicationServices;

public record UpdateBrandAgentDividendTimeIntervalCommand(
	Guid BrandId,
	DateTime PreviousSettlementFrom,
	DateTime PreviousSettlementTo,
	DateTime CurrentSettlementFrom,
	DateTime CurrentSettlementTo,
	DateTime NextSettlementFrom,
	DateTime NextSettlementTo);