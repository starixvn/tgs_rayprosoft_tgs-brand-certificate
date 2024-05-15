using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandProductFeeCommandService(
	ITgsRepository tgsRepository)
	: ICommandService<UpdateBrandProductFeeCommand>
{
	public async ValueTask ExecuteAsync(
		UpdateBrandProductFeeCommand command,
		CancellationToken cancellationToken = default)
	{
		var fee = await tgsRepository.GetBrandProductFeeAsync(
			command.BrandId,
			cancellationToken).ConfigureAwait(false);

		if (fee == null)
			await tgsRepository.CreateBrandProductFeeAsync(command.BrandId,
				command.CardGamePercentage,
				command.LiveDealerPercentage,
				command.LotteryPercentage,
				command.SlotPercentage,
				command.SportsPercentage,
				command.VirtualSportsPercentage,
				command.EditorUsername,
				cancellationToken).ConfigureAwait(false);
		else
			await tgsRepository.UpdateBrandProductFeeAsync(command.BrandId,
				command.CardGamePercentage,
				command.LiveDealerPercentage,
				command.LotteryPercentage,
				command.SlotPercentage,
				command.SportsPercentage,
				command.VirtualSportsPercentage,
				command.EditorUsername,
				cancellationToken).ConfigureAwait(false);
	}
}
