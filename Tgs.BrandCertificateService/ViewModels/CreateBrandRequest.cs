using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public record CreateBrandRequest
{
	public string Code { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? OperationalEmail { get; set; }
	public decimal PlatformCharge { get; set; }
	public int TimeSpanMinutes { get; set; }
	public bool EnablePlayerIdentity { get; set; }
	public Guid LicenseeId { get; set; }
	public SettlementIntervalType SettlementIntervalType { get; set; }
	public int? SettlementDay { get; set; }
	public AgentDividendType AgentDividendType { get; set; }
	public bool CheckDividendActiveCount { get; set; }
	public bool AutoClearWageringRequirement { get; set; }
	public bool AutoSendAgentBonus { get; set; }
	public bool EnablePlayerAgent { get; set; }
	public decimal AutoClearWageringRequirementBalance { get; set; }
	public decimal DepositHandlingFeeRateTenPercentile { get; set; }
	public decimal WithdrawHandlingFeeRateTenPercentile { get; set; }
	public string EditorUsername { get; set; } = string.Empty;
	public List<string> AreaCodes { get; set; } = default!;
}
