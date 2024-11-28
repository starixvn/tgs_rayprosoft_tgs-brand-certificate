namespace Tgs.BrandCertificate;

public record BrandInfo(
	Guid Id,
	string Code,
	BrandStatus Status,
	string Currency,
	List<string> AreaCodes,
	bool SmsEnabled,
	bool SmsRegisterFlowEnabled,
	bool PlayerIdentityEnabled,
	bool EmailRegisterFlowEnabled,
	bool BankInfoRegisterFlowEnabled,
	int? TimeSpanMinutes);
