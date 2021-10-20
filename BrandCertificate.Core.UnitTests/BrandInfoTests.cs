using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;

namespace BrandCertificate.Core.UnitTests
{
	[TestFixture]
	public class BrandInfoTests
	{
		[Test]
		public async Task Get_BrandInfo_From_Repository()
		{
			var fakeTgsRepository = Substitute.For<ITgsRepository>();

			var sut = new GetBrandInfoQueryService(fakeTgsRepository);

			var query = new GetBrandInfoQuery(Guid.NewGuid());

			await sut.GetAsync(query);

			_ = fakeTgsRepository.Received(1)
				.GetBrandInfoAsync(Arg.Is(query.BrandId));
		}
	}
}
