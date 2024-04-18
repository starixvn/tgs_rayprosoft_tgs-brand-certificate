
namespace Tgs.BrandCertificateService.ViewModels;

public record CreateUpdateBrandProductFeeRequest
{
	public Guid BrandId { get; init; }
	public decimal CardGameRebatePercentage { get; init; }
	public decimal LiveDealerRebatePercentage { get; init; }
	public decimal LotteryRebatePercentage { get; init; }
	public decimal SlotRebatePercentage { get; init; }
	public decimal SportsRebatePercentage { get; init; }
	public decimal VirtualSportsRebatePercentage { get; init; }
	public string EditorUsername { get; init; } = string.Empty;
}
