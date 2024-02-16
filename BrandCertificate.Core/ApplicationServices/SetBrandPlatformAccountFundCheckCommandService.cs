using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class SetBrandPlatformAccountFundCheckCommandService(ITgsRepository tgsRepository)
	: ICommandService<SetBrandPlatformAccountFundCheckCommand>
{
	public ValueTask ExecuteAsync(SetBrandPlatformAccountFundCheckCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.SetBrandPlatformAccountFundCheckFlagAsync(
			command.BrandId,
			command.Enableld,
			command.Modifier,
			cancellationToken);
}
