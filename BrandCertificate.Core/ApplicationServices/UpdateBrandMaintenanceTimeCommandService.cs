using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandMaintenanceTimeCommandService(ITgsRepository tgsRepository) : ICommandService<UpdateBrandMaintenanceTimeCommand>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public ValueTask ExecuteAsync(UpdateBrandMaintenanceTimeCommand command, CancellationToken cancellationToken = default)
		=> _tgsRepository.UpdateBrandMaintenanceTimeAsync(command.BrandId, command.BeginTime, command.EndTime, cancellationToken);
}
