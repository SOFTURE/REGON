using REGON.Client.Enums;
using REGON.Client.Models;

namespace REGON.Client;

public partial class Client
{
    private async Task<AdvanceReportData> GetAdvanceReportData(LegalForm legalForm, string regon)
    {
        switch (legalForm)
        {
            case LegalForm.JDG:
            case LegalForm.SC:
                return await GetReportForPhyisicalPerson(regon);
            case LegalForm.SPZOO:
            case LegalForm.SA:
            case LegalForm.SPZOOSK:
            case LegalForm.SJ:
            case LegalForm.SKA:
            case LegalForm.SK:
                return await GetReportForLegalPerson(regon);
            case LegalForm.ERROR:
            default:
                return null;
        }
    }

    private async Task<AdvanceReportData> GetReportForPhyisicalPerson(string regon)
    {
        var fullReport = await _httpClient.PobierzPelnyRaportCeidg(regon);

        return new AdvanceReportData
        {
            Krs = "",
            StartDate = ClientHelpers.GetDate(fullReport.Dane.FizDataPowstania),
            EndDate = ClientHelpers.GetDate(fullReport.Dane.FizDataZakonczeniaDzialalnosci),
            IsSuspended = ClientHelpers.IsHappened(fullReport.Dane.FizDataZawieszeniaDzialalnosci),
            PhoneNumber = ClientHelpers.ValueOrNull(fullReport.Dane.FizNumerTelefonu),
            WebsiteUrl = ClientHelpers.ValueOrNull(fullReport.Dane.FizAdresStronyinternetowej),
            Email = ClientHelpers.ValueOrNull(fullReport.Dane.FizAdresEmail)
        };
    }

    private async Task<AdvanceReportData> GetReportForLegalPerson(string regon)
    {
        var fullReport = await _httpClient.PobierzPelnyRaportPrawna(regon);

        return new AdvanceReportData
        {
            Krs = ClientHelpers.ValueOrNull(fullReport.Dane.PrawNumerWRejestrzeEwidencji),
            StartDate = ClientHelpers.GetDate(fullReport.Dane.PrawDataPowstania),
            EndDate = ClientHelpers.GetDate(fullReport.Dane.PrawDataZakonczeniaDzialalnosci),
            IsSuspended = ClientHelpers.IsHappened(fullReport.Dane.PrawDataZawieszeniaDzialalnosci),
            PhoneNumber = ClientHelpers.ValueOrNull(fullReport.Dane.PrawNumerTelefonu),
            WebsiteUrl = ClientHelpers.ValueOrNull(fullReport.Dane.PrawAdresStronyinternetowej),
            Email = ClientHelpers.ValueOrNull(fullReport.Dane.PrawAdresEmail)
        };
    }
}