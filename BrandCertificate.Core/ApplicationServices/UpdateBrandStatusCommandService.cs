using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandStatusCommandService : ICommandService<UpdateBrandStatusCommand>
{
	private readonly ITgsRepository _tgsRepository;

	public UpdateBrandStatusCommandService(
		ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask ExecuteAsync(
		UpdateBrandStatusCommand command, 
		CancellationToken cancellationToken = default) 
		=> _tgsRepository.UpdateBrandStatusAsync(
			command.BrandId, 
			command.Status,
			command.Modifier, 
			cancellationToken);
}