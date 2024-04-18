
namespace Tgs.BrandCertificateService.ViewModels;

public class BrandProductFeeInfoViewModel
{
	public Guid BrandId { get; init; }
	public decimal CardGameRebatePercentage { get; init; }
	public decimal LiveDealerRebatePercentage { get; init; }
	public decimal LotteryRebatePercentage { get; init; }
	public decimal SlotRebatePercentage { get; init; }
	public decimal SportsRebatePercentage { get; init; }
	public decimal VirtualSportsRebatePercentage { get; init; }
}
