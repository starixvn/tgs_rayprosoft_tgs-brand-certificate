using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandListItemViewModel
{
	public Guid BrandId { get; init; }

	public string BrandCode { get; init; } = default!;

	public string BrandName { get; init; } = default!;

	public Guid LicenseeId { get; init; }

	public string LicenseeName { get; init; } = default!;

	public Guid ResellerId { get; init; }

	public string ResellerName { get; init; } = default!;

	public BrandStatus Status { get; init; }

	public string? Description { get; init; }

	public string? OperationalEmail { get; init; }

	public DateTime CreatedOn { get; init; }

	public DateTime UpdatedOn { get; init; }
}
