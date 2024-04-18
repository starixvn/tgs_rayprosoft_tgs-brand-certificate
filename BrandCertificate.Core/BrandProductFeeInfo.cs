namespace Tgs.BrandCertificate;

public record BrandProductFeeInfo(
	Guid BrandId,
	decimal CardGameRebatePercentage,
	decimal LiveDealerRebatePercentage,
	decimal LotteryRebatePercentage,
	decimal SlotRebatePercentage,
	decimal SportsRebatePercentage,
	decimal VirtualSportsRebatePercentage);
