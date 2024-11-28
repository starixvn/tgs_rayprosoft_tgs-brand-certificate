namespace Tgs.BrandCertificate.Repositories.Entities;

public partial class Brand
{
	public List<string> GetAreaCodesList()
	{
		return this.AreaCodes?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
	}
}