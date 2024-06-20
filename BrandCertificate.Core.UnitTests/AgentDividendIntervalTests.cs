using Tgs.BrandCertificate;

namespace BrandCertificate.Core.UnitTests;

public class AgentDividendIntervalTests
{
    [Test]
    public void Get_Agent_Dividend_Interval_Daily()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.Daily,
            theDate,
            null,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 17, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 02, 17, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Weekly()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.Weekly,
            theDate,
            null,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 18, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 19, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 02, 25, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Monthly_Before_16()
    {
        var theDate = new DateTimeOffset(2024, 02, 10, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 1, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.DoubleWeekly,
            theDate,
            null,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 10, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 02, 29, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Double_Weekly_After_15()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.DoubleWeekly,
            theDate,
            null,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 29, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 03, 1, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 03, 15, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Monthly()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.Monthly,
            theDate,
            null,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 29, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 03, 1, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 03, 31, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Monthly_By_Settle_Day()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 12, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.Monthly,
            theDate,
            10,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 12, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 03, 10, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 03, 11, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 04, 10, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Monthly_By_Settle_Day2()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 12, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.Monthly,
            theDate,
            18,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 12, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 18, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 19, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 03, 18, 0, 0, 0, TimeSpan.FromHours(8))));
    }

    [Test]
    public void Get_Agent_Dividend_Interval_Monthly_By_Settle_Day3()
    {
        var theDate = new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8));

        var brandCurrentDate = new DateTimeOffset(2024, 02, 12, 0, 0, 0, TimeSpan.FromHours(8));

        var (cFrom, cTo, nFrom, nTo) = AgentDividendSettlementIntervalGenerator.GetInterval(
            SettlementIntervalType.Monthly,
            theDate,
            16,
            brandCurrentDate);

        Assert.That(cFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 12, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(cTo, Is.EqualTo(new DateTimeOffset(2024, 02, 16, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nFrom, Is.EqualTo(new DateTimeOffset(2024, 02, 17, 0, 0, 0, TimeSpan.FromHours(8))));
        Assert.That(nTo, Is.EqualTo(new DateTimeOffset(2024, 03, 16, 0, 0, 0, TimeSpan.FromHours(8))));
    }

}
