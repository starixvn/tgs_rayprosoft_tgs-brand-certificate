namespace Tgs.BrandCertificate.Clients;

public record BrandProductFeeInfo(
	Guid BrandId,
	decimal CardGamePercentage,
	decimal LiveDealerPercentage,
	decimal LotteryPercentage,
	decimal SlotPercentage,
	decimal SportsPercentage,
	decimal VirtualSportsPercentage);
