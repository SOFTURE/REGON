using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;
using REGON.Client.Models;
using RegonApiService;
using WcfCoreMtomEncoder;

namespace REGON.Client
{
    public class HttpClient
    {
        private readonly string _apiKey;
        
        public HttpClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        private async Task<ClientModel> PrepareClient()
        {
            var encodingBinding = new TextMessageEncodingBindingElement
            {
                ReaderQuotas =
                {
                    MaxStringContentLength = int.MaxValue
                }
            };

            var encoding = new MtomMessageEncoderBindingElement(encodingBinding);
            var transport = new HttpsTransportBindingElement
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue
            };

            var customBinding = new CustomBinding(encoding, transport);

            var regonApiClient = new UslugaBIRzewnPublClient();

            regonApiClient.Endpoint.Binding = customBinding;

            var sid = await regonApiClient.ZalogujAsync(_apiKey);

            new OperationContextScope(regonApiClient.InnerChannel);

            var requestProperties = new HttpRequestMessageProperty();

            requestProperties.Headers.Add("sid", sid.ZalogujResult);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperties;

            return new ClientModel
            {
                Client = regonApiClient,
                Sid = sid.ZalogujResult
            };
        }

        private static T ConvertXml<T>(string textXml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textXml);

            var xml = xmlDoc.InnerXml;

            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("root"));

            using var reader = new StringReader(xml);
            
            var resultObject = (T)serializer.Deserialize(reader);

            return resultObject;
        }

        // TYPE: F - sole proprietorship, P - company, LF - ?, LP - ?
        public async Task<SzukajPodmioty> SzukajPodmiotuByNip(string nip)
        {
            var regon = await PrepareClient();

            var parametry = new ParametryWyszukiwania
            {
                Nip = nip,
            };

            var response = await regon.Client.DaneSzukajPodmiotyAsync(parametry);

            await regon.Client.WylogujAsync(regon.Sid);
            regon.Client.Close();

            var result = ConvertXml<SzukajPodmioty>(response.DaneSzukajPodmiotyResult.ToString());

            return result;
        }

        // TYPE: F - sole proprietorship, P - company, LF - ?, LP - ?
        public async Task<SzukajPodmioty> SzukajPodmiotuByKrs(string krs)
        {
            var regon = await PrepareClient();

            var parametry = new ParametryWyszukiwania
            {
                Krs = krs,
            };

            var response = await regon.Client.DaneSzukajPodmiotyAsync(parametry);

            await regon.Client.WylogujAsync(regon.Sid);
            regon.Client.Close();

            var result = ConvertXml<SzukajPodmioty>(response.DaneSzukajPodmiotyResult.ToString());

            return result;
        }

        // TYPE: F - sole proprietorship, P - company, LF - ?, LP - ?
        public async Task<SzukajPodmioty> SzukajPodmiotuByRegon(string regon)
        {
            var regonClient = await PrepareClient();

            var parametry = new ParametryWyszukiwania
            {
                Regon = regon,
            };

            var response = await regonClient.Client.DaneSzukajPodmiotyAsync(parametry);

            await regonClient.Client.WylogujAsync(regonClient.Sid);
            regonClient.Client.Close();

            var result = ConvertXml<SzukajPodmioty>(response.DaneSzukajPodmiotyResult.ToString());

            return result;
        }

        // Companies (legal entities)
        public async Task<PobierzPelnyRaport> PobierzPelnyRaportPrawna(string regonNumber)
        {
            var regon = await PrepareClient();

            var response = await regon.Client.DanePobierzPelnyRaportAsync(regonNumber, "BIR11OsPrawna");

            await regon.Client.WylogujAsync(regon.Sid);
            regon.Client.Close();

            var result = ConvertXml<PobierzPelnyRaport>(response.DanePobierzPelnyRaportResult.ToString());

            return result;
        }

        // Companies (legal entities) - PKD codes
        public async Task<PobierzPKDPrawna> PobierzPKDPrawna(string regonNumber)
        {
            var regon = await PrepareClient();

            var response = await regon.Client.DanePobierzPelnyRaportAsync(regonNumber, "BIR11OsPrawnaPkd");

            await regon.Client.WylogujAsync(regon.Sid);
            regon.Client.Close();

            var result = ConvertXml<PobierzPKDPrawna>(response.DanePobierzPelnyRaportResult.ToString());

            return result;
        }

        // Sole proprietorships (CEIDG)
        public async Task<PobierzPelnyRaportCeidg> PobierzPelnyRaportCeidg(string regonNumber)
        {
            var regon = await PrepareClient();

            var response = await regon.Client.DanePobierzPelnyRaportAsync(regonNumber, "BIR11OsFizycznaDzialalnoscCeidg");

            await regon.Client.WylogujAsync(regon.Sid);
            regon.Client.Close();

            var result = ConvertXml<PobierzPelnyRaportCeidg>(response.DanePobierzPelnyRaportResult.ToString());

            return result;
        }

        // Sole proprietorships - PKD codes
        public async Task<PobierzPKDFizyczna> PobierzPkdFizyczna(string regonNumber)
        {
            var regon = await PrepareClient();

            var response = await regon.Client.DanePobierzPelnyRaportAsync(regonNumber, "BIR11OsFizycznaPkd");
            
            await regon.Client.WylogujAsync(regon.Sid);
            regon.Client.Close();

            var result = ConvertXml<PobierzPKDFizyczna>(response.DanePobierzPelnyRaportResult.ToString());

            return result;
        }
    }
}