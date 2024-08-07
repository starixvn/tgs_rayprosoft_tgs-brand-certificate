using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class SetBrandPlayerContractDisplayCommandService(ITgsRepository repository)
	: ICommandService<SetBrandPlayerContractDisplayCommand>
{
	public ValueTask ExecuteAsync(SetBrandPlayerContractDisplayCommand command, CancellationToken cancellationToken = default)
		=> repository.UpsertBrandPlayerContractDisplayAsync(
			command.BrandId,
			command.Name,
			command.IsVisualized,
			command.Modifier,
			cancellationToken);
}