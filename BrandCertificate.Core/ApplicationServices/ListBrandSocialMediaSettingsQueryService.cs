using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class ListBrandSocialMediaSettingsQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListBrandSocialMediaSettingsQuery, SocialMediaSetting>
{
	public IAsyncEnumerable<SocialMediaSetting> QueryAsync(ListBrandSocialMediaSettingsQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.QueryBrandSocialMediaSettingsAsync(query.BrandId, cancellationToken)
			.OrderBy(setting => setting.Order);
}
