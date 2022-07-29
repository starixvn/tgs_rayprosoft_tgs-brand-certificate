namespace Tgs.BrandCertificate.ApplicationServices
{
	public record UpdateAllBrandMaintenanceTimeCommand(TimeOnly BeginTime, TimeOnly EndTime);
}