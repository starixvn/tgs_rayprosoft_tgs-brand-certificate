namespace Tgs.BrandCertificate.ApplicationServices
{
	public record BatchUpdateBrandMaintenanceCommand(Guid[] BrandIds, TimeOnly BeginTime, TimeOnly EndTime);
}