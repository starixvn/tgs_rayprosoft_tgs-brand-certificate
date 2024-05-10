using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class AddSocialMediaSettingCommandService(ITgsRepository tgsRepository)
	: IGetService<AddSocialMediaSettingCommand, Guid>
{
	public async ValueTask<Guid> GetAsync(AddSocialMediaSettingCommand command, CancellationToken cancellationToken = default)
	{
		var newSocialMediaSetting = new SocialMediaSetting(
			Guid.NewGuid(),
			command.BrandId,
			command.Name,
			command.Type,
			command.Url);

		await tgsRepository.InsertBrandSocialMediaSettingAsync(
			newSocialMediaSetting,
			command.Modifier,
			cancellationToken).ConfigureAwait(false);

		return newSocialMediaSetting.Id;
	}
}
