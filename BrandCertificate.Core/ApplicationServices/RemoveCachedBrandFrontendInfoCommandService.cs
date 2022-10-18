using Google.Protobuf;
using Tgs.BrandCertificateService.Protos;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class RemoveCachedBrandFrontendInfoCommandService : ICommandService<RemoveCachedBrandFrontendInfoCommand>
	{
		private readonly IMessageSender _sender;

		public RemoveCachedBrandFrontendInfoCommandService(IMessageSender sender)
		{
			_sender = sender ?? throw new ArgumentNullException(nameof(sender));
		}

		public async ValueTask ExecuteAsync(RemoveCachedBrandFrontendInfoCommand command, CancellationToken cancellationToken = default)
		{
			var request = new RemoveBrandCachedInfoRequest
			{
				BrandId = command.BrandId,
			};

			await _sender
				.SendAsync("tgs.brand.info.cache.remove", request.ToByteArray(), cancellationToken)
				.ConfigureAwait(false);
		}
	}
}