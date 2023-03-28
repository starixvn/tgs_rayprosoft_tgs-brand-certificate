using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels;

public class BrandListItemViewModel
{
	public Guid BrandId { get; init; }

	public BrandStatus Status { get; init; }

	public string? Description { get; init; }

	public string? OperationalEmail { get; init; }

	public DateTime CreatedOn { get; init; }

	public DateTime UpdatedOn { get; init; }
}
