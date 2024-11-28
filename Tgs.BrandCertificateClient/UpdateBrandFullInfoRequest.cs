namespace Tgs.BrandCertificate.Clients;

public record UpdateBrandFullInfoRequest
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? Description { get; init; }
	public string? OperationalEmail { get; init; }
	public decimal PlatformCharge { get; init; }
	public bool EnablePlayerIdentity { get; init; }
	public SettlementIntervalType SettlementIntervalType { get; init; }
	public int? SettlementDay { get; init; }
	public bool CheckDividendActiveCount { get; init; }
	public bool AutoClearWageringRequirement { get; init; }
	public bool AutoSendAgentBonus { get; init; }
	public bool EnablePlayerAgent { get; init; }
	public decimal AutoClearWageringRequirementBalance { get; init; }
	public decimal DepositHandlingFeeRateTenPercentile { get; init; }
	public decimal WithdrawHandlingFeeRateTenPercentile { get; init; }
	public string EditorUsername { get; init; } = string.Empty;

	public IEnumerable<string> AreaCodes { get; init; } = [];
}