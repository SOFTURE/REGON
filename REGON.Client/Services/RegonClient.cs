using REGON.Client.Enums;
using REGON.Client.Models;
using REGON.Client.Responses;

namespace REGON.Client.Services
{
    public class RegonClient : IRegonClient
    {
        private readonly Client _clientRegon;

        public RegonClient(string apiKey)
        {
            _clientRegon = new Client(apiKey);
        }

        public async Task<Company> GetCompanyDataByNip(string nip)
        {
            try
            {
                var response = await _clientRegon.GetFullCompanyDataByNip(nip);

                return response != null 
                    ? CreateRegonResult(response) 
                    : new Company();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to the REGON API.", ex);
            }
        }
        
        public async Task<Company> GetCompanyDataByKrs(string krs)
        {
            try
            {
                var response = await _clientRegon.GetFullCompanyDataByKrs(krs);

                return response != null 
                    ? CreateRegonResult(response) 
                    : new Company();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to the REGON API.", ex);
            }
        }

        public async Task<Company> GetCompanyDataByRegon(string regon)
        {
            try
            {
                var response = await _clientRegon.GetFullCompanyDataByRegon(regon);

                return response != null
                    ? CreateRegonResult(response)
                    : new Company();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to the REGON API.", ex);
            }
        }

        private static Company CreateRegonResult(RegonResponse response)
        {
            var pkdsResult = new List<Pkd>();
            if (response.Pkds != null)
            {
                var pkds = response.Pkds.Select(pkd => new Pkd { Value = pkd.Value, Name = pkd.Name });
                pkdsResult.AddRange(pkds);
            }
            
            var legalForm = (LegalForm)Enum.Parse(typeof(LegalForm), response.LegalForm);

            return new Company
            {
                Nip = response.NIP,
                Krs = response.Krs,
                Name = response.Name,
                REGON = response.REGON,
                BuildingNumber = response.BuildingNumber,
                FlatNumber = response.FlatNumber,
                City = response.City,
                PostCode = response.PostCode,
                LegalForm = legalForm,
                Street = response.Street,
                MainPkd = response.MainPkd?.Value,
                Pkds = pkdsResult,
                StartDate = response.StartDate,
                EndDate = response.EndDate,
                IsSuspended = response.IsSuspended,
                IsActive = response.IsActive,
                Voivodeship = response.Voivodeship,
                District = response.District,
                Commune = response.Commune,
                WebsiteUrl = response.WebsiteUrl,
                Email = response.Email,
                PhoneNumber = response.PhoneNumber,
            };
        }
    }
}
