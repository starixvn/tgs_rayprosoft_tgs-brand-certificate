using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class SetSocialMediaSettingOrderCommandService(ITgsRepository tgsRepository)
	: ICommandService<SetSocialMediaSettingOrderCommand>
{
	public ValueTask ExecuteAsync(SetSocialMediaSettingOrderCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.UpdateSocialMediaSettingOrderAsync(
			command.SettingId,
			command.NewOrder,
			command.Modifier,
			cancellationToken);
}
