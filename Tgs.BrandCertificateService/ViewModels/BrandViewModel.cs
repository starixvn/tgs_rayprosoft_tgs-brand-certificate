using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandViewModel
{
	public Guid Id { get; init; }

	public string Code { get; init; } = default!;

	public string Currency { get; init; } = default!;

	public BrandStatus Status { get; init; }

	public bool SmsEnabled { get; init; }

	public bool SmsRegisterFlowEnabled { get; init; }

	public bool PlayerIdentityEnabled { get; init; }

	public bool EmailRegisterFlowEnabled { get; init; }

	public bool BankInfoRegisterFlowEnabled { get; init; }

	public int? TimeSpanMinutes { get; init; }
}
