namespace Tgs.BrandCertificate.ApplicationServices;

public record UpdateBrandProductFeeCommand(
	Guid BrandId,
	decimal CardGameRebatePercentage,
	decimal LiveDealerRebatePercentage,
	decimal LotteryRebatePercentage,
	decimal SlotRebatePercentage,
	decimal SportsRebatePercentage,
	decimal VirtualSportsRebatePercentage,
	string EditorUsername);
