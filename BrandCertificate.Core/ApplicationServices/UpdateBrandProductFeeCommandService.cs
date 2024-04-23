using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandProductFeeCommandService(
	ITgsRepository tgsRepository) : ICommandService<UpdateBrandProductFeeCommand>
{
	private readonly ITgsRepository _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));

	public async ValueTask ExecuteAsync(
		UpdateBrandProductFeeCommand command,
		CancellationToken cancellationToken = default)
	{
		var fee = await _tgsRepository.GetBrandProductFeeAsync(command.BrandId, cancellationToken);

		if (fee == null)
		{
			await _tgsRepository.CreateBrandProductFeeAsync(command.BrandId,
				command.CardGamePercentage,
				command.LiveDealerPercentage,
				command.LotteryPercentage,
				command.SlotPercentage,
				command.SportsPercentage,
				command.VirtualSportsPercentage,
				command.EditorUsername,
				cancellationToken);
		}
		else
		{
			await _tgsRepository.UpdateBrandProductFeeAsync(command.BrandId,
				command.CardGamePercentage,
				command.LiveDealerPercentage,
				command.LotteryPercentage,
				command.SlotPercentage,
				command.SportsPercentage,
				command.VirtualSportsPercentage,
				command.EditorUsername,
				cancellationToken);
		}
	}
}