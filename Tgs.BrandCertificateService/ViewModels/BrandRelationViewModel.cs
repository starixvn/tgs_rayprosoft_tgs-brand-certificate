namespace Tgs.BrandCertificateService.ViewModels;

public class BrandRelationViewModel
{
	public Guid BrandId { get; init; }
	public string BrandCode { get; init; } = default!;
	public string BrandName { get; init; } = default!;
	public Guid LicenseeId { get; init; }
	public string LicenseeCode { get; init; } = default!;
	public string LicenseeName { get; init; } = default!;
	public Guid ResellerId { get; init; }
	public string ResellerCode { get; init; } = default!;
	public string ResellerName { get; init; } = default!;
}