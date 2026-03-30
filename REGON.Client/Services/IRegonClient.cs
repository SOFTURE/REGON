using REGON.Client.Responses;

namespace REGON.Client.Services
{
    public interface IRegonClient
    {
        Task<Company> GetCompanyDataByNip(string nip);
        Task<Company> GetCompanyDataByKrs(string krs);
        Task<Company> GetCompanyDataByRegon(string regon);
    }
}
