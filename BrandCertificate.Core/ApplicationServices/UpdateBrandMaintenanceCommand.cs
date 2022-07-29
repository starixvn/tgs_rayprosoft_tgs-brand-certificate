namespace Tgs.BrandCertificate.ApplicationServices
{
	public record UpdateBrandMaintenanceCommand(Guid BrandMaintenanceId, string Content);
}