using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace REGON.Client
{
    internal sealed class SidHeaderBehavior : IEndpointBehavior, IClientMessageInspector
    {
        public string Sid { get; set; }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (string.IsNullOrEmpty(Sid))
            {
                return null;
            }

            if (!request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out var property) || property is not HttpRequestMessageProperty httpRequest)
            {
                httpRequest = new HttpRequestMessageProperty();
                request.Properties[HttpRequestMessageProperty.Name] = httpRequest;
            }

            httpRequest.Headers["sid"] = Sid;

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
