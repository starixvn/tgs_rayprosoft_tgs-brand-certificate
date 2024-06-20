using Tgs.Core.Infrastructures.Utility;

namespace Tgs.BrandCertificate;

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

			case SettlementIntervalType.DoubleWeekly:
				if (startDate.Day > 15)
				{
					var nextMonth = startDate.AddMonths(1);
					var next15OfMonth = new DateTimeOffset(nextMonth.Year,
						nextMonth.Month,
						15,
						0,
						0,
						0,
						nextMonth.Offset);
					return (startDate,
						DateGenerator.GetLastDayOfMonth(startDate),
						DateGenerator.GetFirstDayOfMonth(nextMonth),
						next15OfMonth);
				}
				else
				{
					var the15OfMonth = new DateTimeOffset(startDate.Year,
						startDate.Month,
						15,
						0,
						0,
						0,
						startDate.Offset);
					return (startDate,
						the15OfMonth,
						the15OfMonth.AddDays(1),
						DateGenerator.GetLastDayOfMonth(startDate));
				}

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
						DateGenerator.GetSpecifyDayOfMonth(startDate, dayOfMonth.Value),
						DateGenerator.GetSpecifyDayOfMonth(startDate, dayOfMonth.Value).AddDays(1),
						DateGenerator.GetSpecifyDayOfMonth(startDate.AddMonths(1), dayOfMonth.Value));
				}

				return (currentFromDate,
					DateGenerator.GetSpecifyDayOfMonth(startDate.AddMonths(1), dayOfMonth.Value),
					DateGenerator.GetSpecifyDayOfMonth(startDate.AddMonths(1), dayOfMonth.Value).AddDays(1),
					DateGenerator.GetSpecifyDayOfMonth(startDate.AddMonths(2), dayOfMonth.Value));

			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
		}
	}
}
