using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class SetBrandEnableSmsCommandService(ITgsRepository tgsRepository)
	: ICommandService<SetBrandEnableSmsCommand>
{
	public ValueTask ExecuteAsync(SetBrandEnableSmsCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.SetBrandEnableSmsAsync(command.BrandId, command.EnableSms, command.Modifier, cancellationToken);
}