namespace Tgs.BrandCertificate.ApplicationServices
{
	public record UpdateBrandMaintenanceContentCommand(Guid BrandMaintenanceId, string? Content);
}