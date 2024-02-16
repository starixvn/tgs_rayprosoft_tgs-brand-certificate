using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandFullInfoViewModel
{
	public Guid Id { get; init; }
	public Guid LicenseeId { get; init; }
	public string LicenseeName { get; init; } = default!;
	public string Code { get; init; } = default!;
	public string Name { get; init; } = default!;
	public string? Description { get; init; }
	public string? OperationalEmail { get; init; }
	public string Currency { get; init; } = default!;
	public string ClientId { get; init; } = default!;
	public string SecretKey { get; init; } = default!;
	public string ApiKey { get; init; } = default!;
	public BrandStatus Status { get; init; }
	public AgentDividendType AgentDividendType { get; init; }
	public SettlementIntervalType SettlementIntervalType { get; init; }
	public bool SmsEnabled { get; init; }
	public bool SmsRegisterFlowEnabled { get; init; }
	public bool PlayerIdentityEnabled { get; init; }
	public bool EmailRegisterFlowEnabled { get; init; }
	public bool BankInfoRegisterFlowEnabled { get; init; }
	public bool CheckDividendActiveCount { get; init; }
	public bool EnablePlayerIdentity { get; init; }
	public bool AutoClearWageringRequirement { get; init; }
	public bool AutoSendAgentBonus { get; init; }
	public bool EnablePlayerAgent { get; init; }
	public decimal AutoClearWageringRequirementBalance { get; init; }
	public decimal DepositHandlingFeeRateTenPercentile { get; init; }
	public decimal WithdrawHandlingFeeRateTenPercentile { get; init; }
	public decimal PlatformCharge { get; init; }
	public DateTime CreatedOn { get; init; }
	public DateTime UpdatedOn { get; init; }
	public int? SettlementDay { get; init; }
	public int? TimeSpanMinutes { get; init; }
}
