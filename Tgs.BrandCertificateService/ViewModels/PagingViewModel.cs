using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;

namespace Tgs.BrandCertificateService.ViewModels
{
	public class PagingViewModel
	{
		[FromQuery(Name = "page")]
		public int Page { get; set; }

		[FromQuery(Name = "rows")]
		public int Rows { get; set; }

		[FromQuery(Name = "orderby")]
		public short OrderBy { get; set; }

		[FromQuery(Name = "desc")]
		public bool IsDesc => OrderBy == (short)SortOrder.Descending;

		[FromQuery(Name = "all")]
		public bool ShowAll { get; set; } = false;

		[FromQuery(Name = "sort")]
		public string? SortBy { get; set; }
	}
}