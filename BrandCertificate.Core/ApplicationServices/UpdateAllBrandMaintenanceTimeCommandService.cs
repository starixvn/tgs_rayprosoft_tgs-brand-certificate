using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateAllBrandMaintenanceTimeCommandService(ITgsRepository tgsRepository) : ICommandService<UpdateAllBrandMaintenanceTimeCommand>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask ExecuteAsync(UpdateAllBrandMaintenanceTimeCommand command, CancellationToken cancellationToken = default)
		=> _tgsRepository.UpdateAllBrandMaintenanceTimeAsync(command.BeginTime, command.EndTime, cancellationToken);
}
