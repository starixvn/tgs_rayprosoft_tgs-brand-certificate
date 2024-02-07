using Tgs.Core.Infrastructures.Utility;

namespace Tgs.BrandCertificate
{
	public static class AgentDividendSettlementIntervalGenerator
	{
		public static (DateTimeOffset, DateTimeOffset, DateTimeOffset, DateTimeOffset) GetInterval(
			SettlementIntervalType type,
			DateTimeOffset startDate,
			int? dayOfMonth,
			DateTimeOffset currentFromDate)
		{
			switch (type)
			{
				case SettlementIntervalType.Daily:
					return (startDate, startDate, startDate.AddDays(1), startDate.AddDays(1));

				case SettlementIntervalType.Weekly:
					return (startDate,
						DateGenerator.GetLastDayOfWeek(startDate),
						DateGenerator.GetFirstDayOfWeek(startDate.AddDays(7)),
						DateGenerator.GetLastDayOfWeek(startDate.AddDays(7)));

				case SettlementIntervalType.Monthly:
					if (dayOfMonth == null)
					{
						return (startDate,
							DateGenerator.GetLastDayOfMonth(startDate),
							DateGenerator.GetFirstDayOfMonth(startDate.AddMonths(1)),
							DateGenerator.GetLastDayOfMonth(startDate.AddMonths(1)));
					}

					if (startDate.Day <= dayOfMonth)
					{
						return (currentFromDate,
							startDate,
							startDate.AddDays(1),
							DateGenerator.GetSpecifyDayOfMonth(startDate, dayOfMonth.Value!));
					}

					return (currentFromDate,
						startDate,
						startDate.AddDays(1),
						DateGenerator.GetSpecifyDayOfMonth(startDate.AddMonths(1),
							dayOfMonth.Value!));

				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

	}
}
