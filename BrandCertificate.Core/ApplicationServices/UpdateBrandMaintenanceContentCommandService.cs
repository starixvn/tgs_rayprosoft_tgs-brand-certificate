using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandMaintenanceContentCommandService(ITgsRepository tgsRepository) : ICommandService<UpdateBrandMaintenanceContentCommand>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask ExecuteAsync(UpdateBrandMaintenanceContentCommand command, CancellationToken cancellationToken = default)
		=> _tgsRepository.UpdateBrandMaintenanceContentAsync(command.BrandMaintenanceId, command.Content, cancellationToken);
}
