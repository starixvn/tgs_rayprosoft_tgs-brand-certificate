using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class DeleteSocialMediaSettingCommandService(ITgsRepository tgsRepository) : ICommandService<DeleteSocialMediaSettingCommand>
{
	public async ValueTask ExecuteAsync(DeleteSocialMediaSettingCommand command, CancellationToken cancellationToken = default)
	{
		var setting = await tgsRepository.GetSocialMediaSettingAsync(
			command.SettingId,
			cancellationToken).ConfigureAwait(false);

		if (setting is null || setting.BrandId != command.BrandId)
			return;

		await tgsRepository.DeleteBrandSocialMediaSettingAsync(
			command.SettingId,
			cancellationToken).ConfigureAwait(false);
	}
}
