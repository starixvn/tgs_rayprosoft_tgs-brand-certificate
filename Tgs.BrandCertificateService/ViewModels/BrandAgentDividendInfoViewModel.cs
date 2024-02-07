using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandAgentDividendInfoViewModel
{
	public Guid Id { get; set; }
	public string Code { get; set; } = null!;
	public BrandStatus Status { get; set; }
	public AgentDividendType AgentDividendType { get; set; }
	public SettlementIntervalType SettlementIntervalType { get; set; }
	public bool AutoSentAgentBonus { get; set; }
	public bool CheckDividendActiveCount { get; set; }
	public decimal PlatformCharge { get; set; }
	public DateTime PreviousSettlementFrom { get; set; }
	public DateTime PreviousSettlementTo { get; set; }
	public DateTime CurrentSettlementFrom { get; set; }
	public DateTime CurrentSettlementTo { get; set; }
	public DateTime NextSettlementFrom { get; set; }
	public DateTime NextSettlementTo { get; set; }
	public int? SettlementDay { get; set; }
}
