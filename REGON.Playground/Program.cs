using Microsoft.Extensions.DependencyInjection;
using REGON.Client;
using REGON.Client.Services;

namespace REGON.Playground
{
    public abstract class Program
    {
        private const string GusKey = "<GUS-API-KEY>";
        
        private static async Task Main(string[] args)
        {
            Console.WriteLine("REGON PLAYGROUND - START");
            
            var client = GetRegonClient();
            
            const string exampleKrs = "0000709622";
            
            var krsResponse = await client.GetCompanyDataByKrs(exampleKrs);

            Console.WriteLine(krsResponse.Name);

            const string exampleNip = "6392021335";
            
            var nipResponse = await client.GetCompanyDataByNip(exampleNip);

            Console.WriteLine(nipResponse.Name);

            const string exampleRegon = "387967759";
            
            var regonResponse = await client.GetCompanyDataByRegon(exampleRegon);
            
            Console.WriteLine(regonResponse.Name);
            
            Console.WriteLine("REGON PLAYGROUND - END");
        }

        private static IRegonClient GetRegonClient()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRegonClient(GusKey);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider.GetService<IRegonClient>()!;
        }
    }
}