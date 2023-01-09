using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateAllBrandMaintenanceTimeCommandService : ICommandService<UpdateAllBrandMaintenanceTimeCommand>
{
	private readonly ITgsRepository _tgsRepository;

	public UpdateAllBrandMaintenanceTimeCommandService(ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask ExecuteAsync(UpdateAllBrandMaintenanceTimeCommand command, CancellationToken cancellationToken = default)
		=> _tgsRepository.UpdateAllBrandMaintenanceTimeAsync(command.BeginTime, command.EndTime, cancellationToken);
}
