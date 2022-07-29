using TGS.Paging;

namespace Tgs.BrandCertificate.ApplicationServices
{
	public record GetBrandMaintenanceInfosQuery(
		string? BrandCode,
		Paging<BrandMaintenanceInfo> Paging);
}