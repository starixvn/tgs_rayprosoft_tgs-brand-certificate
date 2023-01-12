namespace Tgs.BrandCertificateService.ViewModels;

public class BrandViewModel
{
	public Guid Id { get; set; }

	public string Code { get; set; } = default!;

	public string Currency { get; set; } = default!;

	public bool SmsEnabled { get; set; }

	public bool SmsRegisterFlowEnabled { get; set; }

	public bool PlayerIdentityEnabled { get; set; }

	public bool EmailRegisterFlowEnabled { get; set; }

	public bool BankInfoRegisterFlowEnabled { get; set; }

	public int? TimeSpanMinutes { get; set; }
}
