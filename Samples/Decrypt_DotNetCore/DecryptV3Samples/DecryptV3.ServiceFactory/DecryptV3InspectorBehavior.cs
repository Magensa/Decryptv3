using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SCRAv2.ServiceFactory
{
    /// <summary>
    /// defines soap inspector beharior
    /// </summary>
    public class DecryptV3InspectorBehavior : IEndpointBehavior
    {
        /// <summary>
        /// Last requested soap xml
        /// </summary>
        public string LastRequestXML
        {
            get
            {
                return myMessageInspector.LastRequestXML;
            }
        }
        /// <summary>
        /// Last received response soap xml
        /// </summary>
        public string LastResponseXML
        {
            get
            {
                return myMessageInspector.LastResponseXML;
            }
        }


        private readonly DecryptV3MessageInspector myMessageInspector = new DecryptV3MessageInspector();
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }


        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(myMessageInspector);
        }
    }
}
