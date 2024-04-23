namespace Tgs.BrandCertificate.ApplicationServices;

public record UpdateBrandProductFeeCommand(
	Guid BrandId,
	decimal CardGamePercentage,
	decimal LiveDealerPercentage,
	decimal LotteryPercentage,
	decimal SlotPercentage,
	decimal SportsPercentage,
	decimal VirtualSportsPercentage,
	string EditorUsername);
