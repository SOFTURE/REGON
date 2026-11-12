using REGON.Client.Enums;
using REGON.Client.Services;
using Shouldly;

namespace REGON.Tests;

[Category("Integration")]
public class GusTestEnvironmentTests
{
    private const string TestEnvironmentAddress = "https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc";
    private const string TestEnvironmentKey = "abcde12345abcde12345";
    private const string Variable = "REGON_INTEGRATION";

    private RegonClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        if (Environment.GetEnvironmentVariable(Variable) != "1")
        {
            Assert.Ignore($"Set {Variable}=1 to call the public GUS BIR test environment.");
        }

        _client = new RegonClient(TestEnvironmentKey, TestEnvironmentAddress);
    }

    [Test]
    public async Task GetCompanyDataByNip_ForJointStockCompany_ReturnsFullReport()
    {
        var company = await _client.GetCompanyDataByNip("7740001454");

        company.Nip.ShouldBe("7740001454");
        company.REGON.ShouldBe("610188201");
        company.Krs.ShouldBe("0000028860");
        company.Name.ShouldContain("ORLEN");
        company.LegalForm.ShouldBe(LegalForm.SA);
        company.City.ShouldNotBeNullOrWhiteSpace();
        company.PostCode.ShouldNotBeNullOrWhiteSpace();
        company.Voivodeship.ShouldNotBeNullOrWhiteSpace();
        company.MainPkd.ShouldNotBeNullOrWhiteSpace();
        company.Pkds.ShouldNotBeEmpty();
        company.StartDate.Year.ShouldBeLessThan(2000);
        company.IsActive.ShouldBeTrue();
    }

    [Test]
    public async Task GetCompanyDataByKrs_ReturnsTheSameCompanyAsNip()
    {
        var byKrs = await _client.GetCompanyDataByKrs("0000028860");

        byKrs.Nip.ShouldBe("7740001454");
        byKrs.REGON.ShouldBe("610188201");
        byKrs.LegalForm.ShouldBe(LegalForm.SA);
    }

    [Test]
    public async Task GetCompanyDataByRegon_ReturnsTheSameCompanyAsNip()
    {
        var byRegon = await _client.GetCompanyDataByRegon("610188201");

        byRegon.Nip.ShouldBe("7740001454");
        byRegon.Krs.ShouldBe("0000028860");
    }

    [Test]
    public async Task GetCompanyDataByNip_WhenNameHasQuotes_ReplacesThemWithApostrophes()
    {
        var company = await _client.GetCompanyDataByNip("5250007313");

        company.REGON.ShouldBe("010684960");
        company.Name.ShouldStartWith("'POCZTA POLSKA");
        company.Name.ShouldNotContain("\"");
    }

    [Test]
    public async Task GetCompanyDataByNip_WhenCompanyDoesNotExist_ReturnsEmptyCompany()
    {
        var company = await _client.GetCompanyDataByNip("1111111111");

        company.Nip.ShouldBeNull();
        company.Name.ShouldBeNull();
    }
}
