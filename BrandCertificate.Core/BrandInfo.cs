namespace Tgs.BrandCertificate;

public record BrandInfo(
	Guid Id,
	string Code,
	string Currency,
	bool SmsEnabled,
	bool SmsRegisterFlowEnabled,
	bool PlayerIdentityEnabled,
	bool EmailRegisterFlowEnabled,
	bool BankInfoRegisterFlowEnabled,
	int? TimeSpanMinutes);
