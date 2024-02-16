namespace Tgs.BrandCertificate.Clients;

public record UpdateBrandAgentDividendTimeIntervalRequest(
	Guid BrandId,
	DateTime PreviousSettlementFrom,
	DateTime PreviousSettlementTo,
	DateTime CurrentSettlementFrom,
	DateTime CurrentSettlementTo,
	DateTime NextSettlementFrom,
	DateTime NextSettlementTo);