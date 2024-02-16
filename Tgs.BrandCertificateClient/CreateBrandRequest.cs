namespace Tgs.BrandCertificate.Clients;

public record CreateBrandRequest
{
	public string Code { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public string? Description { get; init; }
	public string? OperationalEmail { get; init; }
	public decimal PlatformCharge { get; init; }
	public int TimeSpanMinutes { get; init; }
	public bool EnablePlayerIdentity { get; init; }
	public Guid LicenseeId { get; init; }
	public SettlementIntervalType SettlementIntervalType { get; init; }
	public int? SettlementDay { get; init; }
	public AgentDividendType AgentDividendType { get; init; }
	public bool CheckDividendActiveCount { get; init; }
	public bool AutoClearWageringRequirement { get; init; }
	public bool AutoSendAgentBonus { get; init; }
	public bool EnablePlayerAgent { get; init; }
	public decimal AutoClearWageringRequirementBalance { get; init; }
	public decimal DepositHandlingFeeRateTenPercentile { get; init; }
	public decimal WithdrawHandlingFeeRateTenPercentile { get; init; }
	public string EditorUsername { get; init; } = string.Empty;
}
