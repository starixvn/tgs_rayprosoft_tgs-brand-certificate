namespace Tgs.BrandCertificate.Clients;

public record BrandProductFeeInfo(
	Guid BrandId,
	decimal CardGameRebatePercentage,
	decimal LiveDealerRebatePercentage,
	decimal LotteryRebatePercentage,
	decimal SlotRebatePercentage,
	decimal SportsRebatePercentage,
	decimal VirtualSportsRebatePercentage);
