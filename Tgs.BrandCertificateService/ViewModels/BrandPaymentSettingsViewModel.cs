namespace Tgs.BrandCertificateService.ViewModels;

public class BrandPaymentSettingsViewModel
{
	public bool PaymentDepositLockEnabled { get; set; }

	public bool AutoClearWageringRequirement { get; set; }

	public decimal ClearWageringRequirementBalance { get; set; }
}
