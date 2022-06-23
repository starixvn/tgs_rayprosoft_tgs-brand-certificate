namespace Tgs.BrandCertificate.Clients;
public record BrandInfo
{
	public Guid Id { get; init; }

	public string Code { get; init; } = default!;

	public string Currency { get; init; } = default!;

	public bool SmsEnabled { get; init; }

	public bool SmsRegisterFlowEnabled { get; init; }

	public bool PlayerIdentityEnabled { get; init; }

	public bool EmailRegisterFlowEnabled { get; init; }

	public bool BankInfoRegisterFlowEnabled { get; init; }
}
