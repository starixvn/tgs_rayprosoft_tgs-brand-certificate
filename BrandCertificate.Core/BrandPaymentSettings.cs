namespace Tgs.BrandCertificate;
public record BrandPaymentSettings(
	bool PaymentDepositLockEnabled,
	bool AutoClearWageringRequirement,
	decimal ClearWageringRequirementBalance);
