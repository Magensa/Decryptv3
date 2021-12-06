using DecryptV3.Dtos;
using Magensa.DecryptV3.Services;
using Microsoft.Extensions.Configuration;
using SCRAv2.ServiceFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DecryptV3.ServiceFactory
{
    public class DecryptV3Client : IDecryptV3Client
    {
        private readonly IConfiguration _config;
        public Uri Host { get; private set; }
        public string CertificateFileName { get; private set; }
        public string CertificatePassword { get; private set; }
        public DecryptV3Client(IConfiguration config)
        {
            _config = config;
            Host = new Uri(_config.GetValue<string>(Constants.DECRYPTSERVICEURL));
            CertificateFileName = _config.GetValue<string>(Constants.CERTIFICATE_FILENAME);
            CertificatePassword = _config.GetValue<string>(Constants.CERTIFICATE_PASSWORD);
        }

        public async Task<(DecryptCardSwipeResponseDto Response, RawSoapDetails SoapDetails)> DecryptCardSwipe(DecryptCardSwipeRequestDto dto)
        {
            (DecryptCardSwipeResponseDto Response, RawSoapDetails SoapDetails) result = (default, default);
            try
            {
                var soapRequest = new DecryptCardSwipeRequestV2
                {
                    BillingLabel = dto.BillingLabel,
                    CustomerTransactionID = dto.CustomerTransactionID,
                    Authentication = new Authentication()
                    {
                        CustomerCode = dto.CustomerCode,
                        Username = dto.Username,
                        Password = dto.Password
                    },
                    EncryptedCardSwipe = new EncryptedCardSwipe()
                    {
                        DeviceSN = dto.DeviceSN,
                        KSN = dto.KSN,
                        KeyType = (KeyVariantEnum)Enum.Parse(typeof(KeyVariantEnum), dto.KeyType),
                        MagnePrint = dto.MagnePrint,
                        MagnePrintStatus = dto.MagnePrintStatus,
                        Track1 = dto.Track1,
                        Track2 = dto.Track2,
                        Track3 = dto.Track3
                    },
                    AdditionalRequestData = dto.AdditionalRequestData.ToArray()
                };

                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                EndpointAddress endpointAddress = new EndpointAddress(Host);
                ChannelFactory<IDecryptV2> factory = new ChannelFactory<IDecryptV2>(binding, endpointAddress);
                DecryptV3InspectorBehavior requestInterceptorBehavior = new DecryptV3InspectorBehavior();
                factory.Endpoint.EndpointBehaviors.Add(requestInterceptorBehavior);
                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(CertificateFileName), CertificatePassword, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                factory.Credentials.ClientCertificate.Certificate = certificate;
                IDecryptV2 channel = factory.CreateChannel();
                var svcResponse = await channel.DecryptCardSwipeAsync(soapRequest);
                result.SoapDetails = new RawSoapDetails
                {
                    RequestXml = requestInterceptorBehavior.LastRequestXML,
                    ResponseXml = requestInterceptorBehavior.LastResponseXML
                };
                if (svcResponse != null)
                {
                    result.Response = new DecryptCardSwipeResponseDto
                    {
                        IsReplay = svcResponse.IsReplay,
                        MagTranId = svcResponse.MagTranId,
                        CardID = svcResponse.CardID,
                        DecryptedCardSwipe = new Dtos.DecryptedCardSwipe
                        {
                            MagnePrint = svcResponse.DecryptedCardSwipe.MagnePrint,
                            Track1 = svcResponse.DecryptedCardSwipe.Track1,
                            Track2 = svcResponse.DecryptedCardSwipe.Track2,
                            Track3 = svcResponse.DecryptedCardSwipe.Track3
                        },
                        MagnePrintScore = svcResponse.MagnePrintScore,
                        AdditionalResponseData = svcResponse.AdditionalResponseData
                    };
                }
            }
            catch (Exception ex) when (ex is CommunicationException || ex is ProtocolException || ex is FaultException || ex is Exception)
            {
                throw ex;
            }
            return result;
        }

        public async Task<(DecryptDataResponseDto Response, RawSoapDetails SoapDetails)> DecryptData(DecryptDataRequestDto dto)
        {
            (DecryptDataResponseDto Response, RawSoapDetails SoapDetails) result = (default, default);
            try
            {
                var soapRequest = new DecryptRequestV2
                {
                    BillingLabel = dto.BillingLabel,
                    CustomerTransactionID = dto.CustomerTransactionID,
                    Authentication = new Authentication
                    {
                        CustomerCode = dto.CustomerCode,
                        Username = dto.Username,
                        Password = dto.Password
                    },
                    EncryptedData = dto.EncryptedData,
                    KSN = dto.KSN,
                    KeyType = (KeyVariantEnum)Enum.Parse(typeof(KeyVariantEnum), dto.KeyType),
                    AdditionalRequestData = dto.AdditionalRequestData.ToArray(),
                };

                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                EndpointAddress endpointAddress = new EndpointAddress(Host);
                ChannelFactory<IDecryptV2> factory = new ChannelFactory<IDecryptV2>(binding, endpointAddress);
                DecryptV3InspectorBehavior requestInterceptorBehavior = new DecryptV3InspectorBehavior();
                factory.Endpoint.EndpointBehaviors.Add(requestInterceptorBehavior);
                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(CertificateFileName), CertificatePassword, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                factory.Credentials.ClientCertificate.Certificate = certificate;
                IDecryptV2 channel = factory.CreateChannel();
                var svcResponse = await channel.DecryptDataAsync(soapRequest);
                result.SoapDetails = new RawSoapDetails();
                result.SoapDetails.RequestXml = requestInterceptorBehavior.LastRequestXML;
                result.SoapDetails.ResponseXml = requestInterceptorBehavior.LastResponseXML;
                if (svcResponse != null)
                {
                    result.Response = new DecryptDataResponseDto()
                    {
                        CustomerTransactionId = svcResponse.CustomerTransactionId,
                        IsReplay = svcResponse.IsReplay,
                        MagTranId = svcResponse.MagTranId,
                        DecryptedData = svcResponse.DecryptedData,
                        AdditionalResponseData = svcResponse.AdditionalResponseData
                    };
                }
            }
            catch (Exception ex) when (ex is CommunicationException || ex is ProtocolException || ex is FaultException || ex is Exception)
            {
                throw ex;
            }
            return result;
        }

        public async Task<(GenerateMacResponseDto Response, RawSoapDetails SoapDetails)> GenerateMac(GenerateMacRequestDto dto)
        {
            (GenerateMacResponseDto Response, RawSoapDetails SoapDetails) result = (default, default);
            try
            {
                var soapRequest = new GenerateMacRequestV2
                {
                    BillingLabel = dto.BillingLabel,
                    CustomerTransactionID = dto.CustomerTransactionID,
                    Authentication = new Authentication
                    {
                        CustomerCode = dto.CustomerCode,
                        Username = dto.Username,
                        Password = dto.Password
                    },
                    DataToMAC = dto.DataToMAC,
                    KSN = dto.KSN,
                    KeyDerivationType = (KeyDerivationEnum)Enum.Parse(typeof(KeyDerivationEnum), dto.KeyDerivationType),
                    AdditionalRequestData = dto.AdditionalRequestData.ToArray()
                };

                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                EndpointAddress endpointAddress = new EndpointAddress(Host);
                ChannelFactory<IDecryptV2> factory = new ChannelFactory<IDecryptV2>(binding, endpointAddress);
                DecryptV3InspectorBehavior requestInterceptorBehavior = new DecryptV3InspectorBehavior();
                factory.Endpoint.EndpointBehaviors.Add(requestInterceptorBehavior);
                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(CertificateFileName), CertificatePassword, X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                factory.Credentials.ClientCertificate.Certificate = certificate;
                IDecryptV2 channel = factory.CreateChannel();
                var svcResponse = await channel.GenerateMacAsync(soapRequest);
                result.SoapDetails = new RawSoapDetails();
                result.SoapDetails.RequestXml = requestInterceptorBehavior.LastRequestXML;
                result.SoapDetails.ResponseXml = requestInterceptorBehavior.LastResponseXML;
                if (svcResponse != null)
                {
                    result.Response = new GenerateMacResponseDto
                    {
                        CustomerTransactionId = svcResponse.CustomerTransactionId,
                        IsReplay = svcResponse.IsReplay,
                        MagTranId = svcResponse.MagTranId,
                        MACedString = svcResponse.MACedString,
                        AdditionalResponseData = svcResponse.AdditionalResponseData
                    };
                }
            }
            catch (Exception ex) when (ex is CommunicationException || ex is ProtocolException || ex is FaultException || ex is Exception)
            {
                throw ex;
            }
            return result;
        }
    }
}
