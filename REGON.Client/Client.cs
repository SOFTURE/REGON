using REGON.Client.Models;

namespace REGON.Client
{
    public partial class Client
    {
        private readonly HttpClient _httpClient;

        public Client(string apiKey)
        {
            _httpClient = new HttpClient(apiKey);
        }

        public async Task<RegonResponse> GetFullCompanyDataByNip(string nip)
        {
            ClientHelpers.ValidateInput(nip);

            var basicData = await _httpClient.SzukajPodmiotuByNip(nip);

            return await CreateRegonResponse(basicData.Dane);
        }

        public async Task<RegonResponse> GetFullCompanyDataByKrs(string krs)
        {
            ClientHelpers.ValidateInput(krs);

            var basicData = await _httpClient.SzukajPodmiotuByKrs(krs);

            return await CreateRegonResponse(basicData.Dane);
        }

        public async Task<RegonResponse> GetFullCompanyDataByRegon(string regon)
        {
            ClientHelpers.ValidateInput(regon);

            var basicData = await _httpClient.SzukajPodmiotuByRegon(regon);

            return await CreateRegonResponse(basicData.Dane);
        }

        private async Task<RegonResponse> CreateRegonResponse(DaneSzukajPodmioty basicData)
        {
            if (basicData.Nazwa == null)
            {
                return null;
            }

            var legalForm = ClientHelpers.CheckLegalForm(basicData.Nazwa, basicData.Typ);
            
            var pkds = await GetPkds(legalForm, basicData.Regon);
            
            var advancedReportData = await GetAdvanceReportData(legalForm, basicData.Regon);

            return new RegonResponse
            {
                NIP = basicData.Nip,
                Krs = advancedReportData?.Krs ?? "",
                REGON = basicData.Regon,
                LegalForm = legalForm.ToString(),
                Name = basicData.Nazwa.Replace("\"", "'"),
                City = basicData.Miejscowosc,
                PostCode = basicData.KodPocztowy,
                Street = basicData.Ulica,
                FlatNumber = basicData.NrLokalu,
                BuildingNumber = basicData.NrNieruchomosci,
                MainPkd = pkds?.MainPkd,
                Pkds = pkds?.RestPkds,
                Commune = basicData.Gmina,
                District = basicData.Powiat,
                Voivodeship = basicData.Wojewodztwo,
                IsSuspended = advancedReportData == null || advancedReportData.IsSuspended,
                IsActive = ClientHelpers.IsActive(basicData.Nazwa),
                StartDate = advancedReportData?.StartDate ?? DateTime.Now,
                EndDate = advancedReportData?.EndDate,
                WebsiteUrl = advancedReportData?.WebsiteUrl,
                Email = advancedReportData?.Email,
                PhoneNumber = advancedReportData?.PhoneNumber
            };
        }
    }
}