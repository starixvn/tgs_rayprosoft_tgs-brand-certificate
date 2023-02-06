namespace Tgs.BrandCertificate.Clients;
public record BrandPaymentSettings(
	bool PaymentDepositLockEnabled,
	bool AutoClearWageringRequirement,
	decimal ClearWageringRequirementBalance);
