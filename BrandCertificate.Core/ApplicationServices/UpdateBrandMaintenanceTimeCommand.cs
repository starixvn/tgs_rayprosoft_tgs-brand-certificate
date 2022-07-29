namespace Tgs.BrandCertificate.ApplicationServices
{
	public record UpdateBrandMaintenanceTimeCommand(Guid BrandId, TimeOnly BeginTime, TimeOnly EndTime);
}